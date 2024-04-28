using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags
{
    public class TagDto
    {
        public string TagId { get; set; }
        public string TagDetail { get; set; }

        public TagDto( string tagId, string tagDetail)
        {
           TagId = tagId;
           TagDetail = tagDetail;          
        }
    }
}
