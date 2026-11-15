namespace PokeMatchup
{
    public class MockDataService : IDataService
    {
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// Test data, always returns data for Shuckle.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Pokemon?> GetPokemon(string name)
        {
            Pokemon pokemon = new(213, "shuckle",
            [
                new(1, new NamedAPIResource("bug", "https://pokeapi.co/api/v2/type/7/")),
                new(2, new NamedAPIResource("rock", "https://pokeapi.co/api/v2/type/6/")),
            ]);
            return pokemon;
        }

        /// <summary>
        /// Test data, always returns relations for Bug type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<DamageRelations?> GetTypeRelations(PokemonTypes type)
        {
            DamageRelations damageRelations = new(
                [],
                [new("fighting", "https://pokeapi.co/api/v2/type/2/"), new("flying", "https://pokeapi.co/api/v2/type/3/"), new("poison", "https://pokeapi.co/api/v2/type/4/"), new("ghost", "https://pokeapi.co/api/v2/type/8/"), new("steel", "https://pokeapi.co/api/v2/type/9/"), new("fire", "https://pokeapi.co/api/v2/type/10/"), new("fairy", "https://pokeapi.co/api/v2/type/18/")],
                [new("grass", "https://pokeapi.co/api/v2/type/12/"), new("psychic", "https://pokeapi.co/api/v2/type/14/"), new("dark", "https://pokeapi.co/api/v2/type/17/")],
                [],
                [new("fighting", "https://pokeapi.co/api/v2/type/2/"), new("ground", "https://pokeapi.co/api/v2/type/5/"), new("grass", "https://pokeapi.co/api/v2/type/12/")],
                [new("flying", "https://pokeapi.co/api/v2/type/3/"), new("rock", "https://pokeapi.co/api/v2/type/6/"), new("fire", "https://pokeapi.co/api/v2/type/10/")]
            );
            return damageRelations;
        }

        /// <summary>
        /// Test data, always returns data for oricorio
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<PokemonSpeciesVariety>> GetVarietiesBySpecies(string name)
        {
            List<PokemonSpeciesVariety> psv = [
                new PokemonSpeciesVariety(
                    true,
                    new NamedAPIResource(
                        "oricorio-baile",
                        "https://pokeapi.co/api/v2/pokemon/741/"
                    )
                ),
                new PokemonSpeciesVariety(
                    false,
                    new NamedAPIResource(
                        "oricorio-pom-pom",
                        "https://pokeapi.co/api/v2/pokemon/10123/"
                    )
                ),
                new PokemonSpeciesVariety(
                    false,
                    new NamedAPIResource(
                        "oricorio-pau",
                        "https://pokeapi.co/api/v2/pokemon/10124/"
                    )
                ),
                new PokemonSpeciesVariety(
                    false,
                    new NamedAPIResource(
                        "oricorio-sensu",
                        "https://pokeapi.co/api/v2/pokemon/10125/"
                    )
                )
            ];
            return psv;
        }
    }
}