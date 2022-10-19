import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { EndPointController } from '@shared/constants/endpoint-controller.enum';

import {
  CategoryModel,
  CreateCategoryInputModel,
  UpdateCategoryInputModel
} from '@models/category/category.model';

@Injectable()
export class CategoryService {
  url = `${environment.envApiBaseUrl}/${EndPointController.category}`;

  constructor(private http: HttpClient) {}

  getCategorys(): Observable<Array<CategoryModel>> {
    return this.http.get<Array<CategoryModel>>(this.url);
  }

  getCategoryById(id: number): Observable<CategoryModel> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.get<CategoryModel>(url);
  }

  createCategory(
    inputModel: CreateCategoryInputModel
  ): Observable<Array<CategoryModel>> {
    const url = this.url;
    const body = inputModel;

    return this.http.post<Array<CategoryModel>>(url, body);
  }

  updateCategory(
    id: number,
    inputModel: UpdateCategoryInputModel
  ): Observable<CategoryModel> {
    const route = `/${id}`;
    const url = this.url + route;
    const body = inputModel;

    return this.http.put<CategoryModel>(url, body);
  }

  deleteCategory(id: number): Observable<Array<CategoryModel>> {
    const route = `/${id}`;
    const url = this.url + route;

    return this.http.delete<Array<CategoryModel>>(url);
  }
}
