using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects
{
    public class Project
    {
        public string ProjectName { get;  set; }
        public DateTime StartDate { get;  set; }
        public DateTime EndDate { get; set; }
        public DateTime RealEndDate { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }

        public Project(string projectName, DateTime startDate, DateTime endDate, DateTime realEndDate, string description, bool approved)
        {
            ProjectName = projectName;
            StartDate = startDate;
            EndDate = endDate;
            RealEndDate = realEndDate;
            Description = description;
            Approved = approved;
        }
    }
}
