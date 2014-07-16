//   PerformanceTest.cs
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
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RSI
{
    [TestClass]
    public class PerformanceTest
    {
        private Logger log = new Logger();
        private SpatialIndex si;

        private static float RandomFloat(Random r, double min, double max)
        {
            return (float)(r.NextDouble() * (max - min) + min);
        }

        protected static Point RandomPoint(Random r)
        {
            return new Point(RandomFloat(r, 0, 100), RandomFloat(r, 0, 100));
        }

        private static Rectangle RandomRectangle(Random r, float size)
        {
            float x = RandomFloat(r, 0, 100);
            float y = RandomFloat(r, 0, 100);
            return new Rectangle(x, y, x + RandomFloat(r, 0, size), y + RandomFloat(r, 0, size));
        }

        private void Benchmark(Operation o, int repetitions)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Random r = new Random();
            for (int j = 0; j < repetitions; j++) 
            {
                o.Execute();
            };
            stopwatch.Stop();

            log.Info(o.GetDescription() + ", " +
                    "avg callbacks = " + ((float) o.CallbackCount() / repetitions) + ", " +
                    "avg time = " + (long) (1000000f * (stopwatch.Elapsed.TotalMilliseconds / (double) repetitions)) + " ns ");
        }

        [TestMethod]
        public void Benchmark_1() {
            log.Debug("Beginning Benchmark 1");


            Random rand  = new Random(0);
            si = new RTree();
            si.Init();

            int rectangleCount = 10000;
            int benchCount = 1000;
            Rectangle[] rects = new Rectangle[rectangleCount];
            for (int i = 0; i < rectangleCount; i++) {
                rects[i] = RandomRectangle(rand, 0.01f);
            }

            for (int j = 0; j < 5; j++) 
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < rectangleCount; i++) 
                {
                    si.Add(rects[i], i);
                }
                stopwatch.Stop();
                log.Info("add " + rectangleCount + " avg tme = " + (long) (1000000d * (stopwatch.Elapsed.TotalMilliseconds / (double) rectangleCount)) + " ns");

                if (j == 4) break; // don't do the delete on the last iteration

                stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < rectangleCount; i++) 
                {
                    si.Delete(rects[i], i);
                }
                stopwatch.Stop();
                log.Info("delete " + rectangleCount + " avg tme = " + (long) (1000000d * (stopwatch.Elapsed.TotalMilliseconds / (double) rectangleCount)) + " ns");
            }

            Func<Counter, bool> nearestFunc = delegate(Counter c)
            {
                si.Nearest(RandomPoint(rand), c, 0.1f);
                return true;
            };

            Func<Counter, bool> nearestNUnsorted = delegate(Counter c)
            {
                si.NearestNUnsorted(RandomPoint(rand), c, 10, 0.16f);
                return true;
            };

            Func<Counter, bool> nearestN = delegate(Counter c)
            {
                si.NearestN(RandomPoint(rand), c, 10, 0.16f);
                return true;
            };

            Func<Counter, bool> intersects = delegate(Counter c)
            {
                si.Intersects(RandomRectangle(rand, 0.6f), c);
                return true;
            };

            Func<Counter, bool> contains = delegate(Counter c)
            {
                si.Contains(RandomRectangle(rand, 0.6f), c);
                return true;
            };

            Operation o0 = new Operation("nearest", si, nearestFunc);
            Operation o1 = new Operation("nearestNUnsorted", si, nearestNUnsorted);
            Operation o2 = new Operation("nearestN", si, nearestN);
            Operation o3 = new Operation("intersects", si, intersects);
            Operation o4 = new Operation("contains", si, contains);

            Benchmark(o0, benchCount);
            Benchmark(o1, benchCount);
            Benchmark(o2, benchCount);
            Benchmark(o3, benchCount);
            Benchmark(o4, benchCount);


        }

    }

    class Operation
    {
        protected static int count = 0;
        protected String description;
        protected SpatialIndex si;
        protected Counter c;
        protected Func<Counter, bool> f;

        public Operation(String description, SpatialIndex si, Func<Counter, bool> f)
        {
            this.description = description;
            this.c = new Counter(GetCounter());
            this.si = si;
            this.f = f;
        }

        public static Func<int, bool> GetCounter() {
            Func<int, bool> counter = delegate(int i)
            {
                count++;
                return true;
            };
            return counter;
        }

        public int CallbackCount()
        {
            return count;
        }

        public String GetDescription()
        {
            return description;
        }

        public void Execute()
        {
            f.Invoke(c);
        }

    }

    class Counter : Invokable<int>
    {
        Func<int, bool> closure;

        public Counter(Func<int, bool> closure)
        {
            this.closure = closure;
        }

        public bool Invoke(int value)
        {
            return closure(0);
        }
    }

}
