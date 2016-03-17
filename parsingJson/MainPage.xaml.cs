using parsingJson.Entities;
using parsingJson.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace parsingJson
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public  ObservableCollection<Personne> Personnes;

        string responseString;
        public MainPage()
        {
            this.InitializeComponent();
            Personnes = new ObservableCollection<Personne>();
            VisiblePersonList.ItemsSource = Personnes;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Personnes.Clear();
            responseString = JsonText.Text;
            JsonArray jsonArray = JsonArray.Parse(responseString);

            foreach (IJsonValue jsonValue in jsonArray)
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    JsonObject Jo = jsonValue.GetObject();
                    addtoList(Jo);
                }
        }
        public  void addtoList(JsonObject Jo)
        {
            Personne p = new Personne();
            p.Nom = Jo.GetNamedString("last_name");
            p.Prenom= Jo.GetNamedString("first_name");
            p.Age = Convert.ToInt32(Jo.GetNamedString("age"));
            p.id = Convert.ToInt32(Jo.GetNamedString("id"));
            Personnes.Add(p);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Personnes.Clear();
            Uri resourceUri;
            Helpers.TryGetUri("http://localhost/ForGit/getPersons.php", out resourceUri);

            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(resourceUri);
                responseString = await response.Content.ReadAsStringAsync();
                JsonArray jsonArray = JsonArray.Parse(responseString);


                foreach (IJsonValue jsonValue in jsonArray)
                    if (jsonValue.ValueType == JsonValueType.Object)
                    {
                        JsonObject Jo = jsonValue.GetObject();
                        addtoList(Jo);
                    }
            }
            catch (TaskCanceledException)
            {
                //rootPage.NotifyUser("Request canceled.", NotifyType.ErrorMessage);
            }
            catch (Exception ex)
            {
                // rootPage.NotifyUser("Error: " + ex.Message, NotifyType.ErrorMessage);
            }
        }
    }
}
