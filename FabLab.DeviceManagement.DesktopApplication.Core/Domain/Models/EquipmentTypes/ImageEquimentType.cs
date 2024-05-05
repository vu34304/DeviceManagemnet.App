using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes
{
    public class ImageEquimentType:BaseViewModel
    {
        private string _ImagePath;
        public string ImagePath 
        {
            get => _ImagePath;
            set 
            {
                _ImagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            } 
        }
        public string ImageName { get; set; }

    }
}
