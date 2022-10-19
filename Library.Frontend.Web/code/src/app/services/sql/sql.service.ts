import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { EndPointController } from '@shared/constants/endpoint-controller.enum';

@Injectable()
export class SQLService {
  url = `${environment.envApiBaseUrl}/${EndPointController.sql}`;

  constructor(private http: HttpClient) {}

  updateConnectionString(inputModel: {
    connectionString: string;
  }): Observable<string> {
    const route = `/setup/connection-string`;
    const url = this.url + route;

    return this.http.put<string>(url, inputModel, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    });
  }

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
