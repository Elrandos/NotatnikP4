namespace WPF_lab3.Dto
{
    public class NoteDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        public NoteDto(string name, string description, int userId)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }
    }
}