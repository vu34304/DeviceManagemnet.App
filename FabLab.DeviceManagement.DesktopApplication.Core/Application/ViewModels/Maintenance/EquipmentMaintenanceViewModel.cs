using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance
{
    public class EquipmentMaintenanceViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly EquipmentStore _equipmentStore;
        private readonly EquipmentTypeStore _equipmentTypeStore;

        private List<EquipmentDto> equipments = new();
        private List<EquipmentDto> filteredEquipments = new();
        private List<SupplierDto> suppliers = new();
        private List<LocationDto> locations = new();
        private List<EquipmentTypeDto> equimentTypes = new();

        public DeviceEntryViewModel DeviceEntry { get; set; }
        public ObservableCollection<DeviceEntryViewModel> DeviceEntries { get; set; } = new();

        public string EquipmentId { get; set; } = "";
        public string EquipmentName { get; set; } = "";
        public string EquipmentName1 { get; set; } = "";
        public string YearOfSupply { get; set; } = "";
        public string CodeOfManage { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public string LocationId { get; set; } = "";
        public string EquipmentTypeName { get; set; } = "";
        public string EquipmentTypeId { get; set; } = "";     
        public EStatus Status { get; set; }
        private string _StatusStr;
        public string StatusStr 
        {
            get=>_StatusStr;
            set
            {
                _StatusStr = value;
                switch (_StatusStr)
                {
                    case "Khả dụng":
                        {
                            Status=EStatus.Active; break;
                        }
                    case "Đang mượn":
                        {
                            Status = EStatus.Inactive; break;
                        }
                    case "Đang hỏng":
                        {
                            Status = EStatus.NonFunctional; break;
                        }
                    case "Đang bảo trì":
                        {
                            Status = EStatus.Maintenance; break;
                        }
                    default: break;
                }
            }
        }
       
        public ECategory Category { get; set; }
        private string _CategoryStr;
        public string CategoryStr
        {
            get => _CategoryStr;
            set
            {
                _CategoryStr = value;
                switch (_CategoryStr)
                {
                    case "Tất cả":
                        {
                            Category = ECategory.All;                       
                            break;
                        }
                    case "Cơ khí":
                        {
                            Category = ECategory.Mechanical;
                            break;
                        }
                    case "Tự động":
                        {
                            Category = ECategory.Automation;
                            break;

                        }
                    case "IoT_Robotics":
                        {
                            Category = ECategory.IoT_Robotics;
                            break;

                        }
                        
                    default: break;
                }
            }
        }

        public List<string> EquipmentNames { get; set; } = new();
        public List<string> EquipmentIds { get; set; } = new();
        public List<string> EquipmentTypeIds { get; set; } = new();
        public List<string> EquipmentTypeNames { get; set; } = new();
        public List<string> LocationIds { get; set; } = new();
        public List<string> SupplierNames { get; set; } = new();
        public List<string> CodeOfManagers { get; set; } = new();

        public ICommand LoadInitialCommand { get; set; }
        public ICommand LoadEquipmentMaintenanceViewCommand { get; set; }
        public ICommand LoadDeviceEntriesCommand { get; set; }




        public ObservableCollection<string> Years { get; set; } = new();
        public EquipmentMaintenanceViewModel(IApiService apiService, IMapper mapper) 
        {
            _apiService = apiService;
            _mapper = mapper;
            Years = new ObservableCollection<string>();
            for (int i = DateTime.Now.Year; i >= 1975; i--)
            {
                Years.Add(i.ToString());
            }

            LoadEquipmentMaintenanceViewCommand = new RelayCommand(LoadEquipmentMaintenaceView);
            LoadDeviceEntriesCommand = new RelayCommand(LoadDeviceEntriesAdvance);
        }

        private void LoadEquipmentMaintenaceView()
        {
            LoadInitial();
            UpdateSupplier();
            UpdateEquimentTypeId();
            UpdateEquimentTypeName();
            UpdateLocation();

            OnPropertyChanged(nameof(EquipmentIds));
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(EquipmentTypeIds));
            OnPropertyChanged(nameof(EquipmentTypeNames));
            OnPropertyChanged(nameof(LocationIds));
            OnPropertyChanged(nameof(SupplierNames));

        }
        private async void LoadInitial()
        {
            
            EquipmentId = "";
            EquipmentName = "";
            EquipmentTypeId = "";
            EquipmentTypeName = "";

            try
            {
                equipments = (await _apiService.GetAllEquipmentsAsync()).ToList();
                //var filteredEquipmentsDtos = equipments.Where(i => i.ItemId.Contains(ItemIdFilter));

                var viewModels = _mapper.Map<IEnumerable<EquipmentDto>, IEnumerable<DeviceEntryViewModel>>(equipments);
                DeviceEntries = new(viewModels);

                foreach (var entry in DeviceEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.SetStatusEquipment();
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                }
                EquipmentNames = DeviceEntries.Select(i => i.EquipmentName).ToList();
                EquipmentIds = DeviceEntries.Select(i => i.EquipmentId).ToList();
                CodeOfManagers = DeviceEntries.Select(i => i.CodeOfManager).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server .");
            }
        }
        private async void LoadDeviceEntriesAdvance()
        {
            try
            {
                if (CategoryStr is not null || StatusStr is not null || !String.IsNullOrEmpty(EquipmentId) 
                    || !String.IsNullOrEmpty(EquipmentName) || !String.IsNullOrEmpty(YearOfSupply)
                    || !String.IsNullOrEmpty(EquipmentTypeId))
                {
                    filteredEquipments = (await _apiService.GetEquipmentsRecordsAsync(EquipmentId, EquipmentName, YearOfSupply, EquipmentTypeId, CategoryStr, StatusStr, null)).ToList();
                }
                else filteredEquipments= (await _apiService.GetAllEquipmentsAsync()).ToList();


                //filteredEquipments = (await _apiService.GetEquipmentsRecordsAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<EquipmentDto>, IEnumerable<DeviceEntryViewModel>>(filteredEquipments);
                DeviceEntries = new(viewModels);

                foreach (var entry in DeviceEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);                  
                    entry.Updated += LoadInitial;
                    entry.SetStatusEquipment();
                    entry.OnException += Error;
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server test.");
            }
            CategoryStr = null;
            StatusStr = null;
            
            

        }
        private void Error()
        {
            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
        }

        private async void UpdateSupplier()
        {
            try
            {
                suppliers = (await _apiService.GetAllSuppliersAsync()).ToList();
                SupplierNames = suppliers.Select(i => i.SupplierName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private async void UpdateLocation()
        {
            try
            {
                locations = (await _apiService.GetAllLocationsAsync()).ToList();
                LocationIds = locations.Select(i => i.LocationId).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private async void UpdateEquimentTypeId()
        {
            try
            {
                equimentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();
                EquipmentTypeIds = equimentTypes.Select(i => i.EquipmentTypeId).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private async void UpdateEquimentTypeName()
        {
            try
            {
                equimentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();
                EquipmentTypeNames = equimentTypes.Select(i => i.EquipmentTypeName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
    }
}
