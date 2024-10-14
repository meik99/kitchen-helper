import { Component } from '@angular/core';
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
export class LoginFormComponent {
  user: User = new User()

  constructor(
    private _authServer: AuthenticationService
  ) {}

  login() {
    this._authServer.login(this.user).subscribe(
      result => 
        this._authServer.jwt = result      
    )
  }
}
