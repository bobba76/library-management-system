import { Component, OnDestroy, OnInit } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClrLoadingState } from '@clr/angular';
import { Actions, ofActionSuccessful, Select, Store } from '@ngxs/store';
import { first, flatMap, map, mergeMap, Observable } from 'rxjs';

import {
  CreateEmployeeInputModel,
  EmployeeModel,
  EmployeeRole,
  salaryCoefficient,
  UpdateEmployeeInputModel,
} from '@models/employee/employee.model';
import { EmployeeActions } from '@services/employee/employee.action';
import { EmployeeState } from '@services/employee/employee.state';
import {
  ConfigurationMode,
  configurationModeName,
} from '@shared/constants/configuration-mode.enum';
import { FormService } from '@shared/services/form.service';

@Component({
  selector: 'employee-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.scss'],
})
export class EmployeeConfigurationComponent implements OnInit, OnDestroy {
  // Access EmployeeRole in HTML
  EmployeeRoleEnum = EmployeeRole;
  ConfigurationModeEnum = ConfigurationMode;

  submitBtnState: ClrLoadingState = ClrLoadingState.DEFAULT;
  loadingState: ClrLoadingState = ClrLoadingState.DEFAULT;

  // Shows which mode the user is in
  configurationMode: ConfigurationMode = ConfigurationMode.Create;

  @Select(EmployeeState.getEmployees) employees$: Observable<
    Array<EmployeeModel>
  >;
  @Select(EmployeeState.getEmployee) employee$: Observable<EmployeeModel>;
  employee = new EmployeeModel();

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    salary: [
      0,
      [
        Validators.required,
        Validators.min(1),
        Validators.max(10),
        Validators.pattern('^[0-9]*$'),
      ],
    ],
    role: [EmployeeRole.Employee, Validators.required],
    managerId: [0],
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: NonNullableFormBuilder,
    private store: Store,
    private actions$: Actions
  ) {}

  ngOnInit() {
    this.loadingState = ClrLoadingState.LOADING;

    const id = this.route.snapshot.paramMap.get('id');

    this.setConfigurationMode(id);

    if (this.configurationMode === ConfigurationMode.Edit)
      this.getEmployeeById(parseInt(id));

    this.updateForm();

    // We need all the employees to be able to set Manager.
    this.getEmployees();
  }

  ngOnDestroy(): void {
    this.store.dispatch(EmployeeActions.ClearStore);
  }

  setConfigurationMode(id: string): void {
    if (!id) this.redirectTo('employee');

    if (id === ConfigurationMode.Create) {
      this.configurationMode = ConfigurationMode.Create;
      return;
    }

    this.configurationMode = ConfigurationMode.Edit;
  }

  getEmployeeById(id: number): void {
    this.store.dispatch(new EmployeeActions.GetById(id));
  }

  getEmployees(): void {
    this.store.dispatch(new EmployeeActions.Get());
  }

  // #region Form
  // Updates form as soon as GetById completes.
  updateForm(): void {
    switch (this.configurationMode) {
      case ConfigurationMode.Edit:
        this.actions$
          .pipe(ofActionSuccessful(EmployeeActions.GetById))
          .subscribe({
            next: () => {
              this.employee$.pipe(first()).subscribe({
                next: (employee) => {
                  this.employee = employee;
                  this.updateFormValues();
                },
                error: (err: any) => {
                  console.error(err);
                  this.redirectTo('employee');
                },
              });
            },
            error: (err) => {
              console.error(err);
              this.redirectTo('employee');
            },
          });

        break;

      case ConfigurationMode.Create:
      default:
        this.employee = new EmployeeModel();
        this.form.controls.salary.setValue(null);
        break;
    }

    this.loadingState = ClrLoadingState.SUCCESS;
  }

  updateFormValues(): void {
    this.form = FormService.fillFormWithData(this.form, this.employee);

    this.form.controls.salary.setValue(
      Math.round(
        this.employee.salary / salaryCoefficient.get(this.employee.role)
      )
    );

    switch (this.employee.role) {
      case EmployeeRole.Ceo:
        this.form.controls.managerId.disable();
        break;
    }
  }

  submit(): void {
    this.navigateBackOnSuccess();

    this.submitBtnState = ClrLoadingState.LOADING;

    switch (this.form.controls.role.value) {
      case EmployeeRole.Ceo:
        this.form.controls.managerId.setValue(null);
        break;
    }

    const id = this.employee.id;

    if (!id) {
      const inputModel: CreateEmployeeInputModel = {
        firstName: this.form.controls.firstName.value,
        lastName: this.form.controls.lastName.value,
        salary: this.form.controls.salary.value,
        role: this.form.controls.role.value,
        managerId: this.form.controls.managerId.value,
      };

      this.createEmployee(inputModel);
      return;
    }

    const inputModel: UpdateEmployeeInputModel = {
      firstName: this.form.controls.firstName.value,
      lastName: this.form.controls.lastName.value,
      salary: this.form.controls.salary.value,
      role: this.form.controls.role.value,
      managerId: this.form.controls.managerId.value,
    };

    this.updateEmployee(id, inputModel);
  }

  createEmployee(inputModel: CreateEmployeeInputModel): void {
    this.store.dispatch(new EmployeeActions.Create(inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  updateEmployee(id: number, inputModel: UpdateEmployeeInputModel): void {
    this.store.dispatch(new EmployeeActions.Update(id, inputModel));

    this.submitBtnState = ClrLoadingState.DEFAULT;
  }

  cancel(): void {
    this.redirectTo('employee');
  }

  formIsInvalid(): boolean {
    return (
      this.form.invalid || this.form.pristine || this.roleEmployeeFormInvalid()
    );
  }

  roleEmployeeFormInvalid(): boolean {
    let managerIsCeo = false;
    this.getManagersAndCEO()
      .pipe(
        mergeMap((employees) => employees),
        first(
          (manager) =>
            manager.id === this.form.controls.managerId.value &&
            manager.role === EmployeeRole.Ceo
        )
      )
      .subscribe(() => (managerIsCeo = true));

    return (
      this.roleEmployeeChosen() &&
      (!this.form.controls.managerId.value || managerIsCeo)
    );
  }

  roleEmployeeChosen(): boolean {
    return this.form.controls.role.value === EmployeeRole.Employee;
  }

  roleManagerChosen(): boolean {
    return this.form.controls.role.value === EmployeeRole.Manager;
  }

  roleCEOChosen(): boolean {
    return this.form.controls.role.value === EmployeeRole.Ceo;
  }
  // #endregion

  // #region Filters
  managerFilter(employee: EmployeeModel) {
    return employee.role === EmployeeRole.Manager;
  }

  managerAndCEOFilter(employee: EmployeeModel) {
    return (
      employee.role === EmployeeRole.Manager ||
      employee.role === EmployeeRole.Ceo
    );
  }
  // #endregion

  // #region Others
  isLoading(): boolean {
    return this.loadingState === ClrLoadingState.LOADING;
  }
  submitIsLoading(): boolean {
    return this.submitBtnState === ClrLoadingState.LOADING;
  }

  switchedRole(role: EmployeeRole): void {
    switch (role) {
      case EmployeeRole.Ceo:
        setTimeout(() => this.form.controls.managerId.disable());
        break;

      case EmployeeRole.Manager:
        setTimeout(() => this.form.controls.managerId.enable());
        break;

      case EmployeeRole.Employee:
      default:
        setTimeout(() => this.form.controls.managerId.enable());
        break;
    }
  }

  getConfigurationModeName(mode: ConfigurationMode): string {
    return configurationModeName.get(mode) ?? '';
  }

  redirectTo(uri: string) {
    this.router
      .navigateByUrl('/', { skipLocationChange: true })
      .then(() => this.router.navigate([uri]));
  }

  getManagers(): Observable<Array<EmployeeModel>> {
    return this.store.select(EmployeeState.getManagers);
  }

  getManagersAndCEO(): Observable<Array<EmployeeModel>> {
    return this.store.select(EmployeeState.getManagersAndCEO);
  }

  isEditMode(): boolean {
    return this.configurationMode === this.ConfigurationModeEnum.Edit;
  }

  navigateBackOnSuccess(): void {
    this.actions$
      .pipe(
        ofActionSuccessful(
          EmployeeActions.CreateSuccessful,
          EmployeeActions.UpdateSuccessful
        )
      )
      .subscribe(() => {
        this.redirectTo('employee');
      });
  }
  // #endregion
}
