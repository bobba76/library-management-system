import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';

import { CategoryActions } from '@services/category/category.action';
import { LibraryItemActions } from '@services/library-item/library-item.action';

@Component({
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss'],
})
export class LibraryItemComponent implements OnDestroy {
  constructor(private store: Store) {}

  ngOnDestroy(): void {
    this.store.dispatch(LibraryItemActions.ClearStore);
    this.store.dispatch(CategoryActions.ClearStore);
  }
}
