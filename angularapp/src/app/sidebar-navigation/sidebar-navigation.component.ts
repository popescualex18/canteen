import { AfterViewChecked, AfterViewInit, ChangeDetectorRef, Component, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable, Subscription } from 'rxjs';
import { delay, filter, map, shareReplay, timeInterval, timeout } from 'rxjs/operators';
import { SharedService } from '../shared/services/shared/shared.service';
import { MenuService } from '../menu/services/menu.service';
import { saveAs } from 'file-saver'; // Import saveAs from file-saver
import { AuthService } from '../shared/services/auth/auth.service';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { NavigationEnd, Router } from '@angular/router';
import { MatSidenav } from '@angular/material/sidenav';
import { Token } from '@angular/compiler';
import { MatIconButton } from '@angular/material/button';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
@Component({
  selector: 'app-sidebar-navigation',
  templateUrl: './sidebar-navigation.component.html',
  styleUrls: ['./sidebar-navigation.component.css']
})
export class SidebarNavigationComponent implements AfterViewChecked {
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  isLoggedIn!: boolean;
  isDailyMenuRoute!: boolean;
  loggedInUser!: string | null;
  userInitial!: string | null;
  constructor(private observer: BreakpointObserver,
    private router: Router,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private menuService: MenuService) { }

  ngAfterViewChecked(): void {
    this.cdr.detectChanges();
  }

  ngAfterViewInit(): void {
    this.checkRoute();
    this.router.events.subscribe(() => this.checkRoute());
    this.setupListeners();

  }

  private setupListeners() {
    if (this.isLoggedIn) {
      console.log("setup")
      this.observer
        .observe(['(max-width: 800px)'])
        .subscribe((res) => {
          setTimeout(() => {
            if (res.matches) {
              this.sidenav.mode = 'over';
              this.sidenav.close();
            } else {
              this.sidenav.mode = 'side';
              this.sidenav.open();
            }
          }, 500);

        });

      this.router.events
        .pipe(
          filter((e) => e instanceof NavigationEnd)
        )
        .subscribe(() => {
          if (this.sidenav && this.sidenav.mode === 'over') {
            this.sidenav.close();
          }
        });
    }
  }

  checkRoute(): void {
    this.isDailyMenuRoute = this.router.url.startsWith('/menu/daily-menu-overview');
  }

  ngOnInit() {
    this.authService.isAuthenticated.subscribe((status: boolean) => {
      console.log(status)

      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        this.loggedInUser = this.authService.name;
        this.userInitial = this.loggedInUser.charAt(0).toLocaleUpperCase();
      }

      this.setupListeners();
    });

  }


  download() {
    this.menuService.downloadScreenshot().subscribe(
      (blob: Blob) => {
        saveAs(blob, 'screenshot.jpg');
      },
      error => {
        console.error('Download error:', error);
      }
    );

  }

  logout() {
    this.authService.logout();
  }

}
