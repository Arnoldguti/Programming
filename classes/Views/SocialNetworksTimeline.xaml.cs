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
using Windows.UI.Xaml.Documents;
using Windows.Networking.Connectivity;
using System.Diagnostics;




namespace Projects.Views.News.socialNetworks
{
    /// <summary>

    /// </summary>
    public sealed partial class SocialNetworksTimeline : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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


        public SocialNetworksTimeline()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            TimeLineWeb.NavigationStarting += TimeLineWeb_NavigationStarting;
            TimeLineWeb.ContentLoading += webView1_ContentLoading;
            TimeLineWeb.DOMContentLoaded += webView1_DOMContentLoaded;
            TimeLineWeb.UnviewableContentIdentified += webView1_UnviewableContentIdentified;
            TimeLineWeb.NavigationCompleted += webView1_NavigationCompleted;

          
        }

        public void Inactivity()
        {
            int download = 0;
            App.timerSessionPostals.Stop();
            App.timerSession.Stop();
            Helpers.UpdateForTimes help = new Helpers.UpdateForTimes();

            download = help.getUpdateResult();
            Debug.WriteLine("Soy el download " + download);

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
                        if (InternetConectivity && download == 1)
                        {

                            App.charged_postals = false;
                            download = 0;
                            this.Frame.Navigate(typeof(Views.News.postals));
                        }
                        else
                            if (InternetConectivity == false && download == 1)
                            {

                            }
                            else
                            {
                                Debug.WriteLine("INTERNET");

                                //No hay conexión a Internet
                                //   MessageDialog info = new MessageDialog("No hay conexión a Internet");
                                //  await info.ShowAsync();
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
      
       


        private void webView1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            pageIsLoading = false;
            if (args.IsSuccess)
            {
                string url = (args.Uri != null) ? args.Uri.ToString() : "<null>";
                appendLog(String.Format("Navigation to \"{0}\"completed successfully.\n", url));
            }
            else
            {
                string url = "";
                try { url = args.Uri.ToString(); }
                finally
                {
                   // address.Text = url;
                    appendLog(String.Format("Navigation to: \"{0}\" failed with error code {1}.\n", url, args.WebErrorStatus.ToString()));
                }
            }
        }

        async void webView1_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            appendLog(String.Format("Content for \"{0}\" cannot be loaded into webview. Invoking the default launcher instead.\n", args.Uri.ToString()));
            // We turn around and hand the Uri to the system launcher to launch the default handler for it
            await Windows.System.Launcher.LaunchUriAsync(args.Uri);
            pageIsLoading = false;
        }

        private void webView1_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            string url = (args.Uri != null) ? args.Uri.ToString() : "<null>";
            appendLog(String.Format("Content for \"{0}\" has finished loading.\n", url));
        }

        private void webView1_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            string url = (args.Uri != null) ? args.Uri.ToString() : "<null>";
            appendLog(String.Format("Loading content for \"{0}\".\n", url));
        }

        private void TimeLineWeb_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string url = "";
            try { url = args.Uri.ToString(); }
            finally
            {
             
                appendLog(String.Format("Starting navigation to: \"{0}\".\n", url));
                pageIsLoading = true;
            }
        }

        void appendLog(string logEntry)
        {
            Run r = new Run();
            r.Text = logEntry;
            Paragraph p = new Paragraph();
            p.Inlines.Add(r);
            //logResults.Blocks.Add(p);
        }

        private void NavigateWebview(string url)
        {
            try
            {
                Uri targetUri = new Uri(url);
                TimeLineWeb.Navigate(targetUri);
                Charging.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            catch (FormatException myE)
            {
                // Bad address
                TimeLineWeb.NavigateToString(String.Format("<h1>Address is invalid, try again.  Details --> {0}.</h1>", myE.Message));
            }
        }




        private bool _pageIsLoading;
        bool pageIsLoading
        {
            get { return _pageIsLoading; }
            set
            {
                _pageIsLoading = value;
                //goButton.Content = (value ? "Stop" : "Go");
                Charging.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);

                if (!value)
                {
                  //  navigateBack.IsEnabled = webView1.CanGoBack;
                   // navigateForward.IsEnabled = webView1.CanGoForward;
                }
            }
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
            Charging.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if (!pageIsLoading)
            {
                // Charging.IsActive = true;
                NavigateWebview(Helpers.Constants.SOCIAL);

            }
            else
            {
                TimeLineWeb.Stop();
                pageIsLoading = false;

            }

            Inactivity();
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

        #endregion

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            TimeLineWeb.Stop();

       
            this.Frame.Navigate(typeof(Views.Home.HomeTiles));
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Inactivity();
        }
    }
}
