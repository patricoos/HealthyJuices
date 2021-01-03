import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { Unavailability } from 'src/app/management/models/unavailability.model';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { UnavailabilityReason } from 'src/app/_shared/models/enums/unavailability-reason.enum';
import { UnavailabilitiesService } from 'src/app/_shared/services/http/unavailabilities.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { EnumExtension } from 'src/app/_shared/utils/enum.extension';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';

@Component({
  selector: 'app-unavailabilit-edit-form-modal',
  templateUrl: './unavailabilit-edit-form-modal.component.html',
  styleUrls: ['./unavailabilit-edit-form-modal.component.scss']
})
export class UnavailabilitEditFormModalComponent implements OnInit {

  unavailabilitEditFormModalLoader = 'unavailabilitEditFormModalLoader';
  editForm: FormGroup = this.initForm(undefined);
  unavailability: Unavailability | undefined = undefined;
  reasons: SelectItem[] = EnumExtension.getLabelAndValues(UnavailabilityReason);
  calendarLocale = FullCallendarConsts.getCallendarLocale();

  constructor(public ref: DynamicDialogRef, public config: DynamicDialogConfig, private toastsService: ToastsService,
    private unavailabilitiesService: UnavailabilitiesService) { }

  ngOnInit(): void {
    this.unavailability = this.config.data?.unavailability;
    this.editForm = this.initForm(this.unavailability);
  }

  private initForm(unavailability: Unavailability | undefined): FormGroup {
    return new FormGroup({
      id: new FormControl(unavailability ? unavailability.id : null),
      from: new FormControl(unavailability ? new Date(unavailability.from) : null, Validators.required),
      to: new FormControl(unavailability ? new Date(unavailability.to) : null, Validators.required),
      reason: new FormControl(unavailability ? unavailability.reason : null, Validators.required),
      comment: new FormControl(unavailability ? unavailability.comment : null),
    });
  }

  onDelete(): void {
    if (this.unavailability) {
      this.unavailabilitiesService.delete(this.unavailability.id, this.unavailabilitEditFormModalLoader).subscribe(x => {
        this.toastsService.showSuccess('Unavailability deleted!');
        this.ref.close(true);
      }, error => this.toastsService.showError(error));
      this.ref.close(true);
    }
  }

  onSave(): void {
    if (this.editForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.editForm);
      this.toastsService.showError('Invalid Form');
      return;
    }

    this.unavailabilitiesService.addOrEdit(this.editForm.value, this.unavailabilitEditFormModalLoader).subscribe(x => {
      this.toastsService.showSuccess('Unavailability added!');
      this.ref.close(true);
    }, error => this.toastsService.showError(error));
  }

  onClose(): void {
    this.ref.close();
  }
}
