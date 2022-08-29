import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssueComponent } from './issue/issue.component';
import { IssuesViewComponent } from './issues-view/issues-view.component';
import { CategoriesViewComponent } from './categories-view/categories-view.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';

const routes: Routes = [
  {path: 'issues', component: IssuesViewComponent},
  {path: '', redirectTo: '/issues', pathMatch: 'full'},
  {path: 'issues/detail/:id', component: IssueComponent},
  {path: 'issues/detail', component: IssueComponent},
  {path: 'categories', component: CategoriesViewComponent},
  {path: 'categories/detail/:id', component: CategoryDetailComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
