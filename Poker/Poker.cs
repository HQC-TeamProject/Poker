namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Properties;

    using Timer = System.Windows.Forms.Timer;

    public partial class Poker : Form
    {

        #region Fields
        // 5 bots x 2 cards + Player 2 cards = 12
        // community cards (board) = 5 cards total (3 cards for flop, 1 for turn, 1 for river)
        private const int TotalCardsDealedPerHand = 17;
        private const int TotalCardsInDeck = 52;
        private const int DefaultCardHeight = 130;
        private const int DefaultCardWidth = 80;

        // these panels are the dark blue frame surrounding someone's cards at the end of the hand
        // maybe the initial idea is someone's panel to be set to visible when it's his turn to act
        // so you can easily recognize who's turn is it at the given moment. (iliyan)
        private readonly Panel pPanel = new Panel();
        private readonly Panel botOnePanel = new Panel();
        private readonly Panel botTwoPanel = new Panel();
        private readonly Panel botThreePanel = new Panel();
        private readonly Panel botFourPanel = new Panel();
        private readonly Panel b5Panel = new Panel();

        private int call = 500;
        private int foldedPlayers = 5;
        private int startingChipsDefault = 10000;

        private int bot1Chips = 10000;
        private int bot2Chips = 10000;
        private int bot3Chips = 10000;
        private int bot4Chips = 10000;
        private int bot5Chips = 10000;

        private double type;
        private double rounds;
        private double botOnePower;
        private double botTwoPower;
        private double botThreePower;
        private double botFourPower;
        private double botFivePower;
        private double playerPower;
        private double playerType = -1;
        private double raise;
        private double botOneType = -1;
        private double botTwoType = -1;
        private double botThreeType = -1;
        private double botFourType = -1;
        private double b5Type = -1;

        private bool isBotOneTurn;
        private bool isBotTwoTurn;
        private bool isBotThreeTurn;
        private bool isBotFourTurn;
        private bool isFiveTwoTurn;
        private bool isBotOneFirstTurn;
        private bool isBotTwoFirstTurn;
        private bool isBotThreeFirstTurn;
        private bool isBotFourFirstTurn;
        private bool isBotFiveFirstTurn;
        private bool isPlayerFolded;
        private bool isBotOneFolded;
        private bool isBotTwoFolded;
        private bool isBotThreeFolded;
        private bool isBotFourFolded;
        private bool isBotFiveFolded;
        private bool intsadded;
        private bool changed;

        private int playerCall;
        private int botOneCall;
        private int botTwoCall;
        private int botThreeCall;
        private int botFourCall;
        private int botFiveCall;
        private int playerRaise;
        private int botOneRaise;
        private int botTwoRaise;
        private int botThreeRaise;
        private int botFourRaise;
        private int b5Raise;
        private int height;
        private int width;
        private int winners;
        private int flop = 1;
        private int turn = 2;
        private int river = 3;
        private int end = 4;
        private int maxLeft = 6;
        //        private int last = 123;
        private int raisedTurn = 1;

        private readonly List<bool?> bools = new List<bool?>();
        private readonly List<Type> Win = new List<Type>();
        private readonly List<string> CheckWinners = new List<string>();
        private readonly List<int> ints = new List<int>();

        private Random random = new Random();

        private bool PFturn;
        private bool Pturn = true;
        private bool restart;
        private bool raising;

        private Type sorted;

        private string[] cardsImageLocations = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

        private readonly int[] Reserve = new int[TotalCardsDealedPerHand];

        private readonly Image[] deckImages = new Image[TotalCardsInDeck];

        private readonly PictureBox[] CardsPicturesHolder = new PictureBox[TotalCardsInDeck];

        private readonly Timer timer = new Timer();
        private readonly Timer Updates = new Timer();

        private int t = 60;
        private int i;

        private int bb = 500;
        private int sb = 250;

        private int up = 10000000;
        private int turnCount;
        #endregion

        public Poker()
        {
            // bools.Add(PFturn); bools.Add(isBotOneFirstTurn); bools.Add(isBotTwoFirstTurn); bools.Add(isBotThreeFirstTurn); bools.Add(isBotFourFirstTurn); bools.Add(isBotFiveFirstTurn);
            this.call = this.bb;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Updates.Start();
            this.InitializeComponent();
            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();
            this.tbTotalPot.Enabled = false;
            this.tbPlayerChips.Enabled = false;
            this.tbBotChips1.Enabled = false;
            this.tbBotChips2.Enabled = false;
            this.tbBotChips3.Enabled = false;
            this.tbBotChips4.Enabled = false;
            this.tbBotChips5.Enabled = false;
            this.tbPlayerChips.Text = Resources.Chips + this.startingChipsDefault;
            this.tbBotChips1.Text = Resources.Chips + this.bot1Chips;
            this.tbBotChips2.Text = Resources.Chips + this.bot2Chips;
            this.tbBotChips3.Text = Resources.Chips + this.bot3Chips;
            this.tbBotChips4.Text = Resources.Chips + this.bot4Chips;
            this.tbBotChips5.Text = Resources.Chips + this.bot5Chips;
            this.timer.Interval = 1 * 1 * 1000;
            this.timer.Tick += this.TimerTick;
            this.Updates.Interval = 1 * 1 * 100;
            this.Updates.Tick += this.UpdateTick;
            this.tbBB.Visible = true;
            this.tbSB.Visible = true;
            this.bBB.Visible = true;
            this.bSB.Visible = true;
            this.tbBB.Visible = true;
            this.tbSB.Visible = true;
            this.bBB.Visible = true;
            this.bSB.Visible = true;
            this.tbBB.Visible = false;
            this.tbSB.Visible = false;
            this.bBB.Visible = false;
            this.bSB.Visible = false;
            this.tbRaise.Text = (this.bb * 2).ToString();
        }

        private async Task Shuffle()
        {
            this.bools.Add(this.PFturn);
            this.bools.Add(this.isBotOneFirstTurn);
            this.bools.Add(this.isBotTwoFirstTurn);
            this.bools.Add(this.isBotThreeFirstTurn);
            this.bools.Add(this.isBotFourFirstTurn);
            this.bools.Add(this.isBotFiveFirstTurn);

            this.botCall.Enabled = false;
            this.bRaise.Enabled = false;
            this.bFold.Enabled = false;
            this.bCheck.Enabled = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            bool check = false;

            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            int horizontal = 580;
            int vertical = 480;

            // this loop is the logic behind shuffling the deck. everything else is not supposed to be here (iliyan)
            for (this.i = this.cardsImageLocations.Length; this.i > 0; this.i--)
            {
                int j = random.Next(this.i);

                var k = this.cardsImageLocations[j];
                this.cardsImageLocations[j] = this.cardsImageLocations[this.i - 1];
                this.cardsImageLocations[this.i - 1] = k;
            }

            // this loop picks the first 17 cards from the shuffled deck which will be dealed and assigns then to the players
            for (this.i = 0; this.i < TotalCardsDealedPerHand; this.i++)
            {
                this.deckImages[this.i] = Image.FromFile(this.cardsImageLocations[this.i]);

                var charsToRemove = new[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    this.cardsImageLocations[this.i] = this.cardsImageLocations[this.i].Replace(c, string.Empty);
                }

                this.Reserve[this.i] = int.Parse(this.cardsImageLocations[this.i]) - 1;

                // initializing Cards' picturesBoxes
                this.CardsPicturesHolder[this.i] = new PictureBox();
                this.CardsPicturesHolder[this.i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.CardsPicturesHolder[this.i].Height = DefaultCardHeight;
                this.CardsPicturesHolder[this.i].Width = DefaultCardWidth;
                this.CardsPicturesHolder[this.i].Name = "pb" + this.i;

                this.Controls.Add(this.CardsPicturesHolder[this.i]);

                await Task.Delay(200);

                // first 2 cards to be dealt are for the player (human, not bots) and this is where it happens (iliyan)
                if (this.i < 2)
                {
                    // this block checks if the current player has already been dealt the first card
                    //if (this.CardsPicturesHolder[0].Tag != null)
                    //{
                    //    this.CardsPicturesHolder[1].Tag = this.Reserve[1];
                    //}

                    this.CardsPicturesHolder[this.i].Tag = this.Reserve[this.i];
                    this.CardsPicturesHolder[this.i].Image = this.deckImages[this.i];
                    this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Bottom;

                    // CardsPicturesHolder[i].Dock = DockStyle.Top;
                    this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                    horizontal += this.CardsPicturesHolder[this.i].Width;
                    this.Controls.Add(this.pPanel);
                    this.pPanel.Location = new Point(this.CardsPicturesHolder[this.i].Left - 10, this.CardsPicturesHolder[this.i].Top - 10);
                    this.pPanel.BackColor = Color.DarkBlue;
                    this.pPanel.Height = 150;
                    this.pPanel.Width = 180;
                    this.pPanel.Visible = false;
                }

                #region botChips conditions
                if (this.bot1Chips > 0)
                {
                    this.foldedPlayers--;
                    if (this.i >= 2 && this.i < 4)
                    {
                        if (this.CardsPicturesHolder[2].Tag != null)
                        {
                            this.CardsPicturesHolder[3].Tag = this.Reserve[3];
                        }

                        this.CardsPicturesHolder[2].Tag = this.Reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += this.CardsPicturesHolder[this.i].Width;
                        this.CardsPicturesHolder[this.i].Visible = true;
                        this.Controls.Add(this.botOnePanel);
                        this.botOnePanel.Location = new Point(this.CardsPicturesHolder[2].Left - 10, this.CardsPicturesHolder[2].Top - 10);
                        this.botOnePanel.BackColor = Color.DarkBlue;
                        this.botOnePanel.Height = 150;
                        this.botOnePanel.Width = 180;
                        this.botOnePanel.Visible = false;
                        if (this.i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bot2Chips > 0)
                {
                    this.foldedPlayers--;
                    if (this.i >= 4 && this.i < 6)
                    {
                        if (this.CardsPicturesHolder[4].Tag != null)
                        {
                            this.CardsPicturesHolder[5].Tag = this.Reserve[5];
                        }

                        this.CardsPicturesHolder[4].Tag = this.Reserve[4];

                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += this.CardsPicturesHolder[this.i].Width;
                        this.CardsPicturesHolder[this.i].Visible = true;
                        this.Controls.Add(this.botTwoPanel);
                        this.botTwoPanel.Location = new Point(this.CardsPicturesHolder[4].Left - 10, this.CardsPicturesHolder[4].Top - 10);
                        this.botTwoPanel.BackColor = Color.DarkBlue;
                        this.botTwoPanel.Height = 150;
                        this.botTwoPanel.Width = 180;
                        this.botTwoPanel.Visible = false;
                        if (this.i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bot3Chips > 0)
                {
                    this.foldedPlayers--;
                    if (this.i >= 6 && this.i < 8)
                    {
                        if (this.CardsPicturesHolder[6].Tag != null)
                        {
                            this.CardsPicturesHolder[7].Tag = this.Reserve[7];
                        }

                        this.CardsPicturesHolder[6].Tag = this.Reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Top;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += this.CardsPicturesHolder[this.i].Width;
                        this.CardsPicturesHolder[this.i].Visible = true;
                        this.Controls.Add(this.botThreePanel);
                        this.botThreePanel.Location = new Point(this.CardsPicturesHolder[6].Left - 10, this.CardsPicturesHolder[6].Top - 10);
                        this.botThreePanel.BackColor = Color.DarkBlue;
                        this.botThreePanel.Height = 150;
                        this.botThreePanel.Width = 180;
                        this.botThreePanel.Visible = false;
                        if (this.i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bot4Chips > 0)
                {
                    this.foldedPlayers--;
                    if (this.i >= 8 && this.i < 10)
                    {
                        if (this.CardsPicturesHolder[8].Tag != null)
                        {
                            this.CardsPicturesHolder[9].Tag = this.Reserve[9];
                        }

                        this.CardsPicturesHolder[8].Tag = this.Reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += this.CardsPicturesHolder[this.i].Width;
                        this.CardsPicturesHolder[this.i].Visible = true;
                        this.Controls.Add(this.botFourPanel);
                        this.botFourPanel.Location = new Point(this.CardsPicturesHolder[8].Left - 10, this.CardsPicturesHolder[8].Top - 10);
                        this.botFourPanel.BackColor = Color.DarkBlue;
                        this.botFourPanel.Height = 150;
                        this.botFourPanel.Width = 180;
                        this.botFourPanel.Visible = false;
                        if (this.i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bot5Chips > 0)
                {
                    this.foldedPlayers--;
                    if (this.i >= 10 && this.i < 12)
                    {
                        if (this.CardsPicturesHolder[10].Tag != null)
                        {
                            this.CardsPicturesHolder[11].Tag = this.Reserve[11];
                        }

                        this.CardsPicturesHolder[10].Tag = this.Reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += this.CardsPicturesHolder[this.i].Width;
                        this.CardsPicturesHolder[this.i].Visible = true;
                        this.Controls.Add(this.b5Panel);
                        this.b5Panel.Location = new Point(this.CardsPicturesHolder[10].Left - 10, this.CardsPicturesHolder[10].Top - 10);
                        this.b5Panel.BackColor = Color.DarkBlue;
                        this.b5Panel.Height = 150;
                        this.b5Panel.Width = 180;
                        this.b5Panel.Visible = false;
                        if (this.i == 11)
                        {
                            check = false;
                        }
                    }
                }

                if (this.i >= 12)
                {
                    this.CardsPicturesHolder[12].Tag = this.Reserve[12];
                    if (this.i > 12)
                    {
                        this.CardsPicturesHolder[13].Tag = this.Reserve[13];
                    }

                    if (this.i > 13)
                    {
                        this.CardsPicturesHolder[14].Tag = this.Reserve[14];
                    }

                    if (this.i > 14)
                    {
                        this.CardsPicturesHolder[15].Tag = this.Reserve[15];
                    }

                    if (this.i > 15)
                    {
                        this.CardsPicturesHolder[16].Tag = this.Reserve[16];
                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.CardsPicturesHolder[this.i] != null)
                    {
                        this.CardsPicturesHolder[this.i].Anchor = AnchorStyles.None;
                        this.CardsPicturesHolder[this.i].Image = backImage;

                        // CardsPicturesHolder[i].Image = Deck[i];
                        this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }



                if (this.bot1Chips <= 0)
                {
                    this.isBotOneFirstTurn = true;
                    this.CardsPicturesHolder[2].Visible = false;
                    this.CardsPicturesHolder[3].Visible = false;
                }
                else
                {
                    this.isBotOneFirstTurn = false;
                    if (this.i == 3)
                    {
                        if (this.CardsPicturesHolder[3] != null)
                        {
                            this.CardsPicturesHolder[2].Visible = true;
                            this.CardsPicturesHolder[3].Visible = true;
                        }
                    }
                }

                if (this.bot2Chips <= 0)
                {
                    this.isBotTwoFirstTurn = true;
                    this.CardsPicturesHolder[4].Visible = false;
                    this.CardsPicturesHolder[5].Visible = false;
                }
                else
                {
                    this.isBotTwoFirstTurn = false;
                    if (this.i == 5)
                    {
                        if (this.CardsPicturesHolder[5] != null)
                        {
                            this.CardsPicturesHolder[4].Visible = true;
                            this.CardsPicturesHolder[5].Visible = true;
                        }
                    }
                }

                if (this.bot3Chips <= 0)
                {
                    this.isBotThreeFirstTurn = true;
                    this.CardsPicturesHolder[6].Visible = false;
                    this.CardsPicturesHolder[7].Visible = false;
                }
                else
                {
                    this.isBotThreeFirstTurn = false;
                    if (this.i == 7)
                    {
                        if (this.CardsPicturesHolder[7] != null)
                        {
                            this.CardsPicturesHolder[6].Visible = true;
                            this.CardsPicturesHolder[7].Visible = true;
                        }
                    }
                }

                if (this.bot4Chips <= 0)
                {
                    this.isBotFourFirstTurn = true;
                    this.CardsPicturesHolder[8].Visible = false;
                    this.CardsPicturesHolder[9].Visible = false;
                }
                else
                {
                    this.isBotFourFirstTurn = false;
                    if (this.i == 9)
                    {
                        if (this.CardsPicturesHolder[9] != null)
                        {
                            this.CardsPicturesHolder[8].Visible = true;
                            this.CardsPicturesHolder[9].Visible = true;
                        }
                    }
                }

                if (this.bot5Chips <= 0)
                {
                    this.isBotFiveFirstTurn = true;
                    this.CardsPicturesHolder[10].Visible = false;
                    this.CardsPicturesHolder[11].Visible = false;
                }
                else
                {
                    this.isBotFiveFirstTurn = false;
                    if (this.i == 11)
                    {
                        if (this.CardsPicturesHolder[11] != null)
                        {
                            this.CardsPicturesHolder[10].Visible = true;
                            this.CardsPicturesHolder[11].Visible = true;
                        }
                    }
                }
                #endregion

                if (this.i == 16)
                {
                    if (!this.restart)
                    {
                        this.MaximizeBox = true;
                        this.MinimizeBox = true;
                    }

                    this.timer.Start();
                }
            }

            #region endgame logic
            if (this.foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                this.foldedPlayers = 5;
            }

            if (this.i == TotalCardsDealedPerHand)
            {
                this.bRaise.Enabled = true;
                this.botCall.Enabled = true;
                this.bRaise.Enabled = true;
                this.bRaise.Enabled = true;
                this.bFold.Enabled = true;
            }
            #endregion
        }

        // this method is added by me (iliyan) [not finished]
        private void InitializePlayerPictureCards(AnchorStyles anchorStyle, int horizontal, int vertical)
        {
            this.CardsPicturesHolder[this.i].Tag = this.Reserve[this.i];
            this.CardsPicturesHolder[this.i].Image = this.deckImages[this.i];
            this.CardsPicturesHolder[this.i].Anchor = anchorStyle;
            this.CardsPicturesHolder[this.i].Location = new Point(horizontal, vertical);
            this.CardsPicturesHolder[this.i].Visible = true;
        }

        // this method is added by me (iliyan)
        private void InitializePanel(Panel panel)
        {
            Controls.Add(panel);
            panel.Location = new Point(this.CardsPicturesHolder[this.i].Left - 10, this.CardsPicturesHolder[this.i].Top - 10);
            panel.BackColor = Color.DarkBlue;
            panel.Height = 150;
            panel.Width = 180;
            panel.Visible = false;
        }

        private async Task Turns()
        {
            if (!this.PFturn)
            {
                if (this.Pturn)
                {
                    this.FixCall(this.playerChipsStatus, ref this.playerCall, ref this.playerRaise, 1);

                    // MessageBox.Show("Player's turn");
                    this.pbTimer.Visible = true;
                    this.pbTimer.Value = 1000;
                    this.t = 60;
                    this.up = 10000000;
                    this.timer.Start();
                    this.bRaise.Enabled = true;
                    this.botCall.Enabled = true;
                    this.bRaise.Enabled = true;
                    this.bRaise.Enabled = true;
                    this.bFold.Enabled = true;
                    this.turnCount++;
                    this.FixCall(this.playerChipsStatus, ref this.playerCall, ref this.playerRaise, 2);
                }
            }

            if (this.PFturn || !this.Pturn)
            {
                await this.AllIn();
                if (this.PFturn && !this.isPlayerFolded)
                {
                    if (this.botCall.Text.Contains("All in") == false || this.bRaise.Text.Contains("All in") == false)
                    {
                        this.bools.RemoveAt(0);
                        this.bools.Insert(0, null);
                        this.maxLeft--;
                        this.isPlayerFolded = true;
                    }
                }

                await this.CheckRaise(0, 0);
                this.pbTimer.Visible = false;
                this.bRaise.Enabled = false;
                this.botCall.Enabled = false;
                this.bRaise.Enabled = false;
                this.bRaise.Enabled = false;
                this.bFold.Enabled = false;
                this.timer.Stop();
                this.isBotOneTurn = true;

                if (!this.isBotOneFirstTurn)
                {
                    if (this.isBotOneTurn)
                    {
                        this.FixCall(this.botOneActionStatus, ref this.botOneCall, ref this.botOneRaise, 1);
                        this.FixCall(this.botOneActionStatus, ref this.botOneCall, ref this.botOneRaise, 2);

                        this.Rules(2, 3, ref this.botOneType, ref this.botOnePower, this.isBotOneFirstTurn);
                        MessageBox.Show("Bot 1's turn");
                        this.AI(2, 3, ref this.bot1Chips, ref this.isBotOneTurn, ref this.isBotOneFirstTurn, this.botOneActionStatus, 0, this.botOnePower, this.botOneType);
                        this.turnCount++;
                        this.isBotOneTurn = false;
                        this.isBotTwoTurn = true;
                    }
                }

                if (this.isBotOneFirstTurn && !this.isBotOneFolded)
                {
                    this.bools.RemoveAt(1);
                    this.bools.Insert(1, null);
                    this.maxLeft--;
                    this.isBotOneFolded = true;
                }

                if (this.isBotOneFirstTurn || !this.isBotOneTurn)
                {
                    await this.CheckRaise(1, 1);
                    this.isBotTwoTurn = true;
                }

                if (!this.isBotTwoFirstTurn)
                {
                    if (this.isBotTwoTurn)
                    {
                        this.FixCall(this.botTwoActionStatus, ref this.botTwoCall, ref this.botTwoRaise, 1);
                        this.FixCall(this.botTwoActionStatus, ref this.botTwoCall, ref this.botTwoRaise, 2);
                        this.Rules(4, 5, ref this.botTwoType, ref this.botTwoPower, this.isBotTwoFirstTurn);
                        MessageBox.Show("Bot 2's turn");
                        this.AI(
                            4,
                            5,
                            ref this.bot2Chips,
                            ref this.isBotTwoTurn,
                            ref this.isBotTwoFirstTurn,
                            this.botTwoActionStatus,
                            1,
                            this.botTwoPower,
                            this.botTwoType);
                        this.turnCount++;
                        this.isBotTwoTurn = false;
                        this.isBotThreeTurn = true;
                    }
                }

                if (this.isBotTwoFirstTurn && !this.isBotTwoFolded)
                {
                    this.bools.RemoveAt(2);
                    this.bools.Insert(2, null);
                    this.maxLeft--;
                    this.isBotTwoFolded = true;
                }

                if (this.isBotTwoFirstTurn || !this.isBotTwoTurn)
                {
                    await this.CheckRaise(2, 2);
                    this.isBotThreeTurn = true;
                }

                if (!this.isBotThreeFirstTurn)
                {
                    if (this.isBotThreeTurn)
                    {
                        this.FixCall(this.botThreeActionStatus, ref this.botThreeCall, ref this.botThreeRaise, 1);
                        this.FixCall(this.botThreeActionStatus, ref this.botThreeCall, ref this.botThreeRaise, 2);
                        this.Rules(6, 7, ref this.botThreeType, ref this.botThreePower, this.isBotThreeFirstTurn);
                        MessageBox.Show("Bot 3's turn");
                        this.AI(6, 7, ref this.bot3Chips, ref this.isBotThreeTurn, ref this.isBotThreeFirstTurn, this.botThreeActionStatus, 2, this.botThreePower, this.botThreeType);
                        this.turnCount++;
                        this.isBotThreeTurn = false;
                        this.isBotFourTurn = true;
                    }
                }

                if (this.isBotThreeFirstTurn && !this.isBotThreeFolded)
                {
                    this.bools.RemoveAt(3);
                    this.bools.Insert(3, null);
                    this.maxLeft--;
                    this.isBotThreeFolded = true;
                }

                if (this.isBotThreeFirstTurn || !this.isBotThreeTurn)
                {
                    await this.CheckRaise(3, 3);
                    this.isBotFourTurn = true;
                }

                if (!this.isBotFourFirstTurn)
                {
                    if (this.isBotFourTurn)
                    {
                        this.FixCall(this.botFourActionStatus, ref this.botFourCall, ref this.botFourRaise, 1);
                        this.FixCall(this.botFourActionStatus, ref this.botFourCall, ref this.botFourRaise, 2);
                        this.Rules(8, 9, ref this.botFourType, ref this.botFourPower, this.isBotFourFirstTurn);
                        MessageBox.Show("Bot 4's turn");
                        this.AI(8, 9, ref this.bot4Chips, ref this.isBotFourTurn, ref this.isBotFourFirstTurn, this.botFourActionStatus, 3, this.botFourPower, this.botFourType);
                        this.turnCount++;
                        this.isBotFourTurn = false;
                        this.isFiveTwoTurn = true;
                    }
                }

                if (this.isBotFourFirstTurn && !this.isBotFourFolded)
                {
                    this.bools.RemoveAt(4);
                    this.bools.Insert(4, null);
                    this.maxLeft--;
                    this.isBotFourFolded = true;
                }

                if (this.isBotFourFirstTurn || !this.isBotFourTurn)
                {
                    await this.CheckRaise(4, 4);
                    this.isFiveTwoTurn = true;
                }

                if (!this.isBotFiveFirstTurn)
                {
                    if (this.isFiveTwoTurn)
                    {
                        this.FixCall(this.botFiveActionStatus, ref this.botFiveCall, ref this.b5Raise, 1);
                        this.FixCall(this.botFiveActionStatus, ref this.botFiveCall, ref this.b5Raise, 2);
                        this.Rules(10, 11, ref this.b5Type, ref this.botFivePower, this.isBotFiveFirstTurn);
                        MessageBox.Show("Bot 5's turn");
                        this.AI(10, 11, ref this.bot5Chips, ref this.isFiveTwoTurn, ref this.isBotFiveFirstTurn, this.botFiveActionStatus, 4, this.botFivePower, this.b5Type);
                        this.turnCount++;
                        this.isFiveTwoTurn = false;
                    }
                }

                if (this.isBotFiveFirstTurn && !this.isBotFiveFolded)
                {
                    this.bools.RemoveAt(5);
                    this.bools.Insert(5, null);
                    this.maxLeft--;
                    this.isBotFiveFolded = true;
                }

                if (this.isBotFiveFirstTurn || !this.isFiveTwoTurn)
                {
                    await this.CheckRaise(5, 5);
                    this.Pturn = true;
                }

                if (this.PFturn && !this.isPlayerFolded)
                {
                    if (this.botCall.Text.Contains("All in") == false || this.bRaise.Text.Contains("All in") == false)
                    {
                        this.bools.RemoveAt(0);
                        this.bools.Insert(0, null);
                        this.maxLeft--;
                        this.isPlayerFolded = true;
                    }
                }

                await this.AllIn();

                if (!this.restart)
                {
                    await this.Turns();
                }

                this.restart = false;
            }
        }

        private void Rules(int c1, int c2, ref double current, ref double Power, bool foldedTurn)
        {
            if (c1 == 0 && c2 == 1)
            {
            }

            if (!foldedTurn || c1 == 0 && c2 == 1 && this.playerChipsStatus.Text.Contains(Resources.FoldStr) == false)
            {


                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = this.Reserve[c1];
                Straight[1] = this.Reserve[c2];
                Straight1[0] = Straight[2] = this.Reserve[12];
                Straight1[1] = Straight[3] = this.Reserve[13];
                Straight1[2] = Straight[4] = this.Reserve[14];
                Straight1[3] = Straight[5] = this.Reserve[15];
                Straight1[4] = Straight[6] = this.Reserve[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);


                // TODO: NullRefferenceException occurs
                for (this.i = 0; this.i < 16; this.i++)
                {
                    if (this.Reserve[this.i] == int.Parse(this.CardsPicturesHolder[c1].Tag.ToString())
                        && this.Reserve[this.i + 1] == int.Parse(this.CardsPicturesHolder[c2].Tag.ToString()))
                    {
                        // Pair from Hand current = 1
                        this.rPairFromHand(ref current, ref Power);

                        #region Pair or Two Pair from Table current = 2 || 0

                        this.rPairTwoPair(ref current, ref Power);

                        #endregion

                        #region Two Pair current = 2

                        this.rTwoPair(ref current, ref Power);

                        #endregion

                        #region Three of a kind current = 3

                        this.rThreeOfAKind(ref current, ref Power, Straight);

                        #endregion

                        #region Straight current = 4

                        this.rStraight(ref current, ref Power, Straight);

                        #endregion

                        #region Flush current = 5 || 5.5

                        this.rFlush(ref current, ref Power, ref vf, Straight1);

                        #endregion

                        #region Full House current = 6

                        this.rFullHouse(ref current, ref Power, ref done, Straight);

                        #endregion

                        #region Four of a Kind current = 7

                        this.rFourOfAKind(ref current, ref Power, Straight);

                        #endregion

                        #region Straight Flush current = 8 || 9

                        this.rStraightFlush(ref current, ref Power, st1, st2, st3, st4);

                        #endregion

                        #region High Card current = -1

                        this.rHighCard(ref current, ref Power);

                        #endregion
                    }
                }
            }
        }

        private void rStraightFlush(ref double current, ref double Power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        Power = st1.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 8 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = st1.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 9 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = st2.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 8 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = st2.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 9 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = st3.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 8 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = st3.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 9 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = st4.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 8 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = st4.Max() / 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 9 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFourOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4
                        && Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        Power = (Straight[j] / 4) * 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 7 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0
                        && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 7 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFullHouse(ref double current, ref double Power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                this.type = Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                Power = 13 * 2 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 6 });
                                this.sorted =
                                    this.Win.OrderByDescending(op1 => op1.Current)
                                        .ThenByDescending(op1 => op1.Power)
                                        .First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 6 });
                                this.sorted =
                                    this.Win.OrderByDescending(op1 => op1.Current)
                                        .ThenByDescending(op1 => op1.Power)
                                        .First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (current != 6)
                {
                    Power = this.type;
                }
            }
        }

        private void rFlush(ref double current, ref double Power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.Reserve[this.i] % 4 == this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f1[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }

                        if (this.Reserve[this.i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else if (this.Reserve[this.i] / 4 < f1.Max() / 4 && this.Reserve[this.i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)
                {
                    // different cards in hand
                    if (this.Reserve[this.i] % 4 != this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f1[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }

                    if (this.Reserve[this.i + 1] % 4 != this.Reserve[this.i] % 4
                        && this.Reserve[this.i + 1] % 4 == f1[0] % 4)
                    {
                        if (this.Reserve[this.i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (this.Reserve[this.i] % 4 == f1[0] % 4 && this.Reserve[this.i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.Reserve[this.i + 1] % 4 == f1[0] % 4 && this.Reserve[this.i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i + 1] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.Reserve[this.i] / 4 < f1.Min() / 4 && this.Reserve[this.i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.Reserve[this.i] % 4 == this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f2[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }

                        if (this.Reserve[this.i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else if (this.Reserve[this.i] / 4 < f2.Max() / 4 && this.Reserve[this.i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)
                {
                    // different cards in hand
                    if (this.Reserve[this.i] % 4 != this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f2[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }

                    if (this.Reserve[this.i + 1] % 4 != this.Reserve[this.i] % 4
                        && this.Reserve[this.i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.Reserve[this.i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (this.Reserve[this.i] % 4 == f2[0] % 4 && this.Reserve[this.i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.Reserve[this.i + 1] % 4 == f2[0] % 4 && this.Reserve[this.i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i + 1] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.Reserve[this.i] / 4 < f2.Min() / 4 && this.Reserve[this.i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.Reserve[this.i] % 4 == this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f3[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }

                        if (this.Reserve[this.i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else if (this.Reserve[this.i] / 4 < f3.Max() / 4 && this.Reserve[this.i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)
                {
                    // different cards in hand
                    if (this.Reserve[this.i] % 4 != this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f3[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }

                    if (this.Reserve[this.i + 1] % 4 != this.Reserve[this.i] % 4
                        && this.Reserve[this.i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.Reserve[this.i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (this.Reserve[this.i] % 4 == f3[0] % 4 && this.Reserve[this.i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.Reserve[this.i + 1] % 4 == f3[0] % 4 && this.Reserve[this.i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i + 1] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.Reserve[this.i] / 4 < f3.Min() / 4 && this.Reserve[this.i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.Reserve[this.i] % 4 == this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f4[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }

                        if (this.Reserve[this.i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else if (this.Reserve[this.i] / 4 < f4.Max() / 4 && this.Reserve[this.i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)
                {
                    // different cards in hand
                    if (this.Reserve[this.i] % 4 != this.Reserve[this.i + 1] % 4
                        && this.Reserve[this.i] % 4 == f4[0] % 4)
                    {
                        if (this.Reserve[this.i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }

                    if (this.Reserve[this.i + 1] % 4 != this.Reserve[this.i] % 4
                        && this.Reserve[this.i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.Reserve[this.i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.Reserve[this.i + 1] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 5 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (this.Reserve[this.i] % 4 == f4[0] % 4 && this.Reserve[this.i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.Reserve[this.i + 1] % 4 == f4[0] % 4 && this.Reserve[this.i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.Reserve[this.i + 1] + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.Reserve[this.i] / 4 < f4.Min() / 4 && this.Reserve[this.i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                // ace
                if (f1.Length > 0)
                {
                    if (this.Reserve[this.i] / 4 == 0 && this.Reserve[this.i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.Reserve[this.i + 1] / 4 == 0 && this.Reserve[this.i + 1] % 4 == f1[0] % 4 && vf
                        && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (this.Reserve[this.i] / 4 == 0 && this.Reserve[this.i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.Reserve[this.i + 1] / 4 == 0 && this.Reserve[this.i + 1] % 4 == f2[0] % 4 && vf
                        && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (this.Reserve[this.i] / 4 == 0 && this.Reserve[this.i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.Reserve[this.i + 1] / 4 == 0 && this.Reserve[this.i + 1] % 4 == f3[0] % 4 && vf
                        && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (this.Reserve[this.i] / 4 == 0 && this.Reserve[this.i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.Reserve[this.i + 1] / 4 == 0 && this.Reserve[this.i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 5.5 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rStraight(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            Power = op.Max() + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 4 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 4 });
                            this.sorted =
                                this.Win.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        this.Win.Add(new Type { Power = Power, Current = 4 });
                        this.sorted =
                            this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rThreeOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            Power = 13 * 3 + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 3 });
                            this.sorted =
                                this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 3 });
                            this.sorted =
                                this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        private void rTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.Reserve[this.i] / 4 != this.Reserve[this.i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (this.Reserve[this.i] / 4 == this.Reserve[tc] / 4
                                    && this.Reserve[this.i + 1] / 4 == this.Reserve[tc - k] / 4
                                    || this.Reserve[this.i + 1] / 4 == this.Reserve[tc] / 4
                                    && this.Reserve[this.i] / 4 == this.Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.Reserve[this.i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.Reserve[this.i + 1] / 4) * 2 + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (this.Reserve[this.i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.Reserve[this.i] / 4) * 2 + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (this.Reserve[this.i + 1] / 4 != 0 && this.Reserve[this.i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.Reserve[this.i] / 4) * 2 + (this.Reserve[this.i + 1] / 4) * 2
                                                    + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (this.Reserve[tc] / 4 == this.Reserve[tc - k] / 4)
                            {
                                if (this.Reserve[tc] / 4 != this.Reserve[this.i] / 4
                                    && this.Reserve[tc] / 4 != this.Reserve[this.i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.Reserve[this.i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.Reserve[this.i] / 4) * 2 + 13 * 4 + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (this.Reserve[this.i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.Reserve[this.i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (this.Reserve[this.i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.Reserve[tc] / 4) * 2 + (this.Reserve[this.i + 1] / 4) * 2
                                                    + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (this.Reserve[this.i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.Reserve[tc] / 4) * 2 + (this.Reserve[this.i] / 4) * 2
                                                    + current * 100;
                                            this.Win.Add(new Type { Power = Power, Current = 2 });
                                            this.sorted =
                                                this.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.Reserve[this.i] / 4 > this.Reserve[this.i + 1] / 4)
                                        {
                                            if (this.Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.Reserve[this.i] / 4 + current * 100;
                                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                                this.sorted =
                                                    this.Win.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.Reserve[tc] / 4 + this.Reserve[this.i] / 4 + current * 100;
                                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                                this.sorted =
                                                    this.Win.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                        }
                                        else
                                        {
                                            if (this.Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.Reserve[this.i + 1] + current * 100;
                                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                                this.sorted =
                                                    this.Win.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.Reserve[tc] / 4 + this.Reserve[this.i + 1] / 4
                                                        + current * 100;
                                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                                this.sorted =
                                                    this.Win.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairFromHand(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (this.Reserve[this.i] / 4 == this.Reserve[this.i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (this.Reserve[this.i] / 4 == 0)
                        {
                            current = 1;
                            Power = 13 * 4 + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 1 });
                            this.sorted =
                                this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            Power = (this.Reserve[this.i + 1] / 4) * 4 + current * 100;
                            this.Win.Add(new Type { Power = Power, Current = 1 });
                            this.sorted =
                                this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.Reserve[this.i + 1] / 4 == this.Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.Reserve[this.i + 1] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.Reserve[this.i] / 4 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                this.sorted =
                                    this.Win.OrderByDescending(op => op.Current)
                                        .ThenByDescending(op => op.Power)
                                        .First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.Reserve[this.i + 1] / 4) * 4 + this.Reserve[this.i] / 4 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                this.sorted =
                                    this.Win.OrderByDescending(op => op.Current)
                                        .ThenByDescending(op => op.Power)
                                        .First();
                            }
                        }

                        msgbox = true;
                    }

                    if (this.Reserve[this.i] / 4 == this.Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.Reserve[this.i] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.Reserve[this.i + 1] / 4 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                this.sorted =
                                    this.Win.OrderByDescending(op => op.Current)
                                        .ThenByDescending(op => op.Power)
                                        .First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.Reserve[tc] / 4) * 4 + this.Reserve[this.i + 1] / 4 + current * 100;
                                this.Win.Add(new Type { Power = Power, Current = 1 });
                                this.sorted =
                                    this.Win.OrderByDescending(op => op.Current)
                                        .ThenByDescending(op => op.Power)
                                        .First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        private void rHighCard(ref double current, ref double Power)
        {
            if (current == -1)
            {
                if (this.Reserve[this.i] / 4 > this.Reserve[this.i + 1] / 4)
                {
                    current = -1;
                    Power = this.Reserve[this.i] / 4;
                    this.Win.Add(new Type { Power = Power, Current = -1 });
                    this.sorted =
                        this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    Power = this.Reserve[this.i + 1] / 4;
                    this.Win.Add(new Type { Power = Power, Current = -1 });
                    this.sorted =
                        this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (this.Reserve[this.i] / 4 == 0 || this.Reserve[this.i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    this.Win.Add(new Type { Power = Power, Current = -1 });
                    this.sorted =
                        this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        private void Winner(double current, double Power, string currentText, string lastly)
        {
            if (lastly == " ")
            {
                lastly = Resources.BotFiveStr;
            }

            for (int j = 0; j <= 16; j++)
            {
                // await Task.Delay(5);
                if (this.CardsPicturesHolder[j].Visible)
                {
                    this.CardsPicturesHolder[j].Image = this.deckImages[j];
                }
            }

            if (current == this.sorted.Current)
            {
                if (Power == this.sorted.Power)
                {
                    this.winners++;
                    this.CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    else if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    else if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    else if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    else if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    else if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    else if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    else if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    else if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    else if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }

            if (currentText == lastly)
            {
                // lastfixed
                if (this.winners > 1)
                {
                    if (this.CheckWinners.Contains(Resources.PlayerStr))
                    {
                        this.startingChipsDefault += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbPlayerChips.Text = this.startingChipsDefault.ToString();

                        pPanel.Visible = true;
                    }
                    else if (this.CheckWinners.Contains(Resources.BotOneStr))
                    {
                        this.bot1Chips += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbBotChips1.Text = this.bot1Chips.ToString();

                        botOnePanel.Visible = true;
                    }
                    else if (this.CheckWinners.Contains(Resources.BotTwoStr))
                    {
                        this.bot2Chips += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbBotChips2.Text = this.bot2Chips.ToString();

                        botTwoPanel.Visible = true;
                    }
                    else if (this.CheckWinners.Contains(Resources.BotThreeStr))
                    {
                        this.bot3Chips += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbBotChips3.Text = this.bot3Chips.ToString();

                        botThreePanel.Visible = true;
                    }
                    else if (this.CheckWinners.Contains(Resources.BotFourStr))
                    {
                        this.bot4Chips += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbBotChips4.Text = this.bot4Chips.ToString();

                        botFourPanel.Visible = true;
                    }
                    else if (this.CheckWinners.Contains(Resources.BotFiveStr))
                    {
                        this.bot5Chips += int.Parse(this.tbTotalPot.Text) / this.winners;
                        this.tbBotChips5.Text = this.bot5Chips.ToString();

                        b5Panel.Visible = true;
                    }
                    Thread.Sleep(2500);
                    //await Finish(1);
                }

                if (this.winners == 1)
                {
                    if (this.CheckWinners.Contains(Resources.PlayerStr))
                    {
                        this.startingChipsDefault += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        pPanel.Visible = true;
                    }

                    if (this.CheckWinners.Contains(Resources.BotOneStr))
                    {
                        this.bot1Chips += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        botOnePanel.Visible = true;
                    }

                    if (this.CheckWinners.Contains(Resources.BotTwoStr))
                    {
                        this.bot2Chips += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        botTwoPanel.Visible = true;
                    }

                    if (this.CheckWinners.Contains(Resources.BotThreeStr))
                    {
                        this.bot3Chips += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        botThreePanel.Visible = true;
                    }

                    if (this.CheckWinners.Contains(Resources.BotFourStr))
                    {
                        this.bot4Chips += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        botFourPanel.Visible = true;
                    }

                    if (this.CheckWinners.Contains(Resources.BotFiveStr))
                    {
                        this.bot5Chips += int.Parse(this.tbTotalPot.Text);

                        // await Finish(1);
                        b5Panel.Visible = true;
                    }
                    Thread.Sleep(2500);
                }
            }
        }

        private async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (this.raising)
            {
                this.turnCount = 0;
                this.raising = false;
                this.raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= this.maxLeft - 1 || !this.changed && this.turnCount == this.maxLeft)
                {
                    if (currentTurn == this.raisedTurn - 1 || !this.changed && this.turnCount == this.maxLeft
                        || this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.call = 0;
                        this.raisedTurn = 123;
                        this.rounds++;

                        if (!this.PFturn)
                        {
                            this.playerChipsStatus.Text = string.Empty;
                        }

                        if (!this.isBotOneFirstTurn)
                        {
                            this.botOneActionStatus.Text = string.Empty;
                        }

                        if (!this.isBotTwoFirstTurn)
                        {
                            this.botTwoActionStatus.Text = string.Empty;
                        }

                        if (!this.isBotThreeFirstTurn)
                        {
                            this.botThreeActionStatus.Text = string.Empty;
                        }

                        if (!this.isBotFourFirstTurn)
                        {
                            this.botFourActionStatus.Text = string.Empty;
                        }

                        if (!this.isBotFiveFirstTurn)
                        {
                            this.botFiveActionStatus.Text = string.Empty;
                        }
                    }
                }
            }

            if (this.rounds == this.flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.CardsPicturesHolder[j].Image != this.deckImages[j])
                    {
                        this.CardsPicturesHolder[j].Image = this.deckImages[j];

                        this.playerCall = 0;
                        this.playerRaise = 0;
                        this.botOneCall = 0;
                        this.botOneRaise = 0;
                        this.botTwoCall = 0;
                        this.botTwoRaise = 0;
                        this.botThreeCall = 0;
                        this.botThreeRaise = 0;
                        this.botFourCall = 0;
                        this.botFourRaise = 0;
                        this.botFiveCall = 0;
                        this.b5Raise = 0;
                    }
                }
            }

            if (this.rounds == this.turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.CardsPicturesHolder[j].Image != this.deckImages[j])
                    {
                        this.CardsPicturesHolder[j].Image = this.deckImages[j];
                        this.playerCall = 0;
                        this.playerRaise = 0;
                        this.botOneCall = 0;
                        this.botOneRaise = 0;
                        this.botTwoCall = 0;
                        this.botTwoRaise = 0;
                        this.botThreeCall = 0;
                        this.botThreeRaise = 0;
                        this.botFourCall = 0;
                        this.botFourRaise = 0;
                        this.botFiveCall = 0;
                        this.b5Raise = 0;
                    }
                }
            }

            if (this.rounds == this.river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.CardsPicturesHolder[j].Image != this.deckImages[j])
                    {
                        this.CardsPicturesHolder[j].Image = this.deckImages[j];
                        this.playerCall = 0;
                        this.playerRaise = 0;
                        this.botOneCall = 0;
                        this.botOneRaise = 0;
                        this.botTwoCall = 0;
                        this.botTwoRaise = 0;
                        this.botThreeCall = 0;
                        this.botThreeRaise = 0;
                        this.botFourCall = 0;
                        this.botFourRaise = 0;
                        this.botFiveCall = 0;
                        this.b5Raise = 0;
                    }
                }
            }

            if (this.rounds == this.end && this.maxLeft == 6)
            {
                string fixedLast = "qwerty";

                if (!this.playerChipsStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.PlayerStr;
                    this.Rules(0, 1, ref this.playerType, ref this.playerPower, this.PFturn);
                }

                if (!this.botOneActionStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.BotOneStr;
                    this.Rules(2, 3, ref this.botOneType, ref this.botOnePower, this.isBotOneFirstTurn);
                }

                if (!this.botTwoActionStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.BotTwoStr;
                    this.Rules(4, 5, ref this.botTwoType, ref this.botTwoPower, this.isBotTwoFirstTurn);
                }

                if (!this.botThreeActionStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.BotThreeStr;
                    this.Rules(6, 7, ref this.botThreeType, ref this.botThreePower, this.isBotThreeFirstTurn);
                }

                if (!this.botFourActionStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.BotFourStr;
                    this.Rules(8, 9, ref this.botFourType, ref this.botFourPower, this.isBotFourFirstTurn);
                }

                if (!this.botFiveActionStatus.Text.Contains(Resources.FoldStr))
                {
                    fixedLast = Resources.BotFiveStr;
                    this.Rules(10, 11, ref this.b5Type, ref this.botFivePower, this.isBotFiveFirstTurn);
                }

                this.Winner(this.playerType, this.playerPower, Resources.PlayerStr, fixedLast);
                this.Winner(this.botOneType, this.botOnePower, Resources.BotOneStr, fixedLast);
                this.Winner(this.botTwoType, this.botTwoPower, Resources.BotTwoStr, fixedLast);
                this.Winner(this.botThreeType, this.botThreePower, Resources.BotThreeStr, fixedLast);
                this.Winner(this.botFourType, this.botFourPower, Resources.BotFourStr, fixedLast);
                this.Winner(this.b5Type, this.botFivePower, Resources.BotFiveStr, fixedLast);

                this.restart = true;
                this.Pturn = true;
                this.PFturn = false;
                this.isBotOneFirstTurn = false;
                this.isBotTwoFirstTurn = false;
                this.isBotThreeFirstTurn = false;
                this.isBotFourFirstTurn = false;
                this.isBotFiveFirstTurn = false;

                if (this.startingChipsDefault <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        this.startingChipsDefault = f2.a;
                        this.bot1Chips += f2.a;
                        this.bot2Chips += f2.a;
                        this.bot3Chips += f2.a;
                        this.bot4Chips += f2.a;
                        this.bot5Chips += f2.a;
                        this.PFturn = false;
                        this.Pturn = true;
                        this.bRaise.Enabled = true;
                        this.bFold.Enabled = true;
                        this.bCheck.Enabled = true;
                        this.bRaise.Text = "raise";
                    }
                }

                this.pPanel.Visible = false;
                this.botOnePanel.Visible = false;
                this.botTwoPanel.Visible = false;
                this.botThreePanel.Visible = false;
                this.botFourPanel.Visible = false;
                this.b5Panel.Visible = false;

                this.playerCall = 0;
                this.playerRaise = 0;
                this.botOneCall = 0;
                this.botOneRaise = 0;
                this.botTwoCall = 0;
                this.botTwoRaise = 0;
                this.botThreeCall = 0;
                this.botThreeRaise = 0;
                this.botFourCall = 0;
                this.botFourRaise = 0;
                this.botFiveCall = 0;
                this.b5Raise = 0;
                this.call = this.bb;
                this.raise = 0;

                this.cardsImageLocations = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

                this.bools.Clear();

                this.rounds = 0;
                this.playerPower = 0;
                this.playerType = -1;
                this.type = 0;
                this.botOnePower = 0;
                this.botTwoPower = 0;
                this.botThreePower = 0;
                this.botFourPower = 0;
                this.botFivePower = 0;
                this.botOneType = -1;
                this.botTwoType = -1;
                this.botThreeType = -1;
                this.botFourType = -1;
                this.b5Type = -1;

                this.ints.Clear();
                this.CheckWinners.Clear();

                this.winners = 0;
                this.Win.Clear();
                this.sorted.Current = 0;
                this.sorted.Power = 0;

                for (int os = 0; os < TotalCardsDealedPerHand; os++)
                {
                    this.CardsPicturesHolder[os].Image = null;
                    this.CardsPicturesHolder[os].Invalidate();
                    this.CardsPicturesHolder[os].Visible = false;
                }

                this.tbTotalPot.Text = "0";
                this.playerChipsStatus.Text = string.Empty;
                await this.Shuffle();
                await this.Turns();
            }
        }

        private void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }

                    if (status.Text.Contains(Resources.CallStr))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }

                    if (status.Text.Contains(Resources.CheckStr))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }

                if (options == 2)
                {
                    if (cRaise != this.raise && cRaise <= this.raise)
                    {
                        this.call = Convert.ToInt32(this.raise) - cRaise;
                    }

                    if (cCall != this.call || cCall <= this.call)
                    {
                        this.call = this.call - cCall;
                    }

                    if (cRaise == this.raise && this.raise > 0)
                    {
                        this.call = 0;
                        this.botCall.Enabled = false;
                        this.botCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        private async Task AllIn()
        {
            if (this.startingChipsDefault <= 0 && !this.intsadded)
            {
                if (this.playerChipsStatus.Text.Contains(Resources.RaiseStr))
                {
                    this.ints.Add(this.startingChipsDefault);
                    this.intsadded = true;
                }

                if (this.playerChipsStatus.Text.Contains(Resources.CallStr))
                {
                    this.ints.Add(this.startingChipsDefault);
                    this.intsadded = true;
                }
            }

            this.intsadded = false;
            if (this.bot1Chips <= 0 && !this.isBotOneFirstTurn)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.bot1Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }

            if (this.bot2Chips <= 0 && !this.isBotTwoFirstTurn)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.bot2Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }

            if (this.bot3Chips <= 0 && !this.isBotThreeFirstTurn)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.bot3Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }

            if (this.bot4Chips <= 0 && !this.isBotFourFirstTurn)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.bot4Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }

            if (this.bot5Chips <= 0 && !this.isBotFiveFirstTurn)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.bot5Chips);
                    this.intsadded = true;
                }
            }

            if (this.ints.ToArray().Length == this.maxLeft)
            {
                await this.Finish(2);
            }
            else
            {
                this.ints.Clear();
            }

            var abc = this.bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = this.bools.IndexOf(false);
                if (index == 0)
                {
                    this.startingChipsDefault += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.startingChipsDefault.ToString();
                    this.pPanel.Visible = true;
                    MessageBox.Show(Resources.MBox_PlayerW);
                }
                else if (index == 1)
                {
                    this.bot1Chips += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.bot1Chips.ToString();
                    this.botOnePanel.Visible = true;
                    MessageBox.Show(Resources.MBox_Bot1W);
                }
                else if (index == 2)
                {
                    this.bot2Chips += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.bot2Chips.ToString();
                    this.botTwoPanel.Visible = true;
                    MessageBox.Show(Resources.MBox_Bot2Win);
                }
                else if (index == 3)
                {
                    this.bot3Chips += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.bot3Chips.ToString();
                    this.botThreePanel.Visible = true;
                    MessageBox.Show(Resources.MBox_Bot3W);
                }
                else if (index == 4)
                {
                    this.bot4Chips += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.bot4Chips.ToString();
                    this.botFourPanel.Visible = true;
                    MessageBox.Show(Resources.MBox_Bot4W);
                }
                else if (index == 5)
                {
                    this.bot5Chips += int.Parse(this.tbTotalPot.Text);
                    this.tbPlayerChips.Text = this.bot5Chips.ToString();
                    this.b5Panel.Visible = true;
                    MessageBox.Show(Resources.MBox_Bot5W);
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.CardsPicturesHolder[j].Visible = false;
                }

                await this.Finish(1);
            }

            this.intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && this.rounds >= this.end)
            {
                await this.Finish(2);
            }
            #endregion
        }

        private async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            this.pPanel.Visible = false;
            this.botOnePanel.Visible = false;
            this.botTwoPanel.Visible = false;
            this.botThreePanel.Visible = false;
            this.botFourPanel.Visible = false;
            this.b5Panel.Visible = false;

            this.call = this.bb;
            this.raise = 0;
            this.foldedPlayers = 5;
            this.type = 0;
            this.rounds = 0;
            this.botOnePower = 0;
            this.botTwoPower = 0;
            this.botThreePower = 0;
            this.botFourPower = 0;
            this.botFivePower = 0;
            this.playerPower = 0;
            this.playerType = -1;
            this.raise = 0;
            this.botOneType = -1;
            this.botTwoType = -1;
            this.botThreeType = -1;
            this.botFourType = -1;
            this.b5Type = -1;

            this.isBotOneTurn = false;
            this.isBotTwoTurn = false;
            this.isBotThreeTurn = false;
            this.isBotFourTurn = false;
            this.isFiveTwoTurn = false;
            this.isBotOneFirstTurn = false;
            this.isBotTwoFirstTurn = false;
            this.isBotThreeFirstTurn = false;
            this.isBotFourFirstTurn = false;
            this.isBotFiveFirstTurn = false;
            this.isPlayerFolded = false;
            this.isBotOneFolded = false;
            this.isBotTwoFolded = false;
            this.isBotThreeFolded = false;
            this.isBotFourFolded = false;
            this.isBotFiveFolded = false;
            this.PFturn = false;
            this.Pturn = true;
            this.restart = false;
            this.raising = false;

            this.playerCall = 0;
            this.botOneCall = 0;
            this.botTwoCall = 0;
            this.botThreeCall = 0;
            this.botFourCall = 0;
            this.botFiveCall = 0;
            this.playerRaise = 0;
            this.botOneRaise = 0;
            this.botTwoRaise = 0;
            this.botThreeRaise = 0;
            this.botFourRaise = 0;
            this.b5Raise = 0;
            this.height = 0;
            this.width = 0;
            this.winners = 0;
            this.flop = 1;
            this.turn = 2;
            this.river = 3;
            this.end = 4;
            this.maxLeft = 6;
            this.raisedTurn = 1;

            this.bools.Clear();
            this.CheckWinners.Clear();
            this.ints.Clear();
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.tbTotalPot.Text = "0";
            this.t = 60;
            this.up = 10000000;
            this.turnCount = 0;
            this.playerChipsStatus.Text = string.Empty;
            this.botOneActionStatus.Text = string.Empty;
            this.botTwoActionStatus.Text = string.Empty;
            this.botThreeActionStatus.Text = string.Empty;
            this.botFourActionStatus.Text = string.Empty;
            this.botFiveActionStatus.Text = string.Empty;

            if (this.startingChipsDefault <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    this.startingChipsDefault = f2.a;
                    this.bot1Chips += f2.a;
                    this.bot2Chips += f2.a;
                    this.bot3Chips += f2.a;
                    this.bot4Chips += f2.a;
                    this.bot5Chips += f2.a;
                    this.PFturn = false;
                    this.Pturn = true;
                    this.bRaise.Enabled = true;
                    this.bFold.Enabled = true;
                    this.bCheck.Enabled = true;
                    this.bRaise.Text = Resources.RaiseStr;
                }
            }

            this.cardsImageLocations = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

            for (int os = 0; os < TotalCardsDealedPerHand; os++)
            {
                this.CardsPicturesHolder[os].Image = null;
                this.CardsPicturesHolder[os].Invalidate();
                this.CardsPicturesHolder[os].Visible = false;
            }

            await this.Shuffle();

            // await Turns();
        }

        private void FixWinners()
        {
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            string fixedLast = null;
            if (!this.playerChipsStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.PlayerStr;
                this.Rules(0, 1, ref this.playerType, ref this.playerPower, this.PFturn);
            }

            if (!this.botOneActionStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.BotOneStr;
                this.Rules(2, 3, ref this.botOneType, ref this.botOnePower, this.isBotOneFirstTurn);
            }

            if (!this.botTwoActionStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.BotTwoStr;
                this.Rules(4, 5, ref this.botTwoType, ref this.botTwoPower, this.isBotTwoFirstTurn);
            }

            if (!this.botThreeActionStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.BotThreeStr;
                this.Rules(6, 7, ref this.botThreeType, ref this.botThreePower, this.isBotThreeFirstTurn);
            }

            if (!this.botFourActionStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.BotFourStr;
                this.Rules(8, 9, ref this.botFourType, ref this.botFourPower, this.isBotFourFirstTurn);
            }

            if (!this.botFiveActionStatus.Text.Contains(Resources.FoldStr))
            {
                fixedLast = Resources.BotFiveStr;
                this.Rules(10, 11, ref this.b5Type, ref this.botFivePower, this.isBotFiveFirstTurn);
            }

            this.Winner(this.playerType, this.playerPower, Resources.PlayerStr, fixedLast);
            this.Winner(this.botOneType, this.botOnePower, Resources.BotOneStr, fixedLast);
            this.Winner(this.botTwoType, this.botTwoPower, Resources.BotTwoStr, fixedLast);
            this.Winner(this.botThreeType, this.botThreePower, Resources.BotThreeStr, fixedLast);
            this.Winner(this.botFourType, this.botFourPower, Resources.BotFourStr, fixedLast);
            this.Winner(this.b5Type, this.botFivePower, Resources.BotFiveStr, fixedLast);
        }

        private void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    this.HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus);
                }

                if (botCurrent == 0)
                {
                    this.PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus);
                }

                if (botCurrent == 1)
                {
                    this.PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 2)
                {
                    this.TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 3)
                {
                    this.ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 4)
                {
                    this.Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    this.Flush(ref sChips, ref sTurn, ref sFTurn, sStatus);
                }

                if (botCurrent == 6)
                {
                    this.FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }

                if (botCurrent == 7)
                {
                    this.FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name);
                }

                if (botCurrent == 8 || botCurrent == 9)
                {
                    this.StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name);
                }
            }

            if (sFTurn)
            {
                this.CardsPicturesHolder[c1].Visible = false;
                this.CardsPicturesHolder[c2].Visible = false;
            }
        }

        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            this.HP(ref sChips, ref sTurn, ref sFTurn, sStatus, 20, 25);
        }

        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            this.HP(ref sChips, ref sTurn, ref sFTurn, sStatus, 16, 25);
        }

        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int rCall = this.random.Next(10, 16);
            int rRaise = this.random.Next(10, 13);

            if (botPower <= 199 && botPower >= 140)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }

            if (botPower <= 139 && botPower >= 128)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }

            if (botPower < 128 && botPower >= 101)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }

        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int rCall = this.random.Next(6, 11);
            int rRaise = this.random.Next(6, 11);

            if (botPower <= 290 && botPower >= 246)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }

            if (botPower <= 244 && botPower >= 234)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }

            if (botPower < 234 && botPower >= 201)
            {
                this.PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int tCall = random.Next(3, 7);

            if (botPower <= 390 && botPower >= 330)
            {
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, tCall);
            }

            if (botPower <= 327 && botPower >= 321)
            {
                // 10  8
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, tCall);
            }

            if (botPower < 321 && botPower >= 303)
            {
                // 7 2
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, tCall);
            }
        }

        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int sCall = random.Next(3, 6);

            if (botPower <= 480 && botPower >= 410)
            {
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, sCall);
            }

            if (botPower <= 409 && botPower >= 407)
            {
                // 10  8
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, sCall);
            }

            if (botPower < 407 && botPower >= 404)
            {
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, sCall);
            }
        }

        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            int fCall = random.Next(2, 6);
            this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, fCall);
        }

        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int fhCall = random.Next(1, 5);

            if (botPower <= 626 && botPower >= 620)
            {
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, fhCall);
            }

            if (botPower < 620 && botPower >= 602)
            {
                this.Smooth(sChips, ref sTurn, ref sFTurn, sStatus, fhCall);
            }
        }

        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int fkCall = random.Next(1, 4);

            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(sChips, ref sTurn, ref sFTurn, sStatus, fkCall);
            }
        }

        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            int sfCall = random.Next(1, 3);

            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(sChips, ref sTurn, ref sFTurn, sStatus, sfCall);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = Resources.FoldStr;
            sTurn = false;
            sFTurn = true;
        }

        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = Resources.CheckStr;
            cTurn = false;
            this.raising = false;
        }

        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            this.raising = false;
            sTurn = false;
            sChips -= this.call;
            sStatus.Text = Resources.CallStr + this.call;
            this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + this.call).ToString();
        }

        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(this.raise);
            sStatus.Text = Resources.RaiseStr + this.raise;
            this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + Convert.ToInt32(this.raise)).ToString();
            this.call = Convert.ToInt32(this.raise);
            this.raising = true;
            sTurn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1)
        {
            int rnd = random.Next(1, 4);

            if (this.call <= 0)
            {
                this.Check(ref sTurn, sStatus);
            }

            if (this.call > 0)
            {
                if (rnd == 1)
                {
                    if (this.call <= RoundN(sChips, n))
                    {
                        this.Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }

                if (rnd == 2)
                {
                    if (this.call <= RoundN(sChips, n1))
                    {
                        this.Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (rnd == 3)
            {
                if (this.raise == 0)
                {
                    this.raise = this.call * 2;
                    this.Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (this.raise <= RoundN(sChips, n))
                    {
                        this.raise = this.call * 2;
                        this.Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        private void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            int rnd = random.Next(1, 3);

            if (this.rounds < 2)
            {
                if (this.call <= 0)
                {
                    this.Check(ref sTurn, sStatus);
                }

                if (this.call > 0)
                {
                    if (this.call >= RoundN(sChips, n1))
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n))
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (this.call >= RoundN(sChips, n) && this.call <= RoundN(sChips, n1))
                        {
                            this.Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n) && this.raise >= RoundN(sChips, n) / 2)
                        {
                            this.Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(sChips, n);
                                this.Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                this.raise = this.call * 2;
                                this.Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
            }

            if (this.rounds >= 2)
            {
                if (this.call > 0)
                {
                    if (this.call >= RoundN(sChips, n1 - rnd))
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n - rnd))
                    {
                        this.Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (this.call >= RoundN(sChips, n - rnd) && this.call <= RoundN(sChips, n1 - rnd))
                        {
                            this.Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n - rnd) && this.raise >= RoundN(sChips, n - rnd) / 2)
                        {
                            this.Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n - rnd) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(sChips, n - rnd);
                                this.Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                this.raise = this.call * 2;
                                this.Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }

                if (this.call <= 0)
                {
                    this.raise = RoundN(sChips, r - rnd);
                    this.Raised(ref sChips, ref sTurn, sStatus);
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        private void Smooth(int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int n)
        {
            if (this.call <= 0)
            {
                this.Check(ref botTurn, botStatus);
            }
            else
            {
                if (this.call >= RoundN(botChips, n))
                {
                    if (botChips > this.call)
                    {
                        this.Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= this.call)
                    {
                        this.raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = Resources.CallStr + botChips;
                        this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (this.raise > 0)
                    {
                        if (botChips >= this.raise * 2)
                        {
                            this.raise *= 2;
                            this.Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            this.Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        this.raise = this.call * 2;
                        this.Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }

            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }


        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (this.pbTimer.Value <= 0)
            {
                this.PFturn = true;
                await this.Turns();
            }

            if (this.t > 0)
            {
                this.t--;
                //TODO remove the magic number
                this.pbTimer.Value = (this.t / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.startingChipsDefault <= 0)
            {
                this.tbPlayerChips.Text = Resources.ChipsZero;
            }

            if (this.bot1Chips <= 0)
            {
                this.tbBotChips1.Text = Resources.ChipsZero;
            }

            if (this.bot2Chips <= 0)
            {
                this.tbBotChips2.Text = Resources.ChipsZero;
            }

            if (this.bot3Chips <= 0)
            {
                this.tbBotChips3.Text = Resources.ChipsZero;
            }

            if (this.bot4Chips <= 0)
            {
                this.tbBotChips4.Text = Resources.ChipsZero;
            }

            if (this.bot5Chips <= 0)
            {
                this.tbBotChips5.Text = Resources.ChipsZero;
            }

            this.tbPlayerChips.Text = Resources.Chips + this.startingChipsDefault;
            this.tbBotChips1.Text = Resources.Chips + this.bot1Chips;
            this.tbBotChips2.Text = Resources.Chips + this.bot2Chips;
            this.tbBotChips3.Text = Resources.Chips + this.bot3Chips;
            this.tbBotChips4.Text = Resources.Chips + this.bot4Chips;
            this.tbBotChips5.Text = Resources.Chips + this.bot5Chips;

            if (this.startingChipsDefault <= 0)
            {
                this.Pturn = false;
                this.PFturn = true;
                this.botCall.Enabled = false;
                this.bRaise.Enabled = false;
                this.bFold.Enabled = false;
                this.bCheck.Enabled = false;
            }

            if (this.up > 0)
            {
                this.up--;
            }

            if (this.startingChipsDefault >= this.call)
            {
                this.botCall.Text = Resources.CallStr + this.call;
            }
            else
            {
                this.botCall.Text = Resources.AllInStr;
                this.bRaise.Enabled = false;
            }

            if (this.call > 0)
            {
                this.bCheck.Enabled = false;
            }

            if (this.call <= 0)
            {
                this.bCheck.Enabled = true;
                this.botCall.Text = Resources.CallStr;
                this.botCall.Enabled = false;
            }

            if (this.startingChipsDefault <= 0)
            {
                this.bRaise.Enabled = false;
            }

            int parsedValue;

            if (this.tbRaise.Text != string.Empty && int.TryParse(this.tbRaise.Text, out parsedValue))
            {
                if (this.startingChipsDefault <= int.Parse(this.tbRaise.Text))
                {
                    this.bRaise.Text = Resources.AllInStr;
                }
                else
                {
                    this.bRaise.Text = Resources.RaiseStr;
                }
            }

            if (this.startingChipsDefault < this.call)
            {
                this.bRaise.Enabled = false;
            }
        }

        private async void BotFoldClick(object sender, EventArgs e)
        {
            this.playerChipsStatus.Text = Resources.FoldStr;
            this.Pturn = false;
            this.PFturn = true;
            await this.Turns();
        }

        private async void BotCheckClick(object sender, EventArgs e)
        {
            if (this.call <= 0)
            {
                this.Pturn = false;
                this.playerChipsStatus.Text = Resources.CheckStr;
            }
            else
            {
                // playerChipsStatus.Text = "All in " + startingChipsDefault;
                this.bCheck.Enabled = false;
            }

            await this.Turns();
        }

        private async void BotCallClick(object sender, EventArgs e)
        {
            this.Rules(0, 1, ref this.playerType, ref this.playerPower, this.PFturn);
            if (this.startingChipsDefault >= this.call)
            {
                this.startingChipsDefault -= this.call;
                this.tbPlayerChips.Text = Resources.Chips + this.startingChipsDefault;

                if (this.tbTotalPot.Text != string.Empty)
                {
                    this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + this.call).ToString();
                }
                else
                {
                    this.tbTotalPot.Text = this.call.ToString();
                }

                this.Pturn = false;
                this.playerChipsStatus.Text = Resources.CallStr + this.call;
                this.playerCall = this.call;
            }
            else if (this.startingChipsDefault <= this.call && this.call > 0)
            {
                this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + this.startingChipsDefault).ToString();
                this.playerChipsStatus.Text = Resources.AllInStr + this.startingChipsDefault;
                this.startingChipsDefault = 0;
                this.tbPlayerChips.Text = Resources.Chips + this.startingChipsDefault;
                this.Pturn = false;
                this.bFold.Enabled = false;
                this.playerCall = this.startingChipsDefault;
            }

            await this.Turns();
        }

        private async void BotRaiseClick(object sender, EventArgs e)
        {
            this.Rules(0, 1, ref this.playerType, ref this.playerPower, this.PFturn);
            int parsedValue;

            if (this.tbRaise.Text != string.Empty && int.TryParse(this.tbRaise.Text, out parsedValue))
            {
                if (this.startingChipsDefault > this.call)
                {
                    if (this.raise * 2 > int.Parse(this.tbRaise.Text))
                    {
                        this.tbRaise.Text = (this.raise * 2).ToString();
                        MessageBox.Show(Resources.MBox_RaiseTwice);
                        return;
                    }

                    if (this.startingChipsDefault >= int.Parse(this.tbRaise.Text))
                    {
                        this.call = int.Parse(this.tbRaise.Text);
                        this.raise = int.Parse(this.tbRaise.Text);
                        this.playerChipsStatus.Text = Resources.RaiseStr + this.call;
                        this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + this.call).ToString();
                        this.botCall.Text = Resources.CallStr;
                        this.startingChipsDefault -= int.Parse(this.tbRaise.Text);
                        this.raising = true;
                        this.playerRaise = Convert.ToInt32(this.raise);
                    }
                    else
                    {
                        this.call = this.startingChipsDefault;
                        this.raise = this.startingChipsDefault;
                        this.tbTotalPot.Text = (int.Parse(this.tbTotalPot.Text) + this.startingChipsDefault).ToString();
                        this.playerChipsStatus.Text = Resources.RaiseStr + this.call;
                        this.startingChipsDefault = 0;
                        this.raising = true;
                        this.playerRaise = Convert.ToInt32(this.raise);
                    }
                }
            }
            else
            {
                MessageBox.Show(Resources.MBox_NumberField);
                return;
            }

            this.Pturn = false;
            await this.Turns();
        }

        private void BotAddClick(object sender, EventArgs e)
        {
            if (this.tbAddChips.Text != string.Empty)
            {
                this.startingChipsDefault += int.Parse(this.tbAddChips.Text);
                this.bot1Chips += int.Parse(this.tbAddChips.Text);
                this.bot2Chips += int.Parse(this.tbAddChips.Text);
                this.bot3Chips += int.Parse(this.tbAddChips.Text);
                this.bot4Chips += int.Parse(this.tbAddChips.Text);
                this.bot5Chips += int.Parse(this.tbAddChips.Text);
            }

            this.tbPlayerChips.Text = Resources.Chips + this.startingChipsDefault;
        }

        private void BotOptionsClick(object sender, EventArgs e)
        {
            this.tbBB.Text = this.bb.ToString();
            this.tbSB.Text = this.sb.ToString();

            if (this.tbBB.Visible == false)
            {
                this.tbBB.Visible = true;
                this.tbSB.Visible = true;
                this.bBB.Visible = true;
                this.bSB.Visible = true;
            }
            else
            {
                this.tbBB.Visible = false;
                this.tbSB.Visible = false;
                this.bBB.Visible = false;
                this.bSB.Visible = false;
            }
        }

        private void BotSBClick(object sender, EventArgs e)
        {
            int parsedValue;

            if (this.tbSB.Text.Contains(",") || this.tbSB.Text.Contains("."))
            {
                MessageBox.Show(Resources.MBox_SBlindRoundNumber);
                this.tbSB.Text = this.sb.ToString();
                return;
            }

            if (!int.TryParse(this.tbSB.Text, out parsedValue))
            {
                MessageBox.Show(Resources.MBox_NumberField);
                this.tbSB.Text = this.sb.ToString();
                return;
            }

            if (int.Parse(this.tbSB.Text) > 100000)
            {
                MessageBox.Show(Resources.MBox_MaxCashBlind);
                this.tbSB.Text = this.sb.ToString();
            }
            else if (int.Parse(this.tbSB.Text) < 250)
            {
                MessageBox.Show(Resources.MBox_MinCashBlind);
            }
            else if (int.Parse(this.tbSB.Text) >= 250 && int.Parse(this.tbSB.Text) <= 100000)
            {
                this.sb = int.Parse(this.tbSB.Text);
                MessageBox.Show(Resources.MBox_SavedChanges);
            }
        }

        private void BotBBClick(object sender, EventArgs e)
        {
            int parsedValue;

            if (this.tbBB.Text.Contains(",") || this.tbBB.Text.Contains("."))
            {
                MessageBox.Show(Resources.MBox_RoundedNumber);
                this.tbBB.Text = this.bb.ToString();
                return;
            }

            if (!int.TryParse(this.tbSB.Text, out parsedValue))
            {
                MessageBox.Show(Resources.MBox_NumberField);
                this.tbSB.Text = this.bb.ToString();
                return;
            }

            if (int.Parse(this.tbBB.Text) > 200000)
            {
                MessageBox.Show(Resources.MBox_MaxBlind);
                this.tbBB.Text = this.bb.ToString();
            }
            else if (int.Parse(this.tbBB.Text) < 500)
            {
                MessageBox.Show(Resources.MBox_MinBlind);
            }
            else if (int.Parse(this.tbBB.Text) >= 500 && int.Parse(this.tbBB.Text) <= 200000)
            {
                this.bb = int.Parse(this.tbBB.Text);
                MessageBox.Show(Resources.MBox_SavedChanges);
            }
        }

        private void LayoutChange(object sender, LayoutEventArgs e)
        {
            this.width = this.Width;
            this.height = this.Height;
        }
        #endregion
    }
}