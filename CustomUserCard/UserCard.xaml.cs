using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CustomUserCard
{
    /// <summary>
    /// Логика взаимодействия для UserCard.xaml
    /// </summary>
    public partial class UserCard : UserControl
    {
        public string UCName1
        {
            get { return (string)GetValue(UCName1Property); }
            set { SetValue(UCName1Property, value); }
        }

        public static readonly DependencyProperty UCName1Property = DependencyProperty.Register(
                "lb_name1", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                 new PropertyMetadata(string.Empty, Name1Changed)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void Name1Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.lb_name1.Content = UCard.UCName1;
        }

        public string UCName2
        {
            get { return (string)GetValue(UCName2Property); }
            set { SetValue(UCName2Property, value); }
        }

        public static readonly DependencyProperty UCName2Property = DependencyProperty.Register(
                "lb_name2", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                 new PropertyMetadata(string.Empty, Name2Changed)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void Name2Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.lb_name2.Content = UCard.UCName2;
        }

        public string UCPosition
        {
            get { return (string)GetValue(UCPositionProperty); }
            set { SetValue(UCPositionProperty, value); }
        }

        public static readonly DependencyProperty UCPositionProperty = DependencyProperty.Register(
                "lb_position", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                 new PropertyMetadata(string.Empty, UCPosition1Changed)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void UCPosition1Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.lb_position.Content = UCard.UCPosition;
        }

        public string UCCompany
        {
            get { return (string)GetValue(UCPosition2Property); }
            set { SetValue(UCPosition2Property, value); }
        }

        public static readonly DependencyProperty UCPosition2Property = DependencyProperty.Register(
                "lb_company", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                 new PropertyMetadata(string.Empty, UCPosition2Changed)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void UCPosition2Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.lb_company.Content = UCard.UCCompany;
        }

        public string UCAva
        {
            get { return (string)GetValue(UCAvaProperty); }
            set { SetValue(UCAvaProperty, value); }
        }

        public static readonly DependencyProperty UCAvaProperty = DependencyProperty.Register(
                "image_avatar", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                new PropertyMetadata(string.Empty, AvaChanged)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void AvaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.image_avatar.Source = new BitmapImage(new Uri("pack://application:,,,/" + UCard.UCAva));
        }

        public string UCOnline
        {
            get { return (string)GetValue(UCOnlineProperty); }
            set { SetValue(UCOnlineProperty, value); }
        }

        public static readonly DependencyProperty UCOnlineProperty = DependencyProperty.Register(
                "img_online", // имя параметра в разметке
                typeof(string), // тип данных параметра
                typeof(UserCard), // тип данных элемента управления
                new PropertyMetadata(string.Empty, UCOnlineChanged)); // метаданные - значение параметра поумолчанию и обработчик изменения параметра
        private static void UCOnlineChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var UCard = obj as UserCard;
            UCard.img_online.Source = new BitmapImage(new Uri("pack://application:,,,/" + UCard.UCOnline));
        }

        public UserCard()
        {
            InitializeComponent();
            DateTime date = DateTime.Now;
            lb_timeOnline.Content = "был в сети " + date.ToString("d") + " в " + date.ToString("t");

            //this.image_avatar.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/image.jpg"));
            //image_avatar.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/image.jpg"));
        }

        //lb_name1.Content = "Name";
        //lb_name2.Content = "Name2";
        //lb_position.Content = "SEO";
        //lb_position2.Content = "versal";
    }
}
