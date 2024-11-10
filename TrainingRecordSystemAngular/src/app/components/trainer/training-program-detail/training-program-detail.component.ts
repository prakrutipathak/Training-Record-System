import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AddTrainingProgramDetail } from 'src/app/models/add-training-program-detail.model';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-training-program-detail',
  templateUrl: './training-program-detail.component.html',
  styleUrls: ['./training-program-detail.component.css']
})
export class TrainingProgramDetailComponent {
  loading: boolean = false;
  duration: string = '0 hours';
  trainingProgramDetail: AddTrainingProgramDetail = {
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

  constructor(private trainerService: TrainerService, private formBuilder: FormBuilder, private router: Router, private route: ActivatedRoute) {

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

    this.route.params.subscribe((params) => {
      this.trainingProgramDetail.trainerTopicId = params['id'];
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

  onSubmit(): void {
    if (this.programDetailForm.valid) {
      this.trainingProgramDetail.startDate = this.programDetailForm.get('startDate')?.value;
      this.trainingProgramDetail.endDate = this.programDetailForm.get('endDate')?.value;
      this.trainingProgramDetail.startTime = this.programDetailForm.get('startTime')?.value + ':00:00';
      this.trainingProgramDetail.endTime = this.programDetailForm.get('endTime')?.value + ':00:00';
      this.trainingProgramDetail.modePreference = this.programDetailForm.get('modePreference')?.value;
      this.trainingProgramDetail.targetAudience = this.programDetailForm.get('targetAudience')?.value;
      this.trainerService.addTrainingProgramDetail(this.trainingProgramDetail).subscribe({
        next: (response) => {
          if (response.success) {
            alert('Training program details have been added')
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
