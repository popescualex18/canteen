import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderMenuItemsComponent } from './order-menu-items.component';

describe('OrderMenuItemsComponent', () => {
  let component: OrderMenuItemsComponent;
  let fixture: ComponentFixture<OrderMenuItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderMenuItemsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderMenuItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
