import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';

@Component({
  selector: 'app-issue',
  templateUrl: './issue.component.html',
  styleUrls: ['./issue.component.css']
})
export class IssueComponent implements OnInit {

  @Input() issue?: Issue;

  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private location: Location,
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get("id"));
    this.issueService.getIssue(id).subscribe(issue => this.issue = issue);
  }

}
