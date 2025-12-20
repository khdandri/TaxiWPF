using kyrsovarobota;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using kyrsovarobota.Model;

namespace kyrsovarobota
{
    public class UserManager
    {
        private List<User> _users = new List<User>();
        private const string DataFilePath = "users.json";

        public UserManager()
        {
            LoadUsersFromFile();
        }

        private void SaveUsersToFile()
        {
            try
            {
                // Налаштування, щоб JSON виглядав красиво (з відступами)
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_users, options);
                File.WriteAllText(DataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        private void LoadUsersFromFile()
        {
            if (!File.Exists(DataFilePath)) return;

            try
            {
                string jsonString = File.ReadAllText(DataFilePath);
                // Перетворюємо JSON-текст назад у список об'єктів
                _users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Помилка завантаження: {ex.Message}");
            }
        }

        private bool ValidateRegistrationData(string username, string password, out string errorMessage)
        {
            // 1. Перевірка логіна 
            if (string.IsNullOrWhiteSpace(username) || username.Length < 5)
            {
                errorMessage = "Логін має бути не менше 5 символів.";
                return false;
            }


            // 2. Перевірка пароля 
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                errorMessage = "Пароль має бути не менше 6 символів.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool RegisterUser(string username, string password, out string errorMessage)
        {
            // 1. Валідація даних (можете використати вашу стару логіку)
            if (!ValidateRegistrationData(username, password, out errorMessage))
            {
                return false;
            }

            // 2. Перевірка, чи не зайнятий логін/пошта
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                errorMessage = "Користувач з таким логіном вже існує.";
                return false;
            }

            // 3. Додавання нового користувача
            var newUser = new User(username, password);
            _users.Add(newUser);

            // 4. Збереження оновленого списку в JSON
            SaveUsersToFile();

            errorMessage = "Реєстрація успішна!";
            return true;
        }


        public User AuthenticateUser(string username, string password, out string errorMessage)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                errorMessage = "Користувача не знайдено.";
                return null;
            }

            if (user.Password == password)
            {
                errorMessage = "Успіх!";
                return user;
            }

            errorMessage = "Невірний пароль.";
            return null;
        }
    }
}