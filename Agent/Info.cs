using System;
using System.Collections.Generic;
using System.Text;

namespace Agent
{
    class Info
    {
        public List<process> processes { get; set; }
        public List<PrintDoc> PrintDoc { get; set; }
        public List<UsbDriver> usb { get; set; }

        public Info()
        {
            processes = new List<process>();
            PrintDoc = new List<PrintDoc>();
            usb = new List<UsbDriver>();
        }
    }

    class process
    {
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }

    }

    class PrintDoc
    {
        public string Name { get; set; }
        public string Program { get; set; }
        public DateTime Date_Time { get; set; }
    }

    class UsbDriver
    {
        public string Name { get; set; }
        public DateTime Date_Time { get; set; }
    }
}
