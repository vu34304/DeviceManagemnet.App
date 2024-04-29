using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes
{
    public class Tag
    {
        private string _tagId { get; set; }

        public string TagId => this._tagId;

        public Tag(string tagId)
        {
            this._tagId = tagId;
        }
    }
}
