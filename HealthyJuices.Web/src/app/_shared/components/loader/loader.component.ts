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

  isVisible: boolean;
  loaderName = '';
  loadersService: LoadersService;
  toHideSub: Subscription | undefined;
  toShowSub: Subscription | undefined;

  constructor(loadersService: LoadersService) {
    this.loadersService = loadersService;
    this.isVisible = false;
  }

  ngOnInit(): void {
    this.toShowSub = this.loadersService.getPendingToShow().subscribe(loaderName => {
      this.handleSignal(loaderName, this.loaderName, 'show');
    });

    this.toHideSub = this.loadersService.getPendingToHide().subscribe(loaderName => {
      this.handleSignal(loaderName, this.loaderName, 'hide');
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

  handleSignal(name: any, myName: string, action: string): void {
    if (name !== myName) { return; }
    if (name === myName) {
      if (action === 'show') {
        this.isVisible = true;
      }
      if (action === 'hide') {
        this.isVisible = false;
      }
    }
  }

  ngOnDestroy(): void {
    if (this.toShowSub) {
      this.toShowSub.unsubscribe();
    }
    if (this.toHideSub) {
      this.toHideSub.unsubscribe();
    }
  }
}
