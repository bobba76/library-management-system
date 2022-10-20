import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { EmployeeActions } from '@services/employee/employee.action';

@Component({
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss'],
})
export class EmployeeComponent implements OnDestroy {
  constructor(private store: Store) {}

  ngOnDestroy(): void {
    this.store.dispatch(EmployeeActions.ClearStore);
  }
}
