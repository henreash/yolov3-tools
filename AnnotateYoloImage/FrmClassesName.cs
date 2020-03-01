using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnnotateYoloImage
{
    public partial class FrmClassesName : Form
    {
        public FrmClassesName()
        {
            InitializeComponent();
        }

        private void FrmClassesName_Load(object sender, EventArgs e)
        {
            lbNames.Items.Clear();
            LoadNames();
        }

        private void LoadNames()
        {
            var fileName = YOLOv3Files.ClassesNameFile;
            if (!File.Exists(fileName))
                return;
            var list = IoUtils.File2List(fileName);
            foreach (var v in list)
            {
                lbNames.Items.Add(v);
            }
        }

        private bool IsNameValid(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入名称");
                return false;
            }
            if (lbNames.Items.Cast<string>().ToList().Exists(v => v.Equals(name)))
            {
                MessageBox.Show("名称不能重复");
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = tbName.Text.Trim();
            if (!IsNameValid(name))
            {
                return;
            }
            lbNames.Items.Add(name);
        }

        private void lbNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNames.SelectedIndex == -1)
                return;
            tbName.Text = lbNames.SelectedItem.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lbNames.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个名称");
                return;
            }
            var name = tbName.Text.Trim();
            if (!IsNameValid(name))
            {
                return;
            }
            lbNames.Items[lbNames.SelectedIndex] = name;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbNames.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个名称");
                return;
            }
            if (MessageBox.Show("确定删除吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            lbNames.Items.RemoveAt(lbNames.SelectedIndex);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var list = lbNames.Items.Cast<string>().Select(v => v.ToString()).ToList();
            IoUtils.List2File(YOLOv3Files.ClassesNameFile, list);
            DialogResult = DialogResult.OK;
        }

        public static void Execute()
        {
            using(var frm = new FrmClassesName())
            {
                frm.ShowDialog();
            }
        }
    }
}
