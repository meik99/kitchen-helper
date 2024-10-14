import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthenticationService } from './authentication/authentication.service';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { LoginComponent } from "./login/login.component";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule, RouterModule, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  $isAuthenticated: Observable<boolean>

  constructor(
    private _authService: AuthenticationService,
    private _router: Router
  ) {
    this.$isAuthenticated = of(false)
  }

  ngOnInit(): void {
    this.$isAuthenticated = this._authService.isAuthenticated
    this.$isAuthenticated.subscribe(value => {
      if (value === true) {
        this._router.navigate(["inventory"])
      }
    })
  }
}
