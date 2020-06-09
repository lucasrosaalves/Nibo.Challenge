import { NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HomeService } from './services/home.service';
import { SharedModule } from '../shared/shared.module';
import { UiService } from './services/ui.service';
import { LoadingComponent } from './loading/loading.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';


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
        HomeService,
        UiService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    ]
})
export class CoreModule {

}
