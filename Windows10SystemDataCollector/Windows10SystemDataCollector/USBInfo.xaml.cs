using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows10SystemDataCollector.DataTypes;
using Microsoft.Win32;

namespace Windows10SystemDataCollector
{
    /// <summary>
    /// Interaction logic for USBInfo.xaml
    /// </summary>
    public partial class USBInfo
    {
        private List<USBInfoData> usbDevices = new List<USBInfoData>();
        private List<RegistryKey> regKeys = new List<RegistryKey>();

        public USBInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var usbStor = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Enum\\USBSTOR\\");
            GetUSBSTORRegistryKeys(usbStor);
        }

        /// <summary>
        ///     Enumerate all of the 
        /// </summary>
        /// <param name="key"></param>
        private void GetUSBSTORRegistryKeys(RegistryKey key)
        {
            foreach (var v in key.GetSubKeyNames())
            {
                var productKey1 = key.OpenSubKey(v);

                foreach (var subKeyName in productKey1.GetSubKeyNames())
                {
                    var productKey2 = productKey1.OpenSubKey(subKeyName);

                    try
                    {
                        var usbDevice = new USBInfoData();
                        usbDevice.Name = Convert.ToString(productKey2.GetValue("FriendlyName"));
                        usbDevice.Guid = Convert.ToString(productKey2.GetValue("ClassGUID"));
                        usbDevice.Description = Convert.ToString(productKey2.GetValue("DeviceDesc"));
                        usbDevice.ContainerID = Convert.ToString(productKey2.GetValue("ContainerID"));
                        usbDevices.Add(usbDevice);
                    }
                    catch (Exception)
                    { }
                }
            }
            RootDataGrid.ItemsSource = usbDevices;
        }
    }
}
