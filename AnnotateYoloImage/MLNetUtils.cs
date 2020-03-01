using AnnotateYoloImage.DataModels;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnotateYoloImage
{
    public class MLNetUtils
    {
        private static MLNetUtils _instance;

        public static MLNetUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MLNetUtils();
                }
                return _instance;
            }
        }

        private PredictionEngine<InMemoryImageData, ImagePrediction> _pokerPredictionEngine;
        private PredictionEngine<InMemoryImageData, ImagePrediction> _chipPredictionEngine;
        private PredictionEngine<InMemoryImageData, ImagePrediction> _daojishiPredictionEngine;

        private MLNetUtils()
        {
            InitPredictEngine();
        }

        private void InitPredictEngine()
        {
            if (_pokerPredictionEngine == null)
            {
                var imageClassifierModelZipFilePath = Path.Combine(FileSysUtils.StartupPath, "PokerClassifier.zip");
                var mlContext = new MLContext(seed: 1);
                if (File.Exists(imageClassifierModelZipFilePath))
                {
                    Console.WriteLine($"Loading model from: {imageClassifierModelZipFilePath}");
                    // Load the model
                    var loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);
                    // Create prediction engine to try a single prediction (input = ImageData, output = ImagePrediction)
                    _pokerPredictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);
                }
            }
            if (_chipPredictionEngine == null)
            {
                var imageClassifierModelZipFilePath = Path.Combine(FileSysUtils.StartupPath, "ChipClassifier.zip");
                var mlContext = new MLContext(seed: 1);
                if (File.Exists(imageClassifierModelZipFilePath))
                {
                    Console.WriteLine($"Loading model from: {imageClassifierModelZipFilePath}");
                    // Load the model
                    var loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);
                    // Create prediction engine to try a single prediction (input = ImageData, output = ImagePrediction)
                    _chipPredictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);
                }
            }
            if(_daojishiPredictionEngine == null)
            {
                var imageClassifierModelZipFilePath = Path.Combine(FileSysUtils.StartupPath, "DaojishiClassifier.zip");
                var mlContext = new MLContext(seed: 1);
                if (File.Exists(imageClassifierModelZipFilePath))
                {
                    Console.WriteLine($"Loading model from: {imageClassifierModelZipFilePath}");
                    // Load the model
                    var loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);
                    // Create prediction engine to try a single prediction (input = ImageData, output = ImagePrediction)
                    _daojishiPredictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);
                }
            }
        }

        public void StartTrainModel(string samplePath, string modelName, Action<string> logFun, EventHandler<LoggingEventArgs> mlLog)
        {
            Task.Run(() =>
            {
                var mlContext = new MLContext(seed: 1);
                mlContext.Log += mlLog;//MlContext_Log;
                IEnumerable<ImageData> images = LoadImagesFromDirectory(samplePath);
                IDataView fullImagesDataset = mlContext.Data.LoadFromEnumerable(images);
                IDataView shuffledFullImageFilePathsDataset = mlContext.Data.ShuffleRows(fullImagesDataset);
                //已经将路径中加载图像信息并打散，接下来提取图像数据，并将标签转换为Key（独热编码）
                IDataView shuffledFullImagesDataset = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label", keyOrdinality: Microsoft.ML.Transforms.ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                    .Append(mlContext.Transforms.LoadRawImageBytes(outputColumnName: "Image", imageFolder: samplePath, inputColumnName: "ImagePath"))
                    .Fit(shuffledFullImageFilePathsDataset)//做数据转换
                    .Transform(shuffledFullImageFilePathsDataset);//转换为IDataView
                                                                  //将数据集拆分为训练集和验证集
                var trainTestData = mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2);
                IDataView trainDataView = trainTestData.TrainSet; //提取训练集
                IDataView testDataView = trainTestData.TestSet;   //提取验证集
                                                                  //定义dnn（深度神经网络）模型训练管道
                var pipeLine = mlContext.MulticlassClassification.Trainers.ImageClassification(featureColumnName: "Image", labelColumnName: "LabelAsKey", validationSet: testDataView)
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));
                var watch = Stopwatch.StartNew();
                //训练
                ITransformer trainedModel = pipeLine.Fit(trainDataView);
                watch.Stop();
                logFun($"训练耗时{watch.ElapsedMilliseconds}ms");
                //评估
                EvaluateModel(mlContext, testDataView, trainedModel, logFun);
                //保存
                var outputM1NetModelFilePath = Path.Combine(FileSysUtils.StartupPath, $"{modelName}.zip");
                mlContext.Model.Save(trainedModel, trainDataView.Schema, outputM1NetModelFilePath);
                //做单次预测
                //var predImgPath = Path.Combine(FileSysUtils.StartupPath, "images-for-predictions");
                //TrySinglePrediction(predImgPath, mlContext, trainedModel, logFun);
            });
        }

        private void TrySinglePrediction(string path, MLContext mlContext, ITransformer trainedModel, Action<string> logFun)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(trainedModel);
            var testImages = FileUtils.LoadInMemoryImagesFromDirectory(path, false);
            var imageToPredict = testImages.First();
            var prediction = predictionEngine.Predict(imageToPredict);
            logFun($"图像<{imageToPredict.ImageFileName}>");
            logFun($"Scores:[{string.Join(",", prediction.Score)}]");
            logFun($"预测标签：<{prediction.PredictedLabel}>");
        }

        private void EvaluateModel(MLContext mlContext, IDataView testDataset, ITransformer trainedModel, Action<string> logFun)
        {
            var watch = Stopwatch.StartNew();
            var predictionsDataView = trainedModel.Transform(testDataset);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
            watch.Stop();
            logFun($"评估完毕，结果<{metrics}>，耗时<{watch.ElapsedMilliseconds}ms>");
        }

        private IEnumerable<ImageData> LoadImagesFromDirectory(string folder)
        {
            return FileUtils.LoadImagesFromDirectory(folder, true).Select(x => new ImageData(x.imagePath, x.label));
        }

        public string PredictPoker(string file)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromFile(file, "unknown");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _pokerPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _pokerPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            return prediction.PredictedLabel;
        }

        public string PredictChip(string file)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromFile(file, "unknown");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _chipPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _chipPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            return prediction.PredictedLabel;
        }

        public string PredictChip(Image<Bgr, byte> img)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromOpenCvImage(img, "unknown", "opencv_memory_img");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _chipPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _chipPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            return prediction.PredictedLabel;
        }

        public string PredictDaojishi(string file)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromFile(file, "unknown");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _daojishiPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _daojishiPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            return prediction.PredictedLabel;
        }

        public string PredictDaojishi(Image<Bgr, byte> img)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromOpenCvImage(img, "unknown", "opencv_memory_img");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _daojishiPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _daojishiPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            return prediction.PredictedLabel;
        }

        public (string label, string pokerName) PredictPoker(Image<Bgr, byte> img)
        {
            InitPredictEngine();
            //从文件中读取图像数据                
            var imageToPredict = FileUtils.LoadInMemoryImageFromOpenCvImage(img, "unknown", "opencv_memory_img");

            // Measure #1 prediction execution time.
            var watch = Stopwatch.StartNew();
            var prediction = _pokerPredictionEngine.Predict(imageToPredict);
            // Stop measuring time.
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("预测耗时: " + elapsedMs + "mlSecs");
            // Get the highest score and its index
            var maxScore = prediction.Score.Max();
            // Double-check using the index
            var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
            VBuffer<ReadOnlyMemory<char>> keys = default;
            _pokerPredictionEngine.OutputSchema[3].GetKeyValues(ref keys);
            var keysArray = keys.DenseValues().ToArray();
            var predictedLabelString = keysArray[maxIndex];
            Debug.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                              $"Predicted Label : [{prediction.PredictedLabel}], " +
                              $"Probability : [{maxScore}] "
                              );
            var label = prediction.PredictedLabel;
            var pokerName = LabelToPokerName(label);
            return (label, pokerName);
        }

        private static Dictionary<string, string> LabelPokerNameDict = new Dictionary<string, string>
        { {"GrassA","A"},{ "HeartA","A"},{ "SpadeA","A"},{ "SquareA","A"},
        {"Grass2","2"},{ "Heart2","2"},{ "Spade2","2"},{ "Square2","2"},
        {"Grass3","3"},{ "Heart3","3"},{ "Spade3","3"},{ "Square3","3"},
        {"Grass4","4"},{ "Heart4","4"},{ "Spade4","4"},{ "Square4","4"},
        {"Grass5","5"},{ "Heart5","5"},{ "Spade5","5"},{ "Square5","5"},
        {"Grass6","6"},{ "Heart6","6"},{ "Spade6","6"},{ "Square6","6"},
        {"Grass7","7"},{ "Heart7","7"},{ "Spade7","7"},{ "Square7","7"},
        {"Grass8","8"},{ "Heart8","8"},{ "Spade8","8"},{ "Square8","8"},
        {"Grass9","9"},{ "Heart9","9"},{ "Spade9","9"},{ "Square9","9"},
        {"Grass10","10"},{ "Heart10","10"},{ "Spade10","10"},{ "Square10","10"},
        {"GrassJ","J"},{ "HeartJ","J"},{ "SpadeJ","J"},{ "SquareJ","J"},
        {"GrassQ","Q"},{ "HeartQ","Q"},{ "SpadeQ","Q"},{ "SquareQ","Q"},
        {"GrassK","K"},{ "HeartK","K"},{ "SpadeK","K"},{ "SquareK","K"},
        };

        private string LabelToPokerName(string label)
        {
            return LabelPokerNameDict[label];
        }
    }
}
