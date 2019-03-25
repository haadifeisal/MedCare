import { Component, OnInit, Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { DoctorService } from '../services/doctor.service';
import { Router, CanActivate } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Observable } from 'rxjs/Observable';
import { Doctor } from '../models/Doctor';

@Injectable()

export class AdminGuard implements CanActivate {

  doctor: Doctor;

  constructor(
    private authService: AuthService,
    private docService: DoctorService,
    private router: Router,
    private flashM: FlashMessagesService
  ) { }

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authService.adminLoggedIn()) {
      return true;
    } else {
      return false;
    }
  }
  
}
