import { NgModule } from '@angular/core';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { DefaultComponent } from './default/default.component';
import { RouterModule } from '@angular/router';
import { TerminalsComponent } from '../terminals/terminals.component';
import { SharedModule } from '../shared/shared.module';
import { MatSidenavModule } from '@angular/material/sidenav'; 
import { MatDividerModule } from '@angular/material';

@NgModule({
  declarations: [
    DefaultComponent,
    DashboardComponent,
    TerminalsComponent
  ],
  imports: [
    RouterModule,
    SharedModule,
    MatSidenavModule,
    MatDividerModule
  ]

})
export class DefaultModule {
}
