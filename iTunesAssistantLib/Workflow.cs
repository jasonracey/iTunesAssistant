using System;

namespace iTunesAssistantLib
{
    public class Workflow
    {
        public string Name { get; private set; }

        public string? FileName { get; private set; }

        public string? NewValue { get; private set; }

        public string? OldValue { get; private set; }

        public WorkflowType? Type { get; private set;}

        private Workflow(
            string name,
            string? fileName = null,
            string? oldValue = null,
            string? newValue = null,
            WorkflowType? type = WorkflowType.None)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            
            Name = name;
            FileName = fileName;
            OldValue = oldValue;
            NewValue = newValue;
            Type = type;
        }

        public static Workflow Create(
            string name, 
            string? oldValue = null, 
            string? newValue = null, 
            string? fileName = null)
        {
            return name switch
            {
                WorkflowName.FindAndReplace => new Workflow(name, oldValue: oldValue, newValue: newValue),
                WorkflowName.ImportTrackNames => new Workflow(name, fileName: fileName),
                WorkflowName.MergeAlbums => new Workflow(name),
                WorkflowName.FixCountOfTracksOnAlbum => new Workflow(name, type: WorkflowType.Album),
                WorkflowName.FixTrackNumbers => new Workflow(name, type: WorkflowType.Album),
                _ => new Workflow(name, type: WorkflowType.Track)
            };
        }
    }
}
