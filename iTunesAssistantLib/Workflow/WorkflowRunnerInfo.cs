using System;
using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class WorkflowRunnerInfo : IWorkflowRunnerInfo
    {
        public WorkflowRunnerInfo(IList<IITTrack> tracks, IEnumerable<Workflow>? workflows = null, string? inputFilePath = null)
        {
            this.Tracks = tracks ?? throw new ArgumentNullException(nameof(tracks));
            this.Workflows = workflows;
            this.InputFilePath = inputFilePath;
        }

        public IList<IITTrack> Tracks { get; private set; }

        public IEnumerable<Workflow>? Workflows { get; private set; }

        public string? InputFilePath { get; private set; }
    }
}
