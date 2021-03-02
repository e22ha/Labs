using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;


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
        TimeSpan difDate = new TimeSpan(0, 0, 0, 0, 3100);

        bool ping = true;

        struct User
        {
            public DateTime lastPong;
            public TcpClient client;
            public NetworkStream stream;
            public bool availabel;

            public void set (bool st)
            {
                availabel = st;
            }
        }

        List<User> Users = new List<User>();

        //List<NetworkStream> users = new List<NetworkStream>();
        //List<TcpClient> clients = new List<TcpClient>();

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

                User u;
                u.client = client;
                u.stream = stream;
                u.lastPong = DateTime.Now;
                u.availabel = true;


                Users.Add(u);

                //цикл ожидания и отправки сообщений
                while (true)
                {
                    if ((u.availabel == true) & ((DateTime.Now - u.lastPong) < difDate))
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
                            Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));

                            foreach (User ur in Users)
                            {
                                if (ur.stream == stream)
                                {
                                    stream.Close();
                                    client.Close();
                                    u.availabel = false;
                                }
                            }
                        }
                        else if (message == "/pong")
                        {
                            u.lastPong = DateTime.Now;
                            Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));

                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));
                            data = Encoding.Unicode.GetBytes(message);
                            foreach (User us in Users)
                            {
                                if (us.availabel == true)
                                {
                                    us.stream.Write(data, 0, data.Length);
                                }
                                //отправка сообщения обратно клиенту
                            }
                        }
                    }
                    else
                    {
                        u.stream.Close();
                        u.client.Close();
                        u.availabel = false;
                        Dispatcher.BeginInvoke(new Action(() => log.Items.Add("client gone")));
                        break;

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
            ping = true;
            //создание объекта для отслеживания сообщений переданных с ip адреса через порт
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            //начало прослушивания
            listener.Start();

            //создание нового потока для ожидания и подключения клиентов
            Thread listenThread = new Thread(() => listen());
            listenThread.Start();
            Thread pingThread = new Thread(() => ping_pong());
            pingThread.Start();
        }

        private void stop_server_btn_Click(object sender, RoutedEventArgs e)
        {
            ping = false;
            send_msg("/close");
            listener.Stop();
            //тогда закрываем подключения и очищаем список
            try
            {
                foreach (User ur in Users)
                {
                    ur.set(false);
                    ur.stream.Close();
                    ur.client.Close();
                }
                Users.Clear();
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
            }
            Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Сервер остановлен.")));
        }

        void send_msg(string ms)
        {
            foreach (User u in Users)
            {
                if (u.availabel == true)
                {

                    NetworkStream ns = u.stream;
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
        }

        void ping_pong()
        {
            while (ping == true)
            {
                Thread msgThread = new Thread(() => send_msg("/ping"));
                msgThread.Start();
                Thread.Sleep(3000);
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
