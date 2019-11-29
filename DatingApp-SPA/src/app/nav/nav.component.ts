import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Routes, Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertifyservice: AlertifyService,
              private router: Router) { }

  ngOnInit() {
  }

 login() {

     this.authService.login(this.model).subscribe(next => {
        this.alertifyservice.success('logged in successfully');
        // console.log('logged in successfully');
     },
       error => {
       // console.log(error);
        this.alertifyservice.error(error);
       },
       () => {
         this.router.navigate(['/members']);
        }
      );
  }

  loggedIn() {
   return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertifyservice.message('Logged out');
    this.router.navigate(['/home']);
  }

  register() {
    return this.authService.register(this.model).subscribe(next => {
      this.alertifyservice.message('You registration was successful');
    },
  error => {
    this.alertifyservice.error(error);
  });
  }
}
