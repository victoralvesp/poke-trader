import playerServInstance,{ PlayerDto } from "./playerService";
import pokemonServInstance, { PokemonApi, PokemonDto } from "./pokemonService";

export type TradeParticipantDto = {
    trader: PlayerDto,

    tradeOffers: PokemonDto[],
}

export type TradeInfoDto = {
    tradeFairness: Fairness,

    first: TradeParticipantDto,
    firstScore: number,
    
    second: TradeParticipantDto
    secondScore: number,
}
export type TradeDto = {
    info: TradeInfoDto,
    tradeDate: Date
}


export class TradeService {
    async makeTrade(tradeInfo: TradeInfoDto) : Promise<TradeDto> {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(tradeInfo)
        };
        const response = await fetch("trade", requestOptions);
        const trade = (await response.json()) as TradeDto;
        return trade;
    }
    async approveTrade(firstParticipant: TradeParticipantDto, secondParticipant: TradeParticipantDto) : Promise<TradeInfoDto> {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ first: firstParticipant, second: secondParticipant })
        };
        const response = await fetch("trade/approve", requestOptions);
        const info = (await response.json()) as TradeInfoDto;
        return info;
    }

    async convertToParticipant(playerName: string, playerOffersNames: string[]) : Promise<TradeParticipantDto>{
        const player: PlayerDto = await playerServInstance.getPlayer(playerName);
        const playerOffers: PokemonApi[] = [];
        for (let i = 0; i < playerOffersNames.length; i++) {
            const name = playerOffersNames[i];
            const poke = await pokemonServInstance.findPokemonByName(name);
            if (poke === null)
                continue;
            playerOffers.push(poke!);
        }
        
        const pokemons: PokemonDto[] = playerOffers.map(pk => this.convert(pk))
        return {
            trader: player,
            tradeOffers: pokemons
        };
    }
    convert(pk: PokemonApi): PokemonDto {
        return {
            id : pk.id,
            name: pk.name,
            baseExperience : pk.baseExperience
        }
    }

    constructor() {

    }

}
const tradeServInstance = new TradeService();

export enum Fairness {
    InvalidTrade,
    Fair,
    SlightlyFavorsFirst,
    SlightlyFavorsSecond,
    FavorsFirst,
    FavorsSecond
}

export default tradeServInstance;