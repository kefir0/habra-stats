namespace HabrApi.EntityModel
{
    public partial class HabraStatsEntities
    {
        public const string DefaultConnectionString =
            @"metadata=res://*/EntityModel.HabraStats.csdl|res://*/EntityModel.HabraStats.ssdl|res://*/EntityModel.HabraStats.msl;provider=System.Data.SqlClient;provider connection string="";data source=NAXAH-PC\SQLEXPRESS;initial catalog=habrastats;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework""";

        public static HabraStatsEntities CreateInstance()
        {
            return new HabraStatsEntities(DefaultConnectionString);
        }
    }
}