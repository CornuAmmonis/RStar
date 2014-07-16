//   Point.cs
//   Java Spatial Index Library
//   Copyright (C) 2012 Aled Morris <aled@users.sourceforge.net>
//
//   C# Port Copyright (C) 2014 Cameron Turner <cjt36@cornell.edu>
//  
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//  
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//  Lesser General Public License for more details.
//  
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;

namespace RSI
{
	public class Point
	{
        /// <summary>
        ///  The (x, y) coordinates of the point.
        /// </summary>
		public float x, y;

        /// <summary>
        ///  Constructor. 
        /// </summary>
        /// <param name="x">The x coordinate of the point</param>
        /// <param name="y">The y coordinate of the point</param>
		public Point(float x, float y) {
			this.x = x;
			this.y = y;
		}

        /// <summary>
        ///  Copy from another point into this one
        /// </summary>
		public void set(Point other) {
			x = other.x;
			y = other.y;
		}

        /// <summary>
        ///  Print as a string in format "(x, y)"
        /// </summary>
		public override String ToString() {
			return "(" + x + ", " + y + ")";
		}

        /// <returns>X coordinate rounded to an int</returns>
		public int xInt() {
			return (int) Math.Round(x);
		}

        /// <returns>Y coordinate rounded to an int</returns>
		public int yInt() {
			return (int) Math.Round(y);
		}
	}
}

