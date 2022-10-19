import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { first, mergeMap, Observable } from 'rxjs';

import {
  LibraryItemModel,
  LibraryItemType,
  libraryItemTypeName
} from '@models/library-item/library-item.model';
import { LibraryItemActions } from '@services/library-item/library-item.action';
import { LibraryItemState } from '@services/library-item/library-item.state';

import { CategoryModel } from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';

@Component({
  selector: 'library-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class LibraryGridComponent implements OnInit {
  @Select(LibraryItemState.getLibraryItems) libraryItems$: Observable<
    Array<LibraryItemModel>
  >;
  @Select(CategoryState.getCategories) categories$: Observable<
    Array<CategoryModel>
  >;

  constructor(private store: Store) {}

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
}
