﻿namespace HabrApi.EntityModel
{
    public partial class HabraStatsEntities
    {
        public const string DefaultConnectionString =
            @"metadata=res://*/EntityModel.HabraStats.csdl|res://*/EntityModel.HabraStats.ssdl|res://*/EntityModel.HabraStats.msl;provider=System.Data.SqlClient;provider connection string="";data source=NAXAH-PC\SQLEXPRESS;initial catalog=habrastats;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework""";

        public static HabraStatsEntities CreateInstance()
        {
            return new HabraStatsEntities(DefaultConnectionString);
        }

        public void UpsertPost(Post post)
        {
            ExecuteStoreCommand("DELETE FROM POSTS WHERE ID=" + post.Id);
            Posts.AddObject(post);
        }

        partial void OnContextCreated()
        {
            CommandTimeout = 60*5;
        }
    }
}