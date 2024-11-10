import { Time } from "@angular/common"

      export interface TrainingProgramDetails {
      trainerProgramDetailId: number,
      startDate: Date,
      endDate: Date,
      startTime: string
      endTime: string, 
      duration : number, 
      modePreference:  string,
      targetAudience :  string,
      trainerTopicId :  number,
      trainerTopic : {
          trainerTopicId:number
          userId:number,
          topicId:number,
          jobId :number
        }


    }
    
    