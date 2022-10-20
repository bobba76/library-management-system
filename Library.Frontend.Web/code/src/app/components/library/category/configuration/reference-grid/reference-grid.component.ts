import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Select } from '@ngxs/store';
import { map, Observable } from 'rxjs';

import { CategoryModel } from '@models/category/category.model';
import { LibraryItemModel } from '@models/library-item/library-item.model';
import { CategoryState } from '@services/category/category.state';
import { LibraryItemState } from '@services/library-item/library-item.state';

@Component({
  selector: 'configuration-reference-grid',
  templateUrl: './reference-grid.component.html',
  styleUrls: ['./reference-grid.component.scss'],
})
export class ConfigurationReferenceGridComponent {
  @Select(CategoryState.getCategory) category$: Observable<CategoryModel>;
  @Select(LibraryItemState.getLibraryItems) libraryItems$: Observable<
    Array<LibraryItemModel>
  >;
  selected = new Array<LibraryItemModel>();

  constructor(private route: ActivatedRoute, private router: Router) {}

  getLibraryItemReferences(): Observable<Array<LibraryItemModel>> {
    const id = parseInt(this.route.snapshot.paramMap.get('id'));

    return this.libraryItems$.pipe(
      map((libraryItems) =>
        libraryItems.filter((libraryItem) => libraryItem.categoryId === id)
      )
    );
  }

  goToSelectedLibraryItem(): void {
    if (this.selected.length !== 1) return;

    // Refreshes site on route change to get new data.
    this.redirectTo('/library/library-item/' + this.selected[0].id);
  }

  redirectTo(uri: string) {
    this.router
      .navigateByUrl('/', { skipLocationChange: true })
      .then(() => this.router.navigate([uri]));
  }
}
