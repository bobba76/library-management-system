import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { EndPointController } from '@shared/constants/endpoint-controller.enum';

import {
  CreateLibraryItemInputModel,
  LibraryItemModel,
  UpdateLibraryItemInputModel
} from '@models/library-item/library-item.model';

@Injectable()
export class LibraryItemService {
  url = `${environment.envApiBaseUrl}/${EndPointController.libraryItem}`;

  constructor(private http: HttpClient) {}

  getLibraryItems(): Observable<Array<LibraryItemModel>> {
    return this.http.get<Array<LibraryItemModel>>(this.url);
  }

  getLibraryItemById(id: number): Observable<LibraryItemModel> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.get<LibraryItemModel>(url);
  }

  createLibraryItem(
    inputModel: CreateLibraryItemInputModel
  ): Observable<Array<LibraryItemModel>> {
    const url = this.url;
    const body = inputModel;

    return this.http.post<Array<LibraryItemModel>>(url, body);
  }

  updateLibraryItem(
    id: number,
    inputModel: UpdateLibraryItemInputModel
  ): Observable<LibraryItemModel> {
    const route = `/${id}`;
    const url = this.url + route;
    const body = inputModel;

    return this.http.put<LibraryItemModel>(url, body);
  }

  deleteLibraryItem(id: number): Observable<Array<LibraryItemModel>> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.delete<Array<LibraryItemModel>>(url);
  }
}
