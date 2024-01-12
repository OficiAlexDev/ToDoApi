import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './views/auth/auth.component';
import { HomeComponent } from './views/home/home.component';
import { ErrorComponent } from './views/error/error.component';

const routes: Routes = [
  {path:"",component:HomeComponent},
  {path:"auth",component:AuthComponent},
  {path:"404",component:ErrorComponent},
  {path:"home",redirectTo:""},
  {path:"**",redirectTo:"404"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
