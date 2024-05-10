using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using MessageBox = System.Windows.MessageBox;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Returnings;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class CreateNewReturnRequestViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;
        public List<BorrowDto> Borrows { get; set; } = new();
        private BorrowDto _SelectedItem;
        public BorrowDto SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                if (SelectedItem != null)
                {
                   
                    SelectedBorrowId = SelectedItem.BorrowId;
                    BorrowId = SelectedItem.BorrowId;
                }
            }
        }
        public List<CreateBorrowDto> BorrowEquipmentDtos { get; set; } = new();
        public List<string> BorrowEquipments { get; set; } = new();
        public ObservableCollection<AddBorrowEquipments> BorrowEquipment { get; set; } = new ObservableCollection<AddBorrowEquipments>();
        public List<string> BorrowIds { get; set; } = new();
        public string BorrowId { get; set; }
        public List<string> ProjectNames { get; set; } = new();
        public string ProjectName { get; set; }
        public DateTime RealReturnDate { get; set; } = DateTime.Now;
        public List<ProjectDto> projects { get; set; } = new();
        public string SelectedBorrowId { get; set; }
        public bool IsShowBorrowEquipmentView { get; set; } 
        //Kiem tra da tim kiem ma don hay chua
        public bool IsSearched { get; set; }
        public bool IsEnable { get; set; }
        public ICommand LoadCreateNewReturnRequestViewCommand {  get; set; }    
        public ICommand LoadBorrowIdsCommand {  get; set; }
        public ICommand LoadBorrowEquipmentsCommand {  get; set; }
        public ICommand EnableButtonSearchCommand {  get; set; }
        public ICommand ReturnRequestCommand {  get; set; }
        public ICommand ShowBorrowEquipmentViewCommand {  get; set; }
        public ICommand CloseBorrowEquipmentViewCommand {  get; set; }

        public CreateNewReturnRequestViewModel(IApiService apiService) 
        { 
            _apiService = apiService;
            LoadCreateNewReturnRequestViewCommand = new RelayCommand(LoadCreateNewReturnRequestView);
            LoadBorrowIdsCommand = new RelayCommand(LoadBorrows);
            ReturnRequestCommand = new RelayCommand(ReturnRequest);
            EnableButtonSearchCommand = new RelayCommand(EnableButtonSearch);
            LoadBorrowEquipmentsCommand = new RelayCommand(LoadEquipmentBorrows);
            ShowBorrowEquipmentViewCommand = new RelayCommand(OpenShowBorrowEquipment);
            CloseBorrowEquipmentViewCommand = new RelayCommand(CloseBorrowEquipmentView);
            BorrowId = "";
           
        }

        private void LoadCreateNewReturnRequestView()
        {
            UpdateProjectNames();
            OnPropertyChanged(nameof(ProjectNames));
            ProjectName = string.Empty;
            RealReturnDate = DateTime.Now;
            BorrowIds.Clear();
            BorrowEquipments.Clear();
            BorrowEquipment.Clear();
            BorrowId = ""; Borrows.Clear();
        }

        private void CloseBorrowEquipmentView()
        {
            IsShowBorrowEquipmentView = false;
        }
        private void EnableButtonSearch()
        {
            IsEnable = true;
        }
        private async void UpdateProjectNames()
        {
            try
            {
                projects = (await _apiService.GetAllProjectsAsync()).ToList();
                ProjectNames = projects.Select(i => i.ProjectName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void LoadBorrows()
        {
            try
            {
                Borrows = (await _apiService.GetBorrowsAsync(ProjectName)).ToList();
                foreach(var item in Borrows)
                {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    if (item.RealReturnedDate == null)
                    {
                        item.IsReturned = true;
                    }
                    else item.IsReturned = false;
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                }
                BorrowIds = Borrows.Select(i => i.BorrowId).ToList();
                if(BorrowIds.Count > 0)
                {
                    BorrowId = BorrowIds.First();
                }
                IsSearched = true;
                IsEnable = false;
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void OpenShowBorrowEquipment()
        {
            IsShowBorrowEquipmentView = true;
            if (!String.IsNullOrEmpty(ProjectName) && !String.IsNullOrEmpty(SelectedBorrowId))
            {
                BorrowEquipment.Clear();
                BorrowEquipments.Clear();
                try
                {
                    BorrowEquipmentDtos = (await _apiService.GetEquipmentFromBorrowIdAsync(ProjectName, SelectedBorrowId)).ToList();
                    BorrowEquipments = BorrowEquipmentDtos.Select(i => i.Equipments).First().ToList();

                    foreach (var a in BorrowEquipments)
                    {
                        BorrowEquipment.Add(new()
                        {
                            index = BorrowEquipment.Count() + 1,
                            name = a
                        });
                    }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
        }

        private async void LoadEquipmentBorrows()
        {
           
          if (!String.IsNullOrEmpty(ProjectName) && !String.IsNullOrEmpty(SelectedBorrowId))
            {
                BorrowEquipment.Clear();
                BorrowEquipments.Clear();
                try
                {
                    BorrowEquipmentDtos = (await _apiService.GetEquipmentFromBorrowIdAsync(ProjectName, SelectedBorrowId)).ToList();
                    BorrowEquipments = BorrowEquipmentDtos.Select(i => i.Equipments).First().ToList();
                    
                    foreach(var a in BorrowEquipments)
                    {
                        BorrowEquipment.Add(new()
                        {
                            index = BorrowEquipment.Count()+1,                    
                            name = a
                        });
                    }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }
        }

        private async void ReturnRequest()
        {
            if (!String.IsNullOrEmpty(BorrowId))
            {
                var returnDto = new ReturnDto
                {
                    BorrowId = BorrowId,
                    RealReturnedDate = RealReturnDate,
                };
                try
                {
                    if (MessageBox.Show("Xác nhận trả", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.ReturnAsync(returnDto);
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        BorrowIds.Remove(BorrowId);
                        OnPropertyChanged(nameof(BorrowIds));
                        IsSearched = true;

                    }
                    else { }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
                LoadBorrows();
                if(BorrowIds.Count() == 0)
                {
                    LoadCreateNewReturnRequestView();   
                }
         
            }
            else
            {
                MessageBox.Show("Chưa chọn mã đơn mượn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }

    }
}
