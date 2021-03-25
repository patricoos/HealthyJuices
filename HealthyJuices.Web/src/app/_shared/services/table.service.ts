import { Injectable } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TimeConverter } from '../utils/time.converter';
import { FullCallendarConsts } from '../constants/full-calendar.const';
import { FilterService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class TableQueryService {
  private readonly tableFiltersName = 'tableFilters';
  private readonly sortOrderName = 'sortOrder';
  private readonly sortFieldName = 'sortField';

  readonly defaultSortField = null;
  readonly defaultSortOrder = 1;

  sortField: string | null = null;
  sortOrder = 1;

  type: number | null = null;

  tableFilters = {};
  filterValues = {};

  calendarLocale = FullCallendarConsts.getCallendarLocale();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private filterService: FilterService) {
    this.addCustomFiltersToTable();
    this.init();
  }

  init(): void {
    this.sortField = this.defaultSortField;
    this.sortOrder = this.defaultSortOrder;
    this.type = null;
    this.tableFilters = {};
    this.filterValues = {};
    this.checkRouteParams();
  }

  checkRouteParams(): void {
    if (!this.activatedRoute.snapshot.queryParamMap.keys.length) {
      return;
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
  }

  onSort(field: string | null, order: number): Params {
    this.sortOrder = order;
    this.sortField = field;
    const queryParams: Params = { [this.sortOrderName]: order.toString(), [this.sortFieldName]: field };
    this.router.navigate([], { relativeTo: this.activatedRoute, queryParams, replaceUrl: true, queryParamsHandling: 'merge' });
    return queryParams;
  }

  onFilter(get?: () => void): void {
    let queryParams: Params = {};
    if (Object.keys(this.tableFilters).length) {
      queryParams = { ...queryParams, [this.tableFiltersName]: JSON.stringify(this.tableFilters) };
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
      if (!filterValue || filterValue.length !== 2 || !filterValue[0]) {
        return true;
      }

      let endDay = TimeConverter.getMaxTime(new Date(filterValue[0]));
      if (filterValue[1]) {
        endDay = TimeConverter.getMaxTime(new Date(filterValue[1]));
      }

      const value = new Date(cellValue);
      return value.getTime() >= new Date(filterValue[0]).getTime() && value.getTime() <= endDay.getTime();
    });

    this.filterService.register('dateToDurationFilter', (cellValue: any, filterValue: Date): boolean => {
      if (!filterValue) {
        return true;
      }
      return cellValue <= new Date(filterValue).getMilliseconds();
    });
  }
}
