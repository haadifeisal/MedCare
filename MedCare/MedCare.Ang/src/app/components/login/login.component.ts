import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor.service';
import { Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  doctorModel: any;
  patientModel: any;
  user: any;
  id: number;
  constructor(
    private authService: AuthService,
    private docService: DoctorService,
    private router: Router,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    this.loginForm = new FormGroup({
      type: new FormControl('doctor'),
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  login() {
    if (this.loginForm.valid) {
      if (this.loginForm.get('type').value == 'doctor') {
        this.doctorModel = this.loginForm.value;
        this.doctorLogin();
      }
      else {
        this.patientModel = this.loginForm.value;
        this.patientLogin();
      }
    } else {
      console.log('Form is not valid');
    }
  }
  doctorLogin() {
    this.authService.doctorLogin(this.doctorModel).subscribe(response => {
      this.flashM.show('You are logged in', {
        cssClass: 'alert-success', timeout: 3000
      });
      this.router.navigate(['/']);
    }, error => {
      this.flashM.show('Failed to login', {
        cssClass: 'alert-danger', timeout: 3000
      });
      this.router.navigate(['/login']);
    });
  }
  patientLogin() {
    this.authService.patientLogin(this.patientModel).subscribe(response => {
      this.flashM.show('You are logged in', {
        cssClass: 'alert-success', timeout: 3000
      });
      this.router.navigate(['/patient-records']);
    }, error => {
      this.flashM.show('Failed to login', {
        cssClass: 'alert-danger', timeout: 3000
      });
      this.router.navigate(['/login']);
    });
  }

}
