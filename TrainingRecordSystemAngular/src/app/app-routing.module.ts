import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { AddTrainerComponent } from './components/admin/add-trainer/add-trainer.component';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { TrainerListComponent } from './components/admin/trainer-list/trainer-list.component';
import { ParticipantsListComponent } from './components/trainer/participants-list/participants-list.component';
import { adminGuard } from './guards/admin.guard';
import { TrainingtopicListComponent } from './components/trainer/trainingtopic-list/trainingtopic-list.component';
import { UpcomingtrainingprogramComponent } from './components/manager/upcomingtrainingprogram/upcomingtrainingprogram.component';
import { AddparticipateComponent } from './components/manager/addparticipate/addparticipate.component';
import { NominateparticipantComponent } from './components/manager/nominateparticipant/nominateparticipant.component';
import { ParticipantComponent } from './components/manager/participant/participant.component';
import { AssignTopicComponent } from './components/admin/assign-topic/assign-topic.component';
import { TrainingProgramDetailComponent } from './components/trainer/training-program-detail/training-program-detail.component';
import { MonthlyReportComponent } from './components/admin/monthly-report/monthly-report.component';
import { DateRangeReportComponent } from './components/admin/date-range-report/date-range-report.component';
import { trainerGuard } from './guards/trainer.guard';
import { managerGuard } from './guards/manager.guard';
import { authGuard } from './guards/auth.guard';
import { ProgramDetailsComponent } from './components/trainer/program-details/program-details.component';
import { UpdateTrainingProgramDetailsComponent } from './components/trainer/update-training-program-details/update-training-program-details.component';

const routes: Routes = [

  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'privacy', component: PrivacyComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'changePassword', component: ChangePasswordComponent ,canActivate: [authGuard] },
  //admin
  { path: 'trainer-list', component: TrainerListComponent, canActivate: [adminGuard] },
  { path: 'addtrainer', component: AddTrainerComponent, canActivate: [adminGuard] },
  { path: 'monthly-report', component: MonthlyReportComponent , canActivate: [adminGuard]},
    { path: 'Date-range-report', component: DateRangeReportComponent, canActivate: [adminGuard] },
    { path: 'assign-topic/:id', component: AssignTopicComponent, canActivate: [adminGuard] },
  //trainer
  { path: 'training-topics', component: TrainingtopicListComponent, canActivate: [trainerGuard] },
  { path: 'all-participants', component: ParticipantsListComponent, canActivate: [trainerGuard] },
  { path: 'add-training-program-detail/:id', component: TrainingProgramDetailComponent, canActivate: [trainerGuard]},
  { path: 'program-training/:id', component: ProgramDetailsComponent, canActivate: [trainerGuard]},
  { path: 'update-training-program-detail/:id', component:UpdateTrainingProgramDetailsComponent, canActivate: [trainerGuard]},
  //manager
  { path: 'participantsByManager', component: ParticipantComponent, canActivate: [managerGuard] },
  { path: 'upcomingtrainingTopic', component: UpcomingtrainingprogramComponent , canActivate: [managerGuard]},
  { path: 'addParticipant', component: AddparticipateComponent , canActivate: [managerGuard]},
  { path: 'nominateParticipant/:id', component: NominateparticipantComponent , canActivate: [managerGuard]},
  
 

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

