using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class BorrowReturnNavigationViewModel
    {
        public CreateNewLendRequestViewModel CreateNewLendRequest { get; set; }
        public CreateNewReturnRequestViewModel CreateNewReturnRequest { get; set; }

        public BorrowReturnNavigationViewModel(CreateNewLendRequestViewModel createNewLendRequest, CreateNewReturnRequestViewModel createNewReturnRequest)
        {
            CreateNewLendRequest = createNewLendRequest;
            CreateNewReturnRequest = createNewReturnRequest;
        }
    }
}
