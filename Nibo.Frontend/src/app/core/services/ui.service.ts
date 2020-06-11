import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class UiService {

    private loading =  new BehaviorSubject<boolean>(false);
    public readonly loading$ = this.loading.asObservable();

    changeLoading(loading: boolean) {
        this.loading.next(loading);
    }
}
