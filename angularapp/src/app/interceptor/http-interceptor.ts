import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, finalize } from 'rxjs/operators';
import { SharedService } from '../shared/services/shared/shared.service';
import { AuthService } from '../shared/services/auth/auth.service';
// Import the shared service

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private activeRequests = 0;

  constructor(private sharedService: SharedService, private authService : AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.activeRequests++;
    // Show the spinner
    this.sharedService.setSpinnerVisibility(true);
    const authToken = this.authService.token;
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`
      }
    });
    return next.handle(authReq).pipe(
      tap(() => {},
        error => {
          // Handle error (optional)
        }
      ),
      finalize(() => {
        this.activeRequests--;
        if(this.activeRequests == 0) {
          this.sharedService.setSpinnerVisibility(false);
        }
      })
    );
  }
}
