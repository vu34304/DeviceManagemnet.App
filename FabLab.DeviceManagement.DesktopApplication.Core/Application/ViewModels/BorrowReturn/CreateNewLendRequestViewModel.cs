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
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;


namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class CreateNewLendRequestViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        public ObservableCollection<AddBorrowEquipments> BorrowEquipments { get; set; } = new();
        public List<BorrowEquipmentDto> BorrowEquipmentDtos { get; set; } = new();
        public List<string> BorrowEquipmentNames { get; set; } = new();

        public List<string> BorrowEquipmentNames1 { get; set; } = new();

        public string BorrowEquipmentName { get; set; }
        public bool IsReady { get; set; }
        public List<string> ProjectNames { get; set; } = new();
        public string ProjectName { get; set; }
        public List<ProjectDto> projects { get; set; } = new();
        public IEnumerable<ProjectDto> ProjectsFilter { get; set; }
        public IEnumerable<BorrowDto> Borrows { get; set; }
        public bool Approved { get; set; }

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

        private CreateNewLendRequestEntryViewModel _SelectedItem;
        public CreateNewLendRequestEntryViewModel SelectedItem
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

        public ObservableCollection<CreateNewLendRequestEntryViewModel> EquipmentEntries { get; set; } = new();
        public CreateNewLendRequestEntryViewModel createNewLendRequestEntryViewModel { get; set; }

        public ICommand LoadCreateNewLendRequestViewCommand { get; set; }
        public ICommand FilterEquipmentCommand { get; set; }
        public ICommand AddBorrowEquipmentCommand { get; set; }
        public ICommand CreateLendRequestCommand { get; set; }
        public CreateNewLendRequestViewModel(IApiService apiService, IMapper mapper, CreateNewLendRequestEntryViewModel createNewLendRequestEntryViewModel)
        {
            _apiService = apiService;
            _mapper = mapper;
            LoadCreateNewLendRequestViewCommand = new RelayCommand(LoadCreateNewLendRequestView);
            FilterEquipmentCommand = new RelayCommand(FilterEquipment);
            AddBorrowEquipmentCommand = new RelayCommand(AddEquipment);
            CreateLendRequestCommand = new RelayCommand(CreateLendRequest);
            this.createNewLendRequestEntryViewModel = createNewLendRequestEntryViewModel;
        }

        private void LoadCreateNewLendRequestView()
        {
            UpdateEquipment();
            UpdateProjectNames();
            OnPropertyChanged(nameof(ProjectNames));
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(EquipmentIds));
            BorrowEquipments.Clear();
            EquipmentEntries.Clear();
            BorrowId = "";
            BorrowedDate = DateTime.Now;
            ReturnedDate = DateTime.Now;
            Borrower = "";
            Reason = "";
            ProjectName = "";
            Approved = true;
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
                    BorrowEquipmentDtos = (await _apiService.GetBorrowEquipmentAsync(ProjectName)).ToList();
                    ProjectsFilter = await _apiService.GetProjectsAsync(ProjectName);

                    if (BorrowEquipmentDtos.Count() != 0)
                    {
                        var viewModels = _mapper.Map<IEnumerable<BorrowEquipmentDto>, IEnumerable<CreateNewLendRequestEntryViewModel>>(BorrowEquipmentDtos);
                        EquipmentEntries = new(viewModels);
                        BorrowEquipmentName = "";
                        BorrowEquipments.Clear();
                        if (ProjectsFilter.Where(i => i.Approved == false).Count() != 0)
                        {
                            MessageBox.Show("Dự án chưa được duyệt!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            Approved = false;
                        }
                    }
                    else MessageBox.Show("Dự án chưa đăng kí thiết bị!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

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

                if (equipment != null)
                {
                    BorrowEquipments.Add(new()
                    {
                        index = BorrowEquipments.Count() + 1,
                        id = equipment.EquipmentId,
                        name = BorrowEquipmentName

                    });
                    BorrowEquipmentName = "";
                }

                var item = EquipmentEntries.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                if (item != null) { EquipmentEntries.Remove(item); }

                BorrowEquipmentName = "";
            }
            else MessageBox.Show("Vui lòng chọn vào thiết bị cần đăng kí !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
            catch (DuplicateEntityException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Dự án đã tồn tại.");
            }
            catch (Exception)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Không thể tạo dự án mới.");
            }
            MessageBox.Show("Đã Cập Nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            ProjectName = "";

        }

  



    }
}
