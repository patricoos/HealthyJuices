import { Component, OnInit, OnDestroy, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoadersService } from '../../services/loaders.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit, OnDestroy, OnChanges {

  @Input()
  public set name(value: string) {
    this.loaderName = value;
  }
  @Input() showProgressBar = false;

  @Input() value = 0;
  @Input() maxValue = 100;

  progress = 0;

  isVisible = false;
  loaderName = '';
  loadersSub: Subscription | undefined;

  constructor(private loadersService: LoadersService) { }

  ngOnInit(): void {
    this.loadersSub = this.loadersService.getPendingToShow().subscribe(loaderNames => {
      this.isVisible = loaderNames.includes(this.loaderName);
    });
    this.calculateProgress();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.calculateProgress();
  }

  calculateProgress(): void {
    if (this.showProgressBar && this.value && this.maxValue && this.value <= this.maxValue) {
      this.progress = Math.round(100 * this.value / this.maxValue);
    }
  }

  ngOnDestroy(): void {
    if (this.loadersSub) {
      this.loadersSub.unsubscribe();
    }
  }
}
