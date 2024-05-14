using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings
{
    public class BorrowEquipmentDto: BaseViewModel
    {
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime YearOfSupply { get; set; }
        public string CodeOfManager { get; set; }
        public EStatus Status { get; set; }
        private string _StatusStr;
        public string StatusStr
        {
            get => _StatusStr;
            set
            {
                _StatusStr = value;
                OnPropertyChanged();
                switch (Status)
                {
                    case EStatus.Active:
                        {
                            _StatusStr = "Khả dụng";
                            StatusFontWeight = "Bold";
                           
                           
                            break;
                        }
                    case EStatus.Inactive:
                        {
                            _StatusStr = "Đang mượn";
                            StatusFontWeight = "Normal";
                            
                          
                            break;
                        }
                    case EStatus.NonFunctional:
                        {
                            _StatusStr = "Đang hỏng";
                           
                            break;
                        }
                    case EStatus.Maintenance:
                        {
                            _StatusStr = "Đang bảo trì";
                           
                            break;
                        }
                    default: break;
                }
            }
        }

        private string _StatusFontWeight;
        public string StatusFontWeight
        {
            get => _StatusFontWeight;
            set
            {
                _StatusFontWeight = value;
                OnPropertyChanged();
            }
        }

        private bool _IsChecked;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                OnPropertyChanged();
            }
        }
        private bool _IsUnChecked;
        public bool IsUnChecked
        {
            get => _IsUnChecked;
            set
            {
                _IsUnChecked = value;               
                OnPropertyChanged();
            }
        }



    }
}
