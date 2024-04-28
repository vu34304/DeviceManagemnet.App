using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
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
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class SupplierLocationTagViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        //Location
        private readonly LocationStore _locationStore;
        public string LocationId { get; set; } = "";
        public string Note { get; set; } = "";
        // Create New Location
        public string NewLocationId { get; set; } = "";
        public string NewNote { get; set; } = "";
        private List<LocationDto> Locations = new();
        private List<LocationDto> FilteredLocations = new();
        public ObservableCollection<LocationEntryViewModel> LocationEntries { get; set; } = new();
        public List<string> LocationIds { get; set; } = new();

        //Tag

        public string TagId { get; set; } = "";
        public string TagDetail { get; set; } = "";
        // Create New Tag
        public string NewTagId { get; set; } = "";
        public string NewTagDetail { get; set; } = "";
        private List<TagDto> Tags = new();
        private List<TagDto> FilteredTags = new();
        public ObservableCollection<TagEntryViewModel> TagEntries { get; set; } = new();
        public List<string> TagIds { get; set; } = new();

        //Supplier
        private readonly SupplierStore _supplierStore;     
        public string SupplierName { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; }= "";    
        //Create new supplier
        public string NewSupplierName { get; set; } = "";
        public string NewAddress { get; set; } = "";
        public string NewPhoneNumber { get; set; } = "";
        private List<SupplierDto> Suppliers = new();
        private List<SupplierDto> FilteredSuppliers = new();
        public ObservableCollection<SupplierEntryViewModel> SupplierEntries { get; set; } = new();
        public List<string> SupplierNames { get; set; } = new()
            
            ;
        public ICommand LoadTagEntriesCommand { get; set; }
        public ICommand LoadSupplierEntriesCommand { get; set; }
        public ICommand LoadLocationEntriesCommand { get; set; }
        public ICommand LoadInitialCommand { get; set; }
        public ICommand LoadViewCommand { get; set; }
        public ICommand CreateSupplierCommand { get; set; }
        public ICommand CreateLocationCommand { get; set; }
        public ICommand CreateTagCommand { get; set; }
        public SupplierLocationTagViewModel(IApiService apiService, IMapper mapper, SupplierStore supplierStore, LocationStore locationStore)
        {
            _apiService = apiService;
            _mapper = mapper;
            _supplierStore = supplierStore;
            _locationStore = locationStore;

            LoadInitialCommand = new RelayCommand(LoadInitial);
            LoadSupplierEntriesCommand = new RelayCommand(LoadSupplierEntries);
            LoadViewCommand = new RelayCommand(LoadView);
            CreateSupplierCommand = new RelayCommand(CreateSupplierAsync);

            LoadLocationEntriesCommand = new RelayCommand(LoadLocationEntries);
            CreateLocationCommand = new RelayCommand(CreateLocationAsync);

            LoadTagEntriesCommand = new RelayCommand(LoadTagEntries);
            CreateTagCommand = new RelayCommand(CreateTagAsync);

        } 

        private void LoadView()
        {
            LoadInitial();
            OnPropertyChanged(nameof(SupplierNames));
            NewSupplierName = "";
            NewAddress = "";
            NewPhoneNumber = "";
            NewLocationId = "";
            NewNote = "";
            NewTagDetail = "";
            NewTagId = "";
        }

        private async void LoadInitial()
        {
            try
            {
                Suppliers = (await _apiService.GetAllSuppliersAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<SupplierDto>, IEnumerable<SupplierEntryViewModel>>(Suppliers);
                SupplierEntries = new(viewModels);

                foreach(var entry in SupplierEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                }
                SupplierNames = SupplierEntries.Select(i=>i.SupplierName).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }

            try
            {
                Locations = (await _apiService.GetAllLocationsAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<LocationDto>, IEnumerable<LocationEntryViewModel>>(Locations);
                LocationEntries = new(viewModels);

                foreach (var entry in LocationEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                }
                LocationIds = LocationEntries.Select(i => i.LocationId).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }

            try
            {
                Tags = (await _apiService.GetAllTagAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<TagDto>, IEnumerable<TagEntryViewModel>>(Tags);
                TagEntries = new(viewModels);

                foreach (var entry in TagEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                }
                TagIds = TagEntries.Select(i => i.TagId).ToList();
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

        private void LoadSupplierEntries()
        {
            try
            {
                FilteredSuppliers = Suppliers;
                if (!String.IsNullOrEmpty(SupplierName))
                {
                    FilteredSuppliers = Suppliers.Where(i => i.SupplierName.Contains(SupplierName)).ToList();
                }
                if (!String.IsNullOrEmpty(Address))
                {
                    FilteredSuppliers = Suppliers.Where(i => i.Address.Contains(Address)).ToList();
                }
                if (!String.IsNullOrEmpty(PhoneNumber))
                {
                    FilteredSuppliers = Suppliers.Where(i => i.PhoneNumber.Contains(PhoneNumber)).ToList();
                }

                var viewModels = _mapper.Map<IEnumerable<SupplierDto>, IEnumerable<SupplierEntryViewModel>>(FilteredSuppliers);
                SupplierEntries = new(viewModels);

                foreach (var entry in SupplierEntries)
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
        private void LoadLocationEntries()
        {
            try
            {
                FilteredLocations = Locations;
                if (!String.IsNullOrEmpty(LocationId))
                {
                    FilteredLocations = Locations.Where(i => i.LocationId.Contains(LocationId)).ToList();
                }
                if (!String.IsNullOrEmpty(Note))
                {
                    FilteredLocations = Locations.Where(i => i.Note.Contains(Note)).ToList();
                }              
                var viewModels = _mapper.Map<IEnumerable<LocationDto>, IEnumerable<LocationEntryViewModel>>(FilteredLocations);
                LocationEntries = new(viewModels);

                foreach (var entry in LocationEntries)
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

        private void LoadTagEntries()
        {
            try
            {
                FilteredTags= Tags;
                if (!String.IsNullOrEmpty(TagId))
                {
                    FilteredTags = Tags.Where(i => i.TagId.Contains(TagId)).ToList();
                }
                if (!String.IsNullOrEmpty(TagDetail))
                {
                    FilteredTags = Tags.Where(i => i.TagDetail.Contains(TagDetail)).ToList();
                }
                var viewModels = _mapper.Map<IEnumerable<TagDto>, IEnumerable<TagEntryViewModel>>(FilteredTags);
                TagEntries = new(viewModels);

                foreach (var entry in TagEntries)
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
        private async void CreateSupplierAsync()
        {
            var createDto = new SupplierDto(NewSupplierName, NewAddress, NewPhoneNumber);
            try
            {
                await _apiService.CreateSupplier(createDto);
                LoadView();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Tên nhà cung cấp đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo nhà cung cấp mới.");
            }
            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            NewSupplierName = "";
            NewAddress = "";
            NewPhoneNumber = "";
            //LoadManageItemView();
        }
        private async void CreateLocationAsync()
        {
            var createDto = new LocationDto(NewLocationId,NewNote);
            try
            {
                await _apiService.CreateLocation(createDto);
                LoadView();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mã vị trí đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo vị trí mới.");
            }
            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            NewLocationId = "";
            NewNote = "";
            //LoadManageItemView();
        }

        private async void CreateTagAsync()
        {
            var createDto = new TagDto(NewTagId, NewTagDetail);
            try
            {
                await _apiService.CreateTagAsync(createDto);
                LoadView();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mã tag đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo tag mới.");
            }
            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            NewTagId = "";
            NewTagDetail = "";
            //LoadManageItemView();
        }

    }
}
