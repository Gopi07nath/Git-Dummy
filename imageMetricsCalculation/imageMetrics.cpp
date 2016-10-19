#include "imageMetrics.h"

imageMetrics::imageMetrics(void)
{
	mask1mp_emgu_color = cvLoadImage("Resources\\Mask_1MP.bmp",CV_LOAD_IMAGE_COLOR);
	mask3mp_emgu_color = cvLoadImage("Resources\\Mask_3MP.bmp",CV_LOAD_IMAGE_COLOR);

	mask1mp_emgu = cvLoadImage("Resources\\Mask_1MP.bmp",CV_LOAD_IMAGE_GRAYSCALE);
	mask3mp_emgu = cvLoadImage("Resources\\Mask_3MP.bmp",CV_LOAD_IMAGE_GRAYSCALE);

	ImageInnerMask_halfRes = cvLoadImage("Resources\\InnerMask2.bmp",CV_LOAD_IMAGE_GRAYSCALE);
	ImageInnerMask_fullRes = cvLoadImage("Resources\\InnerMask1.bmp",CV_LOAD_IMAGE_GRAYSCALE);

	m_pSectorMask1mp = cvLoadImage("Resources\\sectorMask_1MP.bmp",CV_LOAD_IMAGE_GRAYSCALE);
	m_pSectorMask3mp = cvLoadImage("Resources\\sectorMask_3MP.bmp",CV_LOAD_IMAGE_GRAYSCALE);

	m_pSectorRegionMask1mp = cvCreateImage(cvGetSize(m_pSectorMask1mp), m_pSectorMask1mp->depth, 1);
	m_pSectorRegionMask3mp = cvCreateImage(cvGetSize(m_pSectorMask3mp), m_pSectorMask3mp->depth, 1);
	innerImageDataThresh_halfRes = cvCreateImage(cvGetSize(ImageInnerMask_halfRes),ImageInnerMask_halfRes->depth,1);
	innerImageDataThresh_fullRes = cvCreateImage(cvGetSize(ImageInnerMask_fullRes),ImageInnerMask_fullRes->depth,1);

	peripheryImageDataThresh_halfRes = cvCreateImage(cvGetSize(ImageInnerMask_halfRes),ImageInnerMask_halfRes->depth,1);
	peripheryImageDataThresh_fullRes = cvCreateImage(cvGetSize(ImageInnerMask_fullRes),ImageInnerMask_fullRes->depth,1);

	peripheryImageData_halfRes = cvCreateImage(cvGetSize(ImageInnerMask_halfRes),ImageInnerMask_halfRes->depth,1);
	peripheryImageData_fullRes = cvCreateImage(cvGetSize(ImageInnerMask_fullRes),ImageInnerMask_fullRes->depth,1);

	im_halfRes = cvCreateImage(cvGetSize(ImageInnerMask_halfRes),ImageInnerMask_halfRes->depth,1);
	im_fullRes = cvCreateImage(cvGetSize(ImageInnerMask_fullRes),ImageInnerMask_fullRes->depth,1);

	innerImageData_halfRes = cvCreateImage(cvGetSize(ImageInnerMask_halfRes),ImageInnerMask_halfRes->depth,1);
	innerImageData_fullRes = cvCreateImage(cvGetSize(ImageInnerMask_fullRes),ImageInnerMask_fullRes->depth,1);

	ImageMetrics = new imageMetricsInfo();

}


imageMetrics::~imageMetrics(void)
{
	cvReleaseImage(&mask1mp_emgu);
	cvReleaseImage(&mask3mp_emgu);
	cvReleaseImage(&mask1mp_emgu_color);
	cvReleaseImage(&mask3mp_emgu_color);
	cvReleaseImage(&ImageInnerMask_halfRes);
	cvReleaseImage(&ImageInnerMask_fullRes);
	cvReleaseImage(&innerImageDataThresh_halfRes);
	cvReleaseImage(&innerImageDataThresh_fullRes);
	cvReleaseImage(&peripheryImageDataThresh_halfRes);
	cvReleaseImage(&peripheryImageDataThresh_fullRes);
	cvReleaseImage(&peripheryImageData_halfRes);
	cvReleaseImage(&peripheryImageData_fullRes);
	cvReleaseImage(&im_halfRes);
	cvReleaseImage(&im_fullRes);
	cvReleaseImage(&innerImageData_halfRes);
	cvReleaseImage(&innerImageData_fullRes);
	cvReleaseImage(&m_pSectorMask1mp);
	cvReleaseImage(&m_pSectorMask3mp);
	cvReleaseImage(&m_pSectorRegionMask1mp);
	cvReleaseImage(&m_pSectorRegionMask3mp);
}

std::vector<double> rowprofileSum ;
CvScalar sumOfsumPeripheryImage,SumofSumInnerthreshImage,sumOfSumPeripherythreshImage,sumOfSumInnerImage;

CvScalar rowValue ;
void imageMetrics::CalcRowProfile(IplImage* bm)
{
	if (bm->width == mask1mpWidth)
	{
		cvSplit(bm,0,0,im_halfRes,0);
		cvAnd(im_halfRes,mask1mp_emgu,im_halfRes);
		for (int i = 0; i < im_halfRes->height; i++)
		{
			cvSetImageROI(im_halfRes,cvRect(0, i, im_halfRes->width, 1));
			rowValue =cvSum(im_halfRes);                   
			rowprofileSum.push_back (rowValue.val[0]);
			rowprofileSum[i] = rowprofileSum[i] / pixelValues_IR[i];
			cvResetImageROI(im_halfRes);
		}
		cvAnd(im_halfRes,ImageInnerMask_halfRes,innerImageData_halfRes);
		cvThreshold(innerImageData_halfRes,innerImageDataThresh_halfRes,0,1,CV_THRESH_BINARY);          
		SumofSumInnerthreshImage = cvSum(innerImageDataThresh_halfRes);

		cvSub(im_halfRes,innerImageData_halfRes,peripheryImageData_halfRes);
		cvThreshold(peripheryImageData_halfRes,peripheryImageDataThresh_halfRes,0,1,CV_THRESH_BINARY);          

		sumOfSumPeripherythreshImage = cvSum(peripheryImageDataThresh_halfRes);
		sumOfsumPeripheryImage = cvSum(peripheryImageData_halfRes);
		sumOfSumInnerImage = cvSum(innerImageData_halfRes);

	}
	else
	{	
		cvSplit(bm,0,0,im_fullRes,0);
		cvAnd(im_fullRes,mask3mp_emgu,im_fullRes);
		for (int i = 0; i < im_fullRes->height; i++)
		{
			cvSetImageROI(im_fullRes,cvRect(0, i, im_fullRes->width, 1));
			rowValue =cvSum(im_fullRes);                   
			rowprofileSum.push_back (rowValue.val[0]);
			rowprofileSum[i] = rowprofileSum[i] / pixelValues[i];
			cvResetImageROI(im_fullRes);
		}
		cvAnd(im_fullRes,ImageInnerMask_fullRes,innerImageData_fullRes);
		cvThreshold(innerImageData_fullRes,innerImageDataThresh_fullRes,0,1,CV_THRESH_BINARY);          
		SumofSumInnerthreshImage = cvSum(innerImageDataThresh_fullRes);
		cvSub(im_fullRes,innerImageData_fullRes,peripheryImageData_fullRes);
		cvThreshold(peripheryImageData_fullRes,peripheryImageDataThresh_fullRes,0,1,CV_THRESH_BINARY);          

		sumOfSumPeripherythreshImage = cvSum(peripheryImageDataThresh_fullRes);
		sumOfsumPeripheryImage = cvSum(peripheryImageData_fullRes);
		sumOfSumInnerImage = cvSum(innerImageData_fullRes);

	}
	double peripheryToInnerImageRatio =0;
	double divider = (sumOfSumInnerImage.val[0] * sumOfSumPeripherythreshImage.val[0]);
	double divisor = (sumOfsumPeripheryImage.val[0] * SumofSumInnerthreshImage.val[0]);
	if((divisor !=0&&divider!=0))
		peripheryToInnerImageRatio = divisor / divider;

	double sum = std::accumulate(rowprofileSum.begin(), rowprofileSum.end(), 0.0);
	double mean = sum / rowprofileSum.size();
	std::vector<double> diff(rowprofileSum.size());
	std::transform(rowprofileSum.begin(), rowprofileSum.end(), diff.begin(),
		std::bind2nd(std::minus<double>(), mean));
	double sq_sum = std::inner_product(diff.begin(), diff.end(), diff.begin(), 0.0);
	double stdev = std::sqrt(sq_sum / rowprofileSum.size());
	/*double lowerLimit1 = average.val[0] - 2 * std_ev.val[0];
	double lowerLimit2 = average.val[0] - 3 * std_ev.val[0];
	double upperLimit1 = average.val[0] + 2 * std_ev.val[0];
	double upperLimit2 = average.val[0] + 3 * std_ev.val[0];*/
	double cv=0;
	if((stdev!=0&&mean!=0))
		cv = stdev / mean;
	this->ImageMetrics->topToBottonVariation = cv;
	this->ImageMetrics->peripheryInnerVariation = peripheryToInnerImageRatio;
	rowprofileSum.clear();
	//double lowerPerLimit1[] = rowprofileSum.Select(val => val < lowerLimit1 ? 1.0 : 0).ToArray();
	//double lowerPerLimit2[] = rowprofileSum.Select(val => val < lowerLimit2 ? 1.0 : 0).ToArray();
	//double upperPerLimit1[] = rowprofileSum.Select(val => val > upperLimit1 ? 1.0 : 0).ToArray();
	//double upperPerLimit2[] = rowprofileSum.Select(val => val > upperLimit2 ? 1.0 : 0).ToArray();
	//double lowerPer1 = lowerPerLimit1.Sum();
	//double lowerPer2 = lowerPerLimit2.Sum();
	//double upperPer1 = upperPerLimit1.Sum();
	//double upperPer2 = upperPerLimit2.Sum();
	//Array.Copy(rowprofileSum, rowProfileArray, rowprofileSum.Length);
	//double percentage1stLevelDeviations = ((lowerPer1 + upperPer1) * 100) / bm->height;
	//double percentage2ndLevelDeviations = ((lowerPer2 + upperPer2) * 100) / bm->height;
	////AvgRowProfileVal_lbl.Text = "\rAverageRowProfile : " + (Convert.ToInt32(average)).ToString();
	//cv = cv + 0.05;
	//covVal_lbl.Text = (Math.Round(cv, 2)).ToString();
	//peripheryToInnerImageRatio = peripheryToInnerImageRatio + 0.05;
	//perToInnerVarVal_lbl.Text =  (Math.Round(peripheryToInnerImageRatio, 2)).ToString();
}

void imageMetrics::CalcAverageIntensity(IplImage *bm)
{
	CvScalar avgValue;
	if(bm->width == this->mask1mp_emgu->width)
	{
		avgValue = cvAvg(bm,mask1mp_emgu);
	}
	else
	{
		avgValue = cvAvg(bm,mask3mp_emgu);

	}
	this->ImageMetrics->AverageIntensity = (int)avgValue.val[2];
}

void imageMetrics::CalcSectorIntensities(IplImage *bm)
{
	IplImage *sectorMask;
	IplImage *sectorRegionMask;
	int indx;

	if (bm->width == this->m_pSectorMask1mp->width)
	{
		sectorMask = m_pSectorMask1mp;
		sectorRegionMask = m_pSectorRegionMask1mp;
	}
	else
	{
		sectorMask = m_pSectorMask3mp;
		sectorRegionMask = m_pSectorRegionMask3mp;
	}
	for (indx = 0; indx < 8; indx++)
	{
		cvCmpS(sectorMask, 32*(indx+1)-1, sectorRegionMask, CV_CMP_EQ);
		this->ImageMetrics->m_sectorIntensities[indx] = (int)cvAvg(bm, sectorRegionMask).val[2];
	}
}

void imageMetrics::CalcImageMetrics(IplImage *bm, bool isSectorCalc)
{
	CalcRowProfile(bm);
	CalcAverageIntensity(bm);
	if(isSectorCalc)
	{
		CalcSectorIntensities(bm);
	}
}
