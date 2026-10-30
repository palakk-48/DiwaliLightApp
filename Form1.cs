
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DiwaliLightApp
{
    public partial class Form1 : Form
    {
        private Timer jhalarTimer;
        private Label[] bulbs = new Label[12];
        private bool blinkState = false;

        public Form1()
        {
            InitializeComponent();

            CreateBulbs();

            jhalarTimer = new Timer();
            jhalarTimer.Interval = 400;
            jhalarTimer.Tick += JhalarTimer_Tick;

            pnlJhalar.Paint += PnlJhalar_Paint;

            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            
        }


        private void CreateBulbs()
        {
            int totalBulbs = 12;

            for (int i = 0; i < totalBulbs; i++)
            {
                bulbs[i] = new Label();

                bulbs[i].Size = new Size(32, 32);

                int x = 20 + (i * 51);
                int y = 75 + (int)(15 * Math.Sin(i * 0.5));

                bulbs[i].Location = new Point(x, y);

                bulbs[i].BackColor = Color.White;
                bulbs[i].Text = "";
                bulbs[i].AutoSize = false;

                // Make the bulb perfectly circular
                GraphicsPath path = new GraphicsPath();

                path.AddEllipse(0, 0, 31, 31);

                bulbs[i].Region = new Region(path);

                path.Dispose();

                // Draw black circular border
                bulbs[i].Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        e.Graphics.DrawEllipse(
                            pen, 1, 1, 29, 29);
                    }
                };

                pnlJhalar.Controls.Add(bulbs[i]);
            }
        }



        private void PnlJhalar_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen wire = new Pen(Color.Black, 2))
            {
                // Draw the main curved wire
                e.Graphics.DrawBezier(
                    wire,
                    10, 35,
                    160, 75,
                    450, 5,
                    620, 35
                );

                // Draw small hanging wires
                for (int i = 0; i < 12; i++)
                {
                    int x = 35 + (i * 51);
                    int y = 75 + (int)(15 * Math.Sin(i * 0.5));

                    int startY = 35 + (int)(20 * Math.Sin(i * 0.5));

                    e.Graphics.DrawLine(
                        wire,
                        x, startY,
                        x, y
                    );
                }
            }
        }


        // Start button
        private void BtnStart_Click(object sender, EventArgs e)
        {
            blinkState = false;
            UpdateBulbs();
            jhalarTimer.Start();
        }

        // Stop button
        private void BtnStop_Click(object sender, EventArgs e)
        {
            jhalarTimer.Stop();
        }

        // Reset button
        private void BtnReset_Click(object sender, EventArgs e)
        {
            jhalarTimer.Stop();
            blinkState = false;

            foreach (Label bulb in bulbs)
            {
                bulb.BackColor = Color.White;
                bulb.Invalidate();
            }
        }

        // Timer animation
        private void JhalarTimer_Tick(object sender, EventArgs e)
        {
            blinkState = !blinkState;
            UpdateBulbs();
        }

        // Change bulb colors
        private void UpdateBulbs()
        {
            for (int i = 0; i < bulbs.Length; i++)
            {
                if ((i % 2 == 0) == blinkState)
                {
                    bulbs[i].BackColor = Color.Red;
                }
                else
                {
                    bulbs[i].BackColor = Color.White;
                }

                bulbs[i].Invalidate();
            }
        }
    }
}
