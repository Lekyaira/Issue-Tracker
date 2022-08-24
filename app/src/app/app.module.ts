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

@NgModule({
  declarations: [
    AppComponent,
    IssuesViewComponent,
    IssueComponent,
    TopBarComponent,
    CategoriesBarComponent
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
