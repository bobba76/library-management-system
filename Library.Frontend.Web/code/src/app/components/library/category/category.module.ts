import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '@shared/modules/shared.module';

import { CategoryComponent } from '@components/library/category/category.component';
import { CategoryConfigurationComponent } from '@components/library/category/configuration/configuration.component';
import { ConfigurationReferenceGridComponent } from '@components/library/category/configuration/reference-grid/reference-grid.component';
import { CategoryGridComponent } from '@components/library/category/grid/grid.component';

const routes: Routes = [
  {
    path: '',
    component: CategoryComponent,
  },
  {
    path: ':id',
    component: CategoryConfigurationComponent,
  },
];

const components = [
  CategoryComponent,
  CategoryConfigurationComponent,
  CategoryGridComponent,
  ConfigurationReferenceGridComponent,
];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class CategoryModule {}
