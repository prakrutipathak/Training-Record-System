import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-trainingtopic-list',
  templateUrl: './trainingtopic-list.component.html',
  styleUrls: ['./trainingtopic-list.component.css']
})
export class TrainingtopicListComponent implements OnInit {

  trainingTopics: TrainingTopic[] | undefined | null;
  loading: boolean = false;

  userId: number = 0;
  userIdString : string | null | undefined ='';

  //pagination
  pageNumber: number = 1;
  pageSize: number = 4;
  totalItems: number = 0;
  totalPages: number = 0;

  constructor(private trainerService: TrainerService, private router: Router,private authService:  AuthService, private cdr: ChangeDetectorRef) { }


  ngOnInit(): void {
    this.authService.getUserId().subscribe((userIdString: string | null | undefined) => {
      this.userIdString = userIdString;
      if (userIdString != null && userIdString != undefined) {
        this.userId = Number(userIdString);
      }
      this.cdr.detectChanges();  //it code trigger change detection automatically
    });
    
    this.loadTopics(this.userId, this.pageNumber)
  }

  loadTopicCount(userId: number): void {
    this.trainerService.totalCountofTrainingTopicbyTrainerId(userId).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = Number(response.data);
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);

        } else {
          console.error('Failed to fetch participants count', response.message);

        }
        this.loading = false;
      },
      error: (err) => {
        this.trainingTopics = [];
        console.error('Error featching participants count', err.error);
        this.loading = false;
      },
    })
  }

  loadTopics(userId: number, pageNumber: number): void {
    this.loading = true;
    this.pageNumber = pageNumber;
    this.loadTopicCount(this.userId);
    this.trainerService.getAllTrainingTopicbyTrainerId(userId, pageNumber, this.pageSize).subscribe({
      next: (response: ApiResponse<TrainingTopic[]>) => {
        if (response.success) {
          this.trainingTopics = response.data;
        }
        else {
          console.error('Failed to participants trainer', response.message);

        }
        this.loading = false;
      }, error: (error) => {
        this.trainingTopics = null;
        console.error('Error participants trainer', error);
        this.loading = false;
      }
    });
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadTopics(this.userId, pageNumber)

  }

  changePageSize(pageSize: number): void {

    this.pageSize = pageSize;
    this.pageNumber = 1; // Reset to first page
    this.totalPages = Math.ceil(this.totalItems / this.pageSize); // Recalculate total pages
    this.loadTopics(this.userId, this.pageNumber)

  };


  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadTopics(this.userId, this.pageNumber)

    }
  }

  previousPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadTopics(this.userId, this.pageNumber)

    }
  }

}
