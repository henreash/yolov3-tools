# yolov3-tools
本工具使用vs2019社区版C# winform开发，提供手动标注yolov3训练图像功能。<br />
下载代码后，请阅读<工具使用说明>，下载已编译好的工具压缩包，内带依赖资源（含google提供的雪人训练范例图像及标注文件）。<br />
工具依赖alexeyab版本的darknet windows可执行程序darknet.exe，而且依赖nvidia驱动，如下载的工具运行有问题，请自行编译darknet。<br />

# 参考资料
[darknet在windows下的编译过程](doc/yolov3-train-tutorial.pdf)<br />
[工具使用说明书](https://henreash.blog.csdn.net/article/details/104602718)

# 扩展话题
yolo训练需要大量样本，因此标注的成本较大，可考虑采用opencv做一个目标定位、检测的小模块，自动进行标注，节省大量的人力成本。
