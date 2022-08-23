import { Component, OnInit } from '@angular/core';

import { Issue } from '../issue';
import { IssueService } from '../issue.service';

@Component({
  selector: 'app-issues-view',
  templateUrl: './issues-view.component.html',
  styleUrls: ['./issues-view.component.css']
})
export class IssuesViewComponent implements OnInit {

  issues: Issue[] = [];

  constructor(
    private issueService: IssueService,
  ) { }

  ngOnInit(): void {
    this.issueService.getIssues().subscribe(issues => this.issues = issues);
  }

}
