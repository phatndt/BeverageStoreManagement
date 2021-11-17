using BeverageStoreManagement.DAL;
using BeverageStoreManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeverageStoreManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Connection connection = new Connection();
            //connection.OpenConnection();
            //this.PagesNavigation. = NavigationCacheMode.Enabled;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //private void rdHome_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new SalePage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/SalePage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdSounds_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new GoodsPage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/GoodsPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdNotes_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new IncidentPage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/IncidentPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdPayment_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new InvoicePage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/InvoicePage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdManage_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new PaymentVoucherPage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/PaymentVoucherPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdReceipt_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new ReceiptVoucherPage();
        //    //PagesNavigation.Navigate(new System.Uri("Views/Pages/ReceiptVoucherPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdEmployee_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Content = new EmployeePage();
        //}
        
        //private void PagesNavigation_Navigated(object sender, NavigationEventArgs e)
        //{

        //}

        //private void rdProduct_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Navigate(new System.Uri("Views/Pages/ProductPage.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void rdAcount_Click(object sender, RoutedEventArgs e)
        //{
        //    PagesNavigation.Navigate(new System.Uri("Views/Pages/AccountManagement.xaml", UriKind.RelativeOrAbsolute));
        //}
    }
}
