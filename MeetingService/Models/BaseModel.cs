using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingService.Models
{
    public class BaseModel
    {
        public int? ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
    }
}