namespace Project;
using OpenCvSharp;

public class HoughLinesDetection
{
    public static Mat RunHoughLines(Mat src)
    {
        const string windowName = "Hough Lines Detection";
        Cv2.NamedWindow(windowName);

        Mat gray = new Mat();
        Mat edges = gray.Clone();
        // Mat edges = new Mat();

        // 입력 이미지가 이미 그레이스케일인지 확인
        if (src.Channels() == 1)
        {
            gray = src.Clone();
        }
        else
        {
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        }

        // 1. Canny Edge Detection
        // Cv2.Canny(gray, edges, 50, 150, 3, true);

        // 2. Hough Line Transform
        LineSegmentPolar[] lines = Cv2.HoughLines(edges, 1, Math.PI / 180, 150);
        // 결과 이미지 생성
        
        Mat result = src.Clone();

        // 3. 직선 그리기
        foreach (var line in lines)
        {
            float rho = line.Rho;
            float theta = line.Theta;

            // 선의 극좌표를 직교 좌표로 변환
            double a = Math.Cos(theta);
            double b = Math.Sin(theta);
            double x0 = a * rho;
            double y0 = b * rho;
            Point pt1 = new Point((int)(x0 + 1000 * (-b)), (int)(y0 + 1000 * a));
            Point pt2 = new Point((int)(x0 - 1000 * (-b)), (int)(y0 - 1000 * a));

            Cv2.Line(result, pt1, pt2, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
        }

        // 결과 표시
        Cv2.ImShow(windowName, result);
        Cv2.WaitKey();
        return result;
    }
}