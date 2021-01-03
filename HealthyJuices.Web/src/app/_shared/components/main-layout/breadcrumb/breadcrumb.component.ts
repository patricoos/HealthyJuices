import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd, PRIMARY_OUTLET } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { BreadCrumb } from 'src/app/_shared/utils/bread-crumb.model';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {

  public breadcrumbs: BreadCrumb[] = [];
  subscription?: Subscription;

  constructor(private activatedRoute: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.initBreadCrumb();
    this.breadcrumbs = this.getBreadcrumbs(this.activatedRoute);
  }

  onHome(): void {
    this.authService.navigateByUserRole();
  }

  initBreadCrumb(): void {
    this.subscription = this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(event => {
      const root: ActivatedRoute = this.activatedRoute.root;
      this.breadcrumbs = this.getBreadcrumbs(root);
    });
  }

  // onHome() {
  //   this.authService.navigateByUserRole();
  // }

  private getBreadcrumbs(route: ActivatedRoute, url: string = '', breadcrumbs: BreadCrumb[] = []): BreadCrumb[] {
    const ROUTE_DATA_BREADCRUMB = 'breadcrumb';

    // get the child routes
    const children: ActivatedRoute[] = route.children;

    // return if there are no more children
    if (children.length === 0) {
      return breadcrumbs;
    }

    // iterate over each children
    for (const child of children) {
      // verify primary route
      if (child.outlet !== PRIMARY_OUTLET) {
        continue;
      }

      // verify the custom data property "breadcrumb" is specified on the route
      if (!child.snapshot.data.hasOwnProperty(ROUTE_DATA_BREADCRUMB)) {
        return this.getBreadcrumbs(child, url, breadcrumbs);
      }

      // get the route's URL segment
      const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
      // console.log(child.snapshot);

      // append route URL to URL
      url += `/${routeURL}`;

      // add breadcrumb
      const breadcrumb: BreadCrumb = {
        label: child.snapshot.data[ROUTE_DATA_BREADCRUMB],
        url,
        canNavigateFromBreadcrumb: child.snapshot.data.canNavigateFromBreadcrumb
      };
      if (Object.keys(child.snapshot.params).length > 0) {
        breadcrumb.params = child.snapshot.params;
      }

      if (breadcrumbs.length > 0 && (breadcrumbs[breadcrumbs.length - 1].url + '/') === breadcrumb.url) {
      } else {
        breadcrumbs.push(breadcrumb);
      }

      // console.log(breadcrumbs);

      // recursive
      return this.getBreadcrumbs(child, url, breadcrumbs);
    }
    return [];
  }
}
