import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FormsModule } from '@angular/forms';

import { IssueService } from '../issue.service';
import { Issue } from '../issue';

// declare var RichTextEditor: any;

@Component({
  selector: 'app-issue',
  templateUrl: './issue.component.html',
  styleUrls: ['./issue.component.css']
})
export class IssueComponent implements OnInit {

  @Input() issue?: Issue;
  htmlContent = '';

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
    private location: Location,
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get("id"));
    console.log(id);
    if(id){
      this.issueService.getIssue(id).subscribe(issue => this.issue = issue);
    } else {
      this.issue = {
        title: 'New Issue',
        priority: 1,
        creator: 'Ryan',    // TODO: Get creator info from current users
        creatorId: 1
      }
    }
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if(this.issue){
      if(this.issue.id){
        this.issueService.updateIssue(this.issue).subscribe();
      } else {
        this.issueService.createIssue(this.issue).subscribe();
      }
      this.location.back();
    }
  }
}
