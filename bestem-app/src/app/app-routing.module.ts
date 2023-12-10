import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component'; 
import { DashboardComponent } from './dashboard/dashboard.component';
import { MicrosoftLoginComponent } from './microsoft-login/microsoft-login.component';

const routes: Routes = [
  {path:'', component:LoginComponent},
  {path:'dashboard', component: DashboardComponent},
  { path: 'microsoft-login', component: MicrosoftLoginComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
