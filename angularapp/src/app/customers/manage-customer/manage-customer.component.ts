import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomerService } from '../service/customer.service';
import { CustomerModel } from '../models/customer-model';
import { SnackbarService } from 'src/app/shared/services/snackbar/snackbar.service';

@Component({
  selector: 'app-manage-customer',
  templateUrl: './manage-customer.component.html',
  styleUrls: ['./manage-customer.component.css']
})
export class ManageCustomerComponent implements OnInit {
  form!: any;
  responseErrorMessage: undefined;
  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<ManageCustomerComponent>, private snackBar: SnackbarService, private customerService: CustomerService,
    @Inject(MAT_DIALOG_DATA) public data: { id?: string }) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      surName: ['', [Validators.required, Validators.minLength(3)]],
      address: ['', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&_])[A-Za-z\d@$#!%*?&_].{7,}$/)
        ]
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&_])[A-Za-z\d@$#!%*?&_].{7,}$/)
        ]
      ],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, romanianPhoneNumberValidator]],
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }
  onSubmit() {
    if (this.form.valid) {
     this.customerService.register(this.form.value).subscribe({
      next: () => this.snackBar.show("Clientul a fost creat", "success"),
      error: (v) => {
        this.responseErrorMessage = v;
        console.log(v)
        this.snackBar.show(v, "error");
      }
     });
    }
  }
}
export function romanianPhoneNumberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {

    if (!control.value) {
      return null; // Don't validate empty values (use required validator if needed)
    }

    // Regular expression to match Romanian mobile phone numbers
    const regex = /^(?:\+40|0040)?07\d{8}$/;

    return regex.test(control.value) ? null : { invalidPhoneNumber: true };
  };
}
