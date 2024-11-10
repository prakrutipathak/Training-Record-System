import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TrainingProgramDetails } from 'src/app/models/training-program-details.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { TrainerService } from 'src/app/services/trainer.service';

@Component({
  selector: 'app-program-details',
  templateUrl: './program-details.component.html',
  styleUrls: ['./program-details.component.css']
})
export class ProgramDetailsComponent implements OnInit {

  loading : boolean = false;
  programData : TrainingProgramDetails = {
    trainerProgramDetailId: 0,
    startDate: new Date,
    endDate: new Date,
    startTime: '',
    endTime: '', 
    duration : 0, 
    modePreference:  '',
    targetAudience :  '',
    trainerTopicId :  0,
    trainerTopic : {
        trainerTopicId:0,
        userId:0,
        topicId:0,
        jobId :0
      }
  };
  userId !: number;
  userIdString !: string | undefined | null;
  topicId !: number;

  constructor(
    private trainerService : TrainerService,
    private authService : AuthService,
    private router : Router,
    private route : ActivatedRoute,

  ) {}
  ngOnInit(): void {
    this.authService.getUserId().subscribe((userIdString: string | null | undefined) => {
      this.userIdString = userIdString;
      if (userIdString != null && userIdString != undefined) {
          this.userId = Number(userIdString);
      }
      // this.cdr.detectChanges();  //it code trigger change detection automatically
    });

    this.route.params.subscribe((params) => {
      this.topicId = params['id'];
  })

  this.loadProgramDetails();
  };


  loadProgramDetails(){
    this.loading = true;
this.trainerService.getTrainingProgramDetails(this.userId,this.topicId).subscribe({
  next : (response) =>{
    if(response.success){
      this.programData = response.data;
      console.log(this.programData);

    }else{
      console.error('Failed to fetch program details: ', response.message);
    }
    this.loading=false;
  },
  error : (err) =>{
        console.error('Error fetching program details: ', err.error.message);
        this.loading =false;
  }
})
  }

}
