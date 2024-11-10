import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AddParticipants } from 'src/app/models/addParticipate.model';
import { Job } from 'src/app/models/job.model';
import { AdminService } from 'src/app/services/admin.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { ManagerService } from 'src/app/services/manager.service';

@Component({
  selector: 'app-addparticipate',
  templateUrl: './addparticipate.component.html',
  styleUrls: ['./addparticipate.component.css']
})
export class AddparticipateComponent  implements OnInit{
  loading: boolean = false;
  allJobs : Job[] = [];
  userId: string | null | undefined;
  addParticipateForm!: FormGroup;
  constructor(private authService: AuthService, private cdr: ChangeDetectorRef, private fb: FormBuilder, private router: Router, 
    private managerService: ManagerService,private adminService:AdminService ) { }
  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId:string |null|undefined)=>{
      this.userId=userId
          });
    this.addParticipateForm=this.fb.group({
      jobId: [0,[Validators.required,this.jobValidator]],
      firstName: ['',[Validators.required,Validators.minLength(2)]],
      lastName: ['',[Validators.required,Validators.minLength(2)]],
      email: ['',[Validators.required, Validators.email]],
    });
    this.getAllJobs();
  }
  get formControls() {
    return this.addParticipateForm.controls;
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
  jobValidator(controls: any) {
    return controls.value == '' ? { invalidJob: true } : null;
  }
  onSubmit() {
    this.loading = true;
    if (this.addParticipateForm.valid) {
      const addParticipateData: AddParticipants = {
        firstName: this.addParticipateForm.value.firstName,
        lastName: this.addParticipateForm.value.lastName,
        email: this.addParticipateForm.value.email,
        userId: Number(this.userId),
        jobId: this.addParticipateForm.value.jobId
      };

      console.log(this.addParticipateForm.value);
      console.log(addParticipateData);
      this.managerService.addParticipate(addParticipateData)
        .subscribe({
          next: (response) => {
            if (response.success) {
              console.log("Participant Added", response);
              this.router.navigate(['/participantsByManager']);
            }
            else {
              alert(response.message);
            }
            this.loading = false;
          },
          error: (err) => {
            this.loading = false;
            alert(err.error.message);
          },
          complete: () => {
            this.loading = false;
            console.log('Completed');
          }
        });
    }
  }
}
