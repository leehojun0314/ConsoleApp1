using OpenCvSharp.Dnn;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class DNN : CvDnn
    {
        //static readonly DNN Instance;
        private static DNN _instance;
        public static DNN Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DNN();
                }
                return _instance;
            }
        }
    }
}
