using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class ImportTrackNamesWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowRunnerInfo workflowRunnerInfo, ref Status status)
        {
            status = Status.Create(workflowRunnerInfo.Tracks.Count, "Reading track names to import...");

            var newTrackNames = System.IO.File.ReadAllLines(workflowRunnerInfo.InputFilePath);

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

            if (workflowRunnerInfo.Tracks.Any(track => track.TrackNumber == 0))
            {
                throw new iTunesAssistantException("One or more tracks does not have a track number");
            }

            if (workflowRunnerInfo.Tracks.Count != cleanedNewTrackNames.Count)
            {
                throw new iTunesAssistantException("The number of names to import must match the number of tracks selected");
            }

            status = Status.Create(workflowRunnerInfo.Tracks.Count, "Assigning new track names...");

            for (var i = 0; i < workflowRunnerInfo.Tracks.Count; i++)
            {
                workflowRunnerInfo.Tracks[i].Name = cleanedNewTrackNames[i];
                status.ItemProcessed();
            }

            return;
        }
    }
}
