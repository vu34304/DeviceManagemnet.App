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
    public class CreateNewProjectViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        public List<string> EquipmentNames { get; set; } = new();
        public List<EquipmentDto> equipments { get; set; } = new();
        public List<EquipmentDto> TempEquipments { get; set; } = new();
        public ObservableCollection<AddBorrowEquipments> BorowEquipments { get; set; } = new();
        public string BorrowEquipmentName { get; set; } = "";
        public string EquipmentName { get; set; } = "";
        //Create New Project
        public string ProjectName { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        public string Description { get; set; } = "";


        private EquipmentDto _SelectedItem;
        public EquipmentDto SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                if (SelectedItem != null)
                {
                    OnPropertyChanged(nameof(SelectedItem));
                    BorrowEquipmentName = SelectedItem.EquipmentName;
                }
            }
        }

        public ICommand LoadCreateProjectViewModelCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }
        public ICommand RemoveEquipmentCommand { get; set; }
        public ICommand DeleteEquipmentCommand { get; set; }
        public ICommand CreateProjectCommand { get; set; }
        public ICommand SearchEquipmentCommand { get; set; }

        public CreateNewProjectViewModel( IApiService apiService)
        {
            _apiService = apiService;
            LoadCreateProjectViewModelCommand = new RelayCommand(LoadCreateProjectView);
            AddEquipmentCommand = new RelayCommand(AddBorrowEquipment);
            RemoveEquipmentCommand = new RelayCommand(RemoveBorrowEquipment);
            CreateProjectCommand = new RelayCommand(CreateProject);
            SearchEquipmentCommand =new RelayCommand(SearchEquipment);
            DeleteEquipmentCommand = new RelayCommand<AddBorrowEquipments>(execute: DeleteBorrowEquipment);
        }

        public void LoadCreateProjectView()
        {
            UpdateEquipmentIds();
            BorowEquipments.Clear();
            OnPropertyChanged(nameof(EquipmentNames));
            OnPropertyChanged(nameof(BorowEquipments));
            EquipmentName = "";
        }
   
        private async void UpdateEquipmentIds()
        {
            try
            {               
                equipments = new List<EquipmentDto>((await _apiService.GetAllEquipmentsActive()).ToList());
                TempEquipments = new List<EquipmentDto>((await _apiService.GetAllEquipmentsActive()).ToList());
                EquipmentNames = equipments.Select(i => i.EquipmentName).ToList();

            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }

        private  void SearchEquipment()
           {
            if (!String.IsNullOrEmpty(EquipmentName))
            {
               
                equipments = TempEquipments.Where(i => i.EquipmentName == EquipmentName).ToList();

            }
            else
            {
                equipments = TempEquipments;
               
            }
        }
        private void AddBorrowEquipment()
        {
            if (!String.IsNullOrEmpty(BorrowEquipmentName))
            {                
                var item = equipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                if (item != null)
                {
                    item.IsChecked = true;
                    BorowEquipments.Add(new()
                    {
                        index = BorowEquipments.Count() +1,
                        name = BorrowEquipmentName,
                        id = item.EquipmentId
                    });
                }
                var temp = TempEquipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                if (temp != null)
                {
                    temp.IsChecked = true;
                }
            }
           

        }

        private void RemoveBorrowEquipment()
        {
            if (!String.IsNullOrEmpty(BorrowEquipmentName))
            {
                var itemToRemove = BorowEquipments.SingleOrDefault(r => r.name == BorrowEquipmentName);
                var a = equipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                if(a != null) 
                { 
                    a.IsChecked = false;
                }
                var temp = TempEquipments.SingleOrDefault(i => i.EquipmentName == BorrowEquipmentName);
                if (temp != null)
                {
                    temp.IsChecked = false;
                }
                if (itemToRemove != null)
                    BorowEquipments.Remove(itemToRemove);

                var index = 0;
                foreach (var item in BorowEquipments)
                {
                    item.index = index;
                    index++;
                    OnPropertyChanged();
                }
            }
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
