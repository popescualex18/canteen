import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from 'src/app/shared/services/http/http.service';
import { MenuService } from '../services/menu.service';
import { CategoryEnum } from 'src/app/common/category-enum';
import { MenuDto } from '../models/menu-item.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})
export class AddComponent implements OnInit {
  public form!: UntypedFormGroup;
  menuDto: MenuDto = new MenuDto();
  menuBeforeEditValues: MenuDto | null = null;
  categoryOptions: { name: string; value: number }[] = [];
  public id: string = "";

  constructor(
    private httpService: HttpService,
    private formBuilder: UntypedFormBuilder,
    private activatedRoute: ActivatedRoute,
    private menuService: MenuService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getCategoryOptions();
    this.initializeForm();
    this.loadMenuData();
  }

  private initializeForm(): void {
    this.form = this.formBuilder.group({
      id: [this.id ?? 0],
      name: [null, [Validators.required, Validators.minLength(3)]],
      price: [null, Validators.required],
      categoryId: [null, Validators.required],
      breadOrPolenta: [null]
    });

    this.form.valueChanges.subscribe(formValues => {
      this.updateMenuDto(formValues);
    });
  }

  private loadMenuData(): void {
    this.activatedRoute.params.subscribe(params => {
      if (params['id']) {
        this.id = params['id'];
        this.menuService.getMenuById(this.id).subscribe(data => {
          this.form.patchValue(data);
          this.menuBeforeEditValues = data;
          this.form.controls['breadOrPolenta'].setValue(data.hasBread ? 'bread' : data.hasPolenta ? 'polenta' : null);
        });
      }
    });
  }

  private updateMenuDto(formValues: any): void {
    this.menuDto = {
      ...this.menuDto,
      id: formValues.id,
      name: formValues.name,
      price: formValues.price,
      categoryId: formValues.categoryId,
      hasBread: formValues.breadOrPolenta === 'bread',
      hasPolenta: formValues.breadOrPolenta === 'polenta'
    };
  }

  public resetForm(): void {
    this.form.reset({ id: this.id ?? 0 });
    this.form.markAsPristine();
    this.form.markAsUntouched();
  }

  public onSubmit(): void {
    if (this.form.valid) {
      this.menuDto.id = this.id;
      this.menuService.createOrUpdateMenu(this.menuDto).subscribe(
        () => {
          const action = this.id ? 'actualizat' : 'creat';
          if (!this.id) this.resetForm();
          this.menuBeforeEditValues = this.menuDto;
          this.showSnackBar(`Meniul a fost ${action} cu succes.`, 'success');
        },
        (error) => {
          console.error('Error updating menu', error);
          if (this.id) {
            this.menuDto = this.menuBeforeEditValues!;
            this.form.patchValue(this.menuDto);
          }
          this.showSnackBar('A intervenit o eroare', 'error');
        }
      );
    }
  }

  private getCategoryOptions(): void {
    this.categoryOptions = Object.keys(CategoryEnum)
      .filter(key => !isNaN(Number(key)))
      .map(key => ({
        name: CategoryEnum[Number(key)],
        value: Number(key)
      }));
  }

  public resetSelection(): void {
    this.form.controls['breadOrPolenta'].setValue(null);
  }

  public isResetButtonVisible(): boolean {
    return !!this.form.controls['breadOrPolenta'].value;
  }

  private showSnackBar(message: string, panelClass: string): void {
    this.snackBar.open(message, '×', {
      verticalPosition: 'bottom',
      duration: 3000,
      panelClass: [panelClass]
    });
  }
}
