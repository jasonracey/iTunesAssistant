using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class ImportTrackNamesWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowData workflowData, ref Status status)
        {
            status = Status.Create(workflowData.Tracks.Count, "Reading track names to import...");

            var newTrackNames = System.IO.File.ReadAllLines(workflowData.InputFilePath);

            var cleanedNewTrackNames = new List<string>();
            foreach (var newTrackName in newTrackNames)
            {
                var cleanName = newTrackName.Trim();
                if (cleanName != string.Empty)
                {
                    cleanedNewTrackNames.Add(cleanName);
                    status.ItemProcessed();
                }
            }

            if (workflowData.Tracks.Any(track => track.TrackNumber == 0))
            {
                throw new iTunesAssistantException("One or more tracks does not have a track number");
            }

            if (workflowData.Tracks.Count != cleanedNewTrackNames.Count)
            {
                throw new iTunesAssistantException("The number of names to import must match the number of tracks selected");
            }

            status = Status.Create(workflowData.Tracks.Count, "Assigning new track names...");

            for (var i = 0; i < workflowData.Tracks.Count; i++)
            {
                workflowData.Tracks[i].Name = cleanedNewTrackNames[i];
                status.ItemProcessed();
            }

            return;
        }
    }
}
