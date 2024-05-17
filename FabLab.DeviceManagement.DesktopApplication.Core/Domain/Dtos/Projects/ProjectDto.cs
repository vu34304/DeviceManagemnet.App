using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Borrows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public class ProjectDto
    {
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }


        public ProjectDto(string projectName, DateTime startDay, DateTime endDay, DateTime? realEndDay, string description, bool approved)
        {
            ProjectName = projectName;
            StartDate = startDay;
            EndDate = endDay;
            RealEndDate = realEndDay;
            Description = description;
            Approved = approved;
        }
    }
}
