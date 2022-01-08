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
        enum Type
        {
            All,
            Drinks,
            Fastfood,
            Ingredient,
            Item
        }

        enum Status
        {
            All,
            Available,
            Unavailable,
        }

        private string imageFileName;
        private MainWindow mainWindow;
        private UpdateGoodsWindow updateGoodsWindow;
        List<Material> materials = new List<Material>();
        List<Material> updateMaterials = new List<Material>();
        List<UpdateMaterialControl> updateMaterialControls = new List<UpdateMaterialControl>();
        string tbx = "";
        public string Tbx { get => tbx; set { tbx = value; OnPropertyChanged(); } }
        public ICommand LoadMaterialCommand { get; set; }
        public ICommand OpenAddMaterialCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ExitUpdateCommand { get; set; }
        public ICommand OpenUpdateProductCommand { get; set; }
        public ICommand AddMaterialCommand { get; set; }
        public ICommand DeleteMaterialCommand { get; set; }
        public ICommand FilterMaterialCommand { get; set; }
        public ICommand UpdateMaterialCommand { get; set; }
        public ICommand LoadUpdateMaterialCommand { get; set; }
        public ICommand SearchCommand { get; set; }


        public GoodsPageViewModel()
        {
            OpenAddMaterialCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddMaterialWindow(parameter));
            LoadMaterialCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadMaterial(parameter));
            SelectImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => ChooseImage(parameter));
            ExitCommand = new RelayCommand<AddMaterialWindow>((parameter) => true, (parameter) => parameter.Close());
            ExitUpdateCommand = new RelayCommand<UpdateGoodsWindow>((parameter) => true, (parameter) => parameter.Close());
            OpenUpdateProductCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenUpdateMaterialWindow(parameter));
            AddMaterialCommand = new RelayCommand<AddMaterialWindow>(parameter => true, parameter => AddMaterial(parameter));
            DeleteMaterialCommand = new RelayCommand<GoodsViewControl>(parameter => true, parameter => DeleteMaterial(parameter));
            FilterMaterialCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Filter(parameter));
            UpdateMaterialCommand = new RelayCommand<UpdateGoodsWindow>(parameter => true, parameter => UpdateMaterial(parameter));
            LoadUpdateMaterialCommand = new RelayCommand<UpdateGoodsWindow>(parameter => true, parameter => LoadUpdateMaterialWindow(parameter));
            SearchCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Search(parameter));
        }

        private void LoadMaterial(MainWindow parameter)
        {
            this.mainWindow = parameter;
            parameter.txtTimeUpdate.Text = MaterialDAL.Instance.GetMaterial("0").NameMaterial;
            mainWindow.stkMaterial.Children.Clear();

            materials = (List<Material>)MaterialDAL.Instance.GetList();

            foreach (Material material in materials)
            {
                GoodsViewControl goodsViewControl = new GoodsViewControl();
                goodsViewControl.idMaterial.Text = material.IdMaterial.ToString();
                goodsViewControl.txtNo.Content = material.IdMaterial.ToString();
                goodsViewControl.txtMaterial.Content = material.NameMaterial.ToString();
                goodsViewControl.txtType.Content = material.Type.ToString();
                goodsViewControl.txtQuantity.Content = material.Quantity.ToString();
                goodsViewControl.txtUnit.Content = material.CountUnit.ToString();
                goodsViewControl.txtPrice.Content = material.PurchasePrice.ToString("N0");
                goodsViewControl.txtStatus.Content = ConvertBooleanToStatus(material.Status); ;
                goodsViewControl.txtImg.Source = Converter.Instance.ConvertByteToBitmapImage(material.Image);

                mainWindow.stkMaterial.Children.Add(goodsViewControl);
            }
        }
        private void OpenUpdateMaterialWindow(MainWindow parameter)
        {
            UpdateGoodsWindow updateGoodsWindow = new UpdateGoodsWindow();
            updateGoodsWindow.stkMaterialList.Children.Clear();
            updateMaterials = (List<Material>)MaterialDAL.Instance.GetList();
            updateMaterialControls = new List<UpdateMaterialControl>();
            foreach (Material material in materials)
            {
                UpdateMaterialControl updateMaterialControl = new UpdateMaterialControl();
                updateMaterialControl.idMaterial.Content = material.IdMaterial.ToString();
                updateMaterialControl.nameMaterial.Text = material.NameMaterial.ToString();
                updateMaterialControl.cboType.Text = material.Type.ToString();
                updateMaterialControl.txtQuantity.Text = material.Quantity.ToString();
                updateMaterialControl.cboUnit.Text = material.CountUnit.ToString();
                updateMaterialControl.txtPrice.Text = material.PurchasePrice.ToString("N0");
                updateMaterialControl.cbStatus.Text = ConvertBooleanToStatus(material.Status); ;
                updateMaterialControl.txtImg.Source = Converter.Instance.ConvertByteToBitmapImage(material.Image);

                updateMaterialControls.Add(updateMaterialControl);
                updateGoodsWindow.stkMaterialList.Children.Add(updateMaterialControl);
            }

            updateGoodsWindow.ShowDialog();
        }

        private void LoadUpdateMaterialWindow(UpdateGoodsWindow parameter)
        {
            this.updateGoodsWindow = parameter;

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
                parameter.txtPrice.Text = "";
                return false;
            }
            if (parameter.grdSelectImg.Background == null)
            {
                CustomMessageBox.Show("Please enter material image!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool CheckEmptyUpdateMaterial(UpdateMaterialControl parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.nameMaterial.Text))
            {
                CustomMessageBox.Show("Please enter material name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.nameMaterial.Focus();
                parameter.nameMaterial.Text = "";
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
                parameter.txtPrice.Text = "";
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
                    //MessageBox.Show(mainWindow.stkMaterial.Children.IndexOf(parameter).ToString());
                    //updateGoodsWindow.stkMaterialList.Children.RemoveAt(0);
                    updateMaterialControls.RemoveAt(0);
                    mainWindow.stkMaterial.Children.Remove(parameter);
                    Notification.Instance.Success("Delete Material Success!");
                }
                else
                {
                    CustomMessageBox.Show("Action failed, please try again!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Filter(MainWindow parameter)
        {
            ComboBoxItem typeItem = (ComboBoxItem)parameter.cboType.SelectedItem;
            string value = typeItem.Content.ToString();
            ComboBoxItem typeItem1 = (ComboBoxItem)parameter.cboStatus.SelectedItem;
            string value1 = typeItem1.Content.ToString();
            List<Material> materials1 = new List<Material>();
            if (parameter.cboType.SelectedIndex == (int) Type.All && parameter.cboStatus.SelectedIndex == (int)Status.All)
            {
                materials1 = materials;
            }
            if (parameter.cboType.SelectedIndex != (int)Type.All && parameter.cboStatus.SelectedIndex == (int)Status.All)
            {

                for (int i = 0; i < materials.Count; i++)
                {
                    if (materials[i].Type == value)
                    {
                        materials1.Add(materials[i]);
                    }
                }
            }
            if (parameter.cboType.SelectedIndex == (int)Type.All && parameter.cboStatus.SelectedIndex != (int)Status.All)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    if (materials[i].Status == ConvertStatusToBoolean(value1))
                    {
                        materials1.Add(materials[i]);
                    }
                }
            }
            if (parameter.cboType.SelectedIndex != (int)Type.All && parameter.cboStatus.SelectedIndex != (int)Status.All)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    if (materials[i].Status == ConvertStatusToBoolean(value1) && materials[i].Type == value)
                    {
                        materials1.Add(materials[i]);
                    }
                }
            }
            parameter.stkMaterial.Children.Clear();

            foreach (Material material in materials1)
            {
                GoodsViewControl goodsViewControl = new GoodsViewControl();
                goodsViewControl.idMaterial.Text = material.IdMaterial.ToString();
                goodsViewControl.txtNo.Content = material.IdMaterial.ToString();
                goodsViewControl.txtMaterial.Content = material.NameMaterial.ToString();
                goodsViewControl.txtType.Content = material.Type.ToString();
                goodsViewControl.txtQuantity.Content = material.Quantity.ToString();
                goodsViewControl.txtUnit.Content = material.CountUnit.ToString();
                goodsViewControl.txtPrice.Content = material.PurchasePrice.ToString("N0");
                goodsViewControl.txtStatus.Content = ConvertBooleanToStatus(material.Status); ;
                goodsViewControl.txtImg.Source = Converter.Instance.ConvertByteToBitmapImage(material.Image);

                parameter.stkMaterial.Children.Add(goodsViewControl);
            }
        }
    
        private void UpdateMaterial(UpdateGoodsWindow parameter)
        {
            int check = 0;
            for(int i = 0; i < updateMaterials.Count; i++)
            {
                CheckEmptyUpdateMaterial(updateMaterialControls[i]);

                updateMaterials[i].NameMaterial = updateMaterialControls[i].nameMaterial.Text;
                updateMaterials[i].Type = updateMaterialControls[i].cboType.Text;
                updateMaterials[i].CountUnit = updateMaterialControls[i].cboUnit.Text;
                updateMaterials[i].Quantity = double.Parse(updateMaterialControls[i].txtQuantity.Text);
                updateMaterials[i].PurchasePrice = ConvertToNumber(updateMaterialControls[i].txtPrice.Text);
                updateMaterials[i].Status = ConvertStatusToBoolean(updateMaterialControls[i].cbStatus.Text);

                if (MaterialDAL.Instance.UpdateMaterial(updateMaterials[i]))
                {
                    check = 1;
                }
                else
                {

                    CustomMessageBox.Show("Update Material Failed!");
                    parameter.Close();
                    check = 0;
                    LoadMaterial(mainWindow);
                }
            }
            if(check == 1)
            {
                CustomMessageBox.Show("Update Material Success!");
                parameter.Close();
                mainWindow.txtTimeUpdate.Text = DateTime.Now.ToString("hh:mm, MMMM dd yyyy");
                MaterialDAL.Instance.TimeUpdate(DateTime.Now.ToString("hh:mm, MMMM dd yyyy"));
                LoadMaterial(mainWindow);
            }
        }
    
        private void Search(MainWindow parameter)
        {
            if(tbx != "")
            {
                parameter.stkMaterial.Children.Clear();
                for (int i = 0; i < materials.Count; i ++)
                {
                    if(materials[i].NameMaterial.ToLower().Contains(tbx))
                    {
                        GoodsViewControl goodsViewControl = new GoodsViewControl();
                        goodsViewControl.idMaterial.Text = materials[i].IdMaterial.ToString();
                        goodsViewControl.txtNo.Content = materials[i].IdMaterial.ToString();
                        goodsViewControl.txtMaterial.Content = materials[i].NameMaterial.ToString();
                        goodsViewControl.txtType.Content = materials[i].Type.ToString();
                        goodsViewControl.txtQuantity.Content = materials[i].Quantity.ToString();
                        goodsViewControl.txtUnit.Content = materials[i].CountUnit.ToString();
                        goodsViewControl.txtPrice.Content = materials[i].PurchasePrice.ToString("N0");
                        goodsViewControl.txtStatus.Content = ConvertBooleanToStatus(materials[i].Status); ;
                        goodsViewControl.txtImg.Source = Converter.Instance.ConvertByteToBitmapImage(materials[i].Image);

                        parameter.stkMaterial.Children.Add(goodsViewControl);
                    }    
                }    
            }
            else
            {
                LoadMaterial(parameter);
            }
        }
    }
}
