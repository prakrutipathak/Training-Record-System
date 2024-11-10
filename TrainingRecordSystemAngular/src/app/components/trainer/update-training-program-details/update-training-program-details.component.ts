import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TrainingProgramDetails } from 'src/app/models/training-program-details.model';
import { UpdateTrainingProgramDetail } from 'src/app/models/update-training-program-detail.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-update-training-program-details',
  templateUrl: './update-training-program-details.component.html',
  styleUrls: ['./update-training-program-details.component.css'],
  providers: [DatePipe]
})
export class UpdateTrainingProgramDetailsComponent {
  loading: boolean = false;
  duration: string = '0 hours';
  userId!: number;
  topicId!: number;
  programData: TrainingProgramDetails = {
    trainerProgramDetailId: 0,
    startDate: new Date,
    endDate: new Date,
    startTime: '',
    endTime: '',
    duration: 0,
    modePreference: '',
    targetAudience: '',
    trainerTopicId: 0,
    trainerTopic: {
      trainerTopicId: 0,
      userId: 0,
      topicId: 0,
      jobId: 0
    }
  }
  trainingProgramDetail: UpdateTrainingProgramDetail = {
    trainerProgramDetailId: 0,
    trainerTopicId: 0,
    startDate: new Date(1, 1, 2000),
    endDate: new Date(1, 1, 2000),
    startTime: '',
    endTime: '',
    modePreference: '',
    targetAudience: ''
  }
  startTime: number = 0;
  endTime: number = 0;
  programDetailForm!: FormGroup;
  timeList: {
    value: number,
    name: string
  }[] = [
    { value: 10, name: '10:00' },
    { value: 11, name: '11:00' },
    { value: 12, name: '12:00' },
    { value: 13, name: '13:00' },
    { value: 14, name: '14:00' },
    { value: 15, name: '15:00' },
    { value: 16, name: '16:00' },
    { value: 17, name: '17:00' },
    { value: 18, name: '18:00' },
    { value: 19, name: '19:00' },
  ]

  constructor(
    private trainerService: TrainerService, 
    private authService: AuthService, 
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private datePipe: DatePipe
  ){
    
  }

  ngOnInit(): void {
    this.programDetailForm = this.formBuilder.group({
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      startTime: [0, [Validators.required,this.dropdownValidator]],
      endTime: [0, [Validators.required,this.dropdownValidator]],
      modePreference: ['', [Validators.required,this.dropdownValidator]],
      targetAudience: [''],
    })

    this.authService.getUserId().subscribe((userIdString: string | null | undefined) => {
      if(userIdString != null && userIdString != undefined) {
        this.userId = Number(userIdString);
      }
    });

    this.route.params.subscribe((params) => {
      this.topicId = params['id'];
      this.loadProgramDetails();
    })
  }

  dropdownValidator(controls: any) {
    return controls.value == '' ? { invaliddropdown: true } : null;
  }

  get formControls() {
    return this.programDetailForm.controls;
  }

  calculateDuration() {
    this.startTime = this.programDetailForm.get('startTime')?.value;
    this.endTime = this.programDetailForm.get('endTime')?.value;

    if(this.startTime > this.endTime) {
      this.duration = '0 hours'
      return;
    }

    if(this.startTime != 0 && this.endTime != 0) {
      this.duration = this.endTime - this.startTime + ' hours';
    }
  }

  maxDate(): string {
    // Get current date in YYYY-MM-DD format
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return `${yyyy}-${mm}-${dd}`;
  }

  loadProgramDetails() {
    this.loading = true;
    this.trainerService.getTrainingProgramDetails(this.userId, this.topicId).subscribe({
      next: (response) => {
        if(response.success) {
          this.programData = response.data;
          this.programDetailForm.setValue({
            startDate: this.datePipe.transform(this.programData.startDate, 'yyyy-MM-dd'),
            endDate: this.datePipe.transform(this.programData.endDate, 'yyyy-MM-dd'),
            startTime: Number(this.programData.startTime.slice(11, 13)),
            endTime: Number(this.programData.endTime.slice(11, 13)),
            modePreference: this.programData.modePreference,
            targetAudience: this.programData.targetAudience,
          })
          this.startTime = Number(this.programData.startTime.slice(11, 13));
          this.endTime = Number(this.programData.endTime.slice(11, 13));
          this.duration = (this.endTime - this.startTime) + ' hours';
          console.log(this.programDetailForm)
        } else {
          console.error('Failed to fetch program details: ', response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching program details: ', err.error.message)
        this.loading = false;
      }
    })
  }

  onSubmit(): void {
    if (this.programDetailForm.valid) {
      this.trainingProgramDetail.trainerProgramDetailId = this.programData.trainerProgramDetailId;
      this.trainingProgramDetail.trainerTopicId = this.programData.trainerTopicId;
      this.trainingProgramDetail.startDate = this.programDetailForm.get('startDate')?.value;
      this.trainingProgramDetail.endDate = this.programDetailForm.get('endDate')?.value;
      this.trainingProgramDetail.startTime = this.programDetailForm.get('startTime')?.value + ':00:00';
      this.trainingProgramDetail.endTime = this.programDetailForm.get('endTime')?.value + ':00:00';
      this.trainingProgramDetail.modePreference = this.programDetailForm.get('modePreference')?.value;
      this.trainingProgramDetail.targetAudience = this.programDetailForm.get('targetAudience')?.value;
      console.log(this.trainingProgramDetail)
      this.trainerService.updateTrainingProgramDetails(this.trainingProgramDetail).subscribe({
        next: (response) => {
          if (response.success) {
            alert('Training program details have been updated')
            this.router.navigate(['/training-topics']);
          } else {
            alert(response.message);
          }
        },
        error: (err) => {
          alert(err.error.message);
        }
      });
    }
  }
}
