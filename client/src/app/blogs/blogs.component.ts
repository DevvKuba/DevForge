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
  private accountService = inject(AccountService);

  pageNumber : number = 1;
  pageSize : number = 8;
  blogs: Blog[] = [];
  members: Member[] = [];
  filteredMembers: Member[] = [];
  openBlogComments: BlogComment[] = [];
  openCommentsBlogId: number | null = null;
  selectedMember: Member | null = null;
  commentContentByBlog: Record<number, string> = {};
  isCreatingBlog = false;
  newBlogTitle = '';
  newBlogDescription = '';
  editingBlogId: number | null = null;
  editBlogTitle = '';
  editBlogDescription = '';
  editingCommentId: number | null = null;
  editingCommentContent = '';

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
      userId: currentUserId,
      interactingUserId: null,
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
    this.blogService.deleteBlog(blog).subscribe({
      next: () => {
        this.refreshBlogs();
      }
    })
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

  }

  addBlogComment(blogId: number, content: string) {
      const blogComment : BlogComment = {
        id: 0,
        createdAt: new Date(),
        updatedAt: new Date(),
        content: content,
        blogId: blogId,
        userId: this.accountService.currentUser()?.id ?? 0,
      } 

      this.blogService.addBlogComment(blogComment).subscribe({
        next: () => { this.refreshBlogs()}
      });
  }

  deleteBlogComment(blogComment: BlogComment): void {
    this.blogService.deleteBlogComment(blogComment).subscribe({
      next: () => {
        this.refreshBlogs();
      }
    })
  }

  isEditingComment(comment: BlogComment): boolean {
    return this.editingCommentId === comment.id;
  }

  startEditComment(comment: BlogComment): void {
    this.editingCommentId = comment.id;
    this.editingCommentContent = comment.content;
  }

  cancelEditComment(): void {
    this.editingCommentId = null;
    this.editingCommentContent = '';
  }

  updateComment(comment: BlogComment): void {
    const content = this.editingCommentContent.trim();

    if (!content) {
      return;
    }

    const updatedComment: BlogComment = {
      ...comment,
      content,
      updatedAt: new Date()
    };

    this.blogService.updateBlogComment(updatedComment).subscribe({
      next: () => {
        this.refreshBlogs();
        this.cancelEditComment();
      }
    });
  }

  toggleLike(blog: Blog) {
    blog.interactingUserId = this.accountService.currentUser()?.id ?? null;

    if (blog.interactingUserId == null) return;

    this.blogService.isBlogLikedByUser(blog.id, blog.interactingUserId).subscribe({
      next: (response) => {
        if (response) {
          this.blogService.undoBlogLike(blog).subscribe();
        } else {
          this.blogService.addBlogLike(blog).subscribe();
        }
         this.refreshBlogs();
      }
    });
  }


  nextPage(): void {
    this.pageNumber++;
    this.refreshBlogs();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.refreshBlogs();
    }
  }

  private refreshBlogs(): void {
    this.blogService.gatherAllBlogs(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.blogs = response.body || [];
      }
    });
  }

}
