import React from 'react';
import AsyncSelect from 'react-select/async';
import pokemonServInstance, { PokemonService } from '../services/pokemonService';

    

export class PokemonSelector extends React.Component {

    pokemonService: PokemonService = {} as PokemonService;
    selectedPokemon: string[] = [];
    pokemonOptions: Array<{ label: string, value: string }> = [];
    

    constructor(props: {} | Readonly<{}>) {
        super(props);
        this.pokemonService = pokemonServInstance;
        this.pokemonService.getMorePokemon();
    }

    static displayName = PokemonSelector.name;
    state = { inputValue: '', options: [] }
    handleInputChange(newValue: string) {
        const inputValue = newValue;
        this.setState({ inputValue });
        return inputValue;
    };
    handleSelectChange(eventValue: any, params: any): void {
        if (typeof params === typeof undefined)
            return;
        let { action, option, removedValue, name } = params;
        if (action === "select-option" && typeof option !== typeof undefined) {
            let { label } = option;
            if (typeof label !== typeof undefined) {
                this.selectedPokemon = [...this.selectedPokemon, label];
                this.pokemonService.findPokemonByName(label);
            }
        }
        else if (action === "deselect-option" && typeof removedValue !== typeof undefined) {
            let { label } = removedValue;
            if (typeof label !== typeof undefined) {
                const index = this.selectedPokemon.indexOf(label, 0);
                if (index > -1) {
                    this.selectedPokemon.splice(index, 1);
                    this.selectedPokemon = [...this.selectedPokemon];
                }
            }
        }

    };
    getPokemonOptions(inputValue: string): Promise<any> {
        
        return new Promise(resolve => {
            if (typeof this.pokemonService !== typeof undefined) {
                this.pokemonService.searchPokemon(inputValue)
                .then(value => resolve(value));
            }
        });
    }

    async componentDidMount() {
        
    }

    render() {
        return (
            <div>
                <AsyncSelect
                    loadOptions={this.getPokemonOptions.bind(this)}
                    defaultOptions
                    isMulti
                    cacheOptions
                    placeholder="Selecione os Pokemons..."
                    onInputChange={this.handleInputChange.bind(this)}
                    noOptionsMessage={this.noOptions}
                    onChange={this.handleSelectChange.bind(this)}
                />
            </div>
        );
    }
    refreshOptions(event: React.FocusEvent<HTMLElement>): void{
        this.pokemonOptions = [... this.pokemonService.loadedPokemonNames];
        this.setState({inputValue: this.state.inputValue, options: this.pokemonOptions})
        console.log("pokemonOptions");
        console.log(this.pokemonOptions);
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
}
