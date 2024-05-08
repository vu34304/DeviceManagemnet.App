using AutoMapper;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Mapping
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<EquipmentDto, Equipment>();
            //CreateMap<EquipmentDto, DeviceEntryViewModel>()
            //    .ForMember(i => i.LocationId, o => o.MapFrom(dto => dto.LocationId.LocationId))
            //    .ForMember(i => i.SupplierName, o => o.MapFrom(dto => dto.SupplierName.SupplierName))
            //    .ForMember(i => i.EquipmentTypeId, o => o.MapFrom(dto => dto.EquipmentTypeId.EquipmentTypeId));
            //    .ForMember(i => i.EquipmentTypeName, o => o.MapFrom(dto => dto.EquipmentTypeId.EquipmentTypeName));
            CreateMap<EquipmentDto, DeviceEntryViewModel>();
            CreateMap<EquipmentTypeDto, EquipmentTypeEntryViewModel>();
            
            CreateMap<SupplierDto, SupplierEntryViewModel>();
            CreateMap<LocationDto, LocationEntryViewModel>();
            CreateMap<TagDto, TagEntryViewModel>(); 
            CreateMap<ProjectDto, ProjectManagementEntryViewModel>();
            CreateMap<BorrowEquipmentDto, CreateNewLendRequestEntryViewModel>();
            CreateMap<FormSubcribeDto, MessageManagementEntryViewModel>();

        }
    }
}
