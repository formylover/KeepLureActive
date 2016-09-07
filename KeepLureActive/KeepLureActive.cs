using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Frames;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepLureActive
{
    public class KeepLureActive : HBPlugin
    {
        public static KLASettings S = new KLASettings();
        const int AzsunaZoneId = 7334;
        const int HighmountainZoneId = 7503;
        const int StormheimZoneId = 7541;
        const int ValSharahZoneId = 7558;
        const int SuramarZoneId = 7637;
        const int Dalaran = 7502;
        const int MargossRetreat = 8270;
        //const int OpenOceanZone = 0;
        
        public override void Pulse()
        {
            if (!Me.Mounted && !Me.IsCasting && !Me.Combat && !Me.IsChanneling)
            {
                if (Me.ZoneId == Dalaran && Me.SubZoneId == MargossRetreat)
                {
                    infoLog(Me.SubZoneText + ":" + Me.SubZoneId.ToString());
                    if (UseItem(GetItemByID(Items.MarkOfAquaos), S.UseMarkOfAquaos))
                    {
                        return;
                    }
                }

                //infoLog(Me.ZoneText + ": " + Me.ZoneId.ToString());
                if (!HasFishingAura() && !ShouldPauseAfterLureUse() && IsInCorrectZone())
                {

                    // Azsuna
                    if (UseItem(GetItemByID(Items.AromaticMurlocSlime), Me.ZoneId == AzsunaZoneId && S.UseAromaticMurlocSlime)) { return; }
                    if (UseItem(GetItemByID(Items.PearlescentConch), Me.ZoneId == AzsunaZoneId && S.UsePearlescentConch)) { return; }
                    if (UseItem(GetItemByID(Items.RustyQueenfishBroach), Me.ZoneId == AzsunaZoneId && S.UseRustyQueenfishBroach)) { return; }

                    // Highmountain
                    if (UseItem(GetItemByID(Items.FrostWorm), Me.ZoneId == HighmountainZoneId && S.UseFrostWorm)) { return; }
                    if (UseItem(GetItemByID(Items.SalmonLure), Me.ZoneId == HighmountainZoneId && S.UseSalmonLure)) { return; }
                    if (UseItem(GetItemByID(Items.SwollenMurlocEgg), Me.ZoneId == HighmountainZoneId && S.UseSwollenMurlocEgg)) { return; } 

                    // Stormheim
                    if (UseItem(GetItemByID(Items.MoosehornHook), Me.ZoneId == StormheimZoneId && S.UseMoosehornHook)) { return; } // bait to get silverscale minnow
                    if (UseItem(GetItemByID(Items.SilverscaleMinnow), Me.ZoneId == StormheimZoneId && S.UseSilverscaleMinnow)) { return; }
                    if (UseItem(GetItemByID(Items.AncientVrykulRing), Me.ZoneId == StormheimZoneId && S.UseAncientVrykulRing)) { return; }
                    if (UseItem(GetItemByID(Items.SoggyDrakescale), Me.ZoneId == StormheimZoneId && S.UseSoggyDrakescale)) { return; }

                    // val'sharah
                    if (UseItem(GetItemByID(Items.RottenFishbone), Me.ZoneId == ValSharahZoneId && S.UseRottenFishbone)) { return; }
                    if (UseItem(GetItemByID(Items.NightmareNightcrawler), Me.ZoneId == ValSharahZoneId && S.UseNightmareNightcrawler)) { return; }
                    if (UseItem(GetItemByID(Items.DrownedThistleleaf), Me.ZoneId == ValSharahZoneId && S.UseDrownedThistleleaf)) { return; }

                    // Suramar
                    if (UseItem(GetItemByID(Items.DemonicDetrius), Me.ZoneId == SuramarZoneId && S.UseDemonicDetrius)) { return; }
                    if(UseItem(GetItemByID(Items.SleepingMurloc), Me.ZoneId == SuramarZoneId && S.UseSleepingMurloc)) { return; } // will drop Seerspine Puffers on the ground to be picked up.  May need to add somethign to do that.
                    if (UseItem(GetItemByID(Items.EnchantedLure), Me.ZoneId == SuramarZoneId && S.UseEnchantedLure)) { return; }

                    if (UseItem(GetItemByID(Items.StunnedAngryShark), IsInCorrectZone() && S.UseStunnedAngryShark)) { return; } // leaving this here for now.  may need to remove
                    if (UseItem(GetItemByID(Items.MessageInABeerBottle), IsInCorrectZone() && S.UseMessageInABeerBottle)) { return; }
                    if (UseItem(GetItemByID(Items.AxefishLure), IsInCorrectZone() && S.UseAxefishLure)) { return; }
                    if (UseItem(GetItemByID(Items.DecayedWhaleBlubber), IsInCorrectZone() && S.UseDecayedWhaleBlubber)) { return; }
                    if (UseItem(GetItemByID(Items.RavenousFly), S.UseRavenousFly)) { return; }


                    // No other bait and no lure attached, so try to use.
                    if (UseItem(GetItemByID(Items.ArcaneLure), !Me.HasAura(Auras.ArcaneLure))) { return; }

                    // Find some bait in bags if no Arcane Lure!
                    // TODO: Find bait
                }
            }
        }


        #region Helper Methods
        static string lastInformationMSG;
        public  void infoLog(string Message, params object[] args)
        {
            if (Message == lastInformationMSG) { return; }

            Logging.Write(System.Windows.Media.Colors.LightBlue, "[KeepLureActive]: {0}", Message, args);
            lastInformationMSG = Message;
        }
        bool IsInCorrectZone()
        {
            if (Me.ZoneId == AzsunaZoneId || Me.ZoneId == HighmountainZoneId || Me.ZoneId == StormheimZoneId || Me.ZoneId == SuramarZoneId || Me.ZoneId == ValSharahZoneId)
            {
                return true;
            }
            return false;
        }
        bool UseItem(WoWItem item, bool reqs, string log = null)
        {
            if (item == null || !reqs || !CanUseItem(item)) { return false; }

            infoLog(string.Format($"/use {item.Name}" + (String.IsNullOrEmpty(log) ? "" : " - " + log)));
            item.Use();
            _pauseTimer.Start();
            CommonCoroutines.SleepForLagDuration();
            return true;
        }
        bool CanUseItem(WoWItem item)
        {
            return item.Usable && item.Cooldown <= 0 && !MerchantFrame.Instance.IsVisible;
        }

        WoWItem GetItemByID(int itemId)
        {
            return Me.CarriedItems
                .FirstOrDefault(i => i.ItemInfo.Id == itemId);
        }
        private static Stopwatch _pauseTimer = new Stopwatch();
        bool ShouldPauseAfterLureUse()
        {
            if (_pauseTimer.IsRunning && _pauseTimer.ElapsedMilliseconds < (S.PauseSecondsAfterUseLure * 1000))
            {
                return true;
            }
            if (_pauseTimer.IsRunning) { _pauseTimer.Stop(); }
            return false;
        }
        bool HasFishingAura()
        {
            if (Me.HasAura(Auras.ArcaneLure)
                || Me.HasAura(Auras.AromaticMurlocSlime)
                || Me.HasAura(Auras.PearlescentConch)
                || Me.HasAura(Auras.RustyQueenfishBroach)
                || Me.HasAura(Auras.FrostWorm)
                || Me.HasAura(Auras.SalmonLure)
                || Me.HasAura(Auras.BlessingOfTheMurlocs)
                || Me.HasAura(Auras.MoosehornHook)
                || Me.HasAura(Auras.SilverscaleMinnow)
                || Me.HasAura(Auras.AncientVrykulRing)
                || Me.HasAura(Auras.SoggyDrakescale)
                || Me.HasAura(Auras.TheCatsMeow)
                || Me.HasAura(Auras.NightmareNightcrawler)
                || Me.HasAura(Auras.BlessingOfTheThistleleaf)
                || Me.HasAura(Auras.DemonicDetrius)
                || Me.HasAura(Auras.EnchantedLure)
                || Me.HasAura(Auras.AxefishLure)
                || Me.HasAura(Auras.RavenousFlyfishing)
            ) { return true; }

            return false;
        }

        #endregion  

        #region Static Info
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override string Author
        {
            get
            {
                return "SpeshulK926";
            }
        }

        public override string Name
        {
            get
            {
                return "Keep Lure Active";
            }
        }

        public override Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }
        public override string ButtonText
        {
            get
            {
                return "Settings...";
            }
        }
        public override bool WantButton
        {
            get
            {
                return true;
            }
        }
        public override void OnButtonPress()
        {
            SettingsForm frm = new SettingsForm();
            frm.ShowDialog();
            S.Save();
        }
        #endregion

        class Auras
        {
            public const int
            #region General
                ArcaneLure = 218861,                // General "Get Lure" Lure
            #endregion
            #region Azsuna
                AromaticMurlocSlime = 201805,       // Leyshimmer Blenny
                PearlescentConch = 201806,          // Nar'thalas Hermit
                RustyQueenfishBroach = 201807,      // Ghostly Queenfish - Fished from pool, so will not use for now, but want to check to make sure it is not already active.
            #endregion
            #region Highmountain
                FrostWorm = 201815,                 // Coldriver Carp
                SalmonLure = 201813,                // Ancient Highmountain Salmon
                BlessingOfTheMurlocs = 202056,      // Mountain Puffer
            #endregion
            #region Stormheim
                MoosehornHook = 201816,             // Bait to get bait Silverscale Minnow
                SilverscaleMinnow = 201817,         // Thundering Stormray
                AncientVrykulRing = 201818,         // Oodelfjisk
                SoggyDrakescale = 201819,           // Graybelly Lobster
            #endregion
            #region Val'Sharah
                TheCatsMeow = 201809,               // Ancient Mossgill
                NightmareNightcrawler = 201810,     // Terrorfin
                BlessingOfTheThistleleaf = 202067,  // Thorned Flounder
            #endregion
            #region Suramar
                DemonicDetrius = 201822,            // Tainted Runescale Koi
                EnchantedLure = 201820,             // Magic-Eater Frog
            #endregion
            #region Ocean
                AxefishLure = 201823,               // Axefish
                RavenousFlyfishing = 202131,        // Ancient Black Barracuda
            #endregion



            Done = 0;
        }
        class Items
        {
            public const int
            #region General
                ArcaneLure = 139175,
                MarkOfAquaos = 141975,
            #endregion
            #region Azsuna
                AromaticMurlocSlime = 133702,       // Skrog Toenail summons Salteye Skrog-Hunter to drop Aromatic Murloc Slime
                PearlescentConch = 133703,          // Bait to get aura Pearlescent Conch (201806)
                RustyQueenfishBroach = 133704,      // Bait to allow you to see Ghostly Queenfish Pools.  Will not use for now.
            #endregion
            #region Highmountain
                FrostWorm = 133712,                 // Bait to get aura Frost Worm (201815)
                SalmonLure = 133710,                // Bait to get aura Salmon Lure (201813)
                SwollenMurlocEgg = 133711,          // Swollen Murloc Egg summons Swamprock Tadpole to give buff Blessing Of the Murlocs (202056) 
            #endregion
            #region Stormheim
                MoosehornHook = 133713,             // Bait to get aura Moosehorn Hook (201816) - Catch Silverscale Minnow
                SilverscaleMinnow = 133714,         // Bait to get aura Silverscale Minnow (201817)  
                AncientVrykulRing = 133715,         // Bait to get aura Ancient Vrykul Ring (201818)
                SoggyDrakescale = 133716,           // Bait to get aura Soggy Drakescale (201819)

            #endregion
            #region Val'Sharah
                RottenFishbone = 133705,            // Bait to attract NPC Lorlathil Druid to cast The Cat's Meow (201809)
                NightmareNightcrawler = 133707,     // Bait to get aura Nightmare Nightcrawler (201810)
                DrownedThistleleaf = 133708,        // Summons Drowned Thistleleaf which grants buff Blessing of the Thistleleaf (202067)
            #endregion
            #region Suramar
                DemonicDetrius = 133720,            // Bait to get aura Demonic Detrius (201822)
                SleepingMurloc = 133719,            // Seerspine Puffer (will drop on ground and have to run over them?  Not sure how this one will work)
                EnchantedLure = 133717,             // Bait to get aura Enchanted Lure (201820)
            #endregion
            #region Ocean
                StunnedAngryShark = 133723,         // Need to use this on land somehow (1 minute timer), kill shark, loot Seabottom Squid (139668)
                MessageInABeerBottle = 133721,      // Use to get Axefish Lure (133722)
                AxefishLure = 133722,               // Bait to get aura Axefish Lure (201823)
                DecayedWhaleBlubber = 133724,       // Used to attract flies to obtain Ravenous Fly (133795)
                RavenousFly = 133795,               // Bait to get aura Ravenous Flyfishing (202131)
            #endregion



            Done = 0;

        }
    }

    public class KLASettings : Settings
    {
        public KLASettings() 
            : base(System.IO.Path.Combine(CharacterSettingsDirectory, "KeepLureActive.xml"))
        {
        
        
        }
        [Setting, DefaultValue(true)]
        public bool UseAromaticMurlocSlime { get; set; }
        [Setting, DefaultValue(true)]
        public bool UsePearlescentConch { get; set; }
        [Setting, DefaultValue(false)]
        public bool UseRustyQueenfishBroach { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseFrostWorm { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseSalmonLure { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseSwollenMurlocEgg { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseMoosehornHook { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseSilverscaleMinnow { get; set; }
        [Setting, DefaultValue(false)]
        public bool UseAncientVrykulRing { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseSoggyDrakescale { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseRottenFishbone { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseNightmareNightcrawler { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseDrownedThistleleaf { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseDemonicDetrius { get; set; }
        [Setting, DefaultValue(false)]
        public bool UseSleepingMurloc { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseEnchantedLure { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseStunnedAngryShark { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseMessageInABeerBottle { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseAxefishLure { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseDecayedWhaleBlubber { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseRavenousFly { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseArcaneLure { get; set; }
        [Setting, DefaultValue(60)]
        public int PauseSecondsAfterUseLure { get; set; }
        [Setting, DefaultValue(true)]
        public bool UseMarkOfAquaos { get; set; }
        
    }
}
