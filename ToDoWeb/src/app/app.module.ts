import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { ViewsModule } from './views/views.module';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    ViewsModule,
    SharedModule,
    BrowserModule,
    AppRoutingModule
  ],
  providers: [
    {
      provide:LocationStrategy,useClass:HashLocationStrategy
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
