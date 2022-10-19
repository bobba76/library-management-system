import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { CategoryService } from '@services/category/category.service';
import { EmployeeService } from '@services/employee/employee.service';
import { LibraryItemService } from '@services/library-item/library-item.service';
import { SQLService } from '@services/sql/sql.service';
import { FormService } from '@shared/services/form.service';
import { HttpErrorInterceptor } from '@shared/services/http-error-interceptor.service';

export const services = [
  FormService,
  EmployeeService,
  CategoryService,
  LibraryItemService,
  SQLService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true,
  },
];
