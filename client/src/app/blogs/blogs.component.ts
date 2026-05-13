import { Component, effect, inject, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Blog } from '../_models/blog';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Member } from '../_models/member';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MembersService } from '../_services/members.service';

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
  private memberService = inject(MembersService);
  pageNumber : number = 1;
  pageSize : number = 5;
  blogs: Blog[] = [];
  members: Member[] = [];
  filteredMembers: Member[] = [];
  selectedMember: Member | null = null;

  private memberResultsSync = effect(() => {
    const paginatedMembers = this.memberService.paginatedResult();

    if (paginatedMembers?.items) {
      this.members = paginatedMembers.items;
      this.filteredMembers = paginatedMembers.items;
    }
  });

  ngOnInit(): void {
    this.memberService.getMembers();
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.data;
      }, 
    })
  }

  searchMembers(event: any): void {
    const query = (event.query ?? '').toLowerCase().trim();

    this.filteredMembers = !query
      ? this.members
      : this.members.filter(member =>
          member.username.toLowerCase().includes(query) ||
          member.specialization.toLowerCase().includes(query)
        );
  }

}
