using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class DeviceManagementNavigationViewModel
    { 
        public DeviceManagementViewModel DeviceManagement { get; set; }
        public EquipmentTypeViewModel EquipmentType { get; set; }
        public SupplierLocationTagViewModel SupplierLocationTag { get; set; }
        public DeviceManagementNavigationViewModel(DeviceManagementViewModel deviceManagement, EquipmentTypeViewModel equipmentType, SupplierLocationTagViewModel supplierLocationTag)
        {
            DeviceManagement = deviceManagement;
            EquipmentType = equipmentType;
            SupplierLocationTag = supplierLocationTag;
        }
    }
}
