using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using kyrsovarobota.Model;

namespace kyrsovarobota
{

    public class OrderManager
    {
        private List<Orders> _orders = new List<Orders>();
        private const string FilePath = "orders.json";

        public OrderManager() => Load();

        public void AddOrder(Orders orders)
        {
            orders.Id = _orders.Count > 0 ? _orders.Max(o => o.Id) + 1 : 1;
            _orders.Add(orders);
            Save();
        }

        public List<Orders> GetAll() => _orders;

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(_orders, options));
        }

        private void Load()
        {
            if (!File.Exists(FilePath)) return;
            try
            {
                string json = File.ReadAllText(FilePath);
                _orders = JsonSerializer.Deserialize<List<Orders>>(json) ?? new List<Orders>();
            }
            catch { _orders = new List<Orders>(); }
        }
    }
}
