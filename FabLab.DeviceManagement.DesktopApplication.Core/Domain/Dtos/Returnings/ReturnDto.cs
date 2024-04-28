using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Returnings
{
    public class ReturnDto
    {
        public string BorrowId { get; set; }
        public DateTime RealReturnedDate { get; set; }
    }

}
