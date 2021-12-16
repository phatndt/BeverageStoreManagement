using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    class ProductPageViewModel : BaseViewModel
    {
        private string imageFileName;
        private MainWindow mainWindow;
        public ICommand OpenAddProductCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ExitChangeCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand LoadProductCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand OpenEditProductCommand { get; set; }
        public ICommand SaveEditProductCommand { get; set; }
        public ProductPageViewModel()
        {
            OpenAddProductCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddProductWindow(parameter));
            ExitCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => parameter.Close());
            ExitChangeCommand = new RelayCommand<ChangeProductWindow>((parameter) => true, (parameter) => parameter.Close());
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => ChooseImage(parameter));
            LoadProductCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadProduct(parameter));
            AddProductCommand = new RelayCommand<AddProductWindow>(parameter => true, parameter => AddProduct(parameter));
            DeleteProductCommand = new RelayCommand<ProductViewControl>(parameter => true, parameter => DeleteProduct(parameter));
            OpenEditProductCommand = new RelayCommand<ProductViewControl>(parameter => true, parameter => OpenEditProductWindow(parameter));
            SaveEditProductCommand = new RelayCommand<ChangeProductWindow>(parameter => true, parameter => SaveEditProduct(parameter));
        }

        private void ChooseImage(Grid parameter)
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
                }
            }
        }

        private void OpenAddProductWindow(MainWindow parameter)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            try
            {
                string id = (ProductDAL.Instance.GetMaxId() + 1).ToString();

                addProductWindow.txtIdProduct.Text = id;
            }
            catch
            {
                addProductWindow.txtIdProduct.Text = "1";
            }

            addProductWindow.ShowDialog();
        }

        private void LoadProduct(MainWindow parameter)
        {
            int i = 1;
            this.mainWindow = parameter;
            while (mainWindow.stkProduct.Children.Count > 2)
            {
                mainWindow.stkProduct.Children.RemoveAt(mainWindow.stkProduct.Children.Count - 1);

            }
            List<Product> products = (List<Product>)ProductDAL.Instance.GetList();
            foreach (Product product in products)
            {
                ProductViewControl productViewControl = new ProductViewControl();
                productViewControl.idProduct.Text = product.IdProduct.ToString();
                productViewControl.Name.Text = product.NameProduct.ToString();
                productViewControl.Price.Text = product.Price.ToString("N0");
                productViewControl.Status.Text = ConvertBooleanToStatus(product.Status);
                productViewControl.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                mainWindow.stkProduct.Children.Add(productViewControl);
                i++;
            }
        }

        private bool CheckEmptyAddProduct(AddProductWindow parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                CustomMessageBox.Show("Please enter product name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtName.Focus();
                parameter.txtName.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtType.Text))
            {
                CustomMessageBox.Show("Please enter product type!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtPrice.Text))
            {
                CustomMessageBox.Show("Please enter product price!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPrice.Focus();
                parameter.txtPrice.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtStatus.Text))
            {
                CustomMessageBox.Show("Please enter product status!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtStatus.Focus();
                return false;
            }
            if (parameter.grdSelectImg.Background == null)
            {
                CustomMessageBox.Show("Please enter product image!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool CheckEmptyEditProduct(ChangeProductWindow parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                CustomMessageBox.Show("Please enter product name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtName.Focus();
                parameter.txtName.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtType.Text))
            {
                CustomMessageBox.Show("Please enter product type!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtPrice.Text))
            {
                CustomMessageBox.Show("Please enter product price!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPrice.Focus();
                parameter.txtPrice.Text = "";
                return false;
            }
            if (string.IsNullOrEmpty(parameter.txtStatus.Text))
            {
                CustomMessageBox.Show("Please enter product status!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtStatus.Focus();
                return false;
            }
            if (parameter.grdSelectImg.Background == null)
            {
                CustomMessageBox.Show("Please enter product image!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public void AddProduct(AddProductWindow parameter)
        {
            if (CheckEmptyAddProduct(parameter))
            {
                byte[] imgByteArr;
                try
                {
                    imgByteArr = ConvertImageToBytes(imageFileName);
                }
                catch
                {
                    imgByteArr = ProductDAL.Instance.GetProduct(parameter.txtIdProduct.Text).Image;
                }
                Product product = new Product(int.Parse(parameter.txtIdProduct.Text),
                    int.Parse(parameter.txtType.Text),
                    parameter.txtName.Text,
                    "",
                    int.Parse(parameter.txtPrice.Text),
                    ConvertStatusToBoolean(parameter.txtStatus.Text),
                    imgByteArr,
                    false);

                if (ProductDAL.Instance.AddIntoDB(product))
                {
                    Notification.Instance.Success("Add Product Success!");
                    parameter.Close();
                    LoadProduct(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Product Failed!");
                    parameter.Close();
                }
            }

        }

        public void DeleteProduct(ProductViewControl productViewControl)
        {
            MessageBoxResult result = CustomMessageBox.Show("Confirm Delete", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                string idProduct = productViewControl.idProduct.Text;
                bool isSuccessed = ProductDAL.Instance.DeleteProduct(idProduct);
                if (isSuccessed)
                {
                    mainWindow.stkProduct.Children.Remove(productViewControl);
                }
                else
                {
                    CustomMessageBox.Show("Action failed, please try again!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void OpenEditProductWindow(ProductViewControl parameter)
        {
            int idProduct = int.Parse(parameter.idProduct.Text);

            Product product = ProductDAL.Instance.GetProductById(idProduct);
            ChangeProductWindow changeProductWindow = new ChangeProductWindow();

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = Converter.Instance.ConvertByteToBitmapImage(product.Image);

            changeProductWindow.txtIdProduct.Text = product.IdProduct.ToString();
            changeProductWindow.txtName.Text = product.NameProduct;
            changeProductWindow.txtType.Text = ConvertIntToType(product.IdProductType);
            changeProductWindow.txtPrice.Text = product.Price.ToString("N0");
            changeProductWindow.txtStatus.Text = ConvertBooleanToStatus(product.Status);
            changeProductWindow.grdSelectImg.Background = imageBrush;

            changeProductWindow.ShowDialog();
        }
        public void SaveEditProduct(ChangeProductWindow parameter)
        {
            if (CheckEmptyEditProduct(parameter))
            {
                int idProduct = int.Parse(parameter.txtIdProduct.Text);
                byte[] imgByteArr;
                Product product;
                if (imageFileName == null) 
                {
                    product = new Product(idProduct,
                    ConvertTypeToInt(parameter.txtType.Text),
                    parameter.txtName.Text,
                    "",
                    (int)ConvertToNumber( parameter.txtPrice.Text),
                    ConvertStatusToBoolean(parameter.txtStatus.Text),
                    null,
                    false);
                    if (ProductDAL.Instance.UpdateProduct(product) == 1)
                    {
                        Notification.Instance.Success("Update Product Success!");
                        parameter.Close();
                        LoadProduct(mainWindow);
                    }
                    else
                    {
                        Notification.Instance.Failed("Update Product Failed!");
                        parameter.Close();
                    }
                }
                else 
                { 
                    imgByteArr = ConvertImageToBytes(imageFileName);

                    product = new Product(idProduct,
                    ConvertTypeToInt(parameter.txtType.Text),
                    parameter.txtName.Text,
                    "",
                    (int)ConvertToNumber(parameter.txtPrice.Text),
                    ConvertStatusToBoolean(parameter.txtStatus.Text),
                    imgByteArr,
                    false);
                    if (ProductDAL.Instance.UpdateProduct(product) == 1)
                    {
                        Notification.Instance.Success("Update Product Success!");
                        parameter.Close();
                        LoadProduct(mainWindow);
                    }
                    else
                    {
                        Notification.Instance.Failed("Update Product Failed!");
                        parameter.Close();
                    }
                } 
            }
        }
    }
}
