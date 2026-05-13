import { Component, inject, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Blog } from '../_models/blog';

@Component({
  selector: 'app-blogs',
  imports: [],
  templateUrl: './blogs.component.html',
  styleUrl: './blogs.component.css'
})
export class BlogsComponent implements OnInit {
  private blogService = inject(BlogService);
  pageNumber : number = 1;
  pageSize : number = 5;
  blogs: Blog[] = [];

  ngOnInit(): void {
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.data;
      }, 
    })
  }

}
