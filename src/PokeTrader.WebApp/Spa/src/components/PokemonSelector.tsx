import React, { useContext } from 'react';
import AsyncSelect, { makeAsyncSelect } from 'react-select/async';
import { PokemonService } from '../services/pokemonService';
import { StateContext } from '../state';

export  class PokemonSelector extends React.Component {
    pokemonService: PokemonService;

    constructor(props: {} | Readonly<{}>) {
        super(props);
        let { pokemonService } = this.context;
        this.pokemonService = pokemonService as PokemonService;
    }
    
    
    static displayName = PokemonSelector.name;
    state = { inputValue: '' }
    handleInputChange(newValue: string) {
        const inputValue = newValue;
        this.setState({ inputValue });
        return inputValue;
    };
    pokemonOptions(inputValue: string) : Promise<any> {
        return new Promise(resolve => {
            this.pokemonService.searchPokemon(inputValue).then(value => resolve(value));
        });
    }

    componentDidMount() {
        this.pokemonService.getNextPage();
    }

    render() {
        return (
            <div>
                <AsyncSelect
                    loadOptions={this.pokemonOptions}
                    defaultOptions
                    isMulti
                    placeholder="Selecione os Pokemons..."
                    onInputChange={this.handleInputChange}
                    noOptionsMessage={this.noOptions}
                />
            </div>
        );
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
}
