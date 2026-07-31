import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { PhotoGalleryComponent } from '../../components/photo-gallery/photo-gallery.component';
import { ProfileComponent } from './profile.component';

@NgModule({
  declarations: [ProfileComponent, PhotoGalleryComponent],
  imports: [
    SharedModule,
    RouterModule.forChild([{ path: '', component: ProfileComponent }])
  ]
})
export class ProfileModule { }
