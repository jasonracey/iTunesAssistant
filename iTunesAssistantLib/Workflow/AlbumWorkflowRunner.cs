using System;
using System.Linq;

namespace iTunesAssistantLib
{
    public class AlbumWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowRunnerInfo workflowRunnerInfo, ref Status status)
        {
            if (workflowRunnerInfo == null) throw new ArgumentNullException(nameof(workflowRunnerInfo));
            if (workflowRunnerInfo.Tracks == null) throw new ArgumentNullException(nameof(workflowRunnerInfo.Tracks));
            if (workflowRunnerInfo.Workflows == null) throw new ArgumentNullException(nameof(workflowRunnerInfo.Workflows));

            var albums = AlbumBuilder.BuildAlbums(workflowRunnerInfo.Tracks, ref status);

            status = Status.Create(albums.Count, "Running album workflows...");

            foreach (var tracks in albums.Select(album => album.Value))
            {
                // iTunes deletes TrackCount if TrackNumber is set to a higher value, so set number before count
                // in case any old track numbers are higher than new track count.
                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNumbers))
                {
                    TrackNumberFixer.FixTrackNumbers(tracks);
                }

                if (workflowRunnerInfo.Workflows.Any(workflow => workflow.Name == WorkflowName.FixCountOfTracksOnAlbum))
                {
                    TrackCountFixer.FixTrackCounts(tracks);
                }

                status.ItemProcessed();
            }
        }
    }
}
