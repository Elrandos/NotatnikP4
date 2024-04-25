namespace WPF_lab3.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set;  }

        public Note(int id, string name, string description, int userId)
        {
            Id = id;
            Name = name;
            Description = description;
            UserId = userId;
        }
    }
}