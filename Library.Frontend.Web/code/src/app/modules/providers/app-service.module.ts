import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { EmployeeService } from '@services/employee/employee.service';
import { AlertService } from '@shared/services/alert.service';
import { FormService } from '@shared/services/form.service';
import { HttpErrorInterceptor } from '@shared/services/http-error-interceptor.service';

export const services = [
  AlertService,
  FormService,
  EmployeeService,
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true,
  },
];
