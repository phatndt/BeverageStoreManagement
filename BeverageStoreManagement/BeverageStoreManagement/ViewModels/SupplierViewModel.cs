using BeverageStoreManagement.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BeverageStoreManagement;
using BeverageStoreManagement.Views.Supplier;
using System.Text.RegularExpressions;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.DAL;
using System.Windows;

namespace BeverageStoreManagement.ViewModels
{
    public class SupplierViewModel : BaseViewModel
    {
        private MainWindow mainWindow;

        private List<SupplierControl> listSupplierToView = new List<SupplierControl>();
        public List<SupplierControl> ListSupplierToView { get => listSupplierToView; set => listSupplierToView = value; }

        private ObservableCollection<string> itemSource = new ObservableCollection<string>();
        public ObservableCollection<string> IteamSource { get => itemSource; set { itemSource = value; OnPropertyChanged(); } }

        private List<Supplier> suppliers = new List<Supplier>();
        private List<int> moneys = new List<int>();

        //gridSupplier
        public ICommand OpenAddSupplierWindowCommand { get; set; }
        public ICommand SelectSortTypeCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand LoadSupplierCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        //supplierControl
        public ICommand OpenEditSupplierCommand { get; set; }

        //addSupplierWindow
        public ICommand SaveAddSupplierCommand { get; set; }
        public ICommand ExitAddSupplierCommand { get; set; }

        //addSupplierWindow
        public ICommand SaveEditSupplierCommand { get; set; }
        public ICommand ExitEditSupplierCommand { get; set; }
        public SupplierViewModel()
        {
            OpenAddSupplierWindowCommand = new RelayCommand<MainWindow>(parameter => true, parameter => OpenAddSupplierWindow(parameter));
            SelectSortTypeCommand = new RelayCommand<MainWindow>(parameter => true, parameter => SelectSortType(parameter));
            SortCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Sort(parameter));
            LoadSupplierCommand = new RelayCommand<MainWindow>(parameter => true, parameter => LoadSupplier(parameter));
            SearchCommand = new RelayCommand<MainWindow>(parameter => true, parameter => Search(parameter));

            OpenEditSupplierCommand = new RelayCommand<SupplierControl>(parameter => true, parameter => OpenEditSupplier(parameter));

            SaveAddSupplierCommand = new RelayCommand<AddSupplierWindow>(parameter => true, parameter => SaveAddSupplier(parameter));
            ExitAddSupplierCommand = new RelayCommand<AddSupplierWindow>(parameter => true, parameter => parameter.Close());

            SaveEditSupplierCommand = new RelayCommand<ChangeSupplierWindow>(parameter => true, parameter => SaveEditSupplier(parameter));
            ExitEditSupplierCommand = new RelayCommand<ChangeSupplierWindow>(parameter => true, parameter => parameter.Close());
        }

        #region gridSupplier
        public void LoadSupplier(MainWindow parameter)
        {
            mainWindow = parameter;
            ListSupplierToView.Clear();
            parameter.stkSupplier.Children.Clear();
            suppliers = SupplierDAL.Instance.GetList();
            moneys.Clear();

            int id = 1;
            int totalMoney = 0;

            foreach (Supplier supplier in suppliers)
            {
                SupplierControl supplierControl = new SupplierControl();

                supplierControl.id.Text = supplier.IdSupplier.ToString();
                supplierControl.no.Text = id.ToString();
                supplierControl.txb_supplier_name.Text = supplier.NameSupplier;
                supplierControl.txb_supplier_address.Text = supplier.AddressSupplier;
                supplierControl.txb_supplier_phone.Text = supplier.PhoneSupplier;
                supplierControl.txb_supplier_bill.Text = ImportBillDAL.Instance.GetBillQuantityBySupplier(supplier.IdSupplier).ToString();

                int money = ImportBillDAL.Instance.GetTotalMoneyBySupplier(supplier.IdSupplier);
                supplierControl.txb_total_money.Text = money.ToString();

                ListSupplierToView.Add(supplierControl);

                id++;
                totalMoney += money;
                moneys.Add(money);
            }
            LoadToView(parameter);
            parameter.txbSupplierQuantity.Text = suppliers.Count.ToString();
            parameter.txbTotalSpentToSupplier.Text = totalMoney.ToString();
        }

        private void LoadToView(MainWindow parameter)
        {
            parameter.stkSupplier.Children.Clear();
            for (int i = 0; i < ListSupplierToView.Count; i++)
            {
                parameter.stkSupplier.Children.Add(ListSupplierToView[i]);
            }
        }

        private void OpenAddSupplierWindow(MainWindow parameter)
        {
            int idSupplier = SupplierDAL.Instance.GetMaxIdSupplier() + 1;
            if (idSupplier != 0)
            {
                AddSupplierWindow addSupplierWindow = new AddSupplierWindow();
                addSupplierWindow.txtIdSupplier.Text = idSupplier.ToString();
                addSupplierWindow.ShowDialog();
            }
            else
            {
                Notification.Instance.Failed("Connected to database failed!!!");
            }
        }

        void Sort(MainWindow parameter)
        {

            if (parameter.cboSortType.SelectedIndex == 0)
            {
                switch (parameter.cboSortSupplier.SelectedIndex)
                {
                    case 0:
                        ListSupplierToView = ListSupplierToView.OrderBy(x => x.txb_supplier_name.Text).ToList();
                        break;
                    case 1:
                        ListSupplierToView = ListSupplierToView.OrderByDescending(x => x.txb_supplier_name.Text).ToList();
                        break;
                    default:
                        break;
                }
            }
            else if (parameter.cboSortType.SelectedIndex == 1)
            {
                switch (parameter.cboSortSupplier.SelectedIndex)
                {
                    case 0:
                        ListSupplierToView = ListSupplierToView.OrderBy(x => int.Parse(x.txb_supplier_bill.Text)).ToList();
                        break;
                    case 1:
                        ListSupplierToView = ListSupplierToView.OrderByDescending(x => int.Parse(x.txb_supplier_bill.Text)).ToList();
                        break;
                    default:
                        break;
                }
            }
            else if (parameter.cboSortType.SelectedIndex == 2)
            {
                switch (parameter.cboSortSupplier.SelectedIndex)
                {
                    case 0:
                        ListSupplierToView = ListSupplierToView.OrderBy(x => ConvertToNumber(x.txb_total_money.Text)).ToList();
                        break;
                    case 1:
                        ListSupplierToView = ListSupplierToView.OrderByDescending(x => ConvertToNumber(x.txb_total_money.Text)).ToList();
                        break;
                    default:
                        break;
                }
            }

            LoadToView(parameter);
        }

        void SelectSortType(MainWindow parameter)
        {
            itemSource.Clear();
            switch (parameter.cboSortType.SelectedIndex)
            {
                case 0:
                    parameter.cboSortSupplier.SelectedIndex = -1;
                    itemSource.Add("Từ A -> Z");
                    itemSource.Add("Từ Z -> A");
                    break;
                case 1:
                case 2:
                    parameter.cboSortSupplier.SelectedIndex = -1;
                    itemSource.Add("Tăng dần");
                    itemSource.Add("Giảm dần");
                    break;
                default:
                    break;
            }
        }
        private void Search(MainWindow parameter)
        {
            string search = parameter.txtSearchSupplier.Text;
            ListSupplierToView.Clear();

            int id = 1;
            for (int i = 0; i < suppliers.Count; i++)
            {
                if (suppliers[i].NameSupplier.ToLower().Contains(search.ToLower()))
                {
                    SupplierControl supplierControl = new SupplierControl();

                    supplierControl.id.Text = suppliers[i].IdSupplier.ToString();
                    supplierControl.no.Text = id.ToString();
                    supplierControl.txb_supplier_name.Text = suppliers[i].NameSupplier;
                    supplierControl.txb_supplier_address.Text = suppliers[i].AddressSupplier;
                    supplierControl.txb_supplier_phone.Text = suppliers[i].PhoneSupplier;
                    supplierControl.txb_supplier_bill.Text = ImportBillDAL.Instance.GetBillQuantityBySupplier(suppliers[i].IdSupplier).ToString();

                    int money = moneys[i];
                    supplierControl.txb_total_money.Text = money.ToString();

                    ListSupplierToView.Add(supplierControl);

                    id++;
                }
                Sort(parameter);
            }
        }
        #endregion

        #region supplierControl
        public void OpenEditSupplier(SupplierControl parameter)
        {
            int idSupplier = int.Parse(parameter.id.Text);

            Supplier supplier = SupplierDAL.Instance.GetSupplierById(idSupplier);
            ChangeSupplierWindow changeSupplierWindow = new ChangeSupplierWindow();

            changeSupplierWindow.txtIdSupplier.Text = supplier.IdSupplier.ToString();
            changeSupplierWindow.txtNameSupplier.Text = supplier.NameSupplier;
            changeSupplierWindow.txtAddressSupplier.Text = supplier.AddressSupplier;
            changeSupplierWindow.txtPhoneSupplier.Text = supplier.PhoneSupplier;

            changeSupplierWindow.ShowDialog();
        }
        #endregion

        #region addSupplierWinvdow
        private bool CheckEmptyAddSupplier(AddSupplierWindow parameter)
        {
            if (parameter.txtNameSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameSupplier.Focus();
                return false;
            }
            if (parameter.txtAddressSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter address!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtAddressSupplier.Focus();
                return false;
            }
            if (parameter.txtPhoneSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter phone!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPhoneSupplier.Focus();
                return false;
            }
            if (parameter.txtPhoneSupplier.Text.Length != 10)
            {
                CustomMessageBox.Show("Incorrect phone!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPhoneSupplier.Focus();
                return false;
            }
            return true;
        }

        public void SaveAddSupplier(AddSupplierWindow parameter)
        {
            if (CheckEmptyAddSupplier(parameter))
            {
                int idSupplier = int.Parse(parameter.txtIdSupplier.Text);

                Supplier supplier = new Supplier(
                       idSupplier,
                       parameter.txtNameSupplier.Text,
                       parameter.txtAddressSupplier.Text,
                       parameter.txtPhoneSupplier.Text,
                       false);
                if (SupplierDAL.Instance.AddNewSupplier(supplier) == 1)
                {
                    Notification.Instance.Success("Add Supplier Success!");
                    parameter.Close();
                    LoadSupplier(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Add Supplier Failed!");
                    parameter.Close();
                }
            }
        }
        #endregion

        #region editSupplierWindow
        private bool CheckEmptyEditSupplier(ChangeSupplierWindow parameter)
        {
            if (parameter.txtNameSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter name!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtNameSupplier.Focus();
                return false;
            }
            if (parameter.txtAddressSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter address!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtAddressSupplier.Focus();
                return false;
            }
            if (parameter.txtPhoneSupplier.Text == "")
            {
                CustomMessageBox.Show("Please enter phone!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPhoneSupplier.Focus();
                return false;
            }
            if (parameter.txtPhoneSupplier.Text.Length != 10)
            {
                CustomMessageBox.Show("Incorrect phone!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                parameter.txtPhoneSupplier.Focus();
                return false;
            }
            return true;
        }

        public void SaveEditSupplier(ChangeSupplierWindow parameter)
        {
            if (CheckEmptyEditSupplier(parameter))
            {
                int idSupplier = int.Parse(parameter.txtIdSupplier.Text);

                Supplier supplier = new Supplier(
                       idSupplier,
                       parameter.txtNameSupplier.Text,
                       parameter.txtAddressSupplier.Text,
                       parameter.txtPhoneSupplier.Text,
                       false);
                if (SupplierDAL.Instance.UpdateSupplier(supplier) == 1)
                {
                    Notification.Instance.Success("Update Supplier Success!");
                    parameter.Close();
                    LoadSupplier(mainWindow);
                }
                else
                {
                    Notification.Instance.Failed("Update Supplier Failed!");
                    parameter.Close();
                }
            }
        }
        #endregion
    }
}
