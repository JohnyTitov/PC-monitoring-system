using System;
using System.Collections.Generic;

namespace Server
{
    public class Reports
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public List<report> reports { get; set; }

        public Reports()
        {
            reports = new List<report>();
        }
    }

    public class report
    {
        public string Type { get; set; }        // тип отчёта
        public DateTime Date { get; set; }      // дата и время получения отчёта
    }
}
