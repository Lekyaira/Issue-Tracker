import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssueComponent } from './issue/issue.component';
import { IssuesViewComponent } from './issues-view/issues-view.component';

const routes: Routes = [
  {path: 'issues', component: IssuesViewComponent},
  {path: '', redirectTo: '/issues', pathMatch: 'full'},
  {path: 'issues/detail/:id', component: IssueComponent},
  {path: 'issues/detail', component: IssueComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
