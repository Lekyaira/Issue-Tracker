import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FormsModule } from '@angular/forms';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';
import { CategoryService } from '../category.service';
import { Category } from '../category';

// declare var RichTextEditor: any;

@Component({
  selector: 'app-issue',
  templateUrl: './issue.component.html',
  styleUrls: ['./issue.component.css']
})
export class IssueComponent implements OnInit {

  @Input() issue?: Issue;
  htmlContent = '';
  categories: Category[] = [];

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    minHeight: '5rem',
    placeholder: 'Enter text...',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    customClasses: [
      {
        name: 'Quote',
        class: 'quoteClass',
      },
      {
        name: 'Title Heading',
        class: 'titleHead',
        tag: 'h1',
      },
    ],
  };

  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private categoryService: CategoryService,
    private location: Location,
  ) {}

  ngOnInit(): void {
    // Get the issue id from url
    const id = Number(this.route.snapshot.paramMap.get("id"));
    // If there was an id in the url, pull it from the service
    if(id){
      this.issueService.getIssue(id).subscribe(issue => this.issue = issue);
    } else {  // Otherwise, create a new issue
      this.issue = {
        title: 'New Issue',
        priority: 1,
        creator: 'Ryan',    // TODO: Get creator info from current users
        creatorId: 1,
        category: '',
        categoryId: 1
      }
    }

    // Get all categories to populate select list
    this.categoryService.getCategories().subscribe(categories => this.categories = categories);
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if(this.issue){
      console.log(this.issue);
      if(this.issue.id){
        this.issueService.updateIssue(this.issue).subscribe();
      } else {
        this.issueService.createIssue(this.issue).subscribe();
      }
      this.location.back();
    }
  }

  setCategory(id: number | undefined, name: string, color: string): void {
    if(this.issue) {
      this.issue.categoryId = id ? id : 1;
      this.issue.category = name;
      this.issue.categoryColor = color;
    }
  }
}
