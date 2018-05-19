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

namespace Clients
{
    public partial class Frm : Form
    {
        Socket Client;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, 1592);
        Thread Thread = null;
        byte[] data = new byte[1024 * 50];
        AES256 AES = new AES256();
        MD5 MD5 = new MD5();
        SHA256 mySHA256 = SHA256Managed.Create();
        string key = "123456789";
        string stringPublicKeyA;
        string stringPublicKeyB;
        DiffieHellman PublicKeyB;
        public Frm()
        {
            InitializeComponent();
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
                    Client.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Client);

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
                    Client.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Client);
                    txtText.Clear();
                }
            }
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Client.BeginConnect(ipep, new AsyncCallback(ConnectServer), Client);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void ConnectServer(IAsyncResult i)
        {
            Client = ((Socket)i.AsyncState);
            Client.EndConnect(i);
            Thread = new Thread(new ThreadStart(Receive));
            Thread.Start();
        }

        private void SendData(IAsyncResult i)
        {
            Client = (Socket)i.AsyncState;
            int send = Client.EndSend(i);
        }
        void Receive()
        {
            while (true)
            {
                if (Client.Poll(1000000, SelectMode.SelectRead))
                {
                    Client.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceiveData), Client);
                }
            }
        }

        private void ReceiveData(IAsyncResult i)
        {
            Client = (Socket)i.AsyncState;
            int rec = Client.EndReceive(i);
            string messages = Encoding.ASCII.GetString(data, 0, rec);
            string[] arrMessage = messages.Split(';');
            string encryptMessage = arrMessage[0];
            string iv = arrMessage[1];
            string md5MessageChanged = arrMessage[2];
            int paddingLength = Convert.ToInt32(arrMessage[3]);
            //string stringPublicKeyA = arrMessage[4];
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




            //DiffieHellman PublicKeyB = new DiffieHellman(256).GenerateResponse(stringPublicKeyA);
            //stringPublicKeyB = PublicKeyB.ToString();
            //txtKey.Text = Convert.ToBase64String(PublicKeyB.Key);

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
            Client.Close();
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
                    Client.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Client);

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
                    Client.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Client);
                    txtText.Clear();
                }
            }
        }

        private void btnSendKey_Click(object sender, EventArgs e)
        {
            //byte[] sendText = new byte[1024 * 50];

            //string sendTextF = " " + ";" + " " + ";" + " " + ";" + " " + ";" + stringPublicKeyB;
            //sendText = Encoding.UTF8.GetBytes(sendTextF);
            //Client.BeginSend(sendText, 0, sendText.Length, SocketFlags.None, new AsyncCallback(SendData), Client);
        }
    }
}
