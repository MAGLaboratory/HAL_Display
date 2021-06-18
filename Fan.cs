using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                                if (this.checkBoxFanControlOnOff.Checked)
                                {
                                    this.checkBoxFanControlOnOff.Text = "Turn Off";
                                }

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
    }
}
