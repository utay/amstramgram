using Core.Data;

namespace Core.Models
{
    public class Tag : IEntity<long>
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
