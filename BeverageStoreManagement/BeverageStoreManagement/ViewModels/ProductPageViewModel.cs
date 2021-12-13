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
    class ProductPageViewModel : BaseViewModel
    {
        private string imageFileName;
        private MainWindow mainWindow;
        public ICommand OpenAddProductCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand LoadProductCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ProductPageViewModel()
        {
            OpenAddProductCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddProductWindow(parameter));
            ExitCommand = new RelayCommand<AddProductWindow>((parameter) => true, (parameter) => parameter.Close());
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => ChooseImage(parameter));
            LoadProductCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadProduct(parameter));
            AddProductCommand = new RelayCommand<AddProductWindow>(parameter => true, parameter => AddProduct(parameter));
            DeleteProductCommand = new RelayCommand<ProductViewControl>(parameter => true, parameter => DeleteProduct(parameter));
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
                    parameter.Children.Remove(parameter.Children[1]);
                }
            }
        }

        private void OpenAddProductWindow(MainWindow parameter)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            try
            {
                string id = (ProductDAL.Instance.GetMaxId() + 1).ToString();

                addProductWindow.txtIdGoods.Text = id;
            }
            catch
            {
                addProductWindow.txtIdGoods.Text = "1";
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
                string status;
                if(product.Status == true)
                {
                    status = "Available";
                }
                else
                {
                    status = "Unavailable";
                }

                ProductViewControl productViewControl = new ProductViewControl();
                productViewControl.idProduct.Text = product.IdProduct.ToString();
                productViewControl.Name.Text = product.NameProduct.ToString();
                productViewControl.Price.Text = product.Price.ToString();
                productViewControl.Status.Text = status;
                productViewControl.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                

                mainWindow.stkProduct.Children.Add(productViewControl);
                i++;
            }
        }

        public void AddProduct(AddProductWindow parameter)
        {
            List<Product> products = ProductDAL.Instance.ConvertDBToList();
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                parameter.txtName.Focus();
                parameter.txtName.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtType.Text))
            {
                parameter.txtType.Focus();
                parameter.txtPrice.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtPrice.Text))
            {
                parameter.txtPrice.Focus();
                parameter.txtPrice.Text = "";
                return;
            }
            if (string.IsNullOrEmpty(parameter.txtStatus.Text))
            {
                parameter.txtStatus.Focus();
                parameter.txtStatus.Text = "";
                return;
            }
            
            if (parameter.grdSelectImg.Background == null)
            {
                CustomMessageBox.Show("Vui lòng thêm hình ảnh!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            byte[] imgByteArr;
            try
            {
                imgByteArr = ConvertImageToBytes(imageFileName);
            }
            catch
            {
                imgByteArr = ProductDAL.Instance.GetProduct(parameter.txtIdGoods.Text).Image;
            }
            Product newProduct = new Product(int.Parse(parameter.txtIdGoods.Text), ConvertToType(parameter.txtType.Text), parameter.txtName.Text,
                 "", int.Parse(parameter.txtPrice.Text), ConvertToBoolean(parameter.txtStatus.Text),
                 imgByteArr, false);

            bool isSuccessed1 = true, isSuccessed2 = true, isSuccessed4 = true;

            if (products.Count == 0 || newProduct.IdProduct > products[products.Count - 1].IdProduct)
            {
                if (ProductDAL.Instance.IsExistProductName(parameter.txtName.Text))
                {
                    CustomMessageBox.Show("Existed Product, please try again!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    parameter.txtName.Focus();
                    parameter.txtName.Text = "";
                    return;
                }
                isSuccessed1 = ProductDAL.Instance.AddIntoDB(newProduct);
                if (isSuccessed1)
                {
                    CustomMessageBox.Show("Add successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                if (ProductDAL.Instance.GetProduct(parameter.txtIdGoods.Text).NameProduct != parameter.txtName.Text)
                {
                    if (ProductDAL.Instance.IsExistProductName(parameter.txtName.Text))
                    {
                        CustomMessageBox.Show("Existed Product, please try again!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        parameter.txtName.Focus();
                        parameter.txtName.Text = "";
                        return;
                    }
                }
                isSuccessed2 = ProductDAL.Instance.UpdateOnDB(newProduct);
                if (isSuccessed2 && isSuccessed4)
                {
                    CustomMessageBox.Show("Update successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            if (!isSuccessed1 || !isSuccessed2)
            {
                CustomMessageBox.Show("Existed Product, please try again!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            parameter.Close();
            LoadProduct(mainWindow);
        }

        public void DeleteProduct(ProductViewControl productViewControl)
        {
            MessageBoxResult result = MessageBox.Show("Confirm Delete", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                    MessageBox.Show("Action failed, please try again!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
