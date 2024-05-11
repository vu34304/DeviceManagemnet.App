using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments
{
    public enum ECategory
    {
        [Description("Tất cả")]
        All = 0,
        [Description("Cơ khí")]
        Mechanical = 1,
        [Description("IoT_Robotics")]
        IoT_Robotics = 2,
        [Description("Tự động")]
        Automation = 3,
       
    }
}
