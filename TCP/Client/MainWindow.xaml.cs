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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //номер порта для обмена сообщениями
        int port = 8888;
        //ip адрес сервера
        string address = "127.0.0.1";
        //объявление TCP клиента
        TcpClient client = null;
        //объявление канала соединения с сервером
        NetworkStream stream = null;
        //имя пользователя
        string username = "";

        DateTime lastPing;
        TimeSpan difDate = new TimeSpan(0, 0, 0, 0, 3100);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            if (stream != null)
            {
                //получение сообщения
                string message = msg.Text;
                //добавление имени пользователя к сообщению
                message = String.Format("{0}: {1}", username, message);
                //преобразование сообщение в массив байтов
                byte[] data = Encoding.Unicode.GetBytes(message);
                //отправка сообщения
                stream.Write(data, 0, data.Length);
            }
        }

        private void connect_btn_Click(object sender, RoutedEventArgs e)
        {
            //получение имени пользователя
            username = name.Text;
            try //если возникнет ошибка - переход в catch
            {
                //создание клиента
                client = new TcpClient(address, port);
                //получение канала для обмена сообщениями
                stream = client.GetStream();

                //создание нового потока для ожидания сообщения от сервера
                Thread listenThread = new Thread(() => listen());
                listenThread.Start();
            }
            catch (Exception ex)
            {
                log_client.Items.Add(ex.Message);
            }

        }
        //функция ожидания сообщений от сервера
        void listen()
        {
            try //в случае возникновения ошибки - переход к catch
            {
                lastPing = DateTime.Now;

                //цикл ожидания сообщениями
                while (true)
                {
                    //if ((DateTime.Now - lastPing) < difDate)
                    //{
                        //буфер для получаемых данных
                        byte[] data = new byte[64];
                        //объект для построения смтрок
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        //до тех пор, пока есть данные в потоке
                        do
                        {
                            //получение 64 байт
                            bytes = stream.Read(data, 0, data.Length);
                            //формирование строки
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);
                        //получить строку
                        string message = builder.ToString();
                        //вывод сообщения в лог клиента
                        if (message == "/close")
                        {
                            Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(message)));
                            break;
                        }
                        else if (message == "/ping")
                        {
                            Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(message)));
                            lastPing = DateTime.Now;
                            send_msg("/pong");

                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(message)));
                        }
                    //}
                    //else
                    //{
                    //    Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add("Сервер не отвечает более 3 секунд")));
                    //}
                }
            }
            catch (Exception ex)
            {
                //вывести сообщение об ошибке
                Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(ex.Message)));

            }
            finally
            {
                //закрыть канал связи и завершить работу клиента
                stream.Close();
                client.Close();
                Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add("end")));
            }
        }

        private void disconnect_btn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add("Отключение...")));
            send_msg("/bye");
            stream.Close();
            client.Close();
            Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add("Отключено")));
        }

        void send_msg(string ms)
        {
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];// буфер для получаемых данных
                string message = ms;
                Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(message)));
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

            }
            catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
            {
                Dispatcher.BeginInvoke(new Action(() => log_client.Items.Add(ex.Message)));
            }

        }

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
    }
}
