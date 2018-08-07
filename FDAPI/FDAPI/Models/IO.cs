using System;
using System.Collections.Generic;

namespace FDAPI.Models
{
    public class IO
    {
        public int aircraftID { get; set; }
        public List<FDTask> fDTasks { get; set; }
    }
}
