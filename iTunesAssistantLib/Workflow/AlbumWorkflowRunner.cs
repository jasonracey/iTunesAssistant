using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class AlbumWorkflowRunner : IWorkflowRunner
    {
        public void Run(ref Status status, IList<IITTrack> tracksToFix, IEnumerable<Workflow>? workflows, string? inputFilePath = null)
        {
            var albums = AlbumBuilder.BuildAlbums(ref status, tracksToFix);

            status = Status.Create(albums.Count, "Running album workflows...");

            const string trackMissingErrorCode = "0xA0040203";
            const string trackMissingErrorMessage = "One or more tracks could not be found";

            foreach (var album in albums)
            {
                // must set number before count in case old number is higher than count
                if (workflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNumbers))
                {
                    var trackComparer = TrackComparerFactory.GetTrackComparer(album.Value);
                    album.Value.ToList().Sort(trackComparer);
                    for (var i = 0; i < album.Value.Count; i++)
                    {
                        try
                        {
                            album.Value[i].TrackNumber = i + 1;
                        }
                        catch (System.Runtime.InteropServices.COMException e)
                        {
                            if (e.Message.Contains(trackMissingErrorCode))
                            {
                                throw new iTunesAssistantException(trackMissingErrorMessage);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                if (workflows.Any(workflow => workflow.Name == WorkflowName.FixCountOfTracksOnAlbum))
                {
                    foreach (var track in album.Value)
                    {
                        try
                        {
                            track.TrackCount = album.Value.Count;
                        }
                        catch (System.Runtime.InteropServices.COMException e)
                        {
                            if (e.Message.Contains(trackMissingErrorCode))
                            {
                                throw new iTunesAssistantException(trackMissingErrorMessage);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                status.ItemProcessed();
            }
        }
    }
}
