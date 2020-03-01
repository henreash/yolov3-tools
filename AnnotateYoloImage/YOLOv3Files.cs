using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnnotateYoloImage
{
    public class YOLOv3Files
    {
        public static string YOLOv3Detector { get; set; }

        public static string ClassesNameFile
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, "classes-auto-gen.names");
            }
        }

        public static string ConfigFile
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, $"yolov3-{YOLOv3Detector}-auto-gen.cfg");
            }
        }

        public static string ImagesPath
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, "images");
            }
        }

        public static string TrainSampleFile
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, "train-auto-gen.txt");
            }
        }

        public static string TestSampleFile
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, "test-auto-gen.txt");
            }
        }

        public static string DarknetDataFile
        {
            get
            {
                Trace.Assert(!string.IsNullOrEmpty(YOLOv3Detector));
                return Path.Combine(Application.StartupPath, YOLOv3Detector, $"{YOLOv3Detector}-data-auto-gen.data");
            }
        }
    }
}
