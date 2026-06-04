import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberNetworkingComponent } from './members/member-list/member-networking.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { InsightsComponent } from './insights/insights.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './_guards/admin.guard';
import { BlogsComponent } from './blogs/blogs.component';
import { QuizzesComponent } from './quizzes/quizzes.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    // authGuard set up for specific routes
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'members', component: MemberNetworkingComponent},
      {path: 'members/:id', component: MemberDetailComponent,
       resolve: {member: memberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate:[preventUnsavedChangesGuard]},
      {path: 'insights', component: InsightsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'blogs', component: BlogsComponent},
      {path: 'quizzes', component: QuizzesComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard]},
    ]
  },
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'errors', component: TestErrorsComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];
