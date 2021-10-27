using ElevenNote.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Contracts
{
    public interface INoteContext
    {
        DbSet<Note> Notes { get; }
        DbSet<Category> Categories { get; }
        int SaveChanges();
    }
}
