using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn
{
    public class CreateNewLendRequestEntryViewModel : BaseViewModel
    {
        private IMapper? _mapper;
        public string Test { get; set; }

        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public DateTime YearOfSupply { get; set; }
        public string CodeOfManager { get; set; }
        public EStatus Status { get; set; }

        public bool IsAvailable { get; set; } = false;

        public ObservableCollection<BorrowEquipmentDto> BorrowEquipments { get; set; } = new();


        public CreateNewLendRequestEntryViewModel(string equipmentId, string equipmentName, DateTime yearOfSupply, string codeOfManager, EStatus status, bool isBorrow) : this()
        {
            EquipmentId = equipmentId;
            EquipmentName = equipmentName;
            YearOfSupply = yearOfSupply;
            CodeOfManager = codeOfManager;
            Status = status;
            IsAvailable = isBorrow;
        }

        public ICommand AddEquipmentCommand { get; set; }

        public CreateNewLendRequestEntryViewModel()
        {
            AddEquipmentCommand = new RelayCommand(AddEquipment);
        }

        public void SetMapper(IMapper mapper)
        {
            _mapper = mapper;
            OnPropertyChanged();
        }

        public void AddEquipment()
        {
            BorrowEquipments.Add(new()
            {
                EquipmentId = EquipmentId,
                EquipmentName = EquipmentName,
                YearOfSupply = YearOfSupply,
                CodeOfManager = CodeOfManager,
                Status = Status
            });
        }

    }
}
