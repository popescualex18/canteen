import { Component, OnDestroy, OnInit } from '@angular/core';
import { SharedService } from '../shared/services/shared/shared.service';
import { AuthService } from '../shared/services/auth/auth.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent implements OnInit  {
  isVisible = false

  constructor(private sharedService: SharedService, private authService: AuthService) {
    
  }
  ngOnInit(): void {
    this.sharedService.spinnerElementSubject.subscribe({
      next: (v) => {
        this.isVisible = v
      }
    })
   
  }

}
