using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings
{
    public class FixBorrowDto
    {
        public string BorrowId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string Borrower { get; set; }
        public string Reason { get; set; }
        public bool OnSite { get; set; }
        public ProjectDto Project { get; set; }
        public List<EquipmentDto> Equipments { get; set; }

        public FixBorrowDto(string borrowId, DateTime borrowedDate, DateTime returnedDate, string borrower, string reason, bool onSite, ProjectDto project, List<EquipmentDto> equipments)
        {
            BorrowId = borrowId;
            BorrowedDate = borrowedDate;
            ReturnedDate = returnedDate;
            Borrower = borrower;
            Reason = reason;
            OnSite = onSite;
            Project = project;
            Equipments = equipments;
        }
    }
}
