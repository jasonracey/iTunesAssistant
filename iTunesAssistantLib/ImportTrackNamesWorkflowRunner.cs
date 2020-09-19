using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class ImportTrackNamesWorkflowRunner : IWorkflowRunner
    {
        public void Run(Status status, IList<IITTrack> tracksToFix, IEnumerable<Workflow>? workflows, string? inputFilePath = null)
        {
            status.Update(0, tracksToFix.Count, "Reading track names to import...");

            var newTrackNames = System.IO.File.ReadAllLines(inputFilePath);

            var cleanedNewTrackNames = new List<string>();
            foreach (var newTrackName in newTrackNames)
            {
                var cleanName = newTrackName.Trim();
                if (cleanName != string.Empty)
                {
                    cleanedNewTrackNames.Add(cleanName);
                    status.ItemsProcessed++;
                }
            }

            if (tracksToFix.Any(track => track.TrackNumber == 0))
            {
                throw new iTunesAssistantException("One or more tracks does not have a track number");
            }

            if (tracksToFix.Count != cleanedNewTrackNames.Count)
            {
                throw new iTunesAssistantException("The number of names to import must match the number of tracks selected");
            }

            status.Update(0, tracksToFix.Count, "Assigning new track names...");

            for (var i = 0; i < tracksToFix.Count; i++)
            {
                tracksToFix[i].Name = cleanedNewTrackNames[i];
                status.ItemsProcessed++;
            }

            return;
        }
    }
}
