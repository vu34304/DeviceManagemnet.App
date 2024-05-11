using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.Services
{
    public class SignalRClient : ISignalRClient
    {
        private HubConnection connection;
        public event Action<string>? OnTagChanged;
        public event Action<string>? EnvironmentChanged;
        public event Action<string>? DataMachineChanged;
        public event Action<string>? FormNotification;
        public event Action<string>? StatusNotification;


        public SignalRClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://equipmentmanagementapi.azurewebsites.net/notificationHub")
                .WithAutomaticReconnect()
                .Build();
        }
        public async Task ConnectAsync()
        {
            connection.On<string>("OeeChanged", (json) => OnTagChanged?.Invoke(json));
            connection.On<string>("EnvironmentChanged", (json) => EnvironmentChanged?.Invoke(json));
            connection.On<string>("DataMachineChanged", (json) => DataMachineChanged?.Invoke(json));
            connection.On<string>("FormNotification", (json) => FormNotification?.Invoke(json));
            
            await connection.StartAsync();
            var a = connection.State;
        }

        //Get all OEE value
        public async Task<List<TagChangedNotification>> GetBufferList()
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                return new List<TagChangedNotification>();
            }
            return tags;
        }

        //Get all environment value
        public async Task<List<EnvironmentChangedNotification>> GetBufferEnvironmentList()
        {
            var respone = await connection.InvokeAsync<string>("SendAllEnvironment");
            var tags = JsonConvert.DeserializeObject<List<EnvironmentChangedNotification>>(respone);
            if (tags is null)
            {
                return new List<EnvironmentChangedNotification>();
            }
            return tags;
        }

        public async Task<List<DataMachineChangedNotification>> GetBufferMachineDataList()
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineData");
            var tags = JsonConvert.DeserializeObject<List<DataMachineChangedNotification>>(respone);
            if (tags is null)
            {
                return new List<DataMachineChangedNotification>();
            }
            return tags;

        }

        //Get all machinevalue


        #region get customvalue oee
        public async Task<object?> GetBufferTimeStamp(string DeviceId)
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            var tag = tags.LastOrDefault(i => i.DeviceId == DeviceId);
            if (tag is not null)
            {
                return tag.TimeStamp;
            }
            return null;
        }
        public async Task<object?> GetBufferIdleTime(string DeviceId)
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            var tag = tags.LastOrDefault(i => i.DeviceId == DeviceId);
            if (tag is not null)
            {
                return tag.IdleTime;
            }
            return null;
        }
        public async Task<object?> GetBufferShiftTime(string DeviceId)
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            var tag = tags.LastOrDefault(i => i.DeviceId == DeviceId);
            if (tag is not null)
            {
                return tag.ShiftTime;
            }
            return null;
        }
        public async Task<object?> GetBufferOperationTime(string DeviceId)
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            var tag = tags.LastOrDefault(i => i.DeviceId == DeviceId);
            if (tag is not null)
            {
                return tag.OperationTime;
            }
            return null;
        }
        public async Task<object?> GetBufferOee(string DeviceId)
        {
            var respone = await connection.InvokeAsync<string>("SendAllMachineOee");
            var tags = JsonConvert.DeserializeObject<List<TagChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }
            var tag = tags.LastOrDefault(i => i.DeviceId == DeviceId);
            if (tag is not null)
            {
                return tag.Oee;
            }
            return null;
        }
        #endregion get customvalue oee


        public async Task<object?> GetBufferValue(string sensorId)
        {
            string respone = await connection.InvokeAsync<string>("SendAllEnvironment");
            var tags = JsonConvert.DeserializeObject<List<EnvironmentChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }

            var tag = tags.LastOrDefault(i => i.SensorId == sensorId);
            if (tag is not null)
            {
                return tag.Value;
            }
            else return null;
        }
        public async Task<object?> GetBufferValueMachine(string machineId)
        {
            string respone = await connection.InvokeAsync<string>("SendAllMachineData");
            var tags = JsonConvert.DeserializeObject<List<DataMachineChangedNotification>>(respone);
            if (tags is null)
            {
                throw new Exception();
            }

            var tag = tags.LastOrDefault(i => i.machineId == machineId);
            if (tag is not null)
            {
                return tag.value;
            }
            else return null;
        }

        public bool GetState()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                return true;
            }
            else return false;
        }
        
    }
}
