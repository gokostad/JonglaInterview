using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JonglaInterview.Helpers;
using System.Threading;
using System.Net;

namespace JonglaInterview.ViewModels
{
    public class TimerDataRefresher : IDataRefresher
    {
        IDataService dataLoader = null;
        Task taskRefreshModel = null;
        CancellationTokenSource cancelTokensource = new CancellationTokenSource();
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        public void Initialize(IDataService dataLoader, object param)
        {
            if (dataLoader != null)
            {
                dataLoader.Close();
            }
            this.dataLoader = dataLoader;
            if (dataLoader != null)
            {
                dataLoader.Initialize(param);
            }
        }

        public void Start()
        {
            if (taskRefreshModel == null)
            {
                taskRefreshModel = new Task(() => { RefreshTask(); });
                taskRefreshModel.Start();
            }
            else
                resetEvent.Set();
        }

        private void RefreshTask()
        {
            if (dataLoader == null)
                throw new ArgumentNullException();
            try
            {
                do
                {
                    try
                    {
                        dataLoader.LoadData();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.TraceInformation(e.ToString());
                    }
                } while (!resetEvent.Wait(Convert.ToInt32(Properties.Resources.MODEL_REFRESH_TIMEOUT), cancelTokensource.Token));
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Trace.TraceInformation("Service OperationCanceledException");
            }
            finally
            {
                taskRefreshModel = null;
            }
            System.Diagnostics.Trace.TraceInformation("Refresher Finished");
        }

        public void Stop()
        {
            if (dataLoader != null)
            {
                dataLoader.Close();
            }
            if (taskRefreshModel != null)
            {
                cancelTokensource.Cancel();
                System.Diagnostics.Trace.TraceInformation("Closing Refresher");
                taskRefreshModel.Wait(2000);
                System.Diagnostics.Trace.TraceInformation("Refresher Closed");
                taskRefreshModel = null;
            }
        }
    }
}
