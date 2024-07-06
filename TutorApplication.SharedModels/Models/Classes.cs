using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TutorApplication.SharedModels.Models
{
	public class DailyClasses
	{
		public List<DailyClassResponse> DayClasses {  get; set; } 
	}


	public class DailyClassResponse
	{
		public string Timestamp { get; set; }
		public List<ClassResponse> Classes { get; set; }
	}
	public class ClassResponse
	{
		public string CourseId { get; set; }
		public string Timestamp { get; set; }
		public string CourseTitle { get; set; }
		public string MemoInfo { get; set; }
		public string TutorName { get; set; }
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