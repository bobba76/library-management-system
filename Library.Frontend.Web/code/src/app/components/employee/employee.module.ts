import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { EmployeeConfigurationComponent } from '@components/employee/configuration/configuration.component';
import { EmployeeComponent } from '@components/employee/employee.component';
import { EmployeeGridComponent } from '@components/employee/grid/grid.component';

const routes: Routes = [
  {
    path: 'employee',
    component: EmployeeComponent,
  },
  {
    path: 'employee/:id',
    component: EmployeeConfigurationComponent,
  },
];

const components = [
  EmployeeComponent,
  EmployeeConfigurationComponent,
  EmployeeGridComponent,
];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class EmployeeModule {}
