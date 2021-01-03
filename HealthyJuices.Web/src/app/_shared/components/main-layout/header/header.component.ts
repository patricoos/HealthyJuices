import { Component, OnInit } from '@angular/core';
import { UserRole } from 'src/app/_shared/models/enums/user-role.enum';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { MenuItem } from 'src/app/_shared/utils/menu-item.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  showHamurger = false;
  UserRole = UserRole;
  userRoles: Array<UserRole> = [];
  userName: string | null = null;


  menu: MenuItem[] = [
    { label: 'Orders', routerLink: '/orders', roles: [UserRole.BusinessOwner], icon: 'fa fa-check-square-o ' },
    {
      label: 'Management', icon: 'fa fa-cog', roles: [UserRole.BusinessOwner], children: [
        { label: 'Companies', routerLink: '/management/companies', roles: [UserRole.BusinessOwner] },
        { label: 'Unavailabilities', routerLink: '/management/unavailabilities', roles: [UserRole.BusinessOwner] },
        { label: 'Orders', routerLink: '/management/orders', roles: [UserRole.BusinessOwner] },
      ]
    },
  ];

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.userRoles = this.authService.getUserRoles();
    this.userName = this.authService.getUserName();
  }

  logout(): void {
    this.authService.logout();
  }

  onNavigateHome(): void {
    this.authService.navigateByUserRole();
  }

  changeHamburgerVisibilyty(): void {
    this.showHamurger = !this.showHamurger;
  }

  hideHamburgerVisibilyty(): void {
    this.showHamurger = false;
  }

  isUserInOneRoleOf(allowedRoles: Array<UserRole> | undefined): boolean {
    let result = false;
    if (allowedRoles) {
      allowedRoles.forEach(x => {
        if (this.userRoles.includes(x)) {
          result = true;
        }
      });
    }
    return result;
  }

  installPwa(): void {
    //   this.Pwa.installPwa();
  }

  update(): void {
    document.location.reload();
  }
}
