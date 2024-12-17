using OpenCvSharp;
namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat src = Cv2.ImRead("/Users/ihojun/RiderProjects/ConsoleApp1/ConsoleApp1/bin/Debug/net9.0/Sample Image/Large defect/20231113-028-1_05.jpg");
            if (src.Empty())
            {
                Console.WriteLine("Image not found");
                return;
            }

            // Resize the image for better display if necessary
            Cv2.Resize(src, src, new OpenCvSharp.Size(src.Width, src.Height));

            // Grayscale conversion
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Start the UI with menu
            ShowMenu(gray);
        }

      static void ShowMenu(Mat gray)
{
    Mat current = gray.Clone(); // 현재 이미지를 유지하며 연산 적용
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Choose an operation:");
        Console.WriteLine("1. Canny Edge Detection");
        Console.WriteLine("2. Morphological Transformations");
        Console.WriteLine("3. Dilation");
        Console.WriteLine("4. Reset Image");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                current = Canny.RunCanny(current); // 결과 이미지를 반환받아 업데이트
                break;
            case "2":
                current = MorphologicalTransformation.RunMorphologicalTransformation(current); // 결과 이미지를 반환받아 업데이트
                break;
            case "3":
                current = Dilation.RunDilation(current); // 새로운 팽창 연산 추가
                break;
            case "9":
                current = gray.Clone(); // 원본으로 리셋
                Console.WriteLine("Image reset to original state.");
                Cv2.WaitKey(1000);
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
        }
    }
}

    }
}
