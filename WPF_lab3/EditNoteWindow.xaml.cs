using System.Windows;
using WPF_lab3.Model;

namespace WPF_lab3
{
    public partial class EditNoteWindow : Window
    {
        public string EditTaskName { get; set; }
        public string EditTaskDescription { get; set; }
        public int TaskId { get; set; }
        public bool IsEditRequest { get; private set; } = false;
        public bool IsDeleteRequest { get; private set; } = false;


        public EditNoteWindow(Note note)
        {
            InitializeComponent();
            NameTextBox.Text = note.Name;
            DescriptionTextBox.Text = note.Description;
            TaskId = note.Id;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            EditTaskName = NameTextBox.Text;
            EditTaskDescription = DescriptionTextBox.Text;
            IsEditRequest = true;

            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            IsDeleteRequest = true;

            this.Close();
        }
    }
}