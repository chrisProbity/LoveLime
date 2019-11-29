import { Component, OnInit, EventEmitter } from '@angular/core';
import { Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};
  @Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
  register() {
    this.authService.register(this.model).subscribe(() => {
      // console.log('registration succeeded');
      this.alertify.success('registeration successful');
    },
      error => {
      this.alertify.error(error);
      }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);

  }

}
