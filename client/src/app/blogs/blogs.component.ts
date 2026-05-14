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
export class BlogsComponent implements OnInit {
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
  commentContentByBlog: Record<number, string> = {};
  blogCommentsOpen: boolean = false;
  isCreatingBlog = false;
  newBlogTitle = '';
  newBlogDescription = '';
  editingBlogId: number | null = null;
  editBlogTitle = '';
  editBlogDescription = '';

  ngOnInit(): void {
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.body || [];
      }, 
    })
  }

  isOwner(userId : number) : boolean {
    const currentUserId = this.accountService.currentUser()?.id;
    if(currentUserId === null || currentUserId === undefined) return false;
    return Number(userId) === Number(currentUserId);
  }

  isEditingBlog(blog: Blog): boolean {
    return this.editingBlogId === blog.id;
  }

  toggleCreateBlogForm(): void {
    this.isCreatingBlog = !this.isCreatingBlog;
    if(!this.isCreatingBlog){
      this.newBlogTitle = '';
      this.newBlogDescription = '';
    }
  }

  createBlog(): void {
    const title = this.newBlogTitle.trim();
    const description = this.newBlogDescription.trim();

    if(!title || !description){
      return;
    }

    const currentUserId = this.accountService.currentUser()?.id;
    if(currentUserId === null || currentUserId === undefined){
      return;
    }

    const payload: Blog = {
      id: 0,
      title,
      description,
      publishedAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false,
      userId: Number(currentUserId),
      blogLikes: [],
      blogComments: []
    };

    this.blogService.addBlog(payload).subscribe({
      next: () => {
        this.refreshBlogs();
        this.newBlogTitle = '';
        this.newBlogDescription = '';
        this.isCreatingBlog = false;
      }
    });
  }

  startEditBlog(blog: Blog): void {
    this.editingBlogId = blog.id;
    this.editBlogTitle = blog.title;
    this.editBlogDescription = blog.description;
  }

  cancelEditBlog(): void {
    this.editingBlogId = null;
    this.editBlogTitle = '';
    this.editBlogDescription = '';
  }

  saveBlogEdit(blog: Blog): void {
    const title = this.editBlogTitle.trim();
    const description = this.editBlogDescription.trim();

    if(!title || !description){
      return;
    }

    const payload: Blog = {
      ...blog,
      title,
      description,
      updatedAt: new Date()
    };

    this.blogService.updateBlogPost(payload).subscribe({
      next: () => {
        this.blogs = this.blogs.map(b => b.id === blog.id ? payload : b);
        this.cancelEditBlog();
      }
    });
  }

  deleteBlog(blog: Blog): void {

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

  deleteBlogComment(blog: Blog, comment: BlogComment): void {
    
  }

  saveComment(blog: Blog) {

  }

  private refreshBlogs(): void {
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.body || [];
      }
    });
  }

}
