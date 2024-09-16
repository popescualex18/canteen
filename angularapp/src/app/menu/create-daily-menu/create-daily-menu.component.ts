import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MenuType, MenuTypeDescriptions } from 'src/app/common/category-enum';
import { MenuDto } from '../models/menu-item.model';
import { MenuService } from '../services/menu.service';
import { DailyMenuDefinitionDto } from '../models/daily-menu-definition';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-daily-menu',
  templateUrl: './create-daily-menu.component.html',
  styleUrls: ['./create-daily-menu.component.css'],
})
export class CreateDailyMenuComponent implements OnInit {
  menuCtrl = new FormControl();
  filteredMenues: MenuDto[] = [];
  selectedMenues: MenuDto[] = [];
  selectedDate: Date | null = new Date(2024, 2, 1);
  today: Date = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() - 1);
  
  @ViewChild('fruitInput', { static: false }) fruitInput!: ElementRef<HTMLInputElement>;
  @ViewChild('datepicker', { static: false }) datePicker!: ElementRef<HTMLInputElement>;

  previousStep: number = -1;
  currentStep: number = -1;
  menuTypes: string[] = Object.keys(MenuType).filter(key => isNaN(Number(key)));
  menuData: MenuDto[] = [];
  dailyMenuDefinitions: DailyMenuDefinitionDto[] = [];

  constructor(
    private announcer: LiveAnnouncer,
    private menuService: MenuService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMenuData();
    this.menuCtrl.valueChanges.subscribe(value => this.filterMenues(value));
  }

  public loadMenuData(): void {
    this.menuService.getMenu().subscribe(menuData => {
      this.menuData = menuData;
      this.filteredMenues = menuData;
      this.setStep(-1);
    });
  }

  private filterMenues(value: string | null): void {
    const availableMenues = this.menuData.filter(menu => !this.selectedMenues.some(selected => selected.id === menu.id));
    this.filteredMenues = value
      ? availableMenues.filter(menu => menu.name.toLowerCase().includes(value.toLowerCase()))
      : availableMenues;
  }

  setStep(index: number): void {
    this.previousStep = this.currentStep;
    this.currentStep = index;
    this.updateMenuDefinitions();
  }

  private async updateMenuDefinitions(): Promise<void> {
    const formattedDate = this.formatDate(this.selectedDate);
    const existingMenuDef = this.dailyMenuDefinitions.find(def => def.dateTime === formattedDate && def.menuType === this.previousStep);

    const menuToSend = existingMenuDef 
      ? { ...existingMenuDef, menuIds: this.selectedMenues.map(menu => menu.id) }
      : new DailyMenuDefinitionDto(this.selectedMenues.map(menu => menu.id), this.previousStep, formattedDate);

    if (!existingMenuDef) {
      this.dailyMenuDefinitions.push(menuToSend);
    }

    this.updateSelectedAndFilteredMenues(formattedDate);
    this.menuService.postDailyMenu(menuToSend).subscribe(
      () => console.log('Menu successfully updated'),
      error => console.error('Error updating menu', error)
    );
  }

  private formatDate(date: Date | null): string {
    return date ? date.toLocaleDateString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric' }) : '';
  }

  private updateSelectedAndFilteredMenues(formattedDate: string): void {
    const selectedMenuDefs = this.dailyMenuDefinitions
      .filter(def => def.menuType === this.currentStep && def.dateTime === formattedDate);

    const selectedMenuIds = selectedMenuDefs.flatMap(def => def.menuIds);
    this.selectedMenues = this.menuData.filter(menu => selectedMenuIds.includes(menu.id));
    this.filteredMenues = this.menuData.filter(menu => !selectedMenuIds.includes(menu.id));
  }

  async nextStep(): Promise<void> {
    this.previousStep = this.currentStep;
    this.currentStep++;
    await this.updateMenuDefinitions();
  }

  prevStep(): void {
    this.previousStep = this.currentStep;
    this.currentStep--;
    this.updateMenuDefinitions();
  }

  getMenuTitle(menu: MenuType): string {
    return MenuTypeDescriptions[menu];
  }

  public onDateChange(): void {
    const formattedDate = this.formatDate(this.selectedDate);
    this.menuService.getDailyMenuDefinitions(formattedDate).subscribe(
      menuDefinitions => {
        this.dailyMenuDefinitions = menuDefinitions;
        this.updateSelectedAndFilteredMenues(formattedDate);
      },
      error => console.error('Error fetching daily menu definitions', error)
    );
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (value) {
      const menu = this.menuData.find(menu => menu.id === value);
      if (menu && !this.selectedMenues.includes(menu)) {
        this.selectedMenues.push(menu);
        this.menuCtrl.setValue(null);
      }
    }
    event.chipInput!.clear();
  }

  navigateToDailyMenu(): void {
    const formattedDate = this.formatDate(this.selectedDate);
    this.router.navigate(['menu/daily-menu-overview'], { queryParams: { selectedDate: formattedDate } });
  }

  remove(menuId: string): void {
    this.selectedMenues = this.selectedMenues.filter(menu => menu.id !== menuId);
    this.announcer.announce(`Removed menu with ID ${menuId}`);
    this.menuCtrl.setValue(null);
    this.filteredMenues = this.menuData.filter(menu => !this.selectedMenues.some(selected => selected.id === menu.id));
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    const selectedMenuId = event.option.value;
    const menu = this.menuData.find(menu => menu.id === selectedMenuId);
    if (menu && !this.selectedMenues.includes(menu)) {
      this.selectedMenues.push(menu);
      this.filteredMenues = this.menuData.filter(menu => !this.selectedMenues.some(selected => selected.id === menu.id));
    }
    this.fruitInput.nativeElement.value = '';
    this.menuCtrl.setValue(null);
  }
}
