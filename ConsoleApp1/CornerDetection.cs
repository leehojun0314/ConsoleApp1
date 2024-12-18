namespace Project;
using OpenCvSharp;

public class CornerDetection
{
    public static Mat RunCornerDetection(Mat gray)
    {
        const string windowName = "Corner Detection";
        Cv2.NamedWindow(windowName);

        // Trackbar variables
        int maxCorners = 50; // 최대 검출할 코너 수
        double qualityLevel = 0.01; // 코너 품질 수준
        int minDistance = 10; // 코너 간 최소 거리

        Mat cornerImage = gray.Clone();
        Console.WriteLine("Corner Detection: Use trackbars to adjust parameters.");
        Console.WriteLine($"Initial Max Corners: {maxCorners}, Quality Level: {qualityLevel}, Min Distance: {minDistance}");

        // Create trackbars
        Cv2.CreateTrackbar("Max Corners", windowName, ref maxCorners, 200, (pos, _) =>
        {
            UpdateCornerDetection(gray, windowName, pos, qualityLevel, minDistance, ref cornerImage);
        });

        Cv2.CreateTrackbar("Min Distance", windowName, ref minDistance, 50, (pos, _) =>
        {
            UpdateCornerDetection(gray, windowName, maxCorners, qualityLevel, pos, ref cornerImage);
        });

        // Initial display
        UpdateCornerDetection(gray, windowName, maxCorners, qualityLevel, minDistance, ref cornerImage);

        Cv2.WaitKey();
        Cv2.DestroyWindow(windowName);

        return cornerImage; // 코너 검출된 이미지를 반환
    }

    static void UpdateCornerDetection(Mat gray, string windowName, int maxCorners, double qualityLevel, int minDistance, ref Mat cornerImage)
    {
        if (maxCorners < 1) maxCorners = 1; // 최소 코너 수 제한

        // mask에 null을 전달하여 전체 이미지에서 코너를 검출
        Point2f[] corners = Cv2.GoodFeaturesToTrack(
            gray,
            maxCorners,
            qualityLevel,
            minDistance,
            null, // mask를 null로 설정
            3,    // 블록 크기 (기본값 3)
            false, // 사용 여부 : 경사도 기반의 Harris 코너 검출 비활성화
            0.04   // Harris 코너 검출의 자유 파라미터 (사용하지 않으므로 의미 없음)
        );

        // 코너를 원본 이미지에 표시
        cornerImage = gray.CvtColor(ColorConversionCodes.GRAY2BGR);
        foreach (var corner in corners)
        {
            Cv2.Circle(cornerImage, (int)corner.X, (int)corner.Y, 5, Scalar.Red, -1);
        }

        Cv2.ImShow(windowName, cornerImage);
    }

}
