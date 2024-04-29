using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments
{
    public class EquipmentDto
    {
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime YearOfSupply { get; set; }
        public string CodeOfManager { get; set; }
        public string LocationId { get; set; }
        public string SupplierName { get; set; }
        public EStatus Status { get; set; }
        public string EquipmentTypeId { get; set; }

     

        public EquipmentDto(string equipmentId, string equipmentName, DateTime yearOfSupply, string codeOfManager, string locationId, string supplierName, EStatus status, string equipmentTypeId)
        {
            EquipmentId = equipmentId;
            EquipmentName = equipmentName;
            YearOfSupply = yearOfSupply;
            CodeOfManager = codeOfManager;
            LocationId = locationId;
            SupplierName = supplierName;
            Status = status;
            EquipmentTypeId = equipmentTypeId;
        }










        //public LocationDto LocationId { get; set; }
        //public SupplierDto SupplierName { get; set; }
        //public EStatus Status { get; set; }
        //public EquipmentTypeDto EquipmentTypeId { get; set; }
        //public EquipmentDto(string equipmentId, string equipmentName, DateTime yearOfSupply, string codeOfManager, LocationDto locationId, SupplierDto supplierName, EStatus status, EquipmentTypeDto equipmentTypeId)
        //{
        //    EquipmentId = equipmentId;
        //    EquipmentName = equipmentName;
        //    YearOfSupply = yearOfSupply;
        //    CodeOfManager = codeOfManager;
        //    LocationId = locationId;
        //    SupplierName = supplierName;
        //    Status = status;
        //    EquipmentTypeId = equipmentTypeId;
        //}
    }
}
