import React, { Component } from 'react';
import AsyncSelect, { makeAsyncSelect } from 'react-select/async';
import { PokemonSelector } from './PokemonSelector';


export class Trade extends Component {
    static displayName = Trade.name;
    state = { inputValue: '' }

    render() {
        return (

            <div>
                <PokemonSelector />
            </div>
        );
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
}
