import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Nominate } from 'src/app/models/nominate.model';
import { Participants } from 'src/app/models/participants.model';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-participants-list',
  templateUrl: './participants-list.component.html',
  styleUrls: ['./participants-list.component.css']
})
export class ParticipantsListComponent implements OnInit{
  participants: Nominate [] | undefined | null;
  loading:boolean=false;

  //pagination
  pageNumber: number = 1;
  pageSize: number = 4;
  totalItems:number = 0;
  totalPages: number =0;

  sortName: string = 'default';

  constructor(private trainerService:TrainerService,private router:Router){}

  ngOnInit(): void {
    this.loadParticipants(this.pageNumber,this.sortName)
  }

  loadParticipantCount():void{
    this.trainerService.getTotalParticipantsCount().subscribe({
      next:(response: ApiResponse<number>)=>{
        if (response.success) {
          console.log(response.data);
          this.totalItems = Number(response.data);
          this.totalPages = Math.ceil(this.totalItems/this.pageSize);
         
        }else{
          console.error('Failed to fetch participants count', response.message);
          
        }
        this.loading=false;
      },
      error:(err)=> {
        this.participants=[];
        console.error('Error featching participants count',err.error);
        this.loading =false;
      },
    })
  }

  loadParticipants(pageNumber:number,sortName:string):void{
    this.loading=true;
    this.sortName = sortName;
    this.pageNumber = pageNumber;
    this.loadParticipantCount();
    this.trainerService.getAllParticipantsWithPagination(pageNumber,this.pageSize,sortName).subscribe({
      next:(response:ApiResponse<Nominate[]>)=>{
        if(response.success){
          this.participants = response.data;
          console.log(this.participants);
        }
        else{
          console.error('Failed to fetch participants: ',response.message);
          
        }
        this.loading=false;
      },error:(error)=>{
        this.participants=null;
        console.error('Error participants trainer',error);
        this.loading=false;
      }
    });
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadParticipants(pageNumber,this.sortName);
  }

  changePageSize(pageSize: number): void {
   
    this.pageSize = pageSize;
    this.pageNumber = 1; // Reset to first page
    this.totalPages = Math.ceil(this.totalItems / this.pageSize); // Recalculate total pages
    this.loadParticipants(1,this.sortName);
  };

  
  nextPage() {
        if (this.pageNumber < this.totalPages) {
            this.pageNumber++;
            this.loadParticipants(this.pageNumber,this.sortName);
        }
  }

  previousPage() {
        if (this.pageNumber > 1) {
            this.pageNumber--;
            this.loadParticipants(this.pageNumber,this.sortName);

        }
  }

  toggleSort() {
      if (this.sortName == 'default') {
        this.sortName = 'asc';
      } 
      else if (this.sortName == 'asc') {
        this.sortName = 'desc'
      }
      else{
        
        this.sortName = 'default';
      }
  
      this.loadParticipants(this.pageNumber,this.sortName)
  }
}
