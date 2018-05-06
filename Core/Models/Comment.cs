using Core.Data;

namespace Core.Models
{
    public class Comment : IEntity<long>
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
