using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        Fan fan;
        SynopticData synopticData;
        IManagedMqttClient mqttClient;

        public class Config
        {
            public string name;
            public string description;
            public int checkup_freq;
            public int checkup_buffer;
            public string mqtt_broker;
            public int mqtt_port;
            public int mqtt_timeout;
            public bool main_display;
        };
        Config config;

        public class Checkup
        {
            public string Privacy_Switch;
        }
        Checkup checkup;

        class Hal_Controller_Checkup
        {

        }

        class Daisy_Controller_Checkup
        {

        }


        public Display()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string jsonFile = @"HAL_Display.json";
            Debug.WriteLine("Initializing Display.");
            var jsonSerializer = new JsonSerializer();
            Debug.WriteLine(">> Trying file in current working directory: " + jsonFile);
            StreamReader sw;
            if (File.Exists(jsonFile))
            {
                sw = new StreamReader(jsonFile);
                var reader = new JsonTextReader(sw);
                config = jsonSerializer.Deserialize<Config>(reader);
                sw.Close();
            }
            else
            {
                jsonFile = appPath + Path.DirectorySeparatorChar + jsonFile;
                Debug.WriteLine("!  File not found  Trying application directory: " + jsonFile);
                 if (File.Exists(jsonFile))
                {
                    sw = new StreamReader(jsonFile);
                    var reader = new JsonTextReader(sw);
                    config = jsonSerializer.Deserialize<Config>(reader);
                    sw.Close();
                }
                else
                {
                    Close();
                }
            }


            checkup = new Checkup();

            checkup.Privacy_Switch = "0";

            InitializeComponent();
            // even though it is set to allow word wrap through the designer,
            // you have to set it again???
            diagnosticsTextBox.WordWrap = true;
            diagnosticsTextBox.ListenerEnabled = true;
            // double buffer the panel
            typeof(Panel).InvokeMember
            (
                "DoubleBuffered",
                (
                    BindingFlags.SetProperty | 
                    BindingFlags.Instance | 
                    BindingFlags.NonPublic
                ), 
                null, 
                synopticPanel, 
                new object[] { true }
            );
            // show all tabs to preload contents
            tabLog.Show();
            tabInfo.Show();
            tabOverride.Show();
            tabFan.Show();
            tabPageFanControlAdvanced.Show();
            tabPageFanControlBasic.Show();
            tabStatus.Show();
            Debug.WriteLine(">> " + config.name + " is my name.");
            Debug.WriteLine(">> Starting MQTT.");
            mqttEn();
            fan = new Fan();
            this.fan.boxes.Add("Motor_Power", this.textBoxFanInfoMotorPower);
            this.fan.boxes.Add("Motor_Voltage", this.textBoxFanInfoMotorVoltage);
            this.fan.boxes.Add("Motor_Current", this.textBoxFanInfoMotorCurrent);
            this.fan.boxes.Add("DC_Bus_V", this.textBoxFanInfoDCBusV);
            this.fan.boxes.Add("Drive_HS_Temp", this.textBoxFanInfoHSTemp);
            this.fan.boxes.Add("Drive_Internal_Temp", this.textBoxFanInfoInternalTemp);
            this.fan.boxes.Add("Drive_Error", this.textBoxFanControlFault);

            synopticData = new SynopticData();
        }

        private async void mqttEn()
        {
            Debug.WriteLine("Connecting to " + config.mqtt_broker);
            // Setup and start a managed MQTT client.
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(config.name)
                    .WithTcpServer(config.mqtt_broker, config.mqtt_port)
                    .Build())
                .Build();

            var logger = new MqttNetLogger();
            mqttClient = new MQTTnet.MqttFactory(logger).CreateManagedMqttClient();
            string[] subscribeTopics = new string[]
            {
                "reporter/+",
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
                // skip below info messages
                if (e.LogMessage.Level < MqttNetLogLevel.Info)
                {
                    return;
                }

                string trace;

                if (e.LogMessage.Level > MqttNetLogLevel.Info)
                {
                    trace = $"!! [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}";
                }
                else
                {
                    trace = $">> [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}";
                }
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

        async private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs obj)
        {
            if (obj.ApplicationMessage.Topic.StartsWith("fan/"))
            {
                FanMQTTHandler(obj);
                SynopticMQTTHandler(obj);
            }
            else if (obj.ApplicationMessage.Topic.StartsWith("haldor/"))
            {
                SynopticMQTTHandler(obj);
            }
            else if (obj.ApplicationMessage.Topic.StartsWith("daisy/"))
            {
                SynopticMQTTHandler(obj);
            }
            else if (obj.ApplicationMessage.Topic == "reporter/checkup_req")
            {
                Debug.WriteLine(">> Checkup Request Received");
                if (config.main_display)
                {
                    String jsonMessage = JsonConvert.SerializeObject(checkup);
                    var m_message = new MqttApplicationMessageBuilder()
                        .WithTopic("display/checkup")
                        .WithPayload(jsonMessage)
                        .Build();
                    await this.mqttClient.PublishAsync(m_message);
                }
            }
            else if (obj.ApplicationMessage.Topic.StartsWith("display/"))
            {
            }

            // the running message for the fan is omitted because there are too many
            if (obj.ApplicationMessage.Topic != "fan/running")
            {
                string item = $">> Msg: {(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds} | {obj.ApplicationMessage.Topic} | {obj.ApplicationMessage.QualityOfServiceLevel} | {obj.ApplicationMessage.ConvertPayloadToString()}";
                Debug.WriteLine(item);
            }

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
                // This is here in case the fan speed textbox is changed and not the trackbar
                // TODO
            }
        }

        private async void radioButtonOverridePrivacy_Changed(object sender, EventArgs e)
        {
            StringBuilder jsonMessage = new StringBuilder("{");
            char privacy_value = '0';
            if (radioButtonOverridePrivacyOn.Checked)
            {
                jsonMessage.Append("\"Privacy_Switch\":\"1\"");
                privacy_value = '1';
                checkup.Privacy_Switch = "1";
            }
            else
            {
                jsonMessage.Append("\"Privacy_Switch\":\"0\"");
                privacy_value = '0';
                checkup.Privacy_Switch = "0";
            }
            jsonMessage.Append("}");
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("display/event")
                .WithPayload(jsonMessage.ToString())
                .Build();
            await this.mqttClient.PublishAsync(message);
            Debug.WriteLine(">> Privacy Switch : " + privacy_value);
        }

        // stop the MQTT client when the display closes
        private async void Display_FormClosing(object sender, FormClosingEventArgs e)
        {
            await this.mqttClient.StopAsync();
            this.mqttClient = null;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            // TODO: slowly move things into this function
            SynopticTimerHandler();
        }
    }
}
