using CommunityToolkit.Mvvm.Input;
using FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.SeedWork;
using FabLab.DeviceManagement.DesktopApplication.Core.Domain.Services;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Newtonsoft.Json;
using SkiaSharp;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Windows.Media;

namespace FabLab.DeviceManagement.DesktopApplication.Core.Application.ViewModels.Supervise
{
    public class FablabSuperviseViewModel : BaseViewModel
    {
        private readonly ISignalRClient _signalRClient;
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

        public double Power1 { get; set; }
        public double Speed1 { get; set; }
        public double Vibration1 { get; set; }

        //Machine 2
        public DateTime? TimeStamp2 { get; set; }
        public double IdleTime2 { get; set; }
        public double ShiftTime2 { get; set; }
        public double OperationTime2 { get; set; }
        public double Oee2 { get; set; }

        public double Power2 { get; set; }
        public double Speed2 { get; set; }
        public double Vibration2 { get; set; }

        //Machine 3
        public DateTime? TimeStamp3 { get; set; }
        public double IdleTime3 { get; set; }
        public double ShiftTime3 { get; set; }
        public double OperationTime3 { get; set; }
        public double Oee3 { get; set; }
        public double Power3 { get; set; }
        public double Speed3 { get; set; }
        public double Vibration3 { get; set; }

        //Machine 4
        public DateTime? TimeStamp4 { get; set; }
        public double IdleTime4 { get; set; }
        public double ShiftTime4 { get; set; }
        public double OperationTime4 { get; set; }
        public double Oee4 { get; set; }
        public double Power4 { get; set; }
        public double Speed4 { get; set; }
        public double Vibration4 { get; set; }


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
        //Chart environment
        public IEnumerable<ISeries> Series5 { get; set; }
        public IEnumerable<ISeries> Series6 { get; set; }
        public IEnumerable<ISeries> Series7 { get; set; }
        public IEnumerable<ISeries> Series8 { get; set; }
        //Chart OEE
        public IEnumerable<ISeries> Series { get; set; }
        public IEnumerable<ISeries> Series1 { get; set; }
        public IEnumerable<ISeries> Series2 { get; set; }
        public IEnumerable<ISeries> Series3 { get; set; }

        public ICommand LoadFablabSuperviseViewCommand { get; set; }
        public ICommand NextViewCommand { get; set; }
        public ICommand PreviusViewCommand { get; set; }
        //enable button change view
        public bool IsFisrtView { get; set; }
        public bool IsLastView { get; set; }
        public FablabSuperviseViewModel(ISignalRClient signalRClient)
        {
            _signalRClient = signalRClient;
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
    
            UpdateView(0);
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
                                        Power1 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed1 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration1 = Convert.ToDouble(tag.value); break;
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
                                        Power2 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed2 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration2 = Convert.ToDouble(tag.value); break;
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
                                        Power3 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed3 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration3 = Convert.ToDouble(tag.value); break;
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
                                        Power4 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Speed":
                                    {
                                        Speed4 = Convert.ToDouble(tag.value); break;
                                    }
                                case "Vibration":
                                    {
                                        Vibration4 = Convert.ToDouble(tag.value); break;
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
                        case "machine1":
                            {
                                TimeStamp1 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime1 = (Convert.ToDouble(tag.IdleTime))/60;
                                ShiftTime1 = Convert.ToDouble(tag.ShiftTime)/60;
                                OperationTime1 = Convert.ToDouble(tag.OperationTime)/60;
                                Oee1 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine1(IdleTime1, ShiftTime1, OperationTime1, Oee1);
                                break;
                            }

                        case "machine2":
                            {
                                TimeStamp2 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime2 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime2 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime2 = Convert.ToDouble(tag.OperationTime);
                                Oee2 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine2(IdleTime2, ShiftTime2, OperationTime2, Oee2);
                                break;
                            }
                        case "machine3":
                            {
                                TimeStamp3 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime3 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime3 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime3 = Convert.ToDouble(tag.OperationTime);
                                Oee3 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine3(IdleTime3, ShiftTime3, OperationTime3, Oee3);
                                break;
                            }
                        case "machine4":
                            {
                                TimeStamp4 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime4 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime4 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime4 = Convert.ToDouble(tag.OperationTime);
                                Oee4 = Convert.ToDouble(tag.Oee);
                                UpdateValueOEEMachine4(IdleTime4, ShiftTime4, OperationTime4, Oee4);
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
                if (humidity <= 90 && humidity >= 80)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (humidity > 90) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Yellow);


                Series5 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(humidity), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;

             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 75;
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
                if (temperature <= 30 && temperature >= 25)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (temperature > 30) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Yellow);

                Series6 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(temperature), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 75;
                 series.Fill = new SolidColorPaint(new SKColor(100, 181, 246, 90));
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
                if (gas <= 90 && gas >= 80)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (gas > 90) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Yellow);

                Series7 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(gas), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 75;
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
                if (noise <= 90 && noise >= 80)
                {
                    solidColorPaint = new SolidColorPaint(SKColors.YellowGreen);
                }
                else if (noise > 90) solidColorPaint = new SolidColorPaint(SKColors.Red);
                else solidColorPaint = new SolidColorPaint(SKColors.Yellow);

                Series8 = GaugeGenerator.BuildSolidGauge(
             new GaugeItem(Convert.ToDouble(noise), series =>
             {
                 series.Fill = solidColorPaint;
                 series.DataLabelsSize = 50;
                 series.DataLabelsPaint = solidColorPaint;
                 series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                 series.InnerRadius = 75;
             }),
             new GaugeItem(GaugeItem.Background, series =>
             {
                 series.InnerRadius = 75;
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
        }

        private async Task UpdateDataMachine()
        {
            var tags = await _signalRClient.GetBufferMachineDataList();
            var Machine1 = (from tag in tags where tag.machineId == "KB36" select tag).ToList();
            var Machine2 = (from tag in tags where tag.machineId == "TSH1390" select tag).ToList();
            var Machine3 = (from tag in tags where tag.machineId == "ERL1330" select tag).ToList();
            var Machine4 = (from tag in tags where tag.machineId == "FRD900S" select tag).ToList();

            if (Machine1 != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Power1 = Convert.ToDouble(Machine1.LastOrDefault(i => i.name == "Power").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Speed1 = Convert.ToDouble(Machine1.LastOrDefault(i => i.name == "Speed").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Vibration1 = Convert.ToDouble(Machine1.LastOrDefault(i => i.name == "Vibration").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            if (Machine2 != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Power2 = Convert.ToDouble(Machine2.LastOrDefault(i => i.name == "Power").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Speed2 = Convert.ToDouble(Machine2.LastOrDefault(i => i.name == "Speed").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Vibration2 = Convert.ToDouble(Machine2.LastOrDefault(i => i.name == "Vibration").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            if (Machine3 != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Power3 = Convert.ToDouble(Machine3.LastOrDefault(i => i.name == "Power").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Speed3 = Convert.ToDouble(Machine3.LastOrDefault(i => i.name == "Speed").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Vibration3 = Convert.ToDouble(Machine3.LastOrDefault(i => i.name == "Vibration").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            if (Machine4 != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Power4 = Convert.ToDouble(Machine4.LastOrDefault(i => i.name == "Power").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Speed4 = Convert.ToDouble(Machine4.LastOrDefault(i => i.name == "Speed").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Vibration4 = Convert.ToDouble(Machine4.LastOrDefault(i => i.name == "Vibration").value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

        }
        private async Task UpdateOee()
        {
            var tags = await _signalRClient.GetBufferList();

            var Machine1 = tags.LastOrDefault(i => i.DeviceId == "KB36");
            var Machine2 = tags.LastOrDefault(i => i.DeviceId == "TSH1390");
            var Machine3 = tags.LastOrDefault(i => i.DeviceId == "ERL1330");
            var Machine4 = tags.LastOrDefault(i => i.DeviceId == "FRD900S");

            if(Machine1 != null)
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
            
        }
        private async Task UpdateValueEnvironment()
        {
            var tags = await _signalRClient.GetBufferEnvironmentList();

            var _humidity = tags.LastOrDefault(i => i.Name == "Humidity");
            var _temperature = tags.LastOrDefault(i => i.Name == "Humidity");
            var _gas = tags.LastOrDefault(i => i.Name == "Humidity");
            var _noise = tags.LastOrDefault(i => i.Name == "Humidity");

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
            if(_indexView >=0 && _indexView < 3)
            {
                _indexView++;
                UpdateView(_indexView);
              
            }   
        }
        private  void PreviusView()
        {
            if (_indexView > 0 && _indexView <= 3)
            {
                _indexView--;
                UpdateView(_indexView);
                
            }
        }
        private void UpdateView(int index)
        {
            switch(index)
            {
                case 0:
                    {
                        OpenViewMachineKB36 = "Visible";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
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
                        IsFisrtView = false;
                        IsLastView = true;
                        break;
                    }

            }

            
        }

    }
}
