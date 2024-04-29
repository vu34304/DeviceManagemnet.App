using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings
{
    public class CreateBorrowDto
    {
        public string BorrowId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string Borrower { get; set; }
        public string Reason { get; set; }
        public bool OnSite { get; set; }
        public string ProjectName { get; set; }
        public List<string> Equipments { get; set; }

        public CreateBorrowDto(string borrowId, DateTime borrowedDate, DateTime returnedDate, string borrower, string reason, bool onSite, string projectName, List<string> equipments)
        {
            BorrowId = borrowId;
            BorrowedDate = borrowedDate;
            ReturnedDate = returnedDate;
            Borrower = borrower;
            Reason = reason;
            OnSite = onSite;
            ProjectName = projectName;
            Equipments = equipments;
        }
    }
}
