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
using System.Data.SQLite;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=D:\\Code\\LABs_2\\WpfApp1\\dataBase.sqlite;Version=3;");
            //открытие соединения с базой данных
            m_dbConnection.Open();
            //выполнение запросов
            //формирование запроса на добавление данных в поля типа INTEGER и TEXT
            //обратите внимание, что в текстовое поле, данные добавляются в формате ‘data’
            string sql = "INSERT INTO id_name (id, fio) VALUES (" + tb1.Text + ",'" + tb2.Text + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            //извлечение запроса
            command.ExecuteNonQuery();

            //закрытие соединения с базой данных
            m_dbConnection.Close();

        }
    }
}
