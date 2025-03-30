using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Desktop.Model
{
    public class DivisionModel
    {
        public int IDDivision { get; set; }
        public int? IDMainDivision { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? IDDirector { get; set; }
        public List<DivisionModel> Division1 { get; set; }
    }
}
