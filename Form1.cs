using MetroFramework.Controls; using Newtonsoft.Json.Linq; using System; using System.Collections; using System.Collections.Generic; using System.ComponentModel; using System.Drawing; using System.Drawing.Drawing2D;
using System.Globalization; using System.IO; using System.Linq; using System.Net; using System.Reflection; using System.Resources; using System.Runtime.InteropServices; using System.Text.RegularExpressions;
using System.Threading; using System.Windows.Forms;

// Useful Links
// https://riotapi.dev/en/latest/
// https://lolchess.gg/guide/damage

namespace TFT_Overlay
{
    public partial class TFTCrafter : Form
    {
        
        //
        // APP SETTINGS
        //

        readonly Version localVersion = new Version("3.0");
        public string ApiKey = "RGAPI-";

        /////////////////////////////////////////////////////


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
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]  private static extern IntPtr LoadCursorFromFile(string path);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")] public static extern bool ReleaseCapture();


        /// VARIABLES

        public string summonerJSON, rankedJSON, itemsJSON, tiersJSON, champsJSON, versionJSON, originsJSON, classesJSON, compsJSON;
        public string Item1, Item2, rItem, rTier, rDesc;
        public string lolVer;
        public string SummonerName = Properties.Settings.Default.SummonerName;
        public int Wins, Loss;
        public int Levels = 1;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public bool toggleHide;
        public Point OMLoc;

        readonly List<string> ResourcesList = new List<string>();
        readonly ResourceManager rm = new ResourceManager("TFT_Overlay.Properties.Resources", Assembly.GetExecutingAssembly());

        

        int TierIndex = 1;
        string TierType = "all";

        public TFTCrafter()
        {
            InitializeComponent();
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Region = new Region(path);
        }


        // WINRATE CALCULATOR
        private void CalculateWR()
        {
            float TotalGames = Wins + Loss;
            float FinalWinRate = Wins / TotalGames * 100;
            if (TotalGames != 0)
            {
                WinRate.Text = FinalWinRate.ToString("0.0") + "%";
                if (FinalWinRate > 50) { WinRate.ForeColor = Color.ForestGreen; }
                if (FinalWinRate < 50 && FinalWinRate > 15) { WinRate.ForeColor = Color.Orange; }
                if (FinalWinRate < 15) { WinRate.ForeColor = Color.Red; }

            }
            else
            {
                WinRate.Text = "0%";
            }

        }



        // FORM LOAD
        private void Form1_Load(object sender, EventArgs e)

        {

            //var localVersion = new Version("3.0");

            // WINDOW STARTING POSITION
            StartPosition = FormStartPosition.Manual;
            Location = new Point(20, 60);
            TopMost = Properties.Settings.Default.TopMost;
            alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.TopMost;

            
         
          this.Opacity = Properties.Settings.Default.Opacity / 100.0;
           
           metroTrackBar1.Value = Properties.Settings.Default.Opacity;


            // TABS ORDER PERMANENT FIX
            TabControl.TabPages.Clear();
            TabControl.TabPages.Insert(0, ProfileTAB);
            TabControl.TabPages.Insert(1, CraftingTab);
            TabControl.TabPages.Insert(2, ChampionsTab);
            TabControl.TabPages.Insert(3, TierListTab);

            TabControl.SelectedIndex = 0;
            // Delete and Generate Custom Mouse Pointer to Temp Path
            File.Delete(Path.GetTempPath() + "Normal.cur");
            File.Delete(Path.GetTempPath() + "Pointer.cur");
            File.WriteAllBytes(Path.GetTempPath() + "Normal.cur", Properties.Resources.Normal);
            File.WriteAllBytes(Path.GetTempPath() + "Pointer.cur", Properties.Resources.Pointer);


            // Program Title
            Title.Text = "Pocket Tactics | by @xcibe95x";
            Properties.Settings.Default.Version = localVersion.ToString();

            // Set League Cursor
            Cursor = NativeMethods.LoadCustomCursor(Path.GetTempPath() + "Normal.cur");


            toolStripTextBox1.Text = Properties.Settings.Default.SummonerName;
            toolStripComboBox1.Text = Properties.Settings.Default.Server;
            rItem = "Neeko's Help";
            rDesc = @"<div style=""color: #87ceeb; font-size: 10px; width:320px""><li style=""max-width:60em; word-wrap:break-word; overflow-wrap: break-word"">Place on a champion to create a 1-star copy of that champion and add it to your bench.</li></div>";
            ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.neekoshelp;

            WebClient client = new WebClient();
            versionJSON = client.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");
            itemsJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Items.php");
            tiersJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Tierlist.php");
            champsJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Champions.php");
            originsJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Origins.php");
            classesJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Classes.php");
            compsJSON = client.DownloadString("https://dev.playconstraints.com/apps/PocketTactics/Comps.php");

            Properties.Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, false, true);
            ResourceSet set = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            foreach (DictionaryEntry o in set)
            {
                ResourcesList.Add((string)o.Key);
            }
            rm.ReleaseAllResources();


            // Check for new Release
            string Ver = client.DownloadString("https://raw.githubusercontent.com/xcibe95x/TFT-Overlay/master/VERSION.md");


            var netVersion = new Version(Ver);

            if (netVersion > localVersion)
            {
                if (MessageBox.Show("Version: " + netVersion + " is now available on GitHub, it's reccomended to updated, proceed?", "GitHub.com", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://github.com/xcibe95x/TFT-Overlay/releases/" + Ver);
                }
            }


            RankedArmor.Load("https://s3-us-west-2.amazonaws.com/blitz-client-static-all/ranks/default.png");
            RankedArmor.SizeMode = PictureBoxSizeMode.StretchImage;

            try
            {
                // GET JSON DATA
                summonerJSON = client.DownloadString("https://" + Properties.Settings.Default.Server + ".api.riotgames.com/lol/summoner/v4/summoners/by-name/" + SummonerName + "?api_key=" + ApiKey);


                JObject summonerObject = JObject.Parse(summonerJSON);
                string SummonerId = (string)summonerObject.SelectToken("id");
                Summoner.Text = SummonerName;

                pictureBox2.Load("http://ddragon.leagueoflegends.com/cdn/9.14.1/img/profileicon/" + (string)summonerObject.SelectToken("profileIconId") + ".png");

                rankedJSON = client.DownloadString("https://" + Properties.Settings.Default.Server + ".api.riotgames.com/lol/league/v4/entries/by-summoner/" + SummonerId + "?api_key=" + ApiKey);



                JArray rankedArray = JArray.Parse(rankedJSON);
                int QueueIndex = 0;
                string TFTQueue = (string)rankedArray.SelectToken("[" + QueueIndex + "].queueType");
                string TFTTier = (string)rankedArray.SelectToken("[" + QueueIndex + "].tier");
                string TFTRank = (string)rankedArray.SelectToken("[" + QueueIndex + "].rank");
                string TFTLp = (string)rankedArray.SelectToken("[" + QueueIndex + "].leaguePoints");
                string TFTWins = (string)rankedArray.SelectToken("[" + QueueIndex + "].wins");
                string TFTLosses = (string)rankedArray.SelectToken("[" + QueueIndex + "].losses");
                Wins = (int)rankedArray.SelectToken("[" + QueueIndex + "].wins");
                Loss = (int)rankedArray.SelectToken("[" + QueueIndex + "].losses");
                JArray rankedLoop = (JArray)rankedArray.SelectToken("$");

                // Force TFT Ranking
                foreach (JToken arrayname in rankedLoop)
                {
                    if (TFTQueue != "RANKED_TFT")
                    {
                        QueueIndex++;
                        TFTQueue = (string)rankedArray.SelectToken("[" + QueueIndex + "].queueType");
                        TFTTier = (string)rankedArray.SelectToken("[" + QueueIndex + "].tier");
                        TFTRank = (string)rankedArray.SelectToken("[" + QueueIndex + "].rank");
                        TFTLp = (string)rankedArray.SelectToken("[" + QueueIndex + "].leaguePoints");
                        TFTWins = (string)rankedArray.SelectToken("[" + QueueIndex + "].wins");
                        TFTLosses = (string)rankedArray.SelectToken("[" + QueueIndex + "].losses");
                        Wins = (int)rankedArray.SelectToken("[" + QueueIndex + "].wins");
                        Loss = (int)rankedArray.SelectToken("[" + QueueIndex + "].losses");
                    }
                }

                string RankNumber = "";
                if (TFTRank == "IV")
                {
                    RankNumber = "4";
                }
                if (TFTRank == "III")
                {
                    RankNumber = "3";
                }
                if (TFTRank == "II")
                {
                    RankNumber = "2";
                }
                if (TFTRank == "I")
                {
                    RankNumber = "1";
                }
                RankedArmor.Load("https://s3-us-west-2.amazonaws.com/blitz-client-static-all/ranks/" + TFTTier.ToLower() + "_" + RankNumber + ".png");
                RankTierLabel.Text = TFTTier + " " + TFTRank;


                if (TFTQueue != "RANKED_TFT")
                {

                    QueueIndex = 0;
                    RankedArmor.Load("https://s3-us-west-2.amazonaws.com/blitz-client-static-all/ranks/default.png");

                }

            }
            catch { }



            JArray jArray22 = JArray.Parse(versionJSON);
            lolVer = (string)jArray22.SelectToken("[0]");


            // FIX TIER LIST
            metroComboBox1.SelectedIndex = 0;
            TierListBox.SelectedIndex = 0;

            // WINRATE LABELS
            WinLab.Text = Wins.ToString() + " W";
            LoseLab.Text = Loss.ToString() + " L";

            // CALL WINRATE CALCULATOR
            CalculateWR();

            // LOAD CHAMPIONS LIST
            ChampionsListLoop();
            OriginsListLoop();
            ClassesListLoop();

        } // END OF FORM LOAD


        //private string FirstLetterCapital(string str)
        // {
        //    return Char.ToUpper(str[0]) + str.Remove(0, 1);
        //  }

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
                //Lord's Edge
                rItem = (string)jObject.SelectToken("deathblade.name");
                rDesc = (string)jObject.SelectToken("deathblade.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.deathblade;

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
                rItem = (string)jObject.SelectToken("giantslayer.name");
                rDesc = (string)jObject.SelectToken("giantslayer.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.giantslayer;

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
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.yuumii;

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


            if (Item1 == "Glove" && Item2 == "Glove" || Item2 == "Glove" && Item1 == "Glove")
            {
                //Thiefs Gloves
                rItem = (string)jObject.SelectToken("thiefsgloves.name");
                rDesc = (string)jObject.SelectToken("thiefsgloves.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.thiefsgloves;

            }


            if (Item1 == "Glove" && Item2 == "Tear" || Item2 == "Glove" && Item1 == "Tear")
            {
                //Hand of Justice
                rItem = (string)jObject.SelectToken("handofjustice.name");
                rDesc = (string)jObject.SelectToken("handofjustice.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.handofjustice;

            }

            if (Item1 == "Glove" && Item2 == "BF" || Item2 == "Glove" && Item1 == "BF")
            {
                //Infinity Edge
                rItem = (string)jObject.SelectToken("infinityedge.name");
                rDesc = (string)jObject.SelectToken("infinityedge.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.infinityedge;

            }

            if (Item1 == "Glove" && Item2 == "Rod" || Item2 == "Glove" && Item1 == "Rod")
            {
                //Jeweled Gauntlet
                rItem = (string)jObject.SelectToken("jeweledgauntlet.name");
                rDesc = (string)jObject.SelectToken("jeweledgauntlet.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.jeweledgauntlet;

            }

            if (Item1 == "Glove" && Item2 == "Cape" || Item2 == "Glove" && Item1 == "Cape")
            {
                //Quick Silver
                rItem = (string)jObject.SelectToken("quicksilver.name");
                rDesc = (string)jObject.SelectToken("quicksilver.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.quicksilver;

            }

            if (Item1 == "Glove" && Item2 == "Vest" || Item2 == "Glove" && Item1 == "Vest")
            {
                //Iceborne Gauntlet
                rItem = (string)jObject.SelectToken("iceborngauntlet.name");
                rDesc = (string)jObject.SelectToken("iceborngauntlet.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.iceborngauntlet;

            }

            if (Item1 == "Glove" && Item2 == "Belt" || Item2 == "Glove" && Item1 == "Belt")
            {
                //Backhand
                rItem = (string)jObject.SelectToken("trapclaw.name");
                rDesc = (string)jObject.SelectToken("trapclaw.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.trapclaw;

            }

            if (Item1 == "Glove" && Item2 == "Bow" || Item2 == "Glove" && Item1 == "Bow")
            {
                //Repeating Crossbow
                rItem = (string)jObject.SelectToken("repeatingcrossbow.name");
                rDesc = (string)jObject.SelectToken("repeatingcrossbow.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.repeatingcrossbow;

            }

            if (Item1 == "Glove" && Item2 == "Spatula" || Item2 == "Glove" && Item1 == "Spatula")
            {
                //Mittens
                rItem = (string)jObject.SelectToken("mittens.name");
                rDesc = (string)jObject.SelectToken("mittens.bonus");
                rTier = "";
                ResultItemImage.BackgroundImage = TFT_Overlay.Properties.Resources.mittens;

            }


        }


        // ITEMS SET 1
        private void Button2_Click(object sender, EventArgs e)
        {
            Item1 = "BF";
            BF.ForeColor = Color.FromArgb(205, 61, 18);
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Item1 = "Vest";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.FromArgb(205, 61, 18);
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Item1 = "Belt";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.FromArgb(205, 61, 18);
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Item1 = "Rod";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.FromArgb(205, 61, 18);
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Item1 = "Cape";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.FromArgb(205, 61, 18);
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Item1 = "Bow";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.FromArgb(205, 61, 18);
            Tear.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Item1 = "Tear";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.FromArgb(205, 61, 18);
            Glove.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Item1 = "Glove";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.DarkSlateBlue;
            Glove.ForeColor = Color.FromArgb(205, 61, 18);
            DoCheck();
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            Item1 = "Spatula";
            BF.ForeColor = Color.DarkSlateBlue;
            Vest.ForeColor = Color.DarkSlateBlue;
            Belt.ForeColor = Color.DarkSlateBlue;
            Rod.ForeColor = Color.DarkSlateBlue;
            Cape.ForeColor = Color.DarkSlateBlue;
            Bow.ForeColor = Color.DarkSlateBlue;
            Tear.ForeColor = Color.DarkSlateBlue;
            Spatula.ForeColor = Color.FromArgb(205, 61, 18);
            Glove.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        // ITEMS SET 2

        private void Button18_Click(object sender, EventArgs e)
        {
            Item2 = "Glove";
            Glove2.ForeColor = Color.FromArgb(205, 61, 18);
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Item2 = "BF";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.FromArgb(205, 61, 18);
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Item2 = "Vest";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.FromArgb(205, 61, 18);
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            Item2 = "Belt";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.FromArgb(205, 61, 18);
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            Item2 = "Rod";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.FromArgb(205, 61, 18);
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            Item2 = "Cape";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.FromArgb(205, 61, 18);
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            Item2 = "Bow";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.FromArgb(205, 61, 18);
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            Item2 = "Tear";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.FromArgb(205, 61, 18);
            Spatula2.ForeColor = Color.DarkSlateBlue;
            DoCheck();
        }

        private void Button16_Click(object sender, EventArgs e)
        {

            Item2 = "Spatula";
            Glove2.ForeColor = Color.DarkSlateBlue;
            BF2.ForeColor = Color.DarkSlateBlue;
            Vest2.ForeColor = Color.DarkSlateBlue;
            Belt2.ForeColor = Color.DarkSlateBlue;
            Rod2.ForeColor = Color.DarkSlateBlue;
            Cape2.ForeColor = Color.DarkSlateBlue;
            Bow2.ForeColor = Color.DarkSlateBlue;
            Tear2.ForeColor = Color.DarkSlateBlue;
            Spatula2.ForeColor = Color.FromArgb(205, 61, 18);
            DoCheck();
        }


        private void ResultItemImage_BackgroundImageChanged(object sender, EventArgs e)
        {
            ItemName.Text = rItem;
            htmlItemdescription.Text = @"<div style=""color: #87ceeb; font-size: 10px; width:320px""><li style=""max-width:60em; word-wrap:break-word; overflow-wrap: break-word"">" + rDesc + "</li></div>";
        }


        private void AlwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TopMost == true)
            {
                Properties.Settings.Default.TopMost = false;
                TopMost = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.TopMost = true;
                TopMost = true;
                Properties.Settings.Default.Save();
            }
        }



        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
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



            JObject jObjectw = JObject.Parse(champsJSON);





            foreach (JToken arrayname in championsTiers)
            {

                int originsIndex = 0;
                int classesIndex = 0;





                string champName = (string)jObject.SelectToken(TierType + "." + TierIndex + "[" + champIndex.ToString() + "]");
                int cost = (int)jObjectw.SelectToken(champName + ".cost");





                var picture = new PictureBox
                {
                    Name = "pictureBox",
                    Size = new Size(84, 84),
                    //Padding = new Padding(5, 5, 5, 5),
                    Dock = DockStyle.None,
                    Anchor = AnchorStyles.None,
                    Location = new Point(0, 0),
                    BackgroundImage = (Image)(rm.GetObject(champName)),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };

                var basepanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.None,
                    Anchor = AnchorStyles.None,
                    BackColor = Color.Transparent,
                    Location = new Point(3, 52),
                    FlowDirection = FlowDirection.RightToLeft,
                    //AutoSize = true,
                    //AutoSizeMode = AutoSizeMode.GrowOnly,
                    Size = new Size(picture.Width - 6, picture.Height - 55),

                };


                if (ResourcesList.Contains(champName))
                {
                    picture.Controls.Add(basepanel);
                }
                else
                {
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture.Load("https://ddragon.leagueoflegends.com/cdn/" + lolVer + "/img/champion/" + champName + ".png");
                    picture.Controls.Add(basepanel);
                }




                flowLayoutPanel1.Controls.Add(picture);

                var tierbox = new PictureBox
                {
                    Name = "pictureBox",
                    Size = new Size(84, 84),
                    Location = new Point(0, 0),
                    BackColor = Color.Transparent,
                    BackgroundImage = (Image)(rm.GetObject("Tier" + cost.ToString())),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };

                picture.Controls.Add(tierbox);

                var label = new Label
                {
                    Text = cost.ToString(),
                    ForeColor = Color.White,
                    //Size = new Size(84, 84),
                    Location = new Point(0, 0),
                    BackColor = Color.Transparent,
                    Font = new Font(Font, FontStyle.Bold),

                };

                var coin = new PictureBox
                {
                    Name = "pictureBox",
                    Size = new Size(8, 8),
                    Location = new Point(12, 3),
                    BackColor = Color.Transparent,
                    BackgroundImage = (Image)(rm.GetObject("Coin")),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };


                tierbox.Controls.Add(label);
                label.Controls.Add(coin);






                // ORIGIN

                JArray originsLoop = (JArray)jObjectw.SelectToken(champName + ".origin");

                foreach (JToken arrayname2 in originsLoop)
                {

                    string origins = (string)jObjectw.SelectToken(champName + ".origin.[" + originsIndex.ToString() + "]");
                    Console.WriteLine(origins);

                    using (PictureBox defaultHex = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(22, 22),
                        Margin = new Padding(2, 5, 2, 0),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        //Location = new Point(10, 50),
                        BackColor = Color.Transparent,
                        BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                        BackgroundImageLayout = ImageLayout.Stretch,


                    })
                    {
                        using (PictureBox newOrigins = new PictureBox
                        {
                            Name = "newOrigins",
                            Size = new Size(13, 13),
                            Padding = new Padding(5, 5, 5, 5),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.Fill,
                            //Location = new Point(10, 50),
                            BackColor = Color.Transparent,

                        })
                        {
                            using (PictureBox originsBox = new PictureBox
                            {
                                Name = "basepanel",
                                Margin = new Padding(2, 5, 2, 0),
                                Size = new Size(22, 22),
                                Anchor = AnchorStyles.None,
                                Dock = DockStyle.None,
                                BackColor = Color.Transparent,
                                BackgroundImage = (Image)(rm.GetObject(origins)),
                                BackgroundImageLayout = ImageLayout.Stretch,
                            })
                            {
                                if (ResourcesList.Contains(origins))
                                {
                                    basepanel.Controls.Add(originsBox);
                                    PocketTips.SetToolTip(originsBox, origins);
                                }
                                else
                                {

                                    newOrigins.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + origins + ".png");
                                    newOrigins.SizeMode = PictureBoxSizeMode.StretchImage;
                                    basepanel.Controls.Add(defaultHex);
                                    defaultHex.Controls.Add(newOrigins);
                                    PocketTips.SetToolTip(defaultHex, origins);
                                    PocketTips.SetToolTip(newOrigins, origins);

                                }
                            }
                        }
                    }


                    originsIndex++;
                }

                // CLASSES

                JArray classesLoop = (JArray)jObjectw.SelectToken(champName + ".class");

                foreach (JToken arrayname3 in classesLoop)
                {


                    string classes = (string)jObjectw.SelectToken(champName + ".class.[" + classesIndex.ToString() + "]");

                    using (PictureBox defaultHex = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(22, 22),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Margin = new Padding(2, 5, 2, 0),
                        //Location = new Point(10, 50),
                        BackColor = Color.Transparent,
                        BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                        BackgroundImageLayout = ImageLayout.Stretch,


                    })
                    {
                        using (PictureBox newClass = new PictureBox
                        {
                            Size = new Size(13, 13),
                            Padding = new Padding(5, 5, 5, 5),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.Fill,
                            //Location = new Point(10, 50),
                            BackColor = Color.Transparent,

                        })
                        {
                            using (PictureBox classBox = new PictureBox
                            {
                                Name = "basepanel",
                                Size = new Size(22, 22),
                                Margin = new Padding(2, 5, 2, 0),
                                Anchor = AnchorStyles.None,
                                Dock = DockStyle.None,
                                BackColor = Color.Transparent,
                                BackgroundImage = (Image)(rm.GetObject(classes)),
                                BackgroundImageLayout = ImageLayout.Stretch,
                            })
                            {
                                if (ResourcesList.Contains(classes))
                                {
                                    basepanel.Controls.Add(classBox);
                                    PocketTips.SetToolTip(classBox, classes);
                                }
                                else
                                {
                                    newClass.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + classes + ".png");
                                    newClass.SizeMode = PictureBoxSizeMode.StretchImage;
                                    basepanel.Controls.Add(defaultHex);
                                    defaultHex.Controls.Add(newClass);
                                    PocketTips.SetToolTip(defaultHex, classes);
                                    PocketTips.SetToolTip(newClass, classes);
                                }
                            }
                        }
                    }


                    classesIndex++;
                }



                basepanel.BringToFront();
                TierListBox.BringToFront();
                label.BringToFront();

                champIndex++;
            }



        }



        // LVL UP BUTTON
        // PROBABILITIES
        private void MetroButton1_Click_1(object sender, EventArgs e)
        {
            Title.Focus();


            if (Levels < 9)
            {
                Levels++;
            }
            else
            {
                Levels = 1;
                Lvl.Text = "Level " + 1;
                T1P.Text = "100%";
                T2P.Text = "0%";
                T3P.Text = "0%";
                T4P.Text = "0%";
                T5P.Text = "0%";

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
                    T1P.Text = "70%";
                    T2P.Text = "25%";
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
                    T1P.Text = "35%";
                    T2P.Text = "35%";
                    T3P.Text = "25%";
                    T4P.Text = "5%";
                    T5P.Text = "-";
                    break;
                case 6:
                    Lvl.Text = "Level 6";
                    T1P.Text = "25%";
                    T2P.Text = "35%";
                    T3P.Text = "30%";
                    T4P.Text = "10%";
                    T5P.Text = "-";
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
                    T3P.Text = "33%";
                    T4P.Text = "30%";
                    T5P.Text = "12%";
                    break;
            }
        }



        private void MetroButton2_Click(object sender, EventArgs e)
        {

        }

        private void MetroTabPage2_Click(object sender, EventArgs e)
        {

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


        private void MetroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Title.Focus();
            flowLayoutPanel1.Controls.Clear();
            TierIndex = TierListBox.SelectedIndex + 1;
            GetTierList(TierIndex, TierType);

        }

        private void MetroComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Title.Focus();
            flowLayoutPanel1.Controls.Clear();
            TierType = metroComboBox1.SelectedItem.ToString().ToLower();
            GetTierList(TierIndex, TierType);
        }

        private void MetroButton4_Click(object sender, EventArgs e)
        {
            Title.Focus();
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form2 frm2 = new Form2())
            {
                frm2.ShowDialog();
            }
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

        private void ChampionsListLoop()
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


                if (cost == 1)
                {
                    tiercost = Color.DimGray;
                }

                if (cost == 2)
                {
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


                // DEBUG LINE
                //Console.WriteLine((string)jObject.SelectToken(key + ".name"));

                // CODE
                var basepanel = new FlowLayoutPanel
                {
                    Name = key,
                    Text = cost.ToString(),
                    Size = new Size(312, 48),

                };
                ChampsList.Controls.Add(basepanel);


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
                    BackgroundImage = (Image)(rm.GetObject(key)),
                    BackgroundImageLayout = ImageLayout.Stretch,

                };



                if (ResourcesList.Contains(key))
                {
                    ChampBox.Controls.Add(ChampPicture);
                }
                else
                {
                    ChampPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    ChampPicture.Load("https://ddragon.leagueoflegends.com/cdn/" + lolVer + "/img/champion/" + key + ".png");
                    ChampBox.Controls.Add(ChampPicture);
                }



                // CHAMP COST
                var ChampCost = new MetroLabel
                {

                    Name = cost.ToString(),
                    Text = cost.ToString(),
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    ForeColor = tiercost,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                    Size = new Size(12, 18),

                };
                PocketTips.SetToolTip(ChampCost, "Cost");
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
                PocketTips.SetToolTip(CostIcon, "Cost");
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

                    using (PictureBox picturebox = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Location = new Point(0, 0),
                        BackgroundImage = (Image)(rm.GetObject(origins)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    })
                    {
                        using (PictureBox defaultHex = new PictureBox
                        {
                            Name = "basepanel",
                            Size = new Size(25, 25),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.None,
                            //Location = new Point(10, 50),
                            BackColor = Color.Transparent,
                            BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                            BackgroundImageLayout = ImageLayout.Stretch,


                        })
                        {
                            using (PictureBox newOrigins = new PictureBox
                            {
                                Size = new Size(13, 13),
                                Padding = new Padding(5, 5, 5, 5),
                                Anchor = AnchorStyles.None,
                                Dock = DockStyle.Fill,
                                //Location = new Point(10, 50),
                                BackColor = Color.Transparent,

                            })
                            {
                                if (ResourcesList.Contains(origins))
                                {
                                    basepanel.Controls.Add(picturebox);
                                    PocketTips.SetToolTip(picturebox, origins);
                                }
                                else
                                {
                                    newOrigins.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + origins + ".png");
                                    newOrigins.SizeMode = PictureBoxSizeMode.StretchImage;
                                    basepanel.Controls.Add(defaultHex);
                                    defaultHex.Controls.Add(newOrigins);
                                    PocketTips.SetToolTip(defaultHex, origins);
                                    PocketTips.SetToolTip(newOrigins, origins);

                                }
                            }
                        }
                    }



                    originsIndex++;
                }


                // CLASSES

                JArray classesLoop = (JArray)jObject.SelectToken(key + ".class");

                foreach (JToken arrayname in classesLoop)
                {


                    string classes = (string)jObject.SelectToken(key + ".class.[" + classesIndex.ToString() + "]");

                    using (PictureBox picturebox = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        Location = new Point(0, 0),
                        BackgroundImage = (Image)(rm.GetObject(classes)),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    })
                    {
                        using (PictureBox defaultHex = new PictureBox
                        {
                            Name = "basepanel",
                            Size = new Size(25, 25),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.None,
                            //Location = new Point(10, 50),
                            BackColor = Color.Transparent,
                            BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                            BackgroundImageLayout = ImageLayout.Stretch,
                        })
                        {
                            using (PictureBox newClasses = new PictureBox
                            {
                                Size = new Size(13, 13),
                                Padding = new Padding(5, 5, 5, 5),
                                Anchor = AnchorStyles.None,
                                Dock = DockStyle.Fill,
                                //Location = new Point(10, 50),
                                BackColor = Color.Transparent,
                            })
                            {
                                if (ResourcesList.Contains(classes))
                                {
                                    basepanel.Controls.Add(picturebox);
                                    PocketTips.SetToolTip(picturebox, classes);
                                }
                                else
                                {
                                    newClasses.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + classes + ".png");
                                    newClasses.SizeMode = PictureBoxSizeMode.StretchImage;
                                    basepanel.Controls.Add(defaultHex);
                                    defaultHex.Controls.Add(newClasses);
                                    PocketTips.SetToolTip(defaultHex, classes);
                                    PocketTips.SetToolTip(newClasses, classes);
                                }
                            }
                        }
                    }

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
                    PocketTips.SetToolTip(picturebox, itemization);
                }

                //WRAP TIPS
                Regex rgx = new Regex("(.{50}\\s)"); string WrappedMessage = rgx.Replace(ability, "$1\n");

                // TOOLTIPS
                PocketTips.SetToolTip(ChampBox, WrappedMessage);
                PocketTips.SetToolTip(ChampPicture, WrappedMessage);
            





            }

        }





        private void TextBox1_TextChanged(object sender, EventArgs e)
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
                ChampionsListLoop();
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                ((TextBox)sender).Clear();
            }
        }

        private void MetroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Deprecated
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();

        }

        private void AscendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();

            for (int i = 0; i < 5; i++)
            {
                foreach (Control SearchPanel in ChampsList.Controls)
                {


                    if (!SearchPanel.Text.ToLower().Contains("1".ToLower()))
                    {
                        ChampsList.Controls.Remove(SearchPanel);
                    }

                }
            }
        }

        private void DescendingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();
            for (int i = 0; i < 5; i++)
            {
                foreach (Control SearchPanel in ChampsList.Controls)
                {


                    if (!SearchPanel.Text.ToLower().Contains("2".ToLower()))
                    {
                        ChampsList.Controls.Remove(SearchPanel);
                    }

                }
            }
        }

        private void Cost3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();

            for (int i = 0; i < 5; i++)
            {
                foreach (Control SearchPanel in ChampsList.Controls)
                {


                    if (!SearchPanel.Text.ToLower().Contains("3".ToLower()))
                    {
                        ChampsList.Controls.Remove(SearchPanel);
                    }

                }
            }
        }

        private void Cost4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();
            for (int i = 0; i < 5; i++)
            {
                foreach (Control SearchPanel in ChampsList.Controls)
                {


                    if (!SearchPanel.Text.ToLower().Contains("4".ToLower()))
                    {
                        ChampsList.Controls.Remove(SearchPanel);
                    }

                }
            }
        }

        private void Cost5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();

            for (int i = 0; i < 5; i++)
            {
                foreach (Control SearchPanel in ChampsList.Controls)
                {


                    if (!SearchPanel.Text.ToLower().Contains("5".ToLower()))
                    {
                        ChampsList.Controls.Remove(SearchPanel);
                    }

                }
            }
        }


        // ORIGINS LIST

        private void OriginsListLoop()
        {

            JObject jObject = JObject.Parse(originsJSON);

            // ORIGINS LIST (FOREACH)
            var OriginsList = jObject.SelectTokens("$").ToList();


            foreach (JProperty item in OriginsList.Children())
            {

                // VARIABLES
                var originKey = item.Name.ToString();
                string originK = (string)jObject.SelectToken(originKey + ".key");
                string originName = (string)jObject.SelectToken(originKey + ".name");
                string originDesc = (string)jObject.SelectToken(originKey + ".description");


                // CODE
                var DrawPanel = new FlowLayoutPanel
                {
                    Name = originK,
                    Size = new Size(312, 48),
                    BackColor = Color.FromArgb(33, 42, 59),
                    Location = new Point(0, 0),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(1, 1, 1, 1),


                };


                DrawPanel.Click += new EventHandler(DrawPanel_Click);
                OriginsFlowPanel.Controls.Add(DrawPanel);

                //SPACER
                var DrawSpacer = new Panel
                {
                    Name = "ChampBox",
                    Size = new Size(0, 45),
                    Padding = new Padding(2, 2, 2, 2),

                };

                DrawPanel.Controls.Add(DrawSpacer);


                // ORIGIN ICON
                using (PictureBox DrawOrigin = new PictureBox
                {
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Size = new Size(22, 22),
                    Location = new Point(0, 0),
                    BackColor = Color.Transparent,
                    BackgroundImage = (Image)(rm.GetObject(originName)),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Enabled = false,
                })
                {
                    using (PictureBox defaultHex = new PictureBox
                    {
                        Name = "basepanel",
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        BackColor = Color.Transparent,
                        BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    })
                    {
                        using (PictureBox newOrigin = new PictureBox
                        {
                            Size = new Size(13, 13),
                            Padding = new Padding(5, 5, 5, 5),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.Fill,
                            BackColor = Color.Transparent,
                        })
                        {
                            if (ResourcesList.Contains(originName))
                            {
                                DrawPanel.Controls.Add(DrawOrigin);
                            }
                            else
                            {
                                newOrigin.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + originName + ".png");
                                newOrigin.SizeMode = PictureBoxSizeMode.StretchImage;
                                DrawPanel.Controls.Add(defaultHex);
                                defaultHex.Controls.Add(newOrigin);
                            }
                        }
                    }
                }

                var DrawLabel = new MetroLabel
                {
                    Text = originName,
                    ForeColor = Color.White,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    Margin = new Padding(3, 10, 3, 3),
                    FontSize = MetroFramework.MetroLabelSize.Small,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Location = new Point(0, 0),
                    Enabled = false,
                };
                DrawPanel.Controls.Add(DrawLabel);
            }

        }

        // CLASSES LIST

        private void ClassesListLoop()
        {

            JObject jObject = JObject.Parse(classesJSON);

            // ORIGINS LIST (FOREACH)
            var ClassesList = jObject.SelectTokens("$").ToList();


            foreach (JProperty item in ClassesList.Children())
            {

                // VARIABLES
                var classKey = item.Name.ToString();
                string classK = (string)jObject.SelectToken(classKey + ".key");
                string className = (string)jObject.SelectToken(classKey + ".name");
                string classDesc = (string)jObject.SelectToken(classKey + ".description");


                // CODE
                var DrawPanel = new FlowLayoutPanel
                {
                    Name = classK,
                    Size = new Size(312, 48),
                    BackColor = Color.FromArgb(33, 42, 59),
                    Location = new Point(0, 0),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(1, 1, 1, 1),


                };


                DrawPanel.Click += new EventHandler(DrawPanel2_Click);
                ClassesFlowPanel.Controls.Add(DrawPanel);

                //SPACER
                var DrawSpacer = new Panel
                {
                    Name = "ChampBox",
                    Size = new Size(0, 45),
                    Padding = new Padding(2, 2, 2, 2),

                };

                DrawPanel.Controls.Add(DrawSpacer);


                // ORIGIN ICON
                using (PictureBox DrawClass = new PictureBox
                {
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Size = new Size(22, 22),
                    Location = new Point(0, 0),
                    BackColor = Color.Transparent,
                    BackgroundImage = (Image)(rm.GetObject(className)),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Enabled = false,
                })
                {
                    using (PictureBox defaultHex = new PictureBox
                    {
                        Size = new Size(25, 25),
                        Anchor = AnchorStyles.None,
                        Dock = DockStyle.None,
                        //Location = new Point(10, 50),
                        BackColor = Color.Transparent,
                        BackgroundImage = (Image)(rm.GetObject("DefaultHex")),
                        BackgroundImageLayout = ImageLayout.Stretch,
                    })
                    {
                        using (PictureBox newClasses = new PictureBox
                        {
                            Size = new Size(13, 13),
                            Padding = new Padding(5, 5, 5, 5),
                            Anchor = AnchorStyles.None,
                            Dock = DockStyle.Fill,
                            //Location = new Point(10, 50),
                            BackColor = Color.Transparent,
                        })
                        {
                            if (ResourcesList.Contains(className))
                            {
                                DrawPanel.Controls.Add(DrawClass);
                            }
                            else
                            {
                                newClasses.Load("https://img.rankedboost.com/wp-content/plugins/league/assets/tft/" + className + ".png");
                                newClasses.SizeMode = PictureBoxSizeMode.StretchImage;
                                DrawPanel.Controls.Add(defaultHex);
                                defaultHex.Controls.Add(newClasses);
                            }
                        }
                    }
                }

                var DrawLabel = new MetroLabel
                {
                    Text = className,
                    ForeColor = Color.White,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    Margin = new Padding(3, 10, 3, 3),
                    FontSize = MetroFramework.MetroLabelSize.Small,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                    Anchor = AnchorStyles.None,
                    Dock = DockStyle.None,
                    Location = new Point(0, 0),
                    Enabled = false,
                };
                DrawPanel.Controls.Add(DrawLabel);
            }

        }

        private void DrawPanel_Click(object sender, EventArgs e)
        {

            richTextBox1.Clear();
            OriginsDescriptionFlow.Controls.Clear();


            JObject jObject = JObject.Parse(originsJSON);
            string Description = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".description");

            if (Description != null) { OriginsDescriptionFlow.Controls.Add(richTextBox1); }

            JArray championsTiers = (JArray)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses");

            int originsCount = 0;

            foreach (JToken arrayname in championsTiers)
            {

                string ChampsNeeded = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses[" + originsCount + "].needed");
                string ChampsEffect = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses[" + originsCount + "].effect");

                richTextBox1.Text = Description;


                var DrawLabel = new MetroLabel
                {
                    Text = ChampsNeeded,
                    AutoSize = true,
                    ForeColor = Color.White,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                    Anchor = AnchorStyles.Left,
                    Dock = DockStyle.None,

                };


                var DrawLabel2 = new Label
                {

                    AutoSize = true,
                    Text = ChampsEffect,
                    ForeColor = Color.White,
                    Anchor = AnchorStyles.Left,
                    Dock = DockStyle.None,
                };

                OriginsDescriptionFlow.Controls.Add(DrawLabel);
                OriginsDescriptionFlow.Controls.Add(DrawLabel2);

                originsCount++;
            }

        }

        private void DrawPanel2_Click(object sender, EventArgs e)
        {

            richTextBox2.Clear();
            ClassesDescriptionFlow.Controls.Clear();


            JObject jObject = JObject.Parse(classesJSON);
            string Description = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".description");

            if (Description != null) { ClassesDescriptionFlow.Controls.Add(richTextBox2); }

            JArray championsTiers = (JArray)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses");

            int classesCount = 0;

            foreach (JToken arrayname in championsTiers)
            {

                string ChampsNeeded = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses[" + classesCount + "].needed");
                string ChampsEffect = (string)jObject.SelectToken((sender as FlowLayoutPanel).Name + ".bonuses[" + classesCount + "].effect");

                richTextBox2.Text = Description;


                var DrawLabel = new MetroLabel
                {
                    Text = ChampsNeeded,
                    AutoSize = true,
                    ForeColor = Color.White,
                    UseCustomBackColor = true,
                    UseCustomForeColor = true,
                    FontWeight = MetroFramework.MetroLabelWeight.Bold,
                    Anchor = AnchorStyles.Left,
                    Dock = DockStyle.None,

                };


                var DrawLabel2 = new Label
                {

                    AutoSize = true,
                    Text = ChampsEffect,
                    ForeColor = Color.White,
                    Anchor = AnchorStyles.Left,
                    Dock = DockStyle.None,
                };

                ClassesDescriptionFlow.Controls.Add(DrawLabel);
                ClassesDescriptionFlow.Controls.Add(DrawLabel2);

               classesCount++;
            }

        }

        private void ToolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Clear();
        }

        private void ToolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SummonerName = toolStripTextBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Application.Restart();
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void ProfileTAB_Click(object sender, EventArgs e)
        {

        }

        // HIDE TOOL
        private void MetroButton5_Click(object sender, EventArgs e)
        {
            Title.Focus();
            if (Height == 289) { Height = 28; metroButton5.Text = "v"; toggleHide = true; } else { Height = 289; metroButton5.Text = ">"; toggleHide = false; }
        }

        private void Panel1_MouseHover(object sender, EventArgs e)
        {
            if (toggleHide == true) { Height = 289; }
        }

        private void MetroButton3_Click(object sender, EventArgs e)
        {

            if (Switch.Text == "Data")
            {

                Indicator.Visible = false;
                Switch.Text = "Data 2";

                TabControl.TabPages.Clear();
                TabControl.TabPages.Insert(0, ProfileTAB);
                TabControl.TabPages.Insert(1, ProbTab);
                TabControl.TabPages.Insert(2, PDamageTab);
                TabControl.TabPages.Insert(3, OriginsTab);
                TabControl.TabPages.Insert(4, ClassTab);
            }
            else if (Switch.Text == "Data 2")
            {
                Indicator.Visible = false;
                Switch.Text = "Data";
                TabControl.TabPages.Clear();
                TabControl.TabPages.Insert(0, ProfileTAB);
                TabControl.TabPages.Insert(1, CraftingTab);
                TabControl.TabPages.Insert(2, ChampionsTab);
                TabControl.TabPages.Insert(3, TierListTab);
  
            }
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (TabControl.SelectedTab == ProfileTAB)
            {
                Indicator.Visible = false; Switch.Location = new Point(350, 32);
            }
            

                if (TabControl.SelectedTab == CraftingTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.manually;
                    PocketTips.SetToolTip(Indicator, "Data is always up to date, but latest items combinations are updated software-side.");
                }

                if (TabControl.SelectedTab == ChampionsTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "Always Up to Date!");
                }

                if (TabControl.SelectedTab == TierListTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "Always Up to Date!");
                }

                if (TabControl.SelectedTab == ProbTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "");
                }

                if (TabControl.SelectedTab == PDamageTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "");
                }

                if (TabControl.SelectedTab == OriginsTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "Always Up to Date!");
                }

                if (TabControl.SelectedTab == ClassTab)
                {
                Indicator.Visible = true; Switch.Location = new Point(330, 32);
                Indicator.BackgroundImage = Properties.Resources.cloud_data;
                    PocketTips.SetToolTip(Indicator, "Always Up to Date!");
                }

           
        }

        private void MetroButton3_Click_1(object sender, EventArgs e)
        {
                  
            TabControl.TabPages.Insert(TabControl.TabCount, settingsTab);
            TabControl.SelectedTab = settingsTab;
            Indicator.Visible = false;
        }

        private void MetroTrackBar1_ValueChanged(object sender, EventArgs e)
        {

            metroLabel1.Text = "Opacity: " + metroTrackBar1.Value + "%";
          this.Opacity = (double)metroTrackBar1.Value / 100.0;
          Properties.Settings.Default.Opacity = metroTrackBar1.Value;
          Properties.Settings.Default.Save();

        }

        private void TFTCrafter_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void SettingsTab_Leave(object sender, EventArgs e)
        {
            TabControl.TabPages.Remove(settingsTab);
        }

        private void Panel1_MouseLeave(object sender, EventArgs e)
        {
            if (toggleHide == true) { Height = 28; }
        }

        private void ToolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void MetroContextMenu1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedItem.Equals("EUW")) { Properties.Settings.Default.Server = "euw1"; }
            if (toolStripComboBox1.SelectedItem.Equals("NA")) { Properties.Settings.Default.Server = "na"; }
            if (toolStripComboBox1.SelectedItem.Equals("NA1")) { Properties.Settings.Default.Server = "na1"; }
            if (toolStripComboBox1.SelectedItem.Equals("EUNE")) { Properties.Settings.Default.Server = "eun1"; }
            if (toolStripComboBox1.SelectedItem.Equals("JP")) { Properties.Settings.Default.Server = "jp1"; }
            if (toolStripComboBox1.SelectedItem.Equals("KR")) { Properties.Settings.Default.Server = "kr"; }
            if (toolStripComboBox1.SelectedItem.Equals("LAN")) { Properties.Settings.Default.Server = "la1"; }
            if (toolStripComboBox1.SelectedItem.Equals("LAS")) { Properties.Settings.Default.Server = "la2"; }
            if (toolStripComboBox1.SelectedItem.Equals("OCE")) { Properties.Settings.Default.Server = "oc1"; }
            if (toolStripComboBox1.SelectedItem.Equals("TR")) { Properties.Settings.Default.Server = "tr1"; }
            if (toolStripComboBox1.SelectedItem.Equals("RU")) { Properties.Settings.Default.Server = "ru"; }
            if (toolStripComboBox1.SelectedItem.Equals("PBE")) { Properties.Settings.Default.Server = "pbe1"; }
            if (toolStripComboBox1.SelectedItem.Equals("BR")) { Properties.Settings.Default.Server = "br1"; }
            Properties.Settings.Default.Save();

        }

        private void MetroButton2_Click_1(object sender, EventArgs e)
        {
            Title.Focus();
            ChampsList.Controls.Clear();
            ChampsSearchBox.Clear();
            ChampionsListLoop();

        }

        private void MetroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Title.Focus();

            if (metroComboBox2.SelectedIndex == 0)
            {

                ChampsList.Controls.Clear();
                ChampsSearchBox.Clear();
                ChampionsListLoop();

                for (int i = 0; i < 5; i++)
                {
                    foreach (Control SearchPanel in ChampsList.Controls)
                    {


                        if (!SearchPanel.Text.ToLower().Contains("1".ToLower()))
                        {
                            ChampsList.Controls.Remove(SearchPanel);
                        }

                    }
                }

            }

            if (metroComboBox2.SelectedIndex == 1)
            {

                ChampsList.Controls.Clear();
                ChampsSearchBox.Clear();
                ChampionsListLoop();

                for (int i = 0; i < 5; i++)
                {
                    foreach (Control SearchPanel in ChampsList.Controls)
                    {


                        if (!SearchPanel.Text.ToLower().Contains("2".ToLower()))
                        {
                            ChampsList.Controls.Remove(SearchPanel);
                        }

                    }
                }

            }

            if (metroComboBox2.SelectedIndex == 2)
            {

                ChampsList.Controls.Clear();
                ChampsSearchBox.Clear();
                ChampionsListLoop();

                for (int i = 0; i < 5; i++)
                {
                    foreach (Control SearchPanel in ChampsList.Controls)
                    {


                        if (!SearchPanel.Text.ToLower().Contains("3".ToLower()))
                        {
                            ChampsList.Controls.Remove(SearchPanel);
                        }

                    }
                }

            }

            if (metroComboBox2.SelectedIndex == 3)
            {

                ChampsList.Controls.Clear();
                ChampsSearchBox.Clear();
                ChampionsListLoop();

                for (int i = 0; i < 5; i++)
                {
                    foreach (Control SearchPanel in ChampsList.Controls)
                    {


                        if (!SearchPanel.Text.ToLower().Contains("4".ToLower()))
                        {
                            ChampsList.Controls.Remove(SearchPanel);
                        }

                    }
                }

            }

            if (metroComboBox2.SelectedIndex == 4)
            {

                ChampsList.Controls.Clear();
                ChampsSearchBox.Clear();
                ChampionsListLoop();

                for (int i = 0; i < 5; i++)
                {
                    foreach (Control SearchPanel in ChampsList.Controls)
                    {


                        if (!SearchPanel.Text.ToLower().Contains("5".ToLower()))
                        {
                            ChampsList.Controls.Remove(SearchPanel);
                        }

                    }
                }

            }
        }

        private void ChampsSearchBox_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).Clear();
        }

        private void CraftingTab_Click(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TableLayoutPanel2_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.FromArgb(33, 48, 60), 1))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                if (e.Row == (panel.RowCount - 1))
                {
                    rectangle.Height -= 1;
                }

                if (e.Column == (panel.ColumnCount - 1))
                {
                    rectangle.Width -= 1;
                }

                e.Graphics.DrawRectangle(pen, rectangle);
            }
        }

        private void ToolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            Graphics g = e.Graphics;

            LinearGradientBrush b = new LinearGradientBrush(e.Bounds,
                 Color.FromArgb(21, 20, 24), Color.FromArgb(21, 20, 24), 45f);

            g.FillRectangle(b, e.Bounds);
            //Color.FromArgb(10, 27, 38)
            g.DrawRectangle(new Pen(Brushes.DarkCyan, 1), new Rectangle(e.Bounds.X, e.Bounds.Y,
                e.Bounds.Width - 1, e.Bounds.Height - 1));

            g.DrawString(e.ToolTipText, new Font(e.Font, FontStyle.Regular), Brushes.White,
                new PointF(e.Bounds.X + 5, e.Bounds.Y + 1)); // top layer

            b.Dispose();
            
        }

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {
       
          
        }
    }
}