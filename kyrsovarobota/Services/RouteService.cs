using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kyrsovarobota
{
    public static class RouteService
    {
        // Список доступних вулиць
        public static List<string> Streets = new List<string>
    {
        "вул. Соборна", "вул. Київська", "вул. Перемоги", "вул. Шевченка",
        "вул. Лесі Українки", "вул. Миру", "вул. Полтавська", "пр. Свободи",
        "вул. Козацька", "вул. Садова", "вул. Зелена", "вул. Вишнева",
        "вул. Набережна", "вул. Гагаріна", "вул. Європейська"
    };

        // Метод для отримання фіксованої відстані між двома точками
        public static double GetDistance(string from, string to)
        {
            if (from == to) return 0;

            // Імітація бази даних відстаней 
            // Для прикладу: використовуємо індекси масиву для генерації відстані
            int index1 = Streets.IndexOf(from);
            int index2 = Streets.IndexOf(to);

            // Формула для детермінованої, але "різної" відстані
            return Math.Abs(index1 - index2) * 2.5 + 3.0;//2.5 умовна кількість км перед сусідніми вулицями, 3.0 умовна коміссія самої агенції таксі 
        }
    }
}
