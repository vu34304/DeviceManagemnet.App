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
    public class MessageManagementEntryViewModel : BaseViewModel
    {
        private IApiService? _apiService;
        private IMapper? _mapper;

        public int Id { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public string UserName { get; set; }
        public string MSSV { get; set; }
        public bool Onsite { get; set; }
        public string LinkGgDrive { get; set; }
        public bool CheckSeen { get; set; }
        public bool StatusSeen => !CheckSeen;
        public DateTime CreateAt { get; set; }
        public string Equipment { get; set; }


        public event Action? Updated;
        public event Action? OnException;
        public event Action? SeenForm;



        public ICommand SeenFormCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public MessageManagementEntryViewModel()
        {
            SeenFormCommand = new RelayCommand(Seen);
            DeleteCommand = new RelayCommand(Delete);
        }
        public MessageManagementEntryViewModel(int id, string email, string projectName, string userName, string mSSV, bool onsite, string linkGgDrive, bool checkSeen, DateTime createAt, string equipment) : this()
        {
            Id = id;
            Email = email;
            ProjectName = projectName;
            UserName = userName;
            MSSV = mSSV;
            Onsite = onsite;
            LinkGgDrive = linkGgDrive;
            CheckSeen = checkSeen;
            CreateAt = createAt;
            Equipment = equipment;

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
        private async void Seen()
        {

            SeenFromDto seenFromDto = new SeenFromDto(ProjectName);
            if (_mapper is not null && _apiService is not null)
            {
                try
                {   
                    await _apiService.SeenFormAsync(seenFromDto);                   
                    MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    SeenForm?.Invoke();

                }
                catch (HttpRequestException)
                {
                    OnException?.Invoke();
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }
            }           
            
            Updated?.Invoke();         
        }
        private async void Delete()
        {
            if (_mapper is not null && _apiService is not null)
            {
                try
                {
                    if (MessageBox.Show("Xác nhận xóa", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        await _apiService.DeleteFormAsync(ProjectName);
                        Updated?.Invoke();
                        MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        SeenForm?.Invoke();


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
