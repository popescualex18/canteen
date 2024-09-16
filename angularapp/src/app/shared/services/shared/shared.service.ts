import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  constructor() {
  }
  private hideElementSubject: Subject<boolean> = new Subject<boolean>();
  hideElement$: Observable<boolean> = this.hideElementSubject.asObservable();

  spinnerElementSubject: Subject<boolean> = new Subject<boolean>();

  setElementVisibility(hide: boolean) {
    this.hideElementSubject.next(hide);
  }

  setSpinnerVisibility(hide: boolean) {
    this.spinnerElementSubject.next(hide);
  }

}
