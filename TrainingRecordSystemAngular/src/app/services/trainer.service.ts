import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { Participants } from '../models/participants.model';
import { TrainingTopic } from '../models/trainingTopic.model';
import { AddTrainingProgramDetail } from '../models/add-training-program-detail.model';
import { TrainingProgramDetails } from '../models/training-program-details.model';
import { Nominate } from '../models/nominate.model';
import { UpdateTrainingProgramDetail } from '../models/update-training-program-detail.model';

@Injectable({
  providedIn: 'root'
})
export class TrainerService {
  private apiUrl = 'http://localhost:5038/api/Trainer/';

  constructor(private http: HttpClient) { }

  getAllParticipantsWithPagination(page: number, pageSize: number, sort_name: string): Observable<ApiResponse<Nominate[]>> {
    return this.http.get<ApiResponse<Nominate[]>>(this.apiUrl + 'GetAllParticipantsByPaginationSorting?page=' + page + '&pageSize=' + pageSize + '&sort_name=' + sort_name)
  }

  getTotalParticipantsCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + 'TotalParticipantsCount');
  }

  getAllTrainingTopicbyTrainerId(userId: number, page: number, pageSize: number): Observable<ApiResponse<TrainingTopic[]>> {
    return this.http.get<ApiResponse<TrainingTopic[]>>(this.apiUrl + 'GetAllTrainingTopicbyTrainerId/' + userId + '?page=' + page + '&pageSize=' + pageSize)
  }

  totalCountofTrainingTopicbyTrainerId(userId: number): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + 'TotalCountofTrainingTopicbyTrainerId/' + userId);
  }

  addTrainingProgramDetail(detail: AddTrainingProgramDetail): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'AddTrainingProgramDetail', detail)
  }

  getTrainingProgramDetails(userId : number, topicId : number): Observable<ApiResponse<TrainingProgramDetails>> {
    return this.http.get<ApiResponse<TrainingProgramDetails>>(this.apiUrl + 'GetAllTrainingProgramDetails?userId=' + userId + '&topicId=' + topicId)
  }

  updateTrainingProgramDetails(details: UpdateTrainingProgramDetail): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl + 'UpdateTrainingProgramDetail', details);
  }
}
