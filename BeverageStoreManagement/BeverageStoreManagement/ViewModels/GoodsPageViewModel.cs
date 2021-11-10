using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BeverageStoreManagement.ViewModels
{
    class GoodsPageViewModel : BaseViewModel
    {
        private string imageFileName;

        public ICommand OpenAddMaterialCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ExitUpdateCommand { get; set; }
        public ICommand OpenUpdateProductCommand { get; set; }

        public GoodsPageViewModel()
        {
            OpenAddMaterialCommand = new RelayCommand<GoodsPage>(parameter => true, parameter => OpenAddMaterialWindow(parameter));
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => ChooseImage(parameter));
            ExitCommand = new RelayCommand<AddMaterialWindow>((parameter) => true, (parameter) => parameter.Close());
            ExitUpdateCommand = new RelayCommand<UpdateGoodsWindow>((parameter) => true, (parameter) => parameter.Close());
            OpenUpdateProductCommand = new RelayCommand<GoodsPage>(parameter => true, parameter => OpenUpdateProductWindow(parameter));
        }

        private void OpenUpdateProductWindow(GoodsPage parameter)
        {
            UpdateGoodsWindow updateGoodsWindow = new UpdateGoodsWindow();
            updateGoodsWindow.ShowDialog();
        }

        private void OpenAddMaterialWindow(GoodsPage parameter)
        {
            AddMaterialWindow addMaterialWindow = new AddMaterialWindow();
            addMaterialWindow.ShowDialog();
        }

        public void ChooseImage(Grid parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Choose Image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.Background = imageBrush;
                if (parameter.Children.Count > 1)
                {
                    parameter.Children.Remove(parameter.Children[0]);
                    parameter.Children.Remove(parameter.Children[1]);
                }
            }
        }
    }
}
