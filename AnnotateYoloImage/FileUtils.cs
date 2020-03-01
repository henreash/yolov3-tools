using AnnotateYoloImage.DataModels;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AnnotateYoloImage
{
    public class FileUtils
    {
        public static IEnumerable<(string imagePath, string label)> LoadImagesFromDirectory(
            string folder,
            bool useFolderNameasLabel)
        {
            var imagesPath = Directory
                .GetFiles(folder, "*", searchOption: SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png");

            return useFolderNameasLabel
                ? imagesPath.Select(imagePath => (imagePath, Directory.GetParent(imagePath).Name))
                : imagesPath.Select(imagePath =>
                {
                    var label = Path.GetFileName(imagePath);
                    for (var index = 0; index < label.Length; index++)
                    {
                        if (!char.IsLetter(label[index]))
                        {
                            label = label.Substring(0, index);
                            break;
                        }
                    }
                    return (imagePath, label);
                });
        }

        public static IEnumerable<InMemoryImageData> LoadInMemoryImagesFromDirectory(
            string folder,
            bool useFolderNameAsLabel = true)
            => LoadImagesFromDirectory(folder, useFolderNameAsLabel)
                .Select(x => LoadInMemoryImageFromFile(x.imagePath, x.label));
        //=> LoadImagesFromDirectory(folder, useFolderNameAsLabel)
        //    .Select(x => new InMemoryImageData(
        //        image: File.ReadAllBytes(x.imagePath),
        //        label: x.label,
        //        imageFileName: Path.GetFileName(x.imagePath)));

        public static string GetAbsolutePath(Assembly assembly, string relativePath)
        {
            var assemblyFolderPath = new FileInfo(assembly.Location).Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }

        public static InMemoryImageData LoadInMemoryImageFromFile(string file, string label)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(file);
            var bytes = img.ToJpegData();
            return new InMemoryImageData(image: bytes, label: label, imageFileName: Path.GetFileName(file));
            //return new InMemoryImageData(image: File.ReadAllBytes(file), label:"unknown", imageFileName: Path.GetFileName(file));
        }

        public static InMemoryImageData LoadInMemoryImageFromOpenCvImage(Image<Bgr, byte> img, string label, string fileName)
        {
            var bytes = img.ToJpegData();
            return new InMemoryImageData(image: bytes, label: label, imageFileName: fileName);
            //return new InMemoryImageData(image: File.ReadAllBytes(file), label:"unknown", imageFileName: Path.GetFileName(file));
        }
    }
}
