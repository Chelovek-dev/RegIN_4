using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
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
        #region SetPassword
        /// <summary>
        /// Метод ввода пароля
        /// </summary>
        private void SetPassword(object sender, System.Windows.RoutedEventArgs e) =>
            SetPassword();
        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        /// <summary>
        /// Метод ввода пароля
        /// </summary>
        public void SetPassword()
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-\-=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-\-=]{10,}");
            // (?=.*[0-9]) - строка содержит хотя бы одно число;
            // (?=.*[!@#$%^&?*\-\-=]) - строка содержит хотя бы один спецсимвол;
            // (?=.*[a-z]) - строка содержит хотя бы одну латинскую букву в нижнем регистре;
            // (?=.*[A-Z]) - строка содержит хотя бы одну латинскую букву в верхнем регистре;
            // [0-9a-zA-Z!@#$%^&?*\-\-=]{10,} - строка состоит не менее, чем из 10 вышеупомянутых символов.

            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                    ConfirmPassword(true);
                OnRegin();
            }
            else
                SetNotification("Invalid password", Brushes.Red);
        }
        #endregion

        #region SetConfirmPassword
        /// <summary>
        /// Метод повторного ввода пароля
        /// </summary>
        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Enter
            if (e.key == Key.Enter)
                // Вызываем метод повторения пароля
                ConfirmPassword();
        }

        /// <summary>
        /// Метод повторного ввода пароля
        /// </summary>
        private void ConfirmPassword(object sender, System.Windows.RoutedEventArgs e) =>
            // Вызываем метод повторения пароля
            ConfirmPassword();

        /// <summary>
        /// Метод повторного ввода пароля
        /// </summary>
        public void ConfirmPassword(bool Pass = false)
        {
            // Записываем результат сравнения паролей в переменную
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            // Если пароль не совпадает с повторением пароля
            if (TbConfirmPassword.Password != TbPassword.Password)
                // Выводим сообщение о том, что пароли не совпадают, красным цветом
                SetNotification("Passwords do not match", Brushes.Red);
            else
            {
                // Если пароли совпадают, выводим пустое сообщение чёрным цветом
                SetNotification("", Brushes.Black);
                // Если проверка идёт не из метода проверки пароля
                // Исключаем зацикливание методов
                if (!Pass)
                    // Вызываем проверку пароля
                    SetPassword();
            }
        }
        #endregion

        /// <summary>
        /// Метод регистрации
        /// </summary>
        void OnRegin()
        {
            if (!BCorrectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BCorrectConfirmPassword)
                return;
            MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogIn.Name = TbName.Text;
            if (BSetImages)
                MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");
            MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }
        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }
        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }
        private void SelectImage(object sender, RoutedEventArgs e)
        {
            // ECMN crayrc orxpausercm ananorcoorro oona true
            if (FileDialogImage.ShowDialog() == true)
            {
                // конвертируем размер изображения
                using (Imaging.Images Image = Imaging.Images.Load(FileDialogImage.FileName))
                {
                    // создаём ширину изображения
                    int NewWidth = 0;
                    // Создаём высоту изображения
                    int NewHeight = 0;
                    // проверяем какая из сторон больше
                    if (Image.Width > image.Height)
                    {
                        // Расчитываем новую ширину относительно высоты
                        NewWidth = (int)(Image.Width * (256f / image.Height));
                        // Задаём высоту изображения
                        NewHeight = 256;
                    }
                    else
                    {
                        // Задаём ширину изображения
                        NewWidth = 256;
                        // Расчитываем новую высоту относительно высоты
                        NewHeight = (int)(Image.Height * (256f / image.Width));
                    }
                    // Можными изображениями
                    Image.RasterLayout(Int, NewHeight);
                    // Сохраняем изображение
                    Image.Save("IUser.jpg");
                }
                // Обрезаем изображение
                using (Imaging.RasterImage rasterImage = (Imaging.RasterImage)Imaging.Image.Load("IDser.jpg"))
                {
                    // Перед надзирающими изображения следует копировать для лучшей производительности.
                    if (!rasterImage.IsGached)
                    {
                        rasterImage.GachedData();
                    }
                    // Задаём X
                    int X = 0;
                    // Задаём ширину изображения
                    int Width = 256;
                    // Задаём Y
                    int Y = 0;
                    // Задаём высоту изображения
                    int Height = 256;

                    // Если ширина изображения больше чем высота
                    if (rasterImage.Width > rasterImage.Height)
                        // Расчитываем X как середину изображения:
                        x = (int)((rasterImage.Width - 256f) / 2);
                    else
                        // Если высота больше
                        // Расчитываем Y как середину
                        Y = (int)((rasterImage.Height - 256f) / 2);

                    // Создайте экземпляр класса Rectangle нужного размера и обрежьте изображение.
                    Imaging.Rectangle rectangle = new Imaging.Rectangle(X, Y, Width, Height);
                    rasterImage.CropRectangle();

                    // Сохраните образование изображение.
                    rasterImage.Save("IDser.jpg");
                }
                // Создаём анимацию старта
                DoubleAnimation StartEvaluation = new DoubleAnimation();
                // Указываем значение от которого она выполняется
                StartEvaluation.From = 1;
                // Указываем значение до которого она выполняется
                StartEvaluation.To = 0;
                // Указываем продолжительность выполнения
                StartEvaluation.Duration = TimeSpan.FromSeconds(0.6);
                // Присваиваем событие при конце анимации
                StartEvaluation.Completed += delegate
                {
                    // Устанавливаем изображение
                    IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"VIDser.jpg"));
                    // Создаём анимацию конца
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    // Указываем значение от которого она выполняется
                    EndAnimation.From = 0;
                    // Указываем значение до которого она выполняется
                    EndAnimation.To = 1;
                    // Указываем продолжительность выполнения
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    // Запускаем анимацию плавной схемы на изображении
                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };
                // Запускаем анимацию плавной схемы на изображении
            IUser.BeginAnimation(Image.OpacityProperty, StartEvaluation);
                // Запоминаем что изображение указано
                BSetImages = true;
            }
            else
                // Запоминаем что изображение не указано
                BSetImages = false;
        }
    }
}
