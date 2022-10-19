import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsModule } from '@ngxs/store';

import { CategoryState } from '@services/category/category.state';
import { EmployeeState } from '@services/employee/employee.state';
import { LibraryItemState } from '@services/library-item/library-item.state';
import { SQLState } from '@services/sql/sql.state';

export const storeModules = [
  NgxsReduxDevtoolsPluginModule.forRoot(),
  NgxsModule.forRoot([]),
  NgxsModule.forFeature([CategoryState]),
  NgxsModule.forFeature([EmployeeState]),
  NgxsModule.forFeature([LibraryItemState]),
  NgxsModule.forFeature([SQLState]),
];
