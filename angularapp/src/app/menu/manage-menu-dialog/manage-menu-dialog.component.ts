import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CategoryEnum } from 'src/app/common/category-enum';
import { HttpService } from 'src/app/shared/services/http/http.service';
import { MenuService } from '../services/menu.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MenuDto } from '../models/menu-item.model';

@Component({
  selector: 'app-manage-menu-dialog',
  templateUrl: './manage-menu-dialog.component.html',
  styleUrls: ['./manage-menu-dialog.component.css']
})
export class ManageMenuDialogComponent implements OnInit {
  static editted: boolean = false;
  form!: FormGroup;
  categoryOptions: { name: string; value: number }[] = [];
  isEditing = false;
id:string | null = null;
  menuDto: MenuDto = new MenuDto();
  constructor(
    public httpService: HttpService,
    private activatedRoute: ActivatedRoute,
    private menuService: MenuService,
    private snackBar:MatSnackBar,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ManageMenuDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { id?: string } // ID of the item to edit
  ) { }

  ngOnInit() {
    ManageMenuDialogComponent.editted = false;
    this.getCategoryOptions();
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      price: [0, Validators.required],
      categoryId: ['', Validators.required],
      breadOrPolenta: ['bread'],
    });

    if (this.data?.id) {
      this.id = this.data.id;
      this.isEditing = true;
      // Load the menu data based on ID
      this.loadMenuData(this.data.id);
    }
    this.formDefaultValues()
  }
  getCategoryOptions() {
    for (let i = 1; i <= 5; i++) {
      this.categoryOptions.push({
        name: CategoryEnum[i],
        value: i,
      });
    }
  }

  loadMenuData(id: string) {
    this.menuService.getMenuById(id).subscribe(data => {
      this.form.patchValue(data);
      if(data.hasBread === true){
        this.form.controls['breadOrPolenta'].patchValue('bread');
      }
      if(data.hasPolenta === true){
        this.form.controls['breadOrPolenta'].patchValue('polenta');
      }
    }
    );
  }

  onSubmit() {
    if (this.form.valid) {
      this.menuDto.id = this.id ?? "";
      this.menuService.createOrUpdateMenu(this.menuDto).subscribe(
        () => {
          ManageMenuDialogComponent.editted = true;
          let action = ""; 
          if(this.id) {
            action = "actualizat";
          } else {
            action = "creat";
            this.resetSelection();
          }
          this.snackBar.open('Meniul a fost ' + action + ' cu succes.', '×', {
            verticalPosition: 'bottom',
            duration: 3000,
            panelClass: ['success'] 
          });
        },
        (error) => {
          console.error('Error updating menu', error);
          if (this.id) {
            this.form.patchValue(this.menuDto);
          }
          this.snackBar.open('A intervenit o eroare', '×', {
            verticalPosition: 'bottom',
            duration: 3000,
            panelClass: ['success']
          });
        }
      );
    }
  }

  close() {
    console.log("cancel")
    this.dialogRef.close(true);
  }

  isResetButtonVisible() {
    return this.form.controls['breadOrPolenta'].value
  }

  reset() {
    this.form.controls['breadOrPolenta'].setValue(null)
  }
  
  resetSelection() {
    this.form.reset();
  }

  private onFormDataChange() {
    this.form.valueChanges.subscribe(formValues => {
      this.menuDto.id = formValues.id;
      this.menuDto.name = formValues.name;
      this.menuDto.price = formValues.price;
      this.menuDto.categoryId = formValues.categoryId;
      this.menuDto.hasBread = formValues.breadOrPolenta === "bread";
      this.menuDto.hasPolenta = formValues.breadOrPolenta === 'polenta';
    });
  }

  private formDefaultValues() {
    this.form = this.fb.group({
      "id": this.id ?? 0,
      "name": [null, Validators.compose([Validators.required, Validators.minLength(3)])],
      "price": [null, Validators.required],
      "categoryId": [null, Validators.required],
      "breadOrPolenta": [null],
    });
    this.onFormDataChange();
  }
}
