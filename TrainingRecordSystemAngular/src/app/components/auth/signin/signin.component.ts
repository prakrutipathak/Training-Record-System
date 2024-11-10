import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/services/admin.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { LocalstorageService } from 'src/app/services/helpers/localstorage.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {

  username: string = '';
  password: string = '';
  loading : boolean = false;
  alreadyLogin !: boolean ;
  
  constructor(
    private authService: AuthService,
    private adminService : AdminService,
    private router:Router,
    private cdr:ChangeDetectorRef ) {}

    login() {
      this.loading = true;
      this.adminService.getTrainerByLoginId(this.username).subscribe({
        next:(response) => {
          if(response.success) {
            this.alreadyLogin = response.data.loginbit;
            this.signinfunc();
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err) => {
          alert(err.error.message);
          this.loading = false;

}})
    
    }


signinfunc(){

  this.authService.signIn(this.username, this.password).subscribe({
    next:(response) => {
        if(response.success) {
          if(this.alreadyLogin == false)
            { 
              alert("First time loggedIn, please change your password...")
              this.router.navigate(['/changePassword'])
              
            }
            else{
              
              this.router.navigate(['/home']);
            }
          } else {
            alert(response.message);
          }
          this.cdr.detectChanges();
          this.loading = false;
      },
      error:(err) => {
        alert(err.error.message);
        this.loading = false;
}
    })
  }
    // checkLogin() {
    //   this.loading = true;
           
    // }
}
