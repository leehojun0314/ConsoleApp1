namespace Project;
using OpenCvSharp;

public class WaferAlign
{
    public static Mat AlignWafer(Mat src)
    {
        const string windowName = "Wafer Alignment";
        Cv2.NamedWindow(windowName);

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

        // 5. 노치 찾기
        Point2f notchPoint = FindNotch(waferContour, center, radius);

        // 디버깅: 찾은 노치에 빨간색 원 그리기
        Mat debugImage = src.Clone();
        Cv2.Circle(debugImage, (int)center.X, (int)center.Y, 5, new Scalar(0, 255, 0), -1); // 중심점
        Cv2.Circle(debugImage, (int)notchPoint.X, (int)notchPoint.Y, 10, new Scalar(0, 0, 255), -1); // 노치에 빨간색 원

        Cv2.ImShow("Detected Notch", debugImage);

        // 6. 회전 각도 계산
        double angle = CalculateRotationAngle(center, notchPoint);
        Console.WriteLine($"Rotation angle to align notch: {angle} degrees");

        // 7. 이미지 회전
        Mat rotated = RotateImage(src, center, angle);
        Cv2.ImShow(windowName, rotated);
        Cv2.WaitKey();
        return rotated;
    }

    private static Point2f FindNotch(Point[] contour, Point2f center, float radius)
    {
        Point2f notchPoint = new Point2f();
        double minDistance = radius;

        foreach (var point in contour)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));
            if (distance < minDistance)
            {
                minDistance = distance;
                notchPoint = new Point2f(point.X, point.Y);
            }
        }

        Console.WriteLine($"Notch found at: ({notchPoint.X}, {notchPoint.Y}), Distance: {minDistance}");
        return notchPoint;
    }

    private static double CalculateRotationAngle(Point2f center, Point2f notchPoint)
    {
        double dx = notchPoint.X - center.X;
        double dy = center.Y - notchPoint.Y; // y좌표 반전
        double angle = Math.Atan2(dy, dx) * 180 / Math.PI; // 라디안 → 도(degree)

        // 동쪽에서 북쪽으로 회전하기 위해 90도 추가
        return angle - 90;
    }

    private static Mat RotateImage(Mat src, Point2f center, double angle)
    {
        Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, -angle, 1.0);
        Mat rotated = new Mat();
        Cv2.WarpAffine(src, rotated, rotationMatrix, src.Size());
        return rotated;
    }
}
