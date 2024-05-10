using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Borrows;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using Newtonsoft.Json;
using Notifications.Wpf.Core;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project
{
    public class ProjectManagementViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;



        public List<string> ProjectNames { get; set; } = new();
        public string ProjectName { get; set; }
        public string NotificationNull { get; set; }
        public ObservableCollection<ProjectManagementEntryViewModel> ProjectManagementEntries { get; set; } = new();    
        public List<ProjectDto> projects { get; set; } = new();
        private List<ProjectDto> filteredprojects = new();
        public List<BorrowEquipmentDto> BorrowEquipmentDtos { get; set; } = new();

        private ProjectManagementEntryViewModel _SelectedItem;
        public  ProjectManagementEntryViewModel SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                if(SelectedItem != null)
                {
                    ProjectName = SelectedItem.ProjectName;
                }
            }
        }
        public bool IsShowEquipment { get; set; }
        
        public ICommand LoadProjectManagementViewCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }
        public ICommand LoadProjectEntriesCommand { get; set; }
        public ICommand CloseShowEquipmentCommand { get; set; }
        
        //Danh sachs thiet bij duj an dang ki
         public List<string> Equipments { get; set; } = new();  
         public List<CreateProjectDto> EquipmentDtos { get; set; } = new();
        public ObservableCollection<AddBorrowEquipments> BorrowEquipment { get; set; } = new ObservableCollection<AddBorrowEquipments>();


        public ProjectManagementViewModel(IApiService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
            LoadProjectManagementViewCommand = new RelayCommand(LoadProjectManagementView);
            LoadProjectEntriesCommand = new RelayCommand(LoadProjectEntries);
            CloseShowEquipmentCommand = new RelayCommand(CloseShowEquipment);
            IsShowEquipment = false;
        }
     
        private void LoadProjectManagementView()
        {
            LoadInitial();
            UpdateProjectNames();
            OnPropertyChanged(nameof(ProjectNames));
            
        }

        
        private void CloseShowEquipment()
        {
            IsShowEquipment = false;
        }
        private void Error()
        {
            ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
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

        private async void LoadInitial()
        {
            try
            {
                projects = (await _apiService.GetAllProjectsAsync()).ToList();
                var viewModels = _mapper.Map<IEnumerable<ProjectDto>, IEnumerable<ProjectManagementEntryViewModel>>(projects);

                ProjectManagementEntries = new(viewModels);
                if (_mapper is not null && _apiService is not null)
                {
                    foreach (var entry in ProjectManagementEntries)
                    {
                        entry.SetApiService(_apiService);
                        entry.SetMapper(_mapper);
                        entry.Updated += LoadInitial;
                        entry.LoadBorrows();
                        entry.OnException += Error;
                        entry.ShowEquipment += Entry_ShowEquipment;
                    }
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void LoadProjectEntries()
        {
            try
            {

                if (!String.IsNullOrEmpty(ProjectName))
                {
                    filteredprojects = projects.Where(i => i.ProjectName.Contains(ProjectName)).ToList();
                }


                var viewModels = _mapper.Map<IEnumerable<ProjectDto>, IEnumerable<ProjectManagementEntryViewModel>>(filteredprojects);
                ProjectManagementEntries = new(viewModels);

                foreach (var entry in ProjectManagementEntries)
                {
                    entry.SetApiService(_apiService);
                    entry.SetMapper(_mapper);
                    entry.Updated += LoadInitial;
                    entry.OnException += Error;
                    entry.LoadBorrows();

                    entry.ShowEquipment += Entry_ShowEquipment;
                }
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private async void Entry_ShowEquipment()
        {
            IsShowEquipment = true;
            EquipmentDtos.Clear();
            BorrowEquipment.Clear();
            if (!String.IsNullOrEmpty(ProjectName))
            {
                try
                {
                    EquipmentDtos = (await _apiService.GetBorrowEquipment1Async(ProjectName)).ToList();
                    if (EquipmentDtos.Count() == 0) NotificationNull = "Dự án chưa đăng kí thiết bị!";
                    else 
                    {
                        NotificationNull = "";
                        Equipments=EquipmentDtos.Select(i=>i.equipments).First().ToList();
                        foreach(var equipment in Equipments)
                        {
                            BorrowEquipment.Add(new()
                            {
                                index = BorrowEquipment.Count() + 1,
                                name = equipment
                            });
                        }
                    }

                }
                catch (HttpRequestException)
                {
                    ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
                }

            }
        }
    }
}
