import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AngularEditorModule } from '@kolkov/angular-editor';
import { AuthModule } from '@auth0/auth0-angular';
import { AuthHttpInterceptor } from '@auth0/auth0-angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IssuesViewComponent } from './issues-view/issues-view.component';
import { IssueComponent } from './issue/issue.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { CategoriesBarComponent } from './categories-bar/categories-bar.component';
import { CategoriesViewComponent } from './categories-view/categories-view.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { environment as env } from '../environments/environment';
import { TestComponent } from './test/test.component';

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
    TestComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AngularEditorModule,
    FormsModule,

    // Import Auth0 module with configuration data
    AuthModule.forRoot({
      ...env.auth,
      // The AuthHttpInterceptor configuration
      httpInterceptor: {
        allowedList: [
          {
            uri: 'https://localhost:5001/api/*',
            tokenOptions: {
              audience: 'portfolio/IssueTracker',
            }
          },
        ]
      },
    }),
    
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
