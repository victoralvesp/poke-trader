import React from 'react';
import { PokemonService } from './services/pokemonService'

export const pokemonService = new PokemonService();

export const state = {
    pokemonService: pokemonService
}

export const StateContext = React.createContext(state)