using System.Windows;

namespace WPF_lab3
{
    public partial class CreateNoteWindow : Window
    {
        public string CreateNewNoteName { get; set; }
        public string CreateNewNoteDescription { get; set; }
        public bool IsCreateRequest { get; set; }

        public CreateNoteWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CreateNewNoteName = NewNoteName_TextBox.Text;
            CreateNewNoteDescription = NewNoteDescription_TextBox.Text;
            IsCreateRequest = true;
            this.Close();
            MessageBox.Show("Added successfully.");
        }
    }
}
