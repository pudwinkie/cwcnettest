/* GameTrainer 1.0
 Copyright (C) 2007 Luca Tagliaferri

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

 Copyright (C) 2007 Luca Tagliaferri
 e-mai luca.tagliaferri@gmail.com
*/


using System;
using System.Collections.Generic;
using System.Text;

namespace GameTrainer
{
    public class GameTrainerProcess
    {
        public GameTrainerProcess(uint pid,IntPtr handle , string text)
        {
            this.pid = pid;
            this.handle = handle;
            this.text = text;
        }

        private uint pid;
        public uint Pid
        {
            get { return this.pid; }
            set { this.pid = value; }
        }

        private IntPtr handle;
        public System.IntPtr Handle
        {
            get { return handle; }
            set { handle = value; }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return text;
        }
    }
}
