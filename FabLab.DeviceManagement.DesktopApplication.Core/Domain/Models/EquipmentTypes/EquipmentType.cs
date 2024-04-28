using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes
{
    public class EquipmentType
    {
        public string EquimentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public string Description { get; set; }
        public ECategory Category { get; set; }
       

        public EquipmentType(string equimentTypeId, string equimenttypeName, string description, ECategory category) 
        {
            EquimentTypeId = equimentTypeId;
            EquipmentTypeName = equimenttypeName;
            Description = description;
            Category = category;
        }
    }
}
