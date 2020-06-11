import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { apiRoutes } from '../constants/api-routes';
import { AccountData } from 'src/app/shared/models/account';

@Injectable()
export class AccountService {
    constructor(private http: HttpClient) {
    }

    uploadFiles(fileList: FileList) {
        let formData: FormData = new FormData();
        for (let index = 0; index < fileList.length; index++) {
            formData.append('files', fileList[index], fileList[index].name);
        }
        return this.http.post(apiRoutes.import, formData);
    }

    getAll(){
        return this.http.get<AccountData[]>(apiRoutes.getall);
    }

}
