using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes
{
    public class ImageBitmap: BaseViewModel
    {
        private int _index;
        public int index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(index));
            }
        }
        public BitmapImage Source { get; set; }
        
        
    }
}
