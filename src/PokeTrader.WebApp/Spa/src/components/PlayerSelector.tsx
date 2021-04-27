import React from 'react';
import { ToastContainer, toast } from 'react-toastify';
import PropTypes from 'prop-types';
import AsyncCreatableSelect from 'react-select/async-creatable';
import 'react-toastify/dist/ReactToastify.css';
import { OptionsType } from 'react-select';
import playerServInstance, { PlayerService } from '../services/playerService';

type PlayerProps = {
    placeholder: string,
    initialValue: string,
    update: number
}

const MAXIMUM_NMBR_player = 6;
export class PlayerSelector extends React.Component<PlayerProps> {

    playerService: PlayerService = {} as PlayerService;
    selectedplayer: string = "";
    playerOptions: Array<{ label: string, value: string }> = [];
    playerPlaceholder: string = 'Nome';


    constructor(props: PlayerProps | Readonly<PlayerProps>) {
        super(props);
        this.playerService = playerServInstance;
        this.playerService.getPlayers();
        this.setState({ inputValue: this.props.initialValue });
        // this.selectedplayer = selectedOptions;
    }

    static displayName = PlayerSelector.name;
    state = { inputValue: '', update: 0 }
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
        if (action === "select-option" && typeof option !== typeof undefined) {
            if (this.selectedplayer.length == MAXIMUM_NMBR_player) {
                this.showErrorMessage();
                return;
            }
            let { label } = option;
            if (typeof label !== typeof undefined) {
                this.setSelectedPlayer(label);
            }
        }

        console.log("action");
        console.log(action);
        console.log("selected");
        console.log(this.selectedplayer);
    }
    setSelectedPlayer(newPlayer: string) {
        this.selectedplayer = newPlayer;
    }
    showErrorMessage() {
        toast.error('Máximo de seis players', {
            position: "top-right",
            autoClose: 2000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
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
    
    // async isValidNewOption(inputValue: string): Promise<boolean> {
    //     var player = await this.playerService.searchPlayerForSelect(inputValue);
    //     return player.length === 0;
    // }
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
