import { Job } from "./job.model";
import { Topic } from "./topic.model";
import { User } from "./user.model";

export interface TrainingTopic {
    trainerTopicId: number,
    userId: number,
    topicId: number,
    jobId: number,
    user: User,
    topic: Topic,
    job: Job,
    isTrainingScheduled : boolean
}

