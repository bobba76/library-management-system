import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { EmployeeConfigurationComponent } from '@components/employee/configuration/configuration.component';
import { ConfigurationManagerToGridComponent } from '@components/employee/configuration/manager-to-grid/manager-to-grid.component';
import { EmployeeComponent } from '@components/employee/employee.component';
import { EmployeeGridComponent } from '@components/employee/grid/grid.component';

const routes: Routes = [
  {
    path: '',
    component: EmployeeComponent,
  },
  {
    path: ':id',
    component: EmployeeConfigurationComponent,
  },
];

const components = [
  EmployeeComponent,
  EmployeeConfigurationComponent,
  EmployeeGridComponent,
  ConfigurationManagerToGridComponent,
];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class EmployeeModule {}
