namespace Api.Models
{
    public class Picture
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public User User { get; set; }
        public Tag[] Tags { get; set; }
        public Comment[] Comments { get; set; }
        public Like[] Likes { get; set; }
    }
}
