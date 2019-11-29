import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

// const httpOptions = {
// headers: new HttpHeaders({
//   Authorization: 'Bearer ' + localStorage.getItem('token')
// })
// };
@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.apiUrl;
constructor(private httpClient: HttpClient) { }

getUsers(): Observable<User[]> {
 return this.httpClient.get<User[]>(this.baseUrl + 'users');
}

getUser(id): Observable<User> {
  return this.httpClient.get<User>(this.baseUrl + 'users/' + id);
}

updateProfile(id: number, user: User) {
return this.httpClient.put(this.baseUrl + 'users/' + id, user);
}
}
