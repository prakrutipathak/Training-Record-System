export interface UpdateTrainingProgramDetail {
    trainerProgramDetailId: number,
    trainerTopicId: number,
    startDate: Date,
    endDate: Date,
    startTime: string,
    endTime: string,
    modePreference: string,
    targetAudience: string,
}