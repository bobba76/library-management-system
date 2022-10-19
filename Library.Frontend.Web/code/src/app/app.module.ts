import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MenuBarComponent } from '@components/menu-bar/menu-bar.component';
import { AppComponent } from './app.component';

import { routerModule as AppRoutingModule } from '@modules/imports/app-routing.module';
import { storeModules as AppStoreModule } from '@modules/imports/app-store.module';
import { providers as AppProviders } from '@modules/providers/app-providers.module';
import { SharedModule } from '@shared/modules/shared.module';

@NgModule({
  declarations: [AppComponent, MenuBarComponent],

  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    AppRoutingModule,
    AppStoreModule,
  ],
  providers: [AppProviders],
  bootstrap: [AppComponent],
})
export class AppModule {}
