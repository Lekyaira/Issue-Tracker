import { Component, Input, OnInit } from '@angular/core';

import { IssueService } from '../issue.service';
import { UserService } from '../user.service';
import { Issue } from '../issue';
import { AppUser } from '../app-user';

import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  @Input() issue?: Issue;
  @Input() user?: AppUser;

  constructor(
    private issueService: IssueService,
    private userService: UserService,
    public auth: AuthService,
  ) { }

  ngOnInit(): void {
    // this.issueService.getIssue(1).subscribe(result => {
    //   this.issue = result.issue;
    //   this.user = result.user;
    // })
    
    this.userService.getCurrentUser().subscribe(result => {
      this.user = result;
    })
  }

}
