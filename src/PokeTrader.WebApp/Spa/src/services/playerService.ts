export type PlayerDto = {
    name: string
}
export class PlayerService {


    constructor() {

    }
    filter(inputValue: string) {
        return this.loadedPlayers.filter(i =>
            i.name.toLowerCase().includes(inputValue.toLowerCase())
        );
    };

    loadedPlayers: Array<PlayerDto> = [];

    async searchPlayerForSelect(name: string): Promise<Array<{ value: string, label: string }>> {
        const players = await this.searchPlayer(name);
        if (players == null)
            return [];

        return (players)!.map(p => this.getNameForSelect(p) as { value: string, label: string });
    }
    async searchPlayer(name: string): Promise<Array<PlayerDto>> {
        console.log("searching player");
        console.log(this.loadedPlayers.map(p => p.name));
        if (name === '') {
            if (this.loadedPlayers.length === 0) {
                await this.getPlayers();
            }
            return this.loadedPlayers;
        }
        let filteredPlayers = this.filter(name);
        if (filteredPlayers.length > 0)
            return filteredPlayers;

        this.getPlayers();
        filteredPlayers = this.filter(name);
        console.log(this.loadedPlayers);
        return filteredPlayers;
    }
    async AddByName(name: string): Promise<PlayerDto> {
        return this.Add({ name: name });
    }
    async Add(player: PlayerDto): Promise<PlayerDto> {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(player)
        };
        const response = await fetch("player", requestOptions);
        const playerResp = (await response.json()) as PlayerDto;
        return playerResp;
    }
    async findPlayer(name: string): Promise<PlayerDto | null> {
        if (name === '') {
            if (this.loadedPlayers.length === 0) {
                await this.getPlayers();
            }
        }
        const player = this.loadedPlayers.find((pl) => pl.name === name);
        if (typeof player === typeof undefined)
            return null;

        return player as PlayerDto;
    }

    async getPlayer(name: string): Promise<PlayerDto> {
        const response = await fetch(`player/${name}`);
        const player = await response.json() as PlayerDto;
        return player;
    }

    getNameForSelect(player: PlayerDto | null): { value: string, label: string } | null {
        if (player !== null && player.name)
            return { value: player.name, label: player.name };
        else
            return null;
    }

    async getPlayers(): Promise<PlayerDto[]> {
        const response = await fetch(`player`);
        const data = await response.json() as PlayerDto[];
        this.loadNewData(data);
        return this.loadedPlayers;
    }

    private loadNewData(data: PlayerDto[]) {
        var notLoadedData = data.filter(pl => this.loadedPlayers.map(p => p.name).indexOf(pl.name) < 0);
        this.loadedPlayers = [...this.loadedPlayers, ...notLoadedData];
    }

    private async getPlayerForSelect(): Promise<Array<{ value: string, label: string }>> {
        const players = await this.getPlayers();
        if (players == null)
            return [];

        return (players)!.map(p => this.getNameForSelect(p) as { value: string, label: string });
    }
}
const playerServInstance = new PlayerService();

export default playerServInstance;