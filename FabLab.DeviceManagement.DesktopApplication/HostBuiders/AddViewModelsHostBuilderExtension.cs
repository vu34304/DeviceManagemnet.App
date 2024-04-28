using FabLab.DeviceManagement.DesktopApplication.Core.Application.Services;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.BorrowReturn;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Device;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Maintenance;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Project;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Supervise;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using FabLab.DeviceManagement.DesktopApplication.Views.Project;
using FabLab.DeviceManagement.DesktopApplication.Views.Supervise;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabLab.DeviceManagement.DesktopApplication.HostBuiders
{
    public static class AddViewModelsHostBuilderExtension
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<DeviceManagementNavigationViewModel>();
                services.AddTransient<DeviceManagementViewModel>();
                services.AddTransient<EquipmentTypeViewModel>();
                services.AddTransient<SupplierLocationTagViewModel>();

                services.AddTransient<ProjectManagementNavigationViewModel>();
                services.AddTransient<ProjectManagementViewModel>();
                services.AddTransient<CreateNewProjectViewModel>();
                services.AddTransient<MessageManagementViewModel>();
                services.AddTransient<MessageManagementEntryViewModel>();

                services.AddTransient<BorrowReturnNavigationViewModel>();
                services.AddTransient<CreateNewLendRequestViewModel>();
                services.AddTransient<CreateNewReturnRequestViewModel>();
                services.AddTransient<CreateNewLendRequestEntryViewModel>();

                services.AddTransient<MaintenanceNavigationViewModel>();
                services.AddTransient<EquipmentMaintenanceViewModel>();
                services.AddTransient<EquipmentTypeMaintenanceViewModel>();
                services.AddTransient<SupplierLocationMaintenanceViewModel>();

                services.AddTransient<FablabSuperviseViewModel>();
                services.AddTransient<ViamLabSuperviseViewModel>();

                services.AddTransient<MainViewModel>();
                services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            });

            return host;
        }
    }
}
