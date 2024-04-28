using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes
{
    
    public class InformationEquipmentDto
    {
        public List<FileDataBase64EquipmentType> Pics { get; set; }
        public List<SpecificationEquimentType> Specs { get; set; }

        public InformationEquipmentDto(List<FileDataBase64EquipmentType> pics, List<SpecificationEquimentType> specs)
        {
            Pics = pics;
            Specs = specs;
        }



    }


}
