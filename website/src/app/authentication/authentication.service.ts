import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, map, of } from 'rxjs';
import { User } from '../login/login-form/User';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private _jwt: any | null = null
  private $isAuthenticated = new Subject<boolean>()

  constructor(
    private _http: HttpClient
  ) { 
    this.isAuthenticated = false

   
  }

  get isAuthenticated(): Observable<boolean> {
    return this.$isAuthenticated
  }

  set isAuthenticated(authenticated: boolean) {
    this.$isAuthenticated.next(authenticated)
  }

  login(user: User): Observable<any> {    
    return this._http.post<any>(`${environment.API_URL}/api/users/login`, user)
  }

  set jwt(value: any) {
    this._jwt = value
    this.isAuthenticated = this._jwt !== null    
    localStorage.setItem("token", JSON.stringify(this.jwt))
  }

  get jwt(): any {
    return this._jwt
  }
}
