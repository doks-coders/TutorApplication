using Microsoft.AspNetCore.SignalR;
using TutorApplication.SharedModels.Models;

namespace TutorApplication.ApplicationCore.SignalR.Persistence
{
	public class VideoGroupChats
	{
		public Dictionary<string, List<VideoState>> VideoGroups = new();


		public Task<List<VideoState>?> GetVideoGroup(string videoGroupName)
		{
			lock (VideoGroups)
			{
				var group = VideoGroups.GetValueOrDefault(videoGroupName);
				return Task.FromResult(group);
			}

			throw new HubException("No Group Exists");
		}

		public Task<bool> IsGroupVideoCallActive(string videoGroupName)
		{
			if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
			{
				return Task.FromResult(true);
			}
			return Task.FromResult(false);
		}

		public Task CreateVideoGroup(string videoGroupName, VideoState adminState)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					VideoGroups.Remove(videoGroupName);
				}
				VideoGroups.Add(videoGroupName, new List<VideoState>() { adminState });

			}
			return Task.CompletedTask;
		}

		public Task RemoveVideoGroup(string videoGroupName)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					VideoGroups.Remove(videoGroupName);
				}
			}

			return Task.CompletedTask;
		}

		public Task JoinVideoGroup(string videoGroupName, VideoState memberState)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					if (VideoGroups[videoGroupName].Any(u => u.id == memberState.id))
					{
						var index = VideoGroups[videoGroupName].FindIndex(u => u.id == memberState.id);
						VideoGroups[videoGroupName][index] = memberState;
					}
					else
					{
						VideoGroups[videoGroupName].Add(memberState);
					}

				}
			}
			return Task.CompletedTask;
		}


		public Task CommandAction(string videoGroupName, CommandPassed command)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					if (VideoGroups[videoGroupName].Any(u => u.id == command.elementId))
					{
						var index = VideoGroups[videoGroupName].FindIndex(u => u.id == command.elementId);

						var videoState = VideoGroups[videoGroupName][index];
						if (command.command == "audio-toggle")
						{
							videoState.playAudio = !videoState.playAudio;
						}
						if (command.command == "video-toggle")
						{
							videoState.showVideo = !videoState.showVideo;
						}

						VideoGroups[videoGroupName][index] = videoState;
					}

				}
			}
			return Task.CompletedTask;
		}


		public Task LeaveVideoGroup(string videoGroupName, string elementId)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					if (VideoGroups[videoGroupName].Any(u => u.id == elementId))
					{
						var index = VideoGroups[videoGroupName].FindIndex(u => u.id == elementId);

						VideoGroups[videoGroupName].RemoveAt(index);
					}

				}
			}
			return Task.CompletedTask;
		}

		public Task LeaveVideoGroup(string videoGroupName, VideoState memberState)
		{
			lock (VideoGroups)
			{
				if (VideoGroups.GetValueOrDefault(videoGroupName) != null)
				{
					VideoGroups[videoGroupName].Remove(memberState);
				}

			}
			return Task.CompletedTask;
		}




	}



}

/**
 
interface VideoState {
   id:string,
  showVideo:boolean,
  playAudio:boolean,
  peerId:string
}




 */
