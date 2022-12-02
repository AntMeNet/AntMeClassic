using System;
using System.Collections.Generic;

using AntMe.SharedComponents.Properties;

namespace AntMe.SharedComponents.Tools {
    /// <summary>
    /// RGB colours.
    /// </summary>
    /// <remarks>
    /// Colours defined independent from Windows forms or managed DirectX
    /// using this struct colours can be created and mixed.
    /// </remarks>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public struct Color {
        private int blue;
        private int green;
        private int red;

        /// <summary>
        /// Constructor to instantiate a new RGB color.
        /// </summary>
        /// <param name="red">RGB value for red</param>
        /// <param name="green">RGB value for green</param>
        /// <param name="blue">RGB value for blue</param>
        public Color(int red, int green, int blue) {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        /// <summary>
        /// RGB red value minimum 0, maximum 255.
        /// </summary>
        public byte Red {
            get => red < 0 ? (byte) 0 : red > 255 ? (byte) 255 : (byte) red;
            set => red = value;
        }

        /// <summary>
        /// RGB green value minimum 0, maximum 255.
        /// </summary>
        public byte Green {
            get => green < 0 ? (byte) 0 : green > 255 ? (byte) 255 : (byte) green;
            set => green = value;
        }

        /// <summary>
        /// RGB blue value minimum 0, maximum 255.
        /// </summary>
        public byte Blue {
            get => blue < 0 ? (byte) 0 : blue > 255 ? (byte) 255 : (byte) blue;
            set => blue = value;
        }

        /// <summary>
        /// RGB color to string.
        /// </summary>
        /// <returns>(Red,Green,Blue)</returns>
        public override string ToString() {
            return "(" + Red + "," + Green + "," + Blue + ")";
        }

        /// <summary>
        /// Adding up two RGB colours.
        /// </summary><remarks>
        /// Implemented according to the original remark
        /// to mix two RGB colours the result must be divided by two
        /// (colour1 + colour2) / 2
        /// </remarks>
        /// <param name="c1">Color 1</param>
        /// <param name="c2">Color 2</param>
        /// <returns>Color</returns>
        public static Color operator +(Color c1, Color c2) {
            // Division by two for each RGB value.
            return new Color((c1.red + c2.red)/2, (c1.green + c2.green)/2, (c1.blue + c2.blue)/2);
        }

        /// <summary>
        /// RGB color multiplied by an integer value
        /// all RGB values have a maximum of 255.
        /// </summary>
        /// <param name="c">Color</param>
        /// <param name="i">Integer value</param>
        /// <returns>Color</returns>
        public static Color operator *(Color c, int i) {
            return new Color(c.red*i, c.green*i, c.blue*i);
        }

        /// <summary>
        /// New RGB color by dividing all given RGB values by a given integer value.
        /// </summary>
        /// <param name="c">Color</param>
        /// <param name="i">Integer value</param>
        /// <returns>Color</returns>
        public static Color operator /(Color c, int i)
        {
            if (i > 0)
                return new Color(c.red / i, c.green / i, c.blue / i);
            else 
                return c;
        }

        /// <summary>
        /// Distance between two colors in the range 0 - 195075.
        /// </summary><remarks>
        /// Used by the ColorFinder class
        /// maximum = (maximum deltaColorX) * (maximum deltaColorX) * number of colors
        /// 255 * 255 * 3 = 195075
        /// </remarks>
        /// <param name="c1">Color 1</param>
        /// <param name="c2">Color 2</param>
        /// <returns>Color distance as integer value.</returns>
        public static int operator -(Color c1, Color c2) {
            int deltaRed = c1.red - c2.red;
            int deltaGreen = c1.green - c2.green;
            int deltaBlue = c1.blue - c2.blue;
            return deltaRed*deltaRed + deltaGreen*deltaGreen + deltaBlue*deltaBlue;
            //return Math.Abs(deltaRed) + Math.Abs(deltaGreen) + Math.Abs(deltaBlue);
        }
    }

    /// <summary>
    /// Find Color with maximum difference.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public class ColorFinder {
        public static System.Drawing.Color ColonyColor1 => Settings.Default.ColonyColor1;

        public static System.Drawing.Color ColonyColor2 => Settings.Default.ColonyColor2;

        public static System.Drawing.Color ColonyColor3 => Settings.Default.ColonyColor3;

        public static System.Drawing.Color ColonyColor4 => Settings.Default.ColonyColor4;

        public static System.Drawing.Color ColonyColor5 => Settings.Default.ColonyColor5;

        public static System.Drawing.Color ColonyColor6 => Settings.Default.ColonyColor6;

        public static System.Drawing.Color ColonyColor7 => Settings.Default.ColonyColor7;

        public static System.Drawing.Color ColonyColor8 => Settings.Default.ColonyColor8;

        private readonly List<Color> colorsList = new List<Color>();

        /// <summary>
        /// Marks new Color as already existing.
        /// </summary>
        /// <param name="color">New Color.</param>
        public void AddColorToColorList(Color color) {
            colorsList.Add(color);
        }

        /// <summary>
        /// Remove Color from ColorList.
        /// </summary>
        /// <param name="color">Existing Color.</param>
        public void RemoveColorFromColorList(Color color) {
            colorsList.Remove(color);
        }

        private int DetermineMinimumDistance(Color color) {
            int minimumDistance = int.MaxValue;
            for (int f = 0; f < colorsList.Count; f++) {
                int distance = colorsList[f] - color;
                if (distance < minimumDistance) {
                    minimumDistance = distance;
                }
            }
            return minimumDistance;
        }

        /// <summary>
        /// Create Color with maximum distance.
        /// </summary>
        /// <returns>New Color.</returns>
        public Color CreateNewColor() {
            int bestDistance = 0;
            Color bestColor = new Color(0, 0, 0);

            int r, g, b;
            int r0 = 0, g0 = 0, b0 = 0;
            int distance;
            Color color;

            for (r = 8; r < 256; r += 16) {
                for (g = 8; g < 256; g += 16) {
                    for (b = 8; b < 256; b += 16) {
                        color = new Color(r, g, b);
                        distance = DetermineMinimumDistance(color);
                        if (distance > bestDistance) {
                            bestDistance = distance;
                            r0 = r;
                            g0 = g;
                            b0 = b;
                        }
                    }
                }
            }

            for (r = -8; r < 8; r++) {
                for (g = -8; g < 8; g++) {
                    for (b = -8; b < 8; b++) {
                        color = new Color(r0 + r, g0 + g, b0 + b);
                        distance = DetermineMinimumDistance(color);
                        if (distance > bestDistance) {
                            bestDistance = distance;
                            bestColor = color;
                        }
                    }
                }
            }

            return bestColor;
        }

        /// <summary>
        /// Create new Color with maximum distance and randomize the result.
        /// </summary>
        /// <param name="scatter">Scatter value for the random number generator</param>
        /// <returns>new Color.</returns>
        public Color CreateNewColor(int scatter) {
            Color color = CreateNewColor();
            Random random = new Random();
            return
                new Color
                    (
                    (color.Red*100)/(100 + random.Next(-scatter, scatter)),
                    (color.Green*100)/(100 + random.Next(-scatter, scatter)),
                    (color.Blue*100)/(100 + random.Next(-scatter, scatter)));
        }

        /// <summary>
        /// Create Color and add it to the colorsList.
        /// </summary>
        /// <returns>New Color.</returns>
        public Color CreateColorAndAddToColorsList() {
            Color color = CreateNewColor();
            AddColorToColorList(color);
            return color;
        }

        /// <summary>
        /// Create Color, randomize it and add it to the colorsList.
        /// </summary>
        /// <param name="scatter">Scatter value for the random number generator</param>
        /// <returns>New Color.</returns>
        public Color CreateColorAndAddToColorsList(int scatter) {
            Color color = CreateNewColor(scatter);
            AddColorToColorList(color);
            return color;
        }
    }
}