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

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class CreateNewReturnRequestViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;
        public List<BorrowDto> Borrows { get; set; } = new();
        public List<string> BorrowIds { get; set; } = new();
        public string BorrowId { get; set; }
        public List<string> ProjectNames { get; set; } = new();
        public string ProjectName { get; set; }
        public DateTime RealReturnDate { get; set; } = DateTime.Now;
        public List<ProjectDto> projects { get; set; } = new();

        //Kiem tra da tim kiem ma don hay chua
        public bool IsSearched { get; set; }
        public ICommand LoadCreateNewReturnRequestViewCommand {  get; set; }    
        public ICommand LoadBorrowIdsCommand {  get; set; }
        public ICommand ReturnRequestCommand {  get; set; }

        public CreateNewReturnRequestViewModel(IApiService apiService) 
        { 
            _apiService = apiService;
            LoadCreateNewReturnRequestViewCommand = new RelayCommand(LoadCreateNewReturnRequestView);
            LoadBorrowIdsCommand = new RelayCommand(LoadBorrows);
            ReturnRequestCommand = new RelayCommand(ReturnRequest);
            IsSearched = false;
        }

        private void LoadCreateNewReturnRequestView()
        {
            UpdateProjectNames();
            OnPropertyChanged(nameof(ProjectNames));
            ProjectName = string.Empty;
            RealReturnDate = DateTime.Now;
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
                BorrowIds = Borrows.Select(i => i.BorrowId).ToList();
                if(BorrowIds.Count > 0)
                {
                    BorrowId = BorrowIds.First();
                }
                IsSearched = true;
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void ReturnRequest()
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
                    IsSearched = true;
                   
                }
                else { }

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }

            BorrowId = "";
        }

    }
}
