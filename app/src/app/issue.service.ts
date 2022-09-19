import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Issue } from './issue';
import { IssueAppUser } from './issue-app-user';

@Injectable({
  providedIn: 'root'
})
export class IssueService {
  private issuesUrl = "https://localhost:5001/api/issue"; //TODO: Add production URL

  constructor(
    private http: HttpClient,
  ) { }

  getIssues(project: number): Observable<Issue[]> {
    return this.http.get<Issue[]>(`${this.issuesUrl}/project/${project}`);
  }

  getIssue(id: number): Observable<IssueAppUser> {
    const url = `${this.issuesUrl}/${id}`;
    return this.http.get<IssueAppUser>(url);
  }

  updateIssue(issue: Issue): Observable<any> {
    return this.http.put(this.issuesUrl, issue);
  }

  createIssue(issue: Issue): Observable<any> {
    return this.http.post(this.issuesUrl, issue);
  }

  deleteIssue(id: number): Observable<any> {
    const url = `${this.issuesUrl}/${id}`;
    return this.http.delete(url);
  }
}
