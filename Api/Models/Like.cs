namespace Api.Models
{
    public class Like
    {
        public string CreatedAt { get; set; }
        public User User { get; set; }
        public Picture Picture { get; set; }
    }
}
