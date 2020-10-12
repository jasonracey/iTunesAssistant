namespace iTunesAssistantLib
{
    public interface IWorkflowRunner
    {
        void Run(IWorkflowRunnerInfo workflowRunnerInfo, ref Status status);
    }
}
