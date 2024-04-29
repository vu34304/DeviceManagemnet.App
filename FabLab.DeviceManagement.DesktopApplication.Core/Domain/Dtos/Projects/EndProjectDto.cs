using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public class EndProjectDto
    {
        public string ProjectName {  get; set; }
        public DateTime RealEndDate {  get; set; }

        public EndProjectDto(string projectName, DateTime realEndDate)
        {
            ProjectName = projectName;
            RealEndDate = realEndDate;
        }
    }
}
