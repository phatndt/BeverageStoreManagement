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
        private double totalPending = 0;
        public double TotalPending { get => totalPending; set { totalPending = value; OnPropertyChanged(); } }

        private string phoneSupplier = "";
        public string PhoneSupplier { get => phoneSupplier; set { phoneSupplier = value; OnPropertyChanged(); } }
        private string gmailSupplier = "";
        public string GmailSupplier { get => gmailSupplier; set { gmailSupplier = value; OnPropertyChanged(); } }
        string tbx = "";
        public string Tbx { get => tbx; set { tbx = value; OnPropertyChanged(); } }
        private List<ImportBillControl> importBillControls = new List<ImportBillControl>();
        private List<ImportBillControl> importBillPendingControls = new List<ImportBillControl>();
        private AddImportBill addImport;
        private MainWindow mainWindow;
        private CheckPendingBillWindow checkPendingBillWindow;

        private List<AddMaterialImportBillControl> addMaterials = new List<AddMaterialImportBillControl>();
        public ICommand OpenAddImportBillCommand { get; set; }
        public ICommand LoadMaterialCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand GetSupplierCommand { get; set; }
        public ICommand CalculateMoneyCommand { get; set; }
        public ICommand CalculateTotalCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddBillCommand { get; set; }
        public ICommand LoadPendingCommand { get; set; }
        public ICommand LoadImportedCommand { get; set; }
        public ICommand OpenCheckPendingBillCommand { get; set; }
        public ICommand LoadCheckPendingWindowCommand { get; set; }
        public ICommand DeleteUCCommand { get; set; }
        public ICommand CalculateTotalPendingCommand { get; set; }
        public ICommand LoadImportedUCWindowCommand { get; set; }
        public ICommand UpdateStatusCommand { get; set; }


        public ImportBillViewModel()
        {
            OpenAddImportBillCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddImportWindow(parameter));
            LoadMaterialCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => LoadMaterial(parameter));
            AddCommand = new RelayCommand<AddMaterialImportBillControl>(parameter => true, parameter => AddMaterial(parameter));
            GetSupplierCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => GetSupplier(parameter));
            CalculateMoneyCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => CalculateIntoMoney(parameter));
            CalculateTotalCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => CalculateTotal(parameter));
            SearchCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => Search(parameter));
            AddBillCommand = new RelayCommand<AddImportBill>(parameter => true, parameter => AddBillToDB(parameter));
            LoadPendingCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadPendingRequest(parameter));
            LoadImportedCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadImportedRequest(parameter));
            OpenCheckPendingBillCommand = new RelayCommand<ImportMaterialControl>(parameter => true, parameter => OpenCheckPendingBill(parameter));
            LoadCheckPendingWindowCommand = new RelayCommand<CheckPendingBillWindow>(parameter => true, parameter => LoadCheckPending(parameter));
            DeleteUCCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => DeleteUC(parameter));
            CalculateTotalPendingCommand = new RelayCommand<ImportBillControl>(parameter => true, parameter => CalculateTotalPending(parameter));
            LoadImportedUCWindowCommand = new RelayCommand<ImportedMaterialControl>(parameter => true, parameter => LoadImportedBill(parameter));
            UpdateStatusCommand = new RelayCommand<CheckPendingBillWindow>(parameter => true, parameter => UpdateStatus(parameter));
        }

        public Employee GetEmployee(int i)
        {
             return EmployeeDAL.Instance.GetEmployeeById(i);
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
                addImportBill.txbName.Text = GetEmployee(CurrentAccount.IdAccount).Name;
                addImportBill.txbId.Text = "#" + GetEmployee(CurrentAccount.IdAccount).IdEmployee;
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

        private void LoadPendingRequest(MainWindow parameter)
        {
            this.mainWindow = parameter;
            parameter.stkImportMaterial.Children.Clear();

            List<ImportBill> importBills = (List<ImportBill>)ImportBillDAL.Instance.GetListPending();
            foreach(ImportBill importBill in importBills)
            {
                ImportMaterialControl import = new ImportMaterialControl();
                import.idBill.Content = importBill.IdImportBill;
                import.nameEmployee.Content = GetEmployee(importBill.IdEmployee).Name;
                import.nameSupplier.Content = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).NameSupplier;
                import.date.Content = importBill.Date.ToString("hh:mm MMMM dd yyyy");
                import.money.Content = importBill.TotalMoney.ToString("N0");

                parameter.stkImportMaterial.Children.Add(import);
            }    
        }

        private void LoadImportedRequest(MainWindow parameter)
        {
            parameter.stkImportedMaterial.Children.Clear();

            List<ImportBill> importBills = (List<ImportBill>)ImportBillDAL.Instance.GetListImported();
            foreach (ImportBill importBill in importBills)
            {
                ImportedMaterialControl import = new ImportedMaterialControl();
                import.idBill.Content = importBill.IdImportBill;
                import.nameEmployee.Content = GetEmployee(importBill.IdEmployee).Name;
                import.nameSupplier.Content = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).NameSupplier;
                import.date.Content = importBill.Date.ToString("hh:mm MMMM dd yyyy");
                import.money.Content = importBill.TotalMoney.ToString("N0");

                parameter.stkImportedMaterial.Children.Add(import);
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
            if(addImport == null)
            {

            }
            else
            {
                Total = 0;
                for (int i = 0; i < addImport.stkImportBill.Children.Count; i++)
                {
                    Total += ConvertToNumber(importBillControls[i].money.Text);
                }
            }   
        }

        private void DeleteUC(ImportBillControl parameter)
        {
            if(addImport == null || addImport.stkImportBill.Children.Count == 0)
            {
                checkPendingBillWindow.stkImportBill.Children.Remove(parameter);
                importBillPendingControls.Remove(parameter);
                CalculateTotalPending(parameter);
            }
            else
            {
                addImport.stkImportBill.Children.Remove(parameter);
                importBillControls.Remove(parameter);
                CalculateTotal(parameter);
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
    
        private void AddBillToDB(AddImportBill parameter)
        {
            if(parameter.b.Text == "")
            {
                CustomMessageBox.Show("Please choose the supplier!");
            }
            else
            {
                ImportBill importBill = new ImportBill();
                importBill.IdImportBill = int.Parse(parameter.txtIdImportBill.Text);
                importBill.IdEmployee = CurrentAccount.IdAccount;
                importBill.IdSupplier = parameter.cbSupplier.SelectedIndex + 1;
                importBill.Date = DateTime.Parse(parameter.txbTime.Text);
                importBill.TotalMoney = double.Parse(parameter.txbTotal.Text);
                importBill.Note = parameter.txtNote.Text;
                importBill.Status = true;
                importBill.IsDelete = false;

                ImportBillDAL.Instance.AddImportBillIntoDB(importBill);

                for (int i = 0; i < importBillControls.Count; i++)
                {
                    ImportBillInfo billInfo = new ImportBillInfo();

                    billInfo.IdImportBill = int.Parse(parameter.txtIdImportBill.Text);
                    billInfo.IdImportBillInfo = i + 1;
                    billInfo.IdMaterial = int.Parse(importBillControls[i].idMaterial.Text);
                    billInfo.Quantity = int.Parse(importBillControls[i].quantity.Text);
                    billInfo.Price = double.Parse(importBillControls[i].price.Text);
                    billInfo.Unit = importBillControls[i].unit.Text;
                    billInfo.IntoMoney = double.Parse(importBillControls[i].money.Text);

                    ImportBillDAL.Instance.AddImportBillInfoIntoDB(billInfo);
                }

                CustomMessageBox.Show("Send request successfully");
                parameter.Close();
            }
        }

        private void UpdateStatus(CheckPendingBillWindow parameter)
        {
            ImportBillDAL.Instance.UpdateStatus(parameter.txtIdImportBill.Text);
            LoadImportedRequest(mainWindow);
            LoadPendingRequest(mainWindow);

            CustomMessageBox.Show("Paid Succesfully");

            parameter.Close();
        }
        
        private void LoadCheckPending(CheckPendingBillWindow parameter)
        {
            this.checkPendingBillWindow = parameter;

            parameter.stkImportBill.Children.Clear();
            List<ImportBillInfo> importBillInfos = (List<ImportBillInfo>)ImportBillDAL.Instance.GetImportInfoBill(parameter.txtIdImportBill.Text);
            foreach (ImportBillInfo importBillInfo in importBillInfos)
            {
                ImportBillControl import = new ImportBillControl();
                import.idMaterial.Text = importBillInfo.IdMaterial.ToString();
                import.number.Text = importBillInfo.IdImportBillInfo.ToString();
                import.nameMaterial.Text = MaterialDAL.Instance.GetMaterial(importBillInfo.IdMaterial.ToString()).NameMaterial;
                import.price.Text = importBillInfo.Price.ToString();
                import.unit.Text = importBillInfo.Unit.ToString();
                import.quantity.Text = importBillInfo.Quantity.ToString();
                import.money.Text = importBillInfo.IntoMoney.ToString("N0");

                importBillPendingControls.Add(import);
                parameter.stkImportBill.Children.Add(import);

            }
        }

        private void OpenCheckPendingBill(ImportMaterialControl parameter)
        {
            ImportBill importBill = ImportBillDAL.Instance.GetImportBill(parameter.idBill.Content.ToString());

            CheckPendingBillWindow checkPendingBillWindow = new CheckPendingBillWindow();

            checkPendingBillWindow.txbId.Text = importBill.IdEmployee.ToString();
            checkPendingBillWindow.txbName.Text = GetEmployee(importBill.IdEmployee).Name;
            checkPendingBillWindow.cbSupplier.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).NameSupplier;
            checkPendingBillWindow.b.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).PhoneSupplier;
            checkPendingBillWindow.c.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).AddressSupplier;
            checkPendingBillWindow.txtIdImportBill.Text = importBill.IdImportBill.ToString();
            checkPendingBillWindow.txbTime.Text = importBill.Date.ToString("hh:mm, MMMM dd yyyy");
            checkPendingBillWindow.txtNote.Text = importBill.Note.ToString();
            TotalPending = importBill.TotalMoney;

            checkPendingBillWindow.ShowDialog();
        }

        private void CalculateTotalPending(ImportBillControl parameter)
        {
            if(checkPendingBillWindow == null)
            {

            }
            else
            {
                TotalPending = 0;
                for (int i = 0; i < checkPendingBillWindow.stkImportBill.Children.Count; i++)
                {
                    TotalPending += ConvertToNumber(importBillPendingControls[i].money.Text);
                }
            }
        }
    
        private void LoadImportedBill(ImportedMaterialControl parameter)
        {
            ImportBill importBill = ImportBillDAL.Instance.GetImportBill(parameter.idBill.Content.ToString());

            CheckPendingBillWindow checkPendingBillWindow = new CheckPendingBillWindow();

            checkPendingBillWindow.txbId.Text = importBill.IdEmployee.ToString();
            checkPendingBillWindow.txbName.Text = GetEmployee(importBill.IdEmployee).Name;
            checkPendingBillWindow.cbSupplier.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).NameSupplier;
            checkPendingBillWindow.b.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).PhoneSupplier;
            checkPendingBillWindow.c.Text = SupplierDAL.Instance.GetSupplierById(importBill.IdSupplier).AddressSupplier;
            checkPendingBillWindow.txtIdImportBill.Text = importBill.IdImportBill.ToString();
            checkPendingBillWindow.txbTime.Text = importBill.Date.ToString("hh:mm, MMMM dd yyyy");
            checkPendingBillWindow.txtNote.Text = importBill.Note.ToString();
            TotalPending = importBill.TotalMoney;

            checkPendingBillWindow.stkImportBill.Children.Clear();
            List<ImportBillInfo> importBillInfos = (List<ImportBillInfo>)ImportBillDAL.Instance.GetImportInfoBill(checkPendingBillWindow.txtIdImportBill.Text);
            foreach (ImportBillInfo importBillInfo in importBillInfos)
            {
                CompletedImportBillControl importBillControl = new CompletedImportBillControl();
                importBillControl.idMaterial.Text = importBillInfo.IdMaterial.ToString();
                importBillControl.number.Text = importBillInfo.IdImportBillInfo.ToString();
                importBillControl.nameMaterial.Text = MaterialDAL.Instance.GetMaterial(importBillInfo.IdMaterial.ToString()).NameMaterial;
                importBillControl.price.Text = importBillInfo.Price.ToString();
                importBillControl.unit.Text = importBillInfo.Unit.ToString();
                importBillControl.quantity.Text = importBillInfo.Quantity.ToString();
                importBillControl.money.Text = importBillInfo.IntoMoney.ToString("N0");

                checkPendingBillWindow.stkImportBill.Children.Add(importBillControl);

            }

            checkPendingBillWindow.btnPay.Visibility = System.Windows.Visibility.Hidden;

            checkPendingBillWindow.ShowDialog();
        }
    }
}
