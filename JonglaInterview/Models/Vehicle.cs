using JonglaInterview.Helpers;
using Microsoft.Maps.MapControl.WPF;

namespace JonglaInterview.Models
{
    public class Vehicle : ObservableObject
    {
        public static Vehicle CreateVehicle(string vehicle, string line, double latitude, double longitude)
        {
            return new Vehicle()
            {
                VehicleRef = vehicle,
                LineRef = line,
                Latitude = latitude,
                Longitude = longitude
            }; 
        }

        protected const string VehicleRefProperty = "VehicleRef";
        private string _vehicleRef;
        public string VehicleRef
        {
            get { return _vehicleRef; }
            set
            {
                if (_vehicleRef != value)
                {
                    _vehicleRef = value;
                    RaisePropertyChanged(VehicleRefProperty);
                }
            }
        }

        protected const string LineRefProperty = "LineRef";
        private string _lineRef;
        public string LineRef
        {
            get { return _lineRef; }
            set
            {
                if (_lineRef != value)
                {
                    _lineRef = value;
                    RaisePropertyChanged(LineRefProperty);
                }
            }
        }

        protected const string LatitudeProperty = "Latitude";
        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    RaisePropertyChanged(LatitudeProperty);
                }
            }
        }

        protected const string LongitudeProperty = "Longitude";
        private double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    RaisePropertyChanged(LongitudeProperty);
                }
            }
        }

        protected const string LocationProperty = "Location";
        public Location Location
        {
            get
            {
                return new Location()
                {
                    Latitude = _latitude,
                    Longitude = _longitude
                };
            }
        }

        public override string ToString()
        {
            return VehicleRef.ToString() + " " + LineRef.ToString() + " " + Latitude.ToString() + " " + Longitude.ToString();
        }
    }
}
