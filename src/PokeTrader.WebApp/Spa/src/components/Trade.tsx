import React, { Component } from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { Fairness, TradeInfoDto } from '../services/tradeService';

const PENDING_APPROVAL = "PendingApproval";
const FINALIZED = "Finalized";
const PENDING_CONFIRMATION = "PendingConfirmation";
const APPROVAL_TEXT = "Avaliar troca";
const CONFIRMATION_TEXT = "Confirmar troca";
const FINALIZED_TEXT = "Troca realizada"
export class Trade extends Component {
    static displayName = Trade.name;
    state = { tradeText: APPROVAL_TEXT, fairnessText: "", showFairness: false }

    /**
     *
     */
    constructor(props: {} | Readonly<{}>) {
        super(props);
        this.handleFirstPlayerChange.bind(this);
        this.handleFirstPlayerPokemonChange.bind(this);
        this.handleSecondPlayerChange.bind(this);
        this.handleSecondPlayerPokemonChange.bind(this);
        this.nextTradeState.bind(this);
    }
    tradeService = tradeServInstance;
    tradeState: string = PENDING_APPROVAL;
    tradeInfo: TradeInfoDto = {} as TradeInfoDto;
    firstPlayer: string | null = null;
    firstPokeSelection: string[] = [];
    secondPlayer: string | null = null;
    secondPokeSelection: string[] = [];

    render() {
        return (
            <Container fluid>
                <Row className="justify-content-md-right">
                    <Col>
                        <Button variant="outline-primary" size="lg" onClick={this.nextTradeState}>{this.state.tradeText}</Button>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <PlayerSelector onChange={this.handleFirstPlayerChange} placeholder="Primeiro Jogador" initialValue="Jogador 1" />
                        <PokemonSelector onChange={this.handleFirstPlayerPokemonChange} key="first" />
                    </Col>
                    <Col>
                        <PlayerSelector onChange={this.handleSecondPlayerChange} placeholder="Segundo Jogador" initialValue="Jogador 2" />
                        <PokemonSelector onChange={this.handleSecondPlayerPokemonChange} key="second" />
                    </Col>
                </Row>
                <Row className="justify-content-md-center">

                </Row>
            </Container>
        );
    }
    handleFirstPlayerChange(selectedPlayer: string | null) : void {
        this.firstPlayer = selectedPlayer;
    }
    handleFirstPlayerPokemonChange(selectedPokemon: string[]) : void {
        this.firstPokeSelection = selectedPokemon;
    }
    handleSecondPlayerChange(selectedPlayer: string | null) : void {
        this.secondPlayer = selectedPlayer;
    }
    handleSecondPlayerPokemonChange(selectedPokemon: string[]) : void {
        this.secondPokeSelection = selectedPokemon;
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
    async nextTradeState() {
        if (this.tradeState === PENDING_CONFIRMATION) {
            if (typeof this.tradeInfo != typeof undefined) {
                await this.tradeService.makeTrade(this.tradeInfo);
            }
            else {
                const validated = this.validateData();
                if (validated) {
                    const firstParticipant = await this.tradeService.convertToParticipant(this.firstPlayer!, this.firstPokeSelection)
                    const secondParticipant = await this.tradeService.convertToParticipant(this.secondPlayer!, this.secondPokeSelection)
                    const info = await this.tradeService.approveTrade(firstParticipant, secondParticipant);
                    await this.tradeService.makeTrade(info);
                }
            }
            const fairnessText: string = this.convertToText(this.tradeInfo.tradeFairness);
            const tradeText = FINALIZED_TEXT;
            this.setState({ tradeText: tradeText, fairnessText: fairnessText, showFairness: false });
        }
        else if (this.tradeState === PENDING_APPROVAL) {
            const validated = this.validateData();
            if (validated) {
                const firstParticipant = await this.tradeService.convertToParticipant(this.firstPlayer!, this.firstPokeSelection)
                const secondParticipant = await this.tradeService.convertToParticipant(this.secondPlayer!, this.secondPokeSelection)
                this.tradeInfo = await this.tradeService.approveTrade(firstParticipant, secondParticipant);
                const fairnessText: string = this.convertToText(this.tradeInfo.tradeFairness);
                const tradeText = CONFIRMATION_TEXT;
                this.setState({ tradeText: tradeText, fairnessText: fairnessText, showFairness: true });
            }
        }
    }
    convertToText(tradeFairness: Fairness): string {
        switch (tradeFairness) {
            case Fairness.Fair:
                return "Troca justa";
            case Fairness.FavorsFirst:
                return `Troca favorece ${this.firstPlayer}`;
            case Fairness.SlightlyFavorsFirst:
                return `Troca favorece ${this.firstPlayer} ligeiramente`;
            case Fairness.FavorsSecond:
                return `Troca favorece ${this.secondPlayer}`;
            case Fairness.SlightlyFavorsSecond:
                return `Troca favorece ${this.secondPlayer} ligeiramente`;
            default:
                return "";
        }
    }
    validateData(): boolean {
        return (this.firstPlayer != null && this.secondPlayer != null && this.firstPokeSelection.length > 0 && this.secondPokeSelection.length > 0);
    }

}
