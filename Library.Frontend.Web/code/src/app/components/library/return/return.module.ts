import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { LibraryReturnGridComponent } from '@components/library/return/grid/grid.component';
import { LibraryReturnComponent } from '@components/library/return/return.component';

const routes: Routes = [
  {
    path: '',
    component: LibraryReturnComponent,
  },
];

const components = [LibraryReturnComponent, LibraryReturnGridComponent];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class LibraryReturnModule {}
