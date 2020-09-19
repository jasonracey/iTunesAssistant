using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public interface IWorkflowRunner
    {
        void Run(Status status, IList<IITTrack> tracks, IEnumerable<Workflow>? workflows = null, string? inputFilePath = null);
    }
}
