import { SelectItem } from 'primeng/api';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FullCallendarConsts } from 'src/app/_shared/constants/full-calendar.const';
import { ProductUnitType } from 'src/app/_shared/models/enums/product-unit-type.enum';
import { Product } from 'src/app/_shared/models/products/product.model';
import { ProductsService } from 'src/app/_shared/services/http/products.service';
import { TableQueryService } from 'src/app/_shared/services/table.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements AfterViewInit {
  productsComponentLoader = 'productsComponentLoader';

  products: Product[] = [];
  columns = [
    { field: 'name', header: 'Name' },
    { field: 'quantityPerUnit', header: 'Quantity Per Unit' },
    { field: 'unit', header: 'Unit' },
    { field: 'isActive', header: 'Active' },
    { field: 'dateCreated', header: 'Created' },
    { field: 'action', header: 'Action', width: '145px' },
  ];
  boolStatus: SelectItem[] = [{ label: 'True', value: true }, { label: 'False', value: false }];
  calendarLocale = FullCallendarConsts.getCallendarLocale();
  ProductUnitType = ProductUnitType;

  constructor(private productsService: ProductsService, private toastsService: ToastsService,
    private tableQueryService: TableQueryService, private router: Router) { }

  ngAfterViewInit(): void {
    this.productsService.getAll(this.productsComponentLoader).subscribe(x => {
      console.log(x);
      this.products = x;
    }, error => this.toastsService.showError(error));
  }

  onAddNew(): void {
    this.router.navigate(['management/products/add']);
  }

  onEdit(order: Product): void {
    this.router.navigate(['management/products/', order.id]);
  }
}
