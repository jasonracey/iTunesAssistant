using System;
using System.Linq;

namespace iTunesAssistantLib
{
    public class TrackWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowRunnerInfo workflowRunnerInfo, ref Status status)
        {
            if (workflowRunnerInfo == null) throw new ArgumentNullException(nameof(workflowRunnerInfo));
            if (workflowRunnerInfo.Tracks == null) throw new ArgumentNullException(nameof(workflowRunnerInfo.Tracks));
            if (workflowRunnerInfo.Workflows == null) throw new ArgumentNullException(nameof(workflowRunnerInfo.Workflows));

            status = Status.Create(workflowRunnerInfo.Tracks.Count, "Running track workflows...");

            foreach (var track in workflowRunnerInfo.Tracks)
            {
                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.SetAlbumNames))
                {
                    if (string.IsNullOrWhiteSpace(track.Album))
                    {
                        track.Album = track.Name;
                    }
                }

                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.FindAndReplace))
                {
                    var findAndReplace = workflowRunnerInfo.Workflows.First(item => item.Name == WorkflowName.FindAndReplace);
                    if (!string.IsNullOrEmpty(findAndReplace.OldValue))
                    {
                        track.Name = track.Name.Replace(findAndReplace.OldValue, findAndReplace.NewValue);
                    }
                }

                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.FixGratefulDeadTracks))
                {
                    track.Name = GratefulDeadTrackNameFixer.FixTrackName(track.Name);
                    if (string.IsNullOrWhiteSpace(track.Comment))
                    {
                        throw new iTunesAssistantException("One or more Grateful Dead tracks is missing a comment");
                    }
                    else
                    {
                        track.Comment = track.Comment.Replace("https://archive.org/details/", string.Empty);
                    }
                }

                status.ItemProcessed();
            }
        }
    }
}
