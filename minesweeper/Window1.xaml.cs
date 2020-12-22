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

namespace minesweeper
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<string> records = new List<string>();

        public Window1()
        {
            InitializeComponent();

            string line;
            string fullPath = System.IO.Path.GetFullPath(@"data.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(fullPath);
            while ((line = file.ReadLine()) != null)
            {
                string name = line;
                if ((line = file.ReadLine()) == null) break;
                int sec = int.Parse(line);
                if ((line = file.ReadLine()) == null) break;
                string mode = line;

                records.Add(name + "/" + sec + "/" + mode);

                rlist.Items.Add(name + "/" + sec + "/" + mode);
            }

            file.Close();
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(@"data.txt");

            using (outputFile)
            {

                foreach (string kvp in records)
                {
                    int i = 0;
                    char mrk = '/';
                    string name = "";
                    string sec ="";
                    string mode = "";
                    while (kvp[i] != mrk)
                    {
                        name += kvp[i];
                        i++;
                    }
                    i++;
                    outputFile.WriteLine(name);
                    while (kvp[i] != mrk)
                    {
                        sec += kvp[i];
                        i++;
                    }
                    i++;
                    outputFile.WriteLine(sec);
                    while (i < kvp.Length)
                    {
                        mode += kvp[i];
                        i++;
                    }
                    i++;
                    outputFile.WriteLine();

                }
            }
            outputFile.Close();
            this.Close();
        }
    }
}
