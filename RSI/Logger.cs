//   Logger.cs
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
    class Logger
    {

        private bool isDebugEnabled;

        public Logger()
        {
            isDebugEnabled = IsDebugEnabled();
        }

        public void Debug(String log) {
            if (isDebugEnabled)
            {
               System.Diagnostics.Debug.WriteLine(log);
            }
        }

        public void Warn(String log)
        {
            System.Diagnostics.Debug.WriteLine(log);
        }

        public void Info(String log)
        {
            System.Diagnostics.Debug.WriteLine(log);
        }

        public void Error(String log)
        {
            System.Diagnostics.Debug.WriteLine(log);
        }

        public bool IsDebugEnabled()
        {
            return isDebugEnabled;
        }
    }
}
