import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { EmployeeService } from '@services/employee/employee.service';
import { FormService } from '@shared/services/form.service';
import { HttpErrorInterceptor } from '@shared/services/http-error-interceptor.service';

export const services = [
  FormService,
  EmployeeService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true,
  },
];
