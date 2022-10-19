import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';

import {
  EmployeeModel,
  EmployeeRole,
  employeeRoleName
} from '@models/employee/employee.model';
import { tap } from 'rxjs';
import { EmployeeActions } from './employee.action';
import { EmployeeService } from './employee.service';

export interface EmployeeStateModel {
  employee: EmployeeModel;
  employees: Array<EmployeeModel>;
}

// TODO: Kolla om man kan ta bort defaults
@State<EmployeeStateModel>({
  name: 'employees',
  defaults: {
    employee: new EmployeeModel(),
    employees: new Array<EmployeeModel>(),
  },
})
@Injectable()
export class EmployeeState {
  constructor(private employeeService: EmployeeService) {}

  /* ---------- Selectors ---------- */
  @Selector([EmployeeState])
  static getEmployees(state: EmployeeStateModel) {
    return state.employees;
  }

  @Selector([EmployeeState])
  static getEmployee(state: EmployeeStateModel) {
    return state.employee;
  }

  // Only Managers and CEO (If employee is selected, do not include)
  @Selector([EmployeeState])
  static getManagersAndCEO(state: EmployeeStateModel) {
    return state.employees.filter(
      (e) =>
        (e.role === EmployeeRole.Manager || e.role === EmployeeRole.Ceo) &&
        e.id !== state.employee.id
    );
  }

  // Only Managers (If employee is selected, do not include)
  @Selector([EmployeeState])
  static getManagers(state: EmployeeStateModel) {
    return state.employees.filter(
      (e) => e.role === EmployeeRole.Manager && e.id !== state.employee.id
    );
  }

  /* ---------- Actions ---------- */
  @Action(EmployeeActions.ClearStore)
  clearStore(ctx: StateContext<EmployeeStateModel>) {
    ctx.setState({
      employee: new EmployeeModel(),
      employees: new Array<EmployeeModel>(),
    });
  }

  @Action(EmployeeActions.Get)
  get(ctx: StateContext<EmployeeStateModel>) {
    return this.employeeService.getEmployees().pipe(
      tap((employees) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          employees,
        });
      })
    );
  }

  @Action(EmployeeActions.GetById)
  getById(
    ctx: StateContext<EmployeeStateModel>,
    { id }: EmployeeActions.GetById
  ) {
    return this.employeeService.getEmployeeById(id).pipe(
      tap((employee) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          employee,
        });
      })
    );
  }

  @Action(EmployeeActions.Create)
  Create(
    ctx: StateContext<EmployeeStateModel>,
    { inputModel }: EmployeeActions.Create
  ) {
    return this.employeeService.createEmployee(inputModel).pipe(
      tap((employees) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          employees,
        });
      })
    );
  }

  @Action(EmployeeActions.Update)
  Update(
    ctx: StateContext<EmployeeStateModel>,
    { id, inputModel }: EmployeeActions.Update
  ) {
    return this.employeeService.updateEmployee(id, inputModel).pipe(
      tap((employee) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          employee,
        });
      })
    );
  }

  @Action(EmployeeActions.Delete)
  Delete(
    ctx: StateContext<EmployeeStateModel>,
    { id }: EmployeeActions.Delete
  ) {
    return this.employeeService.deleteEmployee(id).pipe(
      tap((employees) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          employees,
        });
      })
    );
  }
}
