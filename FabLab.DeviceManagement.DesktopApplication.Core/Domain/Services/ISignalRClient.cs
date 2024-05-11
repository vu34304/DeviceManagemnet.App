using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services
{
    public interface ISignalRClient
    {
        event Action<string>? OnTagChanged;
        event Action<string>? EnvironmentChanged;
        event Action<string>? DataMachineChanged;
        event Action<string>? FormNotification;
        event Action<string>? StatusNotification;
        Task ConnectAsync();
        Task<List<TagChangedNotification>> GetBufferList();
        Task<List<EnvironmentChangedNotification>> GetBufferEnvironmentList();
        Task<List<DataMachineChangedNotification>> GetBufferMachineDataList();

        Task<object?> GetBufferTimeStamp(string DeviceId);
        Task<object?> GetBufferIdleTime(string DeviceId);
        Task<object?> GetBufferShiftTime(string DeviceId);
        Task<object?> GetBufferOperationTime(string DeviceId);
        Task<object?> GetBufferOee(string DeviceId);
        Task<object?> GetBufferValue(string sensorId);
        Task<object?> GetBufferValueMachine(string machineId);
        bool GetState();

    }
}
