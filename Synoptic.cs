using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HAL_Display
{
	public partial class Display
	{
        class SynopticData
        {
            public Dictionary<string, string> therm;
            // inverse logic: it's true if it's bad!
            public Dictionary<string, bool> daemon;
            public Dictionary<string, bool> door;
            public Dictionary<string, bool> motion;
            public bool Space_Open;

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
                    {"hal", true },
                    {"fan", true },
                    {"tweeter", true }
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

                this.Space_Open = false;
            }
        }

        private void SynopticHandler()
        {

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
            Pen pen = new Pen(Color.Black, (float)4.0);
            g.DrawLines(pen, w1);

            x = 92;
            y = 209;
            Point[] w2 =
            {
                new Point(x, y),
                new Point(x, 299)
            };
            g.DrawLines(pen, w2);

            // kitchen
            x = 92;
            y = 230;
            Point[] w3 =
            {
                new Point(x, y),
                new Point(x+=96,y)
            };
            g.DrawLines(pen, w3);

            x = 238;
            y = 230;
            Point[] w4 =
            {
                new Point(x, y),
                new Point(x+=14, y),
                new Point(x, y+=158)
            };
            g.DrawLines(pen, w4);

            // bathroom
            x = 252;
            y = 290;
            Point[] w5 =
            {
                new Point(x, y),
                new Point(x+=15, y)
            };
            g.DrawLines(pen, w5);

            x = 317;
            y = 290;
            Point[] w10 =
            {
                new Point(x, y),
                new Point(x+=15, y),
                new Point(x, y+=98)
            };
            g.DrawLines(pen, w10);

            // conference room
            x = 360;
            y = 12;
            Point[] w6 =
            {
                new Point(x, y),
                new Point(x, y+=140),
                new Point(x+=40, y)
            };
            g.DrawLines(pen, w6);

            x = 450;
            y = 152;
            Point[] w8 =
            {
                new Point(x, y),
                new Point(x+=138, y),
            };
            g.DrawLines(pen, w8);

            // electronics room
            x = 460;
            y = 152;
            Point[] w7 =
            {
                new Point(x, y),
                new Point(x, y+=40)
            };
            g.DrawLines(pen, w7);

            x = 460;
            y = 242;
            Point[] w9 =
            {
                new Point(x, y),
                new Point(x, 388)
            };
            g.DrawLines(pen, w9);
        }

        void draw_inert_doors(Graphics g)
        {
            //doors
            Pen pen = new Pen(Color.Gray, (float)2.0);
            // kitchen
            int x = 188;
            int y = 230;
            Point[] d1 =
            {
                new Point(x, y),
                new Point(x+=50, y)
            };
            g.DrawLines(pen, d1);

            // bathroom
            x = 267;
            y = 290;
            Point[] d2 =
            {
                new Point(x, y),
                new Point(317, y)
            };
            g.DrawLines(pen, d2);

            // confRm
            x = 400;
            y = 152;
            Point[] d3 =
            {
                new Point(x, y),
                new Point(x+=50, y)
            };
            g.DrawLines(pen, d3);

            // elecRm
            x = 460;
            y = 192;
            Point[] d4 =
            {
                new Point(x,y),
                new Point(x, y+=50)
            };
            g.DrawLines(pen, d4);

        }

        private void thermo_Paint(int x, int y, Pen pen, Brush brush, Graphics g, string temp, bool techBad)
        {
            int width = 72;
            int height = 35;
            if (techBad == true || temp == "XX")
            {
                pen = Pens.Red;
                brush = Brushes.Red;
                temp = "XX";
            }
            g.DrawString("" + temp + "C", this.Font, brush, x + 3.5f, y + 24f);
            g.DrawRectangle(pen, x, y, width, height);
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

            // Pod Bay Door
            if (this.synopticData.daemon["hal"] == true)
            {
                int left_corner_x = 89;
                int left_corner_y = 35;
                int width = 17;
                int height = 173;
                g.DrawRectangle(Pens.Red, left_corner_x, left_corner_y, width, height);
                Point[] x1 =
                {
                    new Point(left_corner_x, left_corner_y),
                    new Point(left_corner_x + width, left_corner_y + height)
                };
                g.DrawLines(Pens.Red, x1);
                Point[] x2 =
                {
                    new Point(left_corner_x, left_corner_y + height),
                    new Point(left_corner_x + width, left_corner_y)
                };
                g.DrawLines(Pens.Red, x2);
            }
            else if(this.synopticData.door["Pod_Bay_Door"] == true)
            {
                // closed
                Pen pen = new Pen(Color.Gold, 2.0f);
                Point[] d1 =
                {
                    new Point(92, 35),
                    new Point(92, 35+20)
                };
                g.DrawLines(pen, d1);

                Point[] d2 =
                {
                    new Point(92, 208),
                    new Point(92, 208-20)
                };
                g.DrawLines(pen, d2);
            }
            else
            {
                // open
                Pen pen = new Pen(Color.LimeGreen, 2.0f);
                Point[] d1 =
                {
                    new Point(92, 35),
                    new Point(92, 209)
                };
            }

            // front door
            if (this.synopticData.daemon["hal"] == true)
            {
                int left_corner_x = 31;
                int left_corner_y = 299;
                int width = 62;
                int height = 63;
                g.DrawRectangle(Pens.Red, left_corner_x, left_corner_y, width, height);
                Point[] x1 =
                {
                    new Point(left_corner_x, left_corner_y),
                    new Point(left_corner_x + width, left_corner_y + height)
                };
                g.DrawLines(Pens.Red, x1);
                Point[] x2 =
                {
                    new Point(left_corner_x, left_corner_y + height),
                    new Point(left_corner_x + width, left_corner_y)
                };
                g.DrawLines(Pens.Red, x2);
            }
            else if (this.synopticData.door["Front_Door"] == true)
            {
                Pen pen = new Pen(Color.Gold, 2.0f);
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
                Pen pen = new Pen(Color.LimeGreen, 2.0f);
                Point[] d1 =
                {
                    new Point(92, 363),
                    new Point(92, 363-64)
                };
                g.DrawLines(pen, d1);
            }

            // thermostats
            thermo_Paint(15, 16, Pens.Black, Brushes.Black, g, "15", true);
        }
    }
}
