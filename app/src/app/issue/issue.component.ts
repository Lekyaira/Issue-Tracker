import { Component, Input, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FormsModule } from '@angular/forms';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';
import { AppUser } from '../app-user';
import { CategoryService } from '../category.service';
import { Category } from '../category';
import { UserService } from '../user.service';
import { CurrentProject } from '../project';

// declare var RichTextEditor: any;

@Component({
  selector: 'app-issue',
  templateUrl: './issue.component.html',
  styleUrls: ['./issue.component.css']
})
export class IssueComponent implements OnInit {

  @Input() issue?: Issue;
  @Input() issueUser?: AppUser;
  htmlContent = '';
  categories: Category[] = [];
  @ViewChild("priority") priorityValue!: ElementRef;

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
    private userService: UserService,
    private location: Location,
    private project: CurrentProject,
  ) {}

  ngOnInit(): void {
    // Get the issue id from url
    const id = Number(this.route.snapshot.paramMap.get("id"));
    // If there was an id in the url, pull it from the service
    if(id){
      this.issueService.getIssue(id).subscribe(result => {
        this.issue = result.issue;
        this.issueUser = result.user;
      });
    } else {  // Otherwise, create a new issue
      // Get the logged in user's info
      this.userService.getCurrentUser().subscribe(result => {
        this.issueUser = result;

        // Create a new issue
        this.issue = {
          title: 'New Issue',
          priority: 1,
          creatorId: result.id,
          category: '',
          categoryId: 1,                //TODO: Get the first category in the project and use that
          projectId: this.project.id,
        }
      })
    }

    // Get all categories to populate select list
    this.categoryService.getCategories().subscribe(categories => this.categories = categories);
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if(this.issue){
      console.log(this.priorityValue);
      this.issue.priority = Number(this.priorityValue.nativeElement.value);
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
