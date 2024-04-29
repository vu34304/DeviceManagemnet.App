using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance
{
    public class MaintenanceNavigationViewModel : BaseViewModel
    {
        public EquipmentMaintenanceViewModel EquipmentMaintenanceViewModel { get; set; }
        public EquipmentTypeMaintenanceViewModel EquipmentTypeMaintenanceViewModel { get; set; }
        public SupplierLocationMaintenanceViewModel SupplierLocationMaintenanceViewModel { get; set;}

        public MaintenanceNavigationViewModel(EquipmentMaintenanceViewModel equipmentMaintenanceViewModel/*, EquipmentTypeMaintenanceViewModel equipmentTypeMaintenanceViewModel, SupplierLocationMaintenanceViewModel supplierLocationMaintenanceViewModel*/)
        {
            EquipmentMaintenanceViewModel = equipmentMaintenanceViewModel;
            //EquipmentTypeMaintenanceViewModel = equipmentTypeMaintenanceViewModel;
            //SupplierLocationMaintenanceViewModel = supplierLocationMaintenanceViewModel;
        }
    }
}
