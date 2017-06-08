using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SImple_Serial
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            getAvailiblePorts();
            if (serialPort1.IsOpen == true)
            {
                try
                {
                    textBoxRecieve.Text = serialPort1.ReadLine();

                }
                catch (TimeoutException)
                {
                    textBoxRecieve.Text = "Timeout Error";
                }
                textBoxRecieve.Update();
            }

        }
        void getAvailiblePorts()
        {
            comboBoxSelectPort.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            comboBoxSelectPort.Items.AddRange(ports);
        }

        String readSerialPort(SerialPort Port)
        {
            String data = null;
            try
            {
                data = Port.ReadExisting();
                //data = Port.ReadLine();
                if (data == null) {
                    data = "nothing\r\n";
                } 
               
            }
            catch (TimeoutException)
            {
                data = "Timeout Error";
            }
            return data;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            getAvailiblePorts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxSelectPort.Text == "")
                {
                    textBoxRecieve.Text = "Select a Port Dumbass!";
                }
                else
                {
                    serialPort1.PortName = comboBoxSelectPort.Text;
                    serialPort1.BaudRate = 9600;
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Handshake = Handshake.None;
                    serialPort1.Encoding = System.Text.Encoding.Default;
                    serialPort1.ReadTimeout = 10000;
                    serialPort1.Open();
                    progressBarStatus.Value = 100;
                    buttonSend.Enabled = true;
                    buttonRecieve.Enabled = true;
                    textBoxSend.Enabled = true;
                    buttonStart.Enabled = false;
                    buttonStop.Enabled = true;
                    comboBoxSelectPort.Enabled = false;

                    timer1.Enabled = true;
                            
                    textBoxRecieve.Text = textBoxRecieve.Text + readSerialPort(serialPort1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                textBoxRecieve.Text = "ID.10.T Error. Something Fucked up. Is the port already in use?";
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            timer1.Enabled = false;
            buttonStop.Enabled = false;
            buttonStart.Enabled = true;
            textBoxSend.Enabled = false;
            buttonSend.Enabled = false;
            buttonRecieve.Enabled = false;
            progressBarStatus.Value = 0;
            comboBoxSelectPort.Enabled = true;
            getAvailiblePorts();
            textBoxRecieve.Text = "";
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine(textBoxSend.Text);
            textBoxSend.Text = "";

        }

        private void buttonRecieve_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxRecieve.Text = serialPort1.ReadLine();

            }
            catch(TimeoutException)
            {
                textBoxRecieve.Text = "Timeout Error";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String recievedData = "";
            recievedData = readSerialPort(serialPort1);
            textBoxRecieve.Text += recievedData;
            SendKeys.Send(recievedData);
        }
    }
}
