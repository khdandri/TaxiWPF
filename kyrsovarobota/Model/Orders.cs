using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kyrsovarobota.Model
{
    public class Orders
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public double Distance { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string DriverName { get; set; } // Нове поле
        public DateTime CreatedAt { get; set; } // Нове поле

        public Orders() { } // Конструктор за замовчуванням
    }
}
