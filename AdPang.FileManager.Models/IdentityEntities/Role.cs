using System;
using System.Collections.Generic;
using AdPang.FileManager.Models.FileManagerEntities.Common;
using Microsoft.AspNetCore.Identity;

namespace AdPang.FileManager.Models.IdentityEntities
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
    }
}
