using ElevenNote.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data.InterfaceAndMockDbContext
{
    public interface INoteContext : IDisposable
    {        
        DbSet<Note> Notes { get; }
        DbSet<Category> Categories { get; }
        int SaveChanges();
    }
}
