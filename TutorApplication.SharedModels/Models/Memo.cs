namespace TutorApplication.SharedModels.Models
{
	public class Memo
	{
		public string Type { get; set; } = "";
		public string BookInfo { get; set; } = "";
		public string Time { get; set; } = "";
		public string Date { get; set; } = "";
	}
}


/*
 export interface DailyClasses {
    dailyClasses : DailyClassResponse []
}
export interface DailyClassResponse {
    timestamp:string
    classes : ClassResponse []
}
export interface ClassResponse {
    courseId:string
    timestamp: number
    courseTitle: string
    memoInfo: string
    tutorName: string
}
 
 */