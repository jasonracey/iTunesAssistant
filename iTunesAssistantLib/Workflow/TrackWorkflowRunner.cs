using System;
using System.Linq;

namespace iTunesAssistantLib
{
    public class TrackWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowData workflowData, ref Status status)
        {
            if (workflowData == null) throw new ArgumentNullException(nameof(workflowData));
            if (workflowData.Tracks == null) throw new ArgumentNullException(nameof(workflowData.Tracks));
            if (workflowData.Workflows == null) throw new ArgumentNullException(nameof(workflowData.Workflows));

            status = Status.Create(workflowData.Tracks.Count, "Running track workflows...");

            foreach (var track in workflowData.Tracks)
            {
                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.SetAlbumNames))
                {
                    if (string.IsNullOrWhiteSpace(track.Album))
                    {
                        track.Album = track.Name;
                    }
                }

                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.FindAndReplace))
                {
                    var findAndReplace = workflowData.Workflows.First(item => item.Name == WorkflowName.FindAndReplace);
                    if (!string.IsNullOrEmpty(findAndReplace.OldValue))
                    {
                        track.Name = track.Name.Replace(findAndReplace.OldValue, findAndReplace.NewValue);
                    }
                }

                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.FixGratefulDeadTracks))
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
