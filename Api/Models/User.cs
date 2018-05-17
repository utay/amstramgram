namespace Api.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Picture { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }

        public User[] Followers { get; set; }
        public Picture[] Pictures { get; set; }
    }
}
