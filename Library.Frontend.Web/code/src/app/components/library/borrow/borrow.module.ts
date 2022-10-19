import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { LibraryBorrowComponent } from '@components/library/borrow/borrow.component';
import { LibraryBorrowGridComponent } from '@components/library/borrow/grid/grid.component';

const routes: Routes = [
  {
    path: '',
    component: LibraryBorrowComponent,
  },
];

const components = [LibraryBorrowComponent, LibraryBorrowGridComponent];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class LibraryBorrowModule {}
