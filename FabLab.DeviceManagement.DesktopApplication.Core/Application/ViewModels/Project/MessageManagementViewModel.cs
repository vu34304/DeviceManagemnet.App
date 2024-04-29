using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using Notifications.Wpf.Core;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using System.Diagnostics;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project
{
    public class MessageManagementViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly ISignalRClient _signalRClient;
        public ObservableCollection<MessageManagementEntryViewModel> MessageManagementEntries { get; set; } = new();
        public List<FormSubcribeDto> Forms { get; set; } = new();
        private List<FormSubcribeDto> filteredprojects = new();
        public List<string> FormSubcribes { get; set; } = new();
  
        public string ProjectName { get; set; }
        public FormSubcribeDto FormSubcribeDto { get; set; }

        //CheckSeenForm
        public int SeenForm { get; set; }

        //Open Browser

        public ICommand LoadViewCommand { get; set; }
        public ICommand OpenExcelFormCommand { get; set; }
        public ICommand OpenCheckMailCommand { get; set; }
        public ICommand FilterCommand { get; set; }

        public MessageManagementViewModel(IApiService apiService, IMapper mapper, ISignalRClient signalRClient)
        {
            _apiService = apiService;
            _mapper = mapper;
            _signalRClient = signalRClient;
            signalRClient.FormNotification += SignalRClient_FormNotification;
            LoadViewCommand = new RelayCommand(LoadMessageManagementView);
            OpenExcelFormCommand = new RelayCommand(OpenExcelForm);
            OpenCheckMailCommand = new RelayCommand(OpenCheckMail);
            FilterCommand = new RelayCommand(LoadFormEntries);
        }

        private async void SignalRClient_FormNotification(string obj)
        {
            LoadInitial();
            Forms = (await _apiService.GetAllFormAsync()).ToList();
            SeenForm = (from formSubcribeDto in Forms
                        where formSubcribeDto.CheckSeen == false
                        select formSubcribeDto).Count();

            var notificationManager = new NotificationManager();
            var notificationContent = new NotificationContent
            {
                Title = "Thông báo",
                Message = $"có {SeenForm} form đăng kí mới!",
                Type = NotificationType.Information
            };

            if (SeenForm > 0) await notificationManager.ShowAsync(notificationContent, areaName: "WindowArea", expirationTime: TimeSpan.MaxValue);
            else await notificationManager.CloseAllAsync();
        }

        
        private void Error()
        {
            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
        }

        private async void LoadMessageManagementView()
        {
            LoadInitial();
            Forms = (await _apiService.GetAllFormAsync()).ToList();
            FormSubcribes = Forms.Select(i => i.ProjectName).ToList();
            OnPropertyChanged(nameof(FormSubcribes));
            SeenForm = (from formSubcribeDto in Forms
                        where formSubcribeDto.CheckSeen == false
                        select formSubcribeDto).Count();

            var notificationManager = new NotificationManager();
            var notificationContent = new NotificationContent
            {
                Title = "Thông báo",
                Message = $"có {SeenForm} form đăng kí mới!",
                Type = NotificationType.Information
            };

            if(SeenForm>0) await notificationManager.ShowAsync(notificationContent, areaName: "WindowArea", expirationTime: TimeSpan.MaxValue);
            else await notificationManager.CloseAllAsync(); 



        }
        private void LoadFormEntries()
        {
            try
            {

                if (!String.IsNullOrEmpty(ProjectName))
                {
                    filteredprojects = Forms.Where(i => i.ProjectName.Contains(ProjectName)).ToList();
                }


                var viewModels = _mapper.Map<IEnumerable<FormSubcribeDto>, IEnumerable<MessageManagementEntryViewModel>>(filteredprojects);

                MessageManagementEntries = new(viewModels);
                if (_mapper is not null && _apiService is not null)
                {
                    foreach (var entry in MessageManagementEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                        entry.SeenForm += Entry_SeenForm;
                    }
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private async void LoadInitial()
        {
            try
            {
                Forms = (await _apiService.GetAllFormAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<FormSubcribeDto>, IEnumerable<MessageManagementEntryViewModel>>(Forms);

                MessageManagementEntries = new(viewModels);
                if (_mapper is not null && _apiService is not null)
                {
                    foreach (var entry in MessageManagementEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.OnException += Error;
                        entry.SeenForm += Entry_SeenForm;
                    }
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void Entry_SeenForm()
        {
            Forms = (await _apiService.GetAllFormAsync()).ToList();
            SeenForm = (from formSubcribeDto in Forms
                        where formSubcribeDto.CheckSeen == false
                        select formSubcribeDto).Count();

            var notificationManager = new NotificationManager();
            var notificationContent = new NotificationContent
            {
                Title = "Thông báo",
                Message = $"có {SeenForm} form đăng kí mới!",
                Type = NotificationType.Information

            };

            await notificationManager.ShowAsync(notificationContent, areaName: "WindowArea", expirationTime: TimeSpan.MaxValue);
        }

        private void OpenExcelForm()
        {
            OpenUrl("https://docs.google.com/spreadsheets/d/1R_KJjQH77od9ZNIROhY0pOAEgyweXOiAOY_hpTMDONE/edit#gid=235138832");           
        }
        private void OpenCheckMail()
        {
            OpenUrl("https://mail.google.com");
        }


        private void OpenUrl(string url)
        {
            Process myProcess = new Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = url;
                myProcess.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }


}
