﻿using GetXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetXml
{
    public interface IHLogic
    {
        void GetXmlData();

        double getHoursOffline(Device device);

        void getHoursOffline();

        void FilterDevices();

        void OfflineHoursCount(Device device);

        void ChangeTerminalData(Device device);

        bool AddNewDevice(Device device);

        void CreateExcelReport();

        void ReadAddressesFromExcel();

        void PostAddressToDb();

        List<Device> ConverDateToMoscowTime(List<Device> listDevises);

        void SaveActivity();
    }
}
