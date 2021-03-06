﻿using System;
using System.Collections;

namespace JonglaInterview.Helpers
{
    public delegate void ModelAvailableEventHandler(ModelAvailableEventArgs e);

    public interface IDataService
    {
        event ModelAvailableEventHandler ModelAvailable;

        void LoadData();
        void Close();
    }

    public class ModelAvailableEventArgs : EventArgs
    {
        public Hashtable Data;

        public ModelAvailableEventArgs(Hashtable data)
        {
            Data = data;
        }
    }
}
