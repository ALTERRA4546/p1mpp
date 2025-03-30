using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Desktop.Model
{
    public class SaveEmployeeModel
    {
        public int? IDEmployee { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PersonalPhone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int IDDivision { get; set; }
        public int IDPost { get; set; }
        public int? IDDirector { get; set; }
        public int? IDAssistent { get; set; }
        public string CorpPhone { get; set; }
        public string Email { get; set; }
        public string Cabinet { get; set; }
        public string OtherInformation { get; set; }
    }
}
