﻿using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Repositories.LocalPrivate
{
    public class PrivateFileRepository : BaseRepository<PrivateFileInfo>, IPrivateFileRepository
    {
        public PrivateFileRepository(FileManagerDbContext context) : base(context)
        {
        }
    }
}
