using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public class ApprovedProjectDto
    {
        public string ProjectName { get; set; }
        public bool Approved { get; set; }

        public ApprovedProjectDto(string projectName, bool approved)
        {
            ProjectName = projectName;
            Approved = approved;
        }
    }
}
