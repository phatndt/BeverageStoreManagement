using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;


namespace BeverageStoreManagement.ViewModels
{
    public class BaseViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string MD5Hash(string str)
        {
            StringBuilder hash = new StringBuilder();
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("X2"));
            }
            return hash.ToString();
        }
        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            return long.Parse(tmp);
        }
        public BitmapImage ConvertByteToBitmapImage(Byte[] image)
        {
            BitmapImage bi = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            if (image == null)
            {
                return null;
            }
            stream.Write(image, 0, image.Length);
            stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public Byte[] ConvertImageToBytes(string imageFileName)
        {
            FileStream fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();
            return imgByteArr;
        }

        public bool ConvertStatusToBoolean(string a)
        {
            if (a == "Available")
                return true;
            if (a == "Unavailable")
                return false;
            return true;
        }
        public string ConvertBooleanToStatus(bool a)
        {
            if (a == true)
                return "Available";
            if (a == false)
                return "Unavailable";
            return "";
        }

        public int ConvertTypeToInt(string a)
        {
            if (a == "Coffee")
                return 1;
            if (a == "Tea")
                return 2;
            if (a == "Milk Tea")
                return 3;
            if (a == "Fruit Tea")
                return 4;
            if (a == "Snacks")
                return 5;
            return 0;
        }
        
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(ConvertToNumber(text).ToString(), System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        
        public void NumberValidationTextBox(object sender, KeyEventArgs e)
        {

            e.Handled = e.Key == Key.Space;
        }
        
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        
        public string ConvertIntToType(int? a)
        {
            if (a == 1)
                return "Coffee";
            if (a == 2)
                return "Tea";
            if (a == 3)
                return "Milk Tea";
            if (a == 4)
                return "Fruit Tea";
            if (a == 5)
                return "Snacks";
            return "";
        }

        public bool ConvertToStatus(double a)
        {
            if (a > 0)
                return true;
            else
                return false;
        }
    }
}
