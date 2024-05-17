using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Borrows;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Store
{
    public class ProjectStore
    {
        public List<ProjectDto> Projects { get; private set; }
        public ObservableCollection<string> ProjectNames { get; private set; }
        public ObservableCollection<DateTime> StartDates { get; private set; }
        public ObservableCollection<DateTime> EndDates { get; private set; }
        public ObservableCollection<DateTime?> RealEndDates { get; private set; }
        public ObservableCollection<string> Descriptions { get; private set; }
        public ObservableCollection<bool> Approveds {  get; private set; }

        public ProjectStore()
        {
            Projects = new();
            ProjectNames = new();
            StartDates = new();
            EndDates = new();
            RealEndDates = new();
            Descriptions = new();
            Approveds = new();
        }

        public void SetProject(IEnumerable<ProjectDto> projects)
        {
            Projects = projects.ToList();
            ProjectNames = new ObservableCollection<string>(Projects.Select(i => i.ProjectName).OrderBy(s => s));
            StartDates = new ObservableCollection<DateTime>(Projects.Select(i => i.StartDate).OrderBy(s => s));
            EndDates = new ObservableCollection<DateTime>(Projects.Select(i => i.EndDate).OrderBy(s => s));
            RealEndDates = new ObservableCollection<DateTime?>(Projects.Select(i => i.RealEndDate).OrderBy(s => s));
            Descriptions = new ObservableCollection<string>(Projects.Select(i => i.Description).OrderBy(s => s));
            Approveds = new ObservableCollection<bool>(Projects.Select(i => i.Approved).OrderBy(s => s));
        }
    }
}
