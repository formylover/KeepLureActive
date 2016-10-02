using Buddy.Coroutines;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Frames;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KeepLureActive
{
    public class KeepLureActive : HBPlugin
    {
        #region data members
        private static bool _quit = false;
        private static bool _initialized = false;
        private Coroutine _coroutine;
        private List<Coroutine> _coroutines;
        static string lastInformationMSG;
        public static List<Fish> _fishToThrowBack = new List<Fish>();
        public static List<Lure> _lures = new List<Lure>();
        private static Stopwatch _pauseTimer = new Stopwatch();
        private static Stopwatch _aquaosPauseTimer = new Stopwatch();
        private static Vector3 _lastFishingLocation;
        private static uint _lastRecordedZoneId;
        private static uint _lastRecordedSubZoneId;
        #region zones
        const int AzsunaZoneId = 7334;
        const int HighmountainZoneId = 7503;
        const int StormheimZoneId = 7541;
        const int ValSharahZoneId = 7558;
        const int SuramarZoneId = 7637;
        const int Dalaran = 7502;
        const int MargossRetreat = 8270;
        const int BrokenShore = 7543;
        const int TheGreatSea = 7656;
        #endregion
        public static KLASettings S = new KLASettings();
        public static List<Fish> FishToThrowBack
        {
            get
            {
                if (_fishToThrowBack.Count <= 0)
                {
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133728, Name = "Terrorfin" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133733, Name = "Ancient Highmountain Salmon" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133732, Name = "Coldriver Carp" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133731, Name = "Mountain Puffer" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133730, Name = "Ancient Mossgill" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133729, Name = "Thorned Flounder" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133725, Name = "Leyshimmer Blenny" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133726, Name = "Nar'thalas Hermit" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133727, Name = "Ghostly Queenfish" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133737, Name = "Magic-Eater Frog" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133739, Name = "Tainted Runescale Koi" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133738, Name = "Seerspine Puffer" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133736, Name = "Thundering Stormray" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133734, Name = "Oodelfjisk" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133735, Name = "Graybelly Lobster" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133740, Name = "Axefish" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Skill, ID = 133741, Name = "Ancient Black Barracuda" });

                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139655, Name = "Terrorfin" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139660, Name = "Ancient Highmountain Salmon" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139659, Name = "Coldriver Carp" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139658, Name = "Mountain Puffer" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139657, Name = "Ancient Mossgill" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139656, Name = "Thorned Flounder" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139652, Name = "Leyshimmer Blenny" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139653, Name = "Nar'thalas Hermit" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139654, Name = "Ghostly Queenfish" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139664, Name = "Magic-Eater Frog" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139666, Name = "Tainted Runescale Koi" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139665, Name = "Seerspine Puffer" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139663, Name = "Thundering Stormray" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139661, Name = "Oodelfjisk" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139662, Name = "Graybelly Lobster" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139667, Name = "Axefish" });
                    _fishToThrowBack.Add(new Fish() { FishType = FishTypes.Ancient, ID = 139669, Name = "Ancient Black Barracuda" });
                }
                return _fishToThrowBack;


            }
        }
        public static List<Lure> Lures
        {
            get
            {
                if (_lures.Count <= 0)
                {
                    ClearAndReAddLures();
                }
                return _lures;
            }
        }
        #endregion

        #region Plugin Initialization
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public override string Author { get { return "SpeshulK926"; } }
        public override string Name { get { return "Keep Lure Active"; } }
        public override Version Version { get { return new Version(1, 4); } }
        public override string ButtonText { get { return "Settings..."; } }
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            SettingsForm frm = new SettingsForm();
            frm.ShowDialog();
            S.Save();
            ClearAndReAddLures();
        }
        public override void OnEnable()
        {
            if (!_initialized)
            {
                BotEvents.OnBotStopped += OnBotStopped;
                BotEvents.OnBotStarted += OnBotStarted;
                BotEvents.OnBotPaused += OnBotPaused;
                BotEvents.OnBotResumed += OnBotResumed;
                //Lua.Events.AttachEvent("LOOT_CLOSED", LOOT_CLOSED);
                _initialized = true;
            }
        }

        private void OnBotResumed(object sender, EventArgs e)
        {
            _quit = false;
        }

        private void OnBotPaused(object sender, EventArgs e)
        {
            StopCoroutine();
            _quit = true;
        }

        public override void OnDisable()
        {
            if (_initialized)
            {
                BotEvents.OnBotStopped -= OnBotStopped;
                //Lua.Events.DetachEvent("LOOT_CLOSED", LOOT_CLOSED);
                _initialized = false;
            }
        }

        private void LOOT_CLOSED(object sender, LuaEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnBotStarted(EventArgs args)
        {
            _quit = false;
        }

        private void OnBotStopped(EventArgs args)
        {
            StopCoroutine();
        }

        #endregion



        public override void Pulse()
        {
            if (_initialized)
            {
                if (Me.IsChanneling && Me.ChanneledSpell?.Name == "Fishing" && (_lastRecordedZoneId != Me.ZoneId || _lastRecordedSubZoneId != Me.SubZoneId))
                {
                    _lastRecordedZoneId = Me.ZoneId;
                    _lastRecordedSubZoneId = Me.SubZoneId;
                    infoLog("New Zone ID or Sub Zone ID fishing in: ZoWneId:" + _lastRecordedZoneId + " (" + Me.ZoneText + "), SubZoneId:" + _lastRecordedSubZoneId + "(" + Me.SubZoneText + ")");
                }
                //if (Me.IsChanneling && Me.ChanneledSpell?.Name == "Fishing" && _lastFishingLocation != Me.Location)
                //{
                //    _lastFishingLocation = Me.Location;
                //    infoLog("Saved new fish location - X:" + _lastFishingLocation.X + " Y:" + _lastFishingLocation.Y + " Z:" + _lastFishingLocation.Z);
                //}
                if (S.UseMarkOfAquaos)
                {
                    if (AquaosBossIsCurrentlySummoned && !_aquaosPauseTimer.IsRunning)
                    {
                        WoWUnit aquaos = AquaosBoss;
                        if (aquaos != null)
                        {
                            _aquaosPauseTimer.Restart();
                            infoLog(aquaos.SafeName + " has been summoned by someone! Waiting 2 minutes to cast mark if you have/get one.");
                            if (Me.CurrentTarget != aquaos) { aquaos.Target(); }
                        }

                    }
                    if (_aquaosPauseTimer.IsRunning && _aquaosPauseTimer.ElapsedMilliseconds >= (120 * 1000))
                    {
                        _aquaosPauseTimer.Reset();
                        infoLog("Can cast Mark of Aquaos again.");
                    }
                    //if (!AquaosBossIsCurrentlySummoned && Me.Location != _lastFishingLocation)
                    //{
                    //    CommonCoroutines.MoveTo(_lastFishingLocation).Wait();
                    //}
                }
                if (_coroutine == null || _coroutine.IsFinished)
                {
                    if (_coroutine != null)
                        Logging.WriteDiagnostic(Colors.Red, "Keep Lure Active Ended.  Restarting...");

                    _coroutine = new Coroutine(() => MainCoroutine());
                }

                try
                {
                    _coroutine.Resume();
                }

                catch (Exception ex)
                {
                    Logging.Write(Colors.Red, ex.Message);
                }

            }
        }


        #region Helper Methods
        #region Coroutines
        private async Task<bool> MainCoroutine()
        {
            while (!_quit)
            {
                try
                {
                    //await Coroutine.ExternalTask(FacePool());
                    // throw the fish back
                    //await FacePool();
                    if (S.ThrowFishBackInWater)
                    {
                        await Coroutine.ExternalTask(ThrowFishBack());
                    }
                    await Coroutine.ExternalTask(UseArcaneLure());
                    await Coroutine.ExternalTask(UseMarkOfAquaos());
                    await Coroutine.ExternalTask(UseZoneSpecificLure());

                }


                catch (Exception ex)
                {
                    Logging.WriteDiagnostic(ex.ToString());

                }
                return false;
            }
            return true;
        }
        private async Task ThrowFishBack()
        {
            await Task.Delay(SpellManager.GlobalCooldownLeft);
            if (!DoingSomethingElse)
            {
                // throw fish back for Artifact updates
                foreach (var item in Me.BagItems)
                {
                    if (FishToThrowBack.Where(f => f.ID == item.ItemInfo.Id).Any())
                    {
                        if (UseItem(item, S.ThrowFishBackInWater))
                        {
                            return;
                        }

                    }
                }
            }
            return;
        }
        private async Task UseArcaneLure()
        {
            await Task.Delay(SpellManager.GlobalCooldownLeft);

            if (!DoingSomethingElse)
            {
                var lure = Me.BagItems.FirstOrDefault(b => b.ItemInfo.Id == Lures.FirstOrDefault(l => l.Name == "Arcane Lure")?.ID);
                if (UseItem(lure, IsInCorrectZone(Zones.AllButDalaran) && !Me.HasAura(Auras.ArcaneLure) && S.UseArcaneLure))
                {
                    return; 
                }
            }
            return;
        }
        private async Task UseMarkOfAquaos()
        {
            await Task.Delay(SpellManager.GlobalCooldownLeft);

            if (!DoingSomethingElse)
            {
                var lure = Me.BagItems.FirstOrDefault(b => b.ItemInfo.Id == Lures.FirstOrDefault(l => l.Name == "Mark of Aquaos")?.ID);
                if (lure != null && AquaosBossIsCurrentlySummoned && _aquaosPauseTimer.ElapsedMilliseconds < (120 * 1000))
                {
                    infoLog("Boss already summoned or recently summoned. Trying again when he is not spawned.");
                    return;
                }
                if (UseItem(lure, IsInCorrectZone(Zones.DalaranMargossRetreat) && S.UseMarkOfAquaos))
                {
                    _aquaosPauseTimer.Reset();
                    return;
                }
            }
            return;

        }
        private async Task UseZoneSpecificLure()
        {
            await Task.Delay(SpellManager.GlobalCooldownLeft);
            if (!HasFishingAura() && !ShouldPauseAfterLureUse() && IsInCorrectZone())
            {
                var bagItems = Me.BagItems;
                foreach (var lure in Lures.Where(l => l.ShouldUse))
                {
                    if (lure.Name == "Arcane Lure") { continue; }
                    var itemToUse = bagItems.FirstOrDefault(b => b.ItemInfo.Id == lure.ID && IsInCorrectZone(lure.LocationToUse));
                    if (itemToUse != null)
                    {
                        if (UseItem(itemToUse, true))
                        {
                            _pauseTimer.Restart();
                            infoLog("Will pause lures for " + S.PauseSecondsAfterUseLure + " seconds.");
                        }
                    }
                }
            }
        }
        //private async Task FacePool()
        //{
        //    var pools = ObjectManager.ObjectList.Where(o => o.IsValid && o.Name.Contains("School"));
        //    if (pools != null)
        //    {
        //        var pool = pools.OrderBy(p => p.Distance).FirstOrDefault();
        //        //infoLog("Found pool.  Trying to face it.");
        //        if (pool != null && pool.Distance <= 10)
        //        {
        //            if (!Me.IsSafelyFacing(pool.Location))
        //            {
        //                WoWMovement.Face(pool.Guid);
        //            }
        //        }
        //    }
        //    await Coroutine.Yield();
        //}
        bool UseItem(WoWItem item, bool reqs, string log = null)
        {
            if (item == null || !reqs || !CanUseItem(item)) { return false; }

            infoLog(string.Format($"/use {item.Name}" + (String.IsNullOrEmpty(log) ? "" : " - " + log)));
            if (item.Use())
            {
                return true;
            }
            return false;
        }
        #endregion

        private void StopCoroutine()
        {
            _quit = true;
            if (_coroutine == null)
                return;

            _coroutine.Dispose();
            _coroutine = null;

        }
        private bool DoingSomethingElse
        {
            get
            {
                if (!Me.Mounted && !Me.IsCasting && !Me.Combat && !Me.IsChanneling)
                {
                    return true;
                }
                return false;
            }

        }

        private static IEnumerable<WoWUnit> surroundingEnemies() { return ObjectManager.GetObjectsOfTypeFast<WoWUnit>(); }

        private static bool AquaosBossIsCurrentlySummoned
        {
            get
            {
                return surroundingEnemies().Where(u => u.Entry == 113378).Any();
            }
        }
        private static WoWUnit AquaosBoss
        {
            get
            {
                return surroundingEnemies().Where(u => u.Entry == 113378).FirstOrDefault();
            }
        }

        bool IsInOpenOcean()
        {
            if (Me.ZoneId == BrokenShore || Me.ZoneId == TheGreatSea) { return true; }
            return false;
        }
        bool IsInCorrectZone()
        {
            if (Me.ZoneId == AzsunaZoneId || Me.ZoneId == HighmountainZoneId || Me.ZoneId == StormheimZoneId || Me.ZoneId == SuramarZoneId || Me.ZoneId == ValSharahZoneId || IsInOpenOcean())
            {
                return true;
            }
            return false;
        }
        // check an item against a specific zone.
        bool IsInCorrectZone(Zones zone)
        {
            if (Me.ZoneId == AzsunaZoneId)
            {
                if (zone.HasFlag(Zones.Azsuna)) { return true; }
            }
            if (Me.ZoneId == HighmountainZoneId)
            {
                if (zone.HasFlag(Zones.Highmountain)) { return true; }
            }
            if (Me.ZoneId == StormheimZoneId)
            {
                if (zone.HasFlag(Zones.Stormheim)) { return true; }
            }
            if (Me.ZoneId == SuramarZoneId)
            {
                if (zone.HasFlag(Zones.Suramar)) { return true; }
            }
            if (Me.ZoneId == ValSharahZoneId)
            {
                if (zone.HasFlag(Zones.ValSharah)) { return true; }
            }
            if (IsInOpenOcean())
            {
                if (zone.HasFlag(Zones.Ocean)) { return true; }
            }
            if (Me.ZoneId == Dalaran && Me.SubZoneId == MargossRetreat)
            {
                if (zone.HasFlag(Zones.DalaranMargossRetreat)) { return true; }
            }
            if (Me.ZoneId != Dalaran)
            {
                if (zone.HasFlag(Zones.AllButDalaran)) { return true; }
            }
            if (Me.ZoneId == AzsunaZoneId || Me.ZoneId == HighmountainZoneId || Me.ZoneId == StormheimZoneId
                || Me.ZoneId == SuramarZoneId || Me.ZoneId == ValSharahZoneId || IsInOpenOcean() || (Me.ZoneId == Dalaran && Me.SubZoneId == MargossRetreat))
            {
                if (zone.HasFlag(Zones.All)) { return true; }
            }
            return false;
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

        bool ShouldPauseAfterLureUse()
        {
            if (_pauseTimer.IsRunning && _pauseTimer.ElapsedMilliseconds < (S.PauseSecondsAfterUseLure * 1000))
            {
                return true;
            }
            if (_pauseTimer.IsRunning)
            {
                infoLog("Lures ready to cast again.");
                _pauseTimer.Reset();
            }
            return false;
        }
        bool HasFishingAura()
        {
            if (Me.HasAura(Auras.AromaticMurlocSlime)
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

        private static void ClearAndReAddLures()
        {
            _lures.Clear();

            _lures.Add(new Lure() { ID = 139175, Name = "Arcane Lure", LocationToUse = Zones.AllButDalaran, ShouldUse = S.UseArcaneLure });

            _lures.Add(new Lure() { ID = 141975, Name = "Mark of Aquaos", LocationToUse = Zones.DalaranMargossRetreat, ShouldUse = S.UseMarkOfAquaos });

            _lures.Add(new Lure() { ID = 133702, Name = "Aromatic Murloc Slime", LocationToUse = Zones.Azsuna, ShouldUse = S.UseAromaticMurlocSlime });
            _lures.Add(new Lure() { ID = 133703, Name = "Pearlescent Conch", LocationToUse = Zones.Azsuna, ShouldUse = S.UsePearlescentConch });
            _lures.Add(new Lure() { ID = 133704, Name = "Rusty Queenfish Broach", LocationToUse = Zones.Azsuna, ShouldUse = S.UseRustyQueenfishBroach });

            _lures.Add(new Lure() { ID = 133712, Name = "Frost Worm", LocationToUse = Zones.Highmountain, ShouldUse = S.UseFrostWorm });
            _lures.Add(new Lure() { ID = 133710, Name = "Salmon Lure", LocationToUse = Zones.Highmountain, ShouldUse = S.UseSalmonLure });
            _lures.Add(new Lure() { ID = 133711, Name = "Swollen Murloc Egg", LocationToUse = Zones.Highmountain, ShouldUse = S.UseSwollenMurlocEgg });

            _lures.Add(new Lure() { ID = 133713, Name = "Moosehorn Hook", LocationToUse = Zones.Stormheim, ShouldUse = S.UseMoosehornHook });
            _lures.Add(new Lure() { ID = 133714, Name = "Silverscale Minnow", LocationToUse = Zones.Stormheim, ShouldUse = S.UseSilverscaleMinnow });
            _lures.Add(new Lure() { ID = 133715, Name = "Ancient Vrykul Ring", LocationToUse = Zones.Stormheim, ShouldUse = S.UseAncientVrykulRing });
            _lures.Add(new Lure() { ID = 133716, Name = "Soggy Drakescale", LocationToUse = Zones.Stormheim, ShouldUse = S.UseSoggyDrakescale });

            _lures.Add(new Lure() { ID = 133705, Name = "Rotten Fishbone", LocationToUse = Zones.ValSharah, ShouldUse = S.UseRottenFishbone });
            _lures.Add(new Lure() { ID = 133707, Name = "Nightmare Nightcrawler", LocationToUse = Zones.ValSharah, ShouldUse = S.UseNightmareNightcrawler });
            _lures.Add(new Lure() { ID = 133708, Name = "Drowned Thistleleaf", LocationToUse = Zones.ValSharah, ShouldUse = S.UseDrownedThistleleaf });

            _lures.Add(new Lure() { ID = 133720, Name = "Demonic Detritus", LocationToUse = Zones.Suramar, ShouldUse = S.UseDemonicDetrius });
            _lures.Add(new Lure() { ID = 133719, Name = "Sleeping Murloc", LocationToUse = Zones.Suramar, ShouldUse = S.UseSleepingMurloc });
            _lures.Add(new Lure() { ID = 133717, Name = "Enchanted Lure", LocationToUse = Zones.Suramar, ShouldUse = S.UseEnchantedLure });

            _lures.Add(new Lure() { ID = 133723, Name = "Stunned Angry Shark", ShouldUse = S.UseStunnedAngryShark });
            _lures.Add(new Lure() { ID = 133721, Name = "Message In A Beer Bottle", ShouldUse = S.UseMessageInABeerBottle });
            _lures.Add(new Lure() { ID = 133722, Name = "Axefish Lure", ShouldUse = S.UseAxefishLure });
            _lures.Add(new Lure() { ID = 133724, Name = "Decayed Whale Blubber", ShouldUse = S.UseDecayedWhaleBlubber });
            _lures.Add(new Lure() { ID = 133795, Name = "Ravenous Fly", ShouldUse = S.UseRavenousFly });
        }

        public void infoLog(string Message, params object[] args)
        {
            if (Message == lastInformationMSG) { return; }

            Logging.Write(System.Windows.Media.Colors.LightBlue, "[KeepLureActive]: {0}", Message, args);
            lastInformationMSG = Message;
        }
        #endregion



        public class Auras
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



    }
    [Flags]
    public enum Zones
    {
        None = 0,
        Azsuna = 1,
        Highmountain = 2,
        Stormheim = 4,
        ValSharah = 8,
        Suramar = 16,
        Ocean = 32,
        DalaranMargossRetreat = 64,
        AllButDalaran = Azsuna | Highmountain | Stormheim | ValSharah | Suramar | Ocean,
        All = AllButDalaran | DalaranMargossRetreat

    }
    public class Fish
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public FishTypes FishType { get; set; }
    }
    public class Lure
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Zones LocationToUse { get; set; }
        public bool ShouldUse { get; set; }
    }
    public enum FishTypes { None, Skill, Ancient };
    public class KLASettings : Settings
    {
        public KLASettings()
            : base(System.IO.Path.Combine(CharacterSettingsDirectory, "KeepLureActive.xml"))
        {


        }
        [Setting, DefaultValue(true)]
        public bool ThrowFishBackInWater { get; set; }
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
