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
        SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=D:\\Code\\LABs_2\\WpfApp1\\dataBase.sqlite;Version=3;");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void add_stud_Click(object sender, RoutedEventArgs e)
        {
            //открытие соединения с базой данных
            m_dbConnection.Open();
            //выполнение запросов
            //формирование запроса на добавление данных в поля типа INTEGER и TEXT
            //обратите внимание, что в текстовое поле, данные добавляются в формате ‘data’
            string sql = "INSERT INTO id_name (id, fio) VALUES (" + id.Text + ",'" + name.Text + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            string sql1 = "INSERT INTO id_mark (id, mark_math, mark_physics) VALUES (" + id.Text + ",'" + markPhysics.Text + "','" + markMath.Text + "')";
            SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
            //извлечение запроса
            command.ExecuteNonQuery();
            data_name_udate(m_dbConnection);
            command1.ExecuteNonQuery();
            data_mark_udate(m_dbConnection);
            //закрытие соединения с базой данных
            m_dbConnection.Close();
        }
        public class Cname
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        void data_name_udate(SQLiteConnection m_dbConnection)
        {
            data_name.Items.Clear();

            string sql = "SELECT * FROM id_name ORDER BY id";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                //создание строки
                var data = new Cname
                {
                    id = int.Parse(reader[0].ToString()),
                    name = reader[1].ToString()
                };
                //добавление строки в DataGrid
                data_name.Items.Add(data);
            }
        }
        public class Cmark
        {
            public int id { get; set; }
            public string mark_math { get; set; }
            public string mark_physics { get; set; }
        }

        void data_mark_udate(SQLiteConnection m_dbConnection)
        {
            dataView.Items.Clear();

            string sql = "SELECT * FROM id_mark ORDER BY id";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                //создание строки
                var data = new Cmark
                {
                    id = int.Parse(reader[0].ToString()),
                    mark_math = reader[1].ToString(),
                    mark_physics = reader[2].ToString()
                };
                //добавление строки в DataGrid
                dataView.Items.Add(data);
            }


        }
        bool exist(int ind)
        {
            int id_of_data = -1;
            string sql = $"SELECT id FROM id_name ORDER BY id";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id_of_data = int.Parse(reader[0].ToString());
                if (ind == id_of_data)
                {
                    return true;
                }
            }

            return false;
        }
        int ind(SQLiteConnection m_dbConnection)
        {
            int index = 0;
            string sql = "SELECT id FROM id_name ORDER BY id DESC LIMIT 1";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                index = int.Parse(reader["id"].ToString());
            return (index + 1);
        }

    }
}
