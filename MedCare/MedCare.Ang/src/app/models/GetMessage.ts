
export interface GetMessage {
  id:number,
  from: string,
  read: boolean,
  timeSent: Date,
  subject: string,
  content: string
};
