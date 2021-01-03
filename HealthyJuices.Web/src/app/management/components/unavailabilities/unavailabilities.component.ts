import { Unavailability } from 'src/app/management/models/unavailability.model';
import { Component, OnInit, ViewChild } from '@angular/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { FullCalendar } from 'primeng/fullcalendar';
import { DialogService } from 'primeng/dynamicdialog';
import { UnavailabilitEditFormModalComponent } from './unavailabilit-edit-form-modal/unavailabilit-edit-form-modal.component';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { UnavailabilitiesService } from 'src/app/_shared/services/http/unavailabilities.service';
import { UnavailabilityReason } from 'src/app/_shared/models/enums/unavailability-reason.enum';

@Component({
  selector: 'app-unavailabilities',
  templateUrl: './unavailabilities.component.html',
  styleUrls: ['./unavailabilities.component.scss']
})
export class UnavailabilitiesComponent implements OnInit {
  unavailabilitiesLoader = 'unavailabilitiesLoader';
  unavailabilities: Unavailability[] = [];

  events: any[] = [];
  options: any = {
    plugins: [dayGridPlugin, interactionPlugin],
    defaultDate: new Date(),
    height: window.innerHeight - 120,
    header: {
      left: 'prev, next',
      center: 'title',
      right: 'today'
    },
    displayEventTime: false,
    firstDay: 1,
    selectable: true,
    selectHelper: true,

    dateClick: (e: any) => this.onDateClick(e.date),
    eventClick: (e: any) => this.onEventClick(e)
  };

  @ViewChild('calendar') private calendar!: FullCalendar;

  constructor(public dialogService: DialogService, private toastsService: ToastsService,
    private unavailabilitiesService: UnavailabilitiesService) { }

  ngOnInit(): void {
    this.unavailabilitiesService.getAll(this.unavailabilitiesLoader).subscribe(x => {
      this.unavailabilities = x;
      this.events = x.map(r => {
        return {
          id: r.id,
          title: UnavailabilityReason[r.reason],
          start: r.from,
          end: r.to,
          color: 'orange'
        };
      });
    }, error => this.toastsService.showError(error));
  }

  onDateClick(day: Date): void {
    const ref = this.dialogService.open(UnavailabilitEditFormModalComponent, {
      header: 'Add Unavailability',
      width: '70%'
    });

    ref.onClose.subscribe((response: any) => {
      if (response) {
        this.ngOnInit();
      }
    });
  }
  onEventClick(event: any): void {
    const ref = this.dialogService.open(UnavailabilitEditFormModalComponent, {
      header: 'Edit Unavailability',
      width: '70%',
      data: {
        unavailability: this.unavailabilities.find(x => x.id === +event.event.id)
      }
    });

    ref.onClose.subscribe((response: any) => {
      if (response) {
        this.ngOnInit();
      }
    });
  }
}
