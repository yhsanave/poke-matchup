# PokeMatchup

Command line tool for checking a Pokemon's type matchups. Created for a technical assessment.

Uses [PokeAPI](https://pokeapi.co) for pokemon data.

## Usage

### Matchup

`PokeMatchup.exe <name>` to check a pokemon's type matchups.

If a pokemon has multiple forms, enter the species name and it will prompt you to select from a list of available forms.

### Strengths/weaknesses are determined by the following rules

The input Pokémon’s type is strong against another type if it:

- Does double damage to the other type

- Takes no damage from the other type

- Takes half damage from the other type

The input Pokémon’s type is weak against another type if it:

- Does no damage to the other type

- Does half damage to the other type

- Takes double damage from the other type

Additionally, if a pokemon would be both weak and strong against a type (e.g. a Normal type is both immune to Ghost and takes no damage from Ghost), it is considered neutral and included in neither. Abilities are not considered in this calculation.

### Debug

`PokeMatchup.exe debug types` will print a list of all available types, for testing type colors.
