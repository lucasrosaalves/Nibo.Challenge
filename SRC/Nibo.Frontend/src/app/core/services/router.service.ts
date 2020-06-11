import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { routes } from '../constants/routes';

@Injectable()
export class RouterService {
    constructor(private router: Router) {
    }

    goToHome() {
        this.navigate(routes.home);
    }

    goToUpload() {
        this.navigate(routes.upload);
    }

    goToAccount() {
        this.navigate(routes.account);
    }

    private navigate(route: string, extras?: NavigationExtras) {
        this.router.navigate([route], extras);
    }
}
