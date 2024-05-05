using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects
{
    public class AddBorrowEquipments:BaseViewModel
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
            
        public string name { get; set; }
        public string id { get; set; }
    }
}
