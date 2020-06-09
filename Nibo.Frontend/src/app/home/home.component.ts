import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  arquivos;
  fileList: FileList;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.arquivos = [];
  }

  submit(form: NgForm) {
    if (form.invalid) { return; }

    let formData: FormData = new FormData();

    let q = this.fileList.length;

    let files: File[] = [];

    for (let index = 0; index < q; index++) {
      formData.append('files', this.fileList[index], this.fileList[index].name);
    }

    this.http.post('https://localhost:44307/weatherforecast', formData)
      .subscribe(
        data => {
          console.log('success');
          console.log(data);
        },
        error => console.log(error)
      )
  }

  fileChange(event) {
    this.arquivos = [];
    let fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      this.fileList = fileList;

      let quantidade = fileList.length;

      for (let index = 0; index < quantidade; index++) {
        const reader = new FileReader();
        reader.readAsDataURL(fileList[index]);
        reader.onload = () => {
          this.arquivos.push(reader.result);
        };
      }
    }
  }
}
