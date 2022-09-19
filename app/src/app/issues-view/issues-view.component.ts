import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Issue } from '../issue';
import { IssueService } from '../issue.service';

import { CurrentProject } from '../project';

@Component({
  selector: 'app-issues-view',
  templateUrl: './issues-view.component.html',
  styleUrls: ['./issues-view.component.css']
})
export class IssuesViewComponent implements OnInit {

  issues: Issue[] = [];
  editMode: boolean = false;

  constructor(
    private issueService: IssueService,
    private router: Router,
    private project:CurrentProject,
  ) { }

  ngOnInit(): void {
    this.issueService.getIssues(this.project.id).subscribe(issues => this.issues = issues);
  }

  deleteIssue(issue: Issue): void {
    if(issue.id){ // If the given id exists...
      // Delete the issue
      this.issueService.deleteIssue(issue.id).subscribe();
      // Remove the delete item from the list so that the UI reflects the delete
      this.issues = this.issues.filter(i => i !== issue);
    }
  }

  toggleEditMode(): void {
    this.editMode = !this.editMode;
  }
  
}
