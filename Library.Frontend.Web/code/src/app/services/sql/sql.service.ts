import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { EndPointController } from '@shared/constants/endpoint-controller.enum';

@Injectable()
export class SQLService {
  url = `${environment.envApiBaseUrl}/${EndPointController.sql}`;

  constructor(private http: HttpClient) {}

  resetData(): Observable<string> {
    const route = `/setup`;
    const url = this.url + route;

    return this.http.post<string>(url, null);
  }

  deleteAllData(): Observable<string> {
    const route = `/setup/data`;
    const url = this.url + route;

    return this.http.delete<string>(url);
  }
}
