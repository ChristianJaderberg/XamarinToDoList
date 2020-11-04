using System;
using ToDoList.Models;
using ToDoList.ViewModels;
using Xamarin.Forms;

namespace ToDoList.Views
{
    public partial class ToDoList : ContentPage
    {
        public ToDoList()
        {
            InitializeComponent();
        }

        private void FinishItem(object sender, EventArgs e)
        {
            var viewCell = sender as ViewCell;

            var toDoItem = viewCell?.BindingContext as ToDoItem;

            var toDoListViewModel = BindingContext as ToDoListViewModel;
            
            toDoListViewModel?.FinishItem.Execute(toDoItem);
        }
    }
}