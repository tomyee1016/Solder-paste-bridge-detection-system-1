using OpenCvSharp;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat img = Cv2.ImRead("E:/workspace/ConsoleApp1/ConsoleApp1/image/img1.png");
            
            //灰度化
            Mat grayImg = new Mat();
            Cv2.CvtColor(img, grayImg, ColorConversionCodes.BGR2GRAY);
            //滤波
            Cv2.Blur(grayImg, grayImg, new Size(3, 3));
            
            //二值化
            Mat threImg = grayImg.Threshold(100, 255, ThresholdTypes.Otsu);
            

            
            //找轮廓
            Point[][] contours;
            HierarchyIndex[] hierarchly;
            Cv2.FindContours(threImg, out contours, out hierarchly, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contours.Length; i++)
            {
                double contourArea = Cv2.ContourArea(contours[i]);
                if (contourArea < 1000 & contourArea > 800)
                {
                     
                     Rect rect = Cv2.BoundingRect(contours[i]);
                     Mat roiImg1 = new Mat(img, rect);
                     Mat roiImg2 = roiImg1.Clone();
                     Mat roiImg3 = new Mat(threImg, rect);
                     Cv2.ImShow("roi1", roiImg2);
                   
                    Mat kernal_1= Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                    Mat kernal_2 = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));

                    Mat morphImg = new Mat();
                    Cv2.MorphologyEx(roiImg3, morphImg, MorphTypes.Erode,kernal_1);
                    Cv2.MorphologyEx(morphImg, morphImg, MorphTypes.Erode, kernal_2);
                    
                    Cv2.MorphologyEx(morphImg, morphImg, MorphTypes.Dilate, kernal_1);
                    Cv2.ImShow("mor", morphImg);

                    Point[][] contours1;
                    HierarchyIndex[] hierarchly1;
                    Cv2.FindContours(morphImg, out contours1, out hierarchly1, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
                    for(int j = 0; j < contours1.Length; j++)
                    {
                        Rect rect1 = Cv2.BoundingRect(contours1[j]);
                        Cv2.Rectangle(roiImg1, rect1, new Scalar(0, 0, 255), 2);
                        roiImg1.CopyTo(roiImg2);
                        Cv2.ImShow("result", img);
                        
                    }
                }
            }
            Cv2.WaitKey(0);
        }
    }
}

