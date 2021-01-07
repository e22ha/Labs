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

/// <summary>
/// Разработайте и реализуйте приложение WPF, которое:
///-содержит 2 тестовых поля подписанных как “А” и “Б”
///- четыре кнопки подписанных как “+”, “-”, “*” и “/”
///- поле для вывода текста
///- пользователь может ввести числа в текстовые поля, нажать кнопку и получить в текстовом
///поле результат, соответствующей арифметической операции над введёнными числами в
///текстовом поле
///Пример выполнение операции сложения:
/// </summary>
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double a;
        public double b;
        public MainWindow()
        {
            InitializeComponent();
        }
        //Сложение
        private void addition_Click(object sender, RoutedEventArgs e)
        {
            a = double.Parse(a_var.Text);
            b = double.Parse(b_var.Text);
            double c = a + b;
            Output.Content = c;
        }
        //Вычитание
        private void subtraction_Click(object sender, RoutedEventArgs e)
        {
            a = double.Parse(a_var.Text);
            b = double.Parse(b_var.Text);
            double c = a - b;
            Output.Content = c;
        }
        //Деление
        private void division_Click(object sender, RoutedEventArgs e)
        {
            a = double.Parse(a_var.Text);
            b = double.Parse(b_var.Text);
            double c = a / b;
            Output.Content = c;
        }
        //Умножение
        private void multip_Click(object sender, RoutedEventArgs e)
        {
            a = double.Parse(a_var.Text);
            b = double.Parse(b_var.Text);
            double c = a * b;
            Output.Content = c;
        }

        
    }
}
