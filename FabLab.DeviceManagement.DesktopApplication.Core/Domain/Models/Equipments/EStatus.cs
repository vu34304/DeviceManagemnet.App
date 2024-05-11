using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments
{
    public enum EStatus
    {
        [Description("Khả dụng")]
        Active = 0,
        [Description("Đang mượn")]
        Inactive = 1,
        [Description("Đang hỏng")]
        NonFunctional = 2,
        [Description("Đang bảo trì")]
        Maintenance = 3,
       
    }
}
