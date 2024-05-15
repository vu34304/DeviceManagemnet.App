using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.MessageBox;


namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class CreateNewLendRequestViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        public ObservableCollection<AddBorrowEquipments> BorrowEquipments { get; set; } =new ObservableCollection<AddBorrowEquipments>();
        public List<BorrowEquipmentDto> BorrowEquipmentDtos { get; set; } = new();

        public ObservableCollection<BorrowEquipmentDto> ListBorrowEquipments { get; set; } = new();

        public string BorrowEquipmentName { get; set; }
        public bool IsReady { get; set; }
        public List<string> ProjectNames { get; set; } = new();
        public string ProjectName { get; set; }
        public List<ProjectDto> projects { get; set; } = new();
        public IEnumerable<ProjectDto> ProjectsFilter { get; set; }
        public IEnumerable<BorrowDto> Borrows { get; set; }
        public bool Approved { get; set; }
        public string NotificationNull { get; set; } = "";
            
        //Create Borrow
        public string BorrowId { get; set; }
        public DateTime BorrowedDate { get; set; } = DateTime.Now;
        public DateTime ReturnedDate { get; set; } = DateTime.Now;
        public string Borrower { get; set; }
        public string Reason { get; set; }
        public bool OnSite { get; set; }



        public List<string> EquipmentNames { get; set; } = new();
        public List<string> EquipmentIds { get; set; } = new();

        private List<EquipmentDto> equipments = new();

        private BorrowEquipmentDto _SelectedItem;

        public BorrowEquipmentDto SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    BorrowEquipmentName = SelectedItem.EquipmentName;
                    if (SelectedItem.Status == EStatus.Active) { IsReady = true; }
                    else IsReady = false;
                }

            }
        }

        public ICommand LoadCreateNewLendRequestViewCommand { get; set; }
        public ICommand FilterEquipmentCommand { get; set; }
        public ICommand AddBorrowEquipmentCommand { get; set; }
        public ICommand RemoveBorrowEquipmentCommand { get; set; }
        public ICommand CreateLendRequestCommand { get; set; }
        public CreateNewLendRequestViewModel(IApiService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
            LoadCreateNewLendRequestViewCommand = new RelayCommand(LoadCreateNewLendRequestView);
            FilterEquipmentCommand = new RelayCommand(FilterEquipment);
            AddBorrowEquipmentCommand = new RelayCommand(AddEquipment);
            RemoveBorrowEquipmentCommand = new RelayCommand(RemoveEquipment);
            CreateLendRequestCommand = new RelayCommand(CreateLendRequest);
        }

        private void LoadCreateNewLendRequestView()
        {
            UpdateEquipment();
            UpdateProjectNames();
            OnPropertyChanged(nameof(ProjectNames));
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(EquipmentIds));
            BorrowId = "";
            BorrowedDate = DateTime.Now;
            ReturnedDate = DateTime.Now;
            Borrower = "";
            Reason = "";
            ProjectName = "";
            Approved = true;
            BorrowEquipmentDtos.Clear();
            BorrowEquipments.Clear();
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
        private async void UpdateEquipment()
        {
            try
            {
                equipments = (await _apiService.GetAllEquipmentsAsync()).ToList();
                EquipmentNames = equipments.Select(i => i.EquipmentName).ToList();
                EquipmentIds = equipments.Select(i => i.EquipmentId).ToList();
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
        private async void FilterEquipment()
        {
           
            if (!String.IsNullOrEmpty(ProjectName))
            {
                try
                {
                    Approved = true;
                    NotificationNull = "";
                    BorrowEquipmentDtos = (await _apiService.GetBorrowEquipmentAsync(ProjectName)).ToList();

                    foreach (var item in BorrowEquipmentDtos)
                    {
                        if (item.Status == EStatus.Inactive)
                        {
                            item.IsChecked = false;
                            item.IsUnChecked = false;
                        }
                        else
                        {
                            item.IsUnChecked = true;
                            item.IsChecked = false;
                        }
                    }

                   

                    ProjectsFilter = await _apiService.GetProjectsAsync(ProjectName);
                    foreach(var item in BorrowEquipmentDtos)
                    {
                        switch (item.Status)
                        {
                            case EStatus.Active:
                                {
                                    item.StatusStr = "Khả dụng"; break;
                                }
                            case EStatus.Inactive:
                                {
                                    item.StatusStr = "Đang mượn"; break;
                                }
                            case EStatus.NonFunctional:
                                {
                                    item.StatusStr = "Đang hỏng"; break;
                                }
                            case EStatus.Maintenance:
                                {
                                    item.StatusStr = "Đang bảo trì"; break;
                                }
                            default: break;
                        }
                    }
                    if (BorrowEquipmentDtos.Count() != 0)
                    {
                        BorrowEquipments.Clear();
                        if (ProjectsFilter.Where(i => i.Approved == false).Count() != 0)
                        {
                            MessageBox.Show("Dự án chưa được duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            Approved = false;
                        }

                        
                    }
                    else
                    {
                        NotificationNull = "Không tìm thấy thiết bị!";
                    }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }

            }

            else MessageBox.Show("Vui lòng chọn vào dự án!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

       

        private void AddEquipment()
        {

            if (!String.IsNullOrEmpty(BorrowEquipmentName))
            {
                var equipment = equipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                var a = BorrowEquipmentDtos.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);

                if(a != null)
                {
                    
                    a.IsChecked = true;
                    a.IsUnChecked = false;

                }

                if (equipment != null)
                {
                    equipment.IsChecked = true;
                   
                    BorrowEquipments.Add(new()
                    {
                        index = BorrowEquipments.Count(),
                        id = equipment.EquipmentId,
                        name = BorrowEquipmentName

                    });

                }
            }
            
        }
        private void RemoveEquipment()
        {

            if (!String.IsNullOrEmpty(BorrowEquipmentName))
            {
                var temp = equipments.SingleOrDefault(r => r.EquipmentName == BorrowEquipmentName);
                var a = BorrowEquipmentDtos.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);

                if (a != null)
                {
                    a.IsChecked = false;
                   
                    a.IsUnChecked = true;
                }
             
                var itemToRemove = BorrowEquipments.SingleOrDefault(r => r.name == BorrowEquipmentName);
                if (itemToRemove != null)
                BorrowEquipments.Remove(itemToRemove);

                var index = 0;
                foreach (var item in BorrowEquipments)
                {                  
                    item.index = index;
                    index++;
                    OnPropertyChanged();
                }
            }
           
        }
        private async void CreateLendRequest()
        {
            var createDto = new CreateBorrowDto(
                BorrowId,
                BorrowedDate,
                ReturnedDate,
                Borrower,
                Reason,
                OnSite,
                ProjectName,
                BorrowEquipments.Select(i => i.id).ToList()
           );
            try
            {
                await _apiService.CreateLendRequestAsync(createDto);
                MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                FilterEquipment();
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                Approved = true;

            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Dự án đã tồn tại.");
                Approved = true;

            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo dự án mới.");
                Approved = true;

            }
           
            BorrowEquipmentDtos = (await _apiService.GetBorrowEquipmentAsync(ProjectName)).ToList();
            BorrowEquipments.Clear();
            BorrowId = "";
            BorrowedDate = DateTime.Now;
            ReturnedDate = DateTime.Now;
            Borrower = "";
            Reason = "";
            Approved = true;
            
        }

  



    }
}
