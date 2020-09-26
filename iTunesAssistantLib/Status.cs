using System;

namespace iTunesAssistantLib
{
    public class Status
    {
        public int ItemsProcessed { get; private set; }
        public int ItemsTotal { get; private set; }
        public string Message { get; private set; }

        private Status(int itemsTotal, string? message = null)
        {
            ItemsTotal = itemsTotal;
            Message = message ?? string.Empty;
        }

        public static Status Create(int itemsTotal, string? message = null)
        {
            if (itemsTotal < 0) throw new ArgumentOutOfRangeException(nameof(itemsTotal), "Value must be >= 0");
            return new Status(itemsTotal, message);
        }

        public void ItemProcessed()
        {
            ItemsProcessed++;
        }
    }
}
