namespace TutorApplication.SharedModels.Responses.Messages
{
	public class DisplayMessageContact
	{
		public Guid Id { get; set; }
		public Guid RecieverId { get; set; }
		public Guid CourseGroupId { get; set; }
		public string Email { get; set; }
		public bool IsGroup { get; set; } = false;
		public string DisplayName { get; set; }
		public string ImageUrl { get; set; }
		public string lastTimeAvailable { get; set; }
		public string SubText { get; set; }
		public bool IsOnline { get; set; } = false;
	}
}

/**
 
id:string,
    recieverId:number,
    courseGroupId:number,
    isGroup:false,
    displayName:string,
    imageUrl:string,
    lastTimeAvailable:string,
    subText:string,
    isOnline:boolean

 */
