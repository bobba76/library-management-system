import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';

import { CategoryModel } from '@models/category/category.model';
import { first, tap } from 'rxjs';
import { CategoryActions } from './category.action';
import { CategoryService } from './category.service';

export interface CategoryStateModel {
  category: CategoryModel;
  categories: Array<CategoryModel>;
}

@State<CategoryStateModel>({
  name: 'categories',
  defaults: {
    category: new CategoryModel(),
    categories: new Array<CategoryModel>(),
  },
})
@Injectable()
export class CategoryState {
  constructor(private categoryService: CategoryService) {}

  /* ---------- Selectors ---------- */
  @Selector([CategoryState])
  static getCategories(state: CategoryStateModel) {
    return state.categories;
  }

  @Selector([CategoryState])
  static getCategory(state: CategoryStateModel) {
    return state.category;
  }

  /* ---------- Actions ---------- */
  @Action(CategoryActions.ClearStore)
  clearStore(ctx: StateContext<CategoryStateModel>) {
    ctx.setState({
      category: new CategoryModel(),
      categories: new Array<CategoryModel>(),
    });
  }

  @Action(CategoryActions.Get)
  get(ctx: StateContext<CategoryStateModel>) {
    return this.categoryService.getCategorys().pipe(
      tap((categories) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          categories,
        });
      })
    );
  }

  @Action(CategoryActions.GetById)
  getById(
    ctx: StateContext<CategoryStateModel>,
    { id }: CategoryActions.GetById
  ) {
    return this.categoryService.getCategoryById(id).pipe(
      tap((category) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          category,
        });
      })
    );
  }

  @Action(CategoryActions.Create)
  Create(
    ctx: StateContext<CategoryStateModel>,
    { inputModel }: CategoryActions.Create
  ) {
    return this.categoryService
      .createCategory(inputModel)
      .pipe(first())
      .subscribe({
        next: (categories) =>
          ctx.dispatch(new CategoryActions.CreateSuccessful(categories)),
        error: () => ctx.dispatch(new CategoryActions.CreateFailed()),
      });
  }

  @Action(CategoryActions.CreateSuccessful)
  CreateSuccessful(
    ctx: StateContext<CategoryStateModel>,
    { categories }: CategoryActions.CreateSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      categories,
    });
  }

  @Action(CategoryActions.Update)
  Update(
    ctx: StateContext<CategoryStateModel>,
    { id, inputModel }: CategoryActions.Update
  ) {
    return this.categoryService
      .updateCategory(id, inputModel)
      .pipe(first())
      .subscribe({
        next: (category) =>
          ctx.dispatch(new CategoryActions.UpdateSuccessful(category)),
        error: () => ctx.dispatch(new CategoryActions.UpdateFailed()),
      });
  }

  @Action(CategoryActions.UpdateSuccessful)
  UpdateSuccessful(
    ctx: StateContext<CategoryStateModel>,
    { category }: CategoryActions.UpdateSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      category,
    });
  }

  @Action(CategoryActions.Delete)
  Delete(
    ctx: StateContext<CategoryStateModel>,
    { id }: CategoryActions.Delete
  ) {
    return this.categoryService
      .deleteCategory(id)
      .pipe(first())
      .subscribe({
        next: (categories) =>
          ctx.dispatch(new CategoryActions.DeleteSuccessful(categories)),
        error: () => ctx.dispatch(new CategoryActions.DeleteFailed()),
      });
  }

  @Action(CategoryActions.DeleteSuccessful)
  DeleteSuccessful(
    ctx: StateContext<CategoryStateModel>,
    { categories }: CategoryActions.DeleteSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      categories,
    });
  }
}
