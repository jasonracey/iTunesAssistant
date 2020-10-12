using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iTunesAssistantLib
{
    public class ImportTrackNamesWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowRunnerInfo workflowRunnerInfo, ref Status status)
        {
            if (workflowRunnerInfo == null) throw new ArgumentNullException(nameof(workflowRunnerInfo));
            if (workflowRunnerInfo.Tracks == null) throw new ArgumentNullException(nameof(workflowRunnerInfo.Tracks));
            if (string.IsNullOrWhiteSpace(workflowRunnerInfo.InputFilePath)) throw new ArgumentNullException(nameof(workflowRunnerInfo.InputFilePath));

            // Prevent assigning wrong names.
            if (workflowRunnerInfo.Tracks.Any(track => track.TrackNumber == 0))
            {
                throw new iTunesAssistantException("One or more tracks does not have a track number");
            }

            status = Status.Create(workflowRunnerInfo.Tracks.Count, "Reading track names to import...");

            var newTrackNames = new List<string>();

            foreach (var line in File.ReadAllLines(workflowRunnerInfo.InputFilePath))
            {
                var trimmedLine = line.Trim();
                if (trimmedLine != string.Empty)
                {
                    newTrackNames.Add(trimmedLine);
                    status.ItemProcessed();
                }
            }

            if (workflowRunnerInfo.Tracks.Count != newTrackNames.Count)
            {
                throw new iTunesAssistantException("The number of names to import must match the number of tracks selected");
            }

            status = Status.Create(workflowRunnerInfo.Tracks.Count, "Assigning new track names...");

            for (var i = 0; i < workflowRunnerInfo.Tracks.Count; i++)
            {
                workflowRunnerInfo.Tracks[i].Name = newTrackNames[i];
                status.ItemProcessed();
            }

            return;
        }
    }
}
