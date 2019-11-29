import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private alertify: AlertifyService, private userService: UserService,
              private routes: ActivatedRoute) { }

  ngOnInit() {
    this.routes.data.subscribe(data => {
      this.user = data.user;
    });
    this.galleryOptions = [
      {
          width: '500px',
          height: '500px',
          thumbnailsColumns: 4,
          imageAnimation: NgxGalleryAnimation.Slide,
          preview: false
      }
  ];

    this.galleryImages = this.getImages();
 }
 getImages() {
   const imageUrls = [];
   // tslint:disable-next-line:prefer-for-of
   for (let i = 0; i < this.user.photos.length; i++) {
       imageUrls.push({
         small: this.user.photos[i].url,
         medium: this.user.photos[i].url,
         big: this.user.photos[i].url,
         Description: this.user.photos[i].description
       });
   }
   return imageUrls;
 }
}

  // loadUser() {
  //      this.userService.getUser(+this.routes.snapshot.params.id).subscribe((user: User) => {
  //          this.user = user;
  //      }, error => {
  //        this.alertify.error(error);
  //      });
  // }

