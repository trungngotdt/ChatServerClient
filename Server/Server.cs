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
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connection();

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            listSocketClient.ForEach(item =>
            {
                Send(item);

            });
            txtMess.Clear();
        }

        List<Socket> listSocketClient;
        IPEndPoint ipEndPoint;
        Socket socket;
        MessageStruct messageStruct;
        void Connection()
        {
            try
            {
                listSocketClient = new List<Socket>();
                ipEndPoint = new IPEndPoint(IPAddress.Any, 9999);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Bind(ipEndPoint);
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        socket.Listen(100);
                        Socket socketClient = socket.Accept();
                        listSocketClient.Add(socketClient);

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(socketClient);
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
            while (true)
            {
                if (socketClient.Available != 0)
                {
                    byte[] arrayByte = new byte[1024];
                    socketClient.Receive(arrayByte);
                    var dataReceive= Deserialize(arrayByte);
                    var checkData = dataReceive is String;
                    if (dataReceive != null&&!checkData)
                    {
                        messageStruct = (MessageStruct)dataReceive;
                        rtbShowMessage.AppendText($"From : {messageStruct.Sender} : {messageStruct.Contents} \n");
                    }
                    else if(checkData)
                    {
                        bool status = true;
                        if (dataReceive.ToString().Contains("||Close"))
                        {
                            dataReceive= dataReceive.ToString().Substring(0, dataReceive.ToString().Count() - "||Close".Count());
                            status = false;
                        }
                        AddDataToListView(dataReceive as string, status);
                    }
                }
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
        void AddDataToListView(string name,bool status)
        {
            ListViewItem listViewItem=null;
            bool exitsName = lvwUserStatus.Items.OfType<ListViewItem>().ToList().Exists(p => p.Name == name);
            bool checkStatus = lvwUserStatus.Items.OfType<ListViewItem>().ToList().Exists(p =>  p.Name == name && p.SubItems[1].Text ==( status ? "true" : "false"));
            if (!checkStatus)
            {
                listViewItem = new ListViewItem() { Name = name,Text=name };
                listViewItem.SubItems.Add(status ? "true" : "false");

            }
            else if(checkStatus&&exitsName)
            {
                lvwUserStatus.Items.OfType<ListViewItem>().ToList().Where(p => p.Name == name).ToList()[0].SubItems[1].Text = status ? "true" : "false";
            }
            if (listViewItem!=null)
            {
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
            lvwUserStatus.Columns.Add(new ColumnHeader() { Text = "Status"});

        }
    }
}
