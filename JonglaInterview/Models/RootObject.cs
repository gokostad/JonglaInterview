using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JonglaInterview.Models
{
    public class ProducerRef
    {
        public string value { get; set; }
    }

    public class LineRef
    {
        public string value { get; set; }
    }

    public class DirectionRef
    {
        public string value { get; set; }
    }

    public class DataFrameRef
    {
        public string value { get; set; }
    }

    public class FramedVehicleJourneyRef
    {
        public DataFrameRef DataFrameRef { get; set; }
        public string DatedVehicleJourneyRef { get; set; }
    }

    public class OperatorRef
    {
        public string value { get; set; }
    }

    public class VehicleLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class MonitoredCall
    {
        public string StopPointRef { get; set; }
        public int? Order { get; set; }
    }

    public class VehicleRef
    {
        public string value { get; set; }
    }

    public class MonitoredVehicleJourney
    {
        public LineRef LineRef { get; set; }
        public DirectionRef DirectionRef { get; set; }
        public FramedVehicleJourneyRef FramedVehicleJourneyRef { get; set; }
        public OperatorRef OperatorRef { get; set; }
        public bool Monitored { get; set; }
        public VehicleLocation VehicleLocation { get; set; }
        public int Delay { get; set; }
        public MonitoredCall MonitoredCall { get; set; }
        public VehicleRef VehicleRef { get; set; }
        public int? Bearing { get; set; }
    }

    public class VehicleActivity
    {
        public object ValidUntilTime { get; set; }
        public object RecordedAtTime { get; set; }
        public MonitoredVehicleJourney MonitoredVehicleJourney { get; set; }
    }

    public class VehicleMonitoringDelivery
    {
        public string version { get; set; }
        public long ResponseTimestamp { get; set; }
        public bool Status { get; set; }
        public List<VehicleActivity> VehicleActivity { get; set; }
    }

    public class ServiceDelivery
    {
        public long ResponseTimestamp { get; set; }
        public ProducerRef ProducerRef { get; set; }
        public bool Status { get; set; }
        public bool MoreData { get; set; }
        public List<VehicleMonitoringDelivery> VehicleMonitoringDelivery { get; set; }
    }

    public class Siri
    {
        public string version { get; set; }
        public ServiceDelivery ServiceDelivery { get; set; }
    }

    [DataContract]
    public class RootObject // RootObject
    {
        [DataMember]
        public Siri Siri { get; set; }
    }
}
