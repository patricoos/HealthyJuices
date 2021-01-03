import { Injectable } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TimeSpan } from '../models/time-span.model';
import { TimeConverter } from '../utils/time.converter';
import { FullCallendarConsts } from '../constants/full-calendar.const';
import { FilterService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class TableQueryService {
  private readonly tableFiltersName = 'tableFilters';
  private readonly globalDateRangeFiltersName = 'dateRange';
  private readonly sortOrderName = 'sortOrder';
  private readonly sortFieldName = 'sortField';
  private readonly typeName = 'type';
  private readonly pageName = 'page';

  private readonly fromPosision = 0;
  private readonly toPosision = 1;

  readonly defaultDrangeDates: Date[] = TimeConverter.getCurrentTwoWeeksDateRange();
  readonly defaultSortField = null;
  readonly defaultSortOrder = 1;

  sortField: string | null = null;
  sortOrder = 1;

  page = 0;

  type: number | null = null;

  tableFilters = {};
  filterValues = {};

  rangeDates: Date[] = [...this.defaultDrangeDates];

  defaultDurationDate: Date | null = null;

  get fromDate(): Date | null {
    return this.rangeDates[this.fromPosision] ? TimeConverter.getMinTime(this.rangeDates[this.fromPosision]) : null;
  }
  get toDate(): Date | null {
    return this.rangeDates[this.toPosision] ? TimeConverter.getMaxTime(this.rangeDates[this.toPosision]) : null;
  }

  calendarLocale = FullCallendarConsts.getCallendarLocale();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private filterService: FilterService) {
    this.addCustomFiltersToTable();
    this.init();
  }

  init(daterange?: Date[]): void {
    this.sortField = this.defaultSortField;
    this.sortOrder = this.defaultSortOrder;
    this.type = null;
    this.page = 0;
    this.tableFilters = {};
    this.filterValues = {};
    this.rangeDates = daterange ? daterange : [...this.defaultDrangeDates];
    this.defaultDurationDate = TimeConverter.getMinTime(new Date());
    this.checkRouteParams();
  }

  checkRouteParams(): void {
    if (!this.activatedRoute.snapshot.queryParamMap.keys.length) {
      return;
    }

    const globalDateRangeFilters = this.activatedRoute.snapshot.queryParamMap.get(this.globalDateRangeFiltersName);
    if (globalDateRangeFilters) {
      this.rangeDates = (JSON.parse(globalDateRangeFilters) as any[]).map(v => v ? new Date(v) : v);
    }

    const queryTableFilters = this.activatedRoute.snapshot.queryParamMap.get(this.tableFiltersName);
    if (queryTableFilters) {
      const filters = JSON.parse(queryTableFilters);
      Object.keys(filters).forEach(x => {
        const value = filters[x];

        if (filters[x].matchMode === 'dateToDurationFilter') {
          value.value = new Date(value.value);
        }
        if (filters[x].matchMode === 'dateRangeFilter') {
          value.value = (value.value as any[]).map(v => v ? new Date(v) : v);
        }
        this.tableFilters = { ...this.tableFilters, [x]: value };
        this.filterValues = { ...this.filterValues, [x]: value.value };
      });
    }
    const sortField = this.activatedRoute.snapshot.queryParamMap.get(this.sortFieldName);
    if (sortField) {
      this.sortField = sortField;
    }

    const sortOrder = this.activatedRoute.snapshot.queryParamMap.get(this.sortOrderName);
    if (sortOrder) {
      this.sortOrder = JSON.parse(sortOrder);
    }

    const type = this.activatedRoute.snapshot.queryParamMap.get(this.typeName);
    if (type) {
      this.type = JSON.parse(type);
    }

    const page = this.activatedRoute.snapshot.queryParamMap.get(this.pageName);
    if (page) {
      this.page = JSON.parse(page);
    }
  }

  onSort(field: string | null, order: number): Params {
    this.sortOrder = order;
    this.sortField = field;
    const queryParams: Params = { [this.sortOrderName]: order.toString(), [this.sortFieldName]: field };
    this.router.navigate([], { relativeTo: this.activatedRoute, queryParams, replaceUrl: true, queryParamsHandling: 'merge' });
    return queryParams;
  }

  onChangeType(): Params {
    const type = this.type ? this.type.toString() : null;
    const queryParams: Params = { [this.typeName]: type };
    this.router.navigate([], { relativeTo: this.activatedRoute, queryParams, replaceUrl: true, queryParamsHandling: 'merge' });
    return queryParams;
  }

  onChangePage(event: any): Params {
    const page = event.first ? event.first.toString() : null;
    const queryParams: Params = { [this.pageName]: page };
    this.router.navigate([], { relativeTo: this.activatedRoute, queryParams, replaceUrl: true, queryParamsHandling: 'merge' });
    return queryParams;
  }

  onFilter(get?: () => void): void {
    let queryParams: Params = {};
    if (Object.keys(this.tableFilters).length) {
      queryParams = { ...queryParams, [this.tableFiltersName]: JSON.stringify(this.tableFilters) };
    }
    if (this.rangeDates[this.fromPosision] !== this.defaultDrangeDates[this.fromPosision]
      && this.rangeDates[this.toPosision] !== this.defaultDrangeDates[this.toPosision]) {
      queryParams = { ...queryParams, [this.globalDateRangeFiltersName]: JSON.stringify(this.rangeDates) };
    }

    this.router.navigate([], { relativeTo: this.activatedRoute, queryParams, replaceUrl: true });

    if (this.sortOrder !== this.defaultSortOrder || this.sortField !== this.defaultSortField) {
      setTimeout(() => this.onSort(this.sortField, this.sortOrder), 0);
    }
    if (get) {
      get();
    }
  }

  addCustomFiltersToTable(): any {
    this.filterService.register('dateRangeFilter', (cellValue: any, filterValue: Date[]): boolean => {
      if (!filterValue || filterValue.length !== 2 || !filterValue[this.fromPosision]) {
        return true;
      }

      let endDay = TimeConverter.getMaxTime(new Date(filterValue[this.fromPosision]));
      if (filterValue[this.toPosision]) {
        endDay = TimeConverter.getMaxTime(new Date(filterValue[this.toPosision]));
      }

      const value = new Date(cellValue);
      return value.getTime() >= new Date(filterValue[this.fromPosision]).getTime() && value.getTime() <= endDay.getTime();
    });

    this.filterService.register('dateToDurationFilter', (cellValue: any, filterValue: Date): boolean => {
      if (!filterValue) {
        return true;
      }
      return cellValue <= TimeSpan.fromDate(new Date(filterValue)).ticks;
    });
  }
}
