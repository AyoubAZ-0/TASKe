namespace TASKe.Models
{
    public class Taskitem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        public string Status { get; set; } = "NotStarted";
    }
}
