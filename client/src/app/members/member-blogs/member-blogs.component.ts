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
export class MemberBlogsComponent {
  userId: number = 0;

  private blogService = inject(BlogService);
  private route = inject(ActivatedRoute);

  blogs: Blog[] = [];
  pageNumber = 1;
  pageSize = 4;

}
