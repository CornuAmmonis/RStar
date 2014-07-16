//   SpatialIndex.cs
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
    public interface SpatialIndex
    {

        /// <summary>
        ///  Initializes any implementation dependent properties
        ///  of the spatial index. For example, RTree implementations
        ///  will have a NodeSize property.
        /// </summary>
        void Init();

        /// <summary>
        ///  Adds a new rectangle to the spatial index
        /// </summary>
        /// <param name="r">The rectangle to add to the spatial index.</param>
        /// <param name="id">
        ///  The ID of the rectangle to add to the spatial index.
        ///  The result of adding more than one rectangle with the same ID is undefined.
        /// </param>
        void Add(Rectangle r, int id);

        /// <summary>
        ///  Deletes a rectangle from the spatial index.
        /// </summary>
        /// <param name="r">The rectangle to delete from the spatial index</param>
        /// <param name="id">The ID of the rectangle to delete from the spatial index</param>
        /// <returns>
        ///  true  if the rectangle was deleted
        ///  false if the rectangle was not found, or the
        ///  rectangle was found but with a different ID
        /// </returns>
        bool Delete(Rectangle r, int id);

        /// <summary>
        ///  Finds the nearest rectangles to the passed rectangle and calls
        ///  v.execute(id) for each one.
        /// 
        ///  If multiple rectangles are equally near, they will
        ///  all be returned.
        /// </summary>
        /// <param name="p">The point for which this method finds the nearest neighbours</param>
        /// <param name="v">The Invokable method called for each nearest neighbour</param>
        /// <param name="furthestDistance">
        ///  The furthest distance away from the rectangle
        ///  to search. Rectangles further than this will not be found.
        ///  This should be as small as possible to minimise the search time.
        ///  Use float.PositiveInfinity to guarantee that the nearest rectangle is found,
        ///  no matter how far away, although this will slow down the algorithm.
        /// </param>
        void Nearest(Point p, Invokable<int> v, float furthestDistance);

        /// <summary>
        ///  Finds the N nearest rectangles to the passed rectangle, and calls
        ///  execute(id, distance) on each one, in order of increasing distance.
        /// 
        ///  Note that fewer than N rectangles may be found if fewer entries
        ///  exist within the specified furthest distance, or more if rectangles
        ///  N and N+1 have equal distances.
        /// </summary>
        /// <param name="p">The point for which this method finds the nearest neighbours</param>
        /// <param name="v">The Invokable method called for each nearest neighbour</param>
        /// <param name="n">The desired number of rectangles to find (but note that fewer or more may be returned)</param>
        /// <param name="distance">
        ///  The furthest distance away from the rectangle
        ///  to search. Rectangles further than this will not be found.
        ///  This should be as small as possible to minimise the search time.
        ///  Use float.PositiveInfinity to guarantee that the nearest rectangle is found,
        ///  no matter how far away, although this will slow down the algorithm.
        /// </param>
        void NearestN(Point p, Invokable<int> v, int n, float distance);

        /// <summary>
        ///  Same as nearestN, except the found rectangles are not returned
        ///  in sorted order. This will be faster, if sorting is not required
        /// </summary>
        void NearestNUnsorted(Point p, Invokable<int> v, int n, float distance);

        /// <summary>
        ///  Finds all rectangles that intersect the passed rectangle.
        /// </summary>
        /// <param name="r">The rectangle for which this method finds intersecting rectangles.</param>
        /// <param name="ip">The Invokable method called for each intersecting rectangle</param>
        void Intersects(Rectangle r, Invokable<int> ip);

        /// <summary>
        ///  Finds all rectangles contained by the passed rectangle.
        /// </summary>
        /// <param name="r">The rectangle for which this method finds contained rectangles.</param>
        /// <param name="ip">The Invokable method called for each contained rectangle</param>
        void Contains(Rectangle r, Invokable<int> ip);

        /// <summary>
        ///  Returns the number of entries in the spatial index.
        /// </summary>
        int Size();

        /// <summary>
        ///  Returns the bounds of all the entries in the spatial index,
        ///  or null if there are no entries.
        /// </summary>
        Rectangle GetBounds();

        /// <summary>
        ///  Returns a string identifying the type of
        ///  spatial index, and the version number,
        ///  eg "SimpleIndex-0.1"
        /// </summary>
        String GetVersion();
    }
}