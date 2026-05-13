import { Component, inject, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Blog } from '../_models/blog';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Member } from '../_models/member';
import { AutoCompleteModule } from 'primeng/autocomplete';

@Component({
  selector: 'app-blogs',
  imports: [
    CardModule, 
    ButtonModule, 
    AutoCompleteModule, 
    CommonModule, 
    FormsModule
  ],
  templateUrl: './blogs.component.html',
  styleUrl: './blogs.component.css'
})
export class BlogsComponent implements OnInit {
  private blogService = inject(BlogService);
  pageNumber : number = 1;
  pageSize : number = 5;
  blogs: Blog[] = [];
  members: Member[] = [];

  ngOnInit(): void {
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.data;
      }, 
    })
  }

}
