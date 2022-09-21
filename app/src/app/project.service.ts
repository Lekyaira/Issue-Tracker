import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Project } from './project';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private projectsUrl = "https://localhost:5001/api/project"; //TODO: Add production URL

  constructor(
    private http: HttpClient
  ) { }

  getProject(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.projectsUrl}/${id}`);
  }

  getUserProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.projectsUrl);
  }

  deleteProject(id: number): Observable<any> {
    return this.http.delete(`${this.projectsUrl}/${id}`);
  }
}
