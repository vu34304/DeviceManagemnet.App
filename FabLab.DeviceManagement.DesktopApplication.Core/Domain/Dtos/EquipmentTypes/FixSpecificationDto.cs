using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes
{
    public  class FixSpecificationDto
    {
        public string equipmentTypeId { get; set; }
        public ObservableCollection<SpecificationEquimentType> specs { get; set; }

        public FixSpecificationDto(string equipmentTypeId, ObservableCollection<SpecificationEquimentType> specs)
        {
            this.equipmentTypeId = equipmentTypeId;
            this.specs = specs;
        }
    }
}
