import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';

import {
  EmployeeModel,
  EmployeeRole,
  employeeRoleName
} from '@models/employee/employee.model';
import { EmployeeActions } from '@services/employee/employee.action';
import { EmployeeState } from '@services/employee/employee.state';

@Component({
  selector: 'configuration-manager-to-grid',
  templateUrl: './manager-to-grid.component.html',
  styleUrls: ['./manager-to-grid.component.scss'],
})
export class ConfigurationManagerToGridComponent {
  @Select(EmployeeState.getEmployees) employees$: Observable<
    Array<EmployeeModel>
  >;
  @Select(EmployeeState.getEmployee) employee$: Observable<EmployeeModel>;
  selected = new Array<EmployeeModel>();

  constructor(
    private store: Store,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  getManagerReferences(): Observable<Array<EmployeeModel>> {
    const id = parseInt(this.route.snapshot.paramMap.get('id'));

    return this.employees$.pipe(
      map((employees) => employees.filter((e) => e.managerId === id))
    );
  }

  getEmployeeRoleName(role: EmployeeRole): string {
    return employeeRoleName.get(role) ?? '';
  }

  editSelectedEmployee(): void {
    if (this.selected.length !== 1) return;

    // Refreshes site on route change to get new data.
    this.redirectTo('employee/' + this.selected[0].id);
  }

  redirectTo(uri: string) {
    this.router
      .navigateByUrl('/', { skipLocationChange: true })
      .then(() => this.router.navigate([uri]));
  }

  deleteSelectedReferences(): void {
    for (const employee of this.selected) {
      this.store.dispatch(
        new EmployeeActions.Update(employee.id, { managerId: null })
      );
    }
  }
}
