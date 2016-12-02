namespace GreenHouseWebApi.Model
{
    /// <summary>
    /// Be careful with changes here, the enum is used in the database as well as in spark communication
    /// </summary>
    public enum WateringLocation : int
    {
        First = 1,
        Second = 2
    }
}