using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using JonglaInterview.Helpers;
using JonglaInterview.Models;
using System.Windows.Input;

namespace JonglaInterview.ViewModels
{
    public class MapViewModel : JonglaInterview.Helpers.ObservableObject
    {
        IDataRefresher refresher;
        public IDataRefresher Refresher
        {
            get { return refresher; }
        }

        public void InitializeLoader(IDataRefresher refresher, IDataService service, object serviceParam = null)
        {
            if (this.refresher != null)
            {
                this.refresher.Stop();
                this.refresher = null;
            }
            this.refresher = refresher;
            if (this.refresher != null)
            {
                if (service != null)
                {
                    service.ModelAvailable += Service_ModelAvailable;
                }
                this.refresher.Initialize(service, serviceParam);
            }

        }

        private ICommand listSelectionModeCommand;
        public ICommand ListSelectionModeCommand
        {
            get
            {
                return listSelectionModeCommand;
            }
            set
            {
                listSelectionModeCommand = value;
            }
        }

        private bool _canExecuteListSelectionModeCommand = true;
        public bool CanExecuteListSelectionModeCommand(object obj)
        {
            return _canExecuteListSelectionModeCommand;
        }

        public enum ListSelectionModeEnum
        {
            SingleSelectionMode,
            MultipleSelectionMode
        };

        protected const string ListSelectionModeStringValueProperty = "ListSelectionModeStringValue";
        private ListSelectionModeEnum _listSelectionMode = ListSelectionModeEnum.SingleSelectionMode;
        public ListSelectionModeEnum ListSelectionMode
        {
            get
            {
                return _listSelectionMode;
            }

            set
            {
                if (this._listSelectionMode == value)
                {
                    return;
                }

                this._listSelectionMode = value;
                RaisePropertyChanged(ListSelectionModeStringValueProperty);
            }       
        }
        public string ListSelectionModeStringValue
        {
            get 
            { 
                return _listSelectionMode == ListSelectionModeEnum.SingleSelectionMode ? "Single" : "Multiple";
            }
        }


        public void ExecuteListSelectionModeCommand(object obj)
        {
            switch (obj.ToString())
            {
                case "SingleSelectionMode":
                    ListSelectionMode = ListSelectionModeEnum.SingleSelectionMode;
                    break;
                case "MultipleSelectionMode":
                    ListSelectionMode = ListSelectionModeEnum.MultipleSelectionMode;
                    break;
                default:
                    break;
            }
        }

        public MapViewModel()
        {
            VehiclesHash = new Hashtable();
            ListSelectionModeCommand = new RelayCommand(ExecuteListSelectionModeCommand, CanExecuteListSelectionModeCommand);
        }

        public void Service_ModelAvailable(ModelAvailableEventArgs e)
        {
            List<string> keys = _vehicles.Keys.Cast<string>().ToList();
            
            //remove all vehicles not presented in new list
            foreach (string s in keys)
            {
                if (!e.Data.ContainsKey(s))
                    _vehicles.Remove(s);
            }

            //add new and update old vehicles  
            foreach (string s in e.Data.Keys)
            {
                if (!_vehicles.ContainsKey(s))
                    _vehicles.Add(s, e.Data[s]);
                else
                {
                    //have selected vehicle moved?
                    bool selectedLocationChanged = false;
                    if (SelectedVehicle != null
                        && SelectedVehicle.VehicleRef == ((Vehicle)e.Data[s]).VehicleRef
                        && (SelectedVehicle.Longitude != ((Vehicle)e.Data[s]).Longitude
                            || SelectedVehicle.Latitude != ((Vehicle)e.Data[s]).Latitude))
                    {
                        selectedLocationChanged = true;
                    } 

                    ((Vehicle)_vehicles[s]).LineRef = ((Vehicle)e.Data[s]).LineRef;
                    ((Vehicle)_vehicles[s]).Longitude = ((Vehicle)e.Data[s]).Longitude;
                    ((Vehicle)_vehicles[s]).Latitude = ((Vehicle)e.Data[s]).Latitude;

                    //change location of selected vehicle if has been moved
                    if (selectedLocationChanged)
                    {
                        List<Vehicle> vl = new List<Vehicle>();
                        vl.Add(SelectedVehicle);
                        SelectedVehicleLocation = vl;
                    }
                }
            }
            RaisePropertyChanged(VehiclesProperty);
        }

        protected const string VehiclesProperty = "Vehicles";
        private Hashtable _vehicles;
        public List<Vehicle> Vehicles
        {
            get 
            { 
                return (List<Vehicle>)_vehicles.Values.Cast<Vehicle>().ToList(); 
            }
        }

        private Hashtable VehiclesHash
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                RaisePropertyChanged(VehiclesProperty);
            }
        }

        protected const string SelectedVehicleProperty = "SelectedVehicle"; 
        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                RaisePropertyChanged(SelectedVehicleProperty);

                List<Vehicle> vl = new List<Vehicle>();
                vl.Add(_selectedVehicle);
                SelectedVehicleLocation = vl;
            }
        }

        protected const string SelectedVehicleLocationProperty = "SelectedVehicleLocation";
        private List<Vehicle> _selectedVehicleLocation;
        public List<Vehicle> SelectedVehicleLocation
        {
            get { return _selectedVehicleLocation; }
            set
            {
                _selectedVehicleLocation = value;
                RaisePropertyChanged(SelectedVehicleLocationProperty);
            }
        }
    }
}
