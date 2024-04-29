using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class DeviceEntryViewModel : BaseViewModel
    {

        private SupplierStore? _supplierStore; 
        private LocationStore? _locationStore;
        private EquipmentTypeStore? _equipmentTypeStore;
        public ObservableCollection<string>? SupplierNames => _supplierStore?.SupplierNames;
        public ObservableCollection<string>? LocationIds => _locationStore?.LocationIds;
        public ObservableCollection<string>? EquipmentTypeIds => _equipmentTypeStore?.EquipmentTypeIds;
        public ObservableCollection<string>? EquipmentTypeNames => _equipmentTypeStore?.EquipmentTypeNames;
        private IApiService? _apiService;
        private IMapper? _mapper;
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime YearOfSupply { get; set; }
        public string CodeOfManager { get; set; }
        public string LocationId { get; set; }
        public string SupplierName { get; set; }
        public EStatus Status { get; set; }
        public string[] Tags { get; set; }

        public bool IsMaintenance => (Status == EStatus.Maintenance || Status == EStatus.NonFunctional) ? false : true;
        public ObservableCollection<SpecificationEquimentType> Specifications { get; set; } = new();
        public ObservableCollection<ImageBitmap> Pictures { get; set; } = new();
        public List<FileDataBase64EquipmentType> DataPics { get; set; }
        

        private string equipmentTypeId;
        private string? equipmentTypeName;
        public string EquipmentTypeId
        {
            get
            {
                return equipmentTypeId;
            }
            set
            {
                equipmentTypeId = value;
                if (_equipmentTypeStore is not null && !String.IsNullOrEmpty(value))
                {
                    var equipmentType = _equipmentTypeStore.EquipmentTypes.First(i => i.EquipmentTypeId == equipmentTypeId);
                    equipmentTypeName = equipmentType.EquipmentTypeName;
                    OnPropertyChanged(nameof(EquipmentTypeName));
                }
            }

        }
        public string? EquipmentTypeName
        {
            get
            {
                return equipmentTypeName;
            }
            set
            {
                equipmentTypeName = value;
                if (_equipmentTypeStore is not null && !String.IsNullOrEmpty(value))
                {
                    var equipmentType = _equipmentTypeStore.EquipmentTypes.First(i => i.EquipmentTypeName == equipmentTypeName);
                    equipmentTypeId = equipmentType.EquipmentTypeId;
                    OnPropertyChanged(nameof(EquipmentTypeId));
                }
            }
        }
        public event Action? Updated;
        public event Action? OnException;
        public event Action? IsOpenFixView;
        public event Action? IsOpenMoreDetailView;


        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateStatusCommand { get; set; }

        public ICommand GetSpecificationEquipmentTypesAsyncCommand { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DeviceEntryViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            SaveCommand = new RelayCommand(SaveAsync);
            UpdateStatusCommand = new RelayCommand(UpdateStatus);
            DeleteCommand = new RelayCommand(DeleteAsync);
            GetSpecificationEquipmentTypesAsyncCommand = new RelayCommand(GetSpecificationEquipmentTypesAsync);


        }
        public DeviceEntryViewModel(string equipmentId, string equipmentName, DateTime yearOfSupply, string codeOfManager, string locationId, string supplierName, EStatus status, string equipmentTypeId, string? equipmentTypeName, string[] tags) : this()
        {
            EquipmentId = equipmentId;
            EquipmentName = equipmentName;
            YearOfSupply = yearOfSupply;
            CodeOfManager = codeOfManager;
            LocationId = locationId;
            SupplierName = supplierName;
            Status = status;
            EquipmentTypeId = equipmentTypeId;
            EquipmentTypeName = equipmentTypeName;
            Tags = tags;
        }


        public void SetApiService(IApiService apiService)
        {
            _apiService = apiService;
        }
        public void SetMapper(IMapper mapper)
        {
            _mapper = mapper;
            OnPropertyChanged();
        }
        public void SetStore(SupplierStore supplierStore, LocationStore locationStore, EquipmentTypeStore equipmentTypeStore)
        {
            _supplierStore = supplierStore;
            _locationStore = locationStore;
            _equipmentTypeStore = equipmentTypeStore;
            OnPropertyChanged();
        }
        private async void UpdateStatus()
        {
            FixEquipmentDto fixDto = new FixEquipmentDto(EquipmentId, EquipmentName, YearOfSupply, CodeOfManager, EStatus.Maintenance, LocationId,SupplierName,equipmentTypeId);

            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    await _apiService.FixEquipmentAsync(fixDto);
                    Updated?.Invoke();
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (HttpRequestException)
                {
                    OnException?.Invoke();
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
        }
        private void SaveAsync()
        {

            IsOpenFixView?.Invoke();

        }

        private async void DeleteAsync()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteEquipmentAsync(EquipmentId);                     
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        Updated?.Invoke();
                    }
                    else { }

                }
                catch (HttpRequestException)
                {
                    OnException?.Invoke();
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            
        }

        private void GetSpecificationEquipmentTypesAsync()
        {
            IsOpenMoreDetailView?.Invoke();  

        }

       
    }
}
