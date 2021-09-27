using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_Pokemon.Data;
using Api_Pokemon.Models;

namespace Api_Pokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly Api_PokemonContext _context;

        public PokemonsController(Api_PokemonContext context)
        {
            _context = context;
        }

        //// GET: api/Pokemons
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Pokemon>>> GetPokemon()
        //{
        //    return await _context.Pokemon.ToListAsync();
        //}

        // GET: api/Pokemons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pokemon>> GetPokemon(int id)
        {
            var pokemon = await _context.Pokemon.FindAsync(id);

            if (pokemon == null)
            {
                return NotFound();
            }

            return pokemon;
        }

        // PUT: api/Pokemons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPokemon(int id, Pokemon pokemon)
        {
            if (id != pokemon.id)
            {
                return BadRequest();
            }

            _context.Entry(pokemon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pokemons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pokemon>> PostPokemon(Pokemon pokemon)
        {
            _context.Pokemon.Add(pokemon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPokemon", new { id = pokemon.id }, pokemon);
        }

        // DELETE: api/Pokemons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pokemon>> DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemon.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            _context.Pokemon.Remove(pokemon);
            await _context.SaveChangesAsync();

            return pokemon;
        }

        // GET: api/Pokemons
        [HttpGet]
        public async Task<ActionResult> GetPokemon([FromQuery] int? page, int? numxpage)
        {
            int _page = page ?? 1;
            int _countpage = numxpage ?? 10;
            decimal total_records = await _context.Pokemon.CountAsync();
            int total_pages = Convert.ToInt32(Math.Ceiling(total_records / _countpage));
            var pokemons = await _context.Pokemon.Skip((_page - 1) * _countpage).Take(_countpage).ToListAsync();

            return Ok(new
            {
                pages = total_pages,
                records = pokemons,
                current_page = _page

            });
        }

        private bool PokemonExists(int id)
        {
            return _context.Pokemon.Any(e => e.id == id);
        }
    }
}
