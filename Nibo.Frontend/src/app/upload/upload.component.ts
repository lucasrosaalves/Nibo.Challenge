import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../core/services/account.service';
import { RouterService } from '../core/services/router.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  hasNotifications: boolean;
  fileList: FileList;

  constructor(
    private accountService: AccountService,
    private routerService: RouterService) { }

  ngOnInit() {
    this.hasNotifications = false;
  }

  submit(form: NgForm) {
    if (form.invalid || !this.fileList || this.fileList.length === 0) {
      return;
    }
    this.accountService.uploadFiles(this.fileList).subscribe(
      data => {
        this.routerService.goToAccount();
      },
      error => {
        this.showAlert();
      }
    );

  }

  fileChange(event) {
    if (event.target.files && event.target.files.length > 0) {
      this.fileList = event.target.files;
    }
  }

  showAlert() {
    this.hasNotifications = true;
  }

  closeAlert() {
    this.hasNotifications = false;
  }
}
