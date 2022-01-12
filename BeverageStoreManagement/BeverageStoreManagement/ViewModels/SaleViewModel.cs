using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Models;
using BeverageStoreManagement.Resources.UserControls;
using BeverageStoreManagement.Views;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BeverageStoreManagement.ViewModels
{
    public class SaleViewModel : BaseViewModel
    {
        private MainWindow mainWindow;

        private string total = "15,0000";

        public string Total { get => total; set { total = value; OnPropertyChanged(); } }

        private string returnMoney = "15,0000";

        public string ReturnMoney { get => returnMoney; set { returnMoney = value; OnPropertyChanged(); } }

        private string datez = "150000";

        public string Datez { get => datez; set { datez = value; OnPropertyChanged(); } }

        private string idInvoice = "";

        public string IdInvoice { get => idInvoice; set { idInvoice = value; OnPropertyChanged(); } }

        private List<Product> products = new List<Product>();

        private List<InvoiceInfo> invoiceInfos = new List<InvoiceInfo>();

        public ICommand SeparateThousandsCommand { get; set; }

        private List<ProductSale> listProductToView = new List<ProductSale>();
        public List<ProductSale> ListProductToView { get => listProductToView; set => listProductToView = value; }

        //AddInvoiceWindow
        public ICommand OpenAddInvoiceWindowCommand { get; set; }

        //grd
        public ICommand LoadProductCommand { get; set; }
        public ICommand SearchProductCommand { get; set; }
        public ICommand SortProductCommand { get; set; }
        public ICommand CalculateReturnMoneyCommand { get; set; }
        public ICommand SaveInvoiceCommand { get; set; }

        //public ICommand LoadingCommand { get; set; }

        //ProductSale
        public ICommand AddProductToInvoiceCommand { get; set; }

        //InvoiceInfoControl
        public ICommand DeleteProductToInvoiceCommand { get; set; }
        public ICommand InvoiceInfoChangeCommand { get; set; }

        public SaleViewModel()
        {
            SeparateThousandsCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => SeparateThousands(parameter));
            OpenAddInvoiceWindowCommand = new RelayCommand<InvoicePage>((parameter) => true, (parameter) => OpenAddInvoiceWindow(parameter));

            LoadProductCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => LoadProduct(parameter));
            SearchProductCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SearchProduct(parameter));
            SortProductCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SortProduct(parameter));
            CalculateReturnMoneyCommand = new RelayCommand<TextBox>((parameter) => true, (parameter) => CalculateReturnMoney(parameter));
            SaveInvoiceCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => SaveInvoice(parameter));
            //LoadingCommand = new RelayCommand<MainWindow>((parameter) => true, (parameter) => Loading(parameter));

            AddProductToInvoiceCommand = new RelayCommand<ProductSale>((parameter) => true, (parameter) => AddProductToInvoice(parameter));

            DeleteProductToInvoiceCommand = new RelayCommand<InvoiceInfoControl>((parameter) => true, (parameter) => DeleteProductToInvoice(parameter));
            InvoiceInfoChangeCommand = new RelayCommand<InvoiceInfoControl>((parameter) => true, (parameter) => InvoiceInfoChange(parameter));
        }

        #region grdSale
        public void LoadProduct(MainWindow parameter)
        {
            mainWindow = parameter;
            parameter.wrpGoods.Children.Clear();
            ListProductToView.Clear();
            products.Clear();
            products = (List<Product>)ProductDAL.Instance.GetList();

            Loading();

            foreach (Product product in products)
            {
                ProductSale productSale = new ProductSale();

                productSale.idProduct.Text = product.IdProduct.ToString();
                productSale.nameProduct.Content = product.NameProduct.ToString();
                productSale.priceProduct.Content = product.Price.ToString("N0");
                productSale.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                ListProductToView.Add(productSale);
            }
            LoadToView(parameter);

            List<string> itemType = ProductDAL.Instance.GetNameType();
            itemType.Add("All");
            parameter.txtSort.ItemsSource = itemType;
        }

        private void LoadToView(MainWindow parameter)
        {
            for (int i = 0; i < ListProductToView.Count; i++)
            {
                parameter.wrpGoods.Children.Add(ListProductToView[i]);
            }
        }

        private void SearchProduct(MainWindow parameter)
        {
            if (parameter.txtSearch.Text != "")
            {
                parameter.wrpGoods.Children.Clear();
                ListProductToView.Clear();
                for (int i = 0; i < products.Count; i++)
                {
                    string name = products[i].NameProduct;
                    if (name.ToLower().Contains(parameter.txtSearch.Text.ToLower()))
                    {
                        ProductSale productSale = new ProductSale();

                        productSale.idProduct.Text = products[i].IdProduct.ToString();
                        productSale.nameProduct.Content = products[i].NameProduct.ToString();
                        productSale.priceProduct.Content = products[i].Price.ToString("N0");
                        productSale.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(products[i].Image);

                        ListProductToView.Add(productSale);
                    }
                }
                LoadToView(parameter);
            }
            else
            {
                parameter.wrpGoods.Children.Clear();
                ListProductToView.Clear();
                foreach (Product product in products)
                {
                    ProductSale productSale = new ProductSale();

                    productSale.idProduct.Text = product.IdProduct.ToString();
                    productSale.nameProduct.Content = product.NameProduct.ToString();
                    productSale.priceProduct.Content = product.Price.ToString("N0");
                    productSale.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                    ListProductToView.Add(productSale);
                }
                LoadToView(parameter);
            }
        }

        private void SortProduct(MainWindow parameter)
        {
            parameter.wrpGoods.Children.Clear();
            ListProductToView.Clear();
            foreach (Product product in products)
            {
                int id = product.IdProductType;
                if (id == parameter.txtSort.SelectedIndex + 1)
                {
                    ProductSale productSale = new ProductSale();

                    productSale.idProduct.Text = product.IdProduct.ToString();
                    productSale.nameProduct.Content = product.NameProduct.ToString();
                    productSale.priceProduct.Content = product.Price.ToString("N0");
                    productSale.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                    ListProductToView.Add(productSale);
                }
            }
            if (parameter.txtSort.SelectedIndex + 1 == parameter.txtSort.Items.Count)
            {
                foreach (Product product in products)
                {
                    ProductSale productSale = new ProductSale();

                    productSale.idProduct.Text = product.IdProduct.ToString();
                    productSale.nameProduct.Content = product.NameProduct.ToString();
                    productSale.priceProduct.Content = product.Price.ToString("N0");
                    productSale.imgProduct.Source = Converter.Instance.ConvertByteToBitmapImage(product.Image);

                    ListProductToView.Add(productSale);
                }
            }
            LoadToView(parameter);
        }
        #endregion

        //AddInvoiceWindow
        private void OpenAddInvoiceWindow(InvoicePage parameter)
        {
            AddInvoiceWindow addInvoiceWindow = new AddInvoiceWindow();
            addInvoiceWindow.lbDate.Content = DateTime.Now.ToString("dd/MM/yyyy");
            addInvoiceWindow.ShowDialog();
        }

        #region ProductSale
        private void AddProductToInvoice(ProductSale parameter)
        {
            double price = ConvertToNumber(parameter.priceProduct.Content.ToString());
            bool check = true;
            foreach (InvoiceInfo invoice in invoiceInfos)
            {
                if (invoice.IdGood.ToString() == parameter.idProduct.Text)
                {
                    CustomMessageBox.ShowOK("Exist product in bill", "Information", "OK");
                    check = false;
                }
            }
            if (check)
            {
                double intoMoney = price * 1;
                InvoiceInfo invoiceInfo = new InvoiceInfo(1, int.Parse(IdInvoice), int.Parse(parameter.idProduct.Text), 1, price, intoMoney);
                invoiceInfos.Add(invoiceInfo);
            }
            LoadInvoiceInfo(mainWindow);
        }
        #endregion

        #region InvoiceInfoControl
        private void DeleteProductToInvoice(InvoiceInfoControl parameter)
        {
            int index = int.Parse(parameter.id.Text) - 1;
            invoiceInfos.RemoveAt(index);
            LoadInvoiceInfo(mainWindow);
        }

        private void InvoiceInfoChange(InvoiceInfoControl parameter)
        {
            int index = int.Parse(parameter.id.Text) - 1;
            for (int i = 0; i < invoiceInfos.Count; i++)
            {
                if (index == i)
                {
                    invoiceInfos[i].Quantity = int.Parse(parameter.numberic.Text.ToString());
                    invoiceInfos[i].IntoMoney = int.Parse(parameter.numberic.Text.ToString()) * invoiceInfos[i].Price;
                }
            }
            LoadInvoiceInfo(mainWindow);
        }
        #endregion

        private void LoadInvoiceInfo(MainWindow parameter)
        {
            parameter.stkxxxxxBill.Children.Clear();
            for (int i = 0; i < invoiceInfos.Count; i++)
            {
                InvoiceInfoControl invoiceInfoControl = new InvoiceInfoControl();
                int id = i + 1;
                invoiceInfoControl.id.Text = id.ToString();
                invoiceInfoControl.no.Text = id.ToString();
                invoiceInfoControl.name.Text = getName(invoiceInfos[i]);
                invoiceInfoControl.price.Text = SeparateThousands(invoiceInfos[i].Price.ToString());
                invoiceInfoControl.numberic.Text = decimal.Parse(invoiceInfos[i].Quantity.ToString());
                invoiceInfoControl.total.Text = SeparateThousands(invoiceInfos[i].IntoMoney.ToString());
                parameter.stkxxxxxBill.Children.Add(invoiceInfoControl);
            }
            CalculateTotalMoney();
        }

        private string getName(InvoiceInfo invoiceInfo)
        {
            foreach (Product product in products)
            {
                if (product.IdProduct == invoiceInfo.IdGood)
                {
                    return product.NameProduct;
                }
            }
            return "No name";
        }

        public void Loading()
        {
            Datez = DateTime.Now.ToString("dd/MM/yyyy");
            int id = InvoiceDAL.Instance.GetMaxIdInvoice() + 1;
            IdInvoice = id.ToString();
        }
        public void SeparateThousands(TextBox txt)
        {
            if (!string.IsNullOrEmpty(txt.Text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(ConvertToNumber(txt.Text).ToString(), System.Globalization.NumberStyles.AllowThousands);
                txt.Text = String.Format(culture, "{0:N0}", valueBefore);
                txt.Select(txt.Text.Length, 0);
            }
        }

        public void CalculateTotalMoney()
        {
            double money = 0;
            foreach (InvoiceInfo invoiceInfo in invoiceInfos)
            {
                money += invoiceInfo.IntoMoney;
            }
            Total = SeparateThousands(money.ToString());
        }

        public void CalculateReturnMoney(TextBox parameter)
        {
            if (parameter.Text != "")
            {
                double returnMoney =  ConvertToNumber(parameter.Text) - ConvertToNumber(Total);
                if (returnMoney >= 0)
                {
                    ReturnMoney = SeparateThousands(returnMoney.ToString());
                }
                else
                {
                    ReturnMoney = "ERROR";
                }
            }
            else
            {
                ReturnMoney = "ERROR";
            }
        }

        public void SaveInvoice(MainWindow parameter)
        {
            if (invoiceInfos.Count != 0) 
            {
                bool status;
                if (parameter.rdoOnSpot.IsChecked == true)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                Invoice invoice = new Invoice(
                    int.Parse(IdInvoice),
                    CurrentAccount.IdEmployee,
                    DateTime.Now,
                    ConvertToNumber(Total),
                    ConvertToNumber(parameter.txtMoneyCustomer.Text),
                    int.Parse(parameter.txtIdTable.Text),
                    status,
                    false);
                if (InvoiceDAL.Instance.AddNewInvoice(invoice) == 1)
                {
                    foreach (InvoiceInfo invoiceInfo in invoiceInfos)
                    {
                        int id = InvoiceInfoDAL.Instance.GetMaxIdInvoiceInfo() + 1;
                        invoiceInfo.IdInvoiceInfo = id;
                        InvoiceInfoDAL.Instance.AddNewInvoiceInvoiceInfo(invoiceInfo);
                    }
                    CustomMessageBox.ShowOK("Add invoice success", "Information", "Oke");
                    ResetData(parameter);
                }
                else
                {
                    Notification.Instance.Success("Error!");
                }
            } else
            {
                CustomMessageBox.ShowOK("No products in invoice", "Information", "Oke");
            }
        }

        public void ResetData(MainWindow mainWindow)
        {
            Loading();
            Total = "0";
            ReturnMoney = "0";
            mainWindow.txtMoneyCustomer.Text = "";
            mainWindow.txtIdTable.Text = "";
            mainWindow.rdoTakeAway.IsChecked = false;
            mainWindow.rdoOnSpot.IsChecked = false;
            invoiceInfos.Clear();
        }
    }
}
