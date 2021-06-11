using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System.Diagnostics;
using MQTTnet.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;

namespace HAL_Display
{
    public partial class Display : Form
    {
        class Fan
        {
            public class Citem
            {
                public string str;
                public int last_update;
                public int val;

                public Citem(string str, int last_update, int val)
                {
                    this.str = str;
                    this.last_update = last_update;
                    this.val = val;
                }
            }

            public Dictionary<string, Citem> checkup;
            public Dictionary<string, TextBox> boxes;
            
            // latched receive values
            public object l_received; 

            public Fan()
            {
                this.checkup = new Dictionary<string, Citem>()
                {
                    {"Max_Speed", new Citem(null, 0, 60 ) },
                    {"Drive_Control", new Citem(null, 0, 0 ) },
                    {"Output_Frequency", new Citem(null, 0, 0 ) },
                    {"Motor_Current", new Citem(null, 0, 0 ) },
                    {"Motor_Power", new Citem(null, 0, 0 ) },
                    {"Motor_Voltage", new Citem(null, 0, 0 ) },
                    {"DC_Bus_V", new Citem(null, 0, 0 ) },
                    {"Drive_HS_Temp", new Citem(null, 0, 0 ) },
                    {"Drive_Internal_Temp", new Citem(null, 0, 0 ) },
                    {"Drive_Ready", new Citem(null, 0, 0 ) },
                    {"Drive_Tripped", new Citem(null, 0, 0 ) },
                    {"Drive_Running", new Citem(null, 0, 0 ) },
                    {"Drive_Error", new Citem(null, 0, 0 ) }
                };
            }
        }
        
        void updateFan()
        {
            //// Fan Info
            //this.textBoxFanInfoMotorPower.Text = this.fan.checkup["Motor_Power"].ToString();
            //this.textBoxFanInfoMotorVoltage.Text = this.fan.checkup["Motor_Voltage"].ToString();
            //this.textBoxFanInfoMotorCurrent.Text = this.fan.checkup["Motor_Current"].ToString();
            //this.textBoxFanInfoDCBusV.Text = this.fan.checkup["DC_Bus_V"].ToString();
            //this.textBoxFanInfoHSTemp.Text = this.fan.checkup["Drive_HS_Temp"].ToString();
            //this.textBoxFanInfoInternalTemp.Text = this.fan.checkup["Drive_Internal_Temp"].ToString();

            // Fan Speed

            // Fan Control
        }

        Fan fan;
        IManagedMqttClient mqttClient;

        class Hal_Controller_Checkup
        {

        }

        class Daisy_Controller_Checkup
        {

        }

        public Display()
        {
            InitializeComponent();
            mqttEn();
            this.fan.boxes.Add("Motor_Power", this.textBoxFanInfoMotorPower);
            this.fan.boxes.Add("Motor_Voltage", this.textBoxFanInfoMotorVoltage);
            this.fan.boxes.Add("Motor_Current", this.textBoxFanInfoMotorCurrent);
            this.fan.boxes.Add("DC_Bus_V", this.textBoxFanInfoDCBusV);
            this.fan.boxes.Add("Drive_HS_Temp", this.textBoxFanInfoHSTemp);
            this.fan.boxes.Add("Drive_Internal_Temp", this.textBoxFanInfoInternalTemp);
            // even though it is set to allow word wrap through the designer,
            // you have to set it again???
            diagnosticsTextBox.WordWrap = true;
        }

        private async void mqttEn()
        {
            // Setup and start a managed MQTT client.
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("HALDisplay")
                    .WithTcpServer("hal")
                    .Build())
                .Build();

            var logger = new MqttNetLogger();
            mqttClient = new MQTTnet.MqttFactory(logger).CreateManagedMqttClient();
            string[] subscribeTopics = new string[]
            {
                "reporter/+",
                "tweeter/+",
                "haldor/+",
                "daisy/+",
                "fan/+"
            };
            foreach (string topic in subscribeTopics)
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            }

            // Write all trace messages to the console window.
            logger.LogMessagePublished += (s, e) =>
            {
                // skip info messages
                if (e.LogMessage.Level < MqttNetLogLevel.Info)
                {
                    return;
                }

                var trace = $">> [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}";
                if (false && e.LogMessage.Exception != null)
                {
                    trace += Environment.NewLine + e.LogMessage.Exception.ToString();
                }

                Debug.WriteLine(trace);
            };

            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnSubscriberMessageReceived);
            await mqttClient.StartAsync(options);
        }

        private void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            // disable controls and invalidate data
        }

        private void OnConnected(MqttClientConnectedEventArgs obj)
        {
            // enable controls and validate data
        }

        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs obj)
        {
            if (obj.ApplicationMessage.Topic == "fan/checkup")
            {
                object received = JsonConvert.DeserializeObject(obj.ApplicationMessage.ConvertPayloadToString());

                //// parse out properties from the fan
                //PropertyInfo[] properties = this.fan.checkup.GetType().GetProperties();
                //foreach (PropertyInfo property in properties)
                //{
                //    string ts = (string)received.GetType().GetProperty(property.Name).GetValue(received, null);
                //    if (!string.IsNullOrEmpty(ts))
                //    {
                //        var converter = TypeDescriptor.GetConverter(property.GetType());
                //        property.SetValue(this.fan.checkup, converter.ConvertFrom(ts));
                //    }
                //}

                //// update the GUI
                //updateFan();

                //properties = received.GetType().GetProperties();
                //foreach (PropertyInfo property in properties)
                //{
                //    string ts = (string)received.GetType().GetProperty(property.Name).GetValue(received, null);
                //}
            }
            string item = $">> Msg: {(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds} | {obj.ApplicationMessage.Topic} | {obj.ApplicationMessage.QualityOfServiceLevel} | {obj.ApplicationMessage.ConvertPayloadToString()}";
            Debug.WriteLine(item);

        }

        private void synopticPanel_Paint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;

            g.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), p.DisplayRectangle);

            Point[] points = new Point[4];

            points[0] = new Point(0, 0);
            points[1] = new Point(0, p.Height);
            points[2] = new Point(p.Width, p.Height);
            points[3] = new Point(p.Width, 0);

            Brush brush = new SolidBrush(Color.DarkGreen);

            g.FillPolygon(brush, points);
        }

        private void display_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                if (this.WindowState != FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                }
            }
            if (e.KeyData == Keys.Enter && tabControl1.SelectedTab == this.tabFan)
            {
                // tab processing 
            }
        }

        private void trackBarFanSpeed_ValueChanged(object sender, EventArgs e)
        {
            // update the text box if the trackbar changed value
            textBoxFanSpeedTarget.Text = (trackBarFanSpeed.Value
                - ((trackBarFanSpeed.Maximum - trackBarFanSpeed.Minimum) / 2) * 2).ToString();
        }

        private void checkBoxFanControlOnOff_Click(object sender, EventArgs e)
        {
 
        }

        private async void checkBoxFanControlOnOff_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFanControlOnOff.Checked)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("display/drive_run")
                    .WithPayload("true")
                    .Build();
                await this.mqttClient.PublishAsync(message);
                Debug.WriteLine(">> Fan on");
            }
            else
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("display/drive_run")
                    .WithPayload("false")
                    .Build();
                await this.mqttClient.PublishAsync(message);
                Debug.WriteLine(">> Fan off");
            }
        }
    }
}
