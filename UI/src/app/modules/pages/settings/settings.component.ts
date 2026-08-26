import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ProfileVisibility } from 'src/app/core/models/Enums';
import { UserSettings } from 'src/app/core/models/User';
import { UserService } from 'src/app/core/services/user.service';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.css'],
    standalone: false
})
export class SettingsComponent implements OnInit {
  settings: UserSettings | null = null;
  isSaving = false;

  readonly ProfileVisibility = ProfileVisibility;

  constructor(private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userService.getUserSettings().subscribe({
      next: settings => this.settings = settings
    });
  }

  save() {
    if (!this.settings)
      return;

    this.isSaving = true;
    this.userService.updateUserSettings(this.settings).subscribe({
      next: settings => {
        this.isSaving = false;
        this.settings = settings;
        this.toastr.success('Settings have been updated');
      },
      error: _ => this.isSaving = false
    });
  }
}
