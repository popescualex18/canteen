import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from 'src/app/shared/services/http/http.service';
import { MenuDto } from '../models/menu-item.model';
import { DailyMenuDefinitionDto, DailyMenuDefinitionOverviewDto } from '../models/daily-menu-definition';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  baseEndpoint = 'Menu'; // Relative path to the GET endpoint
  postEndpoint = 'menu/create-menu';
  getDailyMenuDefinitionsEndpoint = "menudefinition";
  createMenuDefinitionEndpoint = 'menudefinition/define-daily-menu';
  screenshortUrl = 'Screenshot';

  constructor(private httpService: HttpService) {}
  getMenu(): Observable<any> {
    return this.httpService.get<any>(this.baseEndpoint);
  }

  updateMenu(data: any): Observable<any> {
    const endpoint = '/menu/update';
    return this.httpService.post<any>(endpoint, data);
  }

  getMenuById(id:string):Observable<MenuDto> {
    return this.httpService.get<any>(`${this.baseEndpoint}/${id}`);
  }

  createOrUpdateMenu(data:MenuDto){
    return this.httpService.post<any>(`${this.postEndpoint}`, data);
  }

  deleteMenu(id:string) {
    return this.httpService.delete(`${this.baseEndpoint}/${id}`);
  }

  postDailyMenu(data:DailyMenuDefinitionDto){
    return this.httpService.post<any>(`${this.createMenuDefinitionEndpoint}`, data);
  }

  getDailyMenuDefinitions(selectedDate: string) {
    return this.httpService.get<DailyMenuDefinitionDto[]>(`${this.getDailyMenuDefinitionsEndpoint}/${encodeURIComponent(selectedDate)}`);
  }

  getDailyMenuOverview(selectedDate:string) {
    return this.httpService.get<DailyMenuDefinitionOverviewDto[]>(`${this.getDailyMenuDefinitionsEndpoint}/daily-menu-overview/${encodeURIComponent(selectedDate)}`);
  }
  downloadScreenshot(): Observable<Blob> {
    return this.httpService.getImage(`${this.screenshortUrl}`);
  }
}
