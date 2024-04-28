using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance
{
    public class EquipmentTypeMaintenanceViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly EquipmentTypeStore _equipmentTypeStore;

        public string EquipmentTypeId { get; set; } = "";
        public string EquipmentTypeName { get; set; } = "";
        public ECategory Category { get; set; }

        private List<EquipmentTypeDto> equipmentTypes = new();
        private List<EquipmentTypeDto> filteredEquipmentTypes = new();
        public ObservableCollection<EquipmentTypeEntryViewModel> EquipmentTypeEntries { get; set; } = new();

        public ObservableCollection<string> EquipmentTypeIds => _equipmentTypeStore.EquipmentTypeIds;
        public ObservableCollection<string> EquipmentTypeNames => _equipmentTypeStore.EquipmentTypeNames;
        public ICommand LoadEquipmentTypeEntriesCommand { get; set; }
        public ICommand LoadEquipmentTypeMaintenanceViewCommand { get; set; }

        public EquipmentTypeMaintenanceViewModel(IApiService apiService, IMapper mapper, EquipmentTypeStore equipmentTypeStore) 
        {
            _apiService = apiService;
            _mapper = mapper;
            _equipmentTypeStore = equipmentTypeStore;
            LoadEquipmentTypeEntriesCommand = new RelayCommand(LoadEquipmentTypeEntries);
            LoadEquipmentTypeMaintenanceViewCommand = new RelayCommand(LoadEquipmentTypeMaintenanceView);

        }

        private void LoadEquipmentTypeMaintenanceView()
        {
            LoadInitial();
            OnPropertyChanged(nameof(EquipmentTypeIds));
            OnPropertyChanged(nameof(EquipmentTypeNames));
        }

        private async void LoadInitial()
        {

            Category = ECategory.All;
            EquipmentTypeId = "";
            EquipmentTypeName = "";
            try
            {
                equipmentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<EquipmentTypeDto>, IEnumerable<EquipmentTypeEntryViewModel>>(equipmentTypes);

                EquipmentTypeEntries = new(viewModels);
                if (_mapper is not null && _apiService is not null)
                {
                    foreach (var entry in EquipmentTypeEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                    }
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }

        }
        private void LoadEquipmentTypeEntries()
        {
            try
            {
                if (Category == ECategory.All)
                {
                    filteredEquipmentTypes = equipmentTypes;
                    if (!String.IsNullOrEmpty(EquipmentTypeId))
                    {
                        filteredEquipmentTypes = equipmentTypes.Where(i => i.EquipmentTypeId.Contains(EquipmentTypeId)).ToList();
                    }
                    if (!String.IsNullOrEmpty(EquipmentTypeName))
                    {
                        filteredEquipmentTypes = equipmentTypes.Where(i => i.EquipmentTypeName.Contains(EquipmentTypeName)).ToList();
                    }

                    var viewModels = _mapper.Map<IEnumerable<EquipmentTypeDto>, IEnumerable<EquipmentTypeEntryViewModel>>(filteredEquipmentTypes);
                    EquipmentTypeEntries = new(viewModels);
                }
                else
                {
                    filteredEquipmentTypes = equipmentTypes.Where(i => i.Category == Category).ToList();
                    if (!String.IsNullOrEmpty(EquipmentTypeId))
                    {
                        filteredEquipmentTypes = equipmentTypes.Where(i => i.EquipmentTypeId.Contains(EquipmentTypeId)).ToList();
                    }
                    if (!String.IsNullOrEmpty(EquipmentTypeName))
                    {
                        filteredEquipmentTypes = equipmentTypes.Where(i => i.EquipmentTypeName.Contains(EquipmentTypeName)).ToList();
                    }

                    var viewModels = _mapper.Map<IEnumerable<EquipmentTypeDto>, IEnumerable<EquipmentTypeEntryViewModel>>(filteredEquipmentTypes);
                    EquipmentTypeEntries = new(viewModels);
                }

                foreach (var entry in EquipmentTypeEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void Error()
        {
            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
        }
    }
}
