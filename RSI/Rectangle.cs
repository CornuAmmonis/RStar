//   Rectangle.cs
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
    public class Rectangle
    {
        // use primitives instead of arrays for the coordinates of the rectangle,
        // to reduce memory requirements.
        internal float minX, minY, maxX, maxY;

        public Rectangle()
        {
            minX = float.MaxValue;
            minY = float.MaxValue;
            maxX = -float.MaxValue;
            maxY = -float.MaxValue;
        }


        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="x1">Coordinate of any corner of the rectangle</param>
        /// <param name="y1">(see x1)</param>
        /// <param name="x2">Coordinate of the opposite corner</param>
        /// <param name="y2">(see x2)</param>
        public Rectangle(float x1, float y1, float x2, float y2)
        {
            Set(x1, y1, x2, y2);
        }

        /// <summary>
        ///  Sets the size of the rectangle.
        /// </summary>
        /// <param name="x1">Coordinate of any corner of the rectangle</param>
        /// <param name="y1">(see x1)</param>
        /// <param name="x2">Coordinate of the opposite corner</param>
        /// <param name="y2">(see x2)</param>
        public void Set(float x1, float y1, float x2, float y2)
        {
            minX = Math.Min(x1, x2);
            maxX = Math.Max(x1, x2);
            minY = Math.Min(y1, y2);
            maxY = Math.Max(y1, y2);
        }

        /// <summary>
        ///  Sets the size of this rectangle to equal the passed rectangle.
        /// </summary>
        public void Set(Rectangle r)
        {
            minX = r.minX;
            minY = r.minY;
            maxX = r.maxX;
            maxY = r.maxY;
        }

        /// <summary>
        ///  Make a copy of this rectangle.
        /// </summary>
        /// <returns>
        ///  Copy of this rectangle.
        /// </returns>
        public Rectangle Copy()
        {
            return new Rectangle(minX, minY, maxX, maxY);
        }

        /// <summary>
        ///  Determine whether an edge of this rectangle overlies the equivalent
        ///  edge of the passed rectangle
        /// </summary>
        public bool EdgeOverlaps(Rectangle r)
        {
            return minX == r.minX || maxX == r.maxX || minY == r.minY || maxY == r.maxY;
        }

        /// <summary>
        ///  Determine whether this rectangle intersects the passed rectangle.
        /// </summary>
        /// <param name="r">The rectangle that might intersect this rectangle</param>
        /// <returns>true if the rectangles intersect, false if they do not intersect</returns>
        public bool Intersects(Rectangle r)
        {
            return maxX >= r.minX && minX <= r.maxX && maxY >= r.minY && minY <= r.maxY;
        }

        /// <summary>
        ///  Determine whether or not two rectangles intersect.
        /// </summary>
        /// <param name="r1MinX">Minimum X coordinate of rectangle 1</param>
        /// <param name="r1MinY">Minimum Y coordinate of rectangle 1</param>
        /// <param name="r1MaxX">Maximum X coordinate of rectangle 1</param>
        /// <param name="r1MaxY">Maximum Y coordinate of rectangle 1</param>
        /// <param name="r2MinX">Minimum X coordinate of rectangle 2</param>
        /// <param name="r2MinY">Minimum Y coordinate of rectangle 2</param>
        /// <param name="r2MaxX">Maximum X coordinate of rectangle 2</param>
        /// <param name="r2MaxY">Maximum Y coordinate of rectangle 2</param>
        /// <returns> true if r1 intersects r2, false otherwise.<returns>
        static public bool Intersects(float r1MinX, float r1MinY, float r1MaxX, float r1MaxY, float r2MinX, float r2MinY, float r2MaxX, float r2MaxY)
        {
            return r1MaxX >= r2MinX && r1MinX <= r2MaxX && r1MaxY >= r2MinY && r1MinY <= r2MaxY;
        }

        /// <summary>
        ///  Determine whether this rectangle contains the passed rectangle.
        /// </summary>
        /// <param name="r">The rectangle that might be contained by this rectangle</param>
        /// <returns>true if this rectangle contains the passed rectangle, false if it does not</returns>
        public bool Contains(Rectangle r)
        {
            return maxX >= r.maxX && minX <= r.minX && maxY >= r.maxY && minY <= r.minY;
        }

        /// <summary>
        ///  Determine whether or not one rectangle contains another.
        /// </summary>
        /// <param name="r1MinX">Minimum X coordinate of rectangle 1</param>
        /// <param name="r1MinY">Minimum Y coordinate of rectangle 1</param>
        /// <param name="r1MaxX">Maximum X coordinate of rectangle 1</param>
        /// <param name="r1MaxY">Maximum Y coordinate of rectangle 1</param>
        /// <param name="r2MinX">Minimum X coordinate of rectangle 2</param>
        /// <param name="r2MinY">Minimum Y coordinate of rectangle 2</param>
        /// <param name="r2MaxX">Maximum X coordinate of rectangle 2</param>
        /// <param name="r2MaxY">Maximum Y coordinate of rectangle 2</param>
        /// <returns> true if r1 intersects r2, false otherwise.<returns>
        public static bool Contains(float r1MinX, float r1MinY, float r1MaxX, float r1MaxY, float r2MinX, float r2MinY, float r2MaxX, float r2MaxY)
        {
            return r1MaxX >= r2MaxX && r1MinX <= r2MinX && r1MaxY >= r2MaxY && r1MinY <= r2MinY;
        }

        /// <summary>
        ///  Determine whether this rectangle is contained by the passed rectangle.
        /// </summary>
        /// <param name="r">The rectangle that might contain this rectangle</param>
        /// <returns>true if the passed rectangle contains this rectangle, false if it does not</returns>
        public bool ContainedBy(Rectangle r)
        {
            return r.maxX >= maxX && r.minX <= minX && r.maxY >= maxY && r.minY <= minY;
        }

        /// <summary>
        ///  Return the distance between this rectangle and the passed point.
        ///  If the rectangle contains the point, the distance is zero.
        /// </summary>
        /// <param name="p">Point to find the distance to</param>
        /// <returns>distance beween this rectangle and the passed point</returns>
        public float Distance(Point p)
        {
            float distanceSquared = 0;

            float temp = minX - p.x;
            if (temp < 0)
            {
                temp = p.x - maxX;
            }

            if (temp > 0)
            {
                distanceSquared += (temp * temp);
            }

            temp = minY - p.y;
            if (temp < 0)
            {
                temp = p.y - maxY;
            }

            if (temp > 0)
            {
                distanceSquared += (temp * temp);
            }

            return (float)Math.Sqrt(distanceSquared);
        }

        /// <summary>
        ///  Return the distance between a rectangle and a point.
        ///  If the rectangle contains the point, the distance is zero.
        /// </summary>
        /// <param name="minX">minimum X coordinate of rectangle</param>
        /// <param name="minY">minimum Y coordinate of rectangle</param>
        /// <param name="maxX">maximum X coordinate of rectangle</param>
        /// <param name="maxY">maximum Y coordinate of rectangle</param>
        /// <param name="pX">X coordinate of point</param>
        /// <param name="pY">Y coordinate of point</param>
        /// <returns>distance beween this rectangle and the passed point.</returns>
        static public float Distance(float minX, float minY, float maxX, float maxY, float pX, float pY)
        {
            return (float)Math.Sqrt(DistanceSq(minX, minY, maxX, maxY, pX, pY));
        }

        static public float DistanceSq(float minX, float minY, float maxX, float maxY, float pX, float pY)
        {
            float distanceSqX = 0;
            float distanceSqY = 0;

            if (minX > pX)
            {
                distanceSqX = minX - pX;
                distanceSqX *= distanceSqX;
            }
            else if (pX > maxX)
            {
                distanceSqX = pX - maxX;
                distanceSqX *= distanceSqX;
            }

            if (minY > pY)
            {
                distanceSqY = minY - pY;
                distanceSqY *= distanceSqY;
            }
            else if (pY > maxY)
            {
                distanceSqY = pY - maxY;
                distanceSqY *= distanceSqY;
            }

            return distanceSqX + distanceSqY;
        }

        /// <summary>
        ///  Return the distance between this rectangle and the passed rectangle.
        ///  If the rectangles overlap, the distance is zero.
        /// </summary>
        /// <param name="r">Rectangle to find the distance to</param>
        /// <returns>distance between this rectangle and the passed rectangle</returns>
        public float Distance(Rectangle r)
        {
            float distanceSquared = 0;
            float greatestMin = Math.Max(minX, r.minX);
            float leastMax = Math.Min(maxX, r.maxX);
            if (greatestMin > leastMax)
            {
                distanceSquared += ((greatestMin - leastMax) * (greatestMin - leastMax));
            }
            greatestMin = Math.Max(minY, r.minY);
            leastMax = Math.Min(maxY, r.maxY);
            if (greatestMin > leastMax)
            {
                distanceSquared += ((greatestMin - leastMax) * (greatestMin - leastMax));
            }
            return (float)Math.Sqrt(distanceSquared);
        }

        /// <summary>
        ///  Calculate the area by which this rectangle would be enlarged if
        ///  added to the passed rectangle. Neither rectangle is altered.
        /// </summary>
        /// <param name="r">
        ///  Rectangle to union with this rectangle, in order to compute the difference in area of the union and the original rectangle
        /// </param>
        /// <returns>enlargement</returns>
        public float Enlargement(Rectangle r)
        {
            float enlargedArea = (Math.Max(maxX, r.maxX) - Math.Min(minX, r.minX)) *
                (Math.Max(maxY, r.maxY) - Math.Min(minY, r.minY));

            return enlargedArea - Area();
        }

        /// <summary>
        ///  Calculate the area by which a rectangle would be enlarged if added to the passed rectangle.
        /// </summary>
        /// <param name="r1MinX">Minimum X coordinate of rectangle 1</param>
        /// <param name="r1MinY">Minimum Y coordinate of rectangle 1</param>
        /// <param name="r1MaxX">Maximum X coordinate of rectangle 1</param>
        /// <param name="r1MaxY">Maximum Y coordinate of rectangle 1</param>
        /// <param name="r2MinX">Minimum X coordinate of rectangle 2</param>
        /// <param name="r2MinY">Minimum Y coordinate of rectangle 2</param>
        /// <param name="r2MaxX">Maximum X coordinate of rectangle 2</param>
        /// <param name="r2MaxY">Maximum Y coordinate of rectangle 2</param>
        /// <returns>enlargement<returns>
        public static float Enlargement(float r1MinX, float r1MinY, float r1MaxX, float r1MaxY, float r2MinX, float r2MinY, float r2MaxX, float r2MaxY)
        {
            float r1Area = (r1MaxX - r1MinX) * (r1MaxY - r1MinY);

            if (r1Area == float.PositiveInfinity)
            {
                return 0; // cannot enlarge an infinite rectangle...
            }

            if (r2MinX < r1MinX) r1MinX = r2MinX;
            if (r2MinY < r1MinY) r1MinY = r2MinY;
            if (r2MaxX > r1MaxX) r1MaxX = r2MaxX;
            if (r2MaxY > r1MaxY) r1MaxY = r2MaxY;

            float r1r2UnionArea = (r1MaxX - r1MinX) * (r1MaxY - r1MinY);

            if (r1r2UnionArea == float.PositiveInfinity)
            {
                // if a finite rectangle is enlarged and becomes infinite,
                // then the enlargement must be infinite.
                return float.PositiveInfinity;
            }
            return r1r2UnionArea - r1Area;
        }

        /// <summary>
        ///  Compute the area of this rectangle.
        /// </summary>
        /// <returns>The area of this rectangle</returns>
        public float Area()
        {
            return (maxX - minX) * (maxY - minY);
        }

        /// <summary>
        ///  Calculate the area by which a rectangle would be enlarged if added to the passed rectangle.
        /// </summary>
        /// <param name="minX">Minimum X coordinate of the rectangle</param>
        /// <param name="minY">Minimum Y coordinate of the rectangle</param>
        /// <param name="maxX">Maximum X coordinate of the rectangle</param>
        /// <param name="maxY">Maximum Y coordinate of the rectangle</param>
        /// <returns>The area of the rectangle<returns>
        public static float Area(float minX, float minY, float maxX, float maxY)
        {
            return (maxX - minX) * (maxY - minY);
        }

        /// <summary>
        ///  Computes the union of this rectangle and the passed rectangle, storing
        ///  the result in this rectangle.
        /// </summary>
        /// <param name="r">Rectangle to add to this rectangle</param>
        public void Add(Rectangle r)
        {
            if (r.minX < minX) minX = r.minX;
            if (r.maxX > maxX) maxX = r.maxX;
            if (r.minY < minY) minY = r.minY;
            if (r.maxY > maxY) maxY = r.maxY;
        }

        /// <summary>
        ///  Computes the union of this rectangle and the passed point, storing
        ///  the result in this rectangle.
        /// </summary>
        /// <param name="p">Point to add to this rectangle</param>
        public void Add(Point p)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        /// <summary>
        ///  Find the the union of this rectangle and the passed rectangle.
        ///  Neither rectangle is altered
        /// </summary>
        /// <param name="r">The rectangle to union with this rectangle</param>
        public Rectangle Union(Rectangle r)
        {
            Rectangle union = this.Copy();
            union.Add(r);
            return union;
        }

        // May be more performant to use BitConverter.ToInt32
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + this.maxX.GetHashCode();
            result = prime * result + this.maxY.GetHashCode();
            result = prime * result + this.minX.GetHashCode();
            result = prime * result + this.minY.GetHashCode();
            return result;
        }

        /// <summary>
        ///  Determine whether this rectangle is equal to a given object.
        ///  Equality is determined by the bounds of the rectangle.
        /// </summary>
        /// <param name="o">The object to compare with this rectangle</param>
        public override bool Equals(Object o)
        {
            Rectangle r = o as Rectangle;

            if ((Object)r != null)
            {
                return Equals(r);
            }

            return false;
        }

        public bool Equals(Rectangle r)
        {
            if ((Object)r == null)
            {
                return false;
            }

            return minX == r.minX && minY == r.minY && maxX == r.maxX && maxY == r.maxY;
        }

        /// <summary>
        ///  Determine whether this rectangle is the same as another object
        /// 
        ///  Note that two rectangles can be equal but not the same object,
        ///  if they both have the same bounds.
        /// </summary>
        /// <param name="o">The object to compare with this rectangle</param>
        public bool SameObject(Object o)
        {
            return Object.ReferenceEquals(this, o);
        }

        /// <summary>
        ///  Return a string representation of this rectangle, in the form:
        ///  (1.2, 3.4), (5.6, 7.8)
        /// </summary>
        /// <returns>String representation of this rectangle</returns>
        public override String ToString()
        {
            return "(" + minX + ", " + minY + "), (" + maxX + ", " + maxY + ")";
        }


        // Utility methods (not used by JSI); added to
        // enable this to be used as a generic rectangle class
        public float Width()
        {
            return maxX - minX;
        }

        public float Height()
        {
            return maxY - minY;
        }

        public float AspectRatio()
        {
            return Width() / Height();
        }

        public Point Centre()
        {
            return new Point((minX + maxX) / 2, (minY + maxY) / 2);
        }
    }
}

