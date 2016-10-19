#ifndef IncludeImageMetricsheader
#define IncludeImageMetricsheader
#include "ImageMetricsGlobals.h"
class imageMetrics
{
public:

	imageMetrics(void);
	~imageMetrics(void);
struct imageMetricsInfo 
	{
	public: int AverageIntensity;
	public: double peripheryInnerVariation;
	public: double topToBottonVariation;
	public: int	m_sectorIntensities[8];
	};
	void CalcImageMetrics(IplImage* bm, bool isSectorCalc);
imageMetricsInfo* ImageMetrics;
    
private:
	IplImage * mask1mp_emgu,*mask3mp_emgu,* mask1mp_emgu_color,*mask3mp_emgu_color ,*ImageInnerMask_halfRes,*ImageInnerMask_fullRes,
		*innerImageDataThresh_halfRes,*innerImageDataThresh_fullRes,*peripheryImageDataThresh_halfRes,
		*peripheryImageDataThresh_fullRes,*peripheryImageData_halfRes,*peripheryImageData_fullRes,*im_halfRes,*im_fullRes,
		*innerImageData_halfRes,*innerImageData_fullRes;

	IplImage *m_pSectorMask1mp;
	IplImage *m_pSectorRegionMask1mp;
	IplImage *m_pSectorMask3mp;
	IplImage *m_pSectorRegionMask3mp;

	void CalcRowProfile(IplImage* bm);
	void CalcAverageIntensity(IplImage *bm);
	void CalcSectorIntensities(IplImage *bm);
};
#endif