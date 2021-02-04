using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        public SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\Users\\Глеб\\Source\\Repos\\SergeyTy\\LABs_2\\WpfApp1\\dataBase.sqlite;Version=3;");

        public Editor()
        {
            InitializeComponent();
        }

        private void done_Click(object sender, MouseButtonEventArgs e)
        {
            m_dbConnection.Open();
            int ind = int.Parse(id_tb.Text);
            string name = name_tb.Text;
            string sql = "UPDATE id_name SET fio = '" + name + "' WHERE id = " + id_tb.Text + " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            string sql1 = "UPDATE id_mark SET mark_physics = '" + physics_tb.Text + "', mark_math ='" + math_tb.Text + "' WHERE id = " + id_tb.Text + " ";
            SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);

            //извлечение запроса
            command.ExecuteNonQuery();
            command1.ExecuteNonQuery();

            m_dbConnection.Close();
            this.DialogResult = true;
            this.Close();
        }

        private void del_Click(object sender, MouseButtonEventArgs e)
        {
            m_dbConnection.Open();
            string sql = "DELETE FROM id_name WHERE id = " + id_tb.Text + " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            //извлечение запроса
            command.ExecuteNonQuery();
            string sql1 = "DELETE FROM id_mark WHERE id = " + id_tb.Text + " ";
            SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
            //извлечение запроса
            command1.ExecuteNonQuery();

            m_dbConnection.Close();
            this.DialogResult = true;
            this.Close();
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
