using Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client2
{
    public partial class Server : Form
    {
        List<Socket> listSocketClient = null;
        IPEndPoint ipEndPoint = null;
        Socket socket = null;
        MessageStruct messageStruct = null;
        Thread connection = null;
        Thread receive = null;
        Thread thread = null;
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            connection = new Thread(() =>
            {
                //while (true)
                {
                    if (socket!=null&&socket.Connected)
                    {
                        //continue;
                    }
                    if (receive != null)
                    {

                        receive.Abort();
                    }
                    if (thread != null)
                    {

                        thread.Abort();
                    }
                    if (socket != null)
                    {

                        socket.Close();
                    }
                    Connection();
                }
            });
            connection.IsBackground = true;
            connection.Start();

        }

        bool IsSocketConnected(Socket s)
        {
            if (socket==null)
            {
                return false;
            }
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if ((part1 && part2) || !s.Connected)
                return false;
            else
                return true;

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            listSocketClient.ForEach(item =>
            {
                Send(item);

            });
            txtMess.Clear();
        }

        void Connection()
        {
            try
            {

                listSocketClient = new List<Socket>();
                ipEndPoint = new IPEndPoint(IPAddress.Any, 9999);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Bind(ipEndPoint);
                thread = new Thread(() =>
                {
                    try
                    {
                        while (true)
                        {

                        socket.Listen(100);
                        Socket socketClient = socket.Accept();
                        listSocketClient.Add(socketClient);
                        
                        receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(socketClient);
                        }
                    }
                    catch (Exception)
                    {
                        ipEndPoint = new IPEndPoint(IPAddress.Any, 9999);
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    }
                });

                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối!Mã lỗi \n {ex.Message}", "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SocketClose()
        {
            socket.Close();
        }

        void Send(Socket client)
        {
            if (txtMess.Text.Trim().Length != 0)
            {
                messageStruct = new MessageStruct(txtMess.Text, "", "Server");
                client.Send(Serialize(messageStruct));
                rtbShowMessage.AppendText($"You : {messageStruct.Contents}\n");
            }
        }

        void Receive(object obj)
        {
                Socket socketClient = obj as Socket;
            try
            {

            while (true)
            {
                
                if (socketClient.Available != 0)
                {
                    byte[] arrayByte = new byte[1024];
                    socketClient.Receive(arrayByte);
                    var dataReceive = Deserialize(arrayByte);
                    var checkData = dataReceive is String;
                    if (dataReceive != null && !checkData)
                    {
                        messageStruct = (MessageStruct)dataReceive;
                        rtbShowMessage.AppendText($"From : {messageStruct.Sender} : {messageStruct.Contents} \n");
                    }
                    else if (checkData)
                    {
                        bool status = true;
                        if (dataReceive.ToString().Contains("||Close"))
                        {
                            dataReceive = dataReceive.ToString().Substring(0, dataReceive.ToString().Count() - "||Close".Count());
                            status = false;
                        }
                        AddDataToListView(dataReceive as string, status);
                    }
                }
            }
            }
            catch (Exception)
            {
                listSocketClient.Remove(socketClient);
                socketClient.Close();
            }
            /*
        try
        {

        }
        catch (Exception ex)
        {
            //SocketClose();
            MessageBox.Show(ex.Message);
        }*/
        }
        void AddDataToListView(string name, bool status)
        {
            ListViewItem listViewItem = null;
            var exitsName = lvwUserStatus.Items.OfType<ListViewItem>().ToList().Where(p => p.Name == name);

            if (exitsName.Count() > 0 && status == false)
            {
                lvwUserStatus.Items.Remove(exitsName.FirstOrDefault());
            }
            if (exitsName.Count() == 0)
            {
                listViewItem = new ListViewItem() { Name = name, Text = name };
                listViewItem.SubItems.Add(status ? "true" : "false");
                lvwUserStatus.Items.Add(listViewItem);
            }
        }

        byte[] Serialize(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        object Deserialize(byte[] data)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(data);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                //SocketClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketClose();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            lvwUserStatus.Columns.Add(new ColumnHeader() { Text = "Name" });
            lvwUserStatus.Columns.Add(new ColumnHeader() { Text = "Status" });

        }
    }
}
