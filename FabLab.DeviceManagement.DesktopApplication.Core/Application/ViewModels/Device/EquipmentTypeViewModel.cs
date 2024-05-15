using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Commands;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using NPOI.Util;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.MessageBox;
using Tag = FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes.Tag;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class EquipmentTypeViewModel : BaseViewModel
    {
        public ObservableCollection<ImageEquimentType> ImageEquimentTypes { get; set; } = new();
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;

        private readonly EquipmentTypeStore _equipmentTypeStore;
        public string EquipmentTypeId { get; set; } = "";
        public string EquipmentTypeName { get; set; } = "";
        public ECategory Category { get; set; }

        public bool IsOpenCreateView { get; set; }
        public bool IsOpenFixView { get; set; }
        public bool IsOpenMoreDetailView { get; set; }
        //Thong so
        public ObservableCollection<SpecificationEquimentType> SpecificationEquimentTypes { get; set; } = new();
        //Hinh anh
        public ObservableCollection<ImageBitmap> Pictures { get; set; } = new();

        public ObservableCollection<FileDataBase64EquipmentType> DataPics { get; set; } = new();


        private EquipmentTypeEntryViewModel _SelectedItem;
        public EquipmentTypeEntryViewModel? SelectedItem
        {
            get => _SelectedItem;
            set
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                _SelectedItem = value;
#pragma warning restore CS8601 // Possible null reference assignment.
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    NewEquipmentTypeId = SelectedItem.EquipmentTypeId;
                    NewEquipmentTypeName = SelectedItem.EquipmentTypeName;
                    
                    NewDescription = SelectedItem.Description;
                    NewSpecificationEquimentTypes = SelectedItem.SpecificationEquimentTypes;
                    NewTagStr = string.Join("", SelectedItem.Tags.Select(s => $"#{s}"));
                    switch (SelectedItem.Category)
                    {
                        case ECategory.All:
                            {
                                CategoryStr = "Tất cả";
                                break;
                            }
                        case ECategory.Mechanical:
                            {
                                CategoryStr = "Cơ khí";
                                break;
                            }
                        case ECategory.IoT_Robotics:
                            {
                                CategoryStr = "IoT_Robotics";
                                break;
                            }
                        case ECategory.Automation:
                            {
                                CategoryStr = "Tự động";
                                break;
                            }
                        default: break;
                    }
                }
            }
        }
        //Tag
        public List<TagDto> Tags { get; set; } = new();

       
        public string SelectedTag { get; set; }


        public List<string> TagIds { get; set; } = new();
        public string TagId { get; set; }
        public string TagSelected { get; set; }

        //Create New Equipment
        public string NewEquipmentTypeId { get; set; } = "";
        public string NewEquipmentTypeName { get; set; } = "";
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

        //Update Image
        public List<byte[]> FileData { get; set; } = new();
        public string NewDescription { get; set; } = "";
        public string NewTagStr { get; set; } = "";

        public string[] NewTag { get; set; }

        private List<EquipmentTypeDto> equipmentTypes = new();
        private List<EquipmentTypeDto> filteredEquipmentTypes = new();
        public ObservableCollection<EquipmentTypeEntryViewModel> EquipmentTypeEntries { get; set; } = new();
        

     
        private List<EquipmentTypeDto> equimentTypes = new();
        public List<string> EquipmentTypeIds { get; set; } = new();
        public List<string> EquipmentTypeNames { get; set; } = new();


        public string SearchKeyWord { get; set; }


        //Thong so
        public string NewName { get; set; }
        public string NewValue { get; set; }
        public string NewUnit { get; set; }
        public ObservableCollection<SpecificationEquimentType> NewSpecificationEquimentTypes { get; set; } = new();

        //Picture
        public ObservableCollection<FileDataEquimentType> NewPictures { get; set; } = new();

        //Close or Open SearchView
        public bool IsOpenSearchAdvanceView { get; set; }

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
        //Spinner Create Api
        private bool isBusyCreate;
        public bool IsBusyCreate
        {
            get => isBusyCreate;
            set
            {
                isBusyCreate = value;
                OnPropertyChanged();
            }
        }

        public string NotificationNull { get;set; }

        public ICommand LoadEquipmentTypeEntriesCommand { get; set; }
        public ICommand LoadEquipmentTypeSearchCommand { get; set; }
        public ICommand LoadInitialCommand { get; set; }
        public ICommand LoadEquipmentTypeViewCommand { get; set; }
        public ICommand CreateEquipmentTypeCommand { get; set; }
        public ICommand AddSpecification { get; set; }
        public ICommand DeleteSpecification { get; set; }

        public ICommand DeleteImageCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand OpenCreateViewCommand { get; set; }
        public ICommand CloseCreateViewCommand { get; set; }
        public ICommand CloseFixViewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AddTagCommand { get; set; }
        public ICommand OpenSearchAdvanceViewCommand { get; set; }
        public ICommand CLoseSearchAdvanceViewCommand { get; set; }
        public ICommand CLoseMoreDetailViewCommand { get; set; }
        public ICommand SelectImageFixCommand { get; set; }
        public ICommand DeleteImageFixCommand { get; set; }
        public ICommand SaveImageFixCommand { get; set; }
        public ICommand SaveSpecsFixCommand { get; set; }
        public ICommand DeleteSpecsFixCommand { get; set; }


        public EquipmentTypeViewModel(IApiService apiService, IMapper mapper, EquipmentTypeStore equipmentTypeStore)
        {
            _apiService = apiService;
            _mapper = mapper;
            _equipmentTypeStore = equipmentTypeStore;

            LoadInitialCommand = new RelayCommand(LoadInitial);
            LoadEquipmentTypeEntriesCommand = new RelayCommand(LoadEquipmentTypeEntries);
            LoadEquipmentTypeSearchCommand = new RelayCommand(SearchEquipmentType);
            LoadEquipmentTypeViewCommand = new RelayCommand(LoadEquipmentTypeView);
            CreateEquipmentTypeCommand = new RelayCommand(CreateEquipmentTypeAsync);
            SelectImageCommand = new RelayCommand(SelectImage);
            AddSpecification = new RelayCommand(AddSpec);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            DeleteSpecification = new RelayCommand<SpecificationEquimentType>(execute: DeleteSpec);
            DeleteImageCommand = new RelayCommand<ImageEquimentType>(execute: DeleteImage);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            OpenCreateViewCommand = new RelayCommand(OpenCreateView);
            CloseCreateViewCommand = new RelayCommand(CloseCreateView);
            CloseFixViewCommand = new RelayCommand(CloseFixView);
            SaveCommand = new RelayCommand(SaveAsync);
            AddTagCommand = new RelayCommand(AddTag);
            OpenSearchAdvanceViewCommand = new RelayCommand(OpenSearchView);
            CLoseSearchAdvanceViewCommand = new RelayCommand(CloseSearchView);
            CLoseMoreDetailViewCommand = new RelayCommand(CloseMoreDetailView);
            SelectImageFixCommand = new RelayCommand(SelectFixImage);
            DeleteImageFixCommand = new RelayCommand<ImageBitmap>(execute: DeleteFixImage);
            SaveImageFixCommand = new RelayCommand(SaveFixImage);
            SaveSpecsFixCommand = new RelayCommand(SaveSpecification);
            DeleteSpecsFixCommand = new RelayCommand<SpecificationEquimentType>(execute: DeleteFixSpec);
            IsOpenCreateView = false; 
            IsOpenFixView = false;
            IsOpenSearchAdvanceView = false;
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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void LoadEquipmentTypeView()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            LoadInitial();
            OnPropertyChanged(nameof(EquipmentTypeIds));
            OnPropertyChanged(nameof(EquipmentTypeNames));
            UpdateEquimentType();
            UpdateTag();
            IsOpenCreateView = false;
            SearchKeyWord = "";
            NotificationNull = "";
        }

        

        private async void LoadDetail()
        {
            SpecificationEquimentTypes.Clear();
            DataPics.Clear();
            Pictures.Clear();
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(NewEquipmentTypeId))
                    {
                        var Dto = (await _apiService.GetInformationEquipmenAsync(NewEquipmentTypeId));
                        SpecificationEquimentTypes = new(Dto.Specs);
                        DataPics = new(Dto.Pics);
                        foreach (var pic in DataPics)
                        {
                            if (!String.IsNullOrEmpty(pic.fileData))
                            {
                                Pictures.Add(new ImageBitmap()
                                {
                                    Source = Base64toImage(pic.fileData),
                                    index = Pictures.Count() + 1
                                }) ;
                            }
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
        }
        public byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
        private async void SaveFixImage()
        {
            FileData.Clear();
            foreach(var item in Pictures)
            {
                var a = ConvertBitmapImageToByteArray(item.Source);
                if(a != null)
                {
                    FileData.Add(a);
                }
            }

            FixImageDto fixdto = new FixImageDto(NewEquipmentTypeId, FileData);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    IsBusy = true;
                    await _apiService.FixImageEquipmentTypesAsync(fixdto);
                    IsBusy = false;
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    LoadEquipmentTypeView();
                }
                catch (HttpRequestException)
                {
                    IsBusy = false;
                    
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            NewEquipmentTypeId = "";
            NewEquipmentTypeName = "";
            NewDescription = "";
            TagId = "";
        }
        private void SelectFixImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png;*.webp";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] files = openFileDialog.FileNames;
                foreach (string file in files)
                {           
                    Pictures.Add(new ImageBitmap()
                    {
                        Source = ByteToBitmapImage(System.IO.File.ReadAllBytes(file)),
                        index = Pictures.Count() + 1
                    });
                }

            }

        }

        private void DeleteFixImage(ImageBitmap obj)
        {
            Pictures.Remove(obj);
            var index = 1;
            foreach (var item in Pictures)
            {
                item.index = index;
                index++;
                OnPropertyChanged();
            }
        }
        public BitmapImage ByteToBitmapImage(byte[] image)
        {
            BitmapImage imageSource = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(image))
            {
                stream.Seek(0, SeekOrigin.Begin);
                imageSource.BeginInit();
                imageSource.StreamSource = stream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
            }
            return imageSource;
        }
        //Open and Close View Popup
        private void CloseFixView()
        {
            IsOpenFixView = false;
            SelectedItem = null;
        }

        private void OpenCreateView()
        {
            NewEquipmentTypeId = "";
            NewEquipmentTypeName = "";
            NewDescription = "";
            CategoryStr = "";
            NewCategory = ECategory.All;
            IsOpenCreateView = true;
        }

        private void CloseCreateView()
        {
            IsOpenCreateView = false;
        }

        private void OpenSearchView()
        {
            IsOpenSearchAdvanceView = true;
            CategoryStr = "";
            TagId = "";
        }
        private void CloseSearchView()
        {
            IsOpenSearchAdvanceView = false;
        }
        private void Entry_IsOpenMoreDetailView()
        {
            IsOpenMoreDetailView = true;
            LoadDetail();
        }
        private void CloseMoreDetailView()
        {
            IsOpenMoreDetailView = false;
        }

        //Open fix view
        private void Entry_IsOpenFixView()
        {
            IsOpenFixView = true;
            
            TagId = NewTagStr;
        }

        //

        private async void LoadInitial()
        {

            Category = ECategory.All;

            EquipmentTypeId = "";
            EquipmentTypeName = "";
            NewName = "";
            NewValue = "";
            NewUnit = "";
            NewSpecificationEquimentTypes = new();
            NewPictures = new();
            ImageEquimentTypes = new();

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
                        entry.SetCategoryEquipment();
                        entry.IsOpenFixView += Entry_IsOpenFixView;
                        entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;


                    }
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

       
        private async void SearchEquipmentType()
        {
            NotificationNull = "";
           
                try
                {
                if (!String.IsNullOrEmpty(SearchKeyWord))
                {
                    filteredEquipmentTypes = (await _apiService.GetEquipmentTypesRecordsAsync(SearchKeyWord)).ToList();
                }
                else filteredEquipmentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();

                if (filteredEquipmentTypes.Count > 0)
                    {
                        var viewModels = _mapper.Map<IEnumerable<EquipmentTypeDto>, IEnumerable<EquipmentTypeEntryViewModel>>(filteredEquipmentTypes);
                        EquipmentTypeEntries = new(viewModels);

                        foreach (var entry in EquipmentTypeEntries)
                        {
                            entry.SetApiService(_apiService);
                            entry.SetMapper(_mapper);
                            entry.Updated += LoadInitial;
                            entry.OnException += Error;
                            entry.SetCategoryEquipment();

                            entry.IsOpenFixView += Entry_IsOpenFixView;
                            entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;
                        }
                    }
                    else
                    {
                        EquipmentTypeEntries.Clear();
                        NotificationNull = "Không tìm thấy loại thiết bị!";

                    }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            
            //else filteredEquipmentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();
        }
        private async void LoadEquipmentTypeEntries()
        {
            NotificationNull = "";
            try
            {
                if(!String.IsNullOrEmpty(CategoryStr) || !String.IsNullOrEmpty(EquipmentTypeId)|| !String.IsNullOrEmpty(EquipmentTypeName)
                    || !String.IsNullOrEmpty(TagId))
                {
                    if (TagId.Contains("#"))
                    {
                        NewTag = TagId.Split("#").Skip(1).ToArray();
                    }
                    else NewTag = NewTag;

                    filteredEquipmentTypes = (await _apiService.GetEquipmentTypesRecordsAsync(EquipmentTypeId, EquipmentTypeName, CategoryStr, NewTag)).ToList();
                }
                else filteredEquipmentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();



                if (filteredEquipmentTypes.Count > 0)
                {
                    var viewModels = _mapper.Map<IEnumerable<EquipmentTypeDto>, IEnumerable<EquipmentTypeEntryViewModel>>(filteredEquipmentTypes);
                    EquipmentTypeEntries = new(viewModels);

                    foreach (var entry in EquipmentTypeEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                        entry.SetCategoryEquipment();

                        entry.IsOpenFixView += Entry_IsOpenFixView;
                        entry.IsOpenMoreDetailView += Entry_IsOpenMoreDetailView;
                    }
                    EquipmentTypeId = "";
                    EquipmentTypeName = "";
                    TagId = "";
                    TagSelected = "";
                    Category = ECategory.All;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    NewTag = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                }
                else
                {
                    EquipmentTypeEntries.Clear();
                    NotificationNull = "Không tìm thấy loại thiết bị!";
                    TagId = "";
                    TagSelected = "";
                }
                IsOpenSearchAdvanceView = false;

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

        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png;*.webp";
            openFileDialog.FilterIndex = 1;
            int index = 0;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] files = openFileDialog.FileNames;
                foreach (string file in files)
                {

                    index++;
                    ImageEquimentTypes.Add(new ImageEquimentType()
                    {
                        ImagePath = file,
                        ImageName = "Picture " + $"{index}",

                    });
                    NewPictures.Add(new FileDataEquimentType()
                    {
                        fileData = System.IO.File.ReadAllBytes(file)

                    });


                }
            }
        }
        private void DeleteImage(ImageEquimentType obj)
        {
            ImageEquimentTypes.Remove(obj);
            var index = 0;
            foreach (var item in ImageEquimentTypes)
            {
                item.ImageName = "Picture " + $"{index}";
                index++;
                OnPropertyChanged();
            }
        }

        private async void SaveSpecification()
        {
            FixSpecificationDto fixdto = new FixSpecificationDto(NewEquipmentTypeId, SpecificationEquimentTypes);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    IsBusy = true;
                    await _apiService.FixSpecificationEquipmentTypesAsync(fixdto);
                    IsBusy = false;
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    LoadEquipmentTypeView();
                }
                catch (HttpRequestException)
                {
                    IsBusy = false;
                    
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            
        }
        public void AddSpec()
        {
            NewSpecificationEquimentTypes = new(); 
            

            if (!String.IsNullOrEmpty(NewName) || !String.IsNullOrEmpty(NewUnit) || !String.IsNullOrEmpty(NewValue))
            {
                SpecificationEquimentTypes.Add(new SpecificationEquimentType()
                {
                    name = NewName,
                    value = NewValue,
                    unit = NewUnit
                });
                NewSpecificationEquimentTypes.Add(new SpecificationEquimentType()
                {
                    name = NewName,
                    value = NewValue,
                    unit = NewUnit
                }


            );
            }
            else MessageBox.Show("Vui lòng nhập đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            NewName = "";
            NewValue = "";
            NewUnit = "";
        }

        private void DeleteSpec(SpecificationEquimentType obj)
        {
            if(NewSpecificationEquimentTypes.Count()!=0) NewSpecificationEquimentTypes.Remove(obj);

            
        }
        private void DeleteFixSpec(SpecificationEquimentType obj)
        {
            

            if (SpecificationEquimentTypes.Count() != 0) SpecificationEquimentTypes.Remove(obj);
        }



        private async void CreateEquipmentTypeAsync()
        {
            if (!String.IsNullOrEmpty(NewEquipmentTypeId)|| !String.IsNullOrEmpty(NewEquipmentTypeName)||
                !String.IsNullOrEmpty(NewDescription))
            {
                if (NewCategory != ECategory.All)
                {
                       
                    NewTag = TagId.Split("#").Skip(1).ToArray();

                    var createDto = new CreateEquimentTypeDto(
                        NewEquipmentTypeId,
                        NewEquipmentTypeName,
                        NewDescription,
                        NewCategory,
                        NewTag,
                        NewPictures,
                        NewSpecificationEquimentTypes);


                    try
                    {
                        IsBusyCreate = true;
                        await _apiService.CreateEquipmentType(createDto);
                        IsBusyCreate = false;
                        LoadEquipmentTypeView();
                    }
                    catch (HttpRequestException)
                    {
                        IsBusyCreate = false;
                        ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                        
                    }
                    catch (DuplicateEntityException)
                    {   IsBusyCreate = false;
                        ShowErrorMessage("Đã có lỗi xảy ra: Mã vật tư đã tồn tại.");
                    }
                    catch (Exception)
                    {
                        IsBusyCreate = false;
                        ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo vật tư mới.");
                    }
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    NewEquipmentTypeId = "";
                    NewEquipmentTypeName = "";
                    NewDescription = "";
                    TagId = "";
                    IsOpenCreateView = false;
                }
                else MessageBox.Show("Vui lòng chọn lĩnh vực!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
               
            }
                else MessageBox.Show("Vui lòng nhập đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);


        }

        private async void UpdateTag()
        {
            try
            {
                Tags = (await _apiService.GetAllTagAsync()).ToList();
                TagIds = Tags.Select(i=>i.TagId).ToList();
               
               
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }


        private async void SaveAsync()
        {
            NewTag = TagId.Split("#").Skip(1).ToArray();

            EquipmentTypeDto fixdto = new EquipmentTypeDto(NewEquipmentTypeId, NewEquipmentTypeName, NewDescription, NewCategory,NewTag);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    
                    await _apiService.FixEquipmentTypesAsync(fixdto);
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsOpenFixView = false;
                    LoadEquipmentTypeView();
                }
                catch (HttpRequestException)
                {

                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            NewEquipmentTypeId = "";
            NewEquipmentTypeName = "";
            NewDescription = "";
            TagId = "";  
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

        private void AddTag()
        {
            TagId += "#" + TagSelected;
            TagSelected = "";
        }


        private async void UpdateEquimentType()
        {
            try
            {
                equimentTypes = (await _apiService.GetAllEquipmentTypesAsync()).ToList();
                EquipmentTypeIds = equimentTypes.Select(i => i.EquipmentTypeId).ToList();
                EquipmentTypeNames = equimentTypes.Select(i => i.EquipmentTypeName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        
    }
}
