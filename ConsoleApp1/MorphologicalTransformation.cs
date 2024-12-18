namespace Project;
using OpenCvSharp;

public class MorphologicalTransformation
{
    public static Mat RunMorphologicalTransformation(Mat gray, int operation)
    {
        const string windowName = "Morphological Transformations";
        Cv2.NamedWindow(windowName);

        //operation 0: Erosion, 1: Dilation, 2: Opening, 3: Closing
        int kernelSize = 1;

        Mat result = gray.Clone(); // 결과를 저장할 Mat

        Cv2.CreateTrackbar("Operation", windowName, ref operation, 3, (pos, _) =>
        {
            UpdateMorphology(gray, windowName, kernelSize, pos, ref result);
        });
        Cv2.CreateTrackbar("Kernel Size", windowName, ref kernelSize, 20, (pos, _) =>
        {
            UpdateMorphology(gray, windowName, pos, operation, ref result);
        });

        UpdateMorphology(gray, windowName, kernelSize, operation, ref result);

        Cv2.WaitKey();
        Cv2.DestroyWindow(windowName);

        return result; // 결과 이미지를 반환
    }

    static void UpdateMorphology(Mat gray, string windowName, int kernelSize, int operation, ref Mat result)
    {
        Mat kernel = Cv2.GetStructuringElement(MorphShapes.Cross, new OpenCvSharp.Size(2 * kernelSize + 1, 2 * kernelSize + 1));

        switch (operation)
        {
            case 0:
                Cv2.Erode(gray, result, kernel);
                break;
            case 1:
                Cv2.Dilate(gray, result, kernel);
                break;
            case 2:
                Cv2.MorphologyEx(gray, result, MorphTypes.Open, kernel);
                break;
            case 3:
                Cv2.MorphologyEx(gray, result, MorphTypes.Close, kernel);
                break;
        }

        Cv2.ImShow(windowName, result);
    }

}
