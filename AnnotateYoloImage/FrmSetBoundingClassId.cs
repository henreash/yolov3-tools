using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnnotateYoloImage
{
    public partial class FrmSetBoundingClassId : Form
    {
        public FrmSetBoundingClassId()
        {
            InitializeComponent();
            var list = IoUtils.File2List(YOLOv3Files.ClassesNameFile);
            cmbClassesName.Items.Clear();
            list.ForEach(n => cmbClassesName.Items.Add(n));
        }

        public static int Execute(int classId)
        {
            using (var frm = new FrmSetBoundingClassId())
            {
                frm.cmbClassesName.SelectedIndex = classId;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    return frm.cmbClassesName.SelectedIndex;
                }
                else
                {
                    return -1;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
