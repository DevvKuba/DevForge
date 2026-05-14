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
import { ToastrService } from 'ngx-toastr';

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
  private toastr = inject(ToastrService);

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
      this.toastr.warning('Title and description are required.');
      return;
    }

    const currentUserId = this.accountService.currentUser()?.id;
    if(currentUserId === null || currentUserId === undefined){
      this.toastr.error('You must be logged in to create a blog.');
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
      next: (createdBlog: Blog) => {
        if(createdBlog && typeof createdBlog === 'object' && 'id' in createdBlog){
          this.blogs = [createdBlog, ...this.blogs];
        } else {
          this.refreshBlogs();
        }
        this.newBlogTitle = '';
        this.newBlogDescription = '';
        this.isCreatingBlog = false;
        this.toastr.success('Blog created.');
      },
      error: () => this.toastr.error('Failed to create blog.')
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
      this.toastr.warning('Title and description are required.');
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
        this.toastr.success('Blog updated.');
      },
      error: () => this.toastr.error('Failed to update blog.')
    });
  }

  deleteBlog(blog: Blog): void {
    this.blogService.deleteBlog(blog).subscribe({
      next: () => {
        this.blogs = this.blogs.filter(b => b.id !== blog.id);
        if(this.openCommentsBlogId === blog.id){
          this.openCommentsBlogId = null;
          this.openBlogComments = [];
        }
        this.toastr.success('Blog deleted.');
      },
      error: () => this.toastr.error('Failed to delete blog.')
    });
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
    const trimmedContent = content.trim();
    if(!trimmedContent){
      this.toastr.warning('Comment cannot be empty.');
      return;
    }

    const currentUserId = this.accountService.currentUser()?.id;
    if(currentUserId === null || currentUserId === undefined){
      this.toastr.error('You must be logged in to comment.');
      return;
    }

    const payload: BlogComment = {
      id: 0,
      createdAt: new Date(),
      updatedAt: new Date(),
      content: trimmedContent,
      userId: Number(currentUserId),
      blogId
    };

    this.blogService.addBlogComment(payload).subscribe({
      next: (createdComment: BlogComment) => {
        const commentToAdd = createdComment && typeof createdComment === 'object' && 'id' in createdComment
          ? createdComment
          : payload;

        this.blogs = this.blogs.map(blog => {
          if(blog.id !== blogId) return blog;
          return {
            ...blog,
            blogComments: [...(blog.blogComments || []), commentToAdd]
          };
        });

        this.commentContentByBlog[blogId] = '';
        this.toastr.success('Comment added.');
      },
      error: () => this.toastr.error('Failed to add comment.')
    });
  }

  deleteBlogComment(blog: Blog, comment: BlogComment): void {
    this.blogService.deleteBlogComment(comment).subscribe({
      next: () => {
        this.blogs = this.blogs.map(b => {
          if(b.id !== blog.id) return b;
          return {
            ...b,
            blogComments: b.blogComments.filter(c => c.id !== comment.id)
          };
        });
        this.toastr.success('Comment deleted.');
      },
      error: () => this.toastr.error('Failed to delete comment.')
    });
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
