import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Job } from 'src/app/models/job.model';
import { UpcomingTraining } from 'src/app/models/upcomingtraining.model';
import { AdminService } from 'src/app/services/admin.service';
import { ManagerService } from 'src/app/services/manager.service';

@Component({
  selector: 'app-upcomingtrainingprogram',
  templateUrl: './upcomingtrainingprogram.component.html',
  styleUrls: ['./upcomingtrainingprogram.component.css']
})
export class UpcomingtrainingprogramComponent implements OnInit {
  allJobs : Job[] = [];
  loading:boolean=false;
  jobId: number|null = null;
  upcomingTraining:UpcomingTraining[] | undefined;
  constructor(
    private adminService: AdminService, 
    private managerService:ManagerService
  ){ }
  ngOnInit(): void {
    this.getAllJobs();
    this.loadTrainingProgram();
  }

  getAllJobs() : void{
    this.adminService.getAllJobs().subscribe({
      next : (response) =>{
        if(response.success){
          this.allJobs = response.data;
        }
        else{
          alert(response.message);
        }
      },
      error:(err) =>{
        console.error('Failed to fetch jobs',err.error.message);
        alert(err.error.message);
      },
      complete:() =>{
        console.log('completed');
      }
    })
  }
  loadTrainingProgram():void{
    this.loading=true;
    this.managerService.loadTrainingProgram(null).subscribe({
      next:(response: ApiResponse<UpcomingTraining[]>)=>{
      if(response.success){
        this.upcomingTraining=response.data;
      }
      else{
        console.error('Failed to fetch data',response.message)
      }
      this.loading=false;
    },error:(error)=>{
console.error('Error fetching data',error)
this.loading=false;
    }
    });

  }
  loadTrainingProgramByJobId(jobid:number | null):void{
    this.loading=true;
    this.managerService.loadTrainingProgram(jobid).subscribe({
      next:(response: ApiResponse<UpcomingTraining[]>)=>{
      if(response.success){
        this.upcomingTraining=response.data;
      }
      else{
        this.upcomingTraining=[];
        console.error('Failed to fetch data',response.message)
      }
      this.loading=false;
    },error:(error)=>{
      this.upcomingTraining=[];
console.error('Error fetching data',error)
this.loading=false;
    }
    });

  }
  onJobChange(event: any): void {
   
    this.jobId = +event.target.value; // Convert the string to number
    console.log('Selected Job Number:', this.jobId);
   
    this.loadTrainingProgramByJobId(this.jobId);
  }

}
