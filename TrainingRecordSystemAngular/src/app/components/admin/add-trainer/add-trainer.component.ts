import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Job } from 'src/app/models/job.model';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-add-trainer',
  templateUrl: './add-trainer.component.html',
  styleUrls: ['./add-trainer.component.css']
})
export class AddTrainerComponent {

  loading: boolean = false;
  trainerForm!: FormGroup;
  allJobs : Job[] = [];
  
    constructor(
    private adminService: AdminService, 
    private fb: FormBuilder,
    private router : Router
  ){ }

  ngOnInit(): void {
    this.trainerForm = this.fb.group({
      loginId: ['',[Validators.required,Validators.minLength(2)]],
      firstName: ['',[Validators.required,Validators.minLength(2)]],
      lastName: ['',[Validators.required,Validators.minLength(2)]],
      email: ['',[Validators.required,Validators.email]],
      jobId: [0,[Validators.required,this.jobValidator]]
    });

    this.getAllJobs();

  };

  get formControls() {
    return this.trainerForm.controls;
  }
  jobValidator(controls: any) {
    return controls.value == '' ? { invalidJob: true } : null;
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

  onSubmit(){
    if(this.trainerForm.valid){
      this.adminService.addTrainer(this.trainerForm.value).subscribe({
        next:(response) =>{
          if(response.success){
            this.router.navigate(['/trainer-list'])
          }
          else{
            alert(response.message);
          }
        },
        error:(err) =>{
          console.error('Failed to add trainer',err.error.message);
          alert(err.error.message);
        },
        complete:() =>{
          console.log('completed');
        }
      });
    }
  }
}