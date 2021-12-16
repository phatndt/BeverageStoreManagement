using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BeverageStoreManagement.ViewModels
{
    class GoodsPageViewModel : BaseViewModel
    {
        private string imageFileName;
        private MainWindow mainWindow;
        public ICommand LoadMaterialCommand { get; set; }
        public ICommand OpenAddMaterialCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ExitUpdateCommand { get; set; }
        public ICommand OpenUpdateProductCommand { get; set; }
        public ICommand AddMaterialCommand { get; set; }
        public ICommand DeleteMaterialCommand { get; set; }

        public GoodsPageViewModel()
        {
            OpenAddMaterialCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddMaterialWindow(parameter));
            LoadMaterialCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadMaterial(parameter));
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => ChooseImage(parameter));
            ExitCommand = new RelayCommand<AddMaterialWindow>((parameter) => true, (parameter) => parameter.Close());
            ExitUpdateCommand = new RelayCommand<UpdateGoodsWindow>((parameter) => true, (parameter) => parameter.Close());
            OpenUpdateProductCommand = new RelayCommand<GoodsPage>(parameter => true, parameter => OpenUpdateProductWindow(parameter));
            AddMaterialCommand = new RelayCommand<AddMaterialWindow>(parameter => true, parameter => AddMaterial(parameter));
            DeleteMaterialCommand = new RelayCommand<GoodsViewControl>(parameter => true, parameter => DeleteMaterial(parameter));
        }
        private void LoadMaterial(MainWindow parameter)
        {
            int i = 1;
            this.mainWindow = parameter;
            mainWindow.stkMaterial.Children.Clear();
            List<Material> materials = (List<Material>)MaterialDAL.Instance.GetList();
            foreach (Material material in materials)
            {
                GoodsViewControl goodsViewControl = new GoodsViewControl();
                goodsViewControl.idMaterial.Text = material.IdMaterial.ToString();
                goodsViewControl.txtMaterial.Content = material.NameMaterial.ToString();
                goodsViewControl.txtType.Content = material.Type.ToString();
                goodsViewControl.txtQuantity.Content = material.Quantity.ToString();
                goodsViewControl.txtUnit.Content = material.CountUnit.ToString();
                goodsViewControl.txtPrice.Content = material.PurchasePrice.ToString("N0");
                goodsViewControl.txtStatus.Content = ConvertBooleanToStatus(material.Status); ;
                goodsViewControl.txtImg.Source = Converter.Instance.ConvertByteToBitmapImage(material.Image);

                mainWindow.stkMaterial.Children.Add(goodsViewControl);
                i++;
            }
        }
        private void OpenUpdateProductWindow(GoodsPage parameter)
        {
            UpdateGoodsWindow updateGoodsWindow = new UpdateGoodsWindow();
            updateGoodsWindow.ShowDialog();
        }

        private void OpenAddMaterialWindow(MainWindow parameter)
        {
            AddMaterialWindow addMaterial = new AddMaterialWindow();
            try
            {
                string id = (MaterialDAL.Instance.GetMaxId() + 1).ToString();

                addMaterial.txtIdMaterial.Text = id;
            }
            catch
            {
                addMaterial.txtIdMaterial.Text = "1";
            }

            addMaterial.ShowDialog();
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

        private bool CheckEmptyAddMaterial(AddMaterialWindow parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                CustomMessageBox.Show("Please enter material name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtName.Focus();
                parameter.txtName.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.cboType.Text))
            {
                CustomMessageBox.Show("Please enter material type!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.cboType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(parameter.cboUnit.Text))
            {
                CustomMessageBox.Show("Please enter material count unit!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.cboUnit.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtQuantity.Text))
            {
                CustomMessageBox.Show("Please enter material quantity!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtQuantity.Focus();
                parameter.txtQuantity.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtPrice.Text))
            {
                CustomMessageBox.Show("Please enter material purchase!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPrice.Focus();
                parameter.txtPrice .Text = "";
                return false;
            }
            if (parameter.grdSelectImg.Background == null)
            {
                CustomMessageBox.Show("Please enter material image!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public void AddMaterial(AddMaterialWindow parameter)
        {
            if (CheckEmptyAddMaterial(parameter))
            {
                byte[] imgByteArr;
                try
                {
                    imgByteArr = ConvertImageToBytes(imageFileName);
                }
                catch
                {
                    imgByteArr = MaterialDAL.Instance.GetMaterial(parameter.txtIdMaterial.Text).Image;
                }
                double quantity = double.Parse(parameter.txtQuantity.Text);
                
                Material material = new Material(
                    int.Parse(parameter.txtIdMaterial.Text),
                    parameter.txtName.Text,
                    parameter.cboType.Text,
                    parameter.cboUnit.Text,
                    quantity,
                    double.Parse(parameter.txtPrice.Text),
                    imgByteArr,
                    ConvertToStatus(quantity),
                    false);

                if (MaterialDAL.Instance.AddIntoDB(material))
                {
                    Notification.Instance.Success("Add Material Success!");
                    parameter.Close();
                    LoadMaterial(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Material Failed!");
                    parameter.Close();
                }
            }
        }
    
        public void DeleteMaterial(GoodsViewControl parameter)
        {
            MessageBoxResult result = CustomMessageBox.Show("Confirm Delete", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                string idMaterial = parameter.idMaterial.Text;
                bool isSuccessed = MaterialDAL.Instance.DeleteMaterial(idMaterial);
                if (isSuccessed)
                {
                    mainWindow.stkProduct.Children.Remove(parameter);
                }
                else
                {
                    CustomMessageBox.Show("Action failed, please try again!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
