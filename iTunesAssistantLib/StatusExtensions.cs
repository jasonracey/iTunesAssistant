namespace iTunesAssistantLib
{
    public static class StatusExtensions
    {
        public static void Update(this Status status, int itemsProcessed, int itemsTotal, string state)
        {
            status.ItemsProcessed = itemsProcessed;
            status.ItemsTotal = itemsTotal;
            status.Message = state;
        }
    }
}
