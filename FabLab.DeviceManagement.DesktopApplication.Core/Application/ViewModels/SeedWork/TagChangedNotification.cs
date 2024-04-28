using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork
{
    public class TagChangedNotification
    {
        public string? Topic { get; set; }
        public string? DeviceId { get; set; }
        public object? TimeStamp { get; set; }
        public object? IdleTime { get; set; }
        public object? ShiftTime { get; set; }
        public object? OperationTime { get; set; }
        public object? Oee { get; set; }
    }
}
