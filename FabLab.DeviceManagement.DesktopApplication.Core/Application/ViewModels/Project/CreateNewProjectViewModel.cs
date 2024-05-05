using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project
{
    public class CreateNewProjectViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;
        public List<string> EquipmentNames { get; set; } = new();
        private List<EquipmentDto> equipments = new();
        public ObservableCollection<AddBorrowEquipments> BorowEquipments { get; set; } = new();
        public string EquipmentName { get; set; }

        public string BorrowEquipmentName { get; set; } = "";
        //Create New Project
        public string ProjectName { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        public string Description { get; set; } = "";




        public ICommand LoadCreateProjectViewModelCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }
        public ICommand DeleteEquipmentCommand { get; set; }
        public ICommand CreateProjectCommand { get; set; }

        public CreateNewProjectViewModel( IApiService apiService)
        {
            _apiService = apiService;
            LoadCreateProjectViewModelCommand = new RelayCommand(LoadCreateProjectView);
            AddEquipmentCommand = new RelayCommand(AddBorrowEquipment);
            CreateProjectCommand = new RelayCommand(CreateProject);
            DeleteEquipmentCommand = new RelayCommand<AddBorrowEquipments>(execute: DeleteBorrowEquipment);
        }

        public void LoadCreateProjectView()
        {
            UpdateEquipmentIds();
            BorowEquipments.Clear();
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(BorowEquipments));
        }
   
        private async void UpdateEquipmentIds()
        {
            try
            {
                equipments = (await _apiService.GetAllEquipmentsActive()).ToList();
                EquipmentNames = equipments.Select(i => i.EquipmentName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private void AddBorrowEquipment()
        {
            if (!String.IsNullOrEmpty(BorrowEquipmentName))
            {
                var item = equipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                foreach (var a in BorowEquipments)
                {
                    if (a.name == BorrowEquipmentName)
                    {
                        MessageBox.Show("Thiết bị đã được chọn !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        item=null;
                    }
                }
                

               if(item != null)
                {
                    BorowEquipments.Add(new()
                    {
                        index = BorowEquipments.Count(),
                        name = BorrowEquipmentName,
                        id = item.EquipmentId
                    });
                    BorrowEquipmentName = "";
                }
               
            }
            else MessageBox.Show("Vui lòng chọn vào thiết bị cần đăng kí !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        private void DeleteBorrowEquipment(AddBorrowEquipments obj)
        {
            BorowEquipments.Remove(obj);
            var index = 0;
            foreach (var item in BorowEquipments)
            {
                item.index = index;
                index++;
                OnPropertyChanged();
            }
        }
        private async void CreateProject()
        {
            var createDto = new CreateProjectDto(
                ProjectName,
                StartDate,
                EndDate,
                Description,
                BorowEquipments.Select(i => i.id).ToList()
           );
            try
            {
                await _apiService.CreateProject(createDto);
                LoadCreateProjectView();

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
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            Description = "";
            BorowEquipments.Clear();
            
        }

    }
}
