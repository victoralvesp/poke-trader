import React from 'react';
import AsyncCreatableSelect from 'react-select/async-creatable';
import 'react-toastify/dist/ReactToastify.css';
import { OptionsType } from 'react-select';
import playerServInstance, { PlayerService } from '../services/playerService';

type PlayerProps = {
    placeholder: string,
    initialValue?: string,
    onChange?: (selectedPlayers: string | null) => void
}

export class PlayerSelector extends React.Component<PlayerProps> {

    static displayName = PlayerSelector.name;
    playerService: PlayerService = {} as PlayerService;
    selectedplayer: string = "";
    playerOptions: Array<{ label: string, value: string }> = [];
    playerPlaceholder: string = 'Nome';
    state = { inputValue: '', selectedPlayer: '' }
    

    constructor(props: PlayerProps | Readonly<PlayerProps>) {
        super(props);
        this.playerService = playerServInstance;
        this.playerService.getPlayers();
        
    }

    componentDidMount() {
        this.setState({ inputValue: this.props.initialValue, selectedPlayer: this.state.selectedPlayer });
    }

    handleInputChange(newValue: string) {
        const inputValue = newValue;
        this.playerService.getPlayers();
        this.setState({ inputValue });
        return inputValue;
    };
    handleSelectChange(eventValue: any, params: any): void {
        if (typeof params === typeof undefined)
            return;
        
        let { action, option } = params;
        if (action === "select-option" && typeof eventValue !== typeof undefined) {
            let { label } = eventValue;
            if (typeof label !== typeof undefined) {
                this.setSelectedPlayer(label);
            }
        }
    }
    setSelectedPlayer(newPlayer: string) {
        
        this.selectedplayer = newPlayer;
        if (newPlayer.length < 2) {
            if (typeof this.props.onChange !== typeof undefined) {
                this.props.onChange!(null);
            }
        }
        else {
            if (typeof this.props.onChange !== typeof undefined) {
                this.props.onChange!(newPlayer);
            }
        }
        this.setState({ inputValue: newPlayer, selectedPlayer: newPlayer })
        
    }

    getPlayerOptions(inputValue: string): Promise<any> {

        return new Promise(resolve => {
            if (typeof this.playerService !== typeof undefined) {
                this.playerService.searchPlayerForSelect(inputValue)
                    .then(value => resolve(value));
            }
        });
    }
    
    render() {
        return (
            <div>
                <AsyncCreatableSelect
                    loadOptions={this.getPlayerOptions.bind(this)}
                    defaultOptions
                    cacheOptions
                    isValidNewOption={this.isValidNewOption.bind(this)}
                    formatCreateLabel={this.formatLabel}
                    onCreateOption={this.save.bind(this)}
                    placeholder={this.props.placeholder}
                    onInputChange={this.handleInputChange.bind(this)}
                    onChange={this.handleSelectChange.bind(this)}
                    isOptionSelected={this.checkSelected.bind(this)}
                />
            </div>
        );
    }
    isValidNewOption(inputValue: string, value: any, options: readonly any[]): boolean {
        var player = this.playerService.loadedPlayers.find(p => p.name === inputValue);
        const isNewPlayer = typeof player === typeof undefined;
        return isNewPlayer && inputValue.length >= 2;
    }
    
    formatLabel(inputValue: string) : React.ReactNode {
        return <span>Adicionar {inputValue}</span>
    }
    checkSelected(option: any, options: OptionsType<any>) : boolean {
        if (option.label === this.state.inputValue) {
            return true;
        }
        return false;
    }
    async save(inputValue: string) : Promise<void> {
        const player = await playerServInstance.AddByName(inputValue);
        this.setSelectedPlayer(player.name);
    }
    optionselected(option: any, options: OptionsType<any>): boolean {
        let { label } = option;
        if (typeof label === typeof undefined)
            return false;
        return this.selectedplayer.indexOf(label) >= 0; 
    }
    noOptions() {
        return "Nenhum jogador encontrado"
    }
}
