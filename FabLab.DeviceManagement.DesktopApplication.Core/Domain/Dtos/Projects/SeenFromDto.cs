using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public  class SeenFromDto
    {
        public string ProjectName { get; set; }

        public SeenFromDto(string projectName)
        {
            ProjectName = projectName;
        }
    }
}
