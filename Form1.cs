﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TFT_Overlay
{
    public partial class TFTCrafter : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public string Item1;
        public string Item2;
        public string rItem;
        public string rTier;
        public string rDesc;
        public Point OMLoc;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public TFTCrafter()
        {
            InitializeComponent();
        }

        private void CalculateWR() {
            //Properties.Settings.Default.Reload();
            float TotalGames = Properties.Settings.Default.WINS + Properties.Settings.Default.DEFEAT;
            float FinalWinRate = Properties.Settings.Default.WINS / TotalGames * 100;
            WinRate.Text = FinalWinRate.ToString("0.0") + "%";
        }

        private void Form1_Load(object sender, EventArgs e)

        {


            StartPosition = FormStartPosition.Manual;
            Location = new Point(20, 60);
            TopMost = true;

            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";

            CalculateWR();

        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void DoCheck()
        {
            CrafterWorker();
            
        }

        private void CrafterWorker()
        {

            //BF + Cape <--> Cape + BF
            if (Item1 == "BF" && Item2 == "Cape" || Item2 == "BF" && Item1 == "Cape")
            {
                //BT
                rItem = "Bloodthirster";
                rDesc = "Attacks heal for 50% of damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.bloodthirster;

            }

            //BF + ChainVest <--> Vest + BF
            if (Item1 == "BF" && Item2 == "Vest" || Item2 == "BF" && Item1 == "Vest")
            {
                //GA
                rItem = "Guardian Angel";
                rDesc = "Wearer revives with 300 health";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.guardian_angel;

            }

            //BF + Rod <--> Rod + BF
            if (Item1 == "BF" && Item2 == "Rod" || Item2 == "BF" && Item1 == "Rod")
            {
                //HEXGUNBLADE
                rItem = "Hextech Gunblade";
                rDesc = "Heal for 25% of all damage dealt";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.hextech_gunblade;

            }

            //BF + BF <--> BF + BF
            if (Item1 == "BF" && Item2 == "BF" || Item2 == "BF" && Item1 == "BF")
            {
                //IE
                rItem = "Infinity Edge";
                rDesc = "Critical strikes deal +100% damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.infinity_edge;

            }

            //BF + Tear <--> Tear + BF
            if (Item1 == "BF" && Item2 == "Tear" || Item2 == "BF" && Item1 == "Tear")
            {
                //SPEAR OF SHOJIN
                rItem = "Spear of Shojin";
                rDesc = "After casting, wearer gains 15% of its max mana per attack";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.spear_of_shojin;

            }

            //BF + Bow <--> Bow + BF
            if (Item1 == "BF" && Item2 == "Bow" || Item2 == "BF" && Item1 == "Bow")
            {
                //SWORD OF DIVINE
                rItem = "Sword of the Divine";
                rDesc = "Each second, the wearer has a 5% chance to gain 100% Critical Strike";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.sword_of_the_divine;

            }

            //BF + Spatula <--> Spatula + BF
            if (Item1 == "BF" && Item2 == "Spatula" || Item2 == "BF" && Item1 == "Spatula")
            {
                //YOMUU'S
                rItem = "Yomuu's Ghostblade";
                rDesc = "Wearer is also an Assassin";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.youmuus_ghostblade;

            }

            //BF + Belt <--> Belt + BF
            if (Item1 == "BF" && Item2 == "Belt" || Item2 == "BF" && Item1 == "Belt")
            {
                //Zeke
                rItem = "Zeke's Herald";
                rDesc = "Adjacent allies gain +10% attack speed";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.zekes_herald;

            }

            //Vest + Tear <--> Tear + Vest
            if (Item1 == "Vest" && Item2 == "Tear" || Item2 == "Vest" && Item1 == "Tear")
            {
                //Frozen Heart
                rItem = "Frozen Heart";
                rDesc = "Adjacent enemies lose 20% attack speed";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.frozen_heart;

            }

            //Vest + Rod <--> Rod + Vest
            if (Item1 == "Vest" && Item2 == "Rod" || Item2 == "Vest" && Item1 == "Rod")
            {
                //Locket
                rItem = "Locket of the iron Solari";
                rDesc = "On start of combat, all adjacent allies gain a shield of 200";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.locket_of_the_iron_solari;

            }

            //Vest + Bow <--> Bow + Vest
            if (Item1 == "Vest" && Item2 == "Bow" || Item2 == "Vest" && Item1 == "Bow")
            {
                //Phantom
                rItem = "Phantom Dancer";
                rDesc = "Wearer dodges all Critical Strikes";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.phantom_dancer;

            }

            //Vest + Belt <--> Belt + Vest
            if (Item1 == "Vest" && Item2 == "Belt" || Item2 == "Vest" && Item1 == "Belt")
            {
                //Red Buff
                rItem = "Red Buff";
                rDesc = "Attacks deal 2.5% burn damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.red_buff;

            }

            //Vest + Vest <--> Vest + Vest
            if (Item1 == "Vest" && Item2 == "Vest" || Item2 == "Vest" && Item1 == "Vest")
            {
                //Thornmail
                rItem = "Thornmail";
                rDesc = "Reflect 35% of damage taken from attacks";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.thornmail;

            }

            //Vest + Spatula <--> Spatula + Vest
            if (Item1 == "Vest" && Item2 == "Spatula" || Item2 == "Vest" && Item1 == "Spatula")
            {
                //Knight vow
                rItem = "Knight's Vow";
                rDesc = "Wearer is also a Knight";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.knights_vow;

            }

            //Vest + Cape <--> Cape + Vest
            if (Item1 == "Vest" && Item2 == "Cape" || Item2 == "Vest" && Item1 == "Cape")
            {
                //SWORD BREAKER
                rItem = "Sword Breaker";
                rDesc = "Attacks have a chance to disarm";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.sword_breaker;

            }

            //Belt + Spatula <--> Spatula + Belt
            if (Item1 == "Belt" && Item2 == "Spatula" || Item2 == "Belt" && Item1 == "Spatula")
            {
                //Frozen Mallet
                rItem = "Frozen Mallet";
                rDesc = "Wearer is also a Glacial";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.frozen_mallet;

            }

            //Belt + Rod <--> Rod + Belt
            if (Item1 == "Belt" && Item2 == "Rod" || Item2 == "Belt" && Item1 == "Rod")
            {
                //Morello
                rItem = "Morellonomicon";
                rDesc = "Spells deal burn damage equal to 2.5% of the enemy's maximum health per second";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.morellonomicon;

            }

            //Belt + Tear <--> Spatula + Tear
            if (Item1 == "Belt" && Item2 == "Tear" || Item2 == "Belt" && Item1 == "Tear")
            {
                //Redemption
                rItem = "Redemption";
                rDesc = "On death, heal all nearby allies for 1000 health";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.redemption;

            }

            //Belt + Belt <--> Belt + Belt
            if (Item1 == "Belt" && Item2 == "Belt" || Item2 == "Belt" && Item1 == "Belt")
            {
                //Warmogs
                rItem = "Warmog's Armor";
                rDesc = "Wearer regenerates 3% max health per second";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.warmogs_armor;

            }

            //Belt + Cape <--> Cape + Belt
            if (Item1 == "Belt" && Item2 == "Cape" || Item2 == "Belt" && Item1 == "Cape")
            {
                //zephyr
                rItem = "Zephyr";
                rDesc = "On start of combat, banish an enemy for 5 seconds";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.zephyr;

            }

            //Belt + Bow <--> Bow + Belt
            if (Item1 == "Belt" && Item2 == "Bow" || Item2 == "Belt" && Item1 == "Bow")
            {
                //Titanic
                rItem = "Titanic Hydra";
                rDesc = "Attacks deal 10% of wearer's max health as splash damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.titanic_hydra;

            }

            //Rod + Bow <--> Bow + Rod
            if (Item1 == "Rod" && Item2 == "Bow" || Item2 == "Rod" && Item1 == "Bow")
            {
                //Guinsoo
                rItem = "Guinsoo's Rageblade";
                rDesc = "Attacks grant 4% attack speed (stacks infinitely)";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.guinsoos_rageblade;

            }

            //Rod + Cape <--> Cape + Rod
            if (Item1 == "Rod" && Item2 == "Cape" || Item2 == "Rod" && Item1 == "Cape")
            {
                //Ionic Spark
                rItem = "Ionic Spark";
                rDesc = "Whenever an enemy casts a spell, they take 200 damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.ionic_spark;

            }

            //Rod + Rod <--> Rod + Rod
            if (Item1 == "Rod" && Item2 == "Rod" || Item2 == "Rod" && Item1 == "Rod")
            {
                //Deathcap
                rItem = "Rabadon's Deathcap";
                rDesc = "+50% Ability Power";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.rabadons_deathcap;

            }

            //Rod + Tear <--> Tear + Rod
            if (Item1 == "Rod" && Item2 == "Tear" || Item2 == "Rod" && Item1 == "Tear")
            {
                //Ludens
                rItem = "Luden's Echo";
                rDesc = "Spells deal 200 splash damage on hit";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.ludens_echo;

            }

            //Rod + Spatula <--> Spatula + Rod
            if (Item1 == "Rod" && Item2 == "Spatula" || Item2 == "Rod" && Item1 == "Spatula")
            {
                //Yumii
                rItem = "Yuumi";
                rDesc = "Wearer is also a Sorcerer";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.yuumi;

            }

            //Cape + Bow <--> Bow + Cape
            if (Item1 == "Cape" && Item2 == "Bow" || Item2 == "Cape" && Item1 == "Bow")
            {
                //Cursed Blade
                rItem = "Cursed Blade";
                rDesc = "Attacks have a low chance to shrink (reduce enemy's star level by 1)";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.cursed_blade;

            }

            //Cape + Cape <--> Cape + Cape
            if (Item1 == "Cape" && Item2 == "Cape" || Item2 == "Cape" && Item1 == "Cape")
            {
                //Dragons Claw
                rItem = "Dragon's Claw";
                rDesc = "Gain 83% resistance to magic damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.dragons_claw;

            }

            //Cape + Tear <--> Tear + Cape
            if (Item1 == "Cape" && Item2 == "Tear" || Item2 == "Cape" && Item1 == "Tear")
            {
                //Hush
                rItem = "Hush";
                rDesc = "Attacks have a high chance to Silence";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.hush;

            }

            //Cape + Spatula <--> Spatula + Cape
            if (Item1 == "Cape" && Item2 == "Spatula" || Item2 == "Cape" && Item1 == "Spatula")
            {
                //Runnan
                rItem = "Runaan's Hurricane";
                rDesc = "Attacks 2 extra targets on attack. Extra attacks deal 50% damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.runaans_hurricane;

            }

            //Bow + Bow <--> Bow + Bow
            if (Item1 == "Bow" && Item2 == "Bow" || Item2 == "Bow" && Item1 == "Bow")
            {
                //Rapid Fire
                rItem = "Rapid Firecannon";
                rDesc = "Wearer's attacks cannot be dodged. Attack range is doubled";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.rapid_firecannon;

            }


            //Bow + Tear <--> Tear + Bow
            if (Item1 == "Bow" && Item2 == "Tear" || Item2 == "Bow" && Item1 == "Tear")
            {
                //Statik
                rItem = "Statikk Shiv";
                rDesc = "Every 3rd attack deals 100 splash magical damage";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.statikk_shiv;

            }


            //Bow + Spatula <--> Spatula + Bow
            if (Item1 == "Bow" && Item2 == "Spatula" || Item2 == "Bow" && Item1 == "Spatula")
            {
                //Ruined King
                rItem = "Blade of the ruined King";
                rDesc = "Wearer is also a Blademaster";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.blade_of_the_ruined_king;

            }


            //Tear + Tear <--> Tear + Tear
            if (Item1 == "Tear" && Item2 == "Tear" || Item2 == "Tear" && Item1 == "Tear")
            {
                //Seraph
                rItem = "Seraph's Embrace";
                rDesc = "Regain 20% mana each time a spell is cast";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.seraphs_embrace;

            }

            //Tear + Spatula <--> Spatula + Tear
            if (Item1 == "Tear" && Item2 == "Spatula" || Item2 == "Tear" && Item1 == "Spatula")
            {
                //Darkin
                rItem = "Darkin";
                rDesc = "Wearer is also a Demon";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.darkin;

            }

            //Spatula + Spatula <--> Spatula + Spatula
            if (Item1 == "Spatula" && Item2 == "Spatula" || Item2 == "Spatula" && Item1 == "Spatula")
            {
                //Force Of Nature
                rItem = "Force of Nature";
                rDesc = "Gain +1 team size";
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.force_of_nature;

            }



        }



        private void button2_Click(object sender, EventArgs e)
        {
            Item1 = "BF";
            DoCheck();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Item1 = "Vest";
            DoCheck();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Item1 = "Belt";
            DoCheck();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Item1 = "Rod";
            DoCheck();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Item1 = "Cape";
            DoCheck();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Item1 = "Bow";
            DoCheck();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Item1 = "Tear";
            DoCheck();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Item1 = "Spatula";
            DoCheck();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Item2 = "BF";
            DoCheck();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Item2 = "Vest";
            DoCheck();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Item2 = "Belt";
            DoCheck();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Item2 = "Rod";
            DoCheck();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Item2 = "Cape";
            DoCheck();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Item2 = "Bow";
            DoCheck();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Item2 = "Tear";
            DoCheck();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Item2 = "Spatula";
            DoCheck();
        }


        private void AddWinBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.WINS++;
            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            Properties.Settings.Default.Save();

        }

        private void AddLoseBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DEFEAT++;
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";
            Properties.Settings.Default.Save();
         
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";
        }

        private void WinLab_TextChanged(object sender, EventArgs e)
        {
            CalculateWR();
        }

        private void LoseLab_TextChanged(object sender, EventArgs e)
        {
            CalculateWR();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void ResultItemImage_BackgroundImageChanged(object sender, EventArgs e)
        {
            ItemName.Text = rItem;
            ItemDescription.Text = rDesc;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}