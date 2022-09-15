import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';
import { Category } from '../category';
import { CategoryService } from '../category.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  //issues: Issue[] = [];
  // Dictionary with all issues by categoryId
  issues: { [categoryId: number]: Issue[] } = {};
  categories: Category[] = [];
  filteredCategories: Category[] = [];
  filteredIssues: { [categoryId: number]: Issue[] } = {};
  @Input() filterPriority: number = 2;
  @ViewChild('priorityFilterValue') filterPriorityValue!: ElementRef;

  constructor(
    private issueService: IssueService,
    private categoryService: CategoryService,
  ) { }

  ngOnInit(): void {
    // Get categories from server
    this.categoryService.getCategories().subscribe(result => {
      this.categories = result;
      console.log(this.categories);
      // Get issues from server
      this.issueService.getIssues().subscribe(result => {
        this.categories.forEach(category => {
          this.issues[category.id!] = result.filter(issue => issue.categoryId == category.id!);
        });
        this.filterIssues();
      });
    })
    
  }

  filterIssues(): void {
    // TODO: Add category filter
    // Set the filterPriority variable
    this.filterPriority = this.filterPriorityValue.nativeElement.value;
    // Clear filtered issues
    this.filteredIssues = {};
    // Filter each categoryId by priority and populate filteredIssues dictionary
    this.categories.forEach(category => {
      this.filteredIssues[category.id!] = this.issues[category.id!].filter(issue => issue.priority <= this.filterPriority);
    });
  }

}
