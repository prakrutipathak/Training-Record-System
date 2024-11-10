import { Job } from "./job.model";
import { ParticipantsTopic } from "./participants-topic.model";
import { ParticipantsUser } from "./participants-user.model";

export interface Participants{
    participantId : number,
    firstName:string;
    lastName:string;
    email:string;
    userId:number;
    jobId:number;
    user:ParticipantsUser;
    job:Job;
  
}