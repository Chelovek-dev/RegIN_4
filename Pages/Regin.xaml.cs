using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;

namespace RegIN_Bulatov_Perevozshikova.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        bool BCorrectLogin = false;
        bool BCorrectPassword = false;
        bool BCorrectConfirmPassword = false;
        bool BSetImages = false;

        private void CorrectLogin()
        {
            SetNotification("Login already in use", Brushes.Red);
            BCorrectLogin = false;
        }
        private void InCorrectLogin() =>
            SetNotification("", Brushes.Black);

        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png) | *.png | JPG (*.jpg) | *.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                SetLogin();


        }
        private void SetLogin(object sender, System.Windows.RoutedEventArgs e) =>
            SetLogin();
        public void SetLogin()
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._-](4, )@[a-zA-Z0-9._-](2, [a-zA-Z0-9_-](2,))");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text) == true)
            {
                SetNotification("", Brushes.Black);
                // Вызываем получение данных пользователя по логину
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
            SetNotification("Invalid login", Brushes.Red);
            // Вызываем метод авторизации
            OnRegin();
        }
    }
}
