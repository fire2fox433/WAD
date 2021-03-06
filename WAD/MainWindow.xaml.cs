﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading;
using System.Windows.Media.Animation;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
namespace WAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static user currentUser = new user();
        // Client's socket
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // If running as server, we can contain all client's socket
        public static ArrayList arrSocket = new ArrayList();

        public static List<Movie> movieList = new List<Movie>();

        public static DateTime updatedTime;

        public static int listStart = 0;

        public static int currentSelectedMovie = 0;

        public static List<String> showtimeList = new List<string>();

        private static BitmapImage LoadImage(byte[] imageData)
        {
                if (imageData == null || imageData.Length == 0)
                {
                    return null;
                }
                var image = new BitmapImage();
                using (var mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Starts the client connection to server in the background
            // We can change this to on button click, run client
            runClient();
            startAnimationHandler();

        }

        // we can help the user specify ip address + port to connect to.

        // Running the socket connection and reading in the background (Async)
        // If there is no need to run the program in the background, async can be replaced with public
        // Only need to run in the background if the client is constantly waiting on info (without the use of button clicks)
        // Alternative is to use connection handler
        #region runClient() function
        async void runClient()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Connect to server at IP address 127.0.0.1 with port 9000
                    // We can make a popup window to specify what address and port
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
                    socket.Connect(remoteEP);
                }
                catch (SocketException e)
                {
                    MessageBox.Show("Unable to connect to server.");
                    this.Dispatcher.Invoke(() =>
                    {
                        homeGrid.IsEnabled = false;
                    });
                   
                    return;
                }

                NetworkStream stream = new NetworkStream(socket);
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                while (true)
                {
                    // Checks if still connected and close connection if !connected
                    if (!socket.Connected)
                    {
                        // not sure if this is the proper way to shutdown
                        MessageBox.Show("Server disconnected");
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        break;
                    }
                    try
                    {
                        // If connected, waits input from server
                        //string input = reader.ReadLine();

                        // do whatever with input
                    }
                    catch (Exception ex)
                    {
                        // Assuming something went wrong when reading from server (server disconnected/etc)
                        // not sure if this is the proper way to shutdown
                        MessageBox.Show("Server disconnected");
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        break;
                    }
                    Thread.Sleep(5000);
                }
            });
        }
        #endregion

        // If needed, we can run this GUI as a server. code is here.
        #region runServer() function
        async void runServer()
        {
            // again we can make the user specify the port if needed
            int port = 7000;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
            socket.Bind(endpoint);
            socket.Listen(10);
            //SetText("Waiting for clients on port " + port);
            await Task.Run(() =>
            {
                while (true)
                {
                    //try
                    //{
                    //    // if we want every client to have the possibility of being a server, we have to add connection handler class
                    //    Socket client = socket.Accept();
                    //    ConnectionHandler handler = new ConnectionHandler(client, this);
                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(handler.HandleConnection));
                    //}
                    //catch (Exception)
                    //{
                    //    SetText("Connection falied on port " + port);
                    //}
                }
            });
        }
        #endregion

        // Prevent cross-threading and setting text
        #region setText() function
        public void SetText(string msg)
        {
            // I assume this is the way to prevent cross threading from happening, not sure yet to be modified later
            //Action action = () => myTextBlock.Text = "Test";
            //var dispatcher = myTextBlock.Dispatcher;
            //if (dispatcher.CheckAccess())
            //    action();
            //myTextBlock.Text = msg;
            //myLabel.Content = msg;
            //else
            //    dispatcher.Invoke(action);
            //if (this.InvokeRequired)
            //{
            //    SetTextCallback d = new SetTextCallback(SetText);
            //    this.Invoke(d, msg);
            //    return;
            //}
            //lblTurn.Text = msg;
        }
        #endregion

        // Base load function (example)
        // Use of openFileDialog to be user friendly
        #region loadFile() function
        public void loadFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            // default file name, file extension, filter file extension
            dlg.FileName = "Document";
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                // read lines from filename
                string[] data = File.ReadAllLines(dlg.FileName);
                foreach (var key in data)
                {
                    string[] temp = key.Split(';');
                    //dict.Add(temp[0], temp[1]);
                }
                //txtDsiplay.Text = "File loaded!";
            }
        }
        #endregion

        // Base save function (example)
        // Use of SaveFileDialog to be user friendly
        #region saveFile() function
        public void saveFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            // default file name, file extension, filter file extension
            dlg.FileName = "newDocument";
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";

            //string info = "things to be saved";
            //StreamWriter writer = new StreamWriter(dlg.OpenFile());

            if (dlg.ShowDialog() == true)
            {

                // Save document
                //File.WriteAllText(dlg.FileName, info);
                StreamWriter writer = new StreamWriter(dlg.OpenFile());
                //foreach (var kvp in dict)
                //{
                //    writer.WriteLine(kvp.Key + ";" + kvp.Value);
                //}
                writer.Dispose();
                writer.Close();
                //txtDsiplay.Text = "Successfully saved!";
            }
        }
        #endregion


        public void startAnimationHandler()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
            startCanvas.BeginAnimation(Canvas.OpacityProperty, ani);
        }

        private void imgHome1_MouseEnter(object send, MouseEventArgs e)
        {
            DoubleAnimation ani = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.3));
            rctHomeOpacity1.BeginAnimation(Rectangle.OpacityProperty, ani);
            lblHomeMovie1.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        }
        private void imgHome1_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.3));
            rctHomeOpacity1.BeginAnimation(Rectangle.OpacityProperty, ani);
            lblHomeMovie1.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF000000");
        }
        private void imgHome1_ClickEvent(object sender, MouseEventArgs e)
        {
            if (currentUser.getEmail() ==  null)
            {
                hideHomeGrid();
                showLoginGrid();
            }
            else
            {
                hideHomeGrid();
                showListGrid();
            }
        }

        private void btnHomeRegister_Click(object sender, RoutedEventArgs e)
        {
            hideHomeGrid();
            showRegisterGrid();
        }
        private void btnHomeSignIn_Click(object sender, RoutedEventArgs e)
        {
            hideHomeGrid();
            showLoginGrid();
        }

        private void hideHomeGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            homeGrid.BeginAnimation(Grid.OpacityProperty, ani);
            homeGrid.IsEnabled = false;
            homeGrid.Visibility = Visibility.Hidden;
        }
        private void showHomeGrid()
        {
            homeGrid.Opacity = 0;
            homeGrid.IsEnabled = true;
            homeGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            homeGrid.BeginAnimation(Grid.OpacityProperty, ani);
        }
        private void hideLoginGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            loginGrid.BeginAnimation(Grid.OpacityProperty, ani);
            loginGrid.IsEnabled = false;
            loginGrid.Visibility = Visibility.Hidden;
        }
        private void showLoginGrid()
        {
            loginGrid.Opacity = 0;
            loginGrid.IsEnabled = true;
            loginGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            loginGrid.BeginAnimation(Grid.OpacityProperty, ani);
        }
        private void hideRegisterGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            registerGrid.BeginAnimation(Grid.OpacityProperty, ani);
            registerGrid.IsEnabled = false;
            registerGrid.Visibility = Visibility.Hidden;
        }
        private void showRegisterGrid()
        {
            registerGrid.Opacity = 0;
            registerGrid.IsEnabled = true;
            registerGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            registerGrid.BeginAnimation(Grid.OpacityProperty, ani);
        }
        private void hideConfirmGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            confirmationGrid.BeginAnimation(Grid.OpacityProperty, ani);
            confirmationGrid.IsEnabled = false;
            confirmationGrid.Visibility = Visibility.Hidden;
        }
        private void showConfirmGrid()
        {
            confirmationGrid.Opacity = 0;
            confirmationGrid.IsEnabled = true;
            confirmationGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            confirmationGrid.BeginAnimation(Grid.OpacityProperty, ani);
        }
        private void hideListGrid()
        {
            listStart = 0;
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            listGrid.BeginAnimation(Grid.OpacityProperty, ani);
            listGrid.IsEnabled = false;
            listGrid.Visibility = Visibility.Hidden;
        }
        private void showListGrid()
        {
            listGrid.Opacity = 0;
            currentSelectedMovie = 0;
            lblListLabel1.Content = "";
            lblListLabel2.Content = "";
            lblListLabel3.Content = "";
            lblListLabel4.Content = "";
            lblListLabel5.Content = "";
            lblListLabel6.Content = "";
            lblListLabel7.Content = "";
            lblListLabel8.Content = "";
            lblListLabel9.Content = "";
            lblListLabel10.Content = "";
            imgListImage1.Source = null;
            imgListImage2.Source = null;
            imgListImage3.Source = null;
            imgListImage4.Source = null;
            imgListImage5.Source = null;
            imgListImage6.Source = null;
            imgListImage7.Source = null;
            imgListImage8.Source = null;
            imgListImage9.Source = null;
            imgListImage10.Source = null;
            if (movieList.Count() == 0)
            {
                NetworkStream stream = new NetworkStream(socket);
                StreamWriter writer = new StreamWriter(stream);
                StreamReader read = new StreamReader(stream);
                writer.AutoFlush = true;
                writer.WriteLine("request_movie");
                string xml = "";
                string line;
                //string randomVar = read.ReadLine();
                var xs = new XmlSerializer(typeof(HashSet<Movie>));
                while ((line = read.ReadLine()) != "endofxml")
                {
                    xml += line;
                }
                HashSet<Movie> newSet = new HashSet<Movie>();
                using (var reader = new StringReader(xml))
                {
                    newSet = (HashSet<Movie>)xs.Deserialize(reader);
                }
                foreach (Movie details in newSet)
                {
                    if (details.Status == true)
                    {
                        movieList.Add(details);
                    }
                }
                updatedTime = DateTime.Now;
            }
            else
            {
                TimeSpan diff = DateTime.Now - updatedTime;
                if (diff.Days > 0)
                {
                    movieList.Clear();
                    NetworkStream stream = new NetworkStream(socket);
                    StreamWriter writer = new StreamWriter(stream);
                    StreamReader read = new StreamReader(stream);
                    writer.AutoFlush = true;
                    writer.WriteLine("request_movie");
                    string xml = "";
                    string line;
                    //string randomVar = read.ReadLine();
                    var xs = new XmlSerializer(typeof(HashSet<Movie>));
                    while ((line = read.ReadLine()) != "endofxml")
                    {
                        xml += line;
                    }
                    HashSet<Movie> newSet = new HashSet<Movie>();
                    using (var reader = new StringReader(xml))
                    {
                        newSet = (HashSet<Movie>)xs.Deserialize(reader);
                    }
                    foreach (Movie details in newSet)
                    {
                        if (details.Status == true)
                        {
                            movieList.Add(details);
                        }
                    }
                    updatedTime = DateTime.Now;
                }
            }
            int counter = 1;
            int x = 0;
            if (movieList.Count < listStart + 10)
            {
                x = movieList.Count;
            }
            else
            {
                x = listStart + 10;
            }
            for (int i = listStart; i < x; i++)
            {
                if (counter == 1)
                {
                    lblListLabel1.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage1.Source = image;
                    counter += 1;
                }
                else if (counter == 2)
                {
                    lblListLabel2.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage2.Source = image;
                    counter += 1;
                }
                else if (counter == 3)
                {
                    lblListLabel3.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage3.Source = image;
                    counter += 1;
                }
                else if (counter == 4)
                {
                    lblListLabel4.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage4.Source = image;
                    counter += 1;
                }
                else if (counter == 5)
                {
                    lblListLabel5.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage5.Source = image;
                    counter += 1;
                }
                else if (counter == 6)
                {
                    lblListLabel6.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage6.Source = image;
                    counter += 1;
                }
                else if (counter == 7)
                {
                    lblListLabel7.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage7.Source = image;
                    counter += 1;
                }
                else if (counter == 8)
                {
                    lblListLabel8.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage8.Source = image;
                    counter += 1;
                }
                else if (counter == 9)
                {
                    lblListLabel9.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage9.Source = image;
                    counter += 1;
                }
                else if (counter == 10)
                {
                    lblListLabel10.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage10.Source = image;
                    counter += 1;
                }
            }
            if (listStart > 10)
            {
                btnListPrev.Visibility = Visibility.Visible;
                btnListPrev.IsEnabled = true;
            }
            else
            {
                btnListPrev.Visibility = Visibility.Hidden;
                btnListPrev.IsEnabled = false;
            }
            listStart += 10;
            if (movieList.ElementAtOrDefault(listStart) != null)
            {
                btnListNext.Visibility = Visibility.Visible;
                btnListNext.IsEnabled = true;
            }
            else
            {
                btnListNext.Visibility = Visibility.Hidden;
                btnListNext.IsEnabled = false;
            }
            listGrid.Opacity = 0;
            listGrid.IsEnabled = true;
            listGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            listGrid.BeginAnimation(Grid.OpacityProperty, ani);
        }

        private void hideMovieGrid()
        {
            wbMovie.Address = string.Format("http://blank.org/");
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            movieGrid.BeginAnimation(Grid.OpacityProperty, ani);
            movieGrid.IsEnabled = false;
            movieGrid.Visibility = Visibility.Hidden;
            imgLoading.IsEnabled = true;
  
        }

        private void showMovieGrid()
        {
            var image = LoadImage(movieList[currentSelectedMovie].FileData);
            imgMovie.Source = image;
            lblMovieTitle.Content = movieList[currentSelectedMovie].Title;
            lblMovieType.Content = movieList[currentSelectedMovie].MovieType;
            lblMoviePrice.Content = "$ " + String.Format("{0:.00}", movieList[currentSelectedMovie].Price);
            movieGrid.Opacity = 0;
            movieGrid.IsEnabled = true;
            movieGrid.Visibility = Visibility.Visible;
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            movieGrid.BeginAnimation(Grid.OpacityProperty, ani);
            wbMovie.IsEnabled = true;
            wbMovie.Address = string.Format("https://www.youtube.com/embed/{0}?version=3&playlist=1&hd=1&autoplay=1&fs=0&autohide=1&loop=1&controls=0", movieList[currentSelectedMovie].VideoId);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex emailReg = new Regex(emailPattern);
            if (!emailReg.IsMatch(txtLoginEmail.Text) || txtLoginPassword.Password == "")
            {
                lblLoginIncorrect.Visibility = Visibility.Visible;
            }
            else
            {
                NetworkStream stream = new NetworkStream(socket);
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                try
                {
                    writer.AutoFlush = true;
                    writer.WriteLine("login");
                    writer.WriteLine(txtLoginEmail.Text);
                    string password = sha256_hash(txtLoginPassword.Password);
                    writer.WriteLine(password);
                    if (reader.ReadLine() == "authorized")
                    {
                        currentUser.setFirstName(reader.ReadLine());
                        currentUser.setMiddleName(reader.ReadLine());
                        currentUser.setLastName(reader.ReadLine());
                        currentUser.setDOB(reader.ReadLine());
                        currentUser.setEmail(txtLoginEmail.Text);
                        currentUser.setPassword(password);
                        txtLoginEmail.Text = "";
                        txtLoginPassword.Password = "";
                        lblLoginIncorrect.Visibility = Visibility.Hidden;
                        hideLoginGrid();
                        showHomeGrid();
                        btnHomeSignIn.IsEnabled = false;
                        btnHomeSignIn.Visibility = Visibility.Hidden;
                        btnHomeRegister.IsEnabled = false;
                        btnHomeRegister.Visibility = Visibility.Hidden;
                        btnHomeSignIn.Visibility = Visibility.Hidden;
                        lblHomeHello.Visibility = Visibility.Visible;
                        lblHomeName.Visibility = Visibility.Visible;
                        if ((currentUser.getFirstName() + " " + currentUser.getMiddleName() + " " + currentUser.getLastName()).Length > 20)
                        {
                            if ((currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()).Length > 20)
                            {
                                if ((currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".").Length > 20)
                                {
                                    lblHomeName.Content = currentUser.getFirstName()[0] + ". " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".";
                                }
                                else
                                {
                                    lblHomeName.Content = currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".";
                                }
                            }
                            else
                            {
                                lblHomeName.Content = currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName();
                            }
                        }
                        else
                        {
                            lblHomeName.Content = currentUser.getFirstName() + " " + currentUser.getMiddleName() + " " + currentUser.getLastName();
                        }
                    }
                    else
                    {
                        lblLoginIncorrect.Visibility = Visibility.Visible;
                        txtLoginPassword.Password = "";
                    }
                }
                catch (Exception error)
                {
                    throw error;
                }
            }

        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        private void btnLoginRegister_Click(object sender, RoutedEventArgs e)
        {
            hideLoginGrid();
            showRegisterGrid();
        }

        private void btnRegisterRegister_Click(object sender, RoutedEventArgs e)
        {
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex emailReg = new Regex(emailPattern);
            string datePattern = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
            Regex dateReg = new Regex(datePattern);
            string error = "";
            if (!emailReg.IsMatch(txtRegisterEmail.Text))
            {
                error += "Enter a valid email address \n";
            }
            if (txtRegisterPassword.Password == "")
            {
                error += "The password is empty!\n";
            }
            if (txtRegisterPassword.Password != txtRegisterConfirmPassword.Password)
            {
                error += "The passwords do not match\n";
            }
            if (txtRegisterFirstName.Text == "")
            {
                error += "Please enter a first name.\n";
            }
            if (txtRegisterLastName.Text == "")
            {
                error += "Please enter a last name.\n";
            }
            if (!dateReg.IsMatch(txtRegisterDOB.Text))
            {
                error += "Enter a valid date in the format (DD/MM/YYYY)\n";
            }
            if (error != "")
            {
                error.Insert(0, "Error, the following fields have incorrect input:\n");
                MessageBox.Show(error);
            }
            else
            {
                try
                {
                    NetworkStream stream = new NetworkStream(socket);
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    writer.WriteLine("register");
                    writer.WriteLine(txtRegisterEmail.Text);
                    writer.WriteLine(sha256_hash(txtRegisterPassword.Password));
                    writer.WriteLine(txtRegisterFirstName.Text);
                    writer.WriteLine(txtRegisterMiddleName.Text);
                    writer.WriteLine(txtRegisterLastName.Text);
                    writer.WriteLine(txtRegisterDOB.Text);
                    writer.Flush();
                    if (reader.ReadLine() == "success")
                    {
                        currentUser.setEmail(txtRegisterEmail.Text);
                        currentUser.setPassword(sha256_hash(txtRegisterPassword.Password));
                        currentUser.setFirstName(txtRegisterFirstName.Text);
                        currentUser.setMiddleName(txtRegisterMiddleName.Text);
                        currentUser.setLastName(txtRegisterLastName.Text);
                        currentUser.setDOB(txtRegisterDOB.Text);
                        txtRegisterEmail.Text = "";
                        txtRegisterPassword.Password = "";
                        txtRegisterConfirmPassword.Password = "";
                        txtRegisterFirstName.Text = "";
                        txtRegisterMiddleName.Text = "";
                        txtRegisterLastName.Text = "";
                        txtRegisterDOB.Text = "";
                        hideRegisterGrid();
                        showHomeGrid();
                        btnHomeSignIn.IsEnabled = false;
                        btnHomeSignIn.Visibility = Visibility.Hidden;
                        btnHomeRegister.IsEnabled = false;
                        btnHomeRegister.Visibility = Visibility.Hidden;
                        btnHomeSignIn.Visibility = Visibility.Hidden;
                        lblHomeHello.Visibility = Visibility.Visible;
                        lblHomeName.Visibility = Visibility.Visible;
                        if ((currentUser.getFirstName() + " " + currentUser.getMiddleName() + " " + currentUser.getLastName()).Length > 20)
                        {
                            if ((currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()).Length > 20)
                            {
                                if ((currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".").Length > 20)
                                {
                                    lblHomeName.Content = currentUser.getFirstName()[0] + ". " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".";
                                }
                                else
                                {
                                    lblHomeName.Content = currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName()[0] + ".";
                                }
                            }
                            else
                            {
                                lblHomeName.Content = currentUser.getFirstName() + " " + (currentUser.getMiddleName())[0] + ". " + currentUser.getLastName();
                            }
                        }
                        else
                        {
                            lblHomeName.Content = currentUser.getFirstName() + " " + currentUser.getMiddleName() + " " + currentUser.getLastName();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error, another account with the same email exists.");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString());
                }
                
            }
            
                
        }

        private void rctHomeOpacity2_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.3));
            rctHomeOpacity2.BeginAnimation(Rectangle.OpacityProperty, ani);
        }
        private void rctHomeOpacity2_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.3));
            rctHomeOpacity2.BeginAnimation(Rectangle.OpacityProperty, ani);
        }

        private void rctHomeOpacity2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentUser.getEmail() != null)
            {
                hideHomeGrid();
                showListGrid();
            }
            else
            {
                hideHomeGrid();
                showLoginGrid();
            }
        }

        private void btnListNext_Click(object sender, RoutedEventArgs e)
        {
            lblListLabel1.Content = "";
            lblListLabel2.Content = "";
            lblListLabel3.Content = "";
            lblListLabel4.Content = "";
            lblListLabel5.Content = "";
            lblListLabel6.Content = "";
            lblListLabel7.Content = "";
            lblListLabel8.Content = "";
            lblListLabel9.Content = "";
            lblListLabel10.Content = "";
            imgListImage1.Source = null;
            imgListImage2.Source = null;
            imgListImage3.Source = null;
            imgListImage4.Source = null;
            imgListImage5.Source = null;
            imgListImage6.Source = null;
            imgListImage7.Source = null;
            imgListImage8.Source = null;
            imgListImage9.Source = null;
            imgListImage10.Source = null;
            int counter = 1;
            int x = 0;
            if (movieList.Count < listStart + 10)
            {
                x = movieList.Count;
            }
            else
            {
                x = listStart + 10;
            }
            for (int i = listStart; i < x; i++)
            {
                if (counter == 1)
                {
                    lblListLabel1.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage1.Source = image;
                    counter += 1;
                }
                else if (counter == 2)
                {
                    lblListLabel2.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage2.Source = image;
                    counter += 1;
                }
                else if (counter == 3)
                {
                    lblListLabel3.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage3.Source = image;
                    counter += 1;
                }
                else if (counter == 4)
                {
                    lblListLabel4.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage4.Source = image;
                    counter += 1;
                }
                else if (counter == 5)
                {
                    lblListLabel5.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage5.Source = image;
                    counter += 1;
                }
                else if (counter == 6)
                {
                    lblListLabel6.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage6.Source = image;
                    counter += 1;
                }
                else if (counter == 7)
                {
                    lblListLabel7.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage7.Source = image;
                    counter += 1;
                }
                else if (counter == 8)
                {
                    lblListLabel8.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage8.Source = image;
                    counter += 1;
                }
                else if (counter == 9)
                {
                    lblListLabel9.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage9.Source = image;
                    counter += 1;
                }
                else if (counter == 10)
                {
                    lblListLabel10.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage10.Source = image;
                    counter += 1;
                }
            }
            if (listStart > 0)
            {
                btnListPrev.Visibility = Visibility.Visible;
                btnListPrev.IsEnabled = true;
            }
            else
            {
                btnListPrev.Visibility = Visibility.Visible;
                btnListPrev.IsEnabled = true;
            }
            listStart += 10;
            if (movieList.ElementAtOrDefault(listStart) != null)
            {
                btnListNext.Visibility = Visibility.Visible;
                btnListNext.IsEnabled = true;
            }
            else
            {
                btnListNext.Visibility = Visibility.Hidden;
                btnListNext.IsEnabled = false;
            }
        }

        private void btnListPrev_Click(object sender, RoutedEventArgs e)
        {
            listStart -= 20;
            lblListLabel1.Content = "";
            lblListLabel2.Content = "";
            lblListLabel3.Content = "";
            lblListLabel4.Content = "";
            lblListLabel5.Content = "";
            lblListLabel6.Content = "";
            lblListLabel7.Content = "";
            lblListLabel8.Content = "";
            lblListLabel9.Content = "";
            lblListLabel10.Content = "";
            imgListImage1.Source = null;
            imgListImage2.Source = null;
            imgListImage3.Source = null;
            imgListImage4.Source = null;
            imgListImage5.Source = null;
            imgListImage6.Source = null;
            imgListImage7.Source = null;
            imgListImage8.Source = null;
            imgListImage9.Source = null;
            imgListImage10.Source = null;
            int counter = 1;
            int x = 0;
            if (movieList.Count < listStart + 10)
            {
                x = movieList.Count;
            }
            else
            {
                x = listStart + 10;
            }
            for (int i = listStart; i < x; i++)
            {
                if (counter == 1)
                {
                    lblListLabel1.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage1.Source = image;
                    counter += 1;
                }
                else if (counter == 2)
                {
                    lblListLabel2.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage2.Source = image;
                    counter += 1;
                }
                else if (counter == 3)
                {
                    lblListLabel3.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage3.Source = image;
                    counter += 1;
                }
                else if (counter == 4)
                {
                    lblListLabel4.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage4.Source = image;
                    counter += 1;
                }
                else if (counter == 5)
                {
                    lblListLabel5.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage5.Source = image;
                    counter += 1;
                }
                else if (counter == 6)
                {
                    lblListLabel6.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage6.Source = image;
                    counter += 1;
                }
                else if (counter == 7)
                {
                    lblListLabel7.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage7.Source = image;
                    counter += 1;
                }
                else if (counter == 8)
                {
                    lblListLabel8.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage8.Source = image;
                    counter += 1;
                }
                else if (counter == 9)
                {
                    lblListLabel9.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage9.Source = image;
                    counter += 1;
                }
                else if (counter == 10)
                {
                    lblListLabel10.Content = movieList[i].Title;
                    var image = LoadImage(movieList[i].FileData);
                    imgListImage10.Source = image;
                    counter += 1;
                }
            }
            if (listStart > 10)
            {
                btnListPrev.Visibility = Visibility.Visible;
                btnListPrev.IsEnabled = true;
            }
            else
            {
                btnListPrev.Visibility = Visibility.Hidden;
                btnListPrev.IsEnabled = false;
            }
            listStart += 10;
            if (movieList.ElementAtOrDefault(listStart) != null)
            {
                btnListNext.Visibility = Visibility.Visible;
                btnListNext.IsEnabled = true;
            }
            else
            {
                btnListNext.Visibility = Visibility.Hidden;
                btnListNext.IsEnabled = false;
            }
        }

        private void rctList1_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 10) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList1.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList2_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 9) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList2.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList3_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 8) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList3.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList4_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 7) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList4.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList5_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 6) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList5.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList6_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 5) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList6.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList7_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 4) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList7.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList8_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 3) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList8.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList9_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 2) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList9.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList10_MouseEnter(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 1) != null) {
                DoubleAnimation ani = new DoubleAnimation(0.2, TimeSpan.FromSeconds(0.1));
                rctList10.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 10) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList1.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 9) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList2.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 8) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList3.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList4_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 7) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList4.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList5_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 6) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList5.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList6_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 5) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList6.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList7_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 4) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList7.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList8_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 3) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList8.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList9_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 2) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList9.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList10_MouseLeave(object sender, MouseEventArgs e)
        {
            if (movieList.ElementAtOrDefault(listStart - 1) != null)
            {
                DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                rctList10.BeginAnimation(Rectangle.OpacityProperty, ani);
            }
        }

        private void rctList1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 10;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 9;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
                
        }

        private void rctList3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 8;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 7;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 6;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 5;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 4;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 3;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 2;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void rctList10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentSelectedMovie = listStart - 1;
            if (currentSelectedMovie < movieList.Count)
            {
                hideListGrid();
                showMovieGrid();
            }
        }

        private void btnMovieBack_Click(object sender, RoutedEventArgs e)
        {
            hideMovieGrid();
            showListGrid();
        }

        private void btnMovieBook_Click(object sender, RoutedEventArgs e)
        {
            hideMovieGrid();
            showBookingGrid();
        }

        private void wbMovie_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    imgLoading.IsEnabled = false;
                });

            }
        }
        private void showBookingGrid()
        {
            try
            {
                bookingGrid.Opacity = 0;
                bookingGrid.IsEnabled = true;
                bookingGrid.Visibility = Visibility.Visible;
                DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                bookingGrid.BeginAnimation(Grid.OpacityProperty, ani);
                BrushConverter bc = new BrushConverter();
                btnBookingA1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingA2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingA3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingA4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingA5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingB1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingB2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingB3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingB4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingB5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingC1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingC2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingC3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingC4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingC5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                btnBookingA1.IsEnabled = false;
                btnBookingA2.IsEnabled = false;
                btnBookingA3.IsEnabled = false;
                btnBookingA4.IsEnabled = false;
                btnBookingA5.IsEnabled = false;
                btnBookingB1.IsEnabled = false;
                btnBookingB2.IsEnabled = false;
                btnBookingB3.IsEnabled = false;
                btnBookingB4.IsEnabled = false;
                btnBookingB5.IsEnabled = false;
                btnBookingC1.IsEnabled = false;
                btnBookingC2.IsEnabled = false;
                btnBookingC3.IsEnabled = false;
                btnBookingC4.IsEnabled = false;
                btnBookingC5.IsEnabled = false;
                btnBookingConfirm.IsEnabled = false;
                NetworkStream stream = new NetworkStream(socket);
                StreamWriter writer = new StreamWriter(stream);
                StreamReader read = new StreamReader(stream);
                writer.AutoFlush = true;
                writer.WriteLine("request_showtime");
                writer.WriteLine(movieList[currentSelectedMovie].Title);
                lblBookingTicketPrice.Content = "$ " + String.Format("{0:.00}", movieList[currentSelectedMovie].Price);
                lblBookingTotal.Content = "$ 0.00";
                string xml = "";
                string line;
                //string randomVar = read.ReadLine();
                var xs = new XmlSerializer(typeof(List<String>));
                while ((line = read.ReadLine()) != "endofxml")
                {
                    xml += line;
                }
                using (var reader = new StringReader(xml))
                {
                    showtimeList = (List<String>)xs.Deserialize(reader);
                }
                lblBookingTitle.Content = movieList[currentSelectedMovie].Title;
                for (int i = 0; i < showtimeList.Count; i++)
                {
                    string[] tempList = showtimeList[i].Split(';');
                    ddlBookingDate.Items.Add(tempList[1] + " on " + tempList[0]);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }
        private void hideBookingGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            bookingGrid.BeginAnimation(Grid.OpacityProperty, ani);
            bookingGrid.IsEnabled = false;
            bookingGrid.Visibility = Visibility.Hidden;
            lblBookingSeats.Text = "";
            lblBookingTotalSeats.Content = "0";
            showtimeList.Clear();
        }

        private void btnBookingBack_Click(object sender, RoutedEventArgs e)
        {
            hideBookingGrid();
            showListGrid();
        }

        private void ddlBookingDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnBookingA1.IsEnabled = true;
            btnBookingA2.IsEnabled = true;
            btnBookingA3.IsEnabled = true;
            btnBookingA4.IsEnabled = true;
            btnBookingA5.IsEnabled = true;
            btnBookingB1.IsEnabled = true;
            btnBookingB2.IsEnabled = true;
            btnBookingB3.IsEnabled = true;
            btnBookingB4.IsEnabled = true;
            btnBookingB5.IsEnabled = true;
            btnBookingC1.IsEnabled = true;
            btnBookingC2.IsEnabled = true;
            btnBookingC3.IsEnabled = true;
            btnBookingC4.IsEnabled = true;
            btnBookingC5.IsEnabled = true;
            lblBookingSeats.Text = "";
            lblBookingTotalSeats.Content = "0";
            lblBookingTotal.Content = "$ 0.00";
            BrushConverter bc = new BrushConverter();
            btnBookingA1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingA2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingA3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingA4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingA5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingB1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingB2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingB3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingB4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingB5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingC1.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingC2.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingC3.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingC4.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingC5.Background = (Brush)bc.ConvertFrom("#FFFF0000");
            btnBookingConfirm.IsEnabled = true;
            string tempVal = ddlBookingDate.SelectedValue.ToString().Replace(" on ", ";");
            String[] tempArray = tempVal.Split(';');

            for (int i = 0; i < showtimeList.Count; i++)
            {
                if (showtimeList[i].StartsWith(tempArray[1] + ";" + tempArray[0]))
                {
                    if (showtimeList[i].Contains("C1"))
                    {
                        btnBookingC1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("C2"))
                    {
                        btnBookingC2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("C3"))
                    {
                        btnBookingC3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("C4"))
                    {
                        btnBookingC4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("C5"))
                    {
                        btnBookingC5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("B1"))
                    {
                        btnBookingB1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("B2"))
                    {
                        btnBookingB2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("B3"))
                    {
                        btnBookingB3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("B4"))
                    {
                        btnBookingB4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("B5"))
                    {
                        btnBookingB5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("A1"))
                    {
                        btnBookingA1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("A2"))
                    {
                        btnBookingA2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("A3"))
                    {
                        btnBookingA3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("A4"))
                    {
                        btnBookingA4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                    if (showtimeList[i].Contains("A5"))
                    {
                        btnBookingA5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                    }
                }
            }
        }

        private void btnBookingC1_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingC1.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingC1.Background.ToString() == "#FF033A00")
            {
                btnBookingC1.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "C1";
                }
                else
                {
                    lblBookingSeats.Text += ", C1";
                }
            }
            else
            {
                btnBookingC1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("C1"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C1, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C1", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", C1", "");
                }
            }
        }

        private void btnBookingC2_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingC2.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingC2.Background.ToString() == "#FF033A00")
            {
                btnBookingC2.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "C2";
                }
                else
                {
                    lblBookingSeats.Text += ", C2";
                }
            }
            else
            {
                btnBookingC2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("C2"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C2, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C2", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", C2", "");
                }
            }
        }

        private void btnBookingC3_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingC3.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingC3.Background.ToString() == "#FF033A00")
            {
                btnBookingC3.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "C3";
                }
                else
                {
                    lblBookingSeats.Text += ", C3";
                }
            }
            else
            {
                btnBookingC3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("C3"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C3, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C3", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", C3", "");
                }
            }
        }

        private void btnBookingC4_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingC4.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingC4.Background.ToString() == "#FF033A00")
            {
                btnBookingC4.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "C4";
                }
                else
                {
                    lblBookingSeats.Text += ", C4";
                }
            }
            else
            {
                btnBookingC4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("C4"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C4, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C4", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", C4", "");
                }
            }
        }

        private void btnBookingC5_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingC5.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingC5.Background.ToString() == "#FF033A00")
            {
                btnBookingC5.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "C5";
                }
                else
                {
                    lblBookingSeats.Text += ", C5";
                }
            }
            else
            {
                btnBookingC5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("C5"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C5, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("C5", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", C5", "");
                }
            }
        }

        private void btnBookingB1_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingB1.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingB1.Background.ToString() == "#FF033A00")
            {
                btnBookingB1.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "B1";
                }
                else
                {
                    lblBookingSeats.Text += ", B1";
                }
            }
            else
            {
                btnBookingB1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("B1"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B1, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B1", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", B1", "");
                }
            }
        }

        private void btnBookingB2_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingB2.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingB2.Background.ToString() == "#FF033A00")
            {
                btnBookingB2.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "B2";
                }
                else
                {
                    lblBookingSeats.Text += ", B2";
                }
            }
            else
            {
                btnBookingB2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("B2"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B2, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B2", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", B2", "");
                }
            }
        }

        private void btnBookingB3_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingB3.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingB3.Background.ToString() == "#FF033A00")
            {
                btnBookingB3.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "B3";
                }
                else
                {
                    lblBookingSeats.Text += ", B3";
                }
            }
            else
            {
                btnBookingB3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("B3"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B3, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B3", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", B3", "");
                }
            }
        }

        private void btnBookingB4_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingB4.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingB4.Background.ToString() == "#FF033A00")
            {
                btnBookingB4.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "B4";
                }
                else
                {
                    lblBookingSeats.Text += ", B4";
                }
            }
            else
            {
                btnBookingB4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("B4"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B4, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B4", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", B4", "");
                }
            }
        }

        private void btnBookingB5_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingB5.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingB5.Background.ToString() == "#FF033A00")
            {
                btnBookingB5.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "B5";
                }
                else
                {
                    lblBookingSeats.Text += ", B5";
                }
            }
            else
            {
                btnBookingB5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("B5"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B5, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("B5", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", B5", "");
                }
            }
        }

        private void btnBookingA1_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingA1.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingA1.Background.ToString() == "#FF033A00")
            {
                btnBookingA1.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "A1";
                }
                else
                {
                    lblBookingSeats.Text += ", A1";
                }
            }
            else
            {
                btnBookingA1.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("A1"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A1, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A1", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", A1", "");
                }
            }
        }

        private void btnBookingA2_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingA2.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingA2.Background.ToString() == "#FF033A00")
            {
                btnBookingA2.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "A2";
                }
                else
                {
                    lblBookingSeats.Text += ", A2";
                }
            }
            else
            {
                btnBookingA2.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("A2"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A2, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A2", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", A2", "");
                }
            }
        }

        private void btnBookingA3_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingA3.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingA3.Background.ToString() == "#FF033A00")
            {
                btnBookingA3.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "A3";
                }
                else
                {
                    lblBookingSeats.Text += ", A3";
                }
            }
            else
            {
                btnBookingA3.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("A3"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A3, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A3", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", A3", "");
                }
            }
        }

        private void btnBookingA4_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingA4.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingA4.Background.ToString() == "#FF033A00")
            {
                btnBookingA4.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "A4";
                }
                else
                {
                    lblBookingSeats.Text += ", A4";
                }
            }
            else
            {
                btnBookingA4.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("A4"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A4, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A4", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", A4", "");
                }
            }
        }

        private void btnBookingA5_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (btnBookingA5.Background.ToString() == "#FFFF0000")
            {
                System.Windows.MessageBox.Show("This seat is already taken!");
            }
            else if (btnBookingA5.Background.ToString() == "#FF033A00")
            {
                btnBookingA5.Background = (Brush)bc.ConvertFrom("#FF004385");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats += 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.ToString() == "")
                {
                    lblBookingSeats.Text += "A5";
                }
                else
                {
                    lblBookingSeats.Text += ", A5";
                }
            }
            else
            {
                btnBookingA5.Background = (Brush)bc.ConvertFrom("#FF033A00");
                int seats = Convert.ToInt32(lblBookingTotalSeats.Content);
                seats -= 1;
                lblBookingTotalSeats.Content = seats.ToString();
                double total = Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", ""));
                double price = Convert.ToDouble(lblBookingTicketPrice.Content.ToString().Replace("$ ", ""));
                total = seats * price;
                lblBookingTotal.Content = "$ " + String.Format("{0:.00}", total);

                if (lblBookingSeats.Text.StartsWith("A5"))
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A5, ", "");
                    lblBookingSeats.Text = lblBookingSeats.Text.Replace("A5", "");
                }
                else
                {
                    lblBookingSeats.Text = lblBookingSeats.Text.ToString().Replace(", A5", "");
                }
            }
        }

        private void btnBookingConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NetworkStream stream = new NetworkStream(socket);
                StreamWriter writer = new StreamWriter(stream);
                StreamReader read = new StreamReader(stream);
                writer.AutoFlush = true;
                writer.WriteLine("book_movie");
                writer.WriteLine(Guid.NewGuid().ToString());
                writer.WriteLine(movieList[currentSelectedMovie].Title);
                writer.WriteLine(currentUser.getEmail());
                writer.WriteLine(Convert.ToDouble(lblBookingTotal.Content.ToString().Replace("$ ", "")));
                string dt = ddlBookingDate.SelectedValue.ToString().Replace(" on ", ";");
                String[] datetime = dt.Split(';');
                writer.WriteLine(datetime[1]);
                writer.WriteLine(datetime[0]);
                writer.WriteLine(lblBookingSeats.Text.ToString().Replace(", ", "|"));
                string status = read.ReadLine();
                if (status == "success")
                {
                    txtConfirmSeats.Text = "Booked Seats: " + lblBookingSeats.Text;
                    lblConfirmTime.Text = ddlBookingDate.SelectedValue.ToString();
                    hideBookingGrid();
                    showConfirmGrid();
                }
                else
                {
                    System.Windows.MessageBox.Show("Error! Please try again later!");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            
        }

        private void btnConfirmRecept_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "BookingReceipt.txt";

            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == true)

            {

                using (StreamWriter writer = new StreamWriter(save.FileName))
                {
                    writer.WriteLine("Ticketter");
                    writer.WriteLine("================================================");
                    writer.WriteLine("Booking for: " + currentUser.getFirstName() + " " + currentUser.getMiddleName() + " " + currentUser.getLastName());
                    writer.WriteLine("================================================");
                    writer.WriteLine("Seats booked: " + txtConfirmSeats.Text.ToString().Replace("Booked Seats: ", ""));
                    writer.WriteLine("Date and Time of Movie: " + lblConfirmTime.Text);
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("");
                    writer.WriteLine("Total paid: " + String.Format("{0:.00}", lblBookingTotal.Content.ToString()));

                    writer.Flush();

                    writer.Close();
                }
            }
        }

        private void btnConfirmBack_Click(object sender, RoutedEventArgs e)
        {
            hideConfirmGrid();
            showListGrid();
        }

        private void showCheckGrid()
        {
            try
            {
                dgCheckBookings.Items.Clear();
                NetworkStream stream = new NetworkStream(socket);
                StreamWriter writer = new StreamWriter(stream);
                StreamReader read = new StreamReader(stream);
                writer.AutoFlush = true;
                writer.WriteLine("view_booking");
                writer.WriteLine(currentUser.getEmail());
                string xml = "";
                string line;
                //string randomVar = read.ReadLine();
                var xs = new XmlSerializer(typeof(HashSet<Booking>));
                while ((line = read.ReadLine()) != "endofxml")
                {
                    if (line == "fail")
                    {
                        break;
                    }
                    else
                    {
                        xml += line;
                    }
                }

                if (line == "fail")
                {
                    System.Windows.MessageBox.Show("You have no bookings.");
                    showListGrid();
                }
                else
                {
                    bool success = true;
                    HashSet<Booking> newSet = new HashSet<Booking>();
                    using (var reader = new StringReader(xml))
                    {
                        try
                        {
                            newSet = (HashSet<Booking>)xs.Deserialize(reader);
                        }
                        catch (Exception e)
                        {
                            success = false;
                        }

                    }
                    if (success)
                    {
                        List<Booking> bookList = newSet.ToList();
                        for (int i = 0; i < bookList.Count; i++)
                        {
                            string seats = "";
                            bool first = true;
                            for (int x = 0; x < (bookList[i].Seats).Length; x++)
                            {
                                if (first)
                                {
                                    seats += bookList[i].Seats[x];
                                    first = false;
                                }
                                else
                                {
                                    seats += ", " + bookList[i].Seats[x];
                                }
                            }

                            var data = new tempDGClass { ID = bookList[i].TransactionId, Title = bookList[i].Movie, Seats = seats, Time = bookList[i].Timeslot, Date = bookList[i].Date };
                            dgCheckBookings.Items.Add(data);
                        }
                    }
                    checkGrid.Opacity = 0;
                    checkGrid.IsEnabled = true;
                    checkGrid.Visibility = Visibility.Visible;
                    DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                    checkGrid.BeginAnimation(Grid.OpacityProperty, ani);
                }
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }
        }
        private void hideCheckGrid()
        {
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            checkGrid.BeginAnimation(Grid.OpacityProperty, ani);
            checkGrid.IsEnabled = false;
            checkGrid.Visibility = Visibility.Hidden;
        }
        private void btnListBookings_Click(object sender, RoutedEventArgs e)
        {
            hideListGrid();
            showCheckGrid();
        }

        private void btnCheckBack_Click(object sender, RoutedEventArgs e)
        {
            hideCheckGrid();
            showListGrid();
        }

        public class tempDGClass
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string Seats { get; set; }
            public string Time { get; set; }
            public string Date { get; set; }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tempDGClass booking = dgCheckBookings.SelectedItem as tempDGClass;
            Clipboard.SetText(booking.ID.ToString());
            txtCheckDelete.Text = booking.ID.ToString();
        }

        private void btnCheckDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txtCheckDelete.Text == "")
            {
                MessageBox.Show("Error, the ID field cannot be empty.");
            }
            else
            {
                try
                {
                    NetworkStream stream = new NetworkStream(socket);
                    StreamWriter writer = new StreamWriter(stream);
                    StreamReader read = new StreamReader(stream);
                    writer.AutoFlush = true;
                    writer.WriteLine("remove_booking");
                    writer.WriteLine(txtCheckDelete.Text);
                    if (read.ReadLine() == "success")
                    {
                        hideCheckGrid();
                        showCheckGrid();

                    }
                    else
                    {
                        MessageBox.Show("No such ID!");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString());
                }
            }
        }
    }
}
