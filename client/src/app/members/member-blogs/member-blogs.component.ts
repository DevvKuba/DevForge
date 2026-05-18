import { Component, Input, OnInit, inject } from '@angular/core';
import { BlogService } from '../../_services/blog.service';
import { Blog } from '../../_models/blog';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';
import { ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-member-blogs',
  imports: [
    CardModule,
    CommonModule,
  ],
  templateUrl: './member-blogs.component.html',
  styleUrl: './member-blogs.component.css'
})
export class MemberBlogsComponent implements OnInit {
  userId: number = 0;

  private blogService = inject(BlogService);
  private route = inject(ActivatedRoute);

  blogs: Blog[] = [];
  pageNumber = 1;
  pageSize = 4;

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        this.userId = Number.parseInt(params.get('id') || '0');
        this.loadMemberBlogs();
      }
    })
  }

  loadMemberBlogs(): void {
    if(this.userId == null) return;

    this.blogService.gatherSpecificUserBlogs(this.userId, this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.body;
      }
    });
  }
}
