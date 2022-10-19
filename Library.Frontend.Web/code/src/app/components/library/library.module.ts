import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { LibraryComponent } from '@components/library/library.component';
import { LibraryGridComponent } from './grid/grid.component';

const routes: Routes = [
  {
    path: '',
    component: LibraryComponent,
  },
  {
    path: 'library-item',
    loadChildren: () =>
      import('../../components/library/item/item.module').then(
        (m) => m.LibraryItemModule
      ),
  },
  {
    path: 'category',
    loadChildren: () =>
      import('../../components/library/category/category.module').then(
        (m) => m.CategoryModule
      ),
  },
  {
    path: 'borrow',
    loadChildren: () =>
      import('../../components/library/borrow/borrow.module').then(
        (m) => m.LibraryBorrowModule
      ),
  },
  {
    path: 'return',
    loadChildren: () =>
      import('../../components/library/return/return.module').then(
        (m) => m.LibraryReturnModule
      ),
  },
];

const components = [LibraryComponent, LibraryGridComponent];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class LibraryModule {}
