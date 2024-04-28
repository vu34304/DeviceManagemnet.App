using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Services;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Supervise;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using LiveChartsCore.Kernel;
using Notifications.Wpf.Core;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public IApiService _apiService;
        public ISignalRClient _signalRClient;

        private readonly IDatabaseSynchronizationService _databaseSynchronizationService;
        public DeviceManagementNavigationViewModel DeviceManagementNavigation { get; set; }
        public ProjectManagementNavigationViewModel ProjectManagementNavigation { get; set; }
        public BorrowReturnNavigationViewModel BorrowReturnNavigation { get; set; }
        public CreateNewLendRequestViewModel CreateNewLendRequest { get; set; }
        public MaintenanceNavigationViewModel MaintenanceNavigation { get; set; }
        public FablabSuperviseViewModel FablabSupervise { get; set; }
        public ViamLabSuperviseViewModel ViamLabSupervise { get; set; }
        public MessageManagementViewModel MessageManagement {  get; set; }
        public MessageManagementEntryViewModel MessageManagementEntry { get;set; }
        public ICommand LoadDataStoreCommand { get; set; }
        public ICommand OpenViewFormCommand { get; set; }
        //Check SeenForm
        public int SeenForm { get; set; }
        public List<FormSubcribeDto> Forms { get; set; } = new();

        //Open Popup
        public int FormIndex { get; set; }
        public string NotificationShow { get; set; }
        public string NotificationMessage { get; set; }


        public MainViewModel(IDatabaseSynchronizationService databaseSynchronizationService,ISignalRClient signalRClient, IApiService apiService, DeviceManagementNavigationViewModel deviceManagementNavigation, ProjectManagementNavigationViewModel projectManagementNavigation, BorrowReturnNavigationViewModel borrowReturnNavigation, CreateNewLendRequestViewModel createNewLendRequest, MaintenanceNavigationViewModel maintenanceNavigation, FablabSuperviseViewModel fablabSupervise, ViamLabSuperviseViewModel viamLabSupervise)
        {
            _databaseSynchronizationService = databaseSynchronizationService;
            _apiService = apiService;
            _signalRClient = signalRClient;
            signalRClient.FormNotification += SignalRClient_FormNotification;
            DeviceManagementNavigation = deviceManagementNavigation;
            ProjectManagementNavigation = projectManagementNavigation;    
            BorrowReturnNavigation = borrowReturnNavigation;
            CreateNewLendRequest = createNewLendRequest;
            MaintenanceNavigation = maintenanceNavigation;
            FablabSupervise = fablabSupervise;
            ViamLabSupervise = viamLabSupervise;
            LoadDataStoreCommand = new RelayCommand(LoadDataStoreAsync);      
        }


        private async void SignalRClient_FormNotification(string obj)
        {
            Forms = (await _apiService.GetAllFormAsync()).ToList();
            SeenForm = (from formSubcribeDto in Forms
                        where formSubcribeDto.CheckSeen == false
                        select formSubcribeDto).Count();
        }


        private async void LoadDataStoreAsync()
        {
            try
            {
                await _signalRClient.ConnectAsync();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            

            await Task.WhenAll(
                _databaseSynchronizationService.SynchronizeLocationsData(),
                _databaseSynchronizationService.SynchronizeEquipmentsData(),
                _databaseSynchronizationService.SynchronizeEquipmentTypesData(),
                _databaseSynchronizationService.SynchronizeSuppliersData(),
                _databaseSynchronizationService.SynchronizeTagsData(),
                _databaseSynchronizationService.SynchronizeProjectsData()
                );
            
        }


    }
}
