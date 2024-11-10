import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { MonthlyAdminReport } from 'src/app/models/monthly-admin-report.model';
import { Trainer } from 'src/app/models/trainer.model';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-monthly-report',
  templateUrl: './monthly-report.component.html',
  styleUrls: ['./monthly-report.component.css']
})
export class MonthlyReportComponent implements OnInit {

  loading: boolean = false;

  trainers: Trainer[] = [];
  userId: number | null = null;

  year: number | null = null;
  month: number | null = null;
  months: string[] = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ]
  years: number[] = [];

  reportDetails: MonthlyAdminReport[] = [];


  constructor(private adminService: AdminService, private router: Router, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    for (let year = 2000; year <= 2030; year++) {
      this.years.push(year);
    }
    this.loadTrainers();
  }



  onTrainerChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.year = null;
    this.month = null;
    this.userId = parseInt(selectElement.value, 10);

    this.loadMonthlyAdminReport(this.userId, this.month, this.year);
    
  }
  
  onYearChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.year = parseInt(selectElement.value, 10);

  }
  onMonthChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.month = parseInt(selectElement.value, 10);
    this.loadMonthlyAdminReport(this.userId, this.month, this.year);

  }


  loadMonthlyAdminReport(userId: number | null, month: number | null, year: number | null): void {
    this.loading = true;
    this.adminService.monthlyAdminReport(userId, month, year).subscribe({
      next: (response: ApiResponse<MonthlyAdminReport[]>) => {
        if (response.success) {
          console.log(response.data);

          this.reportDetails = response.data;
        }
        else {
          this.reportDetails = [];
          console.error('Failed to fetch report', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.reportDetails = [];
        console.error(err.error.message);
        this.cdr.detectChanges();

      },
      complete: () => {
        this.loading = false;
        console.log("Completed");
      }

    })
  }

  loadTrainers(): void {
    this.loading = true;
    this.adminService.getAllTrainer().subscribe({
      next: (response: ApiResponse<Trainer[]>) => {
        if (response.success) {
          this.trainers = response.data;

        }
        else {
          this.trainers = [];
          console.error('Failed to fetch trainers', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.trainers = [];
        console.error(err.error.message);
        this.cdr.detectChanges();

      },
      complete: () => {
        this.loading = false;
        console.log("Completed");
      }
    });
  }
}
