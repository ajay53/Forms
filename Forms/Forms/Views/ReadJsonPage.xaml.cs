using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Forms.Models;
using SQLite;
using System.IO;

namespace Forms.Views
{
	public partial class ReadJsonPage : ContentPage
	{
        List<ItemsDB> allItems;
        List<ItemsDB> items;
        string dbPath;
        SQLiteConnection db;
        bool isRecordPresent;
        private const string url = "https://jsonplaceholder.typicode.com/posts";

        public ReadJsonPage()
        {
            InitializeComponent();
            listView.ItemsSource = allItems;

            ReadJsonViaUrl();

            BindingContext = this;
        }

        private async void ReadJsonViaUrl()
        {
            Uri uri = new Uri(url);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<List<ItemsDB>>(content);
                foreach (var item in items)
                {
                    Console.WriteLine($"item: {item.ToString()}");

                    InsertIntoDatabase(item);
                }
            }
            ReadDatabase();
        }

        private void ReadDatabase()
        {
            var data = db.Table<ItemsDB>(); //Call Table  
            string query = "SELECT * from Items";

            allItems = db.Query<ItemsDB>(query, "Title");
        }

        private void InsertIntoDatabase(ItemsDB a_item)
        {

            if (!File.Exists(dbPath))
            {
                dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Items_Test"); //Call Database 
                db = new SQLiteConnection(dbPath);
                //var data = db.Table<ItemsDB>(); //Call Table  
                db.CreateTable<ItemsDB>();
            }
            
            IsPresent(a_item.Id);

            if (isRecordPresent)
            {
                isRecordPresent = false;
                return;
            }
            db.Insert(ItemsTable(a_item));
        }

        private void IsPresent(int a_id)
        {
            ItemsDB data = null;
            try
            {
                data = db.Get<ItemsDB>(a_id);
                Console.WriteLine($"data: {data.ToString()}");
            }
            catch (Exception)
            {
                isRecordPresent = true;
            }

        }

        private ItemsDB ItemsTable(ItemsDB a_items)
        {
            ItemsDB items = new ItemsDB
            {
                UserId = a_items.UserId,
                Id = a_items.Id,
                Title = a_items.Title,
                Body = a_items.Body
            };
            return items;
        }
    }
}