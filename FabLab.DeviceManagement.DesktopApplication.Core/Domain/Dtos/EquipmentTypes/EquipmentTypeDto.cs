using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes
{
    public class EquipmentTypeDto
    {
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public string Description { get; set; }
        public ECategory Category { get; set; }
        public string[] Tags { get; set; }

        public EquipmentTypeDto(string equipmentTypeId, string equipmentTypeName, string description, ECategory category, string[] tags)
        {
            EquipmentTypeId = equipmentTypeId;
            EquipmentTypeName = equipmentTypeName;
            Description = description;
            Category = category;
            Tags = tags;
        }
    }
}
