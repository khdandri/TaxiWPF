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
        public double Distance { get; set; } // Кількість км
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public Orders() { } // Конструктор за замовчуванням
    }
}
