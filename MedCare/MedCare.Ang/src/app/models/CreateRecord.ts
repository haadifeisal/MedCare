
export interface CreateRecord {
  id: number,
  doctorId: number,
  pulse: number,
  stressLevel: number,
  oxygenLevel: number,
  description: string,
  dateCreated: Date
};
