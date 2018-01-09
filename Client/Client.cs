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
using Message;

namespace Client
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        IPEndPoint ipEndPoint;
        Socket socket;
        MessageStruct messageStruct;

        private void BtnSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        void Connection()
        {
            try
            {
                ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                if (socket.Connected == false)
                {

                    socket.Connect(ipEndPoint);
                }
                Thread thread = new Thread(Receive);
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
            socket.Send(Serialize("Client||Close"));
            socket.Close();
        }

        void Send()
        {
            if (txtMess.Text.Trim().Length != 0)
            {
                messageStruct = new MessageStruct(txtMess.Text, "", "Client");
                socket.Send(Serialize(messageStruct));
                txtMess.Clear();
                rtbShowMessage.AppendText($"You : {messageStruct.Contents} \n");
            }
        }

        void Receive()
        {

            try
            {

                while (true)
                {
                    byte[] arrayByte = new byte[1024];
                    socket.Receive(arrayByte);
                    messageStruct = (MessageStruct)Deserialize(arrayByte);
                    if (messageStruct != null)
                    {
                        rtbShowMessage.AppendText($" {messageStruct.Sender} : {messageStruct.Contents} \n");
                    }
                }

            }
            catch (Exception ex)
            {
                // SocketClose();
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {

            CheckForIllegalCrossThreadCalls = false;
            Connection();
            Thread threadStatus = new Thread(() =>
            {
                try
                {
                    socket.Send(Serialize("Client"));
                }
                catch (Exception ex)
                {
                    socket.Close();
                    MessageBox.Show($"{ex.Message}"); ;
                }
            });
            threadStatus.IsBackground = true;
            threadStatus.Start();
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketClose();
        }
    }
}
