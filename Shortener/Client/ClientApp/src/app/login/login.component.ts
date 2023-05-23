import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() { }

  login() {
    this.authService.login(this.model.username, this.model.password)
      .subscribe(
        data => {
          // Handle the login success event here
        },
        error => {
          // Handle the login error event here
        });
  }
}
