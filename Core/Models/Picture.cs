using Core.Data;
using System.Collections.Generic;

namespace Core.Models
{
    public class Picture : IEntity<long>
    {
        public string objectID { get; set; }
        public long Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
