using Spectre.Console;
using Spectre.Console.Testing;

namespace PokeMatchup.Tests;

public class UnitTests
{
    private readonly IDataService _dataService = new PokeApiService();
    private readonly IAnsiConsole _console = new TestConsole();

    [Fact]
    public async Task TestMatchups_Shuckle()
    {
        MatchupCommand command = new(_dataService, _console);
        Pokemon? pokemon = await _dataService.GetPokemon("shuckle");
        Matchup matchup = await command.GetMatchups(pokemon?.Types ?? []);

        Assert.Collection(matchup.Strengths,
            e => Assert.Equal("psychic", e),
            e => Assert.Equal("dark", e),
            e => Assert.Equal("normal", e),
            e => Assert.Equal("bug", e),
            e => Assert.Equal("ice", e)
            );

        Assert.Collection(matchup.Weaknesses,
            e => Assert.Equal("ghost", e),
            e => Assert.Equal("steel", e),
            e => Assert.Equal("fairy", e),
            e => Assert.Equal("rock", e),
            e => Assert.Equal("fighting", e),
            e => Assert.Equal("water", e)
        );
    }

    [Fact]
    public async Task TestMatchups_Arceus()
    {
        MatchupCommand command = new(_dataService, _console);
        Pokemon? pokemon = await _dataService.GetPokemon("arceus");
        Matchup matchup = await command.GetMatchups(pokemon?.Types ?? []);

        Assert.Empty(matchup.Strengths);

        Assert.Collection(matchup.Weaknesses,
            e => Assert.Equal("rock", e),
            e => Assert.Equal("steel", e),
            e => Assert.Equal("fighting", e)
        );
    }
}
