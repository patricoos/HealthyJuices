import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { ToastsService } from 'src/app/_shared/services/toasts.service';

@Component({
  selector: 'app-confirm-register',
  templateUrl: './confirm-register.component.html',
  styleUrls: ['./confirm-register.component.scss']
})
export class ConfirmRegisterComponent implements OnInit {

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private authService: AuthService,
    public messageService: ToastsService) { }

  ngOnInit(): void {
    const token = this.activatedRoute.snapshot.queryParamMap.get('token');
    const email = this.activatedRoute.snapshot.queryParamMap.get('email');
    if (token && email) {
      this.messageService.showSuccess(`User "${email}" activated!`);
      this.authService.confirmRegister('', email, token).subscribe(x => {
        this.router.navigate(['/auth/login']);
      }, error => this.messageService.showError(error));
    }

  }

}
