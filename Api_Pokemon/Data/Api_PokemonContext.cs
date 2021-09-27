using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api_Pokemon.Models;

namespace Api_Pokemon.Data
{
    public class Api_PokemonContext : DbContext
    {
        public Api_PokemonContext (DbContextOptions<Api_PokemonContext> options)
            : base(options)
        {
        }

        public DbSet<Api_Pokemon.Models.Pokemon> Pokemon { get; set; }
    }
}
