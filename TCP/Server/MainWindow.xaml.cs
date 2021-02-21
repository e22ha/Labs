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
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //прослушиваемый порт
        int port = 8888;
        //объект, прослушивающий порт
        static TcpListener listener;



        List<NetworkStream> users = new List<NetworkStream>();
        List<TcpClient> clients = new List<TcpClient>();

        public MainWindow()
        {
            InitializeComponent();
        }

        //функция ожидания и приёма запросов на подключение
        void listen()
        {
            Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Сервер запущен.")));
            //цикл подключения клиентов
            try
            {
                while (true)
                {
                    //принятие запроса на подключение
                    TcpClient client = listener.AcceptTcpClient();

                    Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Новый клиент подключен.")));

                    //создание нового потока для обслуживания нового клиента
                    Thread clientThread = new Thread(() => Process(client));
                    clientThread.Start();

                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }
        }

        //обработка сообщений от клиента
        public void Process(TcpClient tcpClient)
        {
            TcpClient client = tcpClient;
            NetworkStream stream = null; //получение канала связи с клиентом

            try //означает что в случае возникновении ошибки, управление перейдёт к блоку catch
            {
                //получение потока для обмена сообщениями
                stream = client.GetStream(); //получение канала связи с клиентом

                byte[] data = new byte[64];// буфер для получаемых данных

                users.Add(stream);
                clients.Add(client);

                //цикл ожидания и отправки сообщений
                while (true)
                {
                    //==========================получение сообщения============================
                    //объект, для формирования строк
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    //до тех пор, пока в потоке есть данные
                    do
                    {
                        //из потока считываются 64 байта и записываются в data начиная с 0
                        bytes = stream.Read(data, 0, data.Length);
                        //из считанных данных формируется строка
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    //преобразование сообщения
                    string message = builder.ToString();
                    if (message == "/bye")
                    {
                        foreach (NetworkStream ns in users)
                        {
                            if (ns == stream)
                            {
                                users.Remove(ns);
                            }
                        }
                        foreach (TcpClient cl in clients)
                        {
                            if (cl == client)
                            {
                                clients.Remove(cl);
                            }
                        }
                        stream.Close();
                        client.Close();
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));
                        data = Encoding.Unicode.GetBytes(message);
                        foreach (NetworkStream ns in users)
                        {
                            //отправка сообщения обратно клиенту
                            ns.Write(data, 0, data.Length);
                        }
                    }
                }
            }
            catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
            {
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }
            finally //после выхода из бесконечного цикла
            {
                //освобождение ресурсов при завершении сеанса
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void start_server_btn_Click(object sender, RoutedEventArgs e)
        {
            //создание объекта для отслеживания сообщений переданных с ip адреса через порт
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            //начало прослушивания
            listener.Start();

            //создание нового потока для ожидания и подключения клиентов
            Thread listenThread = new Thread(() => listen());
            listenThread.Start();
        }

        private void stop_server_btn_Click(object sender, RoutedEventArgs e)
        {
            send_msg("/close");
            //тогда закрываем подключения и очищаем список
            listener.Stop();
            try
            {
                foreach (NetworkStream ns in users)
                {
                    ns.Close();
                    users.Remove(ns);
                }
                foreach (TcpClient tcpc in clients)
                {
                    clients.Remove(tcpc);
                    tcpc.Close();
                }

            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }

        }

        void send_msg(string ms)
        {

            foreach (NetworkStream ns in users)
            {

                try
                {
                    byte[] data = new byte[64];// буфер для получаемых данных

                    string message = ms;
                    Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));
                    data = Encoding.Unicode.GetBytes(message);
                    ns.Write(data, 0, data.Length);

                }
                catch (Exception ex) //если возникла ошибка, вывести сообщение об ошибке
                {
                    Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
                }

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
