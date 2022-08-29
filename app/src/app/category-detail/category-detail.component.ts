import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { Category } from '../category';
import { CategoryService } from '../category.service';

@Component({
  selector: 'app-category-detail',
  templateUrl: './category-detail.component.html',
  styleUrls: ['./category-detail.component.css']
})
export class CategoryDetailComponent implements OnInit {

  @Input() category?: Category;

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private location: Location,
  ) { }

  ngOnInit(): void {
    // Get the issue id from url
    const id = Number(this.route.snapshot.paramMap.get("id"));
    // If there was an id in the url, pull it from the service
    if(id){
      this.categoryService.getCategory(id).subscribe(category => this.category = category);
    } else {  // Otherwise, create a new issue
      this.category = {
        name: 'New Category',
        color: '#ffffff',
      }
    }
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if(this.category){
      console.log(this.category);
      if(this.category.id){
        this.categoryService.updateCategory(this.category).subscribe();
      } else {
        this.categoryService.createCategory(this.category).subscribe();
      }
      this.location.back();
    }
  }


}
