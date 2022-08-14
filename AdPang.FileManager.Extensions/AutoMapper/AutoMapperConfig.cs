using AdPang.FileManager.Extensions.AutoMapper.CloudSaved;
using AdPang.FileManager.Extensions.AutoMapper.LocalPrivate;
using AdPang.FileManager.Extensions.AutoMapper.SystemCommon;
using AutoMapper;

namespace AdPang.FileManager.Extensions.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// AutoMapper配置类
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                var proList = new List<Profile>()
                {
                    new UserProfile(),
                    new RoleProfile(),
                    new PrivateDiskInfoProfile(),
                    new PrivateFileInfoProfile(),
                    new CloudFileInfoProfile(),
                    new DirInfoProfile(),
                    new UserPrivateFileInfoProfile(),
                    new SharedFileInfoProfile()
                };
                cfg.AddProfiles(proList);
            });
        }
    }
}
