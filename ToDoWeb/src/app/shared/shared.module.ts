import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { AppRoutingModule } from '../app-routing.module';
import { ModalComponent } from './modal/modal.component';
import { SwitchComponent } from './switch/switch.component';
import { LoaderComponent } from './loader/loader.component';



@NgModule({
  declarations: [
    NavbarComponent,
    ModalComponent,
    SwitchComponent,
    LoaderComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule
  ],
  exports: [
    NavbarComponent,
    ModalComponent,
    SwitchComponent,
    LoaderComponent
  ]
})
export class SharedModule { }
