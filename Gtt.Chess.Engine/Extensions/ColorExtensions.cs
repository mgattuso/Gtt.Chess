using System;
using System.Collections.Generic;
using System.Text;

namespace Gtt.Chess.Engine.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetOtherColor(this Color color)
        {
            switch (color)
            {
                case Color.White:
                    return Color.Black;
                case Color.Black:
                    return Color.White;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public static string ToReference(this Color color)
        {
            return color.ToString().Substring(0, 1);
        }
    }
}
