using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WPF_lab3.Dto;
using WPF_lab3.Model;
using WPF_lab3.Service;

namespace WPF_lab3
{
    public partial class MainWindow : Window
    {
        private readonly TaskService _taskService;
        public int LoggedUserId { get; set; } = -1;

        public string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RB_Gr1;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        private ObservableCollection<Note> ObservableTasks;

        public MainWindow()
        {
            InitializeComponent();
            _taskService = new TaskService(ConnectionString);
            ObservableTasks = new ObservableCollection<Note>();
            UpdateObservableNotes();
            Task_ListView.ItemsSource = ObservableTasks;
        }

        private void User_Button_Click(object sender, RoutedEventArgs e)
        {
            var createTaskWindow = new LoginWindow(ConnectionString);
            createTaskWindow.Activate();
            createTaskWindow.Show();
            createTaskWindow.Closing += OnLoginWindowClose; 
        }

        private void OnLoginWindowClose(object sender, CancelEventArgs e)
        {
            LoginWindow eventSender = (LoginWindow)sender;
            LoggedUserId = eventSender.LoggedUserId;
            UpdateObservableNotes();
        }

        private void CreateTask_Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedUserId == -1)
            {
                MessageBox.Show("First you need to log in or create account.");
                return;
            }
            var createTaskWindow = new CreateNoteWindow();
            createTaskWindow.Activate();
            createTaskWindow.Show();
            createTaskWindow.Closing += OnCreateNoteWindowClose;
        }

        private void OnCreateNoteWindowClose(object sender, CancelEventArgs e)
        {
            var eventSender = (CreateNoteWindow)sender;
            if (eventSender.IsCreateRequest)
            {
                _taskService.CreateNote(new NoteDto(eventSender.CreateNewNoteName,
                    eventSender.CreateNewNoteDescription, LoggedUserId));
                UpdateObservableNotes();
            }
        }
        
        private void EditNote_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Task_ListView.SelectedItem is Note selectedTask)
            {
                var editTaskWindow = new EditNoteWindow(selectedTask);
                editTaskWindow.Activate();
                editTaskWindow.Show();
                editTaskWindow.Closing += OnEditNoteWindowClose;
            }
            else
            {
                if (LoggedUserId == -1)
                {
                    MessageBox.Show("First you need to log in or create account.");
                }
                else
                {
                    MessageBox.Show("Please select a task to edit.");
                }
            }
        }

        private void OnEditNoteWindowClose(object sender, CancelEventArgs e)
        {
            var eventSender = (EditNoteWindow)sender;
            if (eventSender.IsEditRequest)
            {
                _taskService.EditNote(new Note(eventSender.TaskId, eventSender.EditTaskName, eventSender.EditTaskDescription, 3));
                UpdateObservableNotes();
            }
            if (eventSender.IsDeleteRequest)
            {
                _taskService.DeleteNote(eventSender.TaskId);
                UpdateObservableNotes();
            }
        }

        private void UpdateObservableNotes()
        {
            ObservableTasks.Clear();
            if (LoggedUserId != -1)
            {
                var tasks = _taskService.GetNotesForUser(LoggedUserId);
                foreach (var item in tasks)
                {
                    ObservableTasks.Add(item);
                }
            }
        }
    }
}