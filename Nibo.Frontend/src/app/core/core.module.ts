import { NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AccountService } from './services/account.service';
import { SharedModule } from '../shared/shared.module';
import { UiService } from './services/ui.service';
import { LoadingComponent } from './loading/loading.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterService } from './services/router.service';


@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        RouterModule,
        HttpClientModule
    ],
    exports: [
        RouterModule,
        HttpClientModule,
        LoadingComponent,
        NavbarComponent,
        FooterComponent
    ],
    declarations: [
        LoadingComponent,
        NavbarComponent,
        FooterComponent
    ],
    providers: [
        AccountService,
        UiService,
        RouterService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    ]
})
export class CoreModule {

}
