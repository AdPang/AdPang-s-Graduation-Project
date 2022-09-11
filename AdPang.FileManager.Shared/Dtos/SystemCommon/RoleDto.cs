using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class RoleDto : BaseDto<Guid>
    {
        public string Name { get; set; }
        //public string NormalizedName { get; set; }

    }
}
