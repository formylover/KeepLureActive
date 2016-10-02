using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K = KeepLureActive.KeepLureActive;
namespace KeepLureActive
{
    public partial class SettingsForm : Form
    {
        
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void UseArcaneLure_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseArcaneLure = UseArcaneLure.Checked;
        }

        private void UseFrostWorm_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseFrostWorm = UseFrostWorm.Checked;
        }

        private void UseSalmonLure_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseSalmonLure = UseSalmonLure.Checked;
        }

        private void UseSwollenMurlocEgg_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseSwollenMurlocEgg = UseSwollenMurlocEgg.Checked;
        }

        private void UseAromaticMurlocSlime_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseAromaticMurlocSlime = UseAromaticMurlocSlime.Checked;
        }

        private void UsePearlescentConch_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UsePearlescentConch = UsePearlescentConch.Checked;
        }

        private void UseRustyQueenfishBroach_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseRustyQueenfishBroach = UseRustyQueenfishBroach.Checked;
        }

        private void UseMoosehornHook_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseMoosehornHook = UseMoosehornHook.Checked;
        }

        private void UseSilverscaleMinnow_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseSilverscaleMinnow = UseSilverscaleMinnow.Checked;
        }

        private void UseAncientVrykulRing_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseAncientVrykulRing = UseAncientVrykulRing.Checked;
        }

        private void UseSoggyDrakescale_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseSoggyDrakescale = UseSoggyDrakescale.Checked;
        }

        private void UseRottenFishbone_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseRottenFishbone = UseRottenFishbone.Checked;
        }

        private void UseNightmareNightcrawler_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseNightmareNightcrawler = UseNightmareNightcrawler.Checked;
        }

        private void UseDrownedThistleleaf_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseDrownedThistleleaf = UseDrownedThistleleaf.Checked;
        }

        private void UseDemonicDetrius_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseDemonicDetrius = UseDemonicDetrius.Checked;
        }

        private void UseSleepingMurloc_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseSleepingMurloc = UseSleepingMurloc.Checked;
        }

        private void UseEnchantedLure_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseEnchantedLure = UseEnchantedLure.Checked;
        }

        private void UseStunnedAngryShark_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseStunnedAngryShark = UseStunnedAngryShark.Checked;
        }

        private void UseMessageInABeerBottle_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseMessageInABeerBottle = UseMessageInABeerBottle.Checked;
        }

        private void UseAxefishLure_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseAxefishLure = UseAxefishLure.Checked;
        }

        private void UseDecayedWhaleBlubber_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseDecayedWhaleBlubber = UseDecayedWhaleBlubber.Checked;
        }

        private void UseRavenousFly_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseRavenousFly = UseRavenousFly.Checked;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            PauseSecondsAfterLureUse.Value = K.S.PauseSecondsAfterUseLure;
            UseAncientVrykulRing.Checked = K.S.UseAncientVrykulRing;
            UseArcaneLure.Checked = K.S.UseArcaneLure;
            UseAromaticMurlocSlime.Checked = K.S.UseAromaticMurlocSlime;
            UseAxefishLure.Checked = K.S.UseAxefishLure;
            UseDecayedWhaleBlubber.Checked = K.S.UseDecayedWhaleBlubber;
            UseDemonicDetrius.Checked = K.S.UseDemonicDetrius;
            UseDrownedThistleleaf.Checked = K.S.UseDrownedThistleleaf;
            UseEnchantedLure.Checked = K.S.UseEnchantedLure;
            UseFrostWorm.Checked = K.S.UseFrostWorm;
            UseMessageInABeerBottle.Checked = K.S.UseMessageInABeerBottle;
            UseMoosehornHook.Checked = K.S.UseMoosehornHook;
            UseNightmareNightcrawler.Checked = K.S.UseNightmareNightcrawler;
            UsePearlescentConch.Checked = K.S.UsePearlescentConch;
            UseRavenousFly.Checked = K.S.UseRavenousFly;
            UseRottenFishbone.Checked = K.S.UseRottenFishbone;
            UseRustyQueenfishBroach.Checked = K.S.UseRustyQueenfishBroach;
            UseSalmonLure.Checked = K.S.UseSalmonLure;
            UseSilverscaleMinnow.Checked = K.S.UseSilverscaleMinnow;
            UseSleepingMurloc.Checked = K.S.UseSleepingMurloc;
            UseSoggyDrakescale.Checked = K.S.UseSoggyDrakescale;
            UseStunnedAngryShark.Checked = K.S.UseStunnedAngryShark;
            UseSwollenMurlocEgg.Checked = K.S.UseSwollenMurlocEgg;
            UseMarkOfAquaos.Checked = K.S.UseMarkOfAquaos;
            ThrowFishBackInTheWater.Checked = K.S.ThrowFishBackInWater;
        }

        private void UseMarkOfAquaos_CheckedChanged(object sender, EventArgs e)
        {
            K.S.UseMarkOfAquaos = UseMarkOfAquaos.Checked;
        }

        private void ThrowFishBackInTheWater_CheckedChanged(object sender, EventArgs e)
        {
            K.S.ThrowFishBackInWater = ThrowFishBackInTheWater.Checked;
        }
    }
}
