using LibraryManagement.Models;
using LibraryManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class CategoryViewModel : BaseViewModel
    {
        LibraryManagementEntities DB = new LibraryManagementEntities();

        private ObservableCollection<Models.Category> _ListCategory;
        public ObservableCollection<Models.Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }

        //Selected data of DB
        private Category _SelectedItem;
        public Category SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdCategory = SelectedItem.idCategory;
                    NameCategory = SelectedItem.nameCategory;
                }
            }
        }

        private int _idCategory;
        public int IdCategory { get => _idCategory; set { _idCategory = value; OnPropertyChanged(); } }

        private string _nameCategory;
        public string NameCategory { get => _nameCategory; set { _nameCategory = value; OnPropertyChanged(); } }

        //open Window
        public ICommand AddCategoryCommand { get; set; }


        //effect DB
        public ICommand AddCategoryToDBCommand { get; set; }
        public ICommand DeleteCategorytoDBCommand { get; set; }

        public CategoryViewModel()
        {
            //DB to Window
            ListCategory = new ObservableCollection<Category>(DB.Categories);

            AddCategoryCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                AddCategoryScreen wd = new AddCategoryScreen();
                NameCategory = null;
                wd.ShowDialog();
            });

            //Add Category
            AddCategoryToDBCommand = new AppCommand<object>((p) =>
            {
                if (NameCategory == null) return false;
                var displayList = DB.Categories.Where(x => x.nameCategory == NameCategory);
                if (displayList == null)
                    return false;
                return true;

            }, (p) =>
            {
                var category = new Category()
                {
                    nameCategory = NameCategory
                };

                DB.Categories.Add(category);
                DB.SaveChanges();

                ListCategory.Add(category);
                MessageBox.Show("Thêm thể loại thành công");
            });

            //Delete Category
            DeleteCategorytoDBCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    var category = DB.Categories.Where(x => x.idCategory == SelectedItem.idCategory).SingleOrDefault();
                    DB.Categories.Remove(category);
                    DB.SaveChanges();
                    ListCategory.Remove(category);
                    MessageBox.Show("Xóa thể loại thành công");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Không thể xóa thể loại do thể loại còn được tham chiếu trong sách");
                }
            });
        }
    }
}
