import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDetails } from 'src/app/models/userDetails.model';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  title = 'CivicaShoppingApp';
  isAuthenticated :boolean = false;
  username: string | null | undefined;
  userId: number | undefined;

  role :number | null = null;


  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); // Manually trigger change detection.
    });
    this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
      this.cdr.detectChanges(); // Manually trigger change detection.
    });
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      this.userId = Number(userId);
      this.getUserDetailsByUserId(this.userId);

    });
  }

  getUserDetailsByUserId(userId: number | undefined) {
    this.authService.getUserDetailsByUserId(userId).subscribe({
      next: (response) => {
        if (response.success) {
          this.role = response.data.role;
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }

  signOut() {
    this.role = null;
    this.authService.signOut();
    this.router.navigate(['/home']);
  }
}
