// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError, Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { HttpService } from '../http/http.service';
import { UserLoginModel } from 'src/app/login/models/user-login-model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from 'src/enviroments/enviroments';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private readonly API_URL = `${environment.apiUrl}/auth`; // Replace with your API URL

  constructor(private http: HttpClient, private router: Router, private snackBar: MatSnackBar) {}

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  isAuthenticated = this.isAuthenticatedSubject.asObservable();

  
  private hasToken(): boolean {
    return localStorage.getItem('accessToken') != null;
  }
  public get name(): string {
    return localStorage.getItem('name')!;
  }
  public get token(): string {
    return localStorage.getItem('accessToken')!;
  }


  login(userLoginModel: UserLoginModel): Observable<any> {
    return this.http.post<UserLoginModel>(`${this.API_URL}/login`, userLoginModel).pipe(
      map(response => {
        this.handleLoginResponse(response);
      }),
      catchError(this.handleError)
    );
  }
  updateAuthStatus(): void {
    this.isAuthenticatedSubject.next(this.hasToken());
  }
  private handleLoginResponse(response: any): void {
    const accessToken = response.accessToken;
    const refreshToken = response.refreshToken;
    console.log(response)
    const name = response.fullName;
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('name', name);
    localStorage.setItem('refreshToken', refreshToken);
    this.updateAuthStatus();
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(error.error));
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('name');
    this.updateAuthStatus();
    this.router.navigate(['/login'])
  }

  refreshToken(): Observable<any> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<any>(`${this.API_URL}/refresh-token`, { refreshToken }).pipe(
      tap(response => {
        this.handleLoginResponse(response);
      }),
      catchError(this.handleError)
    );
  }

}
