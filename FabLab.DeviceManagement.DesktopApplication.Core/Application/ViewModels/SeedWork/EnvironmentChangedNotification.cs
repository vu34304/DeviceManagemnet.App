using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork
{
    public class EnvironmentChangedNotification
    {
        public string? SensorId { get; set; }
        public string? Name { get; set; }
        public object? Value { get; set; }
        public string? Timestamp { get; set; }

        public EnvironmentChangedNotification(string? sensorId, string? name, object? value, string timestamp)
        {
            SensorId = sensorId;
            Name = name;
            Value = value;
            Timestamp = timestamp;
        }
    }
}
