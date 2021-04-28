export type PokemonApi = {
    id: number,
    name: string,
    baseExperience: number,
}
export type PokemonDto = {
    id: number,
    name: string,
    baseExperience: number,
}
export class PokemonService {


    constructor(private disabled: boolean = false) {

    }
    loadedPokemonNames: Array<{ label: string, value: string }> = [];
    filterPokemon(inputValue: string) {
        return this.loadedPokemonNames.filter(i =>
            i.label.toLowerCase().includes(inputValue.toLowerCase())
        );
    };
    currentPage = 0;

    loadedPokemons: Array<PokemonApi> = [];

    async searchPokemon(inputValue: string): Promise<Array<{ label: string, value: string }>> {
        if (this.disabled)
            return [];
        console.log("searching pokemon");
        console.log(this.loadedPokemonNames);
        if (inputValue === '') {
            if (this.loadedPokemonNames.length === 0) {
                await this.getMorePokemon();
            }
            return this.loadedPokemonNames;
        }
        let pokemonLeft = this.filterPokemon(inputValue);
        while (pokemonLeft.length === 0) {
            const pokemon = await this.findPokemonByName(inputValue);
            let pokemonWithName = this.getNameForSelect(pokemon);
            this.getMorePokemon();
            if (pokemonWithName != null && this.loadedPokemonNames.indexOf(pokemonWithName) < 0)
                this.loadedPokemonNames.push(pokemonWithName);
            this.loadedPokemonNames = Array.from(new Set(this.loadedPokemonNames).values());
            pokemonLeft = this.filterPokemon(inputValue);
        }
        return pokemonLeft;
    }

    async findPokemonByName(pokemonName: string): Promise<PokemonApi | null> {
        if (this.disabled)
            return null;
        const undefPokemon = this.loadedPokemons.find(poke => poke.name === pokemonName);
        let pokemon: PokemonApi | null = null;

        if (typeof undefPokemon === typeof undefined) {
            const response = await fetch(`pokemon/${pokemonName}`);
            pokemon = await response.json() as PokemonApi;
            this.loadedPokemons = [... this.loadedPokemons, pokemon];
            console.log("loadedPokemons");
            console.log(this.loadedPokemons);
        }
        else {
            pokemon = undefPokemon as PokemonApi;
        }

        return pokemon;
    }

    getNameForSelect(pokemon: PokemonApi | null): { value: string, label: string } | null {
        if (this.disabled)
            return null;
        if (pokemon !== null && pokemon.name)
            return { value: pokemon.name, label: pokemon.name };
        else
            return null;
    }

    async getMorePokemon(): Promise<void> {
        let morePokemonTask = this.getNextPage();
        
        const morePokemon = await morePokemonTask;
        if (typeof morePokemon === typeof undefined || morePokemon!.length === 0) {
            return;
        }
        this.loadedPokemonNames = [...this.loadedPokemonNames, ...morePokemon!];
        this.loadedPokemonNames = Array.from(new Set(this.loadedPokemonNames).values());
    }

    private async getNextPage() {
        if (this.disabled)
            return;
        const response = await fetch(`pokemon/?page=${this.currentPage}`);
        const data = await response.json() as string[];
        if (data.length && data.length > 0) this.currentPage++;
        return data.map((name: string) => {
            return { value: name, label: name }
        });
    }
}
const pokemonServInstance = new PokemonService();

export default pokemonServInstance;