using Spectre.Console.Cli;
using Microsoft.Extensions.DependencyInjection;

namespace PokeMatchup
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDataService, PokeApiService>();

            var registrar = new TypeRegistrar(services);

            var app = new CommandApp(registrar);
            app.SetDefaultCommand<MatchupCommand>();
            app.Configure(config =>
            {
                config.SetApplicationName("PokeMatchup");
                config.AddBranch("debug", debug =>
                {
                    debug.SetDescription("Debug commands");
                    debug.AddCommand<TypesCommand>("types");
                });
            });
            return await app.RunAsync(args);
        }
    }
}


// Dependency Injection Stuff
public sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    public ITypeResolver Build() => new TypeResolver(services.BuildServiceProvider());

    public void Register(Type service, Type implementation) => services.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => services.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory) => services.AddSingleton(service, _ => factory());
}

public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    public object? Resolve(Type? type) => type == null ? null : provider.GetService(type);
}