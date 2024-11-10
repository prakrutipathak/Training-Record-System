import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { UpcomingTraining } from '../models/upcomingtraining.model';
import { AddParticipants } from '../models/addParticipate.model';
import { NominateParticipants } from '../models/nominatepartipate.model';
import { Participants } from '../models/participants.model';
import { ModeOfPrefrence } from '../models/mode-of-prefrence.model';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  private apiUrl='http://localhost:5038/api/Manager/';

  constructor(private http:HttpClient) { }

  loadTrainingProgram(jobId:number|null):Observable<ApiResponse<UpcomingTraining[]>>{
    if(jobId!=null){
    return this.http.get<ApiResponse<UpcomingTraining[]>>(this.apiUrl+'UpcomingTrainingProgram?jobId='+jobId);
    }
    else{
      return this.http.get<ApiResponse<UpcomingTraining[]>>(this.apiUrl+'UpcomingTrainingProgram');
    }
  }
  addParticipate(participant: AddParticipants): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl+'AddParticipate', participant);
  }
  nominateParticipate(participant:NominateParticipants):Observable<ApiResponse<string>>{
    return this.http.post<ApiResponse<string>>(this.apiUrl+"NominateParticipate",participant);
    }
    getAllPartiipantByManagerId(managerId: number): Observable<ApiResponse<Participants[]>> {
        return this.http.get<ApiResponse<Participants[]>>(this.apiUrl + 'GetParticipantByManagerId/' + managerId)
    }
    getParticipantById(id: number): Observable<ApiResponse<Participants>> {
        return this.http.get<ApiResponse<Participants>>(this.apiUrl + 'GetParticipantById/' + id)
    }

    getModeofTrainingByTopicId(topicId: number, userId:number): Observable<ApiResponse<string>> {
      return this.http.get<ApiResponse<string>>(this.apiUrl + 'GetModeofTrainingByTopicId?userId=' + userId+'&topicId='+topicId)
  }
}
