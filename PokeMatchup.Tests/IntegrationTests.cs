using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli.Testing;
using Spectre.Console.Testing;

namespace PokeMatchup.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task IntegrationTest_Shuckle()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDataService, PokeApiService>();
        services.AddSingleton<IAnsiConsole, TestConsole>();

        var registrar = new TypeRegistrar(services);

        var app = new CommandAppTester(registrar);
        app.SetDefaultCommand<MatchupCommand>();

        var result = await app.RunAsync(["shuckle"]);

        Assert.Equal(0, result.ExitCode);

        Assert.Contains("╔════════════ Shuckle ═════════════╗", result.Output);
        Assert.Contains("║ ┏━━ Strong ━━━┓  ┌┄┄┄ Weak ┄┄┄┄┐ ║", result.Output);
        Assert.Contains("║ ┃ Psychic     ┃  ┆ Ghost       ┆ ║", result.Output);
        Assert.Contains("║ ┃ Dark        ┃  ┆ Steel       ┆ ║", result.Output);
        Assert.Contains("║ ┃ Normal      ┃  ┆ Fairy       ┆ ║", result.Output);
        Assert.Contains("║ ┃ Bug         ┃  ┆ Rock        ┆ ║", result.Output);
        Assert.Contains("║ ┃ Ice         ┃  ┆ Fighting    ┆ ║", result.Output);
        Assert.Contains("║ ┗━━━━━━━━━━━━━┛  ┆ Water       ┆ ║", result.Output);
        Assert.Contains("║                  └┄┄┄┄┄┄┄┄┄┄┄┄┄┘ ║", result.Output);
        Assert.Contains("╚══════════════════════════════════╝", result.Output);
    }

    [Fact]
    public async Task IntegrationTest_Arceus()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDataService, PokeApiService>();
        services.AddSingleton<IAnsiConsole, TestConsole>();

        var registrar = new TypeRegistrar(services);

        var app = new CommandAppTester(registrar);
        app.SetDefaultCommand<MatchupCommand>();

        var result = await app.RunAsync(["arceus"]);

        Assert.Equal(0, result.ExitCode);

        Assert.Contains("╔═════════════ Arceus ═════════════╗", result.Output);
        Assert.Contains("║ ┏━ Strong ━┓  ┌┄┄┄┄┄ Weak ┄┄┄┄┄┐ ║", result.Output);
        Assert.Contains("║ ┃ None     ┃  ┆ Rock           ┆ ║", result.Output);
        Assert.Contains("║ ┗━━━━━━━━━━┛  ┆ Steel          ┆ ║", result.Output);
        Assert.Contains("║               ┆ Fighting       ┆ ║", result.Output);
        Assert.Contains("║               └┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┘ ║", result.Output);
        Assert.Contains("╚══════════════════════════════════╝", result.Output);
    }

    [Fact]
    public async Task IntegrationTest_Oricorio()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IDataService, PokeApiService>();
        services.AddSingleton<IAnsiConsole, TestConsole>();

        var registrar = new TypeRegistrar(services);

        var app = new CommandAppTester(registrar);
        app.SetDefaultCommand<MatchupCommand>();

        app.Console.Profile.Capabilities.Interactive = true;
        app.Console.Input.PushKey(ConsoleKey.Enter);

        var result = await app.RunAsync(["oricorio"]);

        Assert.Equal(0, result.ExitCode);

        Assert.Contains("Oricorio has multiple forms. Please select one:", result.Output);
        Assert.Contains("> oricorio-baile", result.Output);
        Assert.Contains("  oricorio-pom-pom", result.Output);
        Assert.Contains("  oricorio-pau", result.Output);
        Assert.Contains("  oricorio-sensu", result.Output);
        
        Assert.Contains("╔═════════ Oricorio-Baile ═════════╗", result.Output);
        Assert.Contains("║ ┏━━ Strong ━━━┓  ┌┄┄┄ Weak ┄┄┄┄┐ ║", result.Output);
        Assert.Contains("║ ┃ Bug         ┃  ┆ Rock        ┆ ║", result.Output);
        Assert.Contains("║ ┃ Grass       ┃  ┆ Water       ┆ ║", result.Output);
        Assert.Contains("║ ┃ Fairy       ┃  ┆ Dragon      ┆ ║", result.Output);
        Assert.Contains("║ ┃ Fighting    ┃  ┆ Electric    ┆ ║", result.Output);
        Assert.Contains("║ ┗━━━━━━━━━━━━━┛  └┄┄┄┄┄┄┄┄┄┄┄┄┄┘ ║", result.Output);
        Assert.Contains("╚══════════════════════════════════╝", result.Output);
    }

    [Fact]
    public void IntegrationTest_Types()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<TypesCommand>();

        var result = app.Run();

        Assert.Equal(0, result.ExitCode);

        Assert.Contains("Normal    Fighting  Flying    Poison    Ground", result.Output);
        Assert.Contains("Rock      Bug       Ghost     Steel     Fire", result.Output);
        Assert.Contains("Water     Grass     Electric  Psychic   Ice", result.Output);
        Assert.Contains("Dragon    Dark      Fairy     Stellar   Unknown", result.Output);
    }
}