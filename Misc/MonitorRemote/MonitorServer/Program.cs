﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace MonitorServer
{
    class MonitorServer
    {
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static int HWND_BROADCAST = 0xffff;
        //the message is sent to all 
        //top-level windows in the system

        const int HWND_TOPMOST = -1;
        //the message is sent to one 
        //top-level window in the system

        const int HWND_TOP = 0;        //
        const int HWND_BOTTOM = 1;        //limited use
        const int HWND_NOTOPMOST = -2;       //        

        const int SC_MONITORPOWER = 0xF170;
        const uint WM_SYSCOMMAND = 0x0112;

        const int MONITOR_ON = (-1);
        const int MONITOR_OFF = 2;
        const int MONITOR_STANBY = 1;


        static void Main(string[] args)
        {
            for (; ; )
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 8000);
                Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                newsock.Bind(localEndPoint);
                newsock.Listen(10);
                Socket client = newsock.Accept();
                byte[] buffer = new byte[512];
                if (client.Receive(buffer, SocketFlags.None) > 0)
                {
                    string s = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                    if (s.CompareTo("MonitorON") == 0)
                    {
                        SendMessage((IntPtr)FindWindow(null, null), WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_ON);
                    }
                    if (s.CompareTo("MonitorOFF") == 0)
                    {
                        SendMessage((IntPtr)FindWindow(null, null), WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_OFF);
                    }
                }
                newsock.Close();
            }
        }
    }
}