import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyMenuOverviewComponent } from './daily-menu-overview.component';

describe('DailyMenuOverviewComponent', () => {
  let component: DailyMenuOverviewComponent;
  let fixture: ComponentFixture<DailyMenuOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DailyMenuOverviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DailyMenuOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
