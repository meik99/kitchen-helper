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
  private _endpoint: string = environment.API_URL
  private $isAuthenticated = new Subject<boolean>()

  constructor(
    private _http: HttpClient
  ) { 
    this.isAuthenticated = false
    this.endpoint = environment.API_URL

    if (!this.endpoint || this.endpoint.length <= 0) {
      this.endpoint = window.location.origin
    }
  }

  get isAuthenticated(): Observable<boolean> {
    return this.$isAuthenticated
  }

  set isAuthenticated(authenticated: boolean) {
    this.$isAuthenticated.next(authenticated)
  }

  get endpoint(): string {
    return this._endpoint
  }

  set endpoint(value: string) {
    this._endpoint = value
  }

  login(user: User): Observable<any> {
    this.endpoint = user.endpoint
    return this._http.post<any>(`${this.endpoint}/api/users/login`, user)
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
