using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnnotateYoloImage
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            YOLOv3Files.YOLOv3Detector = tbYOLOv3Detector.Text;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lbImageList.Items.Clear();
            pbImage.Location = new Point(0, 0);
            LoadDetectorData();
        }

        private void LoadImageList()
        {
            if (string.IsNullOrEmpty(YOLOv3Files.ImagesPath))
                return;
            var arr = Directory.GetFiles(YOLOv3Files.ImagesPath, "*.jpg");
            lbImageList.Items.Clear();
            foreach (var f in arr)
            {
                lbImageList.Items.Add(Path.GetFileName(f));
            }
        }

        private Image _image;
        private string _imgFile;
        private string _labelFile;
        private Image<Bgr, byte> _emguImage;
        private void lbImageList_DoubleClick(object sender, EventArgs e)
        {
            if (lbImageList.SelectedIndex == -1)
                return;
            _imgFile = lbImageList.SelectedItem.ToString();
            _labelFile = Path.GetFileNameWithoutExtension(_imgFile) + ".txt";
            var imgFile = Path.Combine(YOLOv3Files.ImagesPath, _imgFile);
            _image = Image.FromFile(imgFile);
            _emguImage = new Image<Bgr, byte>(imgFile);
            pbImage.Image = _image;
            var labelFile = Path.Combine(YOLOv3Files.ImagesPath, _labelFile);
            LoadBoundingBox(labelFile);
        }

        private List<DragableRectangle> _imgAnnotateBoundingBoxList = new List<DragableRectangle>();

        private void LoadBoundingBox(string file)
        {
            _imgAnnotateBoundingBoxList.Clear();
            pbImage.Controls.Clear();
            var width = _image.Width;
            var height = _image.Height;
            if (!File.Exists(file))
                return;
            var list = IoUtils.File2List(file);
            foreach (var boxInfo in list)
            {
                var arr = boxInfo.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var classId = arr[0].ToInt();
                var centerX = (arr[1].ToDouble() * width).ToInt();
                var centerY = (arr[2].ToDouble() * height).ToInt();
                var boxWidth = (arr[3].ToDouble() * width).ToInt();
                var boxHeight = (arr[4].ToDouble() * height).ToInt();
                var left = centerX - boxWidth / 2;
                var top = centerY - boxHeight / 2;
                var dr = new DragableRectangle() { Parent = pbImage, Left = left, Top = top, Width = boxWidth, Height = boxHeight, ClassId = classId };
                dr.OnRectDoubleClick += Dr_OnRectDoubleClick;
                _imgAnnotateBoundingBoxList.Add(dr);
            }
        }

        private void Dr_OnRectDoubleClick(DragableRectangle sender)
        {
            var newId = FrmSetBoundingClassId.Execute(sender.ClassId);
            if (newId != -1)
            {
                sender.ClassId = newId;
            }
        }

        private void btnSaveAnnotate_Click(object sender, EventArgs e)
        {
            var labelFile = Path.Combine(YOLOv3Files.ImagesPath, _labelFile);
            var width = _image.Width;
            var height = _image.Height;
            var list = new List<string>();
            for (int i = 0; i < _imgAnnotateBoundingBoxList.Count; i++)
            {
                var box = _imgAnnotateBoundingBoxList[i];
                var centerX = (box.Left + box.Width / 2) * 1.0 / width;
                var centerY = (box.Top + box.Height / 2) * 1.0 / height;
                var boxW = box.Width * 1.0 / width;
                var boxH = box.Height * 1.0 / height;
                var label = $"{box.ClassId} {centerX} {centerY} {boxW} {boxH}";
                list.Add(label);
            }
            IoUtils.List2File(labelFile, list);
        }

        private void btnSetClassNames_Click(object sender, EventArgs e)
        {
            FrmClassesName.Execute();
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var box = _imgAnnotateBoundingBoxList.FirstOrDefault(b => b.IsSelected);
                if (box == null)
                    return;
                if (MessageBox.Show("删除标注框吗", "询问", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                _imgAnnotateBoundingBoxList.Remove(box);
                pbImage.Controls.Remove(box);
            }else if(e.KeyCode == Keys.Left)
            {
                var box = _imgAnnotateBoundingBoxList.FirstOrDefault(b => b.IsSelected);
                if (box == null)
                    return;
                box.Left -= 1;
            }
            else if (e.KeyCode == Keys.Right)
            {
                var box = _imgAnnotateBoundingBoxList.FirstOrDefault(b => b.IsSelected);
                if (box == null)
                    return;
                box.Left += 1;
            }
            else if (e.KeyCode == Keys.Up)
            {
                var box = _imgAnnotateBoundingBoxList.FirstOrDefault(b => b.IsSelected);
                if (box == null)
                    return;
                box.Top -= 1;
            }
            else if (e.KeyCode == Keys.Down)
            {
                var box = _imgAnnotateBoundingBoxList.FirstOrDefault(b => b.IsSelected);
                if (box == null)
                    return;
                box.Top += 1;
            }
        }

        private void btnAddRegion_Click(object sender, EventArgs e)
        {
            _addRegion = true;
        }

        private bool _addRegion = false;
        private Point _newRegionStartPoint = new Point(-1, -1);

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_imgAnnotateBoundingBoxList.Exists(c => c.Bounds.Contains(e.X, e.Y)))
                _imgAnnotateBoundingBoxList.ForEach(c =>
                {
                    c.IsSelected = false;
                    c.Invalidate();
                });
            if (_addRegion)
            {
                _newRegionStartPoint = new Point(e.X, e.Y);
                var boxWidth = 30;
                var boxHeight = 30;
                var dr = new DragableRectangle() { Parent = pbImage, Left = _newRegionStartPoint.X - 25, Top = _newRegionStartPoint.Y - 25, Width = boxWidth, Height = boxHeight, ClassId = 0 };
                dr.OnRectDoubleClick += Dr_OnRectDoubleClick;
                _imgAnnotateBoundingBoxList.Add(dr);
                _addRegion = false;
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            //if (_addRegion)
            //{
            //    var boxWidth = e.X - _newRegionStartPoint.X;
            //    var boxHeight = e.Y - _newRegionStartPoint.Y;
            //    if(boxWidth <= 0 || boxHeight <= 0)
            //    {
            //        _addRegion = false;
            //        return;
            //    }
            //    var dr = new DragableRectangle() { Parent = pbImage, Left = _newRegionStartPoint.X, Top = _newRegionStartPoint.Y, Width = boxWidth, Height = boxHeight, ClassId = 0 };
            //    dr.OnRectDoubleClick += Dr_OnRectDoubleClick;
            //    _imgAnnotateBoundingBoxList.Add(dr);
            //    _addRegion = false;
            //}
        }

        private void btnCfgMgr_Click(object sender, EventArgs e)
        {
            FrmGenCfgFile.Execute();
        }

        private void btnSpliteSample_Click(object sender, EventArgs e)
        {
            var rate = tbTestRate.Text.ToInt() / 100.0;
            var imgList = Directory.GetFiles(YOLOv3Files.ImagesPath, "*.jpg").Select(f => Path.GetFileName(f)).ToList();
            var testCnt = (imgList.Count * rate).ToInt();
            Random rmd = new Random();
            var testImgList = new List<string>();
            for (var i = 0; i < testCnt; i++)
            {
                var randomIndex = rmd.Next(imgList.Count);
                testImgList.Add(imgList[randomIndex]);
                imgList.RemoveAt(randomIndex);
            }
            testImgList = testImgList.Select(f => $".\\{tbYOLOv3Detector.Text}\\images\\{f}").ToList();
            imgList = imgList.Select(f => $".\\{tbYOLOv3Detector.Text}\\images\\{f}").ToList();
            IoUtils.List2File(YOLOv3Files.TrainSampleFile, imgList);
            IoUtils.List2File(YOLOv3Files.TestSampleFile, testImgList);
        }

        private void btnGenDarknetDataFile_Click(object sender, EventArgs e)
        {
            var nameList = IoUtils.File2List(YOLOv3Files.ClassesNameFile);
            var dataList = new List<string>();
            dataList.Add($"classes = {nameList.Count}");
            dataList.Add($"train = {tbYOLOv3Detector.Text}/{Path.GetFileName(YOLOv3Files.TrainSampleFile)}");
            dataList.Add($"valid = {tbYOLOv3Detector.Text}/{Path.GetFileName(YOLOv3Files.TestSampleFile)}");
            dataList.Add($"names = {tbYOLOv3Detector.Text}/{Path.GetFileName(YOLOv3Files.ClassesNameFile)}");
            dataList.Add($"backup =  {tbYOLOv3Detector.Text}/");
            var backupPath = Path.Combine(Application.StartupPath, tbYOLOv3Detector.Text);
            if (!Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);
            IoUtils.List2File(YOLOv3Files.DarknetDataFile, dataList);
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            //darknet.exe detector train ./darknet-data-auto-gen.data ./yolov3-obj-auto-gen.cfg ./darknet53.conv.74 ./train.log
            //pause
            var arguments = $"detector train ./{tbYOLOv3Detector.Text}/{tbYOLOv3Detector.Text}-data-auto-gen.data ./{tbYOLOv3Detector.Text}/yolov3-{tbYOLOv3Detector.Text}-auto-gen.cfg ./darknet53.conv.74 ./train.log";
            ShowLog($"darknet.exe {arguments}");
            Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "darknet.exe";
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = false;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = false;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();//启动程序
            p.OutputDataReceived += P_OutputDataReceived;

            //向cmd窗口发送输入信息

            //p.StandardInput.WriteLine(cmd);            
            //p.StandardInput.WriteLine("exit");
            //p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            //string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //char[] buffer = new char[1024];
            //while (true)
            //{
            //    var len = reader.Read(buffer, 0, 1024);
            //    if (len == 0)
            //        Thread.Sleep(50);
            //    var line = new string(buffer.Take(len).ToArray());
            //    ShowLog(line);
            //    Debug.WriteLine("read output...");
            //}
            //p.WaitForExit();//等待程序执行完退出进程
            ////ShowLog(output);
            //p.Close();
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ShowLog(e.Data);
        }

        private void ShowLog(string log)
        {
            tbLog.AppendText(log + Environment.NewLine);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //var path = Path.Combine(Application.StartupPath, tbYOLOv3Detector.Text, "test-images");
            //if(!Directory.Exists(path))
            //{
            //    ShowLog($"{path} 不存在，请将测试图像（.jpg）存入此目录");
            //    return;
            //}
            //var arr = Directory.GetFiles(path, "*.jpg");
            //if (arr.Length == 0)
            //{
            //    ShowLog($"{path} 无图像，请将测试图像（.jpg）存入此目录");
            //    return;
            //}
            var imgFile = string.Empty;
            using(var dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = Application.StartupPath;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                imgFile = dlg.FileName;
            }
            imgFile = $"{tbYOLOv3Detector.Text}/test-images/{Path.GetFileName(imgFile)}";
            //darknet.exe detector test data/coco.data yolov3.cfg yolov3.weights dog.jpg
            var arguments = $"detector test {tbYOLOv3Detector.Text}/{tbYOLOv3Detector.Text}-data-auto-gen.data {tbYOLOv3Detector.Text}/yolov3-{tbYOLOv3Detector.Text}-auto-gen.cfg {tbYOLOv3Detector.Text}/yolov3-{tbYOLOv3Detector.Text}-auto-gen_last.weights {imgFile}";
            ShowLog($"darknet.exe {arguments}");
            Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "darknet.exe";
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = false;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = false;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();//启动程序
        }

        private void btnSelYOLOv3Detector_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = YOLOv3Files.ImagesPath;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                var arr = dlg.SelectedPath.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                tbYOLOv3Detector.Text = arr.Last();
                YOLOv3Files.YOLOv3Detector = tbYOLOv3Detector.Text;
                LoadDetectorData();
            }
        }

        private void LoadDetectorData()
        {
            LoadImageList();
            pbImage.Controls.Clear();
            pbImage.Image = null;
            _imgAnnotateBoundingBoxList.Clear();
        }

        private void btnContinueTrain_Click(object sender, EventArgs e)
        {
            var arguments = $"detector train ./{tbYOLOv3Detector.Text}/{tbYOLOv3Detector.Text}-data-auto-gen.data ./{tbYOLOv3Detector.Text}/yolov3-{tbYOLOv3Detector.Text}-auto-gen.cfg ./{tbYOLOv3Detector.Text}/yolov3-{tbYOLOv3Detector.Text}-auto-gen_last.weights ./train.log";
            ShowLog($"darknet.exe {arguments}");
            Process p = new Process();
            p.StartInfo.FileName = "darknet.exe";
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = false;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = false;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();//启动程序
        }
    }
}
