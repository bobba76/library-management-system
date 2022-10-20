import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { first, mergeMap, Observable } from 'rxjs';

import {
  LibraryItemModel,
  LibraryItemType,
  libraryItemTypeName,
} from '@models/library-item/library-item.model';
import { LibraryItemActions } from '@services/library-item/library-item.action';
import { LibraryItemState } from '@services/library-item/library-item.state';

import { ActivatedRoute, Router } from '@angular/router';
import { CategoryModel } from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';
import { ConfigurationMode } from '@shared/constants/configuration-mode.enum';

@Component({
  selector: 'item-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class LibraryItemGridComponent implements OnInit {
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
    private route: ActivatedRoute
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

  createNewLibraryItem(): void {
    this.router.navigate([ConfigurationMode.Create], {
      relativeTo: this.route,
    });
  }

  editSelectedLibraryItem(): void {
    if (this.selected.length !== 1) return;

    this.router.navigate([this.selected[0].id], { relativeTo: this.route });
  }

  deleteSelectedLibraryItems(): void {
    for (const libraryItem of this.selected) {
      this.store.dispatch(new LibraryItemActions.Delete(libraryItem.id));
    }
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

  getAcronym(word: string): string {
    return word
      .match(/\b(\w)/g)
      .join('')
      .toUpperCase();
  }
}
