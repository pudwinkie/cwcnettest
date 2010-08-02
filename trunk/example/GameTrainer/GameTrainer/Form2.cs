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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime;
using System.Runtime.InteropServices;

namespace GameTrainer
{
    unsafe public partial class Form2 : Form
    {
        [DllImport("kernel32.dll")]
        static extern uint VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, UInt32 dwLength);

        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, UInt32 size, ref IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, bool bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("Kernel32.dll")]
        static extern void GetSystemInfo(ref SYSTEM_INFO systemInfo);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);




        const UInt32 PROCESS_VM_READ = 0x0010;
        const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        const UInt32 SYNCHRONIZE = 0x00100000;
        const UInt32 PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;

        public Form2()
        {
            InitializeComponent();
        }

        private GameTrainerProcess process;
        private System.Collections.Generic.List<IntPtr> foundPositions = null;
        private int mbiCount = 0;
        private int timesRefined = 0;

        public GameTrainer.GameTrainerProcess Process
        {
            get { return process; }
            set { process = value; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Wait ...";
            button1.Enabled = false;
            
            //first allocate and search the first Time ;
            if (foundPositions == null)
            {
                foundPositions = new List<IntPtr>(10);
                PrepareMem();
            }
            else
            {
                RefineMem();
            }
            UpdateProgress(100, 100);
            if (this.foundPositions.Count == 1)
            {
                this.button1.Visible = false;
                this.textBox2.Text = this.foundPositions[0].ToString();
                this.NewVarGroupbox.Visible = true;
            }
            else if (this.foundPositions.Count <= 5 && timesRefined>=3)
            {
                this.button1.Visible = false;
                foreach (IntPtr fp in this.foundPositions)
                {
                    this.textBox2.Text += fp.ToString()+" ";
                }
                this.NewVarGroupbox.Visible = true;
            }

            button1.Text = "Refine Search ("+this.foundPositions.Count.ToString()+")";
            button1.Enabled = true;
        }

     
        unsafe private void RefineMem()
        {
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, this.process.Pid);
            
            List<IntPtr> newFoundPositions = new List<IntPtr>(10);

            uint lpRead;
            IntPtr lpReadPointer = new IntPtr(&(lpRead));
            byte[] buffer = new byte[4];

            for (int i = 0 ; i< this.foundPositions.Count ; i++)
            {
                IntPtr fp = this.foundPositions[i];
                fixed (byte* bufferPointer = &buffer[0])
                {
                    switch (Convert.ToInt32(this.comboBox1.Text))
                    {
                        case 1:
                            ReadProcessMemory(processHandle, fp, buffer, 1, ref lpReadPointer);
                            byte searchVal = Convert.ToByte(this.VarVal.Text);
                            if (*bufferPointer == searchVal)
                            {
                                newFoundPositions.Add(fp);
                            }
                            break;

                        case 2:
                            ReadProcessMemory(processHandle, (IntPtr)((int)(fp)  ), buffer,2, ref lpReadPointer);
                            UInt16 searchValInt16 = Convert.ToUInt16(this.VarVal.Text);
                            UInt16 pointedVal =*((UInt16*)(bufferPointer ));
                            if (pointedVal == searchValInt16)
                            {
                                newFoundPositions.Add(fp);
                            }
                            break;

                        case 4:
                            ReadProcessMemory(processHandle, (IntPtr)((int)(fp)), buffer, 4, ref lpReadPointer);
                            UInt32 searchValInt32 = Convert.ToUInt32(this.VarVal.Text);
                            UInt32 pointedVal32 = *((UInt32*)(bufferPointer));
                            if (pointedVal32 == searchValInt32)
                            {
                                newFoundPositions.Add(fp);
                            }
                            break;
                    }
                }
                UpdateProgress(i, foundPositions.Count);
            }
            CloseHandle(processHandle);
            if (this.foundPositions.Count == newFoundPositions.Count)
            {
                timesRefined++;
            }
            else
            {
                timesRefined = 0;
            }
            this.foundPositions = newFoundPositions;
        }
    

        unsafe private void PrepareMem()
        {
            uint start = 0;
            SYSTEM_INFO si = new SYSTEM_INFO();
            GetSystemInfo(ref si);
            MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();

            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, this.process.Pid);

            while (start < si.lpMaximumApplicationAddress)
            {
                uint sz = VirtualQueryEx(processHandle, (IntPtr)start, ref  mbi, Convert.ToUInt32(sizeof(MEMORY_BASIC_INFORMATION)));
                if ((mbi.State == (int)MemoryType.MEM_COMMIT)
                &&
                (mbi.Protect != (int)MemoryType.PAGE_READONLY)
                &&
                (mbi.Protect != (int)MemoryType.PAGE_EXECUTE_READ)
                &&
                (mbi.Protect != (int)MemoryType.PAGE_GUARD)
                &&
                (mbi.Protect != (int)MemoryType.PAGE_NOACCESS)
                )
                {
                    this.mbiCount++;
                    FindVarIn(mbi, processHandle);
                    UpdateProgress(start, si.lpMaximumApplicationAddress);
                }
                if (start + mbi.RegionSize < start)
                {
                    break;
                }
                start = (UInt32)mbi.BaseAddress + mbi.RegionSize;
            }

            CloseHandle(processHandle);
        }

        private void UpdateProgress(float start, float p)
        {
            this.Percentage.Text = Math.Round((start * 100) / p,2).ToString()+"%";
            this.Percentage.Refresh();
            Application.DoEvents();
        }

        unsafe private void FindVarIn(MEMORY_BASIC_INFORMATION mbi, IntPtr processHandle)
        {
            uint lpRead;
            IntPtr lpReadPointer = new IntPtr(&(lpRead));
            byte[] buffer = new byte[mbi.RegionSize];
            ReadProcessMemory(processHandle, mbi.BaseAddress, buffer, mbi.RegionSize, ref lpReadPointer);
            fixed (byte* bufferPointer = &buffer[0])
            {
                switch (Convert.ToInt32(this.comboBox1.Text))
                {
                    case 1:
                        byte searchVal = Convert.ToByte(this.VarVal.Text);
                        for (uint i = 0; i < mbi.RegionSize; i++)
                        {
                            if (bufferPointer[i] == searchVal)
                            {
                                this.foundPositions.Add( (IntPtr) (i + (int)(mbi.BaseAddress)) );
                            }
                        }
                        break;

                    case 2:
                        UInt16 searchValInt16 = Convert.ToUInt16(this.VarVal.Text);
                        for (uint i = 1; i < mbi.RegionSize; i++)
                        {
                            if ( * ((UInt16*)(bufferPointer+i)) == searchValInt16)
                            {
                                this.foundPositions.Add((IntPtr)(i + (int)(mbi.BaseAddress)));
                            }
                        }
                        break;

                    case 4:
                        UInt32 searchValInt32 = Convert.ToUInt32(this.VarVal.Text);
                        for (uint i = 3; i < mbi.RegionSize; i++)
                        {
                            if ( *((Int32*)(bufferPointer+i)) == searchValInt32)
                            {
                                this.foundPositions.Add((IntPtr)(i + (int)(mbi.BaseAddress)));
                            }
                        }
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, this.process.Pid);
            uint lpWrite;
            IntPtr lpWritePointer = new IntPtr(&(lpWrite));
            byte[] buffer =null;

            foreach (IntPtr fp in this.foundPositions)
            {
                switch (Convert.ToInt32(this.comboBox1.Text))
                {
                    case 1:
                        buffer = new byte[1];
                        break;
                    case 2:
                        buffer = new byte[2];
                        break;
                    case 4:
                        buffer = new byte[4];
                        break;
                }

                fixed (byte* bufferPointer = &buffer[0])
                {
                    switch (Convert.ToInt32(this.comboBox1.Text))
                    {
                        case 1:
                            byte writeVal = Convert.ToByte(this.NewVarVal.Text);
                            buffer[0] = writeVal;
                            WriteProcessMemory(processHandle, fp, buffer, 1, out lpWritePointer);
                            break;

                        case 2:
                            Int16 writeValInt16 = Convert.ToInt16(this.NewVarVal.Text);
                            buffer[1] = (byte)(writeValInt16 / 0xFF);
                            buffer[0] = (byte)(writeValInt16 & 0xFF);
                            WriteProcessMemory(processHandle, fp, buffer, 2, out lpWritePointer);
                            break;

                        case 4:
                            Int32 writeValInt32 = Convert.ToInt32(this.NewVarVal.Text);
                            buffer[3] = (byte)(writeValInt32 / 0xFFFFFF);
                            buffer[2] = (byte)((writeValInt32 & 0xFF0000) / 0xFFFF);
                            buffer[1] = (byte)((writeValInt32 & 0xFF00) / 0xFF);
                            buffer[0] = (byte)((writeValInt32 & 0xFF));
                            WriteProcessMemory(processHandle, fp, buffer, 4, out lpWritePointer);
                            break;
                    }
                }
            }
 
        CloseHandle(processHandle);
     }
  
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public UInt32 AllocationProtect;
        public UInt32 RegionSize;
        public UInt32 State;
        public UInt32 Protect;
        public UInt32 lType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SYSTEM_INFO
    {
        public Int32 dwOemId;
        public Int32 dwPageSize;
        public UInt32 lpMinimumApplicationAddress;
        public UInt32 lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public Int32 dwNumberOfProcessors;
        public Int32 dwProcessorType;
        public Int32 dwAllocationGranularity;
        public Int16 wProcessorLevel;
        public Int16 wProcessorRevision;
    }

    public enum MemoryType
    {
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400,
        MEM_COMMIT = 0x1000,
        MEM_RESERVE = 0x2000,
        MEM_DECOMMIT = 0x4000,
        MEM_RELEASE = 0x8000,
        MEM_FREE = 0x10000,
        MEM_PRIVATE = 0x20000,
        MEM_MAPPED = 0x40000,
        MEM_RESET = 0x80000,
        MEM_TOP_DOWN = 0x100000,
        MEM_WRITE_WATCH = 0x200000,
        MEM_PHYSICAL = 0x400000,
        MEM_LARGE_PAGES = 0x20000000,
        SEC_FILE = 0x800000,
        SEC_IMAGE = 0x1000000,
        SEC_RESERVE = 0x4000000,
        SEC_COMMIT = 0x8000000,
        SEC_NOCACHE = 0x10000000,
    }

}
