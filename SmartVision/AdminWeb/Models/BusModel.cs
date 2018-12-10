using System.Collections.Generic;
using System.Web.Mvc;
using Objects.Buses;

namespace AdminWeb.Models
{
    public class BusModel
    {
        public int SelectedBusId { get; set; }
        public IEnumerable<SelectListItem> Buses { get; set; }
    }
}