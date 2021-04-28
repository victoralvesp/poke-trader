import React from 'react';
import AsyncSelect from 'react-select/async';
import { ToastContainer, toast } from 'react-toastify';
import pokemonServInstance, { PokemonService } from '../services/pokemonService';

import 'react-toastify/dist/ReactToastify.css';

const MAXIMUM_NMBR_POKEMON = 6;
type PokemonProps = {
    onChange?: (selectedPokemon: Array<string>) => void
}

export class PokemonSelector extends React.Component<PokemonProps> {

    pokemonService: PokemonService = {} as PokemonService;
    selectedPokemon: string[] = [];
    pokemonOptions: Array<{ label: string, value: string }> = [];


    constructor(props: PokemonProps | Readonly<PokemonProps>) {
        super(props);
        this.pokemonService = pokemonServInstance;
        this.pokemonService.getMorePokemon();
    }

    static displayName = PokemonSelector.name;
    state = { inputValue: '' }
    handleInputChange(newValue: string) {
        const inputValue = newValue;
        this.setState({ inputValue });
        return inputValue;
    };
    handleSelectChange(eventValue: any, params: any): void {
        if (typeof params === typeof undefined)
            return;

        let { action, option, removedValue } = params;
        if (action === "select-option" && typeof option !== typeof undefined) {
            if (this.selectedPokemon.length == MAXIMUM_NMBR_POKEMON) {
                this.showErrorMessage();
                return;
            }
            let { label } = option;
            if (typeof label !== typeof undefined) {
                this.setSelectedPokemon([...this.selectedPokemon, label]);
                this.pokemonService.findPokemonByName(label);
            }
        }
        else if (action === "remove-value" && typeof removedValue !== typeof undefined) {
            let { label } = removedValue;
            if (typeof label !== typeof undefined) {
                const index = this.selectedPokemon.indexOf(label, 0);
                if (index > -1) {
                    this.selectedPokemon.splice(index, 1);
                    this.setSelectedPokemon([...this.selectedPokemon]);
                }
            }
            console.log(label);
        }
        console.log("action");
        console.log(action);
        console.log("selected");
        console.log(this.selectedPokemon);
    }
    showErrorMessage() {
        toast.error('Máximo de seis pokemons', {
            position: "top-right",
            autoClose: 2000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }

    getPokemonOptions(inputValue: string): Promise<any> {

        return new Promise(resolve => {
            if (typeof this.pokemonService !== typeof undefined) {
                this.pokemonService.searchPokemon(inputValue)
                    .then(value => resolve(value));
            }
        });
    }

    render() {
        return (
            <div>
                <AsyncSelect
                    loadOptions={this.getPokemonOptions.bind(this)}
                    defaultOptions
                    isMulti
                    isOptionSelected={this.optionselected.bind(this)}
                    cacheOptions
                    placeholder="Selecione os Pokemons..."
                    closeMenuOnSelect={false}
                    tabSelectsValue={false}
                    isOptionDisabled={this.maximumReached.bind(this)}
                    onInputChange={this.handleInputChange.bind(this)}
                    noOptionsMessage={this.noOptions}
                    onChange={this.handleSelectChange.bind(this)}
                />
                <ToastContainer
                    position="top-right"
                    autoClose={2000}
                    hideProgressBar={false}
                    newestOnTop
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable
                    pauseOnHover
                />
            </div>
        );
    }

    setSelectedPokemon(newPokemon: string[]): void {
        this.selectedPokemon = newPokemon;
        if (typeof this.props.onChange !== typeof undefined)
            this.props.onChange!(newPokemon);

    }
    optionselected(option: any, options: any): boolean {
        let { label } = option;
        if (typeof label === typeof undefined)
            return false;
        return this.selectedPokemon.indexOf(label) >= 0;
    }
    maximumReached(option: any, options: any): boolean {
        if (this.selectedPokemon.length === MAXIMUM_NMBR_POKEMON)
            return true;
        return false;
    }
    refreshOptions(event: React.FocusEvent<HTMLElement>): void {
        this.pokemonOptions = [... this.pokemonService.loadedPokemonNames];
        this.setState({ inputValue: this.state.inputValue, options: this.pokemonOptions })
        console.log("pokemonOptions");
        console.log(this.pokemonOptions);
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
}
