// $URL: https://forussrv01/svn/CalibrationFundusImages/trunk/imageMetricsCalculation/main.cpp $
// $LastChangedBy: Shivaraj $ on $LastChangedDate: 2016-07-14 16:54:24 +0530 (Thu, 14 Jul 2016) $
//$Rev: 251 $
//Copyright (C) 2012, Forus Health Pvt. Ltd.
// sriram@forushealth.com
/* **********************************************************************************
 * File name : main.cpp
 *  
 * This class is used in Calibration software contains the entry point to the imageMeticsCalculation dll
 * Which mainly computes the average intensity,periphery to inner variation and top to bottom variation and returns the values as a structure. It is also used to
 * initialize  various variables used by imageMeticsCalculation.
 * Author: Sriram
 * Last modifed: 
 * 
 *************************************************************************************
 */
#include "imageMetrics.h"
#include"main.h"
imageMetrics* Metrics;

#pragma region Used for exe application
//void main()
//{
//	IplImage* srcImage = cvLoadImage("C:\\CalibrationImages\\9\\25-02-2013\\9_145015_IR.png");
//	imageMetrics* Metrics = new imageMetrics();
//	double startClock = cvGetTickCount();
//	Metrics->CalcImageMetrics(srcImage);
//	startClock = (cvGetTickCount()- startClock)/(cvGetTickFrequency()*1000);
//	cout<<"Average Intensity = "<<Metrics->ImageMetrics->AverageIntensity<<"\n";
//	cout<<"Periphery To Inner Variation = "<<Metrics->ImageMetrics->peripheryInnerVariation<<"\n";
//	cout<<"Top to Bottom Variation"<<Metrics->ImageMetrics->topToBottonVariation<<"\n";
//	cout<<"Time Taken = "<<startClock;
//	Metrics->~imageMetrics();
//	getchar();
//}
#pragma endregion


extern "C"__declspec(dllexport) void ImageMetrics_Init()
{
 Metrics = new imageMetrics();
}

extern "C"__declspec(dllexport) void ImageMetrics_Calculate(IplImage* srcImage,imageMetrics::imageMetricsInfo* imageParams,int data[], bool isSectorCalc)
{
Metrics->CalcImageMetrics(srcImage, isSectorCalc );
imageParams->AverageIntensity = Metrics->ImageMetrics->AverageIntensity;
imageParams->peripheryInnerVariation = Metrics->ImageMetrics->peripheryInnerVariation;
imageParams->topToBottonVariation = Metrics->ImageMetrics->topToBottonVariation;

if(isSectorCalc)
{
	int indx;
	for (indx = 0; indx < 8; indx++)
	{
		data[indx] = Metrics->ImageMetrics->m_sectorIntensities[indx];
	}
}

}

extern "C"__declspec(dllexport) void ImageMetrics_Exit()
{
Metrics->~imageMetrics();
}


