using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using JonglaInterview.Helpers;
using JonglaInterview.Models;
using JonglaInterview.Properties;

namespace JonglaInterview.ViewModels
{
    public class JsonService : IDataService
    {
        public event ModelAvailableEventHandler ModelAvailable;

        Task taskRefreshModel = null;
        CancellationTokenSource cancelTokensource = new CancellationTokenSource();
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        public JsonService()
        {}

        public void LoadData()
        {
            if (taskRefreshModel == null)
            {
                taskRefreshModel = new Task(() => { RefreshModel(); });
                taskRefreshModel.Start();
            }
        }

        public void Close()
        {
            cancelTokensource.Cancel();
            Thread.Sleep(100);
            taskRefreshModel = null;
        }

        ~JsonService()
        {
            Close();
        }

        protected virtual void OnModelAvailable(ModelAvailableEventArgs e)
        {
            if (ModelAvailable != null)
            {
                ModelAvailable(e);
            }
        }

        private void RefreshModel()
        {
            WebClient syncClient = new WebClient();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject;

            try
            {
                do
                {
                    try
                    {
                        var content = syncClient.DownloadString(Properties.Resources.JSON_SOURCE_URL);

                        // Create the Json serializer and parse the response
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
                        {
                            rootObject = (RootObject)serializer.ReadObject(ms);
                        }

                        Hashtable vehicles = new Hashtable();
                        foreach (VehicleMonitoringDelivery vehMonitoringActivity in rootObject.Siri.ServiceDelivery.VehicleMonitoringDelivery)
                        {
                            foreach (VehicleActivity vehActivity in vehMonitoringActivity.VehicleActivity)
                            {
                                try
                                {
                                    if (vehActivity.MonitoredVehicleJourney.VehicleLocation.Latitude != 0
                                        && vehActivity.MonitoredVehicleJourney.VehicleLocation.Longitude != 0)
                                        vehicles.Add(vehActivity.MonitoredVehicleJourney.VehicleRef.value,
                                                     Models.Vehicle.CreateVehicle(
                                                         vehActivity.MonitoredVehicleJourney.VehicleRef.value,
                                                         vehActivity.MonitoredVehicleJourney.LineRef.value,
                                                         vehActivity.MonitoredVehicleJourney.VehicleLocation.Latitude,
                                                         vehActivity.MonitoredVehicleJourney.VehicleLocation.Longitude));
                                }
                                catch
                                { }
                            }
                        }

                        OnModelAvailable(new ModelAvailableEventArgs(vehicles));
                    }
                    catch { }
                } while (!resetEvent.Wait(Convert.ToInt32(Properties.Resources.MODEL_REFRESH_TIMEOUT), cancelTokensource.Token));
            }
            catch (OperationCanceledException)
            { }
        }

    }
}
