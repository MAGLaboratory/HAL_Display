using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAL_Display
{
    public partial class Display
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

            public int fsLow = 24;
            public int fs1 = 34;
            public int fs2 = 42;
            public int fsHigh = 60;
            public int fsTurbo = 80;

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

        private void FanHandler(MqttApplicationMessageReceivedEventArgs obj)
        {
            dynamic received = JsonConvert.DeserializeObject(obj.ApplicationMessage.ConvertPayloadToString());

            // get time
            int dot_position = ((string)received.time).IndexOf(".");
            int update_time = int.Parse(((string)received.time).Substring(0, dot_position));
            foreach (dynamic entry in received)
            {
                string key = entry.Name;
                string value = entry.Value;

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
                                // change values if necessary
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
                        if (this.textBoxFanSpeedSet.Text == "N/A")
                        {
                            this.trackBarFanSpeed.Invoke(new MethodInvoker(delegate
                            {
                                int v = parsed_value / 20;
                                v += (trackBarFanSpeed.Maximum) / 2;
                                this.trackBarFanSpeed.Value = v;
                            }));
                        }

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
                                // handle as 0.0 and not .0
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
                            this.buttonFanControlReset.Invoke(new MethodInvoker(delegate
                            { 
                                this.buttonFanControlReset.BackColor = Color.LightCoral;
                                this.buttonFanControlReset.Enabled = true;
                            }));
                            
                        }
                        else
                        {
                            this.buttonFanControlReset.Invoke(new MethodInvoker(delegate 
                            { 
                                this.buttonFanControlReset.BackColor = Color.Transparent;
                                this.buttonFanControlReset.Enabled = false;
                            }));
                        }
                    }

                    if (key == "Drive_Running")
                    {
                        if (parsed_value == 1)
                        {
                            this.tabFan.Invoke(new MethodInvoker(delegate
                            {
                                if (this.checkBoxFanControlOnOff.Checked)
                                {
                                    this.checkBoxFanControlOnOff.Text = "Turn Off";
                                }

                                // tab image animation disabled
                                //if (this.tabFan.ImageIndex != 5)
                                //{
                                //    this.tabFan.ImageIndex = 5;
                                //}
                                //else
                                //{
                                //    this.tabFan.ImageIndex = 0;
                                //}
                            }));
                        }
                        else
                        {
                            this.checkBoxFanControlOnOff.Invoke(new MethodInvoker(delegate
                            {
                                if (this.checkBoxFanControlOnOff.Checked == false)
                                {
                                    this.checkBoxFanControlOnOff.Text = "Turn On";
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

        private async void buttonFanControlReset_Click(object sender, EventArgs e)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("display/drive_reset")
                .Build();
            await this.mqttClient.PublishAsync(message);
            Debug.WriteLine(">> Fan Trip Reset.");
        }

        private void buttonFanSpeedDec_Click(object sender, EventArgs e)
        {
            int v = trackBarFanSpeed.Value;
            v -= 1;
            if (v >= trackBarFanSpeed.Minimum)
            {
                trackBarFanSpeed.Value = v;
            }
            Debug.WriteLine(">> Fan Speed Dec: " + v.ToString());
        }

        private void buttonFanSpeedInc_Click(object sender, EventArgs e)
        {
            int v = trackBarFanSpeed.Value;
            v += 1;
            if (v <= trackBarFanSpeed.Maximum)
            {
                trackBarFanSpeed.Value = v;
            }
            Debug.WriteLine(">> Fan Speed Inc: " + v.ToString());
        }

        private void buttonFanSpeedDecPlus_Click(object sender, EventArgs e)
        {
            int v = trackBarFanSpeed.Value;
            v -= 5;
            if (v >= trackBarFanSpeed.Minimum)
            {
                trackBarFanSpeed.Value = v;
            }
            Debug.WriteLine(">> Fan Speed DecPlus: " + v.ToString());
        }

        private void buttonFanSpeedIncPlus_Click(object sender, EventArgs e)
        {
            int v = trackBarFanSpeed.Value;
            v += 5;
            if (v <= trackBarFanSpeed.Maximum)
            {
                trackBarFanSpeed.Value = v;
            }
            Debug.WriteLine(">> Fan Speed IncPlus: " + v.ToString());
        }

        private void radioButtonFanBasicSpeed_Click(object sender, EventArgs e)
        {
            int v = 0;
            StringBuilder debugString = new StringBuilder(">> ");
            bool err = false;
            if (radioButtonFanBasicSpeedOff.Checked)
            {
                v = 0;
                debugString.Append("Fan Set to Off ");
            }
            else if (radioButtonFanBasicSpeedLow.Checked)
            {
                v = this.fan.fsLow;
                debugString.Append("Fan Set to Low ");
            }
            else if (radioButtonFanBasicSpeed1.Checked)
            {
                v = this.fan.fs1;
                debugString.Append("Fan Set to One ");
            }
            else if (radioButtonFanBasicSpeed2.Checked)
            {
                v = this.fan.fs2;
                debugString.Append("Fan Set to Two ");
            }
            else if (radioButtonFanBasicSpeedHigh.Checked)
            {
                v = this.fan.fsHigh;
                debugString.Append("Fan Set to High ");
            }
            else if (radioButtonFanBasicSpeedTurbo.Checked)
            {
                v = this.fan.fsTurbo;
                debugString.Append("Fan Set to Turbo ");
            }
            else
            {
                // error condition where none of the radio buttons are checked???
                debugString.Append("No Fan Speed Set!");
                err = true;
            }

            if (radioButtonFanBasicDirectionForward.Checked)
            {
                debugString.Append(", Forward ");
            }
            else if (radioButtonFanBasicDirectionReverse.Checked)
            {
                debugString.Append(", Reverse ");
                v *= -1;
            }
            else
            {
                // error condition where none of the radio buttons are checked???
                debugString.Append(" , No Fan Direction Set!");
                err = true;
            }

            // calculate track bar fan speed
            if (err == false)
            {
                int iTrackbarSpeed = v;
                iTrackbarSpeed /= 2;
                iTrackbarSpeed += (trackBarFanSpeed.Maximum / 2);
                if (iTrackbarSpeed >= trackBarFanSpeed.Minimum && iTrackbarSpeed <= trackBarFanSpeed.Maximum)
                {
                    trackBarFanSpeed.Value = iTrackbarSpeed;
                }
                else
                {
                    err = true;
                    debugString.Append("Error: speed out-of-bounds.");
                }
            }

            if (err == false)
            {
                debugString.Append(v.ToString());
                Debug.WriteLine(debugString);
            }
            else
            {
                debugString.Append(v.ToString());
                debugString[0] = '!';
                debugString[1] = '!';
                Debug.WriteLine(debugString);
            }

            if (err == false)
            {
                this.buttonFanSpeedApply_Click(this, e);
            }

            // calculate run checked status
            if (err == false)
            {
                if (v != 0)
                {
                    checkBoxFanControlOnOff.Checked = true;
                }
                else
                {
                    checkBoxFanControlOnOff.Checked = false;
                }
            }
        }
    }
}
