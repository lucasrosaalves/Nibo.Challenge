import { NgModule } from '@angular/core';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [AccountRoutingModule, SharedModule],
  declarations: [AccountRoutingModule.components]
})
export class AccountModule { }

