namespace TFT_Overlay
{
    partial class TFTCrafter
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.Title = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button17 = new System.Windows.Forms.Button();
            this.WinLab = new System.Windows.Forms.Label();
            this.LoseLab = new System.Windows.Forms.Label();
            this.AddLoseBtn = new System.Windows.Forms.Button();
            this.AddWinBtn = new System.Windows.Forms.Button();
            this.WinRate = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ResultItemImage = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.ItemName = new System.Windows.Forms.Label();
            this.ItemDescription = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultItemImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.Title);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 24);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel1_MouseDown);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.Enabled = false;
            this.Title.ForeColor = System.Drawing.Color.Azure;
            this.Title.Location = new System.Drawing.Point(6, 5);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(165, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "TFT Overlay - 1.0 | by @xcibe95x";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Controls.Add(this.button7);
            this.flowLayoutPanel1.Controls.Add(this.button8);
            this.flowLayoutPanel1.Controls.Add(this.button9);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(119, 132);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Location = new System.Drawing.Point(-1, 30);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(383, 228);
            this.TabControl.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.ItemDescription);
            this.tabPage1.Controls.Add(this.ItemName);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.flowLayoutPanel2);
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(375, 202);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Crafting";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel2.Controls.Add(this.ResultItemImage);
            this.panel2.Location = new System.Drawing.Point(307, 42);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(60, 60);
            this.panel2.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(279, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "=";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(133, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "+";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.button1);
            this.flowLayoutPanel2.Controls.Add(this.button10);
            this.flowLayoutPanel2.Controls.Add(this.button11);
            this.flowLayoutPanel2.Controls.Add(this.button12);
            this.flowLayoutPanel2.Controls.Add(this.button13);
            this.flowLayoutPanel2.Controls.Add(this.button14);
            this.flowLayoutPanel2.Controls.Add(this.button15);
            this.flowLayoutPanel2.Controls.Add(this.button16);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(169, 8);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(114, 132);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button17);
            this.tabPage2.Controls.Add(this.WinLab);
            this.tabPage2.Controls.Add(this.LoseLab);
            this.tabPage2.Controls.Add(this.AddLoseBtn);
            this.tabPage2.Controls.Add(this.AddWinBtn);
            this.tabPage2.Controls.Add(this.WinRate);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(375, 202);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "WinRate";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.ForeColor = System.Drawing.Color.DarkOrange;
            this.button17.Location = new System.Drawing.Point(125, 73);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(81, 35);
            this.button17.TabIndex = 5;
            this.button17.Text = "Reset";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // WinLab
            // 
            this.WinLab.AutoSize = true;
            this.WinLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinLab.ForeColor = System.Drawing.Color.LawnGreen;
            this.WinLab.Location = new System.Drawing.Point(254, 133);
            this.WinLab.Name = "WinLab";
            this.WinLab.Size = new System.Drawing.Size(37, 20);
            this.WinLab.TabIndex = 4;
            this.WinLab.Text = "0 W";
            this.WinLab.TextChanged += new System.EventHandler(this.WinLab_TextChanged);
            // 
            // LoseLab
            // 
            this.LoseLab.AutoSize = true;
            this.LoseLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoseLab.ForeColor = System.Drawing.Color.Crimson;
            this.LoseLab.Location = new System.Drawing.Point(308, 133);
            this.LoseLab.Name = "LoseLab";
            this.LoseLab.Size = new System.Drawing.Size(31, 20);
            this.LoseLab.TabIndex = 3;
            this.LoseLab.Text = "0 L";
            this.LoseLab.TextChanged += new System.EventHandler(this.LoseLab_TextChanged);
            // 
            // AddLoseBtn
            // 
            this.AddLoseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddLoseBtn.ForeColor = System.Drawing.Color.Crimson;
            this.AddLoseBtn.Location = new System.Drawing.Point(83, 74);
            this.AddLoseBtn.Name = "AddLoseBtn";
            this.AddLoseBtn.Size = new System.Drawing.Size(36, 35);
            this.AddLoseBtn.TabIndex = 2;
            this.AddLoseBtn.Text = "-";
            this.AddLoseBtn.UseVisualStyleBackColor = true;
            this.AddLoseBtn.Click += new System.EventHandler(this.AddLoseBtn_Click);
            // 
            // AddWinBtn
            // 
            this.AddWinBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddWinBtn.ForeColor = System.Drawing.Color.LawnGreen;
            this.AddWinBtn.Location = new System.Drawing.Point(41, 74);
            this.AddWinBtn.Name = "AddWinBtn";
            this.AddWinBtn.Size = new System.Drawing.Size(36, 35);
            this.AddWinBtn.TabIndex = 1;
            this.AddWinBtn.Text = "+";
            this.AddWinBtn.UseVisualStyleBackColor = true;
            this.AddWinBtn.Click += new System.EventHandler(this.AddWinBtn_Click);
            // 
            // WinRate
            // 
            this.WinRate.AutoSize = true;
            this.WinRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinRate.ForeColor = System.Drawing.Color.OrangeRed;
            this.WinRate.Location = new System.Drawing.Point(252, 63);
            this.WinRate.Name = "WinRate";
            this.WinRate.Size = new System.Drawing.Size(87, 31);
            this.WinRate.TabIndex = 0;
            this.WinRate.Text = "100%";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(375, 202);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Probabilities";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ResultItemImage
            // 
            this.ResultItemImage.BackColor = System.Drawing.Color.Black;
            this.ResultItemImage.BackgroundImage = global::TFT_Overlay.Properties.Resources._null;
            this.ResultItemImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ResultItemImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultItemImage.Location = new System.Drawing.Point(3, 3);
            this.ResultItemImage.Name = "ResultItemImage";
            this.ResultItemImage.Size = new System.Drawing.Size(54, 54);
            this.ResultItemImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ResultItemImage.TabIndex = 0;
            this.ResultItemImage.TabStop = false;
            this.ResultItemImage.BackgroundImageChanged += new System.EventHandler(this.ResultItemImage_BackgroundImageChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.BackgroundImage = global::TFT_Overlay.Properties.Resources.BF;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 38);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Black;
            this.button10.BackgroundImage = global::TFT_Overlay.Properties.Resources.Vest;
            this.button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button10.Location = new System.Drawing.Point(46, 3);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(37, 38);
            this.button10.TabIndex = 9;
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.Black;
            this.button11.BackgroundImage = global::TFT_Overlay.Properties.Resources.Belt;
            this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button11.Location = new System.Drawing.Point(3, 47);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(37, 38);
            this.button11.TabIndex = 10;
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Black;
            this.button12.BackgroundImage = global::TFT_Overlay.Properties.Resources.Rod;
            this.button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button12.Location = new System.Drawing.Point(46, 47);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(37, 38);
            this.button12.TabIndex = 12;
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.Black;
            this.button13.BackgroundImage = global::TFT_Overlay.Properties.Resources.Cape;
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button13.Location = new System.Drawing.Point(3, 91);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(37, 38);
            this.button13.TabIndex = 13;
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.Black;
            this.button14.BackgroundImage = global::TFT_Overlay.Properties.Resources.Bow;
            this.button14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button14.Location = new System.Drawing.Point(46, 91);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(37, 38);
            this.button14.TabIndex = 14;
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.Black;
            this.button15.BackgroundImage = global::TFT_Overlay.Properties.Resources.Tear;
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button15.Location = new System.Drawing.Point(3, 135);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(37, 38);
            this.button15.TabIndex = 15;
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.Black;
            this.button16.BackgroundImage = global::TFT_Overlay.Properties.Resources.Spatula;
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button16.Location = new System.Drawing.Point(46, 135);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(37, 38);
            this.button16.TabIndex = 11;
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.BackgroundImage = global::TFT_Overlay.Properties.Resources.BF;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Location = new System.Drawing.Point(3, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 38);
            this.button2.TabIndex = 8;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Black;
            this.button3.BackgroundImage = global::TFT_Overlay.Properties.Resources.Vest;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Location = new System.Drawing.Point(46, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(37, 38);
            this.button3.TabIndex = 9;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button4.BackgroundImage = global::TFT_Overlay.Properties.Resources.Belt;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.Location = new System.Drawing.Point(3, 47);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(37, 38);
            this.button4.TabIndex = 10;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Black;
            this.button5.BackgroundImage = global::TFT_Overlay.Properties.Resources.Rod;
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button5.Location = new System.Drawing.Point(46, 47);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(37, 38);
            this.button5.TabIndex = 12;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Black;
            this.button6.BackgroundImage = global::TFT_Overlay.Properties.Resources.Cape;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button6.Location = new System.Drawing.Point(3, 91);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(37, 38);
            this.button6.TabIndex = 13;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Black;
            this.button7.BackgroundImage = global::TFT_Overlay.Properties.Resources.Bow;
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button7.Location = new System.Drawing.Point(46, 91);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(37, 38);
            this.button7.TabIndex = 14;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Black;
            this.button8.BackgroundImage = global::TFT_Overlay.Properties.Resources.Tear;
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button8.Location = new System.Drawing.Point(3, 135);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(37, 38);
            this.button8.TabIndex = 15;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Black;
            this.button9.BackgroundImage = global::TFT_Overlay.Properties.Resources.Spatula;
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button9.Location = new System.Drawing.Point(46, 135);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(37, 38);
            this.button9.TabIndex = 11;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // ItemName
            // 
            this.ItemName.AutoSize = true;
            this.ItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ItemName.Location = new System.Drawing.Point(9, 144);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(68, 15);
            this.ItemName.TabIndex = 17;
            this.ItemName.Text = "Item Name";
            // 
            // ItemDescription
            // 
            this.ItemDescription.AutoSize = true;
            this.ItemDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ItemDescription.Location = new System.Drawing.Point(9, 160);
            this.ItemDescription.Name = "ItemDescription";
            this.ItemDescription.Size = new System.Drawing.Size(83, 13);
            this.ItemDescription.TabIndex = 18;
            this.ItemDescription.Text = "Item Description";
            // 
            // TFTCrafter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(27)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(382, 258);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TFTCrafter";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultItemImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox ResultItemImage;
        private System.Windows.Forms.Label WinLab;
        private System.Windows.Forms.Label LoseLab;
        private System.Windows.Forms.Button AddLoseBtn;
        private System.Windows.Forms.Button AddWinBtn;
        private System.Windows.Forms.Label WinRate;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Label ItemName;
        private System.Windows.Forms.Label ItemDescription;
    }
}

