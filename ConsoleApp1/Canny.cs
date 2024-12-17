namespace Project;
using OpenCvSharp;

public class Canny
{
    public static Mat RunCanny(Mat gray)
    {
        const string windowName = "Canny Edge Detection";
        Cv2.NamedWindow(windowName);

        int lowerThreshold = 18;
        int upperThreshold = 6;

        Mat edges = gray.Clone(); // 결과를 저장할 Mat

        Cv2.CreateTrackbar("Lower Thrshd", windowName, ref lowerThreshold, 255, (pos, _) =>
        {
            UpdateCanny(gray, windowName, pos, upperThreshold, ref edges);
        });
        Cv2.CreateTrackbar("Upper Thrshd", windowName, ref upperThreshold, 255, (pos, _) =>
        {
            UpdateCanny(gray, windowName, lowerThreshold, pos, ref edges);
        });

        UpdateCanny(gray, windowName, lowerThreshold, upperThreshold, ref edges);

        Cv2.WaitKey();
        Cv2.DestroyWindow(windowName);

        return edges; // 결과 이미지를 반환
    }

    static void UpdateCanny(Mat gray, string windowName, int lower, int upper, ref Mat edges)
    {
        Cv2.Canny(gray, edges, lower, upper);
        Cv2.ImShow(windowName, edges);
    }

}