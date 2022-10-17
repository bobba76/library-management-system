import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsModule } from '@ngxs/store';

import { EmployeeState } from '@services/employee/employee.state';

export const storeModules = [
  NgxsReduxDevtoolsPluginModule.forRoot(),
  NgxsModule.forRoot([]),
  NgxsModule.forFeature([EmployeeState])
];
