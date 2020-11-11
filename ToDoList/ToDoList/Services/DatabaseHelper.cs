using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class DatabaseHelper
    {
        public void SaveList(ObservableCollection<ToDoItem> list)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                // Reset database
                connection.DropTable<ToDoItem>();
                // Check if the provided list has any items in it to save to DB, if so save it
                if (list.Count > 0)
                {
                    connection.CreateTable<ToDoItem>();

                    foreach (var item in list)
                    {
                        connection.Insert(item);
                        Console.WriteLine("HOAS - Inserted to database: " + item.Name);
                    }
                }
            }
        }
        
        public List<ToDoItem> GetList()
        {
            List<ToDoItem> list;
            
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<ToDoItem>();
                list = connection.Table<ToDoItem>().ToList();
            }

            return list;
        }
    }
}