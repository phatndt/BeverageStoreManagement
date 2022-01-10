using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BeverageStoreManagement.ViewModels
{
    class ImportBillViewModel : BaseViewModel
    {

        private double total = 0;
        public double Total { get => total; set { total = value; OnPropertyChanged(); } }

        private string phoneSupplier = "";
        public string PhoneSupplier { get => phoneSupplier; set { phoneSupplier = value; OnPropertyChanged(); } }
        private string gmailSupplier = "";
        public string GmailSupplier { get => gmailSupplier; set { gmailSupplier = value; OnPropertyChanged(); } }
        string tbx = "";
        public string Tbx { get => tbx; set { tbx = value; OnPropertyChanged(); } }
        private List<ImportBillControl> importBillControls = new List<ImportBillControl>();
        private AddImportBill addImport;

        private List<AddMaterialImportBillControl> addMaterials = new List<AddMaterialImportBillControl>();
        public ICommand OpenAddImportBillCommand { get; set; }
        public ICommand LoadMaterialCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand GetSupplierCommand { get; set; }
        public ICommand CalculateMoneyCommand { get; set; }
        public ICommand CalculateTotalCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ImportBillViewModel()
        {
            OpenAddImportBillCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddImportWindow(parameter));
            LoadMaterialCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => LoadMaterial(parameter));
            AddCommand = new RelayCommand<AddMaterialImportBillControl>(parameter => true, parameter => AddMaterial(parameter));
            GetSupplierCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => GetSupplier(parameter));
            CalculateMoneyCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => CalculateIntoMoney(parameter));
            CalculateTotalCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => CalculateTotal(parameter));
            SearchCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => Search(parameter));
        }

        public Employee GetEmployee()
        {
             return EmployeeDAL.Instance.GetEmployeeById(CurrentAccount.IdAccount);
        }

        public void GetSupplier(AddImportBill parameter)
        {
            Supplier a = SupplierDAL.Instance.GetSupplierById(addImport.cbSupplier.SelectedIndex + 1);
            PhoneSupplier = a.PhoneSupplier;
            GmailSupplier = a.AddressSupplier;
        }

        private void OpenAddImportWindow(MainWindow parameter)
        {
            Total = 0;
            AddImportBill addImportBill = new AddImportBill();
            try
            {
                int id = ImportBillDAL.Instance.GetMaxIdImportBill() + 1;
                addImportBill.txtIdImportBill.Text = id.ToString();
                addImportBill.txbName.Text = GetEmployee().Name;
                addImportBill.txbId.Text = "#" + GetEmployee().IdEmployee;
                addImportBill.cbSupplier.ItemsSource = GetSupplierSoure();
            }
            catch
            {
                addImportBill.txtIdImportBill.Text = "1";
            }
            addImportBill.txbTime.Text = DateTime.Now.ToString("hh:mm, MMMM dd yyyy");
            addImportBill.ShowDialog();
        }

        private void LoadMaterial(AddImportBill parameter)
        {
            this.addImport = parameter;
            parameter.wrpMaterial.Children.Clear();

            List<Material> materials = (List<Material>)MaterialDAL.Instance.GetList();

            foreach (Material material in materials)
            {
                AddMaterialImportBillControl add = new AddMaterialImportBillControl();
                add.idProduct.Text = material.IdMaterial.ToString();
                add.Name.Text = material.NameMaterial.ToString();
                add.unit.Text = material.CountUnit.ToString();
                add.Quantity.Text = "Remain: " + material.Quantity.ToString();
                add.Price.Text = material.PurchasePrice.ToString();
                add.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(material.Image);

                addMaterials.Add(add);

                parameter.wrpMaterial.Children.Add(add);
            }
        }

        private List<string> GetSupplierSoure()
        {
            List<string> supplierSources = new List<string>();
            foreach (Supplier supplier in SupplierDAL.Instance.GetList())
            {
                supplierSources.Add(supplier.NameSupplier);
            }
            return supplierSources;
        }

        private void AddMaterial(AddMaterialImportBillControl parameter)
        {
            ImportBillControl import = new ImportBillControl();
            int check = 0;
            for(int i = 0; i < addImport.stkImportBill.Children.Count; i++)
            {
                if (parameter.idProduct.Text == importBillControls[i].idMaterial.Text)
                    check = 1;
            }
            if(check == 0)
            {
                import.idMaterial.Text = parameter.idProduct.Text;
                import.number.Text = (addImport.stkImportBill.Children.Count + 1).ToString();
                import.nameMaterial.Text = parameter.Name.Text;
                import.price.Text = parameter.Price.Text;
                import.unit.Text = parameter.unit.Text;
                import.quantity.Text = "1";
                import.money.Text = (double.Parse(import.quantity.Text) * double.Parse(ConvertToNumber(import.price.Text).ToString())).ToString("N0");

                importBillControls.Add(import);

                addImport.stkImportBill.Children.Add(import);

                Total += double.Parse(import.money.Text);
            }
            else
            {
                CustomMessageBox.Show("This material have been already existed in import list!");
            }
        }

        private void CalculateIntoMoney(ImportBillControl parameter)
        {
            double intoMoney;
            if (parameter.quantity.Text == "" || parameter.price.Text == "")
            {
                intoMoney = 0;
            }
            else
            {
                intoMoney = double.Parse(parameter.quantity.Text) * double.Parse(ConvertToNumber(parameter.price.Text).ToString());
            }
            parameter.money.Text = intoMoney.ToString("N0");
        }
    
        private void CalculateTotal(ImportBillControl parameter)
        {
            Total = 0;
            for (int i = 0; i < addImport.stkImportBill.Children.Count; i++)
            {
                Total += ConvertToNumber(importBillControls[i].money.Text);
            }    
        }

        private void Search(AddImportBill parameter)
        {
            if (tbx != "")
            {
                parameter.wrpMaterial.Children.Clear();
                for (int i = 0; i < addMaterials.Count; i++)
                {
                    if (addMaterials[i].Name.Text.ToLower().Contains(tbx))
                    {
                        AddMaterialImportBillControl add = new AddMaterialImportBillControl();
                        add.idProduct.Text = addMaterials[i].idProduct.Text.ToString();
                        add.Name.Text = addMaterials[i].Name.Text.ToString();
                        add.unit.Text = addMaterials[i].unit.Text.ToString();
                        add.Quantity.Text = addMaterials[i].Quantity.Text.ToString();
                        add.Price.Text = addMaterials[i].Price.Text.ToString();
                        add.imgProduct.Source = addMaterials[i].imgProduct.Source;

                        parameter.wrpMaterial.Children.Add(add);
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
