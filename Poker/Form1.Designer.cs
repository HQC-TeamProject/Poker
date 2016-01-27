namespace Poker
{
    using System.Windows.Forms;

    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bFold = new Button();
            this.bCheck = new Button();
            this.bCall = new Button();
            this.bRaise = new Button();
            this.pbTimer = new ProgressBar();
            this.tbPlayerChips = new TextBox();
            this.bAddChips = new Button();
            this.tbAddChips = new TextBox();
            this.tbBotChips5 = new TextBox();
            this.tbBotChips4 = new TextBox();
            this.tbBotChips3 = new TextBox();
            this.tbBotChips2 = new TextBox();
            this.tbBotChips1 = new TextBox();
            this.tbTotalPot = new TextBox();
            this.bOptions = new Button();
            this.bBB = new Button();
            this.tbSB = new TextBox();
            this.bSB = new Button();
            this.tbBB = new TextBox();
            this.b5ActionStatus = new Label();
            this.b4ActionStatus = new Label();
            this.b3ActionStatus = new Label();
            this.b1ActionStatus = new Label();
            this.pChipsStatus = new Label();
            this.b2ActionStatus = new Label();
            this.labelPot = new Label();
            this.tbRaise = new TextBox();
            this.SuspendLayout();
            // 
            // bFold
            // 
            this.bFold.Anchor = AnchorStyles.Bottom;
            this.bFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFold.Location = new System.Drawing.Point(335, 660);
            this.bFold.Name = "bFold";
            this.bFold.Size = new System.Drawing.Size(130, 62);
            this.bFold.TabIndex = 0;
            this.bFold.Text = "Fold";
            this.bFold.UseVisualStyleBackColor = true;
            this.bFold.Click += new System.EventHandler(this.bFold_Click);
            // 
            // bCheck
            // 
            this.bCheck.Anchor = AnchorStyles.Bottom;
            this.bCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCheck.Location = new System.Drawing.Point(494, 660);
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(134, 62);
            this.bCheck.TabIndex = 2;
            this.bCheck.Text = "Check";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // bCall
            // 
            this.bCall.Anchor = AnchorStyles.Bottom;
            this.bCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCall.Location = new System.Drawing.Point(667, 661);
            this.bCall.Name = "bCall";
            this.bCall.Size = new System.Drawing.Size(126, 62);
            this.bCall.TabIndex = 3;
            this.bCall.Text = "Call";
            this.bCall.UseVisualStyleBackColor = true;
            this.bCall.Click += new System.EventHandler(this.bCall_Click);
            // 
            // bRaise
            // 
            this.bRaise.Anchor = AnchorStyles.Bottom;
            this.bRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRaise.Location = new System.Drawing.Point(835, 661);
            this.bRaise.Name = "bRaise";
            this.bRaise.Size = new System.Drawing.Size(124, 62);
            this.bRaise.TabIndex = 4;
            this.bRaise.Text = "Raise";
            this.bRaise.UseVisualStyleBackColor = true;
            this.bRaise.Click += new System.EventHandler(this.bRaise_Click);
            // 
            // pbTimer
            // 
            this.pbTimer.Anchor = AnchorStyles.Bottom;
            this.pbTimer.BackColor = System.Drawing.SystemColors.Control;
            this.pbTimer.Location = new System.Drawing.Point(335, 631);
            this.pbTimer.Maximum = 1000;
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(667, 23);
            this.pbTimer.TabIndex = 5;
            this.pbTimer.Value = 1000;
            // 
            // tbPlayerChips
            // 
            this.tbPlayerChips.Anchor = AnchorStyles.Bottom;
            this.tbPlayerChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPlayerChips.Location = new System.Drawing.Point(755, 600);
            this.tbPlayerChips.Name = "tbPlayerChips";
            this.tbPlayerChips.Size = new System.Drawing.Size(163, 23);
            this.tbPlayerChips.TabIndex = 6;
            this.tbPlayerChips.Text = "Chips : 0";
            // 
            // bAddChips
            // 
            this.bAddChips.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.bAddChips.Location = new System.Drawing.Point(12, 697);
            this.bAddChips.Name = "bAddChips";
            this.bAddChips.Size = new System.Drawing.Size(75, 25);
            this.bAddChips.TabIndex = 7;
            this.bAddChips.Text = "AddChips";
            this.bAddChips.UseVisualStyleBackColor = true;
            this.bAddChips.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // tbAddChips
            // 
            this.tbAddChips.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.tbAddChips.Location = new System.Drawing.Point(93, 700);
            this.tbAddChips.Name = "tbAddChips";
            this.tbAddChips.Size = new System.Drawing.Size(125, 20);
            this.tbAddChips.TabIndex = 8;
            // 
            // tbBotChips5
            // 
            this.tbBotChips5.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.tbBotChips5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips5.Location = new System.Drawing.Point(1162, 600);
            this.tbBotChips5.Name = "tbBotChips5";
            this.tbBotChips5.Size = new System.Drawing.Size(152, 23);
            this.tbBotChips5.TabIndex = 9;
            this.tbBotChips5.Text = "Chips : 0";
            // 
            // tbBotChips4
            // 
            this.tbBotChips4.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.tbBotChips4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips4.Location = new System.Drawing.Point(1127, 199);
            this.tbBotChips4.Name = "tbBotChips4";
            this.tbBotChips4.Size = new System.Drawing.Size(123, 23);
            this.tbBotChips4.TabIndex = 10;
            this.tbBotChips4.Text = "Chips : 0";
            // 
            // tbBotChips3
            // 
            this.tbBotChips3.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.tbBotChips3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips3.Location = new System.Drawing.Point(606, 159);
            this.tbBotChips3.Name = "tbBotChips3";
            this.tbBotChips3.Size = new System.Drawing.Size(125, 23);
            this.tbBotChips3.TabIndex = 11;
            this.tbBotChips3.Text = "Chips : 0";
            // 
            // tbBotChips2
            // 
            this.tbBotChips2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips2.Location = new System.Drawing.Point(99, 199);
            this.tbBotChips2.Name = "tbBotChips2";
            this.tbBotChips2.Size = new System.Drawing.Size(133, 23);
            this.tbBotChips2.TabIndex = 12;
            this.tbBotChips2.Text = "Chips : 0";
            // 
            // tbBotChips1
            // 
            this.tbBotChips1.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.tbBotChips1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips1.Location = new System.Drawing.Point(28, 588);
            this.tbBotChips1.Name = "tbBotChips1";
            this.tbBotChips1.Size = new System.Drawing.Size(142, 23);
            this.tbBotChips1.TabIndex = 13;
            this.tbBotChips1.Text = "Chips : 0";
            // 
            // tbTotalPot
            // 
            this.tbTotalPot.Anchor = AnchorStyles.None;
            this.tbTotalPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTotalPot.Location = new System.Drawing.Point(609, 226);
            this.tbTotalPot.Name = "tbTotalPot";
            this.tbTotalPot.Size = new System.Drawing.Size(106, 23);
            this.tbTotalPot.TabIndex = 14;
            this.tbTotalPot.Text = "0";
            // 
            // bOptions
            // 
            this.bOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOptions.Location = new System.Drawing.Point(12, 12);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 36);
            this.bOptions.TabIndex = 15;
            this.bOptions.Text = "BB/SB";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // bBB
            // 
            this.bBB.Location = new System.Drawing.Point(12, 254);
            this.bBB.Name = "bBB";
            this.bBB.Size = new System.Drawing.Size(75, 23);
            this.bBB.TabIndex = 16;
            this.bBB.Text = "Big Blind";
            this.bBB.UseVisualStyleBackColor = true;
            this.bBB.Click += new System.EventHandler(this.bBB_Click);
            // 
            // tbSB
            // 
            this.tbSB.Location = new System.Drawing.Point(12, 228);
            this.tbSB.Name = "tbSB";
            this.tbSB.Size = new System.Drawing.Size(75, 20);
            this.tbSB.TabIndex = 17;
            this.tbSB.Text = "250";
            // 
            // bSB
            // 
            this.bSB.Location = new System.Drawing.Point(12, 199);
            this.bSB.Name = "bSB";
            this.bSB.Size = new System.Drawing.Size(75, 23);
            this.bSB.TabIndex = 18;
            this.bSB.Text = "Small Blind";
            this.bSB.UseVisualStyleBackColor = true;
            this.bSB.Click += new System.EventHandler(this.bSB_Click);
            // 
            // tbBB
            // 
            this.tbBB.Location = new System.Drawing.Point(12, 283);
            this.tbBB.Name = "tbBB";
            this.tbBB.Size = new System.Drawing.Size(75, 20);
            this.tbBB.TabIndex = 19;
            this.tbBB.Text = "500";
            // 
            // b5ActionStatus
            // 
            this.b5ActionStatus.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.b5ActionStatus.Location = new System.Drawing.Point(1162, 553);
            this.b5ActionStatus.Name = "b5ActionStatus";
            this.b5ActionStatus.Size = new System.Drawing.Size(152, 32);
            this.b5ActionStatus.TabIndex = 26;
            // 
            // b4ActionStatus
            // 
            this.b4ActionStatus.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.b4ActionStatus.Location = new System.Drawing.Point(1124, 150);
            this.b4ActionStatus.Name = "b4ActionStatus";
            this.b4ActionStatus.Size = new System.Drawing.Size(123, 32);
            this.b4ActionStatus.TabIndex = 27;
            // 
            // b3ActionStatus
            // 
            this.b3ActionStatus.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.b3ActionStatus.Location = new System.Drawing.Point(606, 124);
            this.b3ActionStatus.Name = "b3ActionStatus";
            this.b3ActionStatus.Size = new System.Drawing.Size(125, 32);
            this.b3ActionStatus.TabIndex = 28;
            // 
            // b1ActionStatus
            // 
            this.b1ActionStatus.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.b1ActionStatus.Location = new System.Drawing.Point(28, 544);
            this.b1ActionStatus.Name = "b1ActionStatus";
            this.b1ActionStatus.Size = new System.Drawing.Size(142, 32);
            this.b1ActionStatus.TabIndex = 29;
            // 
            // pChipsStatus
            // 
            this.pChipsStatus.Anchor = AnchorStyles.Bottom;
            this.pChipsStatus.Location = new System.Drawing.Point(755, 553);
            this.pChipsStatus.Name = "pChipsStatus";
            this.pChipsStatus.Size = new System.Drawing.Size(163, 32);
            this.pChipsStatus.TabIndex = 30;
            // 
            // b2ActionStatus
            // 
            this.b2ActionStatus.Location = new System.Drawing.Point(99, 150);
            this.b2ActionStatus.Name = "b2ActionStatus";
            this.b2ActionStatus.Size = new System.Drawing.Size(133, 32);
            this.b2ActionStatus.TabIndex = 31;
            // 
            // labelPot
            // 
            this.labelPot.Anchor = AnchorStyles.None;
            this.labelPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPot.Location = new System.Drawing.Point(569, 229);
            this.labelPot.Name = "labelPot";
            this.labelPot.Size = new System.Drawing.Size(31, 21);
            this.labelPot.TabIndex = 0;
            this.labelPot.Text = "Pot";
            // 
            // tbRaise
            // 
            this.tbRaise.Anchor = AnchorStyles.Bottom;
            this.tbRaise.Location = new System.Drawing.Point(965, 703);
            this.tbRaise.Name = "tbRaise";
            this.tbRaise.Size = new System.Drawing.Size(108, 20);
            this.tbRaise.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.tbRaise);
            this.Controls.Add(this.labelPot);
            this.Controls.Add(this.b2ActionStatus);
            this.Controls.Add(this.pChipsStatus);
            this.Controls.Add(this.b1ActionStatus);
            this.Controls.Add(this.b3ActionStatus);
            this.Controls.Add(this.b4ActionStatus);
            this.Controls.Add(this.b5ActionStatus);
            this.Controls.Add(this.tbBB);
            this.Controls.Add(this.bSB);
            this.Controls.Add(this.tbSB);
            this.Controls.Add(this.bBB);
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.tbTotalPot);
            this.Controls.Add(this.tbBotChips1);
            this.Controls.Add(this.tbBotChips2);
            this.Controls.Add(this.tbBotChips3);
            this.Controls.Add(this.tbBotChips4);
            this.Controls.Add(this.tbBotChips5);
            this.Controls.Add(this.tbAddChips);
            this.Controls.Add(this.bAddChips);
            this.Controls.Add(this.tbPlayerChips);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.bRaise);
            this.Controls.Add(this.bCall);
            this.Controls.Add(this.bCheck);
            this.Controls.Add(this.bFold);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bFold;
        private Button bCheck;
        private Button bCall;
        private Button bRaise;
        private ProgressBar pbTimer;
        private TextBox tbPlayerChips;
        private Button bAddChips;
        private TextBox tbAddChips;
        private TextBox tbBotChips5;
        private TextBox tbBotChips4;
        private TextBox tbBotChips3;
        private TextBox tbBotChips2;
        private TextBox tbBotChips1;
        private TextBox tbTotalPot;
        private Button bOptions;
        private Button bBB;
        private TextBox tbSB;
        private Button bSB;
        private TextBox tbBB;
        private Label b5ActionStatus;
        private Label b4ActionStatus;
        private Label b3ActionStatus;
        private Label b1ActionStatus;
        private Label pChipsStatus;
        private Label b2ActionStatus;
        private Label labelPot;
        private TextBox tbRaise;
    }
}

