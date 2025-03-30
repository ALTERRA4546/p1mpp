using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Desktop.Model
{
    public class EmployeeSelectedModel
    {
        public int IDEmployee { get; set; }
        public int IDDivision { get; set; }
        public int IDDirector { get; set; }
        public string Director { get; set; }
        public int IDAssistent { get; set; }
        public string Assistent { get; set; }
        public string Division { get; set; }
        public int IDPost { get; set; }
        public string Post { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PersonalPhone { get; set; }
        public string CorpPhone { get; set; }
        public string Email { get; set; }
        public string Cabinet { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfFired { get; set; }
        public bool Fired { get; set; }
        public string OtherInformation { get; set; }
    }
}
