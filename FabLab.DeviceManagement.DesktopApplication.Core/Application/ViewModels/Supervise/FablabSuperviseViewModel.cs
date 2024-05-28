using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Dtos.FablabSupervises;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Newtonsoft.Json;
using Org.BouncyCastle.Security.Certificates;
using SkiaSharp;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Windows.Media;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Supervise
{
    public class FablabSuperviseViewModel : BaseViewModel
    {
        #region Khai bao bien
        private readonly ISignalRClient _signalRClient;
        private readonly IApiService _apiService;
        //Environment
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Gas { get; set; }
        public double Noise { get; set; }

        //Machine 1
        public DateTime? TimeStamp1 { get; set; }
        public double IdleTime1 { get; set; }
        public double ShiftTime1 { get; set; }
        public double OperationTime1 { get; set; }
        public double Oee1 { get; set; }

        public object Power1 { get; set; }
        public object Speed1 { get; set; }
        public object Vibration1 { get; set; }
        public string MachineStatus1 { get; set; }
        public string MachineStatusColor1 { get; set; }


        //Machine 2
        public DateTime? TimeStamp2 { get; set; }
        public double IdleTime2 { get; set; }
        public double ShiftTime2 { get; set; }
        public double OperationTime2 { get; set; }
        public double Oee2 { get; set; }

        public object Power2 { get; set; }
        public object Speed2 { get; set; }
        public string MachineStatus2 { get; set; }
        public string MachineStatusColor2 { get; set; }
        public object Vibration2 { get; set; }

        //Machine 3
        public DateTime? TimeStamp3 { get; set; }
        public double IdleTime3 { get; set; }
        public double ShiftTime3 { get; set; }
        public double OperationTime3 { get; set; }
        public double Oee3 { get; set; }
        public object Power3 { get; set; }
        public object Speed3 { get; set; }
        public object Vibration3 { get; set; }
        public string MachineStatus3 { get; set; }
        public string MachineStatusColor3 { get; set; }


        //Machine 4
        public DateTime? TimeStamp4 { get; set; }
        public double IdleTime4 { get; set; }
        public double ShiftTime4 { get; set; }
        public double OperationTime4 { get; set; }
        public double Oee4 { get; set; }
        public object Power4 { get; set; }
        public object Speed4 { get; set; }
        public object Vibration4 { get; set; }
        public string MachineStatus4 { get; set; }
        public string MachineStatusColor4 { get; set; }


        //Machine 5
        public DateTime? TimeStamp5 { get; set; }
        public double IdleTime5 { get; set; }
        public double ShiftTime5 { get; set; }
        public double OperationTime5 { get; set; }
        public double Oee5 { get; set; }
        public object Power5 { get; set; }
        public object Speed5 { get; set; }
        public string MachineStatus5 { get; set; }
        public object Vibration5 { get; set; }
        public string MachineStatusColor5 { get; set; }


        //Machine 6
        public DateTime? TimeStamp6 { get; set; }
        public double IdleTime6 { get; set; }
        public double ShiftTime6 { get; set; }
        public double OperationTime6 { get; set; }
        public double Oee6 { get; set; }
        public object Power6 { get; set; }
        public object Speed6 { get; set; }
        public string MachineStatus6 { get; set; }
        public object Vibration6 { get; set; }
        public string MachineStatusColor6 { get; set; }


        //Status Environment
        public string ColorHumidity { get; set; }
        public string ColorTemperature { get; set; }
        public string ColorGas { get; set; }
        public string ColorNoise { get; set; }


        //Open View Machine
        public string OpenViewMachineKB36 { get; set; }
        public string OpenViewMachineTSH1390 { get; set; }
        public string OpenViewMachineERL1330 { get; set; }
        public string OpenViewMachineFRD900S { get; set; }
        public string OpenViewMachineC200 { get; set; }
        public string OpenViewMachineBSM150 { get; set; }
        public int _indexView { get; set; }

        //Spinner Loading Api
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        //Open Alarm View
        public bool IsOpenAlarmView { get; set; }
        //
        //Chart environment
        public IEnumerable<ISeries> Series5 { get; set; }
        public IEnumerable<ISeries> Series6 { get; set; }
        public IEnumerable<ISeries> Series7 { get; set; }
        public IEnumerable<ISeries> Series8 { get; set; }
        //Chart OEE
        public IEnumerable<ISeries> Series { get; set; }//Machine1
        public IEnumerable<ISeries> Series1 { get; set; }//Machine2
        public IEnumerable<ISeries> Series2 { get; set; }//Machine3
        public IEnumerable<ISeries> Series3 { get; set; }//Machine4
        public IEnumerable<ISeries> Series9 { get; set; }//Machine5
        public IEnumerable<ISeries> Series10 { get; set; }//Machine6

        public ICommand LoadFablabSuperviseViewCommand { get; set; }
        public ICommand NextViewCommand { get; set; }

        public ICommand PreviusViewCommand { get; set; }
        public ICommand OpenAlarmViewCommand { get; set; }
        public ICommand CloseAlarmViewCommand { get; set; }
        public ICommand SearchWarningNotificationCommand { get; set; }

        //enable button change view
        public bool IsFisrtView { get; set; }
        public bool IsLastView { get; set; }

        //WarningNotification
        public List<WarningNotificationDtos> WarningNotificationDtos { get; set; } = new();
        public string TextNotification { get; set; }


        public DateTime EndDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);


        //
        #endregion Khai bao bien
        public FablabSuperviseViewModel(ISignalRClient signalRClient, IApiService apiService)
        {
            _signalRClient = signalRClient;
            _apiService = apiService;
            signalRClient.OnTagChanged += SignalRClient_OnTagChanged;
            signalRClient.EnvironmentChanged += SignalRClient_EnvironmentChanged;
            signalRClient.DataMachineChanged += SignalRClient_DataMachineChanged;
            LoadFablabSuperviseViewCommand = new RelayCommand(LoadFablabSuperviseView);
            NextViewCommand = new RelayCommand(NextView);
            PreviusViewCommand = new RelayCommand(PreviusView);
            ColorHumidity = "White";
            ColorTemperature = "White";
            ColorGas = "White";
            ColorNoise = "White";
            OpenAlarmViewCommand = new RelayCommand(OpenAlarmView);
            CloseAlarmViewCommand = new RelayCommand(CloseAlarmView);
            SearchWarningNotificationCommand = new RelayCommand(GetWarningNotification);
            UpdateView(0);
        }

        private void OpenAlarmView()
        {
            IsOpenAlarmView = true;
        }
        private void CloseAlarmView()
        {
            IsOpenAlarmView = false;
        }
        private void SignalRClient_DataMachineChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<DataMachineChangedNotification>(json);
            if (tag != null)
            {
                switch (tag.machineId)
                {
                    case "KB36":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power1 = tag.value;
                                        break;
                                    }
                                case "Speed":
                                    {
                                        Speed1 = tag.value; break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration1 = tag.value; break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus1 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor1 = ConvertStatus(tag.value)[2];
                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    case "TSH1390":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power2 = tag.value; break;
                                    }
                                case "Speed":
                                    {
                                        Speed2 = tag.value; break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration2 = tag.value; break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus2 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor2 = ConvertStatus(tag.value)[2];

                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    case "ERL1330":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power3 = tag.value; break;
                                    }
                                case "Speed":
                                    {
                                        Speed3 = tag.value; break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration3 = tag.value; break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus3 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor3 = ConvertStatus(tag.value)[2];

                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    case "FRD900S":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power4 = tag.value; break;
                                    }
                                case "Speed":
                                    {
                                        Speed4 = (tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration4 = (tag.value); break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus4 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor4 = ConvertStatus(tag.value)[2];
                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    case "C200":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power5 = (tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed5 = (tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration5 = (tag.value); break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus5 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor5 = ConvertStatus(tag.value)[2];
                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    case "BSM150":
                        {
                            switch (tag.name)
                            {
                                case "Power":
                                    {
                                        Power6 = (tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed6 = (tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration6 = (tag.value); break;
                                    }
                                case "MachineStatus":
                                    {
                                        MachineStatus6 = ConvertStatus(tag.value)[1];
                                        MachineStatusColor6 = ConvertStatus(tag.value)[2];
                                        break;
                                    }
                                default: break;

                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private void SignalRClient_EnvironmentChanged(string json)
        {
            var tag = JsonConvert.DeserializeObject<EnvironmentChangedNotification>(json);
            if (tag != null)
            {
                switch (tag.Name)
                {
                    case "Humidity":
                        {
                            Humidity = Convert.ToInt64(tag.Value);
                            UpdateValueHumidity(Humidity);
                            break;
                        }
                    case "Temperature":
                        {
                            Temperature = Convert.ToInt64(tag.Value);
                            UpdateValueTemperature(Temperature);
                            break;
                        }
                    case "Gas":
                        {
                            Gas = Convert.ToInt64(tag.Value);
                            UpdateValueGas(Gas);

                            break;
                        }
                    case "Noise":
                        {
                            Noise = Convert.ToInt64(tag.Value);
                            UpdateValueNoise(Noise);
                            break;
                        }
                    default:

                        break;
                }
            }
        }
        private async void SignalRClient_OnTagChanged(string json)
        {
            Task read = new(() =>
            {
                var tag = JsonConvert.DeserializeObject<TagChangedNotification>(json);

                if (tag != null)
                {
                    switch (tag.DeviceId)
                    {
                        case "KB36":
                            {
                                TimeStamp1 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime1 = (Convert.ToDouble(tag.IdleTime)) / 60;
                                ShiftTime1 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime1 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee1 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine1(IdleTime1, ShiftTime1, OperationTime1, Oee1);
                                break;
                            }

                        case "TSH1390":
                            {
                                TimeStamp2 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime2 = Convert.ToDouble(tag.IdleTime) / 60;
                                ShiftTime2 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime2 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee2 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine2(IdleTime2, ShiftTime2, OperationTime2, Oee2);
                                break;
                            }
                        case "ERL1330":
                            {
                                TimeStamp3 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime3 = Convert.ToDouble(tag.IdleTime) / 60;
                                ShiftTime3 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime3 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee3 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine3(IdleTime3, ShiftTime3, OperationTime3, Oee3);
                                break;
                            }
                        case "FRD900S":
                            {
                                TimeStamp4 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime4 = Convert.ToDouble(tag.IdleTime) / 60;
                                ShiftTime4 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime4 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee4 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine4(IdleTime4, ShiftTime4, OperationTime4, Oee4);
                                break;
                            }
                        case "C200":
                            {
                                TimeStamp5 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime5 = Convert.ToDouble(tag.IdleTime) / 60;
                                ShiftTime5 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime5 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee5 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine5(IdleTime5, ShiftTime5, OperationTime5, Oee5);
                                break;
                            }
                        case "BSM150":
                            {
                                TimeStamp6 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime6 = Convert.ToDouble(tag.IdleTime) / 60;
                                ShiftTime6 = Convert.ToDouble(tag.ShiftTime) / 60;
                                OperationTime6 = Convert.ToDouble(tag.OperationTime) / 60;
                                Oee6 = Convert.ToDouble(tag.Oee);
                                //UpdateValueOEEMachine6(IdleTime6, ShiftTime6, OperationTime6, Oee6);
                                break;
                            }


                        default:
                            break;
                    }

                }
            });
            read.Start();
            await read;


        }

        private string[] ConvertStatus(string value)
        {
            string[] result = new string[2];
            switch (int.Parse(value))
            {
                case 0:
                    {
                        result[0] = "Máy tắt";
                        result[1] = "Red";
                        break;
                    }
                case 1:
                    {
                        result[0] = "Động cơ chạy";
                        result[1] = "Green";
                        break;
                    }
                case 5:
                    {
                        result[0] = "Máy có điện";
                        result[1] = "Blue";
                        break;
                    }
                default:
                    {
                        result[0] = "Null";
                        break;
                    }
            }
            return result;
        }

        //update chart
        #region update value chart
        private async void UpdateValueOEEMachine1(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                    //series.InnerColor = Color.FromArgb();

                }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueOEEMachine2(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series1 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueOEEMachine3(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series2 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueHumidity(double humidity)
        {
            Task Update = new(() =>
            {
                SolidColorPaint solidColorPaint = new SolidColorPaint();
                if (humidity <= 80 && humidity >= 40)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (humidity > 80) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Blue);


                Series5 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(humidity), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
                 series.RelativeOuterRadius = 8;
                 series.RelativeInnerRadius = 8;


             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 85;
                 series.Fill = new SolidColorPaint(new SKColor(100, 181, 246, 90));
             }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueTemperature(double temperature)
        {
            Task Update = new(() =>
            {
                SolidColorPaint solidColorPaint = new SolidColorPaint();
                if (temperature <= 35 && temperature >= 26)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (temperature > 35) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Blue);

                Series6 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(temperature), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
                 series.RelativeOuterRadius = 8;
                 series.RelativeInnerRadius = 8;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 85;
                 series.Fill = new SolidColorPaint(new SKColor(100, 181, 246, 90));
             }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueOEEMachine5(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series9 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueOEEMachine6(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series10 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueGas(double gas)
        {
            Task Update = new(() =>
            {
                SolidColorPaint solidColorPaint = new SolidColorPaint();
                if (gas >= 80)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.Red);
                }

                else solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);

                Series7 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(gas), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
                 series.RelativeOuterRadius = 8;
                 series.RelativeInnerRadius = 8;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 85;
                 series.Fill = new SolidColorPaint(new SKColor(100, 181, 246, 90));
             }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueNoise(double noise)
        {
            Task Update = new(() =>
            {
                SolidColorPaint solidColorPaint = new SolidColorPaint();
                if (noise >= 450)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.Red);
                }

                else solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);

                Series8 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(noise), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
                 series.RelativeOuterRadius = 8;
                 series.RelativeInnerRadius = 8;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 85;
                 series.Fill = new SolidColorPaint(new SKColor(100, 181, 246, 90));
             }));
            });
            Update.Start();
            await Update;
        }
        private async void UpdateValueOEEMachine4(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
                var A = Math.Round(IdleTime / ShiftTime, 3);
                var P = Math.Round(OperationTime / IdleTime, 3);
                Oee = Math.Round(Oee, 5);

                Series3 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(1, series => SetStyle("Q", series)),
                new GaugeItem(P, series => SetStyle("P", series)),
                new GaugeItem(A, series => SetStyle("A", series)),
                new GaugeItem(Oee, series => SetStyle("OEE", series)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.DataLabelsSize = 30;
                    series.InnerRadius = 20;
                }));
            });
            Update.Start();
            await Update;
        }
        #endregion update value chart
        //

        private async void LoadFablabSuperviseView()
        {
            IsBusy = true;
            await Task.WhenAll(
                UpdateValueEnvironment(),
                UpdateOee(),
                UpdateDataMachine());
            IsBusy = false;

            StartDate = DateTime.Now.AddDays(-30);
            EndDate = DateTime.Now;
            GetWarningNotification();
        }

        private async void GetWarningNotification()
        {
            WarningNotificationDtos.Clear();
            TextNotification = "";
            WarningNotificationDtos = (await _apiService.GetWarningNotificationAsync(EndDate, StartDate)).ToList();
            if (WarningNotificationDtos.Count() == 0)
            {
                TextNotification = "Không có cảnh báo nào!";
            }

        }

        private async Task UpdateDataMachine()
        {
            var tags = await _signalRClient.GetBufferMachineDataList();

            var Machine1 = (from tag in tags where tag.machineId == "KB36" select tag).ToList();
            var Machine2 = (from tag in tags where tag.machineId == "TSH1390" select tag).ToList();
            var Machine3 = (from tag in tags where tag.machineId == "ERL1330" select tag).ToList();
            var Machine4 = (from tag in tags where tag.machineId == "FRD900S" select tag).ToList();
            var Machine5 = (from tag in tags where tag.machineId == "C200" select tag).ToList();
            var Machine6 = (from tag in tags where tag.machineId == "BSM150" select tag).ToList();

            if (Machine1 != null)
            {
                var Speed = Machine1.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed1 = Speed.value;
                else Speed1 = "null";

                var Vibration = Machine1.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration1 = Vibration.value;
                else Vibration1 = "null";

                var Power = Machine1.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power1 = Power.value;
                else Power1 = "null";

                var MachineStatus = Machine1.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus1 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor1 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus1 = "null";
            }

            if (Machine2 != null)
            {
                var Speed = Machine2.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed2 = Speed.value;
                else Speed2 = "null";

                var Vibration = Machine2.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration2 = Vibration.value;
                else Vibration2 = "null";

                var Power = Machine2.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power2 = Power.value;
                else Power2 = "null";

                var MachineStatus = Machine2.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus2 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor2 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus2 = "null";
            }

            if (Machine3 != null)
            {
                var Speed = Machine3.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed3 = Speed.value;
                else Speed3 = "null";

                var Vibration = Machine3.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration3 = Vibration.value;
                else Vibration3 = "null";

                var Power = Machine3.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power3 = Power.value;
                else Power3 = "null";

                var MachineStatus = Machine3.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus3 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor3 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus3 = "null";

            }

            if (Machine4 != null)
            {
                var Speed = Machine4.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed4 = Speed.value;
                else Speed4 = "null";

                var Vibration = Machine4.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration4 = Vibration.value;
                else Vibration4 = "null";

                var Power = Machine4.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power4 = Power.value;
                else Power4 = "null";

                var MachineStatus = Machine4.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus4 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor4 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus4 = "null";
            }

            if (Machine5 != null)
            {
                var Speed = Machine5.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed5 = Speed.value;
                else Speed5 = "null";

                var Vibration = Machine5.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration5 = Vibration.value;
                else Vibration5 = "null";

                var Power = Machine5.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power5 = Power.value;
                else Power5 = "null";

                var MachineStatus = Machine5.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus5 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor5 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus5 = "null";
            }
            if (Machine6 != null)
            {
                var Speed = Machine6.SingleOrDefault(i => i.name == "Speed");
                if (Speed != null) Speed6 = Speed.value;
                else Speed6 = "null";

                var Vibration = Machine6.SingleOrDefault(i => i.name == "Vibration");
                if (Vibration != null) Vibration6 = Vibration.value;
                else Vibration6 = "null";

                var Power = Machine6.SingleOrDefault(i => i.name == "Power");
                if (Power != null) Power6 = Power.value;
                else Power6 = "null";

                var MachineStatus = Machine6.SingleOrDefault(i => i.name == "MachineStatus");
                if (MachineStatus != null)
                {
                    MachineStatus6 = ConvertStatus(MachineStatus.value)[0];
                    MachineStatusColor6 = ConvertStatus(MachineStatus.value)[1];
                }
                else MachineStatus6 = "null";
            }

        }
        private async Task UpdateOee()
        {
            var tags = await _signalRClient.GetBufferList();

            var Machine1 = tags.LastOrDefault(i => i.DeviceId == "KB36");
            var Machine2 = tags.LastOrDefault(i => i.DeviceId == "TSH1390");
            var Machine3 = tags.LastOrDefault(i => i.DeviceId == "ERL1330");
            var Machine4 = tags.LastOrDefault(i => i.DeviceId == "FRD900S");
            var Machine5 = tags.LastOrDefault(i => i.DeviceId == "C200");
            var Machine6 = tags.LastOrDefault(i => i.DeviceId == "BSM150");

            if (Machine1 != null)
            {
                IdleTime1 = Convert.ToDouble(Machine1.IdleTime) / 60;
                ShiftTime1 = Convert.ToDouble(Machine1.ShiftTime) / 60;
                OperationTime1 = Convert.ToDouble(Machine1.OperationTime) / 60;
                Oee1 = Convert.ToDouble(Machine1.Oee);
                UpdateValueOEEMachine1(IdleTime1, ShiftTime1, OperationTime1, Oee1);
            }
            if (Machine2 != null)
            {
                IdleTime2 = Convert.ToDouble(Machine2.IdleTime) / 60;
                ShiftTime2 = Convert.ToDouble(Machine2.ShiftTime) / 60;
                OperationTime2 = Convert.ToDouble(Machine2.OperationTime) / 60;
                Oee2 = Convert.ToDouble(Machine2.Oee);
                UpdateValueOEEMachine2(IdleTime2, ShiftTime2, OperationTime2, Oee2);
            }
            if (Machine3 != null)
            {
                IdleTime3 = Convert.ToDouble(Machine3.IdleTime) / 60;
                ShiftTime3 = Convert.ToDouble(Machine3.ShiftTime) / 60;
                OperationTime3 = Convert.ToDouble(Machine3.OperationTime) / 60;
                Oee3 = Convert.ToDouble(Machine3.Oee);
                UpdateValueOEEMachine3(IdleTime3, ShiftTime3, OperationTime3, Oee3);

            }
            if (Machine4 != null)
            {
                IdleTime4 = Convert.ToDouble(Machine4.IdleTime) / 60;
                ShiftTime4 = Convert.ToDouble(Machine4.ShiftTime) / 60;
                OperationTime4 = Convert.ToDouble(Machine4.OperationTime) / 60;
                Oee4 = Convert.ToDouble(Machine4.Oee);
                UpdateValueOEEMachine4(IdleTime4, ShiftTime4, OperationTime4, Oee4);
            }
            if (Machine5 != null)
            {
                IdleTime5 = Convert.ToDouble(Machine5.IdleTime) / 60;
                ShiftTime5 = Convert.ToDouble(Machine5.ShiftTime) / 60;
                OperationTime5 = Convert.ToDouble(Machine5.OperationTime) / 60;
                Oee5 = Convert.ToDouble(Machine5.Oee);
                UpdateValueOEEMachine5(IdleTime5, ShiftTime5, OperationTime5, Oee5);
            }
            if (Machine6 != null)
            {
                IdleTime6 = Convert.ToDouble(Machine6.IdleTime) / 60;
                ShiftTime6 = Convert.ToDouble(Machine6.ShiftTime) / 60;
                OperationTime6 = Convert.ToDouble(Machine6.OperationTime) / 60;
                Oee6 = Convert.ToDouble(Machine6.Oee);
                UpdateValueOEEMachine6(IdleTime6, ShiftTime6, OperationTime6, Oee6);
            }

        }
        private async Task UpdateValueEnvironment()
        {
            var tags = await _signalRClient.GetBufferEnvironmentList();

            var _humidity = tags.LastOrDefault(i => i.Name == "Humidity");
            var _temperature = tags.LastOrDefault(i => i.Name == "Temperature");
            var _gas = tags.LastOrDefault(i => i.Name == "Gas");
            var _noise = tags.LastOrDefault(i => i.Name == "Noise");

            if (_humidity is not null)
            {
                Humidity = Convert.ToInt64(_humidity.Value);
                UpdateValueHumidity(Humidity);
            }
            if (_temperature is not null)
            {
                Temperature = Convert.ToInt64(_temperature.Value);
                UpdateValueTemperature(Temperature);
            }
            if (_gas is not null)
            {
                Gas = Convert.ToInt64(_gas.Value);
                UpdateValueGas(Gas);
            }
            if (_noise is not null)
            {
                Noise = Convert.ToInt64(_noise.Value);
                UpdateValueNoise(Noise);
            }
        }

        public static void SetStyle(string name, PieSeries<ObservableValue> series)
        {
            series.Name = name;
            series.DataLabelsPosition = PolarLabelsPosition.Start;
            series.DataLabelsFormatter =
                    point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue}";
            series.InnerRadius = 20;
            series.RelativeOuterRadius = 3;
            series.RelativeInnerRadius = 3;
        }




        private void NextView()
        {
            if (_indexView >= 0 && _indexView < 5)
            {
                _indexView++;
                UpdateView(_indexView);

            }
        }
        private void PreviusView()
        {
            if (_indexView > 0 && _indexView <= 5)
            {
                _indexView--;
                UpdateView(_indexView);

            }
        }
        private void UpdateView(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        OpenViewMachineKB36 = "Visible";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
                        OpenViewMachineC200 = "Hidden";
                        OpenViewMachineBSM150 = "Hidden";
                        IsFisrtView = true;
                        IsLastView = false;
                        break;
                    }
                case 1:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Visible";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
                        OpenViewMachineC200 = "Hidden";
                        OpenViewMachineBSM150 = "Hidden";
                        IsFisrtView = true;
                        IsLastView = true;
                        break;
                    }
                case 2:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Visible";
                        OpenViewMachineFRD900S = "Hidden";
                        OpenViewMachineC200 = "Hidden";
                        OpenViewMachineBSM150 = "Hidden";
                        IsFisrtView = true;
                        IsLastView = true;
                        break;
                    }
                case 3:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Visible";
                        OpenViewMachineC200 = "Hidden";
                        OpenViewMachineBSM150 = "Hidden";
                        IsFisrtView = true;
                        IsLastView = true;
                        break;
                    }
                case 4:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
                        OpenViewMachineC200 = "Visible";
                        OpenViewMachineBSM150 = "Hidden";
                        IsFisrtView = true;
                        IsLastView = true;
                        break;
                    }
                case 5:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
                        OpenViewMachineC200 = "Hidden";
                        OpenViewMachineBSM150 = "Visible";
                        IsFisrtView = false;
                        IsLastView = true;
                        break;
                    }
            }


        }

    }
}
