// login.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth/auth.service';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserLoginModel } from './models/user-login-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm?: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&_])[A-Za-z\d@$#!%*?&_].{7,}$/)
        ]
      ]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const email = this.loginForm.get('email').value;
      const password = this.loginForm.get('password').value;
      let userLoginModel = new UserLoginModel(email, password)
      this.authService.login(userLoginModel).subscribe({
        next: () => {
          
          this.snackBar.open("Success", '×', {
            verticalPosition: 'bottom',
            duration: 3000,
            panelClass: ['success']
          });
          this.router.navigate(['/'])
        },
        error: (v) => this.snackBar.open(v.message, '×', {
          verticalPosition: 'bottom',
          duration: 3000,
          panelClass: ['error']
        })
      })


    }
  }
}
