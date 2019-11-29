import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './member/member-List/member-List.component';
import { AuthGuard } from './_guard/auth.guard';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver ';
import { PreventUnSavedChanges } from './_guard/prevent-unsavedchanged.guard';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
       path: '',
       runGuardsAndResolvers: 'always',
       canActivate: [AuthGuard],
       children: [
        {path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
        {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
        {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
         canDeactivate: [PreventUnSavedChanges]},
        {path: 'lists', component: ListsComponent},
        {path: 'messages', component: MessagesComponent},
       ]
    },

    {path: '**', redirectTo: '', pathMatch: 'full'}
];
