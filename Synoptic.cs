using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MAGLabCL;

namespace HAL_Display
{
	public partial class Display
	{
        UInt16 time = 0;
        class SynopticData
        {
            public Dictionary<string, string> therm;
            // inverse logic: it's true if it's bad!
            public Dictionary<string, bool> daemon;
            public Dictionary<string, bool> door;
            public Dictionary<string, bool> motion;
            public string Space_Message;
            public bool Space_Open;
            public class OpenSwitch
            {
                public enum OpenStatus
                {
                    eUnknown = -1,
                    eFalse = 0,
                    eTrue = 1
                };
                public OpenStatus raw;
                public bool updated;
                public Timeout timeout;
                public ConfirmationThreshold<OpenStatus> confirmation_threshold;
            } 
            
            public OpenSwitch openswitch;

            public SynopticData()
            {
                this.therm = new Dictionary<string, string>
                {
                    {"Bay_Temp", "XX"},
                    {"Outdoor_Temp", "XX"},
                    {"ShopB_Temp", "XX" },
                    {"ElecRm_Temp", "XX" },
                    {"ConfRm_Temp", "XX" }
                };

                this.daemon = new Dictionary<string, bool>
                {
                    {"daisy", true },
                    {"haldor", true },
                    {"fan", true }
                };

                this.door = new Dictionary<string, bool>
                {
                    {"Front_Door", false },
                    {"Pod_Bay_Door", false }
                };

                this.motion = new Dictionary<string, bool>
                {
                    {"Shop_Motion", false },
                    {"Office_Motion", false },
                    {"ShopB_Motion", false },
                    {"ElecRm_Motion", false },
                    {"ConfRm_Motion", false }
                };

                this.Space_Message = "is UNKNOWN";
                this.Space_Open = false;

                this.openswitch = new OpenSwitch
                {
                    raw = OpenSwitch.OpenStatus.eUnknown,
                    updated = false,
                    timeout = new Timeout(10 * 60 * 4),
                    confirmation_threshold = new ConfirmationThreshold<OpenSwitch.OpenStatus>(
                        OpenSwitch.OpenStatus.eUnknown, 
                        15 * 60 * 4,
                        3
                    )
                };
            }
        }

        class pens
        {
            public Pen WallPen{ get; internal set; }
            public Pen WallPenHighlight { get; internal set; }
            public Pen InertDoor { get; internal set; }
            public Pen SensorBorder { get; internal set; }
            public Pen InOp { get; internal set; }
            public Pen DoorOpen { get; internal set; }
            public Pen DoorClosed { get; internal set; }
            public Pen OutdoorBorder { get; internal set; }
            public pens()
            {
                WallPen = new Pen(Color.Black, 4.0f);
                WallPenHighlight = new Pen(Color.Gold, 4.0f);
                InertDoor = new Pen(Color.Gray, 2.0f);
                SensorBorder = new Pen(Color.Black, 2.0f);
                InOp = new Pen(Color.Red, 2.0f);
                DoorOpen = new Pen(Color.Gold, 2.0f);
                DoorClosed = new Pen(Color.LimeGreen, 2.0f);
                OutdoorBorder = new Pen(Color.White, 2.0f); 
            }
        }

        pens Pens = new pens();

        private void SynopticMQTTHandler(MqttApplicationMessageReceivedEventArgs obj)
        {
            bool updated = false;

            if (obj.ApplicationMessage.Topic.StartsWith("haldor/"))
            {
                this.synopticData.daemon["haldor"] = false;
            }

            if (obj.ApplicationMessage.Topic.StartsWith("daisy/"))
            {
                this.synopticData.daemon["daisy"] = false;
            }


            dynamic received;
            try
            {
                received = JsonConvert.DeserializeObject(obj.ApplicationMessage.ConvertPayloadToString());
            }
            catch
            {
                return;
            }

            // get time
            int dot_position = ((string)received.time).IndexOf(".");
            int update_time = int.Parse(((string)received.time).Substring(0, dot_position));

            foreach (dynamic entry in received)
            {
                string key = entry.Name;
                string value = entry.Value;

                key = key.Replace(' ', '_');

                if (key.EndsWith("_Temp") && this.synopticData.therm.ContainsKey(key))
                {
                    string s = "";
                    if (value.Length > 2)
                    {
                        int temp = int.Parse(value);
                        temp = (temp + 500) / 1000;
                        s = temp.ToString();
                    }
                    else
                    {
                        s = value;
                    }
                    if (this.synopticData.therm[key] != s)
                    {
                        this.synopticData.therm[key] = s;
                        updated = true;
                    }
                }

                if (key.EndsWith("_Door") && this.synopticData.door.ContainsKey(key))
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

                    bool bv = Convert.ToBoolean(parsed_value);

                    if (this.synopticData.door[key] != bv)
                    {
                        this.synopticData.door[key] = bv;
                        updated = true;
                    }
                }

                if (key.EndsWith("_Motion") && this.synopticData.motion.ContainsKey(key))
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

                    bool bv = Convert.ToBoolean(parsed_value);

                    if (this.synopticData.motion[key] != bv)
                    {
                        this.synopticData.motion[key] = Convert.ToBoolean(parsed_value);
                        updated = true;
                    }
                }
                if (key == "Open_Switch")
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
                    this.synopticData.openswitch.raw = (SynopticData.OpenSwitch.OpenStatus)parsed_value;
                    this.synopticData.openswitch.updated = true;
                }
            }

            if (updated)
            {
                this.synopticPanel.Invoke(new MethodInvoker(delegate { this.synopticPanel.Refresh(); }));
            }
        }

        void SynopticTimerHandler()
        {
            time += 1;
            // default the switch status to "unknown"
            SynopticData.OpenSwitch.OpenStatus timeout_status = SynopticData.OpenSwitch.OpenStatus.eUnknown;
            SynopticData.OpenSwitch.OpenStatus confirmed_status = SynopticData.OpenSwitch.OpenStatus.eUnknown;
            bool timed_out = synopticData.openswitch.timeout.update(synopticData.openswitch.updated, time);
            // if it timed out, keep the open switch status as unknown
            if (!timed_out)
            {
                timeout_status = synopticData.openswitch.raw;
            }
            confirmed_status = synopticData.openswitch.confirmation_threshold.update(synopticData.openswitch.updated, timeout_status, time);
            synopticData.openswitch.updated = false;

            // if the space is unknown (timed out)
            if (timeout_status == SynopticData.OpenSwitch.OpenStatus.eUnknown)
            {
                synopticData.Space_Open = false;
                synopticData.Space_Message = "is UNKNOWN.";
            }
            else
            {
                // if it did not time out
                if (confirmed_status == SynopticData.OpenSwitch.OpenStatus.eTrue)
                {
                    // reset the privacy switch on space open
                    if (synopticData.Space_Open == false)
                    {
                        radioButtonOverridePrivacyOff.Checked = true;
                    }
                    synopticData.Space_Open = true;
                    synopticData.Space_Message = "is OPEN.";
                }
                else if (confirmed_status == SynopticData.OpenSwitch.OpenStatus.eFalse)
                {
                    synopticData.Space_Open = false;
                    synopticData.Space_Message = "is CLOSED.";
                }
                // not sure how we get to here (bad data?)
                else
                {
                    synopticData.Space_Open = false;
                    synopticData.Space_Message = "is UNKNOWN.";
                }
            }

            if (synopticData.openswitch.updated)
            {
                synopticPanel.Refresh();
            }
        }

        void draw_walls(Graphics g)
        {
            int x = 92;
            int y = 35;
            // outer wall
            Point[] w1 =
            {
                new Point(x, y),
                new Point(x, y-=23),
                new Point(x+=496, y),
                new Point(x, y+=376),
                new Point(x-=496,y),
                new Point(x,y-=25)
            };
            g.DrawLines(this.Pens.WallPen, w1);

            x = 92;
            y = 209;
            Point[] w2 =
            {
                new Point(x, y),
                new Point(x, 299)
            };
            g.DrawLines(this.Pens.WallPen, w2);

            // kitchen
            x = 92;
            y = 230;
            Point[] w3 =
            {
                new Point(x, y),
                new Point(x+=96,y)
            };
            g.DrawLines(this.Pens.WallPen, w3);

            x = 238;
            y = 230;
            Point[] w4 =
            {
                new Point(x, y),
                new Point(x+=14, y),
                new Point(x, y+=158)
            };
            g.DrawLines(this.Pens.WallPen, w4);

            // bathroom
            x = 252;
            y = 290;
            Point[] w5 =
            {
                new Point(x, y),
                new Point(x+=15, y)
            };
            g.DrawLines(this.Pens.WallPen, w5);

            x = 317;
            y = 290;
            Point[] w10 =
            {
                new Point(x, y),
                new Point(x+=15, y),
                new Point(x, y+=98)
            };
            g.DrawLines(this.Pens.WallPen, w10);

            // conference room
            x = 360;
            y = 12;
            Point[] w6 =
            {
                new Point(x, y),
                new Point(x, y+=140),
                new Point(x+=40, y)
            };
            g.DrawLines(this.Pens.WallPen, w6);

            x = 450;
            y = 152;
            Point[] w8 =
            {
                new Point(x, y),
                new Point(x+=138, y),
            };
            g.DrawLines(this.Pens.WallPen, w8);

            // electronics room
            x = 460;
            y = 152;
            Point[] w7 =
            {
                new Point(x, y),
                new Point(x, y+=40)
            };
            g.DrawLines(this.Pens.WallPen, w7);

            x = 460;
            y = 242;
            Point[] w9 =
            {
                new Point(x, y),
                new Point(x, 388)
            };
            g.DrawLines(this.Pens.WallPen, w9);
        }

        void draw_inert_doors(Graphics g)
        {
            //doors
            // kitchen
            int x = 188;
            int y = 230;
            Point[] d1 =
            {
                new Point(x, y),
                new Point(x+=50, y)
            };
            g.DrawLines(this.Pens.InertDoor, d1);

            // bathroom
            x = 267;
            y = 290;
            Point[] d2 =
            {
                new Point(x, y),
                new Point(317, y)
            };
            g.DrawLines(this.Pens.InertDoor, d2);

            // confRm
            x = 400;
            y = 152;
            Point[] d3 =
            {
                new Point(x, y),
                new Point(x+=50, y)
            };
            g.DrawLines(this.Pens.InertDoor, d3);

            // elecRm
            x = 460;
            y = 192;
            Point[] d4 =
            {
                new Point(x,y),
                new Point(x, y+=50)
            };
            g.DrawLines(this.Pens.InertDoor, d4);

        }

        private void thermo_Paint(int x, int y, Pen pen, Brush brush, Graphics g, string temp, bool techBad)
        {
            int width = 72;
            int height = 35;
            Font temp_font = new Font(this.Font.Name, 16);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            if (techBad == true || temp == "XX")
            {
                pen = this.Pens.InOp;
                brush = Brushes.Red;
                temp = "XX";
            }
            g.DrawString(temp + "°C", temp_font, brush, x + (width / 2), y + (height / 2), format);
            g.DrawRectangle(pen, x, y, width, height);

            if (techBad == true)
            {
                Point[] x1 =
                {
                    new Point(x - 1, y),
                    new Point(x + width, y + height)
                };
                g.DrawLines(pen, x1);
                Point[] x2 =
                {
                    new Point(x - 1, y + height - 2),
                    new Point(x + width, y)
                };
                g.DrawLines(pen, x2);
            }
        }

        private void motion_Paint(int x, int y, Graphics g, bool motion, bool techBad)
        {
            int width = 72;
            int height = 88;

            Pen borderPen = this.Pens.SensorBorder;
            Color motionColor = Color.LimeGreen;

            if (techBad == true)
            {
                borderPen = this.Pens.InOp;
                motionColor = Color.Red;
                motion = true;
            }

            borderPen.StartCap = LineCap.Round;
            borderPen.EndCap = LineCap.Round;

            int radius = 6;
            g.DrawArc(borderPen, x + 64 - radius, y - radius, radius*2, radius*2, 0, 180);
            radius = 20;
            g.DrawArc(borderPen, x + 64 - radius , y - radius, radius*2, radius*2, 90, 90);
            radius = 40;
            g.DrawArc(borderPen, x + 64 - radius, y - radius, radius*2, radius*2, 90, 90);

            if (motion == true)
            {
                Brush headBrush = new SolidBrush(motionColor);
                g.FillEllipse(headBrush, x + 35, y + 14, 14, 14);

                Pen torsoPen = new Pen(motionColor, 11.0f);
                torsoPen.StartCap = LineCap.Round;
                torsoPen.EndCap = LineCap.Round;
                Point[] torso =
                {
                    new Point(x + 38, y + 36),
                    new Point(x + 34, y + 57)
                };
                g.DrawLines(torsoPen, torso);

                Pen armsPen = new Pen(motionColor, 4.0f);
                armsPen.StartCap = LineCap.Round;
                armsPen.EndCap = LineCap.Round;
                Point[] arm_l =
                {
                    new Point(x + 38, y + 33),
                    new Point(x + 28, y + 37),
                    new Point(x + 23, y + 48)
                };
                g.DrawLines(armsPen, arm_l);
                Point[] arm_r =
                {
                    new Point(x + 39, y + 32),
                    new Point(x + 48, y + 45),
                    new Point(x + 57, y + 47)
                };
                g.DrawLines(armsPen, arm_r);

                // requested thighs not to be thicc 😭
                Pen legsPen = new Pen(motionColor, 5.0f);
                legsPen.StartCap = LineCap.Round;
                legsPen.EndCap = LineCap.Round;
                Point[] leg_l =
                {
                    new Point(x + 30, y + 60),
                    new Point(x + 27, y + 70),
                    new Point(x + 20, y + 80)
                };
                g.DrawLines(legsPen, leg_l);
                Point[] leg_r =
                {
                    new Point(x + 37, y + 60),
                    new Point(x + 41, y + 70),
                    new Point(x + 41, y + 81)
                };
                g.DrawLines(legsPen, leg_r);
            }

            borderPen.StartCap = LineCap.Flat;
            borderPen.EndCap = LineCap.Flat;
            g.DrawRectangle(borderPen, x, y, width, height);

            if (techBad == true)
            {
                Point[] x1 =
                {
                    new Point(x - 1, y),
                    new Point(x + width, y + height)
                };
                g.DrawLines(borderPen, x1);
                Point[] x2 =
                {
                    new Point(x - 1, y + height - 2),
                    new Point(x + width, y)
                };
                g.DrawLines(borderPen, x2);
            }

        }

        private void daemons_Paint(Graphics g)
        {
            int x = 112;
            int y = 190;
            int width = 72;
            int height = 35;
            Font font = new Font(this.Font.Name, 16);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            if (this.synopticData.daemon["haldor"])
            {
                g.DrawString("Haldor", font, Brushes.Red, x + (width / 2), y + (height / 2), format);
                g.DrawRectangle(this.Pens.InOp, x, y, width, height);
                Point[] x1 =
                {
                    new Point(x - 1, y),
                    new Point(x + width, y + height)
                };
                g.DrawLines(this.Pens.InOp, x1);
                Point[] x2 =
                {
                    new Point(x - 1 , y + height - 2),
                    new Point(x + width, y)
                };
                g.DrawLines(this.Pens.InOp, x2);
            }
            else
            {
                g.DrawString("Haldor", font, Brushes.Black, x + (width / 2), y + (height / 2), format);
                g.DrawRectangle(this.Pens.SensorBorder, x, y, width, height);

            }

            x = 465;
            y = 157;
            if (this.synopticData.daemon["daisy"])
            {
                g.DrawString("Daisy", font, Brushes.Red, x + (width / 2), y + (height / 2), format);
                g.DrawRectangle(this.Pens.InOp, x, y, width, height);
                Point[] x1 =
{
                    new Point(x - 1, y),
                    new Point(x + width, y + height)
                };
                g.DrawLines(this.Pens.InOp, x1);
                Point[] x2 =
                {
                    new Point(x - 1 , y + height - 2),
                    new Point(x + width, y)
                };
                g.DrawLines(this.Pens.InOp, x2);
            }
            else
            {
                g.DrawString("Daisy", font, Brushes.Black, x + (width / 2), y + (height / 2), format);
                g.DrawRectangle(this.Pens.SensorBorder, x, y, width, height);
            }
        }

        private void synopticPanel_Paint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            Graphics g = e.Graphics;

            // entire place
            Point[] points = new Point[4];
            points[0] = new Point(0, 0);
            points[1] = new Point(0, p.Height);
            points[2] = new Point(p.Width, p.Height);
            points[3] = new Point(p.Width, 0);
            Brush brush = new SolidBrush(Color.White);
            g.FillPolygon(brush, points);

            // alphalt
            brush = new SolidBrush(Color.DimGray);
            g.FillRectangle(brush, 0, 0, 90, p.Height);
            
            draw_walls(g);
            draw_inert_doors(g);

            Font font = new Font(this.Font.Name, 24);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Far;
            // Space Status
            if (this.synopticData.daemon["haldor"] == true)
            {
                // write the message
                g.DrawString("The Space\n" + "is UNKNOWN", font, Brushes.Red, 360, 18, format);
            }
            else
            {
                g.DrawString("The Space\n" + this.synopticData.Space_Message, font, Brushes.Black, 360, 18, format);
            }

            // Pod Bay Door
            if (this.synopticData.daemon["haldor"] == true)
            {
                int left_corner_x = 91;
                int left_corner_y = 36;
                int width = 16;
                int height = 172;
                g.DrawRectangle(this.Pens.InOp, left_corner_x, left_corner_y, width, height);
                Point[] x1 =
                {
                    new Point(left_corner_x - 1, left_corner_y),
                    new Point(left_corner_x + width, left_corner_y + height)
                };
                g.DrawLines(this.Pens.InOp, x1);
                Point[] x2 =
                {
                    new Point(left_corner_x - 1, left_corner_y + height - 2),
                    new Point(left_corner_x + width, left_corner_y)
                };
                g.DrawLines(this.Pens.InOp, x2);
            }
            else if(this.synopticData.door["Pod_Bay_Door"] == true)
            {
                // Open
                Point[] d1 =
                {
                    new Point(92, 35),
                    new Point(92, 35+20)
                };
                g.DrawLines(this.Pens.DoorOpen, d1);

                Point[] d2 =
                {
                    new Point(92, 209),
                    new Point(92, 209-20)
                };
                g.DrawLines(this.Pens.DoorOpen, d2);
            }
            else
            {
                // Closed
                Point[] d1 =
                {
                    new Point(92, 35),
                    new Point(92, 209)
                };
                g.DrawLines(this.Pens.DoorClosed, d1);
            }

            // front door
            if (this.synopticData.daemon["haldor"] == true)
            {
                int left_corner_x = 33;
                int left_corner_y = 300;
                int width = 60;
                int height = 62;
                g.DrawRectangle(this.Pens.InOp, left_corner_x, left_corner_y, width, height);
                Point[] x1 =
                {
                    new Point(left_corner_x, left_corner_y),
                    new Point(left_corner_x + width, left_corner_y + height)
                };
                g.DrawLines(this.Pens.InOp, x1);
                Point[] x2 =
                {
                    new Point(left_corner_x, left_corner_y + height),
                    new Point(left_corner_x + width, left_corner_y)
                };
                g.DrawLines(this.Pens.InOp, x2);
            }
            else if (this.synopticData.door["Front_Door"] == true)
            {
                Pen pen = (Pen)this.Pens.DoorOpen.Clone();
                pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
                Point[] d1 =
                {
                    new Point(92, 362),
                    new Point(92-58, 362-26)
                };
                g.DrawLines(pen, d1);
            }
            else
            {
                Point[] d1 =
                {
                    new Point(92, 363),
                    new Point(92, 363-64)
                };
                g.DrawLines(this.Pens.DoorClosed, d1);
            }

            // thermostats
            thermo_Paint(15, 16, this.Pens.OutdoorBorder, Brushes.White, g, this.synopticData.therm["Outdoor_Temp"], this.synopticData.daemon["haldor"]);
            thermo_Paint(112, 151, this.Pens.SensorBorder, Brushes.Black, g, this.synopticData.therm["Bay_Temp"], this.synopticData.daemon["haldor"]);
            thermo_Paint(383, 249, this.Pens.SensorBorder, Brushes.Black, g, this.synopticData.therm["ShopB_Temp"], this.synopticData.daemon["daisy"]);
            thermo_Paint(465, 196, this.Pens.SensorBorder, Brushes.Black, g, this.synopticData.therm["ElecRm_Temp"], this.synopticData.daemon["daisy"]);
            thermo_Paint(386, 112, this.Pens.SensorBorder, Brushes.Black, g, this.synopticData.therm["ConfRm_Temp"], this.synopticData.daemon["daisy"]);

            // motion
            motion_Paint(188, 137, g, this.synopticData.motion["Shop_Motion"], this.synopticData.daemon["haldor"]);
            motion_Paint(97, 246, g, this.synopticData.motion["Office_Motion"], this.synopticData.daemon["haldor"]);
            motion_Paint(383, 157, g, this.synopticData.motion["ShopB_Motion"], this.synopticData.daemon["daisy"]);
            motion_Paint(465, 235, g, this.synopticData.motion["ElecRm_Motion"], this.synopticData.daemon["daisy"]);
            motion_Paint(462, 59, g, this.synopticData.motion["ConfRm_Motion"], this.synopticData.daemon["daisy"]);

            daemons_Paint(g);
        }
    }
}
