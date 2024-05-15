using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Borrowings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.FablabSupervises;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Locations;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Projects;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Returnings;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Suppliers;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.Tags;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Exceptions;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.Equipments;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Models.EquipmentTypes;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Services
{
    public class ApiService : IApiService
    {
        public event Action? StartLoading;
        public event Action? FinishLoading;


        private readonly HttpClient _httpClient;
        private const string serverUrl = "https://equipmentmanagementapi.azurewebsites.net/";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }
        //Equipment
        #region Equipment
        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync()
        {

            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Equipment?pageSize=200&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<EquipmentDto>();
            }
            return equipments;
        }
        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsActive()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/Equipment/Enhanced?pageSize=200&pageNumber=1&Status=0");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<EquipmentDto>();
            }
            return equipments;
        }
        public string EquipmentId = "";
        public string EquipmentName = "";
        public string YearOfSupply = "";
        public string EquipmentTypeId = "";
        public string Status = "";

        public async Task<IEnumerable<EquipmentDto>> GetEquipmentsRecordsAsync(string equipmentId, string equipmentName, string yearOfSupply, string equipmentTypeId, string? category, string? status, string[] Tags)
        {
            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    TagIds = TagIds + $"&TagIds={tag}";
                }
            }
            if (equipmentId != "")
            {
                EquipmentId = $"&EquipmentId={equipmentId}";
            }
            if (equipmentName != "")
            {
                EquipmentName = $"&EquipmentName={equipmentName}";
            }
            if (yearOfSupply != "")
            {
                YearOfSupply = $"&YearOfSupply={yearOfSupply}";
            }
            if (equipmentTypeId != "")
            {
                EquipmentTypeId = $"&EquipmentTypeId={equipmentTypeId}";
            }
            if (category != null)
            {
                switch (category)
                {
                    case "Tất cả":
                        {
                            Category = "";
                            break;
                        }
                    case "Cơ khí":
                        {
                            Category = $"&EquipmentCategory=Mechanical";
                            break;
                        }
                    case "Tự động":
                        {
                            Category = $"&EquipmentCategory=Automation";
                            break;

                        }
                    case "IoT_Robotics":
                        {
                            Category = $"&EquipmentCategory=IoT_Robotics";
                            break;

                        }

                    default: break;
                }

            }
            if (status != null)
            {
                switch (status)
                {
                    case "Khả dụng":
                        {

                            Status = $"&Status=Active";
                            break;
                        }
                    case "Đang mượn":
                        {
                            Status = $"&Status=Inactive";
                            break;
                        }
                    case "Đang hỏng":
                        {
                            Status = $"&Status=NonFunctional";

                            break;
                        }
                    case "Đang bảo trì":
                        {
                            Status = $"&Status=Maintenance";

                            break;
                        }
                    default: break;
                }

            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Equipment/Enhanced?pageSize=200&pageNumber=1" + Status + TagIds + Category + EquipmentTypeId + EquipmentId + EquipmentName);


            response.EnsureSuccessStatusCode(); //equipmentId={equipmentId//
            string responseBody = await response.Content.ReadAsStringAsync();
            EquipmentId = "";
            EquipmentName = "";
            YearOfSupply = "";
            EquipmentTypeId = "";
            Category = "";
            TagIds = "";
            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<EquipmentDto>();
            }
            return equipments;
        }


        public async Task<IEnumerable<EquipmentDto>> GetEquipmentsRecordsAsync(string yearSelected, string equipmentId, string equipmentTypeId, ECategory? category)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Equipment/Search1?equipmentId={equipmentId}&YearOfSupply={yearSelected}&equipmentTypeId={equipmentTypeId}&Category={category}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<EquipmentDto>();
            }
            return equipments;
        }

        public async Task<IEnumerable<EquipmentDto>> GetEquipmentsRecordsAsync(string search)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/Equipment?search={search}&pageSize=200&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<EquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<EquipmentDto>();
            }
            return equipments;
        }

        public async Task CreateEquipment(CreateEquipmentDto equipment)
        {
            var json = JsonConvert.SerializeObject(equipment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Equipment", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }

        public async Task FixEquipmentAsync(FixEquipmentDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Equipment", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEquipmentAsync(string equipmentId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}api/Equipment?id={equipmentId}");

            response.EnsureSuccessStatusCode();
        }
        #endregion Equipment

        //Location
        #region Location

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Location");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<LocationDto>>(responseBody);
            if (responses is null)
            {
                return new List<LocationDto>();
            }
            return responses;
        }

        public async Task CreateLocation(LocationDto location)
        {
            var json = JsonConvert.SerializeObject(location);
            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Location", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }
        public async Task FixLocationAsync(LocationDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Location", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteLocationAsync(string locationId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}/api/Location?locationId={locationId}");

            response.EnsureSuccessStatusCode();
        }
        #endregion Location

        //Supplier
        #region Supplier


        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Supplier");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<SupplierDto>>(responseBody);
            if (responses is null)
            {
                return new List<SupplierDto>();
            }
            return responses;
        }

        public async Task CreateSupplier(SupplierDto supplier)
        {
            var json = JsonConvert.SerializeObject(supplier);
            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Supplier", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }
        public async Task FixSupplierAsync(SupplierDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Supplier", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteSupplierAsync(string supplierName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}api/Supplier?supplierName={supplierName}");

            response.EnsureSuccessStatusCode();
        }
        #endregion Supplier

        //EquipmentType
        # region EquipmentType


        public async Task<IEnumerable<EquipmentTypeDto>> GetAllEquipmentTypesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/EquipmentType");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<EquipmentTypeDto>>(responseBody);
            if (responses is null)
            {
                return new List<EquipmentTypeDto>();
            }
            return responses;
        }

        public string TagIds = "";
        public string EquiqmentTypeId = "";
        public string EquiqmentTypeName = "";
        public string Category = "";
        public async Task<IEnumerable<EquipmentTypeDto>> GetEquipmentTypesRecordsAsync(string equiqmentTypeId, string equiqmentTypeName, string? category, string[] Tags)
        {

            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    TagIds = TagIds + $"&tagIds={tag}";
                }
            }
            if (equiqmentTypeId != "")
            {
                EquiqmentTypeId = $"&equipmentTypeId={equiqmentTypeId}";
            }
            if (equiqmentTypeName != "")
            {
                EquiqmentTypeName = $"&equipmentTypeName={equiqmentTypeName}";
            }
            if (category != null)
            {
                switch (category)
                {
                    case "Tất cả":
                        {
                            Category = $"&Category=All";
                            break;
                        }
                    case "Cơ khí":
                        {
                            Category = $"&Category=Mechanical";
                            break;
                        }
                    case "Tự động":
                        {
                            Category = $"&Category=Automation";
                            break;

                        }
                    case "IoT_Robotics":
                        {
                            Category = $"&Category=IoT_Robotics";
                            break;

                        }

                    default: break;
                }
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/EquipmentType/Enhanced?pageSize=200&pageNumber=1" + TagIds + Category + EquiqmentTypeId + EquiqmentTypeName);
            TagIds = "";
            Category = "";
            EquiqmentTypeName = "";
            EquiqmentTypeId = "";
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<EquipmentTypeDto>>(responseBody);
            if (responses is null)
            {
                return new List<EquipmentTypeDto>();
            }
            return responses;

        }

        public async Task<IEnumerable<EquipmentTypeDto>> GetEquipmentTypesRecordsAsync(string serchKeyWord)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/EquipmentType?search={serchKeyWord}&pageSize=200&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<EquipmentTypeDto>>(responseBody);
            if (responses is null)
            {
                return new List<EquipmentTypeDto>();
            }
            return responses;
        }
        public async Task<InformationEquipmentDto> GetInformationEquipmenAsync(string equipmentTypeId)
        {
            StartLoading?.Invoke();
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/EquipmentType/Information?equipmentTypeId={equipmentTypeId}");
            FinishLoading?.Invoke();
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<InformationEquipmentDto>(responseBody);
            if (responses is null)
            {
                //return new List<InformationEquipmentDto>();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return responses;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task CreateEquipmentType(CreateEquimentTypeDto equipmentType)
        {

            string json = JsonConvert.SerializeObject(equipmentType);

            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}api/EquipmentType", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }

        public async Task FixImageEquipmentTypesAsync(FixImageDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Picture", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task FixSpecificationEquipmentTypesAsync(FixSpecificationDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Specification", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task FixEquipmentTypesAsync(EquipmentTypeDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/EquipmentType", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEquipmentTypeAsync(string equipmentTypeId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}api/EquipmentType?id={equipmentTypeId}");

            response.EnsureSuccessStatusCode();
        }

        #endregion EquipmentType

        //Tag
        #region Tag

        public async Task<IEnumerable<TagDto>> GetAllTagAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Tag?pageSize=100&pageNumber=1 ");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<TagDto>>(responseBody);
            if (responses is null)
            {
                return new List<TagDto>();
            }
            return responses;
        }
        public async Task CreateTagAsync(TagDto tag)
        {
            var json = JsonConvert.SerializeObject(tag);
            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Tag", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }
        public async Task FixTagAsync(TagDto fixDto)
        {
            var json = JsonConvert.SerializeObject(fixDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Tag", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteTagAsync(string tagId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}/api/Tag?tagId={tagId}");

            response.EnsureSuccessStatusCode();
        }
        #endregion Tag

        //Project
        #region Project
        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/Project?pageSize=200&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<ProjectDto>>(responseBody);
            if (responses is null)
            {
                return new List<ProjectDto>();
            }
            return responses;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync(string projectName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/Project?pageSize=200&search={projectName}&year=2024&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<ProjectDto>>(responseBody);
            if (responses is null)
            {
                return new List<ProjectDto>();
            }
            return responses;
        }
        public async Task CreateProject(CreateProjectDto projectDto)
        {
            var json = JsonConvert.SerializeObject(projectDto);
            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Project", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }

        public async Task DeleteProjectAsync(string projectName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}api/Project?id={projectName} ");

            response.EnsureSuccessStatusCode();
        }

        public async Task ApprovedProjectAsync(ApprovedProjectDto approvedProjectDto)
        {
            var json = JsonConvert.SerializeObject(approvedProjectDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}/api/Project/Approve", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task EndProjectAsync(EndProjectDto endProjectDto)
        {
            var json = JsonConvert.SerializeObject(endProjectDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}api/Project/EndPrj", content);
            response.EnsureSuccessStatusCode();
        }

        #endregion Project

        //Borrow
        # region Borrow
        public async Task<IEnumerable<BorrowEquipmentDto>> GetBorrowEquipmentAsync(string projectName)
        {
            if (projectName.Contains("#"))
            {
                projectName = projectName.Replace("#", "%23");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Equipment/Enhanced?pageSize=200&ProjectName={projectName}&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<BorrowEquipmentDto>>(responseBody);
            if (equipments is null)
            {
                return new List<BorrowEquipmentDto>();
            }
            return equipments;
        }
        public async Task<IEnumerable<CreateProjectDto>> GetBorrowEquipment1Async(string projectName)
        {
            if (projectName.Contains("#"))
            {
                projectName = projectName.Replace("#", "%23");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Project?pageSize=200&search={projectName}&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<CreateProjectDto>>(responseBody);
            if (equipments is null)
            {
                return new List<CreateProjectDto>();
            }
            return equipments;
        }
        public async Task CreateLendRequestAsync(CreateBorrowDto createBorrowDto)
        {
            string json = JsonConvert.SerializeObject(createBorrowDto);

            var jsonCamelCase = JsonNamingPolicy.CamelCase.ConvertName(json);

            var content = new StringContent(jsonCamelCase, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{serverUrl}/api/Borrow", content);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ServerSideError>(responseBody);
                    if (error is not null)
                    {
                        switch (error.Code)
                        {
                            case "Domain.EntityDuplication":
                                throw new DuplicateEntityException();
                        }
                    }
                    else
                    {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                        throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                    }
                }
                else
                {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                }
            }
        }

        public async Task<IEnumerable<BorrowDto>> GetBorrowsAsync(string projectName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Borrow?projectName={projectName}&pageSize=20&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<BorrowDto>>(responseBody);
            if (equipments is null)
            {
                return new List<BorrowDto>();
            }
            return equipments;
        }
        public async Task<IEnumerable<CreateBorrowDto>> GetEquipmentFromBorrowIdAsync(string projectName, string borrowId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Borrow?borrowId={borrowId}&pageNumber=1&pageSize=200&projectName={projectName}");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var equipments = JsonConvert.DeserializeObject<IEnumerable<CreateBorrowDto>>(responseBody);
            if (equipments is null)
            {
                return new List<CreateBorrowDto>();
            }
            return equipments;
        }

        public async Task ReturnAsync(ReturnDto returnDto)
        {
            var json = JsonConvert.SerializeObject(returnDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}api/Borrow/return", content);
            response.EnsureSuccessStatusCode();

        }
        #endregion Borrow

        #region FormSubcribe
        public async Task<IEnumerable<FormSubcribeDto>> GetAllFormAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}api/Notifications?pageSize=200&pageNumber=1");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<FormSubcribeDto>>(responseBody);
            if (responses is null)
            {
                return new List<FormSubcribeDto>();
            }
            return responses;
        }

        public async Task SeenFormAsync(SeenFromDto seenFromDto)
        {
            var json = JsonConvert.SerializeObject(seenFromDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{serverUrl}api/Notifications", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFormAsync(string projectName)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{serverUrl}api/Notifications?projectName={projectName} ");

            response.EnsureSuccessStatusCode();

        }
        #endregion FormSubcribe

        public async Task<IEnumerable<WarningNotificationDtos>> GetWarningNotificationAsync(DateTime endDate, DateTime startDate)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Notifications/Warning?endDate={endDate}&startDate={startDate}&pageSize=200&pageNumber=1");
            //HttpResponseMessage response = await _httpClient.GetAsync($"{serverUrl}/api/Notifications/Warning?pageSize=200&pageNumber=1");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var responses = JsonConvert.DeserializeObject<IEnumerable<WarningNotificationDtos>>(responseBody);
            if (responses is null)
            {
                return new List<WarningNotificationDtos>();
            }
            return responses;
        }
    }
}
