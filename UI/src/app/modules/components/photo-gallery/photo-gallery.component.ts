import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Photo } from 'src/app/core/models/Photo';

@Component({
  selector: 'app-photo-gallery',
  templateUrl: './photo-gallery.component.html',
  styleUrls: ['./photo-gallery.component.css']
})
export class PhotoGalleryComponent implements OnChanges {
  @Input() photos: Photo[] = [];
  @Output() photoChange = new EventEmitter<Photo>();

  currentIndex = 0;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['photos']) {
      this.currentIndex = 0;
      this.emitCurrent();
    }
  }

  get currentPhoto(): Photo | null {
    return this.photos[this.currentIndex] ?? null;
  }

  select(index: number) {
    if (index < 0 || index >= this.photos.length)
      return;

    this.currentIndex = index;
    this.emitCurrent();
  }

  previous() {
    this.select((this.currentIndex - 1 + this.photos.length) % this.photos.length);
  }

  next() {
    this.select((this.currentIndex + 1) % this.photos.length);
  }

  private emitCurrent() {
    const photo = this.currentPhoto;
    if (photo)
      this.photoChange.emit(photo);
  }
}
