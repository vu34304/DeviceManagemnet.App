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
using System.Windows.Input;

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

        public string Power1 { get; set; }
        public string Speed1 { get; set; }
        public string Vibration1 { get; set; }

        //Machine 2
        public DateTime? TimeStamp2 { get; set; }
        public double IdleTime2 { get; set; }
        public double ShiftTime2 { get; set; }
        public double OperationTime2 { get; set; }
        public double Oee2 { get; set; }
        public string Power2 { get; set; }
        public string Speed2 { get; set; }
        public string Vibration2 { get; set; }

        //Machine 3
        public DateTime? TimeStamp3 { get; set; }
        public double IdleTime3 { get; set; }
        public double ShiftTime3 { get; set; }
        public double OperationTime3 { get; set; }
        public double Oee3 { get; set; }
        public string Power3 { get; set; }
        public string Speed3 { get; set; }
        public string Vibration3 { get; set; }

        //Machine 4
        public DateTime? TimeStamp4 { get; set; }
        public double IdleTime4 { get; set; }
        public double ShiftTime4 { get; set; }
        public double OperationTime4 { get; set; }
        public double Oee4 { get; set; }
        public string Power4 { get; set; }
        public string Speed4 { get; set; }
        public string Vibration4 { get; set; }


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
                                break;
                            }

                        case "machine2":
                            {
                                TimeStamp2 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime2 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime2 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime2 = Convert.ToDouble(tag.OperationTime);
                                Oee2 = Convert.ToDouble(tag.Oee);
                                break;
                            }
                        case "machine3":
                            {
                                TimeStamp3 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime3 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime3 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime3 = Convert.ToDouble(tag.OperationTime);
                                Oee3 = Convert.ToDouble(tag.Oee);
                                break;
                            }
                        case "machine4":
                            {
                                TimeStamp4 = DateTime.TryParse(Convert.ToString(tag.TimeStamp), out var span) ? span : default;
                                IdleTime4 = Convert.ToDouble(tag.IdleTime);
                                ShiftTime4 = Convert.ToDouble(tag.ShiftTime);
                                OperationTime4 = Convert.ToDouble(tag.OperationTime);
                                Oee4 = Convert.ToDouble(tag.Oee);
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

        private async void UpdateValueOEEMachine1(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {
               
                Series = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(IdleTime, series => SetStyle("IdleTime", series)),
                new GaugeItem(ShiftTime, series => SetStyle("ShiftTime", series)),
                new GaugeItem(OperationTime, series => SetStyle("OperationTime", series)),
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
        private async void UpdateValueOEEMachine2(double IdleTime, double ShiftTime, double OperationTime, double Oee)
        {
            Task Update = new(() =>
            {

                Series1 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(IdleTime, series => SetStyle("IdleTime", series)),
                new GaugeItem(ShiftTime, series => SetStyle("ShiftTime", series)),
                new GaugeItem(OperationTime, series => SetStyle("OperationTime", series)),
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

                Series2 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(IdleTime, series => SetStyle("IdleTime", series)),
                new GaugeItem(ShiftTime, series => SetStyle("ShiftTime", series)),
                new GaugeItem(OperationTime, series => SetStyle("OperationTime", series)),
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
                    solidColorPaint = new SolidColorPaint(SKColors.Green);
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
                    solidColorPaint = new SolidColorPaint(SKColors.Green);
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
                    solidColorPaint = new SolidColorPaint(SKColors.Green);
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
                    solidColorPaint = new SolidColorPaint(SKColors.Green);
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

                Series3 = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(IdleTime, series => SetStyle("IdleTime", series)),
                new GaugeItem(ShiftTime, series => SetStyle("ShiftTime", series)),
                new GaugeItem(OperationTime, series => SetStyle("OperationTime", series)),
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

        private async void LoadFablabSuperviseView()
        {
            IsBusy = true;
            await UpdateOee();
            await UpdateValueEnvironment();
            IsBusy = false;
        }

        private async Task UpdateOee()
        {
            if (_signalRClient != null)
            {
                //Machine 1
                TimeStamp1 = DateTime.TryParse(Convert.ToString((await _signalRClient.GetBufferTimeStamp("KB36"))), out var span) ? span : default;
                IdleTime1 = Convert.ToDouble(await _signalRClient.GetBufferIdleTime("KB36"))/60;
                ShiftTime1 = Convert.ToDouble(await _signalRClient.GetBufferShiftTime("KB36")) / 60;
                OperationTime1 = Convert.ToDouble(await _signalRClient.GetBufferOperationTime("KB36"))/60;
                Oee1 = Convert.ToDouble(await _signalRClient.GetBufferOee("KB36"));
                UpdateValueOEEMachine1(IdleTime1, ShiftTime1, OperationTime1, Oee1);

                ////Machine 2
                TimeStamp2 = DateTime.TryParse(Convert.ToString((await _signalRClient.GetBufferTimeStamp("TSH1390"))), out var span2) ? span2 : default;
                IdleTime2 = Convert.ToDouble(await _signalRClient.GetBufferIdleTime("TSH1390"));
                ShiftTime2 = Convert.ToDouble(await _signalRClient.GetBufferShiftTime("TSH1390"));
                OperationTime2 = Convert.ToDouble(await _signalRClient.GetBufferOperationTime("machTSH1390ine2"));
                Oee2 = Convert.ToDouble(await _signalRClient.GetBufferOee("TSH1390"));
                UpdateValueOEEMachine2(IdleTime2, ShiftTime2, OperationTime2, Oee2);

                ////Machine 3
                TimeStamp3 = DateTime.TryParse(Convert.ToString((await _signalRClient.GetBufferTimeStamp("ERL1330"))), out var span3) ? span3 : default;
                IdleTime3 = Convert.ToDouble(await _signalRClient.GetBufferIdleTime("ERL1330"));
                ShiftTime3 = Convert.ToDouble(await _signalRClient.GetBufferShiftTime("ERL1330"));
                OperationTime3 = Convert.ToDouble(await _signalRClient.GetBufferOperationTime("ERL1330"));
                Oee3 = Convert.ToDouble(await _signalRClient.GetBufferOee("ERL1330"));
                UpdateValueOEEMachine3(IdleTime3, ShiftTime3, OperationTime3, Oee3);


                ////Machine 4
                TimeStamp4 = DateTime.TryParse(Convert.ToString((await _signalRClient.GetBufferTimeStamp("FRD900S"))), out var span4) ? span4 : default;
                IdleTime4 = Convert.ToDouble(await _signalRClient.GetBufferIdleTime("FRD900S"));
                ShiftTime4 = Convert.ToDouble(await _signalRClient.GetBufferShiftTime("FRD900S"));
                OperationTime4 = Convert.ToDouble(await _signalRClient.GetBufferOperationTime("FRD900S"));
                Oee4 = Convert.ToDouble(await _signalRClient.GetBufferOee("FRD900S"));
                UpdateValueOEEMachine4(IdleTime4, ShiftTime4, OperationTime4, Oee4);


            }
        }
        private async Task UpdateValueEnvironment()
        {
           
            if (_signalRClient != null)
            {
                Humidity = Convert.ToInt64(await _signalRClient.GetBufferValue("Humidity"));               
                UpdateValueHumidity(Humidity);
                Temperature = Convert.ToInt64(await _signalRClient.GetBufferValue("Tempurature"));
                UpdateValueTemperature(Temperature);
                Gas = Convert.ToInt64(await _signalRClient.GetBufferValue("Gas"));
                UpdateValueGas(Gas);
                Noise = Convert.ToInt64(await _signalRClient.GetBufferValue("Noise"));
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
            series.RelativeOuterRadius = 20;
            series.RelativeInnerRadius = 20;
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
                        break;
                    }
                case 1:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Visible";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Hidden";
                        break;
                    }
                case 2:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Visible";
                        OpenViewMachineFRD900S = "Hidden";
                        break;
                    }
                case 3:
                    {
                        OpenViewMachineKB36 = "Hidden";
                        OpenViewMachineTSH1390 = "Hidden";
                        OpenViewMachineERL1330 = "Hidden";
                        OpenViewMachineFRD900S = "Visible";
                        break;
                    }

            }
        }

    }
}
