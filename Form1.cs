using MetroFramework.Components;
using MetroFramework.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
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
        public int Levels = 1;
        WebClient client = new WebClient();
        public string itemsJSON;
        public string tiersJSON;
        public string champsJSON;
        FlowLayoutPanel SearchPanel;

        int TierIndex = 1;
        string TierType = "all";



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();



        static class NativeMethods
        {
            public static Cursor LoadCustomCursor(string path)
            {
                IntPtr hCurs = LoadCursorFromFile(path);
                if (hCurs == IntPtr.Zero) throw new Win32Exception();
                var curs = new Cursor(hCurs);
                // Note: force the cursor to own the handle so it gets released properly
                var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
                fi.SetValue(curs, true);
                return curs;
            }
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern IntPtr LoadCursorFromFile(string path);
        }
       
        public TFTCrafter()
        {
            InitializeComponent();
        }


        // WINRATE CALCULATOR
        private void CalculateWR()
        {
            float TotalGames = Properties.Settings.Default.WINS + Properties.Settings.Default.DEFEAT;
            float FinalWinRate = Properties.Settings.Default.WINS / TotalGames * 100;
            if (TotalGames != 0)
            {
                WinRate.Text = FinalWinRate.ToString("0.0") + "%";
            } else
            {
                WinRate.Text = "0%";
            }

        }


        // FORM LOAD
        private void Form1_Load(object sender, EventArgs e)

        {

            metroTabControl1.SelectedIndex = 0;

            // Delete and Generate Custom Mouse Pointer to Temp Path
            File.Delete(Path.GetTempPath() + "Normal.cur");
            File.Delete(Path.GetTempPath() + "Pointer.cur");
            File.WriteAllBytes(Path.GetTempPath() + "Normal.cur", Properties.Resources.Normal);
            File.WriteAllBytes(Path.GetTempPath() + "Pointer.cur", Properties.Resources.Pointer);

            var localVersion = new Version("2.2");

            // Program Title
            Title.Text = "TFT Overlay | by @xcibe95x";
            Properties.Settings.Default.Version = localVersion.ToString();


            // Set League Cursor
            Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Normal.cur");


            // Check for new Release
            string Ver = client.DownloadString("https://raw.githubusercontent.com/xcibe95x/TFT-Overlay/master/VERSION.md");


            var netVersion = new Version(Ver);

            if (netVersion > localVersion)
            {
                if (MessageBox.Show("Version: " + netVersion + " is now available on GitHub, Want to get it now?", "GitHub.com", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://github.com/xcibe95x/TFT-Overlay/releases");
                }
            }



        
            // GET JSON DATA
            itemsJSON = client.DownloadString("https://solomid-resources.s3.amazonaws.com/blitz/tft/data/items.json");
            tiersJSON = client.DownloadString("https://solomid-resources.s3.amazonaws.com/blitz/tft/data/tierlist.json");
            champsJSON = client.DownloadString("https://solomid-resources.s3.amazonaws.com/blitz/tft/data/champions.json");


            // FIX TIER LIST
            metroComboBox1.SelectedIndex = 0;
            TierListBox.SelectedIndex = 0;


            // Useful codes for JSON

            // string displayName = (string)jObject.SelectToken("name");
            // string type = (string)jObject.SelectToken("arrayName[0].type");
            //  string value = (string)jObject.SelectToken("arrayname[0].value");




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

            // LOAD CHAMPIONS LIST
            championsListLoop();
     

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

            JObject jObject = JObject.Parse(itemsJSON);

            //BF + Cape <--> Cape + BF
            if (Item1 == "BF" && Item2 == "Cape" || Item2 == "BF" && Item1 == "Cape")
            {
                //BT
                rItem = (string)jObject.SelectToken("bloodthirster.name");
                rDesc = (string)jObject.SelectToken("bloodthirster.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.bloodthirster;

            }

            //BF + ChainVest <--> Vest + BF
            if (Item1 == "BF" && Item2 == "Vest" || Item2 == "BF" && Item1 == "Vest")
            {
                //GA
                rItem = (string)jObject.SelectToken("guardianangel.name");
                rDesc = (string)jObject.SelectToken("guardianangel.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.guardianangel;

            }

            //BF + Rod <--> Rod + BF
            if (Item1 == "BF" && Item2 == "Rod" || Item2 == "BF" && Item1 == "Rod")
            {
                //HEXGUNBLADE
                rItem = (string)jObject.SelectToken("hextechgunblade.name");
                rDesc = (string)jObject.SelectToken("hextechgunblade.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.hextechgunblade;

            }

            //BF + BF <--> BF + BF
            if (Item1 == "BF" && Item2 == "BF" || Item2 == "BF" && Item1 == "BF")
            {
                //IE
                rItem = (string)jObject.SelectToken("infinityedge.name");
                rDesc = (string)jObject.SelectToken("infinityedge.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.infinityedge;

            }

            //BF + Tear <--> Tear + BF
            if (Item1 == "BF" && Item2 == "Tear" || Item2 == "BF" && Item1 == "Tear")
            {
                //SPEAR OF SHOJIN
                rItem = (string)jObject.SelectToken("spearofshojin.name");
                rDesc = (string)jObject.SelectToken("spearofshojin.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.spearofshojin;

            }

            //BF + Bow <--> Bow + BF
            if (Item1 == "BF" && Item2 == "Bow" || Item2 == "BF" && Item1 == "Bow")
            {
                //SWORD OF DIVINE
                rItem = (string)jObject.SelectToken("swordofthedivine.name");
                rDesc = (string)jObject.SelectToken("swordofthedivine.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.swordofthedivine;

            }

            //BF + Spatula <--> Spatula + BF
            if (Item1 == "BF" && Item2 == "Spatula" || Item2 == "BF" && Item1 == "Spatula")
            {
                //YOMUU'S
                rItem = (string)jObject.SelectToken("youmuusghostblade.name");
                rDesc = (string)jObject.SelectToken("youmuusghostblade.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.youmuusghostblade;

            }

            //BF + Belt <--> Belt + BF
            if (Item1 == "BF" && Item2 == "Belt" || Item2 == "BF" && Item1 == "Belt")
            {
                //Zeke
                rItem = (string)jObject.SelectToken("zekesherald.name");
                rDesc = (string)jObject.SelectToken("zekesherald.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.zekesherald;

            }

            //Vest + Tear <--> Tear + Vest
            if (Item1 == "Vest" && Item2 == "Tear" || Item2 == "Vest" && Item1 == "Tear")
            {
                //Frozen Heart
                rItem = (string)jObject.SelectToken("frozenheart.name");
                rDesc = (string)jObject.SelectToken("frozenheart.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.frozenheart;

            }

            //Vest + Rod <--> Rod + Vest
            if (Item1 == "Vest" && Item2 == "Rod" || Item2 == "Vest" && Item1 == "Rod")
            {
                //Locket
                rItem = (string)jObject.SelectToken("locketoftheironsolari.name");
                rDesc = (string)jObject.SelectToken("locketoftheironsolari.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.locketoftheironsolari;

            }

            //Vest + Bow <--> Bow + Vest
            if (Item1 == "Vest" && Item2 == "Bow" || Item2 == "Vest" && Item1 == "Bow")
            {
                //Phantom
                rItem = (string)jObject.SelectToken("phantomdancer.name");
                rDesc = (string)jObject.SelectToken("phantomdancer.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.phantomdancer;

            }

            //Vest + Belt <--> Belt + Vest
            if (Item1 == "Vest" && Item2 == "Belt" || Item2 == "Vest" && Item1 == "Belt")
            {
                //Red Buff
                rItem = (string)jObject.SelectToken("redbuff.name");
                rDesc = (string)jObject.SelectToken("redbuff.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.redbuff;

            }

            //Vest + Vest <--> Vest + Vest
            if (Item1 == "Vest" && Item2 == "Vest" || Item2 == "Vest" && Item1 == "Vest")
            {
                //Thornmail
                rItem = (string)jObject.SelectToken("thornmail.name");
                rDesc = (string)jObject.SelectToken("thornmail.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.thornmail;

            }

            //Vest + Spatula <--> Spatula + Vest
            if (Item1 == "Vest" && Item2 == "Spatula" || Item2 == "Vest" && Item1 == "Spatula")
            {
                //Knight vow
                rItem = (string)jObject.SelectToken("knightsvow.name");
                rDesc = (string)jObject.SelectToken("knightsvow.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.knightsvow;

            }

            //Vest + Cape <--> Cape + Vest
            if (Item1 == "Vest" && Item2 == "Cape" || Item2 == "Vest" && Item1 == "Cape")
            {
                //SWORD BREAKER
                rItem = (string)jObject.SelectToken("swordbreaker.name");
                rDesc = (string)jObject.SelectToken("swordbreaker.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.swordbreaker;

            }

            //Belt + Spatula <--> Spatula + Belt
            if (Item1 == "Belt" && Item2 == "Spatula" || Item2 == "Belt" && Item1 == "Spatula")
            {
                //Frozen Mallet
                rItem = (string)jObject.SelectToken("frozenmallet.name");
                rDesc = (string)jObject.SelectToken("frozenmallet.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.frozenmallet;

            }

            //Belt + Rod <--> Rod + Belt
            if (Item1 == "Belt" && Item2 == "Rod" || Item2 == "Belt" && Item1 == "Rod")
            {
                //Morello
                rItem = (string)jObject.SelectToken("morellonomicon.name");
                rDesc = (string)jObject.SelectToken("morellonomicon.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.morellonomicon;

            }

            //Belt + Tear <--> Spatula + Tear
            if (Item1 == "Belt" && Item2 == "Tear" || Item2 == "Belt" && Item1 == "Tear")
            {
                //Redemption
                rItem = (string)jObject.SelectToken("redemption.name");
                rDesc = (string)jObject.SelectToken("redemption.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.redemption;

            }

            //Belt + Belt <--> Belt + Belt
            if (Item1 == "Belt" && Item2 == "Belt" || Item2 == "Belt" && Item1 == "Belt")
            {
                //Warmogs
                rItem = (string)jObject.SelectToken("warmogsarmor.name");
                rDesc = (string)jObject.SelectToken("warmogsarmor.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.warmogsarmor;

            }

            //Belt + Cape <--> Cape + Belt
            if (Item1 == "Belt" && Item2 == "Cape" || Item2 == "Belt" && Item1 == "Cape")
            {
                //zephyr
                rItem = (string)jObject.SelectToken("zephyr.name");
                rDesc = (string)jObject.SelectToken("zephyr.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.zephyr;

            }

            //Belt + Bow <--> Bow + Belt
            if (Item1 == "Belt" && Item2 == "Bow" || Item2 == "Belt" && Item1 == "Bow")
            {
                //Titanic
                rItem = (string)jObject.SelectToken("titanichydra.name");
                rDesc = (string)jObject.SelectToken("titanichydra.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.titanichydra;

            }

            //Rod + Bow <--> Bow + Rod
            if (Item1 == "Rod" && Item2 == "Bow" || Item2 == "Rod" && Item1 == "Bow")
            {
                //Guinsoo
                rItem = (string)jObject.SelectToken("guinsoosrageblade.name");
                rDesc = (string)jObject.SelectToken("guinsoosrageblade.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.guinsoosrageblade;

            }

            //Rod + Cape <--> Cape + Rod
            if (Item1 == "Rod" && Item2 == "Cape" || Item2 == "Rod" && Item1 == "Cape")
            {
                //Ionic Spark
                rItem = (string)jObject.SelectToken("ionicspark.name");
                rDesc = (string)jObject.SelectToken("ionicspark.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.ionicspark;

            }

            //Rod + Rod <--> Rod + Rod
            if (Item1 == "Rod" && Item2 == "Rod" || Item2 == "Rod" && Item1 == "Rod")
            {
                //Deathcap
                rItem = (string)jObject.SelectToken("rabadonsdeathcap.name");
                rDesc = (string)jObject.SelectToken("rabadonsdeathcap.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.rabadonsdeathcap;

            }

            //Rod + Tear <--> Tear + Rod
            if (Item1 == "Rod" && Item2 == "Tear" || Item2 == "Rod" && Item1 == "Tear")
            {
                //Ludens
                rItem = (string)jObject.SelectToken("ludensecho.name");
                rDesc = (string)jObject.SelectToken("ludensecho.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.ludensecho;

            }

            //Rod + Spatula <--> Spatula + Rod
            if (Item1 == "Rod" && Item2 == "Spatula" || Item2 == "Rod" && Item1 == "Spatula")
            {
                //Yumii
                rItem = (string)jObject.SelectToken("yuumi.name");
                rDesc = (string)jObject.SelectToken("yuumi.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.yuumi;

            }

            //Cape + Bow <--> Bow + Cape
            if (Item1 == "Cape" && Item2 == "Bow" || Item2 == "Cape" && Item1 == "Bow")
            {
                //Cursed Blade
                rItem = (string)jObject.SelectToken("cursedblade.name");
                rDesc = (string)jObject.SelectToken("cursedblade.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.cursedblade;

            }

            //Cape + Cape <--> Cape + Cape
            if (Item1 == "Cape" && Item2 == "Cape" || Item2 == "Cape" && Item1 == "Cape")
            {
                //Dragons Claw
                rItem = (string)jObject.SelectToken("dragonsclaw.name");
                rDesc = (string)jObject.SelectToken("dragonsclaw.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.dragonsclaw;

            }

            //Cape + Tear <--> Tear + Cape
            if (Item1 == "Cape" && Item2 == "Tear" || Item2 == "Cape" && Item1 == "Tear")
            {
                //Hush
                rItem = (string)jObject.SelectToken("hush.name");
                rDesc = (string)jObject.SelectToken("hush.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.hush;

            }

            //Cape + Spatula <--> Spatula + Cape
            if (Item1 == "Cape" && Item2 == "Spatula" || Item2 == "Cape" && Item1 == "Spatula")
            {
                //Runnan
                rItem = (string)jObject.SelectToken("runaanshurricane.name");
                rDesc = (string)jObject.SelectToken("runaanshurricane.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.runaanshurricane;

            }

            //Bow + Bow <--> Bow + Bow
            if (Item1 == "Bow" && Item2 == "Bow" || Item2 == "Bow" && Item1 == "Bow")
            {
                //Rapid Fire
                rItem = (string)jObject.SelectToken("rapidfirecannon.name");
                rDesc = (string)jObject.SelectToken("rapidfirecannon.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.rapidfirecannon;

            }


            //Bow + Tear <--> Tear + Bow
            if (Item1 == "Bow" && Item2 == "Tear" || Item2 == "Bow" && Item1 == "Tear")
            {
                //Statik
                rItem = (string)jObject.SelectToken("statikkshiv.name");
                rDesc = (string)jObject.SelectToken("statikkshiv.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.statikkshiv;

            }


            //Bow + Spatula <--> Spatula + Bow
            if (Item1 == "Bow" && Item2 == "Spatula" || Item2 == "Bow" && Item1 == "Spatula")
            {
                //Ruined King
                rItem = (string)jObject.SelectToken("bladeoftheruinedking.name");
                rDesc = (string)jObject.SelectToken("bladeoftheruinedking.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.bladeoftheruinedking;

            }


            //Tear + Tear <--> Tear + Tear
            if (Item1 == "Tear" && Item2 == "Tear" || Item2 == "Tear" && Item1 == "Tear")
            {
                //Seraph
                rItem = (string)jObject.SelectToken("seraphsembrace.name");
                rDesc = (string)jObject.SelectToken("seraphsembrace.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.seraphsembrace;

            }

            //Tear + Spatula <--> Spatula + Tear
            if (Item1 == "Tear" && Item2 == "Spatula" || Item2 == "Tear" && Item1 == "Spatula")
            {
                //Darkin
                rItem = (string)jObject.SelectToken("darkin.name");
                rDesc = (string)jObject.SelectToken("darkin.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.darkin;

            }

            //Spatula + Spatula <--> Spatula + Spatula
            if (Item1 == "Spatula" && Item2 == "Spatula" || Item2 == "Spatula" && Item1 == "Spatula")
            {
                //Force Of Nature
                rItem = (string)jObject.SelectToken("forceofnature.name");
                rDesc = (string)jObject.SelectToken("forceofnature.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.forceofnature;

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void ResultItemImage_BackgroundImageChanged(object sender, EventArgs e)
        {
            ItemName.Text = rItem;
            htmlItemdescription.Text = @"<div style=""color: #87ceeb; font-size: 8px"">" + rDesc + "</div>";
        }


        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

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

 

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void T4T_Click(object sender, EventArgs e)
        {

        }

        private void GetTierList(int TierIndex, string TierType)
        {

            // FILL TIER LIST

            JObject jObject = JObject.Parse(tiersJSON);

            JArray championsTiers = (JArray)jObject.SelectToken(TierType + "." + TierIndex);
            int champIndex = 0;

            foreach (JToken arrayname in championsTiers)
            {

                ResourceManager rm = new ResourceManager(
                "TFT_Overlay.Properties.Resources",
                Assembly.GetExecutingAssembly());



                string champName = (string)jObject.SelectToken(TierType + "."+ TierIndex + "[" + champIndex.ToString() + "]");

                var picture = new PictureBox
                {
                    Name = "pictureBox",
                    Size = new Size(58, 58),
                    Location = new Point(0, 0),
                    BackgroundImage = (Image)(rm.GetObject(champName)),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };
                flowLayoutPanel1.Controls.Add(picture);
                champIndex++;
            }

        }



        // LVL UP BUTTON
        // PROBABILITIES
        private void metroButton1_Click_1(object sender, EventArgs e)
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
                    T2P.Text = "-";
                    T3P.Text = "-";
                    T4P.Text = "-";
                    T5P.Text = "-";
                    break;
                case 1:
                    Lvl.Text = "Level 1";
                    T1P.Text = "100%";
                    T2P.Text = "-";
                    T3P.Text = "-";
                    T4P.Text = "-";
                    T5P.Text = "-";
                    break;
                case 2:
                    Lvl.Text = "Level 2";
                    T1P.Text = "100%";
                    T2P.Text = "-";
                    T3P.Text = "-";
                    T4P.Text = "-";
                    T5P.Text = "-";
                    break;
                case 3:
                    Lvl.Text = "Level 3";
                    T1P.Text = "65%";
                    T2P.Text = "30%";
                    T3P.Text = "5%";
                    T4P.Text = "-";
                    T5P.Text = "-";
                    break;
                case 4:
                    Lvl.Text = "Level 4";
                    T1P.Text = "50%";
                    T2P.Text = "35%";
                    T3P.Text = "15%";
                    T4P.Text = "-";
                    T5P.Text = "-";
                    break;
                case 5:
                    Lvl.Text = "Level 5";
                    T1P.Text = "37%";
                    T2P.Text = "35%";
                    T3P.Text = "25%";
                    T4P.Text = "3%";
                    T5P.Text = "-";
                    break;
                case 6:
                    Lvl.Text = "Level 6";
                    T1P.Text = "24.5%";
                    T2P.Text = "35%";
                    T3P.Text = "30%";
                    T4P.Text = "10%";
                    T5P.Text = "0.5%";
                    break;
                case 7:
                    Lvl.Text = "Level 7";
                    T1P.Text = "20%";
                    T2P.Text = "30%";
                    T3P.Text = "33%";
                    T4P.Text = "15%";
                    T5P.Text = "2%";
                    break;
                case 8:
                    Lvl.Text = "Level 8";
                    T1P.Text = "15%";
                    T2P.Text = "25%";
                    T3P.Text = "35%";
                    T4P.Text = "20%";
                    T5P.Text = "5%";
                    break;
                case 9:
                    Lvl.Text = "Level 9";
                    T1P.Text = "10%";
                    T2P.Text = "15%";
                    T3P.Text = "35%";
                    T4P.Text = "30%";
                    T5P.Text = "10%";
                    break;
            }
        }


        // RESET LEVEL BUTTON
        private void metroButton2_Click(object sender, EventArgs e)
        {
            Levels = 1;
            Lvl.Text = "Level " + 1;
            T1P.Text = "100%";
            T2P.Text = "0%";
            T3P.Text = "0%";
            T4P.Text = "0%";
            T5P.Text = "0%";
        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void AddWinBtn_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.WINS++;
            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            Properties.Settings.Default.Save();
        }

        private void AddLoseBtn_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.DEFEAT++;
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";
            Properties.Settings.Default.Save();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            WinLab.Text = Properties.Settings.Default.WINS.ToString() + " W";
            LoseLab.Text = Properties.Settings.Default.DEFEAT.ToString() + " L";
        }

        private void WinRate_Click(object sender, EventArgs e)
        {

        }

        private void WinLab_Click(object sender, EventArgs e)
        {

        }

        private void WinLab_TextChanged_1(object sender, EventArgs e)
        {
            CalculateWR();
        }

        private void LoseLab_TextChanged(object sender, EventArgs e)
        {
            CalculateWR();
        }


        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            TierIndex = TierListBox.SelectedIndex + 1;
            GetTierList(TierIndex, TierType);

        }

        private void metroComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            TierType = metroComboBox1.SelectedItem.ToString().ToLower();
            GetTierList(TierIndex, TierType);
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ProbTab_Click(object sender, EventArgs e)
        {

        }

        // DEPRECATED

       // private void Pointer_MouseClick(object sender, MouseEventArgs e)
       // {
       // Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Pointer.cur");
       // System.Threading.Thread.Sleep(60);           
       // Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Normal.cur");
       // }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
        }

        private void PointerLogic(object sender, EventArgs e)
        {
            Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Pointer.cur");
        }

        private void PointerLogicLeave(object sender, EventArgs e)
        {
            Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Normal.cur");
        }



      // CHAMPIONS LIST CODE

        private void championsListLoop()
        {
            
            JObject jObject = JObject.Parse(champsJSON);

            // CHAMP LIST (FOREACH)
            var ChampionNamesList = jObject.SelectTokens("$").ToList();

            foreach (JProperty item in ChampionNamesList.Children())
            {
                int originsIndex = 0;
                int classesIndex = 0;
                int itemsIndex = 0;

                // VARIABLES
                var key = item.Name.ToString();
                int cost = (int)jObject.SelectToken(key + ".cost");              
                string ability = (string)jObject.SelectToken(key + ".ability.description");
                Color tiercost = Color.OrangeRed;

                ToolTip tip = new MetroFramework.Drawing.Html.HtmlToolTip();
                ToolTip tiup = new MetroToolTip();


                if (cost == 1)
                {
                    tiercost = Color.DimGray;
                }

                if (cost == 2) {
                   tiercost = Color.ForestGreen;
                }

                if (cost == 3)
                {
                    tiercost = Color.DodgerBlue;
                }

                if (cost == 4)
                {
                    tiercost = Color.DarkViolet;
                }

                if (cost == 5)
                {
                    tiercost = Color.Gold;
                }


                ChampsSearchBox.AutoCompleteCustomSource.Add(key);

                ResourceManager rm = new ResourceManager(
                "TFT_Overlay.Properties.Resources",
                Assembly.GetExecutingAssembly());


                // DEBUG LINE
                Console.WriteLine((string)jObject.SelectToken(key + ".name"));

                // CODE
                var basepanel = new FlowLayoutPanel
                {
                    Name = key,
                    Size = new Size(312, 48),
                };
                ChampsList.Controls.Add(basepanel);
                SearchPanel = basepanel;


                // CHAMP BORDER
                var ChampBox = new Panel
                {
                    Name = "ChampBox",
                    Size = new Size(45, 45),
                    Padding = new Padding(2, 2, 2, 2),
                    BackColor = tiercost,

                };
                basepanel.Controls.Add(ChampBox);


                // CHAMP ICON
                var ChampPicture = new PictureBox
                {
                    Name = "ChampPicture",
                    Size = new Size(41, 41),
                    Dock = DockStyle.Fill,
                    BackgroundImage = (Image)(rm.GetObject(key + "_n")),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };
                ChampBox.Controls.Add(ChampPicture);


                // CHAMP COST
                var ChampCost = new MetroLabel
                {
                    Text = cost.ToString(),
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    ForeColor = tiercost,
  
                    Size = new Size(12, 18),
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                };
                tiup.SetToolTip(ChampCost, "Cost");
                basepanel.Controls.Add(ChampCost);


                // COST ICON
                var CostIcon = new PictureBox
                {
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Size = new Size(10, 10),
                    BackgroundImage = Properties.Resources.Coin,
                    BackgroundImageLayout = ImageLayout.Stretch,

                };
                tiup.SetToolTip(CostIcon, "Cost");
                basepanel.Controls.Add(CostIcon);


                // SPACER

                var Spacer = new Panel
                {
                    Size = new Size(1, 1),
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Location = new Point(0, 0),
                };
                basepanel.Controls.Add(Spacer);


                // ORIGIN

                JArray originsLoop = (JArray)jObject.SelectToken(key + ".origin");

                foreach (JToken arrayname in originsLoop)
                {

                    string origins = (string)jObject.SelectToken(key + ".origin.[" + originsIndex.ToString() + "]");

                    var picturebox = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Location = new Point(0, 0),
                        BackgroundImage = (Image)(rm.GetObject(origins)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };

                    basepanel.Controls.Add(picturebox);
                    tiup.SetToolTip(picturebox, origins);
                    originsIndex++;
                }


                // CLASSES

                JArray classesLoop = (JArray)jObject.SelectToken(key + ".class");

                foreach (JToken arrayname in classesLoop)
                {


                    string classes = (string)jObject.SelectToken(key + ".class.[" + classesIndex.ToString() + "]");

                    var picturebox = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Location = new Point(0, 0),
                        BackgroundImage = (Image)(rm.GetObject(classes)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };

                    basepanel.Controls.Add(picturebox);
                    tiup.SetToolTip(picturebox, classes);
                    classesIndex++;
                }
            


                // SPACER

                var Spacer2 = new Panel
                {
                    Size = new Size(4, 4),
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Location = new Point(0, 0),
                };
                basepanel.Controls.Add(Spacer2);


                // ITEMS

                JArray itemsLoop = (JArray)jObject.SelectToken(key + ".items");



                foreach (JToken arrayname in itemsLoop)
                {


                    string itemization = (string)jObject.SelectToken(key + ".items.[" + itemsIndex.ToString() + "]");

                    var itemBorder = new Panel
                    {

                        Size = new Size(32, 32),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Location = new Point(0, 0),
                        BackColor = tiercost,
                        Padding = new Padding(2, 2, 2, 2),
                    };


                    var picturebox = new PictureBox
                    {

                        Size = new Size(28, 28),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.Fill,

                        Location = new Point(0, 0),
                        BackgroundImage = (Image)(rm.GetObject(itemization)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    };

                    basepanel.Controls.Add(itemBorder);
                    itemBorder.Controls.Add(picturebox);
                    itemsIndex++;

                }

                // TOOLTIPS
                tiup.SetToolTip(ChampBox, ability);
                tiup.SetToolTip(ChampPicture, "           "+ ability + "           ");


            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (Control SearchPanel in ChampsList.Controls)
            {
                if (!SearchPanel.Name.ToLower().Contains(ChampsSearchBox.Text.ToLower()))
                {
                    ChampsList.Controls.Remove(SearchPanel);
                }

            }

            if (string.IsNullOrWhiteSpace(ChampsSearchBox.Text))
            {
                ChampsList.Controls.Clear();
                championsListLoop();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                ((TextBox)sender).Clear();
            }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroTabControl1.SelectedTab != metroTabControl1.TabPages["ChampionsTab"])
            {
                ChampsSearchBox.Visible = false;
                label9.Visible = false;
            } else
            {
                ChampsSearchBox.Visible = true;
                label9.Visible = true;
            }
        }
    }
}