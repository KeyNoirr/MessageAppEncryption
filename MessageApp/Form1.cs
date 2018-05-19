using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageApp
{
    public partial class Frm : Form
    {
        Socket Server;
        Socket Clients;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 1592);
        byte[] data = new byte[1024 * 50];
        Thread Thread = null;
        AES256 AES = new AES256();
        MD5 MD5 = new MD5();
        SHA256 mySHA256 = SHA256Managed.Create();
        string key = "123456789";
        string stringPublicKeyA;
        string stringPublicKeyB;
        DiffieHellman PublicKeyA;

        public Frm()
        {
            InitializeComponent();
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(ipep);
            Server.Listen(4);
            Server.BeginAccept(new AsyncCallback(AcceptClient), Server);
        }

        private void AcceptClient(IAsyncResult i)
        {
            Clients = ((Socket)i.AsyncState).EndAccept(i);
            Thread = new Thread(new ThreadStart(Receive));
            Thread.Start();
        }

        void Receive()
        {
            while (true)
            {
                if (Clients.Poll(1000000, SelectMode.SelectRead))
                {
                    Clients.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceiveData), Clients);
                }
            }
        }

        private void ReceiveData(IAsyncResult i)
        {
            Clients = (Socket)i.AsyncState;
            int rec = Clients.EndReceive(i);
            string messages = Encoding.ASCII.GetString(data, 0, rec);
            string[] arrMessage = messages.Split(';');
            string encryptMessage = arrMessage[0];
            string iv = arrMessage[1];
            string md5MessageChanged = arrMessage[2];
            int paddingLength = Convert.ToInt32(arrMessage[3]);
            //string stringPublicKeyB = arrMessage[4];
            byte[] bytes = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(iv));
            byte[] ivB = new byte[16];
            Array.Copy(bytes, 16, ivB, 0, 16);
            byte[] keyB = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(key));
            string rawMessage = AES.DecryptString(encryptMessage, keyB, ivB);
            int length = rawMessage.Length;
            int text = length - paddingLength;

            string str1 = rawMessage.Substring(0, text);


            //string[] arrRawMessage = rawMessage.Split(';');
            //string rawArrRawMessage = arrRawMessage[0];


            //PublicKeyA.HandleResponse(stringPublicKeyB);
            //txtKey.Text = Convert.ToBase64String(PublicKeyA.Key);

            string md5EncryptMessage = MD5.EncryptMD5(encryptMessage);
            if (md5EncryptMessage != md5MessageChanged)
            {
                richTextBoxMessage.Invoke((MethodInvoker)delegate ()
                {
                    richTextBoxMessage.Text += "\nTin nhắn có thể đã bị thây đổi";
                }
            );
            }
            else
            {
                richTextBoxMessage.Invoke((MethodInvoker)delegate ()
                {
                    richTextBoxMessage.Text += "\nClient: " + str1;
                }
            );
            }
        }

        private void SendData(IAsyncResult i)
        {
            Clients = (Socket)i.AsyncState;
            int send = Clients.EndSend(i);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtText.Text == "")
            {
                MessageBox.Show("Chưa nhập tin nhắn");
            }
            else
            {
                byte[] bytes = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(getTimeIV()));
                byte[] ivB = new byte[16];
                Array.Copy(bytes, 16, ivB, 0, 16);

                byte[] keyB = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(key));


                int n = txtText.Text.Length;
                if (n % 16 != 0)
                {
                    int mod = n % 16;
                    string padding = MD5.EncryptMD5(getTimeIV()).Remove(16 - mod);
                    string messagePadding = txtText.Text + padding;
                    int paddingLength = padding.Length;
                    string encryptMessage = AES.EncryptString(messagePadding, keyB, ivB);

                    txtHashNoise.Text = encryptMessage;
                    string md5EncryptMessage = MD5.EncryptMD5(encryptMessage);

                    byte[] sendText = new byte[1024 * 50];
                    richTextBoxMessage.Text += "\nMe: " + txtText.Text;
                    string sendTextF = encryptMessage + ";" + getTimeIV() + ";" + md5EncryptMessage + ";" + paddingLength;
                    sendText = Encoding.UTF8.GetBytes(sendTextF);
                    Clients.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Clients);

                    txtText.Clear();

                }
                else
                {
                    byte[] sendText = new byte[1024 * 50];
                    string encryptMessage = AES.EncryptString(txtText.Text, keyB, ivB);
                    txtHashNoise.Text = encryptMessage;
                    string md5EncryptMessage = MD5.EncryptMD5(encryptMessage);
                    richTextBoxMessage.Text += "\nMe: " + txtText.Text;
                    int paddingLength = 0;
                    string sendTextF = encryptMessage + ";" + getTimeIV() + ";" + md5EncryptMessage + ";" + paddingLength;
                    sendText = Encoding.UTF8.GetBytes(sendTextF);
                    Clients.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Clients);
                    txtText.Clear();
                }
            }
        }


        public static string RandomChar()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            Random rnd = new Random();
            int i = rnd.Next(chars.Length);
            return chars[i].ToString();
        }

        public string getTimeIV()
        {
            DateTime myTime = DateTime.Now;// ToString("HH:mm:ss");
            string timeFormat = myTime.ToString("HH:mm:ss");
            return timeFormat;
        }

        void Close()
        {
            Server.Close();
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void btnSendNoir_Click(object sender, EventArgs e)
        {
            if (txtText.Text == "")
            {
                MessageBox.Show("Chưa nhập tin nhắn");
            }
            else
            {
                byte[] bytes = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(getTimeIV()));
                byte[] ivB = new byte[16];
                Array.Copy(bytes, 16, ivB, 0, 16);

                byte[] keyB = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(key));


                int n = txtText.Text.Length;
                if (n % 16 != 0)
                {
                    int mod = n % 16;
                    string padding = MD5.EncryptMD5(getTimeIV()).Remove(16 - mod);
                    string messagePadding = txtText.Text + padding;
                    int paddingLength = padding.Length;


                    string encryptMessage = AES.EncryptString(messagePadding, keyB, ivB);

                    txtHashNoise.Text = encryptMessage;


                    byte[] sendText = new byte[1024 * 50];


                    int vt = encryptMessage.Length;
                    Random rnd = new Random();
                    int randomNoir = rnd.Next(0, vt + 1);
                    string str1 = encryptMessage.Substring(0, randomNoir);
                    string str2 = encryptMessage.Substring(randomNoir);
                    string messageChanged = str1 + RandomChar() + str2;



                    string md5MessageChanged = MD5.EncryptMD5(messageChanged);

                    richTextBoxMessage.Text += "\nMe: " + txtText.Text;
                    string sendTextF = encryptMessage + ";" + getTimeIV() + ";" + md5MessageChanged + ";" + paddingLength;
                    sendText = Encoding.UTF8.GetBytes(sendTextF);
                    Clients.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Clients);

                    txtText.Clear();
                }
                else
                {
                    int paddingLength = 0;
                    byte[] sendText = new byte[1024 * 50];
                    string encryptMessage = AES.EncryptString(txtText.Text, keyB, ivB);

                    txtHashNoise.Text = encryptMessage;

                    int vt = encryptMessage.Length;
                    Random rnd = new Random();
                    int randomNoir = rnd.Next(0, vt + 1);
                    string str1 = encryptMessage.Substring(0, randomNoir);
                    string str2 = encryptMessage.Substring(randomNoir);
                    string messageChanged = str1 + RandomChar() + str2;
                    string md5MessageChanged = MD5.EncryptMD5(messageChanged);
                    richTextBoxMessage.Text += "\nMe: " + txtText.Text;
                    string sendTextF = encryptMessage + ";" + getTimeIV() + ";" + md5MessageChanged + ";" + paddingLength;
                    sendText = Encoding.UTF8.GetBytes(sendTextF);
                    Clients.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Clients);
                    txtText.Clear();
                }
            }
        }

        private void btnSendKey_Click_1(object sender, EventArgs e)
        {
            //byte[] sendText = new byte[1024 * 50];

            //PublicKeyA = new DiffieHellman(256).GenerateRequest();
            //stringPublicKeyA = PublicKeyA.ToString();

            //string sendTextF = " " + ";" + " " + ";" + " " + ";" + " " + ";" + stringPublicKeyA;
            //sendText = Encoding.UTF8.GetBytes(sendTextF);
            //Clients.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Clients);
        }
    }
}
