using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects
{
    public class FormSubcribeDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public string MSSV { get; set; }
        public bool Onsite { get; set; }
        public string LinkGgDrive { get; set; }
        public bool CheckSeen { get; set; }
        public DateTime CreateAt { get; set; }
        public string equipment { get; set; }

    }
}
