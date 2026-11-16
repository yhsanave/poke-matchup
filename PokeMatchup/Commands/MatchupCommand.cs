using System.ComponentModel;
using System.Globalization;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PokeMatchup;

[Description("Check a Pokemon's type matchups.")]
public class MatchupCommand(
    IDataService dataService,
    IAnsiConsole console
    ) : AsyncCommand<MatchupCommand.Settings>
{
    private readonly IDataService _dataService = dataService;
    private readonly IAnsiConsole _console = console;

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("The Pokemon to check.")]
        public required string Name { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        List<PokemonSpeciesVariety> varieties = await _console.Status().StartAsync("Getting species: " + settings.Name + "...", async ctx =>
        {
            return await _dataService.GetVarietiesBySpecies(settings.Name);
        });

        if (!(varieties?.Any() ?? false))
        {
            _console.MarkupLine("[red]Pokemon not found: [/]" + settings.Name);
            return 1;
        }

        var selection = varieties.First().Pokemon.Name;
        if (varieties.Count > 1)
        {
            var options = varieties.Select(v => v.Pokemon.Name);
            selection = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(settings.Name) + " has multiple forms. Please select one:")
                    .AddChoices(options)
                );
        }

        Pokemon pokemon = await _console.Status().StartAsync("Getting Pokemon: " + selection, async ctx =>
        {
            return await _dataService.GetPokemon(selection) ?? throw new Exception("Pokemon not found: " + selection);
        });
        Matchup matchup = await GetMatchups(pokemon.Types);
        _console.Write(BuildMatchupPanel(pokemon, matchup));
        return 0;
    }

    public async Task<Matchup> GetMatchups(List<PokemonTypes> types)
    {
        return await _console.Status().StartAsync("Getting type relationships...", async ctx =>
        {
            // Build matchup lists for each of the pokemon's types
            List<Matchup> matchups = [];
            foreach (PokemonTypes type in types)
            {
                ctx.Status("Getting type relationships for " + MarkupType(type.Type.Name) + " type...");
                DamageRelations? typeRelations = await _dataService.GetTypeRelations(type) ?? throw new Exception("Type Relations not found");
                List<NamedAPIResource> strong = [.. typeRelations.NoDamageFrom, .. typeRelations.HalfDamageFrom, .. typeRelations.DoubleDamageTo];
                List<NamedAPIResource> weak = [.. typeRelations.NoDamageTo, .. typeRelations.HalfDamageTo, .. typeRelations.DoubleDamageFrom];
                matchups.Add(new Matchup()
                {
                    // If the type is both weak and strong vs another type, consider it neutral and include it in neither.
                    Strengths = [.. strong.Distinct().Where(t => !weak.Contains(t)).Select(t => t.Name)],
                    Weaknesses = [.. weak.Distinct().Where(t => !strong.Contains(t)).Select(t => t.Name)]
                });
            }

            ctx.Status("Processing relationships...");
            // If the pokemon has more than one type, merge the two matchups together, removing duplicates
            if (matchups.Count > 1)
            {
                List<string> strong = [.. matchups.SelectMany(m => m.Strengths).Distinct()];
                List<string> weak = [.. matchups.SelectMany(m => m.Weaknesses).Distinct()];
                return new Matchup()
                {
                    // If the pokemon is both weak and strong vs a type, consider it neutral and include it in neither.
                    Strengths = [.. strong.Distinct().Where(t => !weak.Contains(t))],
                    Weaknesses = [.. weak.Distinct().Where(t => !strong.Contains(t))]
                };
            }

            return matchups.First();
        });
    }

    /// <summary>
    /// Returns the name of the type with color markup and capitalization.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>A string.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string MarkupType(string typeName)
    {
        _ = Enum.TryParse(typeName, out PokeTypes pokeType);
        return pokeType switch
        {
            // Colors taken from Bulbapedia
            PokeTypes.normal => "[#9FA19F]Normal[/]",
            PokeTypes.fighting => "[#FF8000]Fighting[/]",
            PokeTypes.flying => "[#81B9EF]Flying[/]",
            PokeTypes.poison => "[#9141CB]Poison[/]",
            PokeTypes.ground => "[#915121]Ground[/]",
            PokeTypes.rock => "[#AFA981]Rock[/]",
            PokeTypes.bug => "[#91A119]Bug[/]",
            PokeTypes.ghost => "[#704170]Ghost[/]",
            PokeTypes.steel => "[#60A1B8]Steel[/]",
            PokeTypes.fire => "[#E62829]Fire[/]",
            PokeTypes.water => "[#2980EF]Water[/]",
            PokeTypes.grass => "[#3FA129]Grass[/]",
            PokeTypes.electric => "[#FAC000]Electric[/]",
            PokeTypes.psychic => "[#EF4179]Psychic[/]",
            PokeTypes.ice => "[#3DCEF3]Ice[/]",
            PokeTypes.dragon => "[#5060E1]Dragon[/]",
            PokeTypes.dark => "[#624D4E]Dark[/]",
            PokeTypes.fairy => "[#EF70EF]Fairy[/]",
            PokeTypes.stellar => "[#40B5A5]Stellar[/]",
            PokeTypes.unknown => "[#68A090]Unknown[/]",
            _ => throw new ArgumentException("Invalid Pokemon type"),
        };
    }

    /// <summary>
    /// Get the color for a given type.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>A color.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Color GetTypeColor(string typeName)
    {
        _ = Enum.TryParse(typeName, out PokeTypes pokeType);
        return pokeType switch
        {
            // Colors taken from Bulbapedia
            PokeTypes.normal => new Color(159, 161, 159),
            PokeTypes.fighting => new Color(255, 128, 0),
            PokeTypes.flying => new Color(129, 185, 239),
            PokeTypes.poison => new Color(145, 65, 203),
            PokeTypes.ground => new Color(145, 81, 33),
            PokeTypes.rock => new Color(175, 169, 129),
            PokeTypes.bug => new Color(145, 161, 25),
            PokeTypes.ghost => new Color(112, 65, 112),
            PokeTypes.steel => new Color(96, 161, 184),
            PokeTypes.fire => new Color(230, 40, 41),
            PokeTypes.water => new Color(41, 128, 239),
            PokeTypes.grass => new Color(63, 161, 41),
            PokeTypes.electric => new Color(250, 192, 0),
            PokeTypes.psychic => new Color(239, 65, 121),
            PokeTypes.ice => new Color(61, 206, 243),
            PokeTypes.dragon => new Color(80, 96, 225),
            PokeTypes.dark => new Color(98, 77, 78),
            PokeTypes.fairy => new Color(239, 112, 239),
            PokeTypes.stellar => new Color(64, 181, 165),
            PokeTypes.unknown => new Color(104, 160, 144),
            _ => throw new ArgumentException("Invalid Pokemon type"),
        };
    }

    public static Panel BuildMatchupPanel(Pokemon pokemon, Matchup matchup)
    {
        Grid grid = new Grid().Expand();
        grid.AddColumns(2);

        Panel strongPanel = new Panel(
            matchup.Strengths.Count != 0 ?
            string.Concat(matchup.Strengths.Select(t => MarkupType(t) + "\n")).Trim() :
            "[dim]None[/]"
            )
            .Header("[bold] Strong [/]", Justify.Center)
            .BorderColor(Color.Red)
            .HeavyBorder()
            .Expand();

        Panel weakPanel = new Panel(
            matchup.Weaknesses.Count != 0 ?
            string.Concat(matchup.Weaknesses.Select(t => MarkupType(t) + "\n")).Trim() :
            "[dim]None[/]"
            )
            .Header("[bold] Weak [/]", Justify.Center)
            .BorderColor(Color.Blue)
            .DashedBorder()
            .Expand();

        grid.AddRow(strongPanel, weakPanel);

        Panel panel = new Panel(grid)
            .Header("[bold] " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pokemon.Name) + " [/]", Justify.Center)
            .BorderColor(GetTypeColor(pokemon.Types.First().Type.Name))
            .DoubleBorder();
        panel.Width = 36;
        return panel;
    }
}