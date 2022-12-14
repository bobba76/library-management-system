import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClrDatagridComparatorInterface } from '@clr/angular';
import { Actions, ofActionSuccessful, Select, Store } from '@ngxs/store';
import { first, mergeMap, Observable, take } from 'rxjs';

import {
  LibraryItemModel,
  LibraryItemType,
  libraryItemTypeName,
  UpdateLibraryItemInputModel,
} from '@models/library-item/library-item.model';
import { LibraryItemActions } from '@services/library-item/library-item.action';
import { LibraryItemState } from '@services/library-item/library-item.state';

import { CategoryModel } from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';

@Component({
  selector: 'library-return-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class LibraryReturnGridComponent implements OnInit {
  @Select(LibraryItemState.getLibraryItems) libraryItems$: Observable<
    Array<LibraryItemModel>
  >;
  @Select(CategoryState.getCategories) categories$: Observable<
    Array<CategoryModel>
  >;
  selected = new Array<LibraryItemModel>();

  constructor(
    private store: Store,
    private router: Router,
    private actions$: Actions
  ) {}

  ngOnInit() {
    this.getLibraryItems();
    this.getCategories();
  }

  getLibraryItems(): void {
    this.store.dispatch(new LibraryItemActions.Get());
  }

  getCategories(): void {
    this.store.dispatch(new CategoryActions.Get());
  }

  getLibraryItemTypeName(type: LibraryItemType): string {
    return libraryItemTypeName.get(type) ?? '';
  }

  getCategoryName(categoryId: number): string {
    if (!categoryId) return '';

    let categoryName = '';

    this.categories$
      .pipe(
        mergeMap((categories) => categories),
        first((category) => category.id === categoryId)
      )
      .subscribe({
        next: (category) => (categoryName = category.categoryName),
        error: (err) => {
          console.error(err);
        },
      });

    return categoryName;
  }

  returnSelectedLibraryItems(): void {
    for (const libraryItem of this.selected) {
      const inputModel: UpdateLibraryItemInputModel = {
        borrower: null,
        borrowDate: null,
      };

      this.store.dispatch(
        new LibraryItemActions.Update(libraryItem.id, inputModel)
      );
    }

    this.navigateBackOnSuccess(this.selected.length);
  }

  // Need to have the same amount of successful updates as parameter to navigate back.
  navigateBackOnSuccess(amount: number): void {
    this.actions$
      .pipe(
        ofActionSuccessful(LibraryItemActions.UpdateSuccessful),
        take(amount)
      )
      .subscribe({
        complete: () => {
          this.router.navigate(['library']);
        },
      });
  }

  getAcronym(word: string): string {
    return word
      .match(/\b(\w)/g)
      .join('')
      .toUpperCase();
  }
}
