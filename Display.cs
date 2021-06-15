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

            synopticData = new SynopticData();
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
                FanHandler(obj);
            }
            string item = $">> Msg: {(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds} | {obj.ApplicationMessage.Topic} | {obj.ApplicationMessage.QualityOfServiceLevel} | {obj.ApplicationMessage.ConvertPayloadToString()}";
            Debug.WriteLine(item);

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
                checkBoxFanControlOnOff.Text = "Turning On";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("display/drive_run")
                    .WithPayload("true")
                    .Build();
                await this.mqttClient.PublishAsync(message);
                Debug.WriteLine(">> Turning fan on");
            }
            else
            {
                checkBoxFanControlOnOff.Text = "Turning Off";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("display/drive_run")
                    .WithPayload("false")
                    .Build();
                await this.mqttClient.PublishAsync(message);
                Debug.WriteLine(">> Turning fan off");
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
