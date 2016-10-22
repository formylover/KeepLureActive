using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepLureActive
{
    public partial class SettingsForm2 : Form
    {
        public SettingsForm2()
        {
            InitializeComponent();
        }

        private void SettingsForm2_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = KLASettings.Instance;
            KLASettings.Instance.Load();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            KLASettings.Instance.Save();
            Dispose();
        }
    }
}
