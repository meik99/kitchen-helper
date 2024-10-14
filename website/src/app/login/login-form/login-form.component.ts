import { Component, OnInit } from '@angular/core';
import { User } from './User';
import { FormsModule } from '@angular/forms';
import { AuthenticationService } from '../../authentication/authentication.service';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss'
})
export class LoginFormComponent implements OnInit {
  user: User = new User()

  constructor(
    private _authServer: AuthenticationService
  ) {}

  ngOnInit(): void {
    if (localStorage.getItem("token") !== null) {
      try {
        const localJWT = localStorage.getItem("token")

        if (localJWT != null) {
          this._authServer.jwt = JSON.parse(localJWT)
        }        
      } catch(e) {
        localStorage.removeItem("token")
      }
    }

    this.user.endpoint = this._authServer.endpoint
  }

  login() {
    this._authServer.login(this.user).subscribe(
      result => 
        this._authServer.jwt = result      
    )
  }
}
