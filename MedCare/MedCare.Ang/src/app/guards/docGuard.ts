import { Component, OnInit, Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, CanActivate } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Observable } from 'rxjs/Observable';

@Injectable()

export class DocGuard implements CanActivate {
 

  constructor(
    private authService: AuthService,
    private router: Router,
    private flashM: FlashMessagesService
  ) { }

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authService.doctorLoggedIn()) {
      return true;
    } else {
      return false;
    }
  }

}
