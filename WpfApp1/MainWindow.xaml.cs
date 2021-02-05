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
        public SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=D:\\Code\\LABs_2\\WpfApp1\\dataBase.sqlite;Version=3;");

        public MainWindow()
        {
            InitializeComponent();
            data_name_udate(m_dbConnection);
        }

        private void data_name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data_name.SelectedItem != null)
            {
                //получение строки из DataGrid
                Cmark test = (Cmark)data_name.SelectedItem;
                Editor edit = new Editor();
                edit.id_tb.Text = test.id.ToString();
                edit.name_tb.Text = test.fio.ToString();
                edit.math_tb.Text = test.mark_math.ToString();
                edit.physics_tb.Text = test.mark_physics.ToString();

                //m_dbConnection.Open();
                //string mark_math = "";
                //string mark_physics = "";
                //string sql = "SELECT * FROM id_mark ORDER BY id";
                //SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                //SQLiteDataReader reader = command.ExecuteReader();

                //while (reader.Read())
                //{
                //    if (int.Parse(reader[0].ToString()) == int.Parse(test.id.ToString()))
                //    {
                //        mark_math = reader[1].ToString();
                //        mark_physics = reader[2].ToString();
                //    }
                //}
                //edit.math_tb.Text = mark_math;
                //edit.physics_tb.Text = mark_physics;

                edit.ShowDialog();

                if (edit.DialogResult == true)
                {
                    data_name_udate(m_dbConnection);
                }

                //m_dbConnection.Close();
            }
        }

        private void add_stud_Click(object sender, MouseButtonEventArgs e)
        {
            if ((id.Text == "") | (name.Text == ""))
            {
                MessageBox.Show("Введите имя и ID ༼ つ ◕_◕ ༽つ", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (exist(int.Parse(id.Text))==true)
            {
                MessageBox.Show("(☞ﾟヮﾟ)☞ Введите уникальный ID ☜(ﾟヮﾟ☜)", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //открытие соединения с базой данных
                m_dbConnection.Open();
                //выполнение запросов
                //формирование запроса на добавление данных в поля типа INTEGER и TEXT
                //обратите внимание, что в текстовое поле, данные добавляются в формате ‘data’
                string sql = "INSERT INTO id_name (id, fio) VALUES (" + id.Text + ",'" + name.Text + "')";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                string sql1 = "INSERT INTO id_mark (id, mark_math, mark_physics) VALUES (" + id.Text + ",'" + markMath.Text + "','" + markPhysics.Text + "')";
                SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                //извлечение запроса
                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                m_dbConnection.Close();
                data_name_udate(m_dbConnection);
                //закрытие соединения с базой данных
            }
        }

        public class Cmark
        {
            public int id { get; set; }
            public string fio { get; set; }
            public string mark_math { get; set; }
            public string mark_physics { get; set; }
        }

        void data_name_udate(SQLiteConnection m_dbConnection)
        {
            data_name.SelectedItem = null;
            data_name.Items.Clear();
            m_dbConnection.Open();

            string sql = "SELECT id_name.id, fio, mark_math, mark_physics FROM id_name,id_mark WHERE id_name.id = id_mark.id ORDER BY id_name.id";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                //создание строки
                var data = new Cmark
                {
                    id = int.Parse(reader[0].ToString()),
                    fio = reader[1].ToString(),
                    mark_math = reader[2].ToString(),
                    mark_physics = reader[3].ToString()
                };
                //добавление строки в DataGrid
                data_name.Items.Add(data);
            }
            m_dbConnection.Close();
        }

        //void data_mark_udate(SQLiteConnection m_dbConnection)
        //{
        //    data_name.SelectedItem = null;
        //    dataView.Items.Clear();

        //    string sql = "SELECT * FROM id_mark ORDER BY id";
        //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        //    SQLiteDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        //создание строки
        //        var data = new Cmark
        //        {
        //            id = int.Parse(reader[0].ToString()),
        //            mark_math = reader[1].ToString(),
        //            mark_physics = reader[2].ToString()
        //        };
        //        //добавление строки в DataGrid
        //        dataView.Items.Add(data);
        //    }
        //}

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool exist(int ind)
        {
            m_dbConnection.Open();
            int id_of_data = -1;
            string sql = $"SELECT id FROM id_name ORDER BY id";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id_of_data = int.Parse(reader[0].ToString());
                if (ind == id_of_data)
                {
                    m_dbConnection.Close();
                    return true;
                }
            }

            m_dbConnection.Close();
            return false;
        }

        //int ind(SQLiteConnection m_dbConnection)
        //{
        //    int index = 0;
        //    string sql = "SELECT id FROM id_name ORDER BY id DESC LIMIT 1";
        //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
        //    SQLiteDataReader reader = command.ExecuteReader();
        //    while (reader.Read())
        //        index = int.Parse(reader["id"].ToString());
        //    return (index + 1);
        //}
    }
}
