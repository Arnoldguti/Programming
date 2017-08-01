using Project.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Globalization;
using System.Diagnostics;
using Project.Views.News;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;


namespace Project.Views.Home
{
   
    public sealed partial class HomeTiles : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //declarations for clock

        DispatcherTimer dispatcherTimer;

        string vday, vmonth, vyear, vmonthL;
        int vhour, minute, second;
        string Decimalhour, Decimalminute, Decimalsecond, vampm;
        public TimeSpan MyTime { get; set; }


      

        /// <summary>
        /// Este puede cambiarse a un modelo de vista fuertemente tipada.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper se usa en cada una de las páginas para ayudar con la navegación y 
        /// la administración de la duración de los procesos
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public HomeTiles()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            NetworkInformation.NetworkStatusChanged += NetworkInformationNetworkStatusChanged;

            MyTime = DateTime.Now.TimeOfDay;
            DataContext = this;

            vday = DateTime.Now.DayOfWeek.ToString();
            vmonth = DateTime.Now.Month.ToString();
            vyear = DateTime.Now.Year.ToString();

            //   var bounds = Window.Current.Bounds;

            DispatcherTimerSetup();
            dispatcherTimer.Start();


            //Image Background
          
            //imageBackEffect.Completed += imageBackEffect_Completed;
            imageRush.Begin();

            //Transitions of FlipView
            flipTimer();
     
        }


        /// <summary>
        /// Se ejecuta cada vez que el estado de la conexión cambia.
        /// </summary>
        /// <param name="sender"></param>
     
        public  void Inactivity()
            {
                int download = 0;
            App.timerSessionPostals.Stop();
            App.timerSession.Stop();
            Helpers.UpdateForTimes help = new Helpers.UpdateForTimes();

            download = help.getUpdateResult();
          

            //DispatcherTimer timer = new DispatcherTimer();
            App.timerSession.Interval = TimeSpan.FromSeconds(Helpers.Constants.TimeInactivity);
           
            App.timerSession.Tick += (o, a) =>
            {
                // hay conexión a internet ?
                if (InternetConectivity && App.charged_postals == false && download == 0)
                {
              
                    this.Frame.Navigate(typeof(Views.News.postals));
                }
                else
                    if (InternetConectivity && App.charged_postals == true && download == 0)
                    {

                       
                        this.Frame.Navigate(typeof(Views.News.postals));
                    }
                    else
                            if (InternetConectivity && download == 1 )
                            {
                               
                                App.charged_postals = false;
                                download = 0;
                                this.Frame.Navigate(typeof(Views.News.postals));
                            }
                            else
                                if (InternetConectivity==false && download == 1)
                                {
                                  
                                }
                        else
                {
     
                }
                   
            };
          
             App.timerSession.Start();
            }

        public static bool InternetConectivity
        {
            get
            {
                var prof = NetworkInformation.GetInternetConnectionProfile();
                return prof != null && prof.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }
        
        public void flipTimer()
        {
              int change = 1;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += (o, a) =>
            {
                // If we'd go out of bounds then reverse
                int newIndex = flipView1.SelectedIndex + change;
                if (newIndex >= flipView1.Items.Count || newIndex < 0)
                {
                    change *= -1;
                }

                try
                {
                    flipView1.SelectedIndex += change;
                }
                catch
                {
                    Debug.WriteLine("Ocurrio un error en el flip");
                    this.Frame.Navigate(typeof(Views.Routes.routesCampus), "1");

                }
            };

            timer.Start();
      }

        public void flipTimer()
        {
            int change = 1;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(7);
            timer.Tick += (o, a) =>
            {
                // If we'd go out of bounds then reverse
                int newIndex = flipUTH.SelectedIndex + change;
                if (newIndex >= flipUTH.Items.Count || newIndex < 0)
                {
                    change *= -1;
                }

                flipUTH.SelectedIndex += change;
            };

            timer.Start();
        }

        /// <summary>
        /// Rellena la página con el contenido pasado durante la navegación. Cualquier estado guardado se
        /// proporciona también al crear de nuevo una página a partir de una sesión anterior.
        /// </summary>
        /// <param name="sender">
        /// El origen del evento; suele ser <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Datos de evento que proporcionan tanto el parámetro de navegación pasado a
        /// <see cref="Frame.Navigate(Type, Object)"/> cuando se solicitó inicialmente esta página y
        /// un diccionario del estado mantenido por esta página durante una sesión
        /// anterior. El estado será null la primera vez que se visite una página.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            ViewModels.CollectionEvents obj = new ViewModels.CollectionEvents();

            obj.getCurrentEvents();
            flipView1.ItemsSource = obj.EventsList;

            Inactivity();

           // App.dispatcherTimer.Tick += dispatcherTimer_Tick2;
            //App.dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
        }



        //Clock
        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //IsEnabled defaults to false
            //  txtDate.Text += "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";

            //  txtDate.Text += "Calling dispatcherTimer.Start()\n";
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
            //   txtDate.Text += "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";
        }

        //Clock Dispatcher timer
        void dispatcherTimer_Tick(object sender, object e)
        {

            //Time since last tick should be very very close to Interval
            Calendar calendar = new Calendar();

            vhour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            second = DateTime.Now.Second;

          

            vampm = "";
            TimeSpan timespan = new TimeSpan(Int32.Parse(Decimalhour), Int32.Parse(Decimalminute), Int32.Parse(Decimalsecond));
            DateTime time = DateTime.Today.Add(timespan);
            Debug.WriteLine(Decimalhour + Decimalminute + Decimalsecond);
            // string displayTime = time.ToString("hh:mm tt"); // It will give "03:00 AM"


            hour.Text = time.ToString("hh:mm tt")  + vampm;
           // ampm.Text = vampm;

           // day.Text = DateTime.Now.DayOfWeek.ToString();

            day.Text = calendar.DayOfWeekAsSoloString() + " "  + time.Day ;
            month.Text = calendar.MonthAsSoloString() + ", " + time.Year;

            /*
            if (hour.Equals("00") && minute.Equals("00") && second.Equals("01"))
            {
                vday = DateTime.Now.DayOfWeek.ToString();
                vmonth = DateTime.Now.Month.ToString();
                vyear = DateTime.Now.Year.ToString();
                month.Text = calendar.MonthAsSoloString();
            }
            */
        }

     
        /// <summary>
        /// Mantiene el estado asociado con esta página en caso de que se suspenda la aplicación o
        /// se descarte la página de la memoria caché de navegación.  Los valores deben cumplir los requisitos
        /// de serialización de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">El origen del evento; suele ser <see cref="NavigationHelper"/></param>
        /// <param name="e">Datos de evento que proporcionan un diccionario vacío para rellenar con
        /// un estado serializable.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region Registro de NavigationHelper

        /// Los métodos proporcionados en esta sección se usan simplemente para permitir
        /// que NavigationHelper responda a los métodos de navegación de la página.
        /// 
        /// Debe incluirse lógica específica de página en los controladores de eventos para 
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// y <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// El parámetro de navegación está disponible en el método LoadState 
        /// junto con el estado de página mantenido durante una sesión anterior.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void click_Login(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        	
			 //this.Frame.Navigate(typeof(Views.LogIn.login));
			
			// TODO: Agregar implementación de controlador de eventos aquí.
        }

        private void Click_Gallery(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        	//this.Frame.Navigate(typeof(Views.News));
			// TODO: Agregar implementación de controlador de eventos aquí.
        }


        public async void ClickValues()
 {

     // hay conexión a internet ?
     if (InternetConectivity)
     {

         // App.charged_postals = false;
         this.Frame.Navigate(typeof(Views.News.postals));
     } if (InternetConectivity == false && App.charged_postals == true)
     {

         // App.charged_postals = false;
         this.Frame.Navigate(typeof(Views.News.postals));
     }
     if (InternetConectivity == false && App.charged_postals == false)
     {

         //No hay conexión a Internet
         MessageDialog info = new MessageDialog("No hay conexión a Internet");
         await info.ShowAsync();
     }
        
 }

        private async void click_news(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            // hay conexión a internet ?
            ClickValues();      			
			// TODO: Agregar implementación de controlador de eventos aquí.
        }

        #endregion

        private void click_routes(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Routes.routesCampus),"1");
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void click_admin(object sender, PointerRoutedEventArgs e)
        {
          this.Frame.Navigate(typeof(Views.News.admin.uploadImages));
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.News.socialNetworks.SocialNetworksTimeline));
        }

        private async void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {

            // hay conexión a internet ?
            if (InternetConectivity)
            {
                 App.displays = 1;
            App.charged_schedule = false;
            
            CustomSetting CustomSettingFlyout = new CustomSetting();
            CustomSettingFlyout.Show();
            }
            else
            {
                Debug.WriteLine("NOO HAYYY INTERNETTTTT");


                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }
          
            //this.Frame.Navigate(typeof(CustomSetting);
          // Windows.UI.ApplicationSettings.SettingsPane.Show();
         //  Windows.UI.ApplicationSettings.CustomSetting.Show();

        }

        private void Grid_PointerPressed_2(object sender, PointerRoutedEventArgs e)
        {
            //App.inactive = false;
           // setInactivity();

            Inactivity();
        }


        public void setInactivity()
             {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
dispatcherTimer.Tick += dispatcherTimer_Tick2;
dispatcherTimer.Interval = new TimeSpan(0,0,1);
dispatcherTimer.Start();
             }

        private void dispatcherTimer_Tick2(object sender, object e)
        {
            ((DispatcherTimer)sender).Stop();
            //Your code here
           // App.inactive = true;
            this.Frame.Navigate(typeof(Views.News.postals));
        }

        private void Grid_PointerPressed_3(object sender, PointerRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Views.News.admin.uploadImages));
        }


        void NetworkInformationNetworkStatusChanged(object sender)
{
if (NetworkInformation.GetInternetConnectionProfile() == null){
    //No hay conexion}
    App.NetworkAvailable = false;
    }

else{
//Hay conexion
}
App.NetworkAvailable = true;
 }


 
        private  async void click_admin2(object sender, PointerRoutedEventArgs e)
        {
            // hay conexión a internet ?
            if (InternetConectivity)
            {
           
                CustomSetting CustomSettingFlyout = new CustomSetting();
                CustomSettingFlyout.Show();
                App.charged_grades = false;
                App.displays = 2;
            }else
            {
              
  
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }


         


         
        }

        private async void Border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // hay conexión a internet ?
            if (InternetConectivity)
            {
                this.Frame.Navigate(typeof(Views.LogIn.login));
            }
            else
            {
           
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }
          
        }

        private async void flipView1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // hay conexión a internet ?
            if (InternetConectivity)
            {
                App.charged_events = false;
                this.Frame.Navigate(typeof(Views.Events.EventsList));
            }
            else
            {
            
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }
          
        }

        private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ClickValues();
        }

        private void TextBlock_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Routes.routesCampus), "1");
        }

        private async void Path_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // hay conexión a internet ?
            if (InternetConectivity)
            {
                CustomSetting CustomSettingFlyout = new CustomSetting();
                CustomSettingFlyout.Show();
                App.charged_grades = false;
                App.displays = 2;
            }
            else
            {
             
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }


        }

        private async void TextBlock_PointerPressed_2(object sender, PointerRoutedEventArgs e)
        {
            // hay conexión a internet ?
            if (InternetConectivity)
            {
                this.Frame.Navigate(typeof(Views.LogIn.login));
            }
            else
            {

                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }
        }

        private async void TextBlock_PointerPressed_3(object sender, PointerRoutedEventArgs e)
        {

            // hay conexión a internet ?
            if (InternetConectivity)
            {
                App.displays = 1;
                App.charged_schedule = false;

                CustomSetting CustomSettingFlyout = new CustomSetting();
                CustomSettingFlyout.Show();
            }
            else
            {
             
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("No hay conexión a Internet");
                await info.ShowAsync();
            }
        }

    }
}
