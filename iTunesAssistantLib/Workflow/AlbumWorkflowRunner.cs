using System;
using System.Linq;

namespace iTunesAssistantLib
{
    public class AlbumWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowData workflowData, ref Status status)
        {
            if (workflowData == null) throw new ArgumentNullException(nameof(workflowData));
            if (workflowData.Tracks == null) throw new ArgumentNullException(nameof(workflowData.Tracks));
            if (workflowData.Workflows == null) throw new ArgumentNullException(nameof(workflowData.Workflows));

            var albums = AlbumBuilder.BuildAlbums(workflowData.Tracks, ref status);

            status = Status.Create(albums.Count, "Running album workflows...");

            foreach (var tracks in albums.Select(album => album.Value))
            {
                // iTunes doesn't allow TrackCount to be set to a value higher than TrackNumber, so set number 
                // before count in case any old track numbers are higher than new track count.
                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNumbers))
                {
                    TrackNumberFixer.FixTrackNumbers(tracks);
                }

                if (workflowData.Workflows.Any(workflow => workflow.Name == WorkflowName.FixCountOfTracksOnAlbum))
                {
                    TrackCountFixer.FixTrackCounts(tracks);
                }

                status.ItemProcessed();
            }
        }
    }
}
