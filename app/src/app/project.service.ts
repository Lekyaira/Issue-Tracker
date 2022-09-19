import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Project, CurrentProject } from './project';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private projectsUrl = "https://localhost:5001/api/project"; //TODO: Add production URL

  constructor(
    private current: CurrentProject,
    private http: HttpClient
  ) { }

  getProject(): Observable<Project> {
    return this.http.get<Project>(`${this.projectsUrl}/${this.current.id}`);
  }

  getUserProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.projectsUrl);
  }
}
