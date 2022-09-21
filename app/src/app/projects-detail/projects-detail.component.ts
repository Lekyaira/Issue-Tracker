import { Component, OnInit } from '@angular/core';
import { AppUser } from '../app-user';
import { CurrentProject } from '../project';
import { ProjectService } from '../project.service';
import { UserService } from '../user.service';

@Component({
  selector: 'app-projects-detail',
  templateUrl: './projects-detail.component.html',
  styleUrls: ['./projects-detail.component.css']
})
export class ProjectsDetailComponent implements OnInit {

  users: {selected: boolean, appUser: AppUser}[] = [];

  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    public project: CurrentProject,
  ) { }

  ngOnInit(): void {
    this.userService.getUsersByProject(this.project.id).subscribe(users => {
      users.forEach(user => {
        this.users.push({selected: false, appUser: user});
      })
    });
  }

}
