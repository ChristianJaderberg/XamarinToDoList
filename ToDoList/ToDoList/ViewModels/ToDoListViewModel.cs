using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoList.Models;
using Xamarin.Forms;

namespace ToDoList.ViewModels
{
    public class ToDoListViewModel : BaseViewModel
    {

        public ToDoListViewModel()
        {
            ToDoItemsList = new ObservableCollection<ToDoItem>();

            AddButtonPressed = new Command(execute: () =>
            {
                AddTodoItem();
                InputFieldValue = "";
            },canExecute: () =>
            {
                return !_inputFieldValue.Equals("");
            });
        }

        private string _inputFieldValue = "";
        
        public string InputFieldValue
        {
            get => _inputFieldValue;
            set
            {
                if (_inputFieldValue == value) return;
                _inputFieldValue = value;
                OnPropertyChanged();
                RefreshCanExecute();
            }
        }
        
        public ObservableCollection<ToDoItem> ToDoItemsList { get; set; }
        
        public ICommand AddButtonPressed { private set; get; }

        public Command<ToDoItem> FinishItem
        {
            get
            {
                return new Command<ToDoItem>((toDoItem) =>
                {
                    ToDoItemsList.Remove(toDoItem);
                });
            }
        }
        
        public void AddTodoItem()
        {
            ToDoItem newItem = new ToDoItem();
            newItem.Name = InputFieldValue;
            ToDoItemsList.Insert(0, newItem);
        }
        
        private void RefreshCanExecute()
        {
            ((Command)AddButtonPressed).ChangeCanExecute();
            //Add more ICommands here
        }
    }
}