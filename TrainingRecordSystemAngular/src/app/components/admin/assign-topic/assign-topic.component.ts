import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { AssignTrainingTopic } from 'src/app/models/assign-training-topic.model';
import { Topic } from 'src/app/models/topic.model';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { UserDetails } from 'src/app/models/userDetails.model';
import { AdminService } from 'src/app/services/admin.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { TopicService } from 'src/app/services/topic.service';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-assign-topic',
  templateUrl: './assign-topic.component.html',
  styleUrls: ['./assign-topic.component.css']
})
export class AssignTopicComponent {
  topics: Topic[] | null = []
  assignedTopics: TrainingTopic[] | null = null;
  userDetails: UserDetails = {
    userId: 0,
    loginId: '',
    firstName: '',
    lastName: '',
    email: '',
    role: 0,
    jobId: 0
  }
  loading: boolean = false;
  assignTrainingTopic: AssignTrainingTopic = {
    userId: 0,
    topicId: 0
  }

  constructor(
    private adminService: AdminService, 
    private authService: AuthService, 
    private topicService: TopicService, 
    private trainerService: TrainerService,
    private router: Router, private route: ActivatedRoute){
    
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.assignTrainingTopic.userId = params['id'];
      this.loadUserDetails(this.assignTrainingTopic.userId);
    })
  }

  loadUserDetails(userId1: number): void {
    this.authService.getUserDetailsByUserId(userId1).subscribe({
      next: (response: ApiResponse<UserDetails>) => {
        if(response.success) {
          this.userDetails = response.data;
          this.loadTopics(this.userDetails.jobId);
          this.loadAssignedTopics(this.userDetails.userId);
        }
        else {
          console.error('Failed to fetch user details: ', response.message);
        }
      },
      error: (err) => {
        console.error('Error fetching user details ', err.error.message);
      }
    })
  }

  loadAssignedTopics(userId: number): void {
    this.loading = true;
    this.trainerService.getAllTrainingTopicbyTrainerId(userId, 1, 10).subscribe({
      next: (response: ApiResponse<TrainingTopic[]>) => {
        if(response.success) {
          this.assignedTopics = response.data;
        }
        else {
          this.assignedTopics = [];
          console.error('Failed to fetch topics: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.assignedTopics = [];
        console.error('Error fetching topics: ', err.error.message);
        this.loading = false;
      }
    })
  }

  loadTopics(jobId: number): void {
    this.loading = true;
    this.topicService.getTopicsByJobId(jobId).subscribe({
      next: (response: ApiResponse<Topic[]>) => {
        if(response.success) {
          this.topics = response.data;
        }
        else {
          console.error('Failed to fetch topics: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.topics = null;
        console.error('Error fetching topics: ', err.error.message);
        this.loading = false;
      }
    })
  }

  assignTopicToTrainer(): void {
    this.loading = true;
    this.adminService.assignTopicToTrainer(this.assignTrainingTopic).subscribe({
      next: (response: ApiResponse<string>) => {
        if(response.success) {
          alert(response.message);
          this.loadAssignedTopics(this.userDetails.userId)
          // this.router.navigate(['/trainer-list']);
        }
        else {
          alert('Failed to assign topic: ' + response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        alert('Failed to assign topic: ' + err.error.message);
        this.loading = false;
      }
    })
  }

  confirmUnassign(topicId: number): void {
    let topic = this.topics?.find(c => c.topicId == topicId)!.topicName
    if(confirm('Are you sure you want to unassign this topic (' + topic + ')?')) {
      let userId = this.userDetails.userId;
      this.unassignTopic(userId, topicId);
    }
  }

  unassignTopic(userId: number, topicId: number): void {
    this.loading = true;
    this.adminService.unassignTopic(userId, topicId).subscribe({
      next: (response: ApiResponse<string>) => {
        if(response.success){
          alert(response.message);
          this.loadAssignedTopics(this.userDetails.userId);
        }
        else {
          alert('Failed to unassign topic: ' + response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        alert('Failed to unassign topic: ' + err.error.message);
        this.loading = false;
      }
    })
  }
}
