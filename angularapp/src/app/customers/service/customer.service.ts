// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

import { UserLoginModel } from 'src/app/login/models/user-login-model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerModel } from '../models/customer-model';


@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly API_URL = 'https://localhost:44359/api'; // Replace with your API URL

  constructor(private http: HttpClient, private router: Router) {}

  public get isAuthenticated(): boolean {
    return localStorage.getItem('accessToken') != null;
  }

  public get token(): string {
    return localStorage.getItem('accessToken')!;
  }


  register(userLoginModel: CustomerModel): Observable<any> {
    return this.http.post<CustomerModel>(`${this.API_URL}/register`, userLoginModel).pipe(
      catchError(this.handleError)
    );
  }

 
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(error.error.message));
  }

}

