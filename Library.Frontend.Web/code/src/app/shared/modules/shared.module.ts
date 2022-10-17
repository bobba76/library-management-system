import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ClarityModule } from '@clr/angular';

import { BackButtonComponent } from '@shared/components/back-button/back-button.component';
import { AutofocusDirective } from '@shared/directives/auto-focus.directive';

const modules = [
  CommonModule,
  ClarityModule,
  FormsModule,
  ReactiveFormsModule,
  HttpClientModule,
  RouterModule,
];

const components = [BackButtonComponent, AutofocusDirective];

@NgModule({
  declarations: [components],
  imports: [modules],
  exports: [components, modules],
})
export class SharedModule {}
