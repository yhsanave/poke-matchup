using System.Text.Json.Serialization;

namespace PokeMatchup
{
    // === API Types ================================================================================================

    /// <summary>
    /// A named resource. <see cref="https://pokeapi.co/docs/v2#namedapiresource"/>
    /// </summary>
    /// 
    /// <param name="Name">The name of the referenced resource.</param>
    /// <param name="Url">The URL of the referenced resource.</param>
    public record class NamedAPIResource
    (
        string Name,
        string Url
    );

    /// <summary>
    /// A Pokemon type. <see cref="https://pokeapi.co/docs/v2#type"/>
    /// </summary>
    /// <param name="Id">The identifier for this resource.</param>
    /// <param name="Name">The name for this resource.</param>
    /// <param name="DamageRelations">A detail of how effective this type is toward others and vice versa.</param>
    public record class Type
    (
        int Id,
        string Name,
        [property: JsonPropertyName("damage_relations")] DamageRelations DamageRelations
    );

    /// <summary>
    /// A collection of a type's effectiveness vs other types. <see cref="https://pokeapi.co/docs/v2#typerelations"/>
    /// </summary>
    /// <param name="NoDamageTo">A list of types this type has no effect on.</param>
    /// <param name="HalfDamageTo">A list of types this type is not very effect against.</param>
    /// <param name="DoubleDamageTo">A list of types this type is very effect against.</param>
    /// <param name="NoDamageFrom">A list of types that have no effect on this type.</param>
    /// <param name="HalfDamageFrom">A list of types that are not very effective against this type.</param>
    /// <param name="DoubleDamageFrom">A list of types that are very effective against this type.</param>
    public record class DamageRelations
    (
        [property: JsonPropertyName("no_damage_to")] List<NamedAPIResource> NoDamageTo,
        [property: JsonPropertyName("half_damage_to")] List<NamedAPIResource> HalfDamageTo,
        [property: JsonPropertyName("double_damage_to")] List<NamedAPIResource> DoubleDamageTo,
        [property: JsonPropertyName("no_damage_from")] List<NamedAPIResource> NoDamageFrom,
        [property: JsonPropertyName("half_damage_from")] List<NamedAPIResource> HalfDamageFrom,
        [property: JsonPropertyName("double_damage_from")] List<NamedAPIResource> DoubleDamageFrom
    );

    /// <summary>
    /// A Pokémon Species forms the basis for at least one Pokémon. 
    /// Attributes of a Pokémon species are shared across all varieties of Pokémon within the species. 
    /// A good example is Wormadam; Wormadam is the species which can be found in three different varieties, Wormadam-Trash, Wormadam-Sandy and Wormadam-Plant. 
    /// <see cref="https://pokeapi.co/docs/v2#pokemon-species"/>
    /// </summary>
    /// <param name="Id">The identifier for this resource.</param>
    /// <param name="Name">The name for this resource.</param>
    /// <param name="Varieties">A list of the Pokémon that exist within this Pokémon species.</param>
    public record class PokemonSpecies
    (
        int Id,
        string Name,
        List<PokemonSpeciesVariety> Varieties
    );

    /// <summary>
    /// Connects a species with it's varieties.
    /// <see cref="https://pokeapi.co/docs/v2#pokemonspeciesvariety"/>
    /// </summary>
    /// <param name="IsDefault">Whether this variety is the default variety.</param>
    /// <param name="Pokemon">The Pokémon variety.</param>
    public record class PokemonSpeciesVariety
    (
        bool IsDefault,
        NamedAPIResource Pokemon
    );

    /// <summary>
    /// Pokémon are the creatures that inhabit the world of the Pokémon games. 
    /// They can be caught using Pokéballs and trained by battling with other Pokémon. 
    /// Each Pokémon belongs to a specific species but may take on a variant which makes it differ from other Pokémon of the same species, such as base stats, available abilities and typings. 
    /// <see cref="https://pokeapi.co/docs/v2#pokemon"/>
    /// </summary>
    /// <param name="Id">The identifier for this resource.</param>
    /// <param name="Name">The name for this resource.</param>
    /// <param name="Types">A list of details showing types this Pokémon has.</param>
    public record class Pokemon
    (
        int Id,
        string Name,
        List<PokemonTypes> Types
    );


    /// <summary>
    /// Connects a Pokemon to one of it's Types
    /// <see cref="https://pokeapi.co/docs/v2#pokemontype"/>
    /// </summary>
    /// <param name="Slot">The order the Pokémon's types are listed in.</param>
    /// <param name="Type">The type the referenced Pokémon has.</param>
    public record class PokemonTypes
    (
        int Slot,
        NamedAPIResource Type
    );


    // === Program classes ============================================================================

    /// <summary>
    /// Collects the strengths and weaknesses for a pokemon.
    /// </summary>
        public struct Matchup
    {
        public List<string> Strengths;
        public List<string> Weaknesses;
    }

    /// <summary>
    /// Enum for Pokemon Type names. Values are Type ids in PokeAPI. 
    /// </summary>
    public enum PokeTypes
    {
        normal = 1,
        fighting = 2,
        flying = 3,
        poison = 4,
        ground = 5,
        rock = 6,
        bug = 7,
        ghost = 8,
        steel = 9,
        fire = 10,
        water = 11,
        grass = 12,
        electric = 13,
        psychic = 14,
        ice = 15,
        dragon = 16,
        dark = 17,
        fairy = 18,
        stellar = 19,
        unknown = 10001
    }
}