using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace TFT_Overlay
{
    public partial class TFTCrafter : Form
    {
        public string Ver = "1.1";
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public string Item1;
        public string Item2;
        public string rItem;
        public string rTier;
        public string rDesc;
        public Point OMLoc;
        public int Levels = 1;



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public TFTCrafter()
        {
            InitializeComponent();
        }


        // WINRATE CALCULATOR
        private void CalculateWR()
        {
            float TotalGames = Properties.Settings.Default.WINS + Properties.Settings.Default.DEFEAT;
            float FinalWinRate = Properties.Settings.Default.WINS / TotalGames * 100;
            WinRate.Text = FinalWinRate.ToString("0.0") + "%";
        }


        // FORM LOAD
        private void Form1_Load(object sender, EventArgs e)

        {
            Title.Text = "TFT Overlay - " + Ver + " | by @xcibe95x";

            // Check for new Release
            WebClient client = new WebClient();
            string NVer = client.DownloadString("https://raw.githubusercontent.com/xcibe95x/TFT-Overlay/master/VERSION.md");


            double iNVer = double.Parse(NVer);
            double iVer = double.Parse(Ver) ;

            if (iNVer > iVer)
            {
                MessageBox.Show("Version: " + iNVer + "is now available on GitHub", "GitHub.com");
            }

            // GET JSON DATA
            using (var webClient2 = new WebClient())
            {
                string itemsJSON = webClient2.DownloadString("https://solomid-resources.s3.amazonaws.com/blitz/tft/data/items.json");
                JObject jObject = JObject.Parse(itemsJSON);

               // string displayName = (string)jObject.SelectToken("displayName");
               // string type = (string)jObject.SelectToken("signInNames[0].type");
                //string bfsword = (string)jObject.SelectToken("recurvebow.name");
              //  string value = (string)jObject.SelectToken("signInNames[0].value");
               // Console.WriteLine("{0}, {1}, {2}", displayName, type, value);
               // JArray signInNames = (JArray)jObject.SelectToken("signInNames");
               // foreach (JToken signInName in signInNames)
                //{
                  //  type = (string)signInName.SelectToken("type");
                   // value = (string)signInName.SelectToken("value");
                    //Console.WriteLine("{0}, {1}", type, value);
                //}
               
            }


            // WINDOW STARTING POSITION
            StartPosition = FormStartPosition.Manual;
            Location = new Point(20, 60);

            TopMost = Properties.Settings.Default.TopMost;
            alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.TopMost;


            // WINRATE LABELS
            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";

            // CALL WINRATE CALCULATOR
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


        // ITEMS SET 1
        private void button2_Click(object sender, EventArgs e)
        {
            Item1 = "BF";
            button2.ForeColor = Color.FromArgb(205, 61, 18);
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Item1 = "Vest";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.FromArgb(205, 61, 18);
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Item1 = "Belt";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.FromArgb(205, 61, 18);
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Item1 = "Rod";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.FromArgb(205, 61, 18);
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Item1 = "Cape";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.FromArgb(205, 61, 18);
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Item1 = "Bow";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.FromArgb(205, 61, 18);
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Item1 = "Tear";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.FromArgb(205, 61, 18);
            button9.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Item1 = "Spatula";
            button2.ForeColor = Color.DarkSlateBlue;
            button3.ForeColor = Color.DarkSlateBlue;
            button4.ForeColor = Color.DarkSlateBlue;
            button5.ForeColor = Color.DarkSlateBlue;
            button6.ForeColor = Color.DarkSlateBlue;
            button7.ForeColor = Color.DarkSlateBlue;
            button8.ForeColor = Color.DarkSlateBlue;
            button9.ForeColor = Color.FromArgb(205, 61, 18);
            DoCheck();
        }


        // ITEMS SET 2
        private void button1_Click(object sender, EventArgs e)
        {
            Item2 = "BF";
            button1.ForeColor = Color.FromArgb(205, 61, 18);
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Item2 = "Vest";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.FromArgb(205, 61, 18);
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Item2 = "Belt";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.FromArgb(205, 61, 18);
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Item2 = "Rod";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.FromArgb(205, 61, 18);
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Item2 = "Cape";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.FromArgb(205, 61, 18);
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Item2 = "Bow";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.FromArgb(205, 61, 18);
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Item2 = "Tear";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.FromArgb(205, 61, 18);
            button16.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Item2 = "Spatula";
            button1.ForeColor = Color.DarkSlateBlue;
            button10.ForeColor = Color.DarkSlateBlue;
            button11.ForeColor = Color.DarkSlateBlue;
            button12.ForeColor = Color.DarkSlateBlue;
            button13.ForeColor = Color.DarkSlateBlue;
            button14.ForeColor = Color.DarkSlateBlue;
            button15.ForeColor = Color.DarkSlateBlue;
            button16.ForeColor = Color.FromArgb(205, 61, 18);
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


        private void metroButton1_Click(object sender, EventArgs e)
        {

        }


        // Exit Application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            Levels = 1;
            Lvl.Text = "Level " + 1;
            T1P.Text = "100%";
            T2P.Text = "0%";
            T3P.Text = "0%";
            T4P.Text = "0%";
            T5P.Text = "0%";
        }

        private void button19_Click_1(object sender, EventArgs e)
        {



            if (Levels < 9)
            {
                Levels++;
            }

            switch (Levels)
            {
               default:
                    Lvl.Text = "Level 1";
                    T1P.Text = "100%";
                    T2P.Text = "0%";
                    T3P.Text = "0%";
                    T4P.Text = "0%";
                    T5P.Text = "0%";
                    break;
                case 1:
                    Lvl.Text = "Level 1";
                    T1P.Text = "100%";
                    T2P.Text = "0%";
                    T3P.Text = "0%";
                    T4P.Text = "0%";
                    T5P.Text = "0%";
                    break;
                case 2:
                    Lvl.Text = "Level 2";
                    T1P.Text = "100%";
                    T2P.Text = "0%";
                    T3P.Text = "0%";
                    T4P.Text = "0%";
                    T5P.Text = "0%";
                    break;
                case 3:
                    Lvl.Text = "Level 3";
                    T1P.Text = "70%";
                    T2P.Text = "30%";
                    T3P.Text = "0%";
                    T4P.Text = "0%";
                    T5P.Text = "0%";
                    break;
                case 4:
                    Lvl.Text = "Level 4";
                    T1P.Text = "55%";
                    T2P.Text = "30%";
                    T3P.Text = "15%";
                    T4P.Text = "0%";
                    T5P.Text = "0%";
                    break;
                case 5:
                    Lvl.Text = "Level 5";
                    T1P.Text = "40%";
                    T2P.Text = "30%";
                    T3P.Text = "25%";
                    T4P.Text = "5%";
                    T5P.Text = "0%";
                    break;
                case 6:
                    Lvl.Text = "Level 6";
                    T1P.Text = "29%";
                    T2P.Text = "29.5%";
                    T3P.Text = "31%";
                    T4P.Text = "10%";
                    T5P.Text = "0.5%";
                    break;
                case 7:
                    Lvl.Text = "Level 7";
                    T1P.Text = "24%";
                    T2P.Text = "28%";
                    T3P.Text = "31%";
                    T4P.Text = "15%";
                    T5P.Text = "2%";
                    break;
                case 8:
                    Lvl.Text = "Level 8";
                    T1P.Text = "20%";
                    T2P.Text = "24%";
                    T3P.Text = "31%";
                    T4P.Text = "20%";
                    T5P.Text = "5%";
                    break;
                case 9:
                    Lvl.Text = "Level 9";
                    T1P.Text = "10%";
                    T2P.Text = "19%";
                    T3P.Text = "31%";
                    T4P.Text = "30%";
                    T5P.Text = "10%";
                    break;
            }



        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TopMost == true)
            {
                Properties.Settings.Default.TopMost = false;
                TopMost = false;
                Properties.Settings.Default.Save();
            } else {
                Properties.Settings.Default.TopMost = true;
                TopMost = true;
                Properties.Settings.Default.Save();
            }
        }
    }
}