import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { MenuService } from '../services/menu.service';
import { ActivatedRoute } from '@angular/router';
import { DailyMenuDefinitionOverviewDto } from '../models/daily-menu-definition';
import { MenuType } from 'src/app/common/category-enum';
import { SharedService } from 'src/app/shared/services/shared/shared.service';
import * as htmlToImage from 'html-to-image';
import { DailyMenuOrderDialogComponent } from '../daily-menu-order-dialog/daily-menu-order-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { saveAs } from 'file-saver'; // Import saveAs from file-saver

@Component({
  selector: 'app-daily-menu-overview',
  templateUrl: './daily-menu-overview.component.html',
  styleUrls: ['./daily-menu-overview.component.css']
})
export class DailyMenuOverviewComponent implements OnInit {
  overViewData: Record<number, DailyMenuDefinitionOverviewDto[]> = {};
  MenuType = MenuType;
  hideToolbar = false;
  selectedDate: string = "";
  dayName: string = "";

  constructor(
    private menuService: MenuService,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.selectedDate = params['selectedDate'];
      this.getDayName();
      this.loadDailyMenuOverview();
    });
  }

  private loadDailyMenuOverview(): void {
    this.menuService.getDailyMenuOverview(this.selectedDate).subscribe(
      menuItems => {
        this.overViewData = menuItems.reduce((acc, item) => {
          const menuType = item.menuType as number;
          if (!acc[menuType]) {
            acc[menuType] = [];
          }
          acc[menuType].push(item);
          return acc;
        }, {} as Record<number, DailyMenuDefinitionOverviewDto[]>);
      },
      error => console.error('Error loading daily menu overview', error)
    );
  }

  private getDayName(): void {
    const daysOfWeek = ["Duminica", "Luni", "Marti", "Miercuri", "Joi", "Vineri", "Sambata"];
    const dayIndex = new Date(this.selectedDate).getDay();
    this.dayName = daysOfWeek[dayIndex];
  }

  getMenuItemsByType(menuType: MenuType): DailyMenuDefinitionOverviewDto[] {
    return this.overViewData[menuType] || [];
  }

  openDialog(menuType: MenuType): void {
    const dialogRef = this.dialog.open(DailyMenuOrderDialogComponent, {
      width: '600px',
      height: '400px',
      data: { 
        items: this.overViewData[menuType], 
        selector: (item: DailyMenuDefinitionOverviewDto) => item.menuName 
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      // Handle dialog close result if needed
    });
  }

  @HostListener('document:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'F9') {
      this.menuService.downloadScreenshot().subscribe(
        (blob: Blob) => {
          saveAs(blob, 'screenshot.jpg');
        },
        error => console.error('Download error:', error)
      );
    }
  }
}
