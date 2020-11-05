using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using SQLite;

namespace ToDoList.ViewModels
{
    public class ToDoListViewModel : BaseViewModel
    {

        public ObservableCollection<ToDoItem> ToDoItemsList { get; set; }
        private string _inputFieldValue = "";
        
        public string BatteryInfo { get; set; }
        
        public ToDoListViewModel()
        {
            ToDoItemsList = new ObservableCollection<ToDoItem>();

            GetBatteryState();
            // GetListFromDb();

            AddButtonPressed = new Command(execute: () =>
            {
                AddTodoItem();
                InputFieldValue = "";
            },canExecute: () =>
            {
                return !_inputFieldValue.Equals("");
            });
        }

        private void GetBatteryState()
        {
            var state = Battery.State;
            
            switch (state)
            {
                case BatteryState.Charging:
                    BatteryInfo = "Currently charging";
                    break;
                case BatteryState.Full:
                    BatteryInfo = "Battery is full";
                    break;
                case BatteryState.Discharging:
                case BatteryState.NotCharging:
                    BatteryInfo = "Currently discharging battery or not being charged";
                    break;
                case BatteryState.NotPresent:
                    BatteryInfo = "Battery doesn't exist in device (desktop computer)";
                    break;
                case BatteryState.Unknown:
                    BatteryInfo = "Unable to detect battery state";
                    break;
            }
        }

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

        // Save current list to DB
        public Command SaveList
        {
            get
            {
                return new Command(() =>
                {
                    Console.WriteLine("HOAS - Save Button Pressed");

                    using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
                    {
                        // Reset database
                        connection.DropTable<ToDoItem>();
                        // Check if ToDoItemsList has any items in it to save to DB, if so save it
                        if (ToDoItemsList.Count > 0)
                        {
                            connection.CreateTable<ToDoItem>();

                            foreach (var item in ToDoItemsList)
                            {
                                connection.Insert(item);
                                Console.WriteLine("HOAS - Inserted to database: " + item.Name);
                            }
                        }
                    }
                });
            }
        }
  
        // Read current list from DB
        public Command ReadList
        {
            get
            {
                return new Command(() =>
                {
                    Console.WriteLine("HOAS - Read Button Pressed");

                    GetListFromDb();
                });
            }
        }

        public void GetListFromDb()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<ToDoItem>();

                var itemsList = connection.Table<ToDoItem>().ToList();
                        
                ToDoItemsList.Clear();

                foreach (var item in itemsList)
                {
                    Console.WriteLine("HOAS - Read from DB: " + item.Name);
                    ToDoItemsList.Add(item);
                }
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