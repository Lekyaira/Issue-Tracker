import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AngularEditorModule } from '@kolkov/angular-editor';
import { AuthModule } from '@auth0/auth0-angular';

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
    DashboardComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AngularEditorModule,
    FormsModule,

    // Import Auth0 module with configuration data
    AuthModule.forRoot({
      domain: 'dev-7gr-w4iu.us.auth0.com',
      clientId: 'C3oBVzWpIfGUDgetkjQwh4G1XsgNdD6W',
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
