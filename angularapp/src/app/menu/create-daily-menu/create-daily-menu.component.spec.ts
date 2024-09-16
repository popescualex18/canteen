import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDailyMenuComponent } from './create-daily-menu.component';

describe('CreateDailyMenuComponent', () => {
  let component: CreateDailyMenuComponent;
  let fixture: ComponentFixture<CreateDailyMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateDailyMenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDailyMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
