//Programmed by Subrat Kumar Gupta.
//Date: 12th March 2018.
//Description: To find the intersecting rectangles and draw intersecting ones in red color and non-interseting ones in white(default) color.
//Code flow is implemented for rotated rectangles, but sin cos values are not coming proper(Stuck, could not reach to the SAT algorithm implementation) when trying to get rotated co-ordinates with respect
//to angle.
//Need to Improve: Draw() method.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangleCode
{    
    class Program
    {
        static void Main()
        {
            while (true)
            {
                int noOfRectI = 0;
                bool isValidate = true;
                RectangleFunctionalities rectFunctionalities = new RectangleFunctionalities();
                Input1:
                Console.WriteLine("Enter the total no of rectangle you want to draw: ");
                string noOfRectS = Console.ReadLine();
                if (rectFunctionalities.ValidateInputs(noOfRectS))
                    noOfRectI = Convert.ToInt32(noOfRectS);
                else
                    goto Input1;
                List<Rectangle> lstRectangle = new List<Rectangle>();
                string[] seperators = { ",", " ", ";", "." }; //To saperate the inputs.
                for (int i = 0; i < noOfRectI; i++)
                {
                    Input2:
                    //Console.WriteLine("Format: height, width, location_X, location_Y, angle (in case roatated rectangle)"); --In case of rotated rectangle
                    Console.WriteLine("Format: height, width, location_X, location_Y");
                    Console.WriteLine("Please enter the values for rectangle no: " + (i + 1));
                    Rectangle rectangle = new Rectangle();
                    string[] splitInputs = Console.ReadLine().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        isValidate = rectFunctionalities.ValidateInputs(splitInputs);
                        if (!isValidate)
                            goto Input2;
                        for (int j = 0; j < splitInputs.Length; j++)
                        {
                            int val = Convert.ToInt32(splitInputs[j]);
                            switch (j)
                            {
                                case 0:
                                    rectangle.Width = val;
                                    break;
                                case 1:
                                    rectangle.Height = val;
                                    break;
                                case 2:
                                    rectangle.Location_X = val;
                                    break;
                                case 3:
                                    rectangle.Location_Y = val;
                                    rectangle.BorderColor = ConsoleColor.White;
                                    break;
                                case 4:
                                    rectangle.Angle = val;
                                    break;
                                default: Console.WriteLine("Something went wrong.");
                                    goto Input2;
                            }
                        }
                    }
                    catch(IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Array Index out of bound exception.\nKindly, try again.");
                        goto Input2;
                    }
                    lstRectangle.Add(rectangle);
                }
                //To clear the screen before drawing the rectangles.
                Console.Clear();
                string intersectedRect = string.Empty;
                for (int i = 0; i < noOfRectI; i++)
                {
                    for (int j = 0; j < noOfRectI; j++)
                    {
                        if (i == j)
                            continue;
                        else
                            try
                            {
                                if (rectFunctionalities.CallIntersectionMethod(lstRectangle[i], lstRectangle[j]))
                                {
                                    lstRectangle[i].BorderColor = lstRectangle[j].BorderColor = ConsoleColor.Red;
                                    if (!intersectedRect.Contains((i + 1).ToString()))
                                    {
                                        intersectedRect = intersectedRect + (i + 1) + " ";
                                    }
                                    if (!intersectedRect.Contains((j + 1).ToString()))
                                        intersectedRect = intersectedRect + (j + 1) + " ";
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine("Array Index out of bound exception.\nKindly, try again.");
                                goto Input1;
                            }
                    }
                    rectFunctionalities.Darw(lstRectangle[i]);
                }
                //When rectangles drawn, then string output must not overlap rectangles.
                //So, printing the output after MaxPostion of Y co-ordinate.
                if (!string.IsNullOrEmpty(intersectedRect))
                    rectFunctionalities.WriteAt( "\nRectangle numbers " + intersectedRect + " are intersecting.\n\n");
                else
                    rectFunctionalities.WriteAt("\nNo intersection found.\n\n");
            }
        }        
    }

    class RectangleFunctionalities
    {
        #region non-rotated rectangle
        double maxLocY = 0;
        //To write at perticular (X,Y) co-ordinate.
        public void WriteAt(string s, double x, double y)
        {
            try
            {
                Console.SetCursorPosition(Convert.ToInt32(x), Convert.ToInt32(y));
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
       
        public void WriteAt(string s)
        {
            try
            {
                Console.SetCursorPosition(0,(int)maxLocY);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        //To find wheather two rectangles are intersecting each other or not.
        public bool IsIntersecting(Rectangle rectangle_1, Rectangle rectangle_2)
        {
            if ((rectangle_2.Location_X >= rectangle_1.Location_X && rectangle_2.Location_X <= rectangle_1.Location_X + rectangle_1.Width) && (rectangle_2.Location_Y >= rectangle_1.Location_Y && rectangle_2.Location_Y <= rectangle_1.Location_Y + rectangle_1.Height) || (rectangle_2.Location_X + rectangle_2.Width >= rectangle_1.Location_X && rectangle_2.Location_X + rectangle_2.Width <= rectangle_1.Location_X + rectangle_1.Width) && (rectangle_2.Location_Y + rectangle_2.Height >= rectangle_1.Location_Y && rectangle_2.Location_Y + rectangle_2.Height <= rectangle_1.Location_Y + rectangle_1.Height))
                return true;
            else
                return false;
        }

        //To Draw non-rotated rectangles only.
        public void Darw(Rectangle rectangle)
        {
            string hS = "═";
            string vS = "║";
            if (rectangle.BorderColor == ConsoleColor.Red)
                Console.ForegroundColor = ConsoleColor.Red;
            double locX = rectangle.Location_X;
            double locY = rectangle.Location_Y;
            if (locY + rectangle.Height > maxLocY)
                maxLocY = locY + rectangle.Height + 5;
            //In the case of (1,1) co-cordinate.
            if (rectangle.Height == 1 && rectangle.Width == 1)
            {
                WriteAt("[]", locX, locY);
                return;
            }
            try
            {
                // 4 loops to draw 4 sides of a rectangle.
                for (int i = 1; i <= rectangle.Height; i++)
                {
                    WriteAt(vS, (locX + rectangle.Width - 1), (locY + i));
                }
                for (int i = 0; i < rectangle.Width; i++)
                {
                    if (i == 0)
                        WriteAt("╔", (locX + i), locY);
                    else
                    if (i == rectangle.Width - 1)
                        WriteAt("╗", (locX + i), locY);
                    else
                        WriteAt(hS, (locX + i), locY);
                }
                for (int i = 1; i <= rectangle.Height; i++)
                {
                    WriteAt(vS, locX, (locY + i));
                }
                for (int i = 0; i < rectangle.Width; i++)
                {
                    if (i == 0)
                        WriteAt("╚", (locX + i), (locY + rectangle.Height));
                    else
                    if (i == rectangle.Width - 1)
                        WriteAt("╝", (locX + i), (locY + rectangle.Height));
                    else
                        WriteAt(hS, (locX + i), (locY + rectangle.Height));
                }
            }
            catch(ArithmeticException e)
            {
                Console.WriteLine("Arithmetic Exception thrown while drawing rectangle.");
            }
            Console.ResetColor();
        }

        //All of the validations regarding rectangle input set we can write here.
        public bool ValidateInputs(string[] inputs)
        {
            if (inputs.Length != 4)
            {
                Console.WriteLine("Number of inputs must be '4'.\nKindly, try again.");
                return false;
            }
            int num = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if(Int32.TryParse(inputs[i], out num))
                {
                    switch(i)
                    {
                        case 0:
                            if (num > Console.WindowHeight || num < 1)
                            {
                                Console.WriteLine("Height must be in the range of '1' and Screen height " + Console.WindowHeight + "");
                                return false;
                            }
                        break;
                        case 1:
                            if (num > Console.WindowWidth || num < 1)
                            {
                                Console.WriteLine("Width must be in the range of '1' and Screen width " + Console.WindowWidth +"");
                                return false;
                            }
                        break;
                        case 2:
                            if (num > Console.WindowWidth || num < 0)
                            {
                                Console.WriteLine("Location_X must be in the range of '0' and Screen width " + Console.WindowWidth + "");
                                return false;
                            }
                        break;
                        case 3:
                            if (num > Console.WindowHeight || num < 0)
                            {
                                Console.WriteLine("Location_Y must be in the range of '0' and Screen height " + Console.WindowHeight + "");
                                return false;
                            }
                        break;
                        default: Console.WriteLine("Input is not handled.");
                                  return false;
                    }
                }
                else
                {
                    Console.WriteLine(inputs[i] +" is not a a number.\nKindly, try again.");
                    return false;
                }
            }
            return true;
        }
        //To validate single input.
        public bool ValidateInputs(string input)
        {
            int num;
            if(string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please, enter the input.");
                return false;
            }
            else
            if(!Int32.TryParse(input, out num))
            {
                Console.WriteLine("Input must be an integer.");
                return false;
            }
            if (Convert.ToInt32(num) <1)
            {
                Console.WriteLine("Input must be greater than zero.");
                return false;
            }
            return true;
        }
        #endregion

        #region rotated-rectangle
        //To select intersection method with respect to shape.
        public bool CallIntersectionMethod(Rectangle rectangle_1, Rectangle rectangle_2)
        {
            if (rectangle_1.Angle != 0 || rectangle_2.Angle != 0)
                return IsIntersectingRotated(rectangle_1, rectangle_2);
            else
                return IsIntersecting(rectangle_1, rectangle_2);
        }
        //To calculate the rotated rectangle points with respect to angle.
        //The problem is sin cos values are in radian but values are not right.(Need to resume the implementation from GetNewCordinates method)
        public bool IsIntersectingRotated(Rectangle rectangle_1, Rectangle rectangle_2)
        {
            double r1_X1 = rectangle_1.Location_X;
            double r1_X2 = rectangle_1.Location_X + rectangle_1.Width;
            double r1_X3 = r1_X2;
            double r1_X4 = r1_X1;
            double r1_Y1 = rectangle_1.Location_Y;
            double r1_Y2 = r1_Y1;
            double r1_Y3 = rectangle_1.Location_Y + rectangle_1.Height;
            double r1_Y4 = r1_Y3;

            double r2_X1 = rectangle_2.Location_X;
            double r2_X2 = rectangle_2.Location_X + rectangle_2.Width;
            double r2_X3 = r2_X2;
            double r2_X4 = r2_X1;
            double r2_Y1 = rectangle_2.Location_Y;
            double r2_Y2 = r2_Y1;
            double r2_Y3 = rectangle_2.Location_Y + rectangle_2.Height;
            double r2_Y4 = r2_Y3;
            GetNewCordinates(ref r1_X1, ref r1_Y1, rectangle_1.Angle);
            GetNewCordinates(ref r1_X2, ref r1_Y2, rectangle_1.Angle);
            GetNewCordinates(ref r1_X3, ref r1_Y3, rectangle_1.Angle);
            GetNewCordinates(ref r1_X4, ref r1_Y4, rectangle_1.Angle);

            GetNewCordinates(ref r2_X1, ref r2_Y1, rectangle_2.Angle);
            GetNewCordinates(ref r2_X2, ref r2_Y2, rectangle_2.Angle);
            GetNewCordinates(ref r2_X3, ref r2_Y3, rectangle_2.Angle);
            GetNewCordinates(ref r2_X4, ref r2_Y4, rectangle_2.Angle);
            return false;
        }
        //Calculate the rotated point.
        //Wrong output.
        private void GetNewCordinates(ref double X, ref double Y, int angle)
        {
            try
            {
                double radianVal = angle * (3.14159265358979323846 / 180.0);
                X = X * Math.Cos(radianVal) - Y * Math.Sin(radianVal);
                Y = Y * Math.Cos(radianVal) + X * Math.Sin(radianVal);
            }
            catch(ArithmeticException e)
            {
                Console.WriteLine("ArithmeticException Exception, while trying to get the rotated co-ordinate.");
            }
        }
        #endregion

    }
    //Rectangle properties.
    public class Rectangle
    {
        private double width;
        private double height;
        private double location_X;
        private double location_Y;
        private ConsoleColor borderColor;
        private int angle;

        public double Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        public double Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public ConsoleColor BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}
