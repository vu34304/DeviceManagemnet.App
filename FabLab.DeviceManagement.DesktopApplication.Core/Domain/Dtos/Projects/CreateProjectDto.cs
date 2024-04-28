using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public class CreateProjectDto
    {
        public string projectName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string description { get; set; }
        public List<string> equipments { get; set; }

        public CreateProjectDto(string projectName, DateTime startDate, DateTime endDate, string description, List<string> equipments)
        {
            this.projectName = projectName;
            this.startDate = startDate;
            this.endDate = endDate;
            this.description = description;
            this.equipments = equipments;
        }
    }
}
