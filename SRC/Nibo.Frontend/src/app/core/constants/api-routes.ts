import { environment } from '../../../environments/environment';

export const apiRoutes =  {
    import : environment.apiUrl + "api/account/import",
    getall: environment.apiUrl + "api/account/getall"
}