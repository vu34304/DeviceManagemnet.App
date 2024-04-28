using AutoMapper;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.Store;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Services
{
    public class DatabaseSynchronizationService : IDatabaseSynchronizationService
    {
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly EquipmentStore _equipmentStore;
        private readonly LocationStore _locationStore;
        private readonly SupplierStore _supplierStore;
        private readonly TagStore _tagStore;
        private readonly EquipmentTypeStore _equipmentTypeStore;
        private readonly ProjectStore _projectStore;

        public DatabaseSynchronizationService(IApiService apiService, IMapper mapper, EquipmentStore equipmentStore, LocationStore locationStore, SupplierStore supplierStore, TagStore tagStore, EquipmentTypeStore equipmentTypeStore, ProjectStore projectStore)
        {
            _apiService = apiService;
            _mapper = mapper;
            _equipmentStore = equipmentStore;
            _locationStore = locationStore;
            _supplierStore = supplierStore;
            _tagStore = tagStore;
            _equipmentTypeStore = equipmentTypeStore;
            _projectStore = projectStore;
        }

        public async Task SynchronizeEquipmentsData()
        {
            var equipmentDtos = await _apiService.GetAllEquipmentsAsync();
            var equipments = _mapper.Map<IEnumerable<EquipmentDto>, IEnumerable<Equipment>>(equipmentDtos);
            _equipmentStore.SetEquipment(equipments);
        }

        public async Task SynchronizeLocationsData()
        {
            var locationDtos = await _apiService.GetAllLocationsAsync();
            _locationStore.SetLocation(locationDtos);
        }

        public async Task SynchronizeEquipmentTypesData()
        {
            var equipmentTypeDtos = await _apiService.GetAllEquipmentTypesAsync();
            _equipmentTypeStore.SetEquipmentType(equipmentTypeDtos);
        }
       
        public async Task SynchronizeSuppliersData()
        {
            var supplierDtos = await _apiService.GetAllSuppliersAsync();
            _supplierStore.SetSupplier(supplierDtos);   
        }

        public async Task SynchronizeTagsData()
        {
            var tagDtos = await _apiService.GetAllTagAsync();
            _tagStore.SetTag(tagDtos);
        }

        public async Task SynchronizeProjectsData()
        {
            var projectDtos = await _apiService.GetAllProjectsAsync();
            _projectStore.SetProject(projectDtos);
        }

    }
}
