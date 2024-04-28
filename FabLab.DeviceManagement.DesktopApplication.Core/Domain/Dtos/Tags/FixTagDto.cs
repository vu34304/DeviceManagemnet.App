using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags
{
    public class FixTagDto
    {
        public string TagId { get; set; }
        public string TagDetail { get; set; }

        public FixTagDto(string tagId, string tagDetail)
        {
            TagId = tagId;
            TagDetail = tagDetail;
        }
    }
}
