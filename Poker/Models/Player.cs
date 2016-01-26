using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    using System.Drawing;
    using System.Windows.Forms;

    // this class should be inherited by Bot and Human
    public abstract class Player
    {
        private const int PanelHeightDefault = 150;

        private const int PanelWidthDefault = 180;

        private const int StartingChipsDefault = 10000;

        private string name;

        private int chips;

        private Panel panel = new Panel();

        private Point locationOnTableOnTable;

        // i think holeCards should be moved to some kind of Manager.cs class (iliyan)
        private PictureBox[] holeCards = new PictureBox[2];

        // private double b1Power;

        private double handStrenght;

        // private bool B1turn;

        private bool shouldAct;

        private bool folded;

        private int call;

        private double Raise; // ??

        private int raise; // ??

        protected Player(string name, Point locationOnTable, int startingChips = StartingChipsDefault)
        {
            this.Name = name;
            this.Chips = startingChips;
            this.LocationOnTableOnTable = locationOnTable;
        }

        public Point LocationOnTableOnTable
        {
            get
            {
                return this.locationOnTableOnTable;
            }
            set
            {
                this.locationOnTableOnTable = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public PictureBox[] HoleCards
        {
            get
            {
                return this.holeCards;
            }
            private set
            {
                this.holeCards = value;
            }
        }

        public int Chips
        {
            get
            {
                return this.chips;
            }
            set
            {
                this.chips = value;
            }
        }

        public Panel Panel
        {
            get
            {
                return this.panel;
            }

            private set
            {
                this.panel = value;
            }
        }

        // i think InitializePanel should be moved to some kind of Manager.cs class (iliyan)
        public virtual void InitializePanel(Control.ControlCollection controls)
        {
            //Control.ControlCollection Controls = controls;
            controls.Add(this.Panel);
            this.Panel.Location = this.LocationOnTableOnTable;
            this.Panel.BackColor = Color.DarkBlue;
            this.Panel.Height = PanelHeightDefault;
            this.Panel.Width = PanelWidthDefault;
            this.Panel.Visible = false;
        }

        public void SetHoleCards(AnchorStyles anchorStyle, Image[] cardsImages)
        {
            // not sure if .Tag is neccessary (coppied it from Form1.cs)
            // this.HoleCards[0].Tag = this.Reserve[this.i];

            for (int i = 0; i < 2; i++)
            {
                this.HoleCards[i].Image = cardsImages[i];
                this.HoleCards[i].Anchor = anchorStyle;
                this.HoleCards[i].Visible = true;
                if (i == 0)
                {
                    this.HoleCards[i].Location = this.LocationOnTableOnTable;
                }
                else
                {
                    this.HoleCards[i].Location = new Point(
                        this.LocationOnTableOnTable.X + cardsImages[i].Width,
                        this.LocationOnTableOnTable.Y);
                }
            }
        }
    }
}
