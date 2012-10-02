namespace HabrApi.EntityModel
{
    public partial class HabraStatsEntities
    {
        public const string DefaultConnectionString =
            @"metadata=res://*/EntityModel.HabraStats.csdl|res://*/EntityModel.HabraStats.ssdl|res://*/EntityModel.HabraStats.msl;provider=System.Data.SqlServerCe.4.0;provider connection string="";data source=E:\HabraStats.sdf;max database size=4000""";

        public static HabraStatsEntities CreateInstance()
        {
            return new HabraStatsEntities(DefaultConnectionString);
        }
    }
}