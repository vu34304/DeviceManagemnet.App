using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes
{
    public class FixImageDto
    {
        public string equipmentTypeId { get; set; }
        public List<byte[]> fileData { get; set; }

        public FixImageDto(string equipmentTypeId, List<byte[]> fileData)
        {
            this.equipmentTypeId = equipmentTypeId;
            this.fileData = fileData;
        }
    }
}
