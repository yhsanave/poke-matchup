namespace PokeMatchup
{
    public interface IDataService : IDisposable
    {
        Task<Pokemon?> GetPokemon(string name);
        Task<DamageRelations?> GetTypeRelations(PokemonTypes type);
        Task<List<PokemonSpeciesVariety>> GetVarietiesBySpecies(string name);
    }
}