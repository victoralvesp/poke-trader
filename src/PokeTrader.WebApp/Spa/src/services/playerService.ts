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
        this.loadedPlayers = Array.from(new Set(this.loadedPlayers).values());
        filteredPlayers = this.filter(name);
        return filteredPlayers;
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
        this.loadedPlayers = [... this.loadedPlayers, ...data];
        this.loadedPlayers = Array.from(new Set(this.loadedPlayers).values());
        return this.loadedPlayers;
    }

    private async getPlayerForSelect() : Promise<Array<{ value: string, label: string }>> {
        return (await this.getPlayers()).map(p => this.getNameForSelect(p));
    }
}
const playerServInstance = new PlayerService();

export default playerServInstance;