namespace iTunesAssistantLib
{
    public interface IWorkflowRunner
    {
        void Run(IWorkflowData workflowData, ref Status status);
    }
}
