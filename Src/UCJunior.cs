using JuniorComputer.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace JuniorComputer
{
    public partial class UCJunior : UserControl
    {
        #region Members

        // Led display 
        public SevenSegment sevenSegmentAddress0;
        public SevenSegment sevenSegmentAddress1;
        public SevenSegment sevenSegmentAddress2;
        public SevenSegment sevenSegmentAddress3;
        public SevenSegment sevenSegmentAddress4;
        public SevenSegment sevenSegmentAddress5;
        public SevenSegment sevenSegmentData0;
        public SevenSegment sevenSegmentData1;

        // Keyboard
        public Key key0;
        public Key key1;
        public Key key2;
        public Key key3;
        public Key key4;
        public Key key5;
        public Key key6;
        public Key key7;
        public Key key8;
        public Key key9;
        public Key keyA;
        public Key keyB;
        public Key keyC;
        public Key keyD;
        public Key keyE;
        public Key keyF;
        public Key keyAD;
        public Key keyDA;
        public Key keyPL;
        public Key keyGO;
        public Key keyPC;

        public Key keyNMI;
        public Key keyRESET;

        public bool step = false;
        public bool display = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UCJunior()
        {
            InitializeComponent();

            // Most left digit
            sevenSegmentAddress3 = new SevenSegment();
            sevenSegmentAddress3.Location = new Point(14, 4);
            Controls.Add(sevenSegmentAddress3);

            sevenSegmentAddress2 = new SevenSegment();
            sevenSegmentAddress2.Location = new Point(sevenSegmentAddress3.Width + 18, 4);
            Controls.Add(sevenSegmentAddress2);

            sevenSegmentAddress1 = new SevenSegment();
            sevenSegmentAddress1.Location = new Point(sevenSegmentAddress3.Width + sevenSegmentAddress2.Width + 22, 4);
            Controls.Add(sevenSegmentAddress1);

            sevenSegmentAddress0 = new SevenSegment();
            sevenSegmentAddress0.Location = new Point(sevenSegmentAddress3.Width + sevenSegmentAddress2.Width + sevenSegmentAddress1.Width + 26, 4);
            Controls.Add(sevenSegmentAddress0);

            sevenSegmentData1 = new SevenSegment();
            sevenSegmentData1.Location = new Point(sevenSegmentAddress3.Width + sevenSegmentAddress2.Width + sevenSegmentAddress1.Width + sevenSegmentAddress0.Width + 40, 4);
            Controls.Add(sevenSegmentData1);

            // Most right digit
            sevenSegmentData0 = new SevenSegment();
            sevenSegmentData0.Location = new Point(sevenSegmentAddress3.Width + sevenSegmentAddress2.Width + sevenSegmentAddress1.Width + sevenSegmentAddress0.Width + sevenSegmentData1.Width + 44, 4);
            Controls.Add(sevenSegmentData0);

            // Button positions
            int startX = 44;
            int startY = 4 + sevenSegmentAddress3.Height + 34;

            keyC = new Key(42, 60, "C", "");
            keyC.Location = new Point(startX, startY);
            Controls.Add(keyC);

            startX += keyC.Width + 15;

            keyD = new Key(42, 60, "D", "");
            keyD.Location = new Point(startX, startY);
            Controls.Add(keyD);

            startX += keyD.Width + 15;

            keyE = new Key(42, 60, "E", "");
            keyE.Location = new Point(startX, startY);
            Controls.Add(keyE);

            startX += keyE.Width + 15;

            keyF = new Key(42, 60, "F", "");
            keyF.Location = new Point(startX, startY);
            Controls.Add(keyF);

            startX += keyF.Width + 74;

            keyPC = new Key(42, 60, "SEARCH", "PC", true);
            keyPC.Location = new Point(startX, startY);
            Controls.Add(keyPC);

            startY += keyPC.Height + 12;
            startX = 44;

            key8 = new Key(42, 60, "8", "");
            key8.Location = new Point(startX, startY);
            Controls.Add(key8);

            startX += key8.Width + 15;

            key9 = new Key(42, 60, "9", "");
            key9.Location = new Point(startX, startY);
            Controls.Add(key9);

            startX += key9.Width + 15;

            keyA = new Key(42, 60, "A", "");
            keyA.Location = new Point(startX, startY);
            Controls.Add(keyA);

            startX += keyA.Width + 15;

            keyB = new Key(42, 60, "B", "");
            keyB.Location = new Point(startX, startY);
            Controls.Add(keyB);

            startX += keyB.Width + 74;

            keyAD = new Key(42, 60, "INSERT", "AD", true);
            keyAD.Location = new Point(startX, startY);
            Controls.Add(keyAD);

            startX += keyAD.Width + 15;

            keyGO = new Key(42, 60, "", "GO", true);
            keyGO.Location = new Point(startX, startY);
            Controls.Add(keyGO);

            startX = 44;
            startY += keyGO.Height + 12;

            key4 = new Key(42, 60, "4", "");
            key4.Location = new Point(startX, startY);
            Controls.Add(key4);

            startX += key4.Width + 15;

            key5 = new Key(42, 60, "5", "");
            key5.Location = new Point(startX, startY);
            Controls.Add(key5);

            startX += key5.Width + 15;

            key6 = new Key(42, 60, "6", "");
            key6.Location = new Point(startX, startY);
            Controls.Add(key6);

            startX += key6.Width + 15;

            key7 = new Key(42, 60, "7", "");
            key7.Location = new Point(startX, startY);
            Controls.Add(key7);

            startX += key7.Width + 74;

            keyDA = new Key(42, 60, "DELETE", "DA", true);
            keyDA.Location = new Point(startX, startY);
            Controls.Add(keyDA);

            startX += keyDA.Width + 15;

            keyNMI = new Key(42, 60, "NMI", "ST", true);
            keyNMI.Location = new Point(startX, startY);
            Controls.Add(keyNMI);

            startX = 44;
            startY += keyNMI.Height + 12;

            key0 = new Key(42, 60, "0", "");
            key0.Location = new Point(startX, startY);
            Controls.Add(key0);

            startX += key0.Width + 15;

            key1 = new Key(42, 60, "1", "");
            key1.Location = new Point(startX, startY);
            Controls.Add(key1);

            startX += key1.Width + 15;

            key2 = new Key(42, 60, "2", "");
            key2.Location = new Point(startX, startY);
            Controls.Add(key2);

            startX += key2.Width + 15;

            key3 = new Key(42, 60, "3", "");
            key3.Location = new Point(startX, startY);
            Controls.Add(key3);

            startX += key3.Width + 74;

            keyPL = new Key(42, 60, "SKIP", "+", true);
            keyPL.Location = new Point(startX, startY);
            Controls.Add(keyPL);

            startX += keyPL.Width + 15;
            keyRESET = new Key(42, 60, "", "RST", true);
            keyRESET.Location = new Point(startX, startY);
            Controls.Add(keyRESET);

            PictureBox pbStep = new PictureBox();
            pbStep.Name = "pbStep";
            pbStep.Tag = "RED";
            pbStep.BackColor = Color.Transparent;
            pbStep.Size = new Size(22, 22);
            pbStep.Location = new Point(406, 155);
            pbStep.Image = Resources.red;
            pbStep.Click += new System.EventHandler(pbClick);
            Controls.Add(pbStep);

            PictureBox pbDisplay = new PictureBox();
            pbDisplay.Name = "pbDisplay";
            pbDisplay.Tag = "GREEN";
            pbDisplay.BackColor = Color.Transparent;
            pbDisplay.Size = new Size(22, 22);
            pbDisplay.Location = new Point(474, 155);
            pbDisplay.Image = Resources.green;
            pbDisplay.Click += new System.EventHandler(pbClick);
            Controls.Add(pbDisplay);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Change picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbClick(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.Tag.ToString() == "RED")
            {
                pb.Tag = "GREEN";
                pb.Image = Resources.green;

                if (pb.Name == "pbStep") step = true;
                if (pb.Name == "pbDisplay")
                {
                    display = true;
                    sevenSegmentAddress0.off = false;
                    sevenSegmentAddress1.off = false;
                    sevenSegmentAddress2.off = false;
                    sevenSegmentAddress3.off = false;
                    sevenSegmentData0.off = false;
                    sevenSegmentData1.off = false;
                }
            } else
            {
                pb.Tag = "RED";
                pb.Image = Resources.red;

                if (pb.Name == "pbStep") step = false;
                if (pb.Name == "pbDisplay")
                {
                    display = false;
                    sevenSegmentAddress0.off = true;
                    sevenSegmentAddress1.off = true;
                    sevenSegmentAddress2.off = true;
                    sevenSegmentAddress3.off = true;
                    sevenSegmentData0.off = true;
                    sevenSegmentData1.off = true;
                }
            }
        }

        #endregion
    }
}
