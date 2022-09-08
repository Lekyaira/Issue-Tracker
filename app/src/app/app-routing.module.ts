import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssueComponent } from './issue/issue.component';
import { IssuesViewComponent } from './issues-view/issues-view.component';
import { CategoriesViewComponent } from './categories-view/categories-view.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { AuthGuard } from '@auth0/auth0-angular';

const routes: Routes = [
  {path: 'issues', component: IssuesViewComponent, canActivate: [AuthGuard]},
  {path: '', redirectTo: '/dashboard', pathMatch: 'full'},
  {path: 'issues/detail/:id', component: IssueComponent},
  {path: 'issues/detail', component: IssueComponent},
  {path: 'categories', component: CategoriesViewComponent},
  {path: 'categories/detail/:id', component: CategoryDetailComponent},
  {path: 'categories/detail', component: CategoryDetailComponent},
  {path: 'dashboard', component: DashboardComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
