import React from 'react';
import AsyncSelect from 'react-select/async';
import { ToastContainer, toast } from 'react-toastify';

import 'react-toastify/dist/ReactToastify.css';
import { OptionsType } from 'react-select';
import playerServInstance, { PlayerService } from '../services/playerService';


const MAXIMUM_NMBR_player = 6;
export class PlayerSelector extends React.Component {

    playerService: PlayerService = {} as PlayerService;
    selectedplayer: string[] = [];
    playerOptions: Array<{ label: string, value: string }> = [];
    playerPlaceholder: ReactNode;


    constructor(props: { selectedOptions: [] } | Readonly<{ selectedOptions: []}>) {
        super(props);
        this.playerService = playerServInstance;
        this.playerService.getPlayers();
        // this.selectedplayer = selectedOptions;
    }

    static displayName = PlayerSelector.name;
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
            if (this.selectedplayer.length == MAXIMUM_NMBR_player) {
                this.showErrorMessage();
                return;
            }
            let { label } = option;
            if (typeof label !== typeof undefined) {
                this.selectedplayer = [...this.selectedplayer, label];
            }
        }
        else if (action === "remove-value" && typeof removedValue !== typeof undefined) {
            let { label } = removedValue;
            if (typeof label !== typeof undefined) {
                const index = this.selectedplayer.indexOf(label, 0);
                if (index > -1) {
                    this.selectedplayer.splice(index, 1);
                    this.selectedplayer = [...this.selectedplayer];
                }
            }
            console.log(label);
        }
        console.log("action");
        console.log(action);
        console.log("selected");
        console.log(this.selectedplayer);
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
                this.playerService.searchPlayer(inputValue)
                    .then(value => resolve(value));
            }
        });
    }

    render() {
        return (
            <div>
                <AsyncSelect
                    loadOptions={this.getPlayerOptions.bind(this)}
                    defaultOptions
                    cacheOptions
                    placeholder={this.playerPlaceholder}
                    closeMenuOnSelect={false}
                    onInputChange={this.handleInputChange.bind(this)}
                    noOptionsMessage={this.noOptions}
                    onChange={this.handleSelectChange.bind(this)}
                    onBlur={this.getOrSave.bind(this)}
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
    getOrSave(): import("react-select").FocusEventHandler {
        throw new Error('Method not implemented.');
    }
    optionselected(option: any, options: OptionsType<any>): boolean {
        let { label } = option;
        if (typeof label === typeof undefined)
            return false;
        return this.selectedplayer.indexOf(label) >= 0; 
    }
    maximumReached(option: any, options: OptionsType<any>): boolean {
        if (this.selectedplayer.length === MAXIMUM_NMBR_player)
            return true;
        return false;
    }
    refreshOptions(event: React.FocusEvent<HTMLElement>): void {
        this.playerOptions = [... this.playerService.loadedplayerNames];
        this.setState({ inputValue: this.state.inputValue, options: this.playerOptions })
        console.log("playerOptions");
        console.log(this.playerOptions);
    }
    noOptions() {
        return "Nenhum player encontrado"
    }
}
