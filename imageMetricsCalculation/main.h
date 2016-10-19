#include "imageMetrics.h"


extern "C"__declspec(dllexport) void ImageMetrics_Init();

extern "C"__declspec(dllexport) void ImageMetrics_Calculate(IplImage* srcImage,imageMetrics::imageMetricsInfo* imageParams,int data[], bool isSectorCalc );

extern "C"__declspec(dllexport) void ImageMetrics_Exit();
