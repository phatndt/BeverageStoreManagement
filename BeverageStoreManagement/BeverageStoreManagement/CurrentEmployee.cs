using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageStoreManagement
{
    public static class CurrentEmployee
    {
        private static string name;
        private static DateTime dateOfBirth;
        private static string address;
        private static bool gender;
        private static int idEmployee;
        private static int idPosition;
        private static DateTime dateStartWorking;
        private static String phoneNumber;
        private static bool isDelete;

        public static string Name { get => name; set => name = value; }
        public static DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public static string Address { get => address; set => address = value; }
        public static bool Gender { get => gender; set => gender = value; }
        public static int IdEmployee { get => idEmployee; set => idEmployee = value; }
        public static int IdPosition { get => idPosition; set => idPosition = value; }
        public static DateTime DateStartWorking { get => dateStartWorking; set => dateStartWorking = value; }
        public static string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public static bool IsDelete { get => isDelete; set => isDelete = value; }
    }
}
