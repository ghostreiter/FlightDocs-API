using System;


namespace NextDueDate.Models
{
    public class FDTask
    {
        public int airCraftID { get; set; }
        public int itemNumber { get; set; }
        public string desc { get; set; }
        public string logDate { get; set; }
        public int? logHours { get; set; }
        public int? intervalMonths { get; set; }
        public int? intervalHours { get; set; }
        public DateTime? nextDue { get; set; }
    }
}
