import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { AddTrainer } from '../models/add-trainer.model';
import { Job } from '../models/job.model';
import { Trainer } from '../models/trainer.model';
import { MonthlyAdminReport } from '../models/monthly-admin-report.model';
import { DaterangeBasedReport } from '../models/daterange-based-report.model';
import { AssignTrainingTopic } from '../models/assign-training-topic.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private apiUrl = "http://localhost:5038/api/Admin/"
  constructor(private http: HttpClient) {

  }
  addTrainer(addtrainer: AddTrainer): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.apiUrl}AddTrainer`, addtrainer);
  }

  getAllJobs(): Observable<ApiResponse<Job[]>> {
    return this.http.get<ApiResponse<Job[]>>(`${this.apiUrl}GetAllJobs`);
  }

  getAllTrainer(): Observable<ApiResponse<Trainer[]>> {
    return this.http.get<ApiResponse<Trainer[]>>(`${this.apiUrl}GetAllTrainer`);
  }

  getTrainerByLoginId(id: string): Observable<ApiResponse<Trainer>> {
    return this.http.get<ApiResponse<Trainer>>(`${this.apiUrl}GetTrainerByLoginId/` + id);
  }

  getAllTrainerWithPagination(pageNumber: number, pageSize: number): Observable<ApiResponse<Trainer[]>> {
    return this.http.get<ApiResponse<Trainer[]>>(this.apiUrl + 'GetAllTrainerByPagination?page=' + pageNumber + '&pageSize=' + pageSize);
  }

  getTotalTrainerCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + 'TotalTrainerCount');
  }

  assignTopicToTrainer(assignTrainingTopic: AssignTrainingTopic): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'AssignTopicToTrainer', assignTrainingTopic);
  }

  monthlyAdminReport(userId: number | null, month: number | null, year: number | null): Observable<ApiResponse<MonthlyAdminReport[]>> {
    if (month == null && year == null) {
      return this.http.get<ApiResponse<MonthlyAdminReport[]>>(`${this.apiUrl}MonthlyAdminReport?userId=${userId}`);
    } else {
      return this.http.get<ApiResponse<MonthlyAdminReport[]>>(`${this.apiUrl}MonthlyAdminReport?userId=${userId}&month=${month}&year=${year}`);
    }
  }

  daterangeBasedReport(jobId: number | null, startDate: string | null, endDate: string | null): Observable<ApiResponse<DaterangeBasedReport[]>> {
    if(startDate == null && endDate == null){
      return this.http.get<ApiResponse<DaterangeBasedReport[]>>(`${this.apiUrl}DaterangeBasedReport?jobId=${jobId}`);
    }
    else{
      return this.http.get<ApiResponse<DaterangeBasedReport[]>>(`${this.apiUrl}DaterangeBasedReport?jobId=${jobId}&startDate=${startDate}&endDate=${endDate}`);
    }
  }

  unassignTopic(userId: number, topicId: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(this.apiUrl + 'UnassignTopic?userId=' + userId + '&topicId=' + topicId);
  }

}
