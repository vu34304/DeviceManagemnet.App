using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Store
{
    public class TagStore
    {
        public List<TagDto> Tags { get; private set; }
        public ObservableCollection<string> TagIds { get; private set; }
        public ObservableCollection<string> TagDetails { get; private set; }
   
        public TagStore()
        {
            Tags = new List<TagDto>();
            TagIds = new ObservableCollection<string>();
            TagDetails = new ObservableCollection<string>();
        }
        public void SetTag(IEnumerable<TagDto> tags)
        {
            Tags = tags.ToList();
            TagIds = new ObservableCollection<string>(Tags.Select(i => i.TagId).OrderBy(s => s));
            TagDetails = new ObservableCollection<string>(Tags.Select(i => i.TagDetail).OrderBy(s => s));
        }
    }
}
