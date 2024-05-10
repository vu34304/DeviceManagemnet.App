using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.MessageBox;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class EquipmentTypeEntryViewModel : BaseViewModel
    {
        private IApiService? _apiService;
        private IMapper? _mapper;
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public ECategory Category { get; set; }
        public string Description { get; set; }
        
        public string CategoryStr { get; set; }
        
        public string[] Tags { get; set; }
        //Thong so
        public ObservableCollection<SpecificationEquimentType> SpecificationEquimentTypes { get; set; }
        //Hinh anh
        public ObservableCollection<ImageBitmap> Pictures { get; set; } = new();

        public List<FileDataBase64EquipmentType> DataPics { get; set; } = new();


        

       
        
        public event Action? Updated;
        public event Action? OnException;
        public event Action? IsOpenFixView;
        public event Action? IsOpenMoreDetailView;

        public ICommand DeleteCommand { get; set; }
        public ICommand OpenFixViewCommand { get; set; }
        public ICommand OpenOpenMoreDetailViewCommand { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public EquipmentTypeEntryViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

            DeleteCommand = new RelayCommand(DeleteAsync);
            OpenFixViewCommand = new RelayCommand(OpenFixView);
            OpenOpenMoreDetailViewCommand = new RelayCommand(OpenMoreDetail);
        }
        public EquipmentTypeEntryViewModel(string equipmentTypeId, string equipmentTypeName, ECategory category, string description, string[] tags) : this()
        {
            EquipmentTypeId = equipmentTypeId;
            EquipmentTypeName = equipmentTypeName;
            Category = category;
            Description = description;
            Tags = tags;
        }

        public void SetCategoryEquipment()
        {

            switch (Category)
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
                case ECategory.Automation:
                    {
                        CategoryStr = "Tự động";
                        break;
                    }
                case ECategory.IoT_Robotics:
                    {
                        CategoryStr = "IoT_Robotics";
                        break;
                    }
                default: break;

            }
            
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

        private void OpenFixView()
        {
            IsOpenFixView?.Invoke();
        
        }

        //private async void SaveAsync()
        //{

        //    EquipmentTypeDto fixdto = new EquipmentTypeDto(EquipmentTypeId, EquipmentTypeName, Description, Category);
        //    if (_mapper is not null && _apiService is not null)
        //    {
        //        try
        //        {
        //            await _apiService.FixEquipmentTypesAsync(fixdto);
        //            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        //        }
        //        catch (HttpRequestException)
        //        {
        //            OnException?.Invoke();
        //            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
        //        }
        //    }
        //    Updated?.Invoke();
        //}

        private async void DeleteAsync()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteEquipmentTypeAsync(EquipmentTypeId);
                        
                        Updated?.Invoke();
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        
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

        public BitmapImage Base64toImage(string Base64)
        {
            byte[] binarydata = Convert.FromBase64String(Base64);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binarydata);
            bi.EndInit();
            return bi;
        }

        private void OpenMoreDetail()
        {
            IsOpenMoreDetailView?.Invoke();
        }
    }
}
