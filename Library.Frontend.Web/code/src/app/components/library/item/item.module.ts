import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { LibraryItemConfigurationComponent } from '@components/library/item/configuration/configuration.component';
import { LibraryItemGridComponent } from '@components/library/item/grid/grid.component';
import { LibraryItemComponent } from '@components/library/item/item.component';

const routes: Routes = [
  {
    path: '',
    component: LibraryItemComponent,
  },
  {
    path: ':id',
    component: LibraryItemConfigurationComponent,
  },
];

const components = [
  LibraryItemComponent,
  LibraryItemConfigurationComponent,
  LibraryItemGridComponent,
];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class LibraryItemModule {}
