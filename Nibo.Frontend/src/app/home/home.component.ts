import { Component } from '@angular/core';
import { RouterService } from '../core/services/router.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  fileList: FileList;
  constructor(private routerService: RouterService) { }

  goToUpload(){
    this.routerService.goToUpload();
  }

  goToAccount(){
    this.routerService.goToAccount();
  }
}
