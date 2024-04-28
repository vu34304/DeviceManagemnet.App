using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Store
{
    public class BorrowStore
    {
        public List<BorrowDto> Borrows { get; private set; }
        public List<string> BorrowId { get; private set; }
        public List<DateTime> BorrowedDate { get; private set; }
        public List<DateTime> ReturnedDate { get; private set; }
        public List<string> Borrower { get; private set; }
        public List<string> Reason { get; private set; }

    }
}
