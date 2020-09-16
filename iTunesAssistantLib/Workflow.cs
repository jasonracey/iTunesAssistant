using System;

namespace iTunesAssistantLib
{
    public class Workflow
    {
        public string Name { get; private set; }

        public string? FileName { get; private set; }

        public string? NewValue { get; private set; }

        public string? OldValue { get; private set; }

        public Workflow(
            string name,
            string? fileName = null,
            string? oldValue = null,
            string? newValue = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            
            Name = name;
            FileName = fileName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
