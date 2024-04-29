using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;


namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device
{
    public class SupplierEntryViewModel: BaseViewModel
    {
        private IApiService? _apiService;
        private IMapper? _mapper;
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }



        public event Action? Updated;
        public event Action? OnException;
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SupplierEntryViewModel()
        {
            SaveCommand = new RelayCommand(SaveAsync);
            DeleteCommand = new RelayCommand(DeleteAsync);
        }

        public SupplierEntryViewModel(string supplierName, string address, string phoneNumber): this()
        {
            SupplierName = supplierName;
            Address = address;
            PhoneNumber = phoneNumber;
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
        private async void SaveAsync()
        {

            SupplierDto fixDto = new SupplierDto(SupplierName, Address, PhoneNumber);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    await _apiService.FixSupplierAsync(fixDto);
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (HttpRequestException)
                {
                    OnException?.Invoke();
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
            Updated?.Invoke();
        }

        private async void DeleteAsync()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteSupplierAsync(SupplierName);
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
    }
}
