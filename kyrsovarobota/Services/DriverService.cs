using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kyrsovarobota.Services
{
    public class Driver {
       public int DriversID { get; private set; }
       public string Name { get; private set; }
       public string Car { get; private set; }
       public string CarClass { get; private set; }
       
       
       public Driver() { }
        
       public Driver(int driversID, string name, string car, string carClass )
        {
            Name = name;
            Car = car;
            CarClass = carClass;
            DriversID = driversID;
        }

    }


    internal class DriverService
    {
        public static List<Driver> drivers = new List<Driver>
        {
        new Driver(1, "Олександр Коваленко", "Toyota Camry", "Business"),
        new Driver(2, "Марія Петренко", "Skoda Octavia", "Standard"),
        new Driver(3, "Андрій Шевченко", "Mercedes S-Class", "Premium"),
        new Driver(4, "Віталій Кличко", "Audi A6", "Business"),
        new Driver(5, "Олена Бондар", "Volkswagen Golf", "Standard"),
        new Driver(6, "Дмитро Марченко", "BMW 520i", "Business"),
        new Driver(7, "Сергій Ткаченко", "Renault Logan", "Economy"),
        new Driver(8, "Анна Сидоренко", "Hyundai Elantra", "Standard"),
        new Driver(9, "Максим Кушнір", "Tesla Model 3", "Premium"),
        new Driver(10, "Ірина Мороз", "Ford Focus", "Standard"),
        new Driver(11, "Артем Павлов", "Kia Sportage", "Business"),
        new Driver(12, "Юлія Савченко", "Chevrolet Aveo", "Economy")
        };

        public static List<string> CarsClasses = new List<string>
        { "Economy","Standard","Business","Premium"};
    }
}
