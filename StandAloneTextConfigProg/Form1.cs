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
using System.IO;
using System.Management;
using System.Threading;

namespace StandAloneTextConfigProg
{
    public partial class Form1 : Form
    {
        bool comportSet=false;
        List<COMPortInfo> ComPortInformation = new List<COMPortInfo>();

        public Form1()
        {


            InitializeComponent();
            // Get a list of serial port names. 
            SetupCOMPortInformation();

            // Display each port name to the console. 
            foreach (COMPortInfo port in ComPortInformation)
            {
                comboBox1.Items.Add(port.friendlyName);
            }
            comboBox1.SelectedIndex = comboBox1.FindString("prolific");

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void SetupCOMPortInformation()
        {
            String[] portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String s in portNames)
            {
                // s is like "COM14"
                COMPortInfo ci = new COMPortInfo();
                ci.portName = s;
                ci.friendlyName = s;
                ComPortInformation.Add(ci);
            }

            String[] usbDevs = GetUSBCOMDevices();
            foreach (String s in usbDevs)
            {
                // Name will be like "USB Bridge (COM14)"
                int start = s.IndexOf("(COM") + 1;
                if (start >= 0)
                {
                    int end = s.IndexOf(")", start + 3);
                    if (end >= 0)
                    {
                        // cname is like "COM14"
                        String cname = s.Substring(start, end - start);
                        for (int i = 0; i < ComPortInformation.Count; i++)
                        {
                            if (ComPortInformation[i].portName == cname)
                            {
                                ComPortInformation[i].friendlyName = s;
                            }
                        }
                    }
                }
            }
        }

        static string[] GetUSBCOMDevices()
        {
            List<string> list = new List<string>();

            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            foreach (ManagementObject mo2 in searcher2.Get())
            {
                string name = mo2["Name"].ToString();
                // Name will have a substring like "(COM12)" in it.
                if (name.Contains("(COM"))
                {
                    list.Add(name);
                }
            }
            // remove duplicates, sort alphabetically and convert to array
            string[] usbDevices = list.Distinct().OrderBy(s => s).ToArray();
            return usbDevices;
        }


        public class COMPortInfo
        {
            public String portName;
            public String friendlyName;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.ReadTimeout = 3000;
                serialPort1.PortName = ComPortInformation[comboBox1.SelectedIndex].portName;
                serialPort1.BaudRate = 19200;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;
                serialPort1.Parity = Parity.None;
                comportSet = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button10.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                button4.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                try
                {
                    serialPort1.ReadExisting();
                }
                catch(Exception ex) { ;}
                
                serialPort1.Write("d");
                serialPort1.ReadLine().Trim();
                textBox3.Text = serialPort1.ReadLine().Trim();
                textBox4.Text = serialPort1.ReadLine().Trim();
                textBox2.Text = serialPort1.ReadLine().Trim();
                textBox1.Text = serialPort1.ReadLine().Trim();
                listBox1.Items.Clear();
                foreach(string s in serialPort1.ReadLine().Split(','))
                {
                    listBox1.Items.Add(s);
                }
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //add in the entry, but make sure something has been entered into the text box...
            if (textBox6.Text != "")
            {
                listBox1.Items.Add(textBox6.Text);
                textBox6.Text = "";
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("T");

                int i = 1;
                foreach (var listBoxItem in listBox1.Items)
                {
                    serialPort1.Write(listBoxItem.ToString().Trim());
                    if (i < listBox1.Items.Count)
                    {
                        serialPort1.Write(",");
                    }
                    i++;
                }

                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("S");
                serialPort1.Write(textBox1.Text);
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("p");
                serialPort1.Write(textBox2.Text);
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("U");
                serialPort1.Write(textBox3.Text);
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("P");
                serialPort1.Write(textBox4.Text);
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                //turn it all back on again
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
        }

        // Send a test message
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.ReadTimeout = 15000;
                serialPort1.Open();
                serialPort1.Write("E");
                serialPort1.ReadTo("done!!123");
                serialPort1.Close();
                MessageBox.Show("Message sent!");
                serialPort1.ReadTimeout = 3000;

                // now turn it back on...
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("The message wasn't sent!" + System.Environment.NewLine + "Check all of your settings and your internet connection");
                serialPort1.Close();
                serialPort1.ReadTimeout = 3000;

                // now turn it back on...
                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }
        }

        //Update All
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    serialPort1.ReadExisting();
                }
                catch (Exception ex) { ;}

                // disable everything
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button12.Enabled = false;

                serialPort1.Open();
                serialPort1.Write("T");

                int i = 1;
                foreach (var listBoxItem in listBox1.Items)
                {
                    serialPort1.Write(listBoxItem.ToString().Trim());
                    if (i < listBox1.Items.Count)
                    {
                        serialPort1.Write(",");
                    }
                    i++;
                }
                serialPort1.ReadTo("done!!123");
                Thread.Sleep(100);

                serialPort1.Write("S");
                serialPort1.Write(textBox1.Text);
                serialPort1.ReadTo("done!!123");
                Thread.Sleep(100);

                serialPort1.Write("p");
                serialPort1.Write(textBox2.Text);
                serialPort1.ReadTo("done!!123");
                Thread.Sleep(100);

                serialPort1.Write("U");
                serialPort1.Write(textBox3.Text);
                serialPort1.ReadTo("done!!123");
                Thread.Sleep(100);

                serialPort1.Write("P");
                serialPort1.Write(textBox4.Text);
                serialPort1.ReadTo("done!!123");
                Thread.Sleep(100);

                serialPort1.Close();

                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                serialPort1.Close();

                if (comportSet)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button5.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                button10.Enabled = true;
            }

        }





    }
}
