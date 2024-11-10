import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';

import { AddTrainerComponent } from './components/admin/add-trainer/add-trainer.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TrainerListComponent } from './components/admin/trainer-list/trainer-list.component';
import { ParticipantsListComponent } from './components/trainer/participants-list/participants-list.component';
import { TrainingtopicListComponent } from './components/trainer/trainingtopic-list/trainingtopic-list.component';
import { UpcomingtrainingprogramComponent } from './components/manager/upcomingtrainingprogram/upcomingtrainingprogram.component';
import { AddparticipateComponent } from './components/manager/addparticipate/addparticipate.component';
import { NominateparticipantComponent } from './components/manager/nominateparticipant/nominateparticipant.component';
import { ParticipantComponent } from './components/manager/participant/participant.component';
import { AssignTopicComponent } from './components/admin/assign-topic/assign-topic.component';
import { TrainingProgramDetailComponent } from './components/trainer/training-program-detail/training-program-detail.component';
import { MonthlyReportComponent } from './components/admin/monthly-report/monthly-report.component';
import { DateRangeReportComponent } from './components/admin/date-range-report/date-range-report.component';
import { TitleCasePipe } from '@angular/common';
import { ProgramDetailsComponent } from './components/trainer/program-details/program-details.component';
import { UpdateTrainingProgramDetailsComponent } from './components/trainer/update-training-program-details/update-training-program-details.component';


@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        FooterComponent,
        HomeComponent,
        PrivacyComponent,
        AddTrainerComponent,
        SigninComponent,
        ChangePasswordComponent,
        TrainerListComponent,
        ParticipantsListComponent,
        TrainingtopicListComponent,
        UpcomingtrainingprogramComponent,
        AddparticipateComponent,
        NominateparticipantComponent,
        ParticipantComponent,
        AssignTopicComponent,
        TrainingProgramDetailComponent,
        MonthlyReportComponent,
        DateRangeReportComponent,
        ProgramDetailsComponent,
        UpdateTrainingProgramDetailsComponent,
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        NgbModule
    ],
    providers: [
        TitleCasePipe
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
