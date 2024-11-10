import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { Participants } from 'src/app/models/participants.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { ManagerService } from 'src/app/services/manager.service';

@Component({
  selector: 'app-participant',
  templateUrl: './participant.component.html',
  styleUrls: ['./participant.component.css']
})
export class ParticipantComponent {

  loading : boolean = false;
  participants: Participants[] =  [];
  managerId !: number;
  userIdString : string | null | undefined;

  constructor(
    private managerService : ManagerService,
    private router : Router,
    private authService : AuthService,
    private cdr : ChangeDetectorRef
  ){}

  ngOnInit(): void {
    this.authService.getUserId().subscribe((userIdString: string | null | undefined) => {
      this.userIdString = userIdString;
      if (userIdString != null && userIdString != undefined) {
        this.managerId = Number(userIdString);
      }
      this.cdr.detectChanges();  //it code trigger change detection automatically
    });
    this.loadParticipant(this.managerId);
  }

  loadParticipant(managerId : number) : void{
    this.managerService.getAllPartiipantByManagerId(managerId).subscribe({
      next : (response) =>{
        if(response.success){
          this.participants = response.data;
        }
        else{
          console.error('Failed to participants trainer',response.message);
          
        }
        this.loading=false;
      },
      error:(error)=>{
        this.participants =[];
        console.error('Error fetching participants',error);
        this.loading=false;
      }
    })
  }


}

