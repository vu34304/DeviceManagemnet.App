using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project
{
    public class ProjectManagementEntryViewModel : BaseViewModel
    {
        private IApiService? _apiService;
        private IMapper? _mapper;

        public string ProjectName { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public DateTime RealEndDay { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
        public bool StatusApproved => !Approved;


        public ApprovedProjectDto ApprovedProject { get; set; }
        public EndProjectDto EndProject { get; set; }

        public event Action? Updated;
        public event Action? OnException;

        public ICommand ApprovedCommand { get; set; }
        public ICommand EndCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ProjectManagementEntryViewModel()
        {
            ApprovedCommand = new RelayCommand(ApprovedAsync);
            EndCommand = new RelayCommand(EndAsync);
            DeleteCommand = new RelayCommand(DeleteAsync);
        }

        public ProjectManagementEntryViewModel(string projectName, DateTime startDay, DateTime endDay, DateTime realEndDay, string description, bool approved) : this()
        {
            ProjectName = projectName;
            StartDay = startDay;
            EndDay = endDay;
            RealEndDay = realEndDay;
            Description = description;
            Approved = approved;
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
        private async void DeleteAsync()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteProjectAsync(ProjectName);
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

        private async void ApprovedAsync()
        {

            ApprovedProjectDto approvedDto = new ApprovedProjectDto(ProjectName, true);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    await _apiService.ApprovedProjectAsync(approvedDto);
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

        private async void EndAsync()
        {

            EndProjectDto endDto = new EndProjectDto(ProjectName, RealEndDay);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    await _apiService.EndProjectAsync(endDto);
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
    }
}
