import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Trainer } from 'src/app/models/trainer.model';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-trainer-list',
  templateUrl: './trainer-list.component.html',
  styleUrls: ['./trainer-list.component.css']
})
export class TrainerListComponent implements OnInit{
  trainer : Trainer [] | undefined |null;
  loading:boolean=false;
  
  //pagination
  pageNumber: number = 1;
  pageSize: number = 4;
  totalItems:number = 0;
  totalPages: number =0;

  constructor(private adminService:AdminService,private router:Router){}

  ngOnInit(): void {
    this.loadTrainer(this.pageNumber);
  }

  loadTrainerCount():void{
    this.adminService.getTotalTrainerCount().subscribe({
      next:(response: ApiResponse<number>)=>{
        if (response.success) {
          console.log(response.data);
          this.totalItems = Number(response.data);
          this.totalPages = Math.ceil(this.totalItems/this.pageSize);
         
        }else{
          console.error('Failed to fetch trainer count', response.message);
          
        }
        this.loading=false;
      },
      error:(err)=> {
        this.trainer=[];
        console.error('Error featching trainer count',err.error);
        this.loading =false;
      },
    });
  }

  loadTrainer(pageNumber:number):void{
    this.loading=true;
    this.pageNumber = pageNumber;
    this.loadTrainerCount();
    this.adminService.getAllTrainerWithPagination(pageNumber,this.pageSize).subscribe({
      next:(response:ApiResponse<Trainer[]>)=>{
        if(response.success){
          this.trainer = response.data;
        }
        else{
          console.error('Failed to fetch trainer',response.message);
          
        }
        this.loading=false;
      },error:(error)=>{
        this.trainer=null;
        console.error('Error fetching trainer',error);
        this.loading=false;
      }
      
    });
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadTrainer(pageNumber);
  }

  changePageSize(pageSize: number): void {
   
    this.pageSize = pageSize;
    this.pageNumber = 1; // Reset to first page
    this.totalPages = Math.ceil(this.totalItems / this.pageSize); // Recalculate total pages
    this.loadTrainer(1);
  };

  
    nextPage() {
        if (this.pageNumber < this.totalPages) {
            this.pageNumber++;
            this.loadTrainer(this.pageNumber);
        }
    }

    previousPage() {
        if (this.pageNumber > 1) {
            this.pageNumber--;
            this.loadTrainer(this.pageNumber);

        }
    }
}
