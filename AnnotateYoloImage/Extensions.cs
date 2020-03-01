using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnotateYoloImage
{
    public static class StringExtensions
    {
        public static int ToInt(this string obj)
        {
            return int.Parse(obj);
        }

        public static double ToDouble(this string obj)
        {
            return double.Parse(obj);
        }

        public static decimal ToDecimal(this string obj)
        {
            return decimal.Parse(obj);
        }
    }

    public static class DoubleExtensions
    {
        public static int ToInt(this double obj)
        {
            return (int)obj;
        }
    }

    public static class RectangleExtensins
    {
        public static Point GetCenter(this Rectangle obj)
        {
            return new Point(obj.X + obj.Width / 2, obj.Y + obj.Height / 2);
        }
    }
}
