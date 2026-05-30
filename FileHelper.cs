using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Kyrsovai
{
    public static class FileHelper
    {
        private static string filePath = "data.json";

        public static void SaveData(List<User> users, List<Order> orders, User currentUser)
        {
            var data = new DataStorage
            {
                Users = users,
                Orders = orders,
                CurrentUserEmail = currentUser?.Email
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static DataStorage LoadData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<DataStorage>(json);
            }
            return null;
        }
    }
}