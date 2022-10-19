import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { map, Observable } from 'rxjs';

import { LibraryItemModel } from '@models/library-item/library-item.model';
import { LibraryItemActions } from '@services/library-item/library-item.action';
import { LibraryItemState } from '@services/library-item/library-item.state';

import { ActivatedRoute, Router } from '@angular/router';
import { CategoryModel } from '@models/category/category.model';
import { CategoryActions } from '@services/category/category.action';
import { CategoryState } from '@services/category/category.state';
import { ConfigurationMode } from '@shared/constants/configuration-mode.enum';

@Component({
  selector: 'category-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
})
export class CategoryGridComponent implements OnInit {
  @Select(LibraryItemState.getLibraryItems) libraryItems$: Observable<
    Array<LibraryItemModel>
  >;
  @Select(CategoryState.getCategories) categories$: Observable<
    Array<CategoryModel>
  >;

  selected = new Array<CategoryModel>();

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

  createNewCategory(): void {
    this.router.navigate([ConfigurationMode.Create], {
      relativeTo: this.route,
    });
  }

  editSelectedCategory(): void {
    if (this.selected.length !== 1) return;

    this.router.navigate([this.selected[0].id], { relativeTo: this.route });
  }

  deleteSelectedCategories(): void {
    for (const category of this.selected) {
      this.store.dispatch(new CategoryActions.Delete(category.id));
    }
  }

  getLibraryItemReferences(id: number): Observable<Array<LibraryItemModel>> {
    return this.libraryItems$.pipe(
      map((libraryItems) =>
        libraryItems.filter((libraryItem) => libraryItem.categoryId === id)
      )
    );
  }

  libraryItemReferencesExists(
    libraryItemsToCheck: Array<CategoryModel>
  ): Observable<Array<LibraryItemModel>> {
    return this.libraryItems$.pipe(
      map((libraryItems) =>
        libraryItems.filter((libraryItem) =>
          libraryItemsToCheck.some(
            (libraryItemToCheck) =>
              libraryItemToCheck.id === libraryItem.categoryId
          )
        )
      )
    );
  }
}
