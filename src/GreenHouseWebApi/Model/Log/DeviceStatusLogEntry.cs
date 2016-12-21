namespace GreenHouse.DataModel.Log
{
    public class DeviceStatusLogEntry : LogEntryBase
    {
        public GeneralDeviceStatus DeviceStatus { get; set; }

        public string StatusMessage { get; set; }
    }
}