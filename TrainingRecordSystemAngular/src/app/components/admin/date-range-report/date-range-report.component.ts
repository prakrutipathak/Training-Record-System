import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { DaterangeBasedReport } from 'src/app/models/daterange-based-report.model';
import { Job } from 'src/app/models/job.model';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-date-range-report',
  templateUrl: './date-range-report.component.html',
  styleUrls: ['./date-range-report.component.css']
})
export class DateRangeReportComponent implements OnInit {

  loading: boolean = false;

  jobsroles: Job[] = [];
  jobId: number | null = null;

  startDate: string | null = null;
  endDate: string | null = null;

  reportDetails: DaterangeBasedReport[] = [];



  constructor(private adminService: AdminService, private router: Router, private cdr: ChangeDetectorRef) { }


  ngOnInit(): void {
    this.loadjobRoles();
  }

  onJobRoleChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.jobId = parseInt(selectElement.value, 10);
    this.startDate = null;
    this.endDate = null;
    this.loadDaterangeBasedReport(this.jobId, this.startDate, this.endDate);
  }

  selectStartDate(): void {
    this.cdr.detectChanges();
    this.startDate = this.startDate;

  }
  selectEndDate(): void {
    this.cdr.detectChanges();
    this.endDate = this.endDate;
    this.loadDaterangeBasedReport(this.jobId, this.startDate, this.endDate);

  }

  loadDaterangeBasedReport(jobId: number | null, startDate: string | null, endDate: string | null): void {
    this.loading = true;
    this.adminService.daterangeBasedReport(jobId, startDate, endDate).subscribe({
      next: (response: ApiResponse<DaterangeBasedReport[]>) => {
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

  loadjobRoles(): void {
    this.loading = true;
    this.adminService.getAllJobs().subscribe({
      next: (response: ApiResponse<Job[]>) => {
        if (response.success) {
          this.jobsroles = response.data;

        }
        else {
          this.jobsroles = [];
          console.error('Failed to fetch jobs', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.jobsroles = [];
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
