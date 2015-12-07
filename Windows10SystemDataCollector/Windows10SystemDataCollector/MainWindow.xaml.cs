using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Windows10SystemDataCollector.DataTypes;
using Microsoft.Win32;
using DriveInfo = System.IO.DriveInfo;

namespace Windows10SystemDataCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region CONSTRUCTORS
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region EVENTS
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowsAndDriveInfo();
            
        }
        #endregion


        #region PRIVATE METHODS
        /// <summary>
        ///     Gets Windows information and adds it to the WidnowsInfoDataGrid. Gets Hard drive info and adds it to the DriveInfoDataGrid.
        /// </summary>
        private void SetWindowsAndDriveInfo()
        {
            var windowsInfo = new List<WindowsInfo>();

            var windowsVersion = new WindowsInfo {TheObject = "Windows Version", TheValue = Environment.OSVersion.ToString()};
            windowsInfo.Add(windowsVersion);
            var machineName = new WindowsInfo { TheObject = "Machine Name", TheValue = Environment.MachineName };
            windowsInfo.Add(machineName);
            var username = new WindowsInfo {TheObject = "Username", TheValue = Environment.UserName};
            windowsInfo.Add(username);
            var domainName = new WindowsInfo { TheObject = "Domain Name", TheValue = Environment.UserDomainName };
            windowsInfo.Add(domainName);
            var is64Bit = new WindowsInfo { TheObject = "64-bit", TheValue = Environment.Is64BitOperatingSystem.ToString() };
            windowsInfo.Add(is64Bit);
            var processors = new WindowsInfo { TheObject = "# of Processors", TheValue = Environment.ProcessorCount.ToString() };
            windowsInfo.Add(processors);
            
            DriveInfoDataGrid.ItemsSource = DriveInfo.GetDrives();
            WindowsInfoDataGrid.ItemsSource = windowsInfo;
        }

        /// <summary>
        /// Function to return a generic listn of Hashtables containing each entry
        /// from the requested entry type, event id, machine (in case doing it across a network)
        /// source, and time logged are stored in their own Hashtable then added to the list
        /// </summary>
        /// <param name="logName">name of the log e.x; Application, Security, etc..</param>
        /// <param name="machineName">Machine we're querying</param>
        /// <param name="instanceId">The Event ID we're searching for (4624 - Successful logon; 4625 - Failed logon;</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<EventsLog> GetEventEntryByEvent(string logName, string machineName, long instanceId)
        {
            try
            {
                //Create our list
                var events = new List<EventsLog>();

                //Connect to the EventLog of the specified machine
                var log = new EventLog(logName, machineName);

                //Now we want to loop through each entry
                foreach (EventLogEntry entry in log.Entries)
                {
                    //If we run across one with the right entry id we create a new Hashtable
                    //then we add the Message, InstanceId,Source, and TimeWritten values
                    //from that entry
                    if (entry.InstanceId == instanceId)
                    {
                        var entryInfo = new EventsLog();

                        entryInfo.Message = entry.Message;
                        entryInfo.Source = entry.Source;
                        entryInfo.Time = entry.TimeWritten;

                        //Add this new Hashtable to our list
                        events.Add(entryInfo);

                        entryInfo = null;
                    }
                }
                //Return the results
                return events;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Gather all logon (succeeded and failed) events and display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllLogonEventsButton_Click(object sender, RoutedEventArgs e)
        {
            var logonEvents = GetEventEntryByEvent("Security", Environment.MachineName, 4624);
            foreach (var failedEvent in GetEventEntryByEvent("Security", Environment.MachineName, 4625))
            {
                logonEvents.Add(failedEvent);
            }

            MoreInfoDataGrid.ItemsSource = logonEvents;
        }

        /// <summary>
        ///     Gather just failed logon events and display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Failed_Logon_Events_Click(object sender, RoutedEventArgs e)
        {
            MoreInfoDataGrid.ItemsSource = GetEventEntryByEvent("Security", Environment.MachineName, 4625);
        }

        /// <summary>
        ///     Gather all image and video files and put paths into datagrid new window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImagesandVideosButton_Click(object sender, RoutedEventArgs e)
        {
            var videoImageWindow = new ImagesAndVideosWindow();
            videoImageWindow.Show();
        }

        private void GetThumbCacheButton_Click(object sender, RoutedEventArgs e)
        {
            var usbWindow = new USBInfo();
            usbWindow.Show();
        }
        #endregion
        
    }
}
