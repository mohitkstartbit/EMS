using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table(name: "shift", Schema = "public")]
    public class Shift
    {
        [Key]

        public int shift_id { get; set; }

        public string shift_name { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public TimeSpan interval { get; set; }
        public TimeSpan delayedby { get; set; }


    }
}
