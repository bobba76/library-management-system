import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';

import {
  LibraryItemModel,
  LibraryItemType,
  libraryItemTypeName,
} from '@models/library-item/library-item.model';
import { first, tap } from 'rxjs';
import { LibraryItemActions } from './library-item.action';
import { LibraryItemService } from './library-item.service';

export interface LibraryItemStateModel {
  libraryItem: LibraryItemModel;
  libraryItems: Array<LibraryItemModel>;
}

@State<LibraryItemStateModel>({
  name: 'libraryItems',
  defaults: {
    libraryItem: new LibraryItemModel(),
    libraryItems: new Array<LibraryItemModel>(),
  },
})
@Injectable()
export class LibraryItemState {
  constructor(private libraryItemService: LibraryItemService) {}

  /* ---------- Selectors ---------- */
  @Selector([LibraryItemState])
  static getLibraryItems(state: LibraryItemStateModel) {
    return state.libraryItems;
  }

  @Selector([LibraryItemState])
  static getLibraryItem(state: LibraryItemStateModel) {
    return state.libraryItem;
  }

  @Selector([LibraryItemState])
  static getBorrowableLibraryItems(state: LibraryItemStateModel) {
    return state.libraryItems.filter(
      (l) => l.isBorrowable && l.borrower === null
    );
  }

  /* ---------- Actions ---------- */
  @Action(LibraryItemActions.ClearStore)
  clearStore(ctx: StateContext<LibraryItemStateModel>) {
    ctx.setState({
      libraryItem: new LibraryItemModel(),
      libraryItems: new Array<LibraryItemModel>(),
    });
  }

  @Action(LibraryItemActions.Get)
  get(ctx: StateContext<LibraryItemStateModel>) {
    return this.libraryItemService.getLibraryItems().pipe(
      tap((libraryItems) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          libraryItems,
        });
      })
    );
  }

  @Action(LibraryItemActions.GetById)
  getById(
    ctx: StateContext<LibraryItemStateModel>,
    { id }: LibraryItemActions.GetById
  ) {
    return this.libraryItemService.getLibraryItemById(id).pipe(
      tap((libraryItem) => {
        const state = ctx.getState();

        ctx.setState({
          ...state,
          libraryItem,
        });
      })
    );
  }

  @Action(LibraryItemActions.Create)
  Create(
    ctx: StateContext<LibraryItemStateModel>,
    { inputModel }: LibraryItemActions.Create
  ) {
    return this.libraryItemService
      .createLibraryItem(inputModel)
      .pipe(first())
      .subscribe({
        next: (libraryItems) =>
          ctx.dispatch(new LibraryItemActions.CreateSuccessful(libraryItems)),
        error: () => ctx.dispatch(new LibraryItemActions.CreateFailed()),
      });
  }

  @Action(LibraryItemActions.CreateSuccessful)
  CreateSuccessful(
    ctx: StateContext<LibraryItemStateModel>,
    { libraryItems }: LibraryItemActions.CreateSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      libraryItems,
    });
  }

  @Action(LibraryItemActions.Update)
  Update(
    ctx: StateContext<LibraryItemStateModel>,
    { id, inputModel }: LibraryItemActions.Update
  ) {
    return this.libraryItemService
      .updateLibraryItem(id, inputModel)
      .pipe(first())
      .subscribe({
        next: (libraryItem) =>
          ctx.dispatch(new LibraryItemActions.UpdateSuccessful(libraryItem)),
        error: () => ctx.dispatch(new LibraryItemActions.UpdateFailed()),
      });
  }

  @Action(LibraryItemActions.UpdateSuccessful)
  UpdateSuccessful(
    ctx: StateContext<LibraryItemStateModel>,
    { libraryItem }: LibraryItemActions.UpdateSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      libraryItem,
    });
  }

  @Action(LibraryItemActions.Delete)
  Delete(
    ctx: StateContext<LibraryItemStateModel>,
    { id }: LibraryItemActions.Delete
  ) {
    return this.libraryItemService
      .deleteLibraryItem(id)
      .pipe(first())
      .subscribe({
        next: (libraryItems) =>
          ctx.dispatch(new LibraryItemActions.DeleteSuccessful(libraryItems)),
        error: () => ctx.dispatch(new LibraryItemActions.DeleteFailed()),
      });
  }

  @Action(LibraryItemActions.DeleteSuccessful)
  DeleteSuccessful(
    ctx: StateContext<LibraryItemStateModel>,
    { libraryItems }: LibraryItemActions.DeleteSuccessful
  ) {
    const state = ctx.getState();

    return ctx.setState({
      ...state,
      libraryItems,
    });
  }
}
