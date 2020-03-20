using SharpMap.Rendering.Decoration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoSharpMap
{
    public partial class MainForm : Form
    {
        private static readonly Dictionary<string, Type> MapDecorationTypes = new Dictionary<string, Type>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            ShowMap(MapType.RunLine);
        }

        private void btnDo2_Click(object sender, EventArgs e)
        {
            ShowMap(MapType.Static);
        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "png";
            sfd.Filter = "PNG图片|*.png|JPG图片|*.jpg";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                Image image = this.mapBox1.Map.GetMap();
                image.Save(fileName);
            }
        }

        private void btnMapInfo_Click(object sender, EventArgs e)
        {
            ShowMap(MapType.MapInfo);
        }

        private void btnShapFile_Click(object sender, EventArgs e)
        {
            ShowMap(MapType.ShapeFile);
        }

        private void ShowMap(MapType tt) {
            Cursor mic = mapBox1.Cursor;
            mapBox1.Cursor = Cursors.WaitCursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                mapBox1.Map = SharpMapHelper.InitializeMap(tt,0);
                mapBox1.Map.Size = Size;
                mapBox1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }
            Cursor = Cursors.Default;
            mapBox1.Cursor = mic;
        }
    }
}
