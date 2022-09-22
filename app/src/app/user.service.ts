import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { AppUser } from './app-user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private issuesUrl = "https://localhost:5001/api/user"; //TODO: Add production URL

  constructor(
    private http: HttpClient
  ) { }

  getCurrentUser(): Observable<AppUser> {
    return this.http.get<AppUser>(this.issuesUrl);
  }

  getUsersByProject(id: number): Observable<AppUser[]> {
    return this.http.get<AppUser[]>(`${this.issuesUrl}/project/${id}`);
  }

  getUsersByEmail(email: string): Observable<AppUser[]> {
    return this.http.get<AppUser[]>(`${this.issuesUrl}/email/${email}`);
  }
}
