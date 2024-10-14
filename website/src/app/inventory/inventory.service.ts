import { Injectable } from '@angular/core';
import { AuthenticationService } from '../authentication/authentication.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Item } from './inventory-list/Item';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  constructor(
    private _authService: AuthenticationService,
    private _http: HttpClient
  ) { }

  find(limit = 10000, page = 1): Observable<any> {
    return this._http.get(`${environment.API_URL}/api/items?limit=${limit}&page=${page}`, {
      headers: {
        "Authorization": `Bearer ${this._authService.jwt.token}`
      }
    })
  }

  increment(item: any): Observable<any> {
    return this._increment(item)
  }
  decrement(item: any): Observable<any> {
    return this._increment(item, -1)
  }
  remove(item: any): Observable<any> {
    return this._http.delete<any>(`${environment.API_URL}/api/items/${item.id}`, {
      headers: {
        "Authorization": `Bearer ${this._authService.jwt.token}`
      }
    })
  }

  create(item: Item): Observable<any> {
    return this._http.post<any>(`${environment.API_URL}/api/items`, item, {
      headers: {
        "Authorization": `Bearer ${this._authService.jwt.token}`
      }
    })
  }

  private _increment(item: any, value = 1): Observable<any> {
    item.amount += value

    return this._http.patch<any>(`${environment.API_URL}/api/items/${item.id}`, item, {
      headers: {
        "Authorization": `Bearer ${this._authService.jwt.token}`
      }
    })
  }
}
