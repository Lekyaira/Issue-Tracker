import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IssuesViewComponent } from './issues-view/issues-view.component';
import { IssueComponent } from './issue/issue.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { CategoriesBarComponent } from './categories-bar/categories-bar.component';
import { CategoriesViewComponent } from './categories-view/categories-view.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    IssuesViewComponent,
    IssueComponent,
    TopBarComponent,
    CategoriesBarComponent,
    CategoriesViewComponent,
    CategoryDetailComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AngularEditorModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
