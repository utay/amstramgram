namespace Api.Models
{
    public class Like
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }
        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
