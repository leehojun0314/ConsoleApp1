using ConsoleApp1;
using OpenCvSharp;
namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                String fileloc = @"Sample Image/Large defect/20231113-028-1_03.jpg";
                Mat src = Cv2.ImRead(fileloc);
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
                ShowMenu(gray, fileloc);
            }
            catch (Exception ex) { 
                Console.WriteLine($"Unhandled exception: {ex}");
                return;
            }
        }

        static void ShowMenu(Mat gray, String fileloc)
        {
            Mat current = gray.Clone(); // 현재 이미지를 유지하며 연산 적용
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Current file: {fileloc}. Width: {gray.Width}, Height: {gray.Height}");
                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1. Canny Edge Detection");
                Console.WriteLine("2. Morphological Transformations");
                Console.WriteLine("3. Dilation");
                Console.WriteLine("4. Corner Detection (GoodFeaturesToTrack)");
                Console.WriteLine("5. Reset Image");
                Console.WriteLine("6. Align Wafer Notch");
                Console.WriteLine("7. Background");
                Console.WriteLine("8. Hough Lines");
                Console.WriteLine("s. Save image");
                Console.WriteLine("v. View image");
                Console.WriteLine("d. Dnn");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        current = Canny.RunCanny(current);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Choice: ");
                        Console.WriteLine("0. Erosion");
                        Console.WriteLine("1. Dilation");
                        Console.WriteLine("2. Opening");
                        Console.WriteLine("3. Closing");
                        var operation = Console.ReadLine();
                        if(operation == null)
                        {
                            break; ;
                        }
                        current = MorphologicalTransformation.RunMorphologicalTransformation(current, Int32.Parse(operation));
                        break;
                    case "3":
                        current = Dilation.RunDilation(current);
                        break;
                    case "4":
                        current = CornerDetection.RunCornerDetection(current);
                        break;
                    case "5":
                        current = gray.Clone();
                        Console.WriteLine("Image reset to original state.");
                        Cv2.WaitKey(1000);
                        break;
                    case "6":
                        current = WaferAlign.AlignWafer(current);
                        break;
                    case "7":
                        Console.Clear();
                        Console.WriteLine("1. Remove background");
                        Console.WriteLine("2. Crop to wafer edge");
                        var choice2 = Console.ReadLine();
                        switch (choice2) {
                            case "1":
                                current = Background.RemoveBackground(current, Scalar.White);
                                break;
                            case "2":
                                current = Background.CropToWaferEdge(current);
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Returning to main menu.");
                                break;
                        }

                        break;
                    case "8":
                        current = HoughLinesDetection.RunHoughLines(current);
                        break;
                    case "0":
                        return;
                    case "s":
                        try
                        {
                            String newPath = fileloc + "_modified.jpg";
                            current.SaveImage(newPath);
                            Console.WriteLine($"Image successfully saved to: {newPath}");
                            Cv2.WaitKey(0);
                        }catch(Exception ex) { 
                            Console.WriteLine(ex.ToString());
                            Cv2.WaitKey(0);
                        }
                        break;
                    case "v":
                        Cv2.ImShow("Current image", current);
                        Cv2.WaitKey(0);
                        break;
                    case "d":
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

    }
}
