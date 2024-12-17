namespace Project;
using OpenCvSharp;

public class Dilation
{
    public static Mat RunDilation(Mat gray)
    {
        const string windowName = "Dilation";
        Cv2.NamedWindow(windowName);

        // Trackbar variables
        int kernelSize = 1; // 기본 커널 크기

        Mat dilated = gray.Clone(); // 결과를 저장할 Mat

        Console.WriteLine("Dilation: Use the trackbar to adjust kernel size.");
        Console.WriteLine($"Initial Kernel Size: {kernelSize}");

        // Create trackbar
        Cv2.CreateTrackbar("Kernel Size", windowName, ref kernelSize, 20, (pos, _) =>
        {
            UpdateDilation(gray, windowName, pos, ref dilated);
        });

        // Initial display
        UpdateDilation(gray, windowName, kernelSize, ref dilated);

        Cv2.WaitKey();
        Cv2.DestroyWindow(windowName);

        return dilated; // 결과 이미지를 반환
    }

    static void UpdateDilation(Mat gray, string windowName, int kernelSize, ref Mat dilated)
    {
        Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2 * kernelSize + 1, 2 * kernelSize + 1));
        Cv2.Dilate(gray, dilated, kernel);
        Cv2.ImShow(windowName, dilated);
    }
}