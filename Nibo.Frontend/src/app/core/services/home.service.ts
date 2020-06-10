import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class HomeService {

    constructor(private http: HttpClient) {

    }

    uploadFiles(fileList: FileList) {
        let formData: FormData = new FormData();
        for (let index = 0; index < fileList.length; index++) {
            formData.append('files', fileList[index], fileList[index].name);
        }

        return this.http.post('https://localhost:44307/api/Account/import', formData);
    }
}
