import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from '@components/home/home.component';
import { SharedModule } from '@shared/modules/shared.module';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
];

const components = [HomeComponent];

@NgModule({
  declarations: [components],
  imports: [RouterModule.forChild(routes), SharedModule],
})
export class HomeModule {}
