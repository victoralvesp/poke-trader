export class PokemonService {
    constructor() {

    }
    loadedPokemon: Array<{label: string, value: string}> = [];
    filterPokemon(inputValue: string) {
        return this.loadedPokemon.filter(i =>
            i.label.toLowerCase().includes(inputValue.toLowerCase())
        );
    };
    currentPage = 0;

    async searchPokemon(inputValue: string) {
        let pokemonLeft = this.filterPokemon(inputValue);
        while (pokemonLeft.length == 0) {
            let morePokemonTask = this.getNextPage();
            let pokemonWithName = await this.findPokemon(inputValue);
            const morePokemon = await morePokemonTask;
            if (typeof morePokemon == typeof undefined || morePokemon.length == 0) {
                return [];
            }
            this.loadedPokemon = [...this.loadedPokemon, ...morePokemon];
            if (pokemonWithName != null)
                this.loadedPokemon.push(pokemonWithName);
            this.loadedPokemon = Array.from(new Set(this.loadedPokemon).values());
            pokemonLeft = this.filterPokemon(inputValue);
        }
        return pokemonLeft;
    }

    async findPokemon(pokemonName: string) {
        const response = await fetch(`pokemon/${pokemonName}`);
        const data = await response.json();
        if (data.name)
            return { value: data.name, label: data.name };
        else
            return null;
    }

    async getNextPage(){
        const response = await fetch(`pokemon/?page=${this.currentPage}`);
        const data = await response.json();
        if (data.length && data.length > 0) this.currentPage++;
        return data.map((name: string) => {
            return { value: name, label: name }
        });
    }
}