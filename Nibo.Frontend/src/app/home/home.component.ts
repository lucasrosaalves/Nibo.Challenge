import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { HomeService } from '../core/services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  fileList: FileList;
  constructor(private homeService: HomeService) { }

  ngOnInit() {
  }

  submit(form: NgForm) {
    if (form.invalid || !this.fileList || this.fileList.length === 0) {
      return;
    }
    this.homeService.uploadFiles(this.fileList).subscribe(
      data => {
        console.log('success');
        console.log(data);
      },
      error => console.log(error)
    );

  }

  fileChange(event) {
    if (event.target.files && event.target.files.length > 0) {
      this.fileList = event.target.files;
    }
  }
}
