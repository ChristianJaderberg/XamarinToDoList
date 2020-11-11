using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using SQLite;
using ToDoList.Services;

namespace ToDoList.ViewModels
{
    public class ToDoListViewModel : BaseViewModel
    {

        public ObservableCollection<ToDoItem> ToDoItemsList { get; set; }
        private string _inputFieldValue = "";
        
        public DatabaseHelper DbHelper = new DatabaseHelper();
        public string BatteryInfo { get; set; }
        
        public ToDoListViewModel()
        {
            ToDoItemsList = new ObservableCollection<ToDoItem>();

            GetBatteryState();

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

                    DbHelper.SaveList(ToDoItemsList);
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
            ToDoItemsList.Clear();

            foreach (var item in DbHelper.GetList())
            {
                Console.WriteLine("HOAS - Read from DB: " + item.Name);
                ToDoItemsList.Add(item);
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