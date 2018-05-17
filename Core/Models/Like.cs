using Core.Data;

namespace Core.Models
{
    public class Like : IEntity<long>
    {
        public long Id { get; set; }
        public string CreatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
