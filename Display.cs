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
using System.Globalization;

namespace HAL_Display
{
    public partial class Display : Form
    {
        class Fan
        {
            // fixed point, ten more than hertz
            public int new_speed;

            // checkup item
            public class Citem
            {
                // null strings make it an integer target
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

            // last receive values
            public Dictionary<string, Citem> checkup;

            // text boxes to update with the values
            public Dictionary<string, TextBox> boxes;

            // last receive values
            public Dictionary<string, string> last;

            public Fan()
            {
                // initialize with all integer targets
                this.checkup = new Dictionary<string, Citem>()
                {
                    {"Max_Speed", new Citem(null, 0, 60 ) },
                    {"Set_Point", new Citem(null, 0, 0) },
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

                this.boxes = new Dictionary<string, TextBox>();

                this.last = new Dictionary<string, string>();
            }
        }

        class SynopticData
        {

        }

        Fan fan;
        SynopticData synopticData;
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
            fan = new Fan();
            this.fan.boxes.Add("Motor_Power", this.textBoxFanInfoMotorPower);
            this.fan.boxes.Add("Motor_Voltage", this.textBoxFanInfoMotorVoltage);
            this.fan.boxes.Add("Motor_Current", this.textBoxFanInfoMotorCurrent);
            this.fan.boxes.Add("DC_Bus_V", this.textBoxFanInfoDCBusV);
            this.fan.boxes.Add("Drive_HS_Temp", this.textBoxFanInfoHSTemp);
            this.fan.boxes.Add("Drive_Internal_Temp", this.textBoxFanInfoInternalTemp);
            this.fan.boxes.Add("Drive_Error", this.textBoxFanControlFault);
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
                    .WithTcpServer("localhost")
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
            if (obj.ApplicationMessage.Topic.StartsWith("fan/"))
            {
                dynamic received = JsonConvert.DeserializeObject(obj.ApplicationMessage.ConvertPayloadToString());
                int dot_position = ((string)received.time).IndexOf(".");
                int update_time = int.Parse(((string)received.time).Substring(0, dot_position));
                foreach (dynamic entry in received)
                {
                    string key = entry.Name;
                    string value = entry.Value;

                    // determine if it's an integer target

                    // TODO: error handling
                    if (this.fan.checkup.ContainsKey(key) && this.fan.checkup[key].str == null)
                    {
                        int parsed_value;
                        if (value.ToLower() == "true")
                        {
                            parsed_value = 1;
                        }
                        else if (value.ToLower() == "false")
                        {
                            parsed_value = 0;
                        }
                        else
                        {
                            parsed_value = int.Parse(value);
                        }
                        this.fan.checkup[key].val = parsed_value;
                        this.fan.checkup[key].last_update = update_time;
                        
                        if (this.fan.boxes.ContainsKey(key))
                        {
                            this.fan.boxes[key].Invoke(new MethodInvoker(delegate { this.fan.boxes[key].Text = value; }));
                        }
                        
                        if (key == "Max_Speed")
                        {
                            if (!this.fan.last.ContainsKey(key) || value != this.fan.last[key])
                            {
                                this.trackBarFanSpeed.Invoke(new MethodInvoker(delegate 
                                {
                                    int v = this.trackBarFanSpeed.Value;
                                    v -= this.trackBarFanSpeed.Maximum / 2;
                                    int target = v * 2;
                                    v += parsed_value / 20;
                                    if (v <= parsed_value / 10)
                                    {
                                        this.trackBarFanSpeed.Maximum = parsed_value / 10;
                                        this.trackBarFanSpeed.Value = v;
                                        this.trackBarFanSpeed.Enabled = true;
                                        this.textBoxFanSpeedSelected.Text = target.ToString();
                                        this.buttonFanSpeedApply.Enabled = true;
                                    }
                                    else
                                    {
                                        this.trackBarFanSpeed.Maximum = parsed_value / 10;
                                        this.trackBarFanSpeed.Value = parsed_value / 5;
                                        this.trackBarFanSpeed.Enabled = true;
                                        this.textBoxFanSpeedSelected.Text = "0";
                                        this.buttonFanSpeedApply.Enabled = true;
                                    }
                                    this.trackBarFanSpeed.Enabled = true;
                                }));
                            }
                        }

                        if (key == "Set_Point")
                        {
                            this.textBoxFanSpeedSelected.Invoke(new MethodInvoker(delegate
                            {
                                this.textBoxFanSpeedSet.Text = (parsed_value / 10).ToString();
                            }));
                        }

                        if (key == "Output_Frequency")
                        {
                            this.textBoxFanSpeedCurrent.Invoke(new MethodInvoker(delegate
                            {
                                int len = value.Length;
                                if (len < 2)
                                {
                                    this.textBoxFanSpeedCurrent.Text = value.Insert(len - 1, "0" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                                }
                                else
                                {
                                    this.textBoxFanSpeedCurrent.Text = value.Insert(len - 1, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                                }
                            }));
                        }

                        if (key == "Drive_Ready")
                        {
                            if (parsed_value == 1)
                            {
                                this.checkBoxFanControlOnOff.Invoke(new MethodInvoker(delegate { this.checkBoxFanControlOnOff.BackColor = Color.LightGreen; }));
                            }
                            else
                            {
                                this.checkBoxFanControlOnOff.Invoke(new MethodInvoker(delegate { this.checkBoxFanControlOnOff.BackColor = Color.Transparent; }));
                            }
                        }

                        if (key == "Drive_Tripped")
                        {
                            if (parsed_value == 1)
                            {
                                this.buttonFanControlReset.Invoke(new MethodInvoker(delegate { this.buttonFanControlReset.BackColor = Color.LightCoral; }));
                                this.buttonFanControlReset.Enabled = true;
                            }
                            else
                            {
                                this.buttonFanControlReset.Invoke(new MethodInvoker(delegate { this.buttonFanControlReset.BackColor = Color.Transparent; }));
                                this.buttonFanControlReset.Enabled = false;
                            }
                        }

                        if (key == "Drive_Running")
                        {
                            if (parsed_value == 1)
                            {
                                this.tabFan.Invoke(new MethodInvoker(delegate
                                {

                                    if (this.tabFan.ImageKey != "fan3.png")
                                    {
                                        this.tabFan.ImageKey = "fan3.png";
                                    }
                                    else
                                    {
                                        this.tabFan.ImageKey = "fan2.png";
                                    }
                                }));
                            }
                        }
                    }
                    else if (this.fan.checkup.ContainsKey(key))
                    {
                        this.fan.checkup[key].str = value;
                        this.fan.checkup[key].last_update = update_time;
                    }
                    else
                    {
                        this.fan.checkup.Add(key, new Fan.Citem(value, update_time, 0));
                    }
                    this.fan.last[key] = value;
                }
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
            int v = trackBarFanSpeed.Value;
            v -= (trackBarFanSpeed.Maximum) / 2;
            v *= 2;
            textBoxFanSpeedSelected.Text = v.ToString();
            this.fan.new_speed = v * 10;
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

        private async void buttonFanSpeedApply_Click(object sender, EventArgs e)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("display/drive_speed")
                .WithPayload(this.textBoxFanSpeedSelected.Text + "0")
                .Build();
            await this.mqttClient.PublishAsync(message);
            Debug.WriteLine(">> Fan Speed Application: " + this.textBoxFanSpeedSelected.Text + "0");
        }
    }
}
