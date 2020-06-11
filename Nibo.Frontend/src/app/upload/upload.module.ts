import { NgModule } from '@angular/core';
import { UploadRoutingModule } from './upload-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [UploadRoutingModule, SharedModule],
  declarations: [UploadRoutingModule.components]
})
export class UploadModule { }
