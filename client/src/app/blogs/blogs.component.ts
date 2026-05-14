import { Component, effect, inject, OnDestroy, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Blog } from '../_models/blog';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Member } from '../_models/member';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MembersService } from '../_services/members.service';
import { AccountService } from '../_services/account.service';
import { BlogComment } from '../_models/blogComment';

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
export class BlogsComponent implements OnInit, OnDestroy {
  private blogService = inject(BlogService);
  private memberService = inject(MembersService);
  private accountService = inject(AccountService);

  pageNumber : number = 1;
  pageSize : number = 5;
  blogs: Blog[] = [];
  members: Member[] = [];
  filteredMembers: Member[] = [];
  openBlogComments: BlogComment[] = [];
  openCommentsBlogId: number | null = null;
  selectedMember: Member | null = null;
  commentContent = '';
  context = '';
  blogCommentsOpen: boolean = false;

  ngOnInit(): void {
    this.memberService.getMembers();
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.body;
      }, 
    })
  }

  ngOnDestroy(): void {
    this.memberService.paginatedResult.set(null);
  }

  private memberResultsSync = effect(() => {
    const paginatedMembers = this.memberService.paginatedResult();
    console.log('Effect fired, paginatedMembers:', paginatedMembers);

    if (paginatedMembers?.items) {
      this.members = paginatedMembers.items;
      this.filteredMembers = paginatedMembers.items;
    }
  });

  searchMembers(event: any): void {
     console.log('Members array:', this.members);
    const query = (event.query ?? '').toLowerCase().trim();

    this.filteredMembers = !query
      ? this.members
      : this.members.filter(member =>
          member.username.toLowerCase().includes(query) ||
          member.specialization.toLowerCase().includes(query)
        );
  }

  isOwner(userId : number) : boolean {
    if(userId == this.accountService.currentUser()?.id){
      return true;
    }
    return false;
  }

  isCommentsSectionOpen(blog: Blog) : boolean {
    return this.openCommentsBlogId == blog.id;
  }

  toggleComments(blog: Blog){
    if(this.openCommentsBlogId == blog.id){
      this.openCommentsBlogId = null;
      this.openBlogComments = [];
      return;
    }
    this.openBlogComments = blog.blogComments;
    this.openCommentsBlogId = blog.id;
    this.blogCommentsOpen = true;

  }

  addBlogComment(blogId: number, content: string) {
    
  }

  saveComment(blog: Blog) {

  }

}
