using LibraryManagement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace LibraryManagement.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string property = null, string value = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    class AppCommand<T> : ICommand
    {
        private readonly Predicate<T> _CanExecute;
        private readonly Action<T> _Execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _CanExecute == null ? true : _CanExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _Execute((T)parameter);
        }
        public AppCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _CanExecute = canExecute;
            _Execute = execute;
        }
    }

    class PagingCollectionView<T> : CollectionView
    {
        private List<T> _innerList;
        private int _itemsPerPage;
        private int _currentPage;

        public PagingCollectionView(List<T> innerList, int itemsPerPage = 10) : base(innerList)
        {
            this._currentPage = 1;
            this._innerList = innerList;
            this._itemsPerPage = itemsPerPage;
        }

        public override int Count
        {
            get
            {
                if (this._innerList.Count == 0) return 0; // No item
                if (this._currentPage < this.PageCount) // No. items in a normal page not the last one
                {
                    return this._itemsPerPage;
                }
                // No. items in the last page
                int itemLeft = this._innerList.Count % this._itemsPerPage;
                return itemLeft == 0 ? this._itemsPerPage : itemLeft;
            }
        }

        public int PageCount { get => 1 + (this._innerList.Count - 1) / this._itemsPerPage; }

        public int CurrentPage
        {
            get => this._currentPage;
            set
            {
                this._currentPage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        public int ItemsPerPage { get => this._itemsPerPage; }

        private int EndIndex
        {
            get
            {
                int end = this._currentPage * this._itemsPerPage - 1;
                return end > this._innerList.Count ? this._innerList.Count : end;
            }
        }

        private int StartIndex
        {
            get => (this._currentPage - 1) * this._itemsPerPage;
        }

        public override object GetItemAt(int index)
        {
            int offset = index % this._itemsPerPage;
            return this._innerList[this.StartIndex + offset];
        }

        public object GetItemById(string objectType, int index)
        {
            try
            {
                switch (objectType)
                {
                    case "Book":
                        return _innerList.Find(el => (el as Book).idBook == index);
                    case "Reader":
                        return _innerList.Find(el => (el as Reader).idReader == index);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi xảy ra!");
                return null;
            }
        }

        public int GetPositionById(string objectType, int index)
        {
            try
            {
                switch (objectType)
                {
                    case "Book":
                        return _innerList.FindIndex(el => (el as Book).idBook == index);
                    case "Reader":
                        return _innerList.FindIndex(el => (el as Reader).idReader == index);
                    default:
                        return -1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi xảy ra!");
                return -1;
            }
        }

        public void MoveToNextPage()
        {
            if (this._currentPage < this.PageCount)
            {
                this.CurrentPage++;
            }
            this.Refresh();
        }

        public void MoveToPreviousPage()
        {
            if (this._currentPage > 1)
            {
                this.CurrentPage--;
            }
            this.Refresh();
        }

        public void MoveToSelectedItem(string objectType, int id)
        {
            int position = GetPositionById(objectType, id);
            int result = (position / ItemsPerPage) + 1;
            CurrentPage = result;
            this.Refresh();
        }

        public void RemoveItem(T obj)
        {
            this._innerList.Remove(obj);
            this.Refresh();
        }
        public void AddItem(T obj)
        {
            this._innerList.Add(obj);
            this.Refresh();
        }
    }
}
