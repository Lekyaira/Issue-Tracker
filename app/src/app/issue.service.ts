import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Issue } from './issue';
import { ISSUES } from './mock-issues';

@Injectable({
  providedIn: 'root'
})
export class IssueService {
  private issuesUrl = "https://localhost:5001/api/issue"; //TODO: Add production URL

  constructor(
    private http: HttpClient,
  ) { }

  getIssues(): Observable<Issue[]> {
    return this.http.get<Issue[]>(this.issuesUrl);
  }

  getIssue(id: number): Observable<Issue> {
    const url = `${this.issuesUrl}/${id}`;
    return this.http.get<Issue>(url);
  }
}
