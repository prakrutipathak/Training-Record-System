import { ParticipantsTopic } from "./participants-topic.model";
import { Participants } from "./participants.model";

export interface Nominate{
    nomiationId : string;
    modePreference: string;
  topicId: string;
  topic:ParticipantsTopic;
  participantId : number,
  participate:Participants
}