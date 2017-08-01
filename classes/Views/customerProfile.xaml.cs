using Profile.Common;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using System.Text;
using Windows.Networking.Connectivity;

//Arnol Gutierrez




namespace Profile.Views
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class customerProfile : Page
    {
       
      
         private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //declaracion de variables
        string accountReceive = "";
        string passwordReceive = "";
        int customerTypeReceive ;
        string requestReceive, flag;
        string imageRepresentation = "";
       //Recibir datos de la pagina Login

        //Datos de sesion
    
        Dictionary<string, string> receiveData;

        //Roaming
        Windows.Storage.ApplicationDataContainer roamingSettings =
           Windows.Storage.ApplicationData.Current.RoamingSettings;


        List<Dictionary<string, string>> dataService;
        List<Dictionary<string, string>> dataServiceImage;
        public int photoExistsSession;

        // JArray dataService;

        Dictionary<string, string> diction;
        Dictionary<string, string> dictionImage;

        Core.JsonParser objParser = new Core.JsonParser();
        Core.ProfileInformation objProfile = new Core.ProfileInformation();
        Helpers.Utility objUtility = new Helpers.Utility();

        string account, password, name, career="", phone, email, about, imageString, tumbnail, relationshipStatus, sex;
        int result, customerType;//, photoExists;
        string photoExists;
    

        bool loadded = false;

      
      
       



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


        public customerProfile()
        {

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

           
          
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
                                Debug.WriteLine("NO HAYYY INTERNET");

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
      
       



        private async void obtainProfileService()
        {
            account = accountReceive;
            customerType = customerTypeReceive;
            tumbnail = "FALSE";

            //Debug.WriteLine(accountReceive + "esta es la cuenta que recibo");
            //Almacenar en el Roaming los datos recibidos
            Windows.Storage.ApplicationDataContainer roamingSettings =
            Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["accountRoaming"] = accountReceive;
            roamingSettings.Values["customerRoaming"] = customerTypeReceive;


            try
            {
                var data = await objProfile.profileInformation(account);
                dataService = objParser.parserJsonObject(data);

                for (int i = 0; i < dataService.Count(); i++)
                {
                    diction = dataService[i];
                    name = diction[Helpers.Constants.NAME];
                    sex = diction[Helpers.Constants.SEX];
                    if (customerType == 2)
                    {
                        career = diction[Helpers.Constants.CAREER];
                    }
                    customerType = Int32.Parse(diction[Helpers.Constants.CUSTOMER_TYPE]);
                    relationshipStatus = diction[Helpers.Constants.RELATIONSHIP_STATUS];
                    email = diction[Helpers.Constants.EMAIL];
                    phone = diction[Helpers.Constants.PHONE_NUMBER];
                    about = diction[Helpers.Constants.ABOUT];
                    photoExists = diction[Helpers.Constants.PHOTO_EXISTS];

                    setProfile(name, career, sex, relationshipStatus, email, phone, about, photoExists);


                }
            }

            catch (Exception e)
            {
                Debug.WriteLine("{0} Exception caught.", e);




            }
            
           
           


            

        }


   



        public async void setProfile(string name, string career, string sex, string relationshipStatus, string email, string phone, string about, string photoExists)
        {
            //set values
            txtName.Text = name;
            //   txtCareer.Text = career;


            if (relationshipStatus == null)
            {
                txtRelatioShip.Text =  Helpers.Constants.INFO_NOT_AVAILABLE;
                relationshipStatus = Helpers.Constants.INFO_NOT_AVAILABLE;
            }
            else
            { relationshipStatus = objUtility.setRelationship(Int32.Parse(relationshipStatus), sex); txtRelatioShip.Text = relationshipStatus; }


            if (email == null || email == "")
            {
                email = Helpers.Constants.INFO_NOT_AVAILABLE; txtEmail.Text = email;
            }
            else
            { txtEmail.Text =  email; }


            if (phone == null || phone == "")
            {
                phone = Helpers.Constants.INFO_NOT_AVAILABLE; txtPhoneNumber.Text =  phone;
            }
            else
            { txtPhoneNumber.Text =  phone; }

            if (career == null || career == "")
            {
                career = Helpers.Constants.INFO_NOT_AVAILABLE; txtCareer.Text = "";
            }
            else
            { txtCareer.Text = career; }


            if (about == null || about == "")
            {
                about = Helpers.Constants.INFO_NOT_AVAILABLE; txtAbout.Text = about;
            }
            else
            {

                string utf8String = about;
                string propEncodeString = string.Empty;

                byte[] utf8_Bytes = new byte[utf8String.Length];
                for (int i = 0; i < utf8String.Length; ++i)
                {
                    utf8_Bytes[i] = (byte)utf8String[i];
                }

                propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);


                byte[] bytes = Encoding.UTF8.GetBytes(about);
                string myString = Encoding.UTF8.GetString(bytes,0, bytes.Length);


                //new
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(about);
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes,0,isoBytes.Length);

                txtAbout.Text =(about); 

          



            }


            if (photoExists.Equals(Helpers.Constants.PHOTO_CORRECT))
            {
                //download image
             
                imageRepresentation = await objUtility.downloadImage(accountReceive);
                Debug.WriteLine(imageRepresentation);
                Debug.WriteLine(photoExists);
                
                 setImage(imageRepresentation, photoExists);
            }
            else
            {
              
                setImage(getImageDefault(sex), photoExists);
            }


        
     

        }


        string ParseUnicodeHex(string hex)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hex.Length; i += 4)
            {
                string temp = hex.Substring(i, 4);
                char character = (char)Convert.ToInt16(temp, 16);
                sb.Append(character);
            }
            return sb.ToString();
        }
    
        public string getImageDefault(string sex)
        {

            var img = "";
            if (sex == "F") { img = Helpers.Constants.PROFILE_WOMAN; }
            if (sex == "M") { img = Helpers.Constants.PROFILE_MAN; }


            return img;
        }

  
        
        //Set Profile photo
        public async void setImage(string imageRepresentation, string photoExists)
        {
          // imageRepresentation = Representation;

            if (photoExists == "False")
            {
               imageRepresentation = getImageDefault(roamingSettings.Values["_sex"].ToString());
              
            }


            var imgBytes = Convert.FromBase64String(imageRepresentation.Replace(" ", "+"));
    
        var ms = new InMemoryRandomAccessStream();
        var dw = new Windows.Storage.Streams.DataWriter(ms);
        dw.WriteBytes(imgBytes);
        await dw.StoreAsync();
        ms.Seek(0);

            var bm = new BitmapImage();
            bm.SetSource(ms);

            // img1 is an Image Control in XAML
            bm.ImageOpened += (s, e) => { this.imgProfile.ImageSource = bm; };
            bm.ImageFailed += (s, e) => { };    

        //    photoProgressRing.IsActive = false;
          //  PR.IsActive = false;
            loadded = true;
        }

      

        //Set Complete profile
        public void setRoamingProfile()
        {
            string imageRepresentation = "";

            //Resturar los valores guardados en el roaming
            // Restore values stored in app data.
  
        
            //set values


            //set profile photo
            //setImage(roamingSettings.Values["_photoRepresentation"].ToString());



        }



        public void close()
        {
            //restore values of sesion
            App.charged_schedule = false;
            App.charged_grades = false;
            this.Frame.Navigate(typeof(Views.Home.HomeTiles));


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

            Inactivity();
        //Si los datos de sesion ya han sido guardados
            if (e.PageState != null && e.PageState.ContainsKey("accountNumber"))
            {
                //Restaurar los valores guardados en la sesion
                photoExists = e.PageState["photoExists"].ToString();              
                receiveData = e.PageState["receiveData"] as Dictionary<string, string>;
                accountReceive = receiveData[Helpers.Constants.ACCOUNT_NUMBER];
                passwordReceive = receiveData[Helpers.Constants.PASSWORD];
                customerTypeReceive = Int32.Parse(receiveData[Helpers.Constants.CUSTOMER_TYPE]);
                

                setRoamingProfile();
                setImage(e.PageState["stringRepresentation"].ToString(), photoExists);
                imageRepresentation = e.PageState["stringRepresentation"].ToString();
            }
            else //Primer ingreso al view
            {

                //recibir los datos pasados
                receiveData = e.NavigationParameter as Dictionary<string, string>;
                accountReceive = receiveData[Helpers.Constants.ACCOUNT_NUMBER];
                passwordReceive = receiveData[Helpers.Constants.PASSWORD];
                customerTypeReceive = Int32.Parse(receiveData[Helpers.Constants.CUSTOMER_TYPE]);
         

                //Descargar la información del servicio web
                obtainProfileService();
               // Debug.WriteLine("1....es primerita vez");
            
            }





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

		public void GoForward()
        {
         if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
         }
		
        private void LogOut(object sender, PointerRoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(GroupedItemsPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CloseSession(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            close();
			
			// TODO: Agregar implementación de controlador de eventos aquí.
        }

        private void Schedule(object sender, PointerRoutedEventArgs e)
        {
       	 this.Frame.Navigate(typeof(Views.Academic.scheduleStudent), receiveData);
   
        }

        private void ScheduleClick(object sender, PointerRoutedEventArgs e)
        {
         //  GoForward();
			 this.Frame.Navigate(typeof(Views.Academic.scheduleStudent), receiveData);
           // this.NavigationService.Navigate("Second.xaml");
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
           

            //Enviar los datos a moficar
            Dictionary<string, string> sendData = new Dictionary<string, string>();
            //Send dictionary of profile
            sendData.Add(Helpers.Constants.ACCOUNT_NUMBER, accountReceive);
            sendData.Add(Helpers.Constants.PASSWORD, passwordReceive);
            sendData.Add(Helpers.Constants.CUSTOMER_TYPE, customerTypeReceive.ToString());
            sendData.Add(Helpers.Constants.NAME, roamingSettings.Values["_name"].ToString());
            sendData.Add(Helpers.Constants.ABOUT, roamingSettings.Values["_about"].ToString());
            sendData.Add(Helpers.Constants.PHONE_NUMBER, roamingSettings.Values["_phone"].ToString());
            sendData.Add(Helpers.Constants.EMAIL, roamingSettings.Values["_email"].ToString());
            sendData.Add(Helpers.Constants.RELATIONSHIP_STATUS, roamingSettings.Values["_relationshipStatus"].ToString());
            sendData.Add(Helpers.Constants.SEX, roamingSettings.Values["_sex"].ToString());

            this.Frame.Navigate(typeof(Views.UpdateProfile), sendData);
            
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Inactivity();
        }

        private void Border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Events.EventsList));
        }

        private void Border_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            close();
			
        }

        private void Border_PointerPressed_2(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Routes.routesCampus), "1");
        }

        private void Border_PointerPressed_3(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.News.socialNetworks.SocialNetworksTimeline));
        }

        private void Border_PointerPressed_4(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Academic.currentGrades), receiveData);
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Enviar los datos a moficar
            Dictionary<string, string> sendData = new Dictionary<string, string>();
            //Send dictionary of profile
            sendData.Add(Helpers.Constants.ACCOUNT_NUMBER, accountReceive);
            sendData.Add(Helpers.Constants.PASSWORD, passwordReceive);
            sendData.Add(Helpers.Constants.CUSTOMER_TYPE, customerTypeReceive.ToString());
            sendData.Add(Helpers.Constants.NAME, roamingSettings.Values["_name"].ToString());
            sendData.Add(Helpers.Constants.ABOUT, roamingSettings.Values["_about"].ToString());
            sendData.Add(Helpers.Constants.PHONE_NUMBER, roamingSettings.Values["_phone"].ToString());
            sendData.Add(Helpers.Constants.EMAIL, roamingSettings.Values["_email"].ToString());
            sendData.Add(Helpers.Constants.RELATIONSHIP_STATUS, roamingSettings.Values["_relationshipStatus"].ToString());
            sendData.Add(Helpers.Constants.SEX, roamingSettings.Values["_sex"].ToString());

            this.Frame.Navigate(typeof(Views.UpdateProfile), sendData);
        }

    

     
     
   

      
    }
}
