import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { first, mergeMap, Observable } from 'rxjs';

import { ConfigurationMode } from '@shared/constants/configuration-mode.enum';

import {
  EmployeeModel,
  EmployeeRole,
  employeeRoleName
} from '@models/employee/employee.model';
import { EmployeeActions } from '@services/employee/employee.action';
import { EmployeeService } from '@services/employee/employee.service';
import { EmployeeState } from '@services/employee/employee.state';

@Component({
  selector: 'employee-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class EmployeeGridComponent implements OnInit {
  @Select(EmployeeState.getEmployees) employees$: Observable<
    Array<EmployeeModel>
  >;
  selected = new Array<EmployeeModel>();

  constructor(
    private store: Store,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getEmployees();
  }

  getEmployees(): void {
    this.store.dispatch(new EmployeeActions.Get());
  }

  getEmployeeRoleName(role: EmployeeRole): string {
    return employeeRoleName.get(role) ?? '';
  }

  getManagerName(managerId: number): string {
    if (!managerId) return '';

    let managerName = '';

    this.employees$
      .pipe(
        mergeMap((employees) => employees),
        first((employee) => employee.id === managerId)
      )
      .subscribe({
        next: (employee) =>
          (managerName = `${employee.firstName} ${employee.lastName}`),
        error: (err) => {
          console.log(err);
        },
      });

    return managerName;
  }

  createNewEmployee(): void {
    this.router.navigate([ConfigurationMode.Create], {
      relativeTo: this.route,
    });
  }

  editSelectedEmployee(): void {
    if (this.selected.length !== 1) return;

    this.router.navigate([this.selected[0].id], { relativeTo: this.route });
  }

  deleteSelectedEmployees(): void {
    for (const employee of this.selected) {
      this.store.dispatch(new EmployeeActions.Delete(employee.id));
    }
  }
}
