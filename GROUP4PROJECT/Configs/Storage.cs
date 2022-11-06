using Supabase.Storage;
using Storage.Interfaces;
using System.Diagnostics;

namespace GROUP4PROJECT.Configs
{
    public class Storage
    {
        public static IStorageFileApi<FileObject> GetResourceBucket()
        {
            var storage = Supabase.StatelessClient.Storage(SupabaseConfig.host, SupabaseConfig.publicAnonKey);

            return storage.From("resources");
        }
    }
}