import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { AssignTrainingTopic } from 'src/app/models/assign-training-topic.model';
import { ModeOfPrefrence } from 'src/app/models/mode-of-prefrence.model';
import { Participants } from 'src/app/models/participants.model';
import { Topic } from 'src/app/models/topic.model';
import { LoadTrainingProgramDetail } from 'src/app/models/trainingprogramdetails.model';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { UserDetails } from 'src/app/models/userDetails.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { ManagerService } from 'src/app/services/manager.service';
import { TopicService } from 'src/app/services/topic.service';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-nominateparticipant',
  templateUrl: './nominateparticipant.component.html',
  styleUrls: ['./nominateparticipant.component.css']
})
export class NominateparticipantComponent implements OnInit {

  loading: boolean = false;
  nominationForm!: FormGroup;
  jobId !: number;
  assignTrainingTopic: AssignTrainingTopic = {
    userId: 0,
    topicId: 0,
   
  }
  trainerId:number=0;
  topicId:number= 0;
  defaultModePrefrence: string | null = null;
  managerId !: number;
  userIdString: string | null | undefined;
  assignedTopics: LoadTrainingProgramDetail[] = [];
  assignedTrainer: LoadTrainingProgramDetail[] = [];
    constructor(
    private managerService: ManagerService, 
    private fb: FormBuilder,
    private router: Router,
    private topicService: TopicService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private trainerService: TrainerService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    this.nominationForm = this.fb.group({
      participateId: [0, [Validators.required]],
      modePreference: [''],
      userId:[0],
      topicId: [0, [Validators.required, this.topicValidator]],
      trainerId: [0, [Validators.required, this.topicValidator]]
    });


    this.authService.getUserId().subscribe((userIdString: string | null | undefined) => {
      this.userIdString = userIdString;
      if (userIdString != null && userIdString != undefined) {
        this.nominationForm.patchValue({
          userId: Number(userIdString)
        })
      }
    });

    this.route.params.subscribe((params) => {
      this.assignTrainingTopic.userId = params['id'];
      this.nominationForm.patchValue({
        participateId: this.assignTrainingTopic.userId

      })

      this.loadUserDetails(this.assignTrainingTopic.userId);
    })

  };
  topicValidator(controls: any) {
    return controls.value == '' ? { invalidTopic: true } : null;
  }
  get formControls() {
    return this.nominationForm.controls;
  }

  onSubmit() {
    console.log(this.nominationForm.value);
    if (this.nominationForm.valid) {
    if (this.nominationForm.value.modePreference === '') {
        this.patchModePreference();
      }
  
      this.managerService.nominateParticipate(this.nominationForm.value).subscribe({
        next: (response) => {
          if (response.success) {
            alert("Participant successfully nominated...")
            this.router.navigate(['/participantsByManager'])
          }
          else {
            alert(response.message);
          }
        },
        error: (err) => {
          console.error('Failed to update nomination', err.error.message);
          alert(err.error.message);
        },
        complete: () => {
          console.log('completed');
        }
      });
    }
  }

  onUserChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.assignTrainingTopic.topicId = parseInt(selectElement.value, 10);
    this.loadModeofTrainingByTopicId(this.topicId,this.trainerId);
  }
  onTopicChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.topicId = parseInt(selectElement.value, 10);
    this.getTrainerByTopicId(this.topicId);
  }

  patchModePreference() {
    const modePreferenceControl = this.nominationForm.get('modePreference');
    if (modePreferenceControl && modePreferenceControl.value === '') {
      modePreferenceControl.patchValue(this.defaultModePrefrence);
    }
  }
  

  loadModeofTrainingByTopicId(topicId: number,userId:number): void {
    this.loading = true;
    this.managerService.getModeofTrainingByTopicId(topicId,userId).subscribe({
      next: (response: ApiResponse<string>) => {
        if (response.success) {
          this.defaultModePrefrence = response.data;

          this.patchModePreference();
        }
        else {
          this.defaultModePrefrence = null;
          console.error('Failed to fetch topics: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.defaultModePrefrence = null;
        console.error('Error fecthing topics: ', err);
        this.loading = false;
      }
    })
  }

  loadUserDetails(userId1: number): void {
    this.managerService.getParticipantById(userId1).subscribe({
      next: (response: ApiResponse<Participants>) => {
        if (response.success) {
          this.loadTopics(response.data.jobId);
        }
        else {
          console.error('Failed ot fetch participant details', response.message);
        }
      },
      error: (err) => {
        console.error('Error fetching participant details', err);
      }
    })
  }

  loadTopics(jobId: number): void {
    this.loading = true;
    this.topicService.getTrainerTopicsByJobId(jobId).subscribe({
      next: (response: ApiResponse<LoadTrainingProgramDetail[]>) => {
        if(response.success) {
          this.assignedTopics = response.data;
         
        }
        else {
          console.error('Failed to fetch topics: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
       
        console.error('Error fetching topics:', err);
        this.loading = false;
      }
    })
  }
  getTrainerByTopicId(topicId: number): void {
    this.loading = true;
    this.topicService.getTrainerByTopicId(topicId).subscribe({
      next: (response: ApiResponse<LoadTrainingProgramDetail[]>) => {
        if(response.success) {
          this.assignedTrainer = response.data;
          console.log("trainer:"+this.assignedTrainer)
        
        }
        else {
          console.error('Failed to fetch trainers: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
       
        console.error('Error fetching trainers:', err);
        this.loading = false;
      }
    })
  }

}
