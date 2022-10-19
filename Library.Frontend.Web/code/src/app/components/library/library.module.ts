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
    path: 'borrow',
    loadChildren: () =>
      import('../../components/library/borrow/borrow.module').then(
        (m) => m.LibraryBorrowModule
      ),
  },
];

const components = [LibraryComponent, LibraryGridComponent];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class LibraryModule {}
