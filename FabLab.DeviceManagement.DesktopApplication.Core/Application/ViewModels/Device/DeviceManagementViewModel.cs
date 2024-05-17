using AutoMapper;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
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
using MathNet.Numerics.RootFinding;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class DeviceManagementViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;

        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7).Date;
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        public string YearSelected { get; set; } = "";

        public bool IsOpenCreateNewEquipmentView { get; set; }
        public bool IsOpenSearchAdvanceView { get; set; }
        public bool IsOpenFixView { get; set; }
        public bool IsOpenMoreDetailView { get; set; }
        //Detail
        public ObservableCollection<SpecificationEquimentType> Specifications { get; set; } = new();
        public ObservableCollection<ImageBitmap> Pictures { get; set; } = new();
        public List<FileDataBase64EquipmentType> DataPics { get; set; } = new();
        //
        public DeviceEntryViewModel DeviceEntryViewModel { get; set; }

        private DeviceEntryViewModel _SelectedItem;
        public DeviceEntryViewModel SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if(SelectedItem != null)
                {
                    NewEquipmentId = SelectedItem.EquipmentId;
                    NewEquipmentName = SelectedItem.EquipmentName;
                    NewCodeOfManage = SelectedItem.CodeOfManager;
                    NewYearOfSupply = SelectedItem.YearOfSupply;
                    NewLocationId = SelectedItem.LocationId;
                    NewSupplierName = SelectedItem.SupplierName;
                    NewEquipmentTypeId = SelectedItem.EquipmentTypeId;
                    switch (SelectedItem.Status)
                    {
                        case EStatus.Active:
                            {
                                StatusStr = "Khả dụng";
                                break;
                            }
                        case EStatus.Inactive:
                            {
                                StatusStr = "Đang mượn";
                                break;
                            }
                        case EStatus.NonFunctional:
                            {
                                StatusStr = "Đang hỏng";
                                break;
                            }
                        case EStatus.Maintenance:
                            {
                                StatusStr = "Đang bảo trì";
                                break;
                            }
                            default: { break; }
                    }
                }
            }
        }

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
            get => _StatusStr;
            set
            {
                _StatusStr = value;
                switch (_StatusStr)
                {
                    case "Khả dụng":
                        {
                            Status = EStatus.Active; 
                            NewStatus = EStatus.Active;
                            break;
                        }
                    case "Đang mượn":
                        {
                            Status = EStatus.Inactive;
                            NewStatus = EStatus.Inactive;
                            break;
                        }
                    case "Đang hỏng":
                        {
                            NewStatus = EStatus.NonFunctional;
                            Status = EStatus.NonFunctional;
                            break;
                        }
                    case "Đang bảo trì":
                        {
                            NewStatus = EStatus.Maintenance;

                            Status = EStatus.Maintenance; break;
                        }
                    default: break;
                }
            }
        }
        public ECategory Category   { get; set; }
     
        public string NewEquipmentId { get; set; } = "";
        public string NewEquipmentName { get; set; } = "";
        public DateTime NewYearOfSupply { get; set; } = DateTime.Now.Date;
        public string NewCodeOfManage { get; set; } = "";
        public string NewSupplierName { get; set; } = "";
        public string NewLocationId { get; set; } = "";
        public string NewEquipmentTypeId { get; set; } = "";
        public EStatus NewStatus { get; set; }
        public ECategory NewCategory { get; set; }
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
                            NewCategory = ECategory.All; 
                            Category = ECategory.All;
                            break;
                        }
                    case "Cơ khí":
                        {
                            NewCategory = ECategory.Mechanical; 
                            Category = ECategory.Mechanical;
                            break;
                        }
                    case "Tự động":
                        {
                            NewCategory = ECategory.Automation;
                            Category = ECategory.Automation;
                            break;

                        }
                    case "IoT_Robotics":
                        {
                            NewCategory = ECategory.IoT_Robotics; 
                            Category = ECategory.IoT_Robotics;
                            break;

                        }
                    default: break;
                }
            }
        }
        public string SearchKeyWord { get; set; }

        //Spinner Loading Api
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        //Tag
        public List<TagDto> Tags = new();
        public List<string> TagIds { get; set; } = new();
        public string TagId { get; set; }
        public string TagSelected { get; set; }
        public string[] NewTag { get; set; }

        private List<EquipmentDto> equipments = new();
        private List<EquipmentDto> filteredEquipments = new();
        private List<SupplierDto> suppliers = new();
        private List<LocationDto> locations = new();
        private List<EquipmentTypeDto> equimentTypes = new();

        public DeviceEntryViewModel DeviceEntry { get; set; }
        public ObservableCollection<DeviceEntryViewModel> DeviceEntries { get; set; } = new();


    
        public List<string> EquipmentNames { get; set; } = new();
        public List<string> EquipmentIds { get; set; } = new();
        public List<string> EquipmentTypeIds { get; set; } = new();
        public List<string> EquipmentTypeNames { get; set; } = new();
        public List<string> LocationIds { get; set; } = new();
        public List<string> SupplierNames { get; set; } = new();
        public List<string> CodeOfManagers { get; set; } = new();
        //
        public string NotificationNull { get; set; }    

        public ObservableCollection<string> Years { get; set; } = new();
        public ICommand LoadDeviceEntriesCommand { get; set; }
        public ICommand LoadDeviceEntriesCommand1 { get; set; }
        public ICommand LoadInitialCommand { get; set; }
        public ICommand LoadDeviceManagementViewCommand { get; set; }
        public ICommand CreateEquipmentCommand { get; set; }
        public ICommand DeleteEquipmentCommand { get; set; }
        public ICommand FixEquipmentCommand { get; set; }
        public ICommand OpenCreateNewEquipmentViewCommand { get; set; }
        public ICommand CLoseCreateNewEquipmentViewCommand { get; set; }
        public ICommand OpenSearchAdvanceViewCommand { get; set; }
        public ICommand CLoseSearchAdvanceViewCommand { get; set; }
        public ICommand CLoseFixViewCommand { get; set; }
        public ICommand CLoseMoreDetailViewCommand { get; set; }
        public ICommand AddTagCommand { get; set; }

        //Loading Animation
        public bool IsLoading { get;set; }


        public DeviceManagementViewModel(IApiService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
            Years = new ObservableCollection<string>();
            for (int i = DateTime.Now.Year; i >= 1975; i--)
            {
                Years.Add(i.ToString());
            }

            LoadInitialCommand = new RelayCommand(LoadInitial);
            LoadDeviceEntriesCommand = new RelayCommand(LoadDeviceEntriesAdvance);
            LoadDeviceEntriesCommand1 = new RelayCommand(LoadDeviceEntriesBase);
            LoadDeviceManagementViewCommand = new RelayCommand(LoadDeviceManagementView);
            CreateEquipmentCommand = new RelayCommand(CreateEquipmentAsync);
            DeleteEquipmentCommand = new RelayCommand(DeleteAsync);
            FixEquipmentCommand = new RelayCommand(SaveAsync);
            OpenCreateNewEquipmentViewCommand = new RelayCommand(OpenCreateView);
            CLoseCreateNewEquipmentViewCommand = new RelayCommand(CloseCreateView);
            OpenSearchAdvanceViewCommand = new RelayCommand(OpenSearchView);
            CLoseSearchAdvanceViewCommand = new RelayCommand(CloseSearchView);
            CLoseFixViewCommand = new RelayCommand(CloseFixView);
            CLoseMoreDetailViewCommand = new RelayCommand(CloseMoreDetailView);
            AddTagCommand = new RelayCommand(AddTag);

            apiService.StartLoading += ApiService_StartLoading;
            apiService.FinishLoading += ApiService_FinishLoading;
           

        }
      

        private void ApiService_FinishLoading()
        {
           IsBusy = false;
        }

        private void ApiService_StartLoading()
        {
           IsBusy = true;
        }

        private void OpenCreateView()
        {
            IsOpenCreateNewEquipmentView = true;
            CategoryStr = "";
            StatusStr = "";
            NewEquipmentId = "";
            NewEquipmentName = "";
            NewYearOfSupply = DateTime.Now.Date;
            NewCodeOfManage = "";
            NewLocationId = "";
            NewSupplierName = "";
            NewStatus = EStatus.Active;
            NewEquipmentTypeId = "";
        }
        private void CloseCreateView()
        {
            IsOpenCreateNewEquipmentView = false;
        }
       

        private void OpenSearchView()
        {
            IsOpenSearchAdvanceView = true;
            CategoryStr = "";
            StatusStr = "";
        }
        private void CloseSearchView()
        {
            IsOpenSearchAdvanceView = false;
        }
        private void LoadDeviceManagementView()
        {
            IsLoading = true;
            LoadInitial();

            UpdateSupplier();
            UpdateEquimentTypeId();
            UpdateEquimentTypeName();
            UpdateLocation();
            UpdateTag();

            OnPropertyChanged(nameof(EquipmentIds));
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(EquipmentTypeIds));
            OnPropertyChanged(nameof(EquipmentTypeNames));
            OnPropertyChanged(nameof(LocationIds));
            OnPropertyChanged(nameof(SupplierNames));
            OnPropertyChanged(nameof(Tags));

            IsOpenCreateNewEquipmentView = false;
            IsOpenSearchAdvanceView = false;
            IsLoading = false;

        }

        private async void LoadInitial()
        {
            Category = ECategory.All;

            YearSelected = "";
            EquipmentId = "";
            EquipmentName = "";
            EquipmentTypeId = "";
            EquipmentTypeName = "";
            SearchKeyWord = "";
            NotificationNull = "";
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
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                    entry.IsOpenFixView += Entry_IsOpen;
                    entry.SetStatusEquipment();
                    entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;
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

      
        private async void Entry_IsOpenMoreDetailView()
        {
            Specifications.Clear();
            DataPics.Clear();
            Pictures.Clear();
            IsOpenMoreDetailView = true;
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(NewEquipmentTypeId))
                    {
                        var Dto = (await _apiService.GetInformationEquipmenAsync(NewEquipmentTypeId));
                        Specifications = new(Dto.Specs);
                       
                        DataPics = Dto.Pics;
                        var index = 1;
                        foreach (var pic in DataPics)
                        {
                            if (!String.IsNullOrEmpty(pic.fileData))
                            {
                                Pictures.Add(new ImageBitmap()
                                {
                                    Source = Base64toImage(pic.fileData),
                                    index = index
                                }) ;
                            }
                            index++;
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                
            }
            
        }
        public BitmapImage Base64toImage(string Base64)
        {
            byte[] binarydata = Convert.FromBase64String(Base64);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binarydata);
            bi.EndInit();
            return bi;
        }
        private void CloseMoreDetailView()
        {
            IsOpenMoreDetailView = false;
            
        }
        private void CloseFixView()
        {
            IsOpenFixView = false;
        }

        private void Entry_IsOpen()
        {
            IsOpenFixView = true;
        }

        private async void LoadDeviceEntriesAdvance()
        {
            NotificationNull = "";
            try
            {


                if (CategoryStr != "" || StatusStr != "" || !String.IsNullOrEmpty(EquipmentId)
                   || !String.IsNullOrEmpty(EquipmentName) || !String.IsNullOrEmpty(YearOfSupply)
                   || !String.IsNullOrEmpty(EquipmentTypeId) || TagId != "")
                {
                    if (TagId != null)
                    {
                        NewTag = TagId.Split("#").Skip(1).ToArray();
                    }
                    IsLoading = true;
                    
                    filteredEquipments = (await _apiService.GetEquipmentsRecordsAsync(EquipmentId, EquipmentName, YearOfSupply, EquipmentTypeId, CategoryStr, StatusStr, null)).ToList();
                    IsLoading = false;

                }
                else
                {
                    IsLoading = true;
                    filteredEquipments = (await _apiService.GetAllEquipmentsAsync()).ToList();
                    IsLoading = false;
                }

                
                if (filteredEquipments.Count > 0)

                {
              
                    var viewModels = _mapper.Map<IEnumerable<EquipmentDto>, IEnumerable<DeviceEntryViewModel>>(filteredEquipments);
                    DeviceEntries = new(viewModels);

                    foreach (var entry in DeviceEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                        entry.SetStatusEquipment();
                        entry.IsOpenFixView += Entry_IsOpen;
                        entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;
                    }
                    TagId = "";

                }
                else
                {
                    DeviceEntries.Clear();
                    NotificationNull = "Không tìm thấy thiết bị!";
                    TagId = "";
                    
                }
                CategoryStr = "";
                StatusStr = "";


            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            IsOpenSearchAdvanceView = false;

        }
        private async void LoadDeviceEntriesBase()
        {
            NotificationNull = "";
            try
            {
                filteredEquipments = (await _apiService.GetEquipmentsRecordsAsync(SearchKeyWord)).ToList();
                if (filteredEquipments.Count > 0)
                {
                    var viewModels = _mapper.Map<IEnumerable<EquipmentDto>, IEnumerable<DeviceEntryViewModel>>(filteredEquipments);
                    DeviceEntries = new(viewModels);

                    foreach (var entry in DeviceEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                        entry.SetStatusEquipment();
                        entry.IsOpenFixView += Entry_IsOpen;
                        entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;
                    }
                }
                else
                {
                    DeviceEntries.Clear();
                    NotificationNull = "Không tìm thấy thiết bị!";
                    TagId = "";
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

        private async void CreateEquipmentAsync()
        {
            if (!String.IsNullOrEmpty(NewEquipmentId) || !String.IsNullOrEmpty(NewEquipmentName) || 
                !String.IsNullOrEmpty(NewCodeOfManage) || !String.IsNullOrEmpty(NewLocationId) || !String.IsNullOrEmpty(NewSupplierName) || !String.IsNullOrEmpty(NewEquipmentTypeId))
            {
                var createDto = new CreateEquipmentDto(
                NewEquipmentId,
                NewEquipmentName,
                NewYearOfSupply,
                NewCodeOfManage,
                NewStatus,
                NewLocationId,
                NewSupplierName,
                NewEquipmentTypeId);
                try
                {
                    await _apiService.CreateEquipment(createDto);
                    LoadDeviceManagementView();
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                catch (DuplicateEntityException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
                }
                catch (Exception)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo vật tư mới.");
                }
                MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                NewEquipmentId = "";
                NewEquipmentName = "";
                NewYearOfSupply = DateTime.Now.Date;
                NewCodeOfManage = "";
                NewLocationId = "";
                NewSupplierName = "";
                NewStatus = EStatus.Active;
                NewEquipmentTypeId = "";
                //LoadManageItemView();
            }

            else MessageBox.Show("Cần điền đầy đủ các thông tin! ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
           
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
        private async void DeleteAsync()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteEquipmentAsync(NewEquipmentId);
                        LoadDeviceManagementView();
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else { }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }

        }

        private async void SaveAsync()
        {
            
            FixEquipmentDto fixDto = new FixEquipmentDto(NewEquipmentId, NewEquipmentName, NewYearOfSupply, NewCodeOfManage, NewStatus, NewLocationId, NewSupplierName, NewEquipmentTypeId);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    await _apiService.FixEquipmentAsync(fixDto);
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDeviceManagementView();
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            NewEquipmentId = "";
            NewEquipmentName = "";
            NewYearOfSupply = DateTime.Now.Date;
            NewCodeOfManage = "";
            NewLocationId = "";
            NewSupplierName = "";
            NewEquipmentTypeId = "";
            IsOpenFixView = false;

        }
        private async void UpdateTag()
        {
            try
            {
                Tags = (await _apiService.GetAllTagAsync()).ToList();
                TagIds = Tags.Select(i => i.TagId).ToList();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private void AddTag()
        {
            TagId += "#" + TagSelected;
            TagSelected = "";
        }




    }
}
