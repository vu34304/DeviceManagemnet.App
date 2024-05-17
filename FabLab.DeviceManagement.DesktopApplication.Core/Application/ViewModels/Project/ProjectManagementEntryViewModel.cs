using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string Description { get; set; }
        public bool Approved { get; set; }
        public string ApprovedStr => (Approved) ? "Đã duyệt" : "Chưa duyệt";
        public bool StatusApproved => !Approved;
        public List<BorrowDto> Borrows { get; set; } = new();




        public ApprovedProjectDto ApprovedProject { get; set; }
        public EndProjectDto EndProject { get; set; }
        
        public bool IsFinished  => (RealEndDate == null)? true : false;
        

        public event Action? Updated;
        public event Action? OnException;
        public event Action? ShowEquipment;

        public ICommand ApprovedCommand { get; set; }
        public ICommand EndCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShowEquipmentCommand { get; set; }

        public ProjectManagementEntryViewModel()
        {
            ApprovedCommand = new RelayCommand(ApprovedAsync);
            EndCommand = new RelayCommand(EndAsync);
            DeleteCommand = new RelayCommand(DeleteAsync);
            ShowEquipmentCommand = new RelayCommand(ShowEquipments);
            
        }

        public ProjectManagementEntryViewModel(string projectName, DateTime startDay, DateTime endDay, DateTime? realEndDay, string description, bool approved) : this()
        {
            ProjectName = projectName;
            StartDate = startDay;
            EndDate = endDay;
            RealEndDate = realEndDay;
            Description = description;
            Approved = approved;
        }
        public async void LoadBorrows()
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Borrows = (await _apiService.GetBorrowsAsync(ProjectName)).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                foreach (var item in Borrows)
                {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    if (item.RealReturnedDate == null)
                    {
                        item.Status = "Chưa trả";
                    }
                    else item.Status = "Đã trả";
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                }

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }
        private void ShowEquipments()
        {
            ShowEquipment?.Invoke();
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
                    
                    MessageBox.Show("Vui lòng hoàn thành dự án trước khi xóa!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                   
                    if (MessageBox.Show("Xác nhận duyệt", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.ApprovedProjectAsync(approvedDto);
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
            Updated?.Invoke();
        }

        private async void EndAsync()
        {
            var now = DateTime.Now;
            EndProjectDto endDto = new EndProjectDto(ProjectName, now);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận hoàn thành", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.EndProjectAsync(endDto);
                        Updated?.Invoke();
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else { }
                  
                }
                catch (HttpRequestException)
                {
                    //OnException?.Invoke();
                    
                    MessageBox.Show("Có đơn mượn chưa được trả, vui lòng kiểm tra lại các đơn mượn!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            Updated?.Invoke();
        }
    }
}
