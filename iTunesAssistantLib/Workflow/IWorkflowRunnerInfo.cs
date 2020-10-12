using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public interface IWorkflowRunnerInfo
    {
        IList<IITTrack> Tracks { get; }

        IEnumerable<Workflow>? Workflows { get; }

        string? InputFilePath { get; }
    }
}
