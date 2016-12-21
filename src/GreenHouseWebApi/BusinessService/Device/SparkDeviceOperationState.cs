namespace GreenHouse.BusinessService.Device
{
    /// <summary>
    /// Must match the internal states of the Spark device
    /// </summary>
    public enum SparkDeviceOperationState
    {
        BootedNoConfiguration = 0,
        Monitoring = 1,
        AutomaticWatering=2,
        ManualWatering =3
    }
}