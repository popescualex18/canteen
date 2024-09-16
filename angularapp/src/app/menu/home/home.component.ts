import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuService } from '../services/menu.service';
import { MenuDto } from '../models/menu-item.model';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { CategoryEnum } from 'src/app/common/category-enum';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ManageMenuDialogComponent } from '../manage-menu-dialog/manage-menu-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  displayedColumns: string[] = ['name', 'price', 'hasPolenta', 'hasBread', 'categoryId', 'actions'];
  dataSource!: MatTableDataSource<MenuDto>;
  menuData!: MenuDto[];
  pagedMenuData: MenuDto[] = [];
  @ViewChild('filter') filterInput!: ElementRef<HTMLInputElement>;
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  constructor(private menuService: MenuService, private snackBar:MatSnackBar, private dialog: MatDialog) { }
  ngOnInit(): void {
    this.getMenus();
  }
  
  private getMenus() {
    this.menuService.getMenu().subscribe(menuData => {
      this.menuData = menuData;
      this.filterResults(this.filterInput.nativeElement.value)
    }, (error) => {
      alert(error.message);
      this.menuData = [];
    });
  }

  filterResults(text: string) {
    if (!text) {
      this.pagedMenuData = this.menuData;
      return;
    }

    this.pagedMenuData = this.menuData.filter(
      housingLocation => housingLocation?.name.toLowerCase().includes(text.toLowerCase())

    );
  }
  delete(id: string) {
    this.menuService.deleteMenu(id).subscribe(
      response => {
        this.snackBar.open('Meniul a fost sters cu succes.', '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['success']
        });
       this.getMenus();
      },
      error => {
        console.error('Error deleting data:', error);
      }
    );
  }
  
  getCategoryName(categoryId: Number) {
    return CategoryEnum[Number(categoryId)];
  }

  edit(id:string) {
    const dialogRef = this.dialog.open(ManageMenuDialogComponent, {
      width: '600px',
      data: { id: id } // Pass ID if editing an existing item
    });

    dialogRef.afterClosed().subscribe(result => {
      if(ManageMenuDialogComponent.editted){
        this.getMenus()
      }
      
    });
  }
  addNew() {
    const dialogRef = this.dialog.open(ManageMenuDialogComponent, {
      width: '600px',
      
    });

    dialogRef.afterClosed().subscribe(result => {
      if(ManageMenuDialogComponent.editted){
        this.getMenus()
      }
      
    });
  }

}
