import playerServInstance,{ PlayerDto } from "./playerService";

export type TradeParticipantDto = {
    trader: PlayerDto,

    tradeOfferIds: number[],
}

export type TradeInfoDto = {
    tradeFairness: Fairness,

    first: TradeParticipantDto,

    second: TradeParticipantDto
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

    async convertToParticipant(playerName: string, playerOffers: number[]) : Promise<TradeParticipantDto>{
        const player: PlayerDto = await playerServInstance.getPlayer(playerName);
        return {
            trader: player,
            tradeOfferIds: playerOffers
        };
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