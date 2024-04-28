
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Commands;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class TagExt : BaseViewModel
    {
        private bool _isSelected;

        public Tag Tag { get; private set; }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public TagExt(Tag tag)
        {
            Tag = tag;
        }
    }
}
