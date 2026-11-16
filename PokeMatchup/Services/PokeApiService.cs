using System.Net.Http.Json;

namespace PokeMatchup;

public class PokeApiService : IDataService
{
    private readonly HttpClient _httpClient;

    public PokeApiService()
    {
        _httpClient = new() { BaseAddress = new("https://pokeapi.co/api/v2/") };
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public async Task<List<PokemonSpeciesVariety>> GetVarietiesBySpecies(string name)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("pokemon-species/" + Sanitize(name));
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new ArgumentException("Pokemon species not found: " + name);
        PokemonSpecies? species = await response.Content.ReadFromJsonAsync<PokemonSpecies>();
        return species?.Varieties ?? [];
    }

    public async Task<Pokemon?> GetPokemon(string name)
    {
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("pokemon/" + Sanitize(name));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new ArgumentException("Pokemon not found: " + name);
        return await responseMessage.Content.ReadFromJsonAsync<Pokemon>();
    }

    public async Task<DamageRelations?> GetTypeRelations(PokemonTypes type)
    {
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("type/" + Sanitize(type.Type.Name));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new ArgumentException("Type not found: " + type.Type.Name);
        Type? typeResponse = await responseMessage.Content.ReadFromJsonAsync<Type>();
        return typeResponse?.DamageRelations;
    }

    private static string Sanitize(string input)
    {
        return input
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace("'", "")
            .ToLower();
    }
}
