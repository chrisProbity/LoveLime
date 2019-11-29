import { Injectable } from '@angular/core';
import { MemberEditComponent } from '../member/member-edit/member-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnSavedChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Aye you sure you want to continue? Any unsaved changes will be lost!');
        }
        return true;
    }
}
