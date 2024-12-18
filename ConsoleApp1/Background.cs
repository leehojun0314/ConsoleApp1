using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Background
    {
        public Background()
        {
        }
        public static Mat RemoveBackground(Mat src, Scalar backgroundColor)
        {
            Mat gray = new Mat();

            // 입력 이미지가 이미 그레이스케일인지 확인
            if (src.Channels() == 1)
            {
                gray = src.Clone();
            }
            else
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            }

            // 1. 평균값 기반 Threshold 이진화
            Scalar meanValue = Cv2.Mean(gray);
            double thresholdVal = meanValue.Val0;
            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, thresholdVal - 50, 255, ThresholdTypes.BinaryInv);
            Cv2.ImShow("Threshold", binary);

            // 2. 윤곽선 찾기
            Cv2.FindContours(binary, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
            {
                Console.WriteLine("윤곽선을 찾을 수 없습니다.");
                return src;
            }

            // 3. 가장 큰 윤곽선 찾기
            int maxContourIdx = 0;
            double maxArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxContourIdx = i;
                }
            }

            Point[] waferContour = contours[maxContourIdx];

            // 4. 최소 외접 원 검출
            Point2f center;
            float radius;
            Cv2.MinEnclosingCircle(waferContour, out center, out radius);
            Console.WriteLine($"Detected Circle - Center: ({center.X}, {center.Y}), Radius: {radius}");

            // 5. 마스크 생성
            Mat mask = new Mat(src.Size(), MatType.CV_8UC1, Scalar.All(0));
            Cv2.Circle(mask, (int)center.X, (int)center.Y, (int)radius, Scalar.All(255), -1);

            // 디버깅용 마스크 표시
            Cv2.ImShow("Mask", mask);

            // 6. 배경 삭제
            Mat result = new Mat(src.Size(), src.Type(), backgroundColor);
            src.CopyTo(result, mask);

            Cv2.ImShow("Result", result);
            Cv2.WaitKey();

            return result;
        }
        public static Mat CropToWaferEdge(Mat src)
        {
            Mat gray = new Mat();

            // 입력 이미지가 이미 그레이스케일인지 확인
            if (src.Channels() == 1)
            {
                gray = src.Clone();
            }
            else
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            }
            Console.WriteLine("flag 1");
            // 1. 평균값 기반 Threshold 이진화
            Scalar meanValue = Cv2.Mean(gray);
            double thresholdVal = meanValue.Val0;
            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, thresholdVal - 50, 255, ThresholdTypes.BinaryInv);

            // 2. 윤곽선 찾기
            Cv2.FindContours(binary, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
            {
                Console.WriteLine("윤곽선을 찾을 수 없습니다.");
                return src;
            }

            // 3. 가장 큰 윤곽선 찾기
            int maxContourIdx = 0;
            double maxArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxContourIdx = i;
                }
            }
            Console.WriteLine("max contour index: " + maxContourIdx);

            Point[] waferContour = contours[maxContourIdx];

            // 4. 최소 외접 원 검출
            Point2f center;
            float radius;
            Cv2.MinEnclosingCircle(waferContour, out center, out radius);
            Console.WriteLine($"Detected Circle - Center: ({center.X}, {center.Y}), Radius: {radius}");

            // 5. Crop 영역 계산
            int x = Math.Max((int)(center.X - radius), 0);
            int y = Math.Max((int)(center.Y - radius), 0);
            int width = Math.Min((int)(2 * radius), src.Width - x);
            int height = Math.Min((int)(2 * radius), src.Height - y);

            Rect cropRect = new Rect(x, y, width, height);
            Console.WriteLine($"Cropping to Rect: X={cropRect.X}, Y={cropRect.Y}, Width={cropRect.Width}, Height={cropRect.Height}");

            // 6. Crop 적용
            Mat cropped = new Mat(src, cropRect);
            Console.WriteLine("crop complete");
            Cv2.ImShow("Cropped Wafer", cropped);
            Cv2.WaitKey();

            return cropped;
        }

    }
}
