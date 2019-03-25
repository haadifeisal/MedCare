import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { DoctorService } from './services/doctor.service';
import { JwtHelper } from 'angular2-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'app';

  constructor(
    private authService: AuthService,
    private docService: DoctorService
  ) { }

  jwtHelper: JwtHelper = new JwtHelper();
  ngOnInit() {
    const token = localStorage.getItem('userToken');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      this.authService.id = parseInt(this.authService.decodedToken.nameid);
      this.authService.username = this.authService.decodedToken.unique_name;
    }
  }

}

