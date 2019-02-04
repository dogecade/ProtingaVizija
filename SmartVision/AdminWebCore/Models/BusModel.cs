using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminWeb.Models
{
    public class BusModel
    {
        public int SelectedBusId { get; set; }
        public IEnumerable<SelectListItem> Buses { get; set; }
    }
}