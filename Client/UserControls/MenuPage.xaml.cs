using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : UserControl
    {
        List<MenuItemControl> list = new List<MenuItemControl>();
        MenuItemControl menuItemControl = new MenuItemControl();
        public MenuPage(string category)
        {
            InitializeComponent();

            Error.Text = "Подождите...";
            Error.Visibility = Visibility.Visible;

            if (category == "")
            {
                GetMenuItems();
            }
            else
            {
                GetMenuItemsByCategory(category);
            }
        }
        private async void GetMenuItems()
        {
            //var l = new Menu();
            //l.Name = "Мистер Нико";
            //l.Price = 30.00;
            //l.Image = "https://www.pngkey.com/png/full/97-970955_wafer-ice-cream-transparent-background-png-waffle-cup.png";
            //l.Category = "Десерт";
            //l.Description = "Мороженое Mister Нико в вафельной корзинке с шоколадным соусом и кусочками брауни.";

            //list.Add(l);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetStringAsync("menu");
                if (response != null)
                {
                    var items = JsonConvert.DeserializeObject<List<Common.Models.MenuItem>>(response);
                    if (items.Count > 0)
                    {
                        Error.Visibility = Visibility.Hidden;
                        foreach (var i in items)
                        {
                            list.Add(ConvertToMenuItemControl(i));
                        }

                        ListViewProducts.ItemsSource = list;
                    }
                    else
                    {
                        Error.Text = "Пусто...";
                        Error.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось установить подключение!", "Ошибка", MessageBoxButton.OK);
            }
            client.Dispose();
        }
        private async void GetMenuItemsByCategory(string category)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9000/api/menu/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var response = await client.GetStringAsync($"{category}");
                if (response != null)
                {
                    var items = JsonConvert.DeserializeObject<List<Common.Models.MenuItem>>(response);
                    if (items.Count > 0)
                    {
                        Error.Visibility = Visibility.Hidden;
                        foreach (var i in items)
                        {
                            list.Add(ConvertToMenuItemControl(i));
                        }
                        ListViewProducts.ItemsSource = list;
                    }
                    else
                    {
                        Error.Text = "Пусто...";
                        Error.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось установить подключение!", "Ошибка", MessageBoxButton.OK);
            }
            client.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuItemControl item = ((Button)sender).Tag as MenuItemControl;
            Content = new MenuItemPage(item);
        }
        // Вернуть объект класса MenuItemControl
        private MenuItemControl ConvertToMenuItemControl(Common.Models.MenuItem menuItem)
        {
            menuItemControl.Id = menuItem.Id;
            menuItemControl.LastChangedAt = menuItem.LastChangedAt;
            menuItemControl.Name = menuItem.Name;
            menuItemControl.Price = menuItem.Price;
            menuItemControl.Image = menuItem.Image;
            menuItemControl.Category = menuItem.Category;
            menuItemControl.Description = menuItem.Description;

            return menuItemControl;
        }
    }
}
