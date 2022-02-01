using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCG
{
    class Program
    {
        static void Main(string[] args)
        {

            String pathInput = "C:\\Users\\User\\Desktop\\3 курс 1 семестр\\КГ\\для тестов кг\\test1BW.jpg";
            String pathOutputNeighbor = "C:\\Users\\User\\Desktop\\3 курс 1 семестр\\КГ\\для тестов кг\\test1BW_newNeighbor.jpg";
            String pathOutputBilinel = "C:\\Users\\User\\Desktop\\3 курс 1 семестр\\КГ\\для тестов кг\\test1BW_newBilinel.jpg";
            String pathOutputBicubic = "C:\\Users\\User\\Desktop\\3 курс 1 семестр\\КГ\\для тестов кг\\test1BW_newBicubic.jpg";
            double sizeChange = 3; //коэффициент увеличения
            try
            {
                MyImage image = new MyImage(pathInput);
                Console.WriteLine("Исходная ширина изображения: " + image.width);
                Console.WriteLine("Исходная высота изображения: " + image.height);
                Console.WriteLine("Полученная ширина изображения: " + image.getWidth(image.NearestNeighborInterpolation(sizeChange)));
                Console.WriteLine("Полученная высота изображения " + image.getHeight(image.NearestNeighborInterpolation(sizeChange)));
                image.newImageJpgNNI(sizeChange, pathOutputNeighbor);
                image.newImageJpgBil(sizeChange, pathOutputBilinel);
                if (image.width > 3 && image.height > 3)
                {
                    image.newImageJpgBicubic(sizeChange, pathOutputBicubic);
                }
                else
                {
                    image.newImageJpgBil(sizeChange, pathOutputBicubic);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
