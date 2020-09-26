using System;
using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class TrackWorkflowRunner : IWorkflowRunner
    {
        public void Run(Status status, IList<IITTrack> tracksToFix, IEnumerable<Workflow>? workflows, string? inputFilePath = null)
        {
            if (status == null) throw new ArgumentNullException(nameof(status));
            if (tracksToFix == null) throw new ArgumentNullException(nameof(tracksToFix));
            if (workflows == null) throw new ArgumentNullException(nameof(workflows));

            status.Update(0, tracksToFix.Count, "Running track workflows...");

            foreach (var track in tracksToFix)
            {
                if (workflows.Any(workflow => workflow.Name == WorkflowName.SetAlbumNames))
                {
                    if (string.IsNullOrWhiteSpace(track.Album))
                    {
                        track.Album = track.Name;
                    }
                }

                if (workflows.Any(workflow => workflow.Name == WorkflowName.FindAndReplace))
                {
                    var findAndReplace = workflows.First(item => item.Name == WorkflowName.FindAndReplace);
                    if (!string.IsNullOrEmpty(findAndReplace.OldValue))
                    {
                        track.Name = track.Name.Replace(findAndReplace.OldValue, findAndReplace.NewValue);
                    }
                }

                if (workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (workflows.Any(workflow => workflow.Name == WorkflowName.FixGratefulDeadTracks))
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

                status.ItemsProcessed++;
            }
        }
    }
}
