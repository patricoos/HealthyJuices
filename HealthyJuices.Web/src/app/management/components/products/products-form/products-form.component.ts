import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, SelectItem } from 'primeng/api';
import { ProductUnitType } from 'src/app/_shared/models/enums/product-unit-type.enum';
import { Product } from 'src/app/_shared/models/products/product.model';
import { ProductsService } from 'src/app/_shared/services/http/products.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';
import { EnumExtension } from 'src/app/_shared/utils/enum.extension';
import { FormGroupExtension } from 'src/app/_shared/utils/form-group.extension';

@Component({
  selector: 'app-products-form',
  templateUrl: './products-form.component.html',
  styleUrls: ['./products-form.component.scss']
})
export class ProductsFormComponent implements AfterViewInit {
  productsFormComponentLoader = 'productsFormComponentLoader';

  lat = 52.22;
  lng = 21.01;

  selectedProduct: Product | undefined;

  id: string | undefined;
  private sub: any;
  editForm: FormGroup = this.initForm();
  units: SelectItem[] = EnumExtension.getLabelAndValues(ProductUnitType);

  constructor(private route: ActivatedRoute, private prodService: ProductsService, private toastsService: ToastsService,
    private confirmationService: ConfirmationService, private router: Router) { }

  ngAfterViewInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      if (this.id) {
        this.getDetails(this.id);
      }
    });
  }

  getDetails(id: string): void {
    this.prodService.get(id, this.productsFormComponentLoader).subscribe(x => {
      console.log(x);
      this.selectedProduct = x;
      this.editForm = this.initForm(x);
    }, error => this.toastsService.showError(error));
  }

  private initForm(product: Product | null = null): FormGroup {
    const form = new FormGroup({
      id: new FormControl(product ? product.id : 0),

      name: new FormControl(product ? product.name : null, Validators.required),
      description: new FormControl(product ? product.description : null),
      isActive: new FormControl(product ? product.isActive : false),

      quantityPerUnit: new FormControl(product ? product.quantityPerUnit : null, Validators.required),
      unit: new FormControl(product ? product.unit : null, Validators.required),
    });
    return form;
  }


  onSave(): void {
    if (this.editForm.invalid) {
      FormGroupExtension.markFormAssDirtyAndTouched(this.editForm);
      this.toastsService.showError('Invalid Form');
      return;
    }
    this.prodService.addOrEdit(this.editForm.value, this.productsFormComponentLoader).subscribe(x => {
      this.toastsService.showSuccess(this.editForm.controls.name.value + ' updated!');
      this.router.navigate(['management/products']);
    }, error => this.toastsService.showError(error));
  }

  onDelete(): void {
    if (!this.selectedProduct || !this.selectedProduct.id) { return; }
    this.confirmationService.confirm({
      message: 'Are you sure tha you want to delete',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.delete()
    });
  }

  onCancel(): void {
    this.router.navigate(['management/products']);
  }

  private delete(): void {
    if (!this.selectedProduct || !this.selectedProduct.id) { return; }
    this.prodService.delete(this.selectedProduct.id, this.productsFormComponentLoader).subscribe(x => {
      this.toastsService.showSuccess(this.selectedProduct?.name + ' deteted!');
      this.router.navigate(['management/products']);
    }, error => this.toastsService.showError(error));
  }
}
