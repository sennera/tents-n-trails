using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TentsNTrails.Models
{
    public class RequestListViewModel
    {
        public ICollection<ConnectionRequest> Requests { get; set; }
        public int RowCount { get; set; }

    }
}