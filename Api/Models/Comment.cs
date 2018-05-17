namespace Api.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
