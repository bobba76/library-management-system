import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { EndPointController } from '@shared/constants/endpoint-controller.enum';

import {
  CreateEmployeeInputModel,
  EmployeeModel,
  UpdateEmployeeInputModel
} from '@models/employee/employee.model';

@Injectable()
export class EmployeeService {
  url = `${environment.envApiBaseUrl}/${EndPointController.employee}`;

  constructor(private http: HttpClient) {}

  getEmployees(): Observable<Array<EmployeeModel>> {
    return this.http.get<Array<EmployeeModel>>(this.url);
  }

  getEmployeeById(id: number): Observable<EmployeeModel> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.get<EmployeeModel>(url);
  }

  createEmployee(
    inputModel: CreateEmployeeInputModel
  ): Observable<Array<EmployeeModel>> {
    const url = this.url;
    const body = inputModel;

    return this.http.post<Array<EmployeeModel>>(url, body);
  }

  updateEmployee(
    id: number,
    inputModel: UpdateEmployeeInputModel
  ): Observable<EmployeeModel> {
    const route = `/${id}`;
    const url = this.url + route;
    const body = inputModel;

    return this.http.put<EmployeeModel>(url, body);
  }

  deleteEmployee(id: number): Observable<Array<EmployeeModel>> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.delete<Array<EmployeeModel>>(url);
  }
}
