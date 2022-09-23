import { Component, Input, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AppUser } from '../app-user';
import { CurrentProject, Project } from '../project';
import { ProjectService } from '../project.service';
import { UserService } from '../user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-projects-detail',
  templateUrl: './projects-detail.component.html',
  styleUrls: ['./projects-detail.component.css']
})
export class ProjectsDetailComponent implements OnInit {

  users: AppUser[] = [];
  @Input() project?: Project;
  @Input() owner?: AppUser;
  addUserEmail: string = "";
  addUserList?: AppUser[];
  isAddUserModalVisible: boolean = false;

  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    private location: Location,
    private route: ActivatedRoute,
    private currentProject: CurrentProject,
  ) { }

  ngOnInit(): void {
    // Get the issue id from url
    const id = Number(this.route.snapshot.paramMap.get("id"));
    // If there was an id in the url, pull it from the service
    if(id){
      this.projectService.getProject(id).subscribe(result => {
        this.project = result;
        this.userService.getUsersByProject(id).subscribe(users => {
          users.forEach(user => {
            this.users.push(user);
          });
        });
      });
    } else {  // Otherwise, create a new project
      // Get the logged in user's info
      this.userService.getCurrentUser().subscribe(result => {
        this.owner = result;

        // Create a new issue
        this.project = {
          name: 'New Project',
          owner: this.owner.id,
          users: []
        }
      })
    }
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if(this.project){
      // Add all the users to the project
      this.project.users = [];
      this.users.forEach(user => {
        this.project!.users.push(user.id);
      });
      // If we have an id, update the project
      if(this.project.id){
        this.projectService.updateProject(this.project).subscribe();
      }
      else {  // Otherwise, create a new one
        this.projectService.createProject(this.project).subscribe(_ => {
          // Update the topbar's dropdown list
          this.projectService.getUserProjects().subscribe(result => {
            if(this.currentProject.topBar){
              this.currentProject.topBar.projects = result;
            }
          })
        });
      }
    }
    this.location.back();
  }

  showAddUserModal(){
    this.userService.getUsersByEmail(this.addUserEmail).subscribe(result => {
      this.addUserList = result;
    });
    this.addUserEmail = "";
    this.isAddUserModalVisible = true;
  }

  closeModal(){
    this.isAddUserModalVisible = false;
    this.addUserList = [];
  }

  addUser(user: AppUser){
    this.users.push(user);
    this.closeModal();
    // this.userService.getUsersByEmail(this.addUserEmail).subscribe(result => {
    //   this.users.push(result[0]);                                       // Just pulling the first one here
    //                                                                     // TODO: Need to make a way to choose out of all results
    // });
    // this.addUserEmail = "";
  }

  deleteUser(user: AppUser){
    // Remove the user from the users list
    this.users = this.users.filter(i => i !== user);
  }

}
