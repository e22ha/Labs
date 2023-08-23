﻿using System;
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
using System.Windows.Shapes;

namespace lab_timer
{
    /// <summary>
    /// Логика взаимодействия для AddTimer.xaml
    /// </summary>
    public partial class AddTimer : Window
    {
        public AddTimer()
        {
            InitializeComponent();
        }

        private void Done_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window error = new War();

            if (int.Parse(Hour.Text) > 24)
            {
                Hour.Text = "";
                Hour.BorderBrush = new SolidColorBrush(Colors.Red);
                error.ShowDialog();
            }
            else if (int.Parse(Min.Text) > 60)
            {
                Min.Text = "";
                Min.BorderBrush = new SolidColorBrush(Colors.Red);
                error.ShowDialog();
            }
            else if (int.Parse(Sec.Text) > 60)
            {
                Sec.Text = "";
                Sec.BorderBrush = new SolidColorBrush(Colors.Red);
                error.ShowDialog();
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
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
