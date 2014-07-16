//   SimpleIndex.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSI
{
    /// <summary>
    ///  SimpleIndex
    ///  
    ///  A very simple (and slow!) spatial index implementation,
    ///  intended only for generating test results.
    ///  
    ///  All of the search methods, ie nearest(), contains() and intersects(),
    ///  run in linear time, so performance will be very slow with more
    ///  than 1000 or so entries.
    ///  
    ///  On the other hand, the add() and delete() methods are very fast :-)
    /// </summary>
    class SimpleIndex : SpatialIndex
    {
        Dictionary<int, Rectangle> m_map = new Dictionary<int, Rectangle>();


        // Does nothing. There are no implementation dependent properties for 
        // the SimpleIndex spatial index.
        public void Init()
        {
            return;
        }


        // Nearest
        private List<int> Nearest(Point p, float furthestDistance)
        {
            List<int> ret = new List<int>();
            float nearestDistance = furthestDistance;
            foreach (KeyValuePair<int, Rectangle> i in m_map)
            {
                int currentId = i.Key;
                Rectangle currentRectangle = i.Value;
                float distance = currentRectangle.Distance(p);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    ret.Clear();
                }
                if (distance <= nearestDistance)
                {
                    ret.Add(currentId);
                }
            }
            return ret;
        }

        public void Nearest(Point p, Invokable<int> v, float furthestDistance)
        {
            List<int> nearestList = Nearest(p, furthestDistance);

            nearestList.ForEach((int i) => v.Invoke(i));
        }

        private List<int> NearestN(Point p, int n, float furthestDistance)
        {
            List<int> ids = new List<int>();
            List<float> distances = new List<float>();

            foreach (KeyValuePair<int, Rectangle> i in m_map)
            {
                int currentId = i.Key;
                Rectangle currentRectangle = i.Value;
                float distance = currentRectangle.Distance(p);

                if (distance <= furthestDistance)
                {
                    int insertionIndex = 0;
                    while (ids.Count > insertionIndex && distances[insertionIndex] <= distance)
                    {
                        insertionIndex++;
                    }
                    ids.Insert(insertionIndex, currentId);
                    distances.Insert(insertionIndex, distance);

                    // remove the entries with the greatest distance, if necessary.
                    if (ids.Count > n)
                    {
                        // check that removing all entries with equal greatest distance
                        // would leave at least N entries.
                        int maxDistanceCount = 1;
                        int currentIndex = distances.Count - 1;
                        float maxDistance = distances[currentIndex];
                        while (currentIndex - 1 >= 0 && distances[currentIndex - 1] == maxDistance)
                        {
                            currentIndex--;
                            maxDistanceCount++;
                        }
                        if (ids.Count - maxDistanceCount >= n)
                        {
                            ids.RemoveRange(currentIndex, maxDistanceCount);
                            distances.RemoveRange(currentIndex, maxDistanceCount);
                        }
                    }
                }
            }

            return ids;
        }

        public void NearestN(Point p, Invokable<int> v, int n, float furthestDistance)
        {
            List<int> nearestList = NearestN(p, n, furthestDistance);

            // TODO: Change Invokable to Action?
            nearestList.ForEach((int i) => v.Invoke(i));
        }


        // Same as nearestN
        public void NearestNUnsorted(Point p, Invokable<int> v, int n, float furthestDistance)
        {
            NearestN(p, v, n, furthestDistance);
        }

        public void Intersects(Rectangle r, Invokable<int> v)
        {
            foreach (KeyValuePair<int, Rectangle> i in m_map)
            {
                int currentId = i.Key;
                Rectangle currentRectangle = i.Value;
                if (r.Intersects(currentRectangle))
                {
                    v.Invoke(currentId);
                }
            }
        }

        public void Contains(Rectangle r, Invokable<int> v)
        {
            foreach (KeyValuePair<int, Rectangle> i in m_map)
            {
                int currentId = i.Key;
                Rectangle currentRectangle = i.Value;
                if (r.Contains(currentRectangle))
                {
                    v.Invoke(currentId);
                }
            }
        }

        public void Add(Rectangle r, int id)
        {
            m_map.Add(id, r.Copy());
        }

        public bool Delete(Rectangle r, int id)
        {
            Rectangle value = m_map[id];

            if (r.Equals(value))
            {
                m_map.Remove(id);
                return true;
            }
            return false;
        }

        public int Size()
        {
            return m_map.Count;
        }

        public Rectangle GetBounds()
        {
            Rectangle bounds = null;
            foreach (KeyValuePair<int, Rectangle> i in m_map)
            {
                Rectangle currentRectangle = i.Value;
                if (bounds == null)
                {
                    bounds = currentRectangle.Copy();
                }
                else
                {
                    bounds.Add(currentRectangle);
                }
            }
            return bounds;
        }

        public String GetVersion()
        {
            // TODO: BuildProperties.getVersion();
            return "SimpleIndex-" + "0.0";
        }
    }
}
