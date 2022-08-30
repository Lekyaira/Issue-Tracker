import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  issues: Issue[] = [];
  filteredIssues: Issue[] = [];
  @Input() filterPriority: number = 2;
  @ViewChild('priorityFilterValue') filterPriorityValue!: ElementRef;

  constructor(
    private issueService: IssueService
  ) { }

  ngOnInit(): void {
    this.issueService.getIssues().subscribe(issues => {
      this.issues = issues;
      this.filterIssues();
    });
  }

  filterIssues(): void {
    this.filterPriority = this.filterPriorityValue.nativeElement.value;
    this.filteredIssues = this.issues.filter(issue => issue.priority <= this.filterPriority);
    console.log(this.filterPriority);
  }

}
