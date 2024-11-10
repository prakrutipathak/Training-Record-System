import { Job } from "./job.model";

export interface Trainer {
  userId: number,
  loginId: string,
  firstName: string,
  lastName: string,
  email: string,
  role: number,
  loginbit: boolean,
  jobId: number,
  job: Job
}
