import React, { Component } from 'react';
import { Badge, Button, Col, Container, Form, FormLabel, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { Fairness, TradeInfoDto } from '../services/tradeService';
import { number } from 'prop-types';

const PENDING_APPROVAL = "PendingApproval";
const FINALIZED = "Finalized";
const PENDING_CONFIRMATION = "PendingConfirmation";
const APPROVAL_TEXT = "Avaliar troca";
const CONFIRMATION_TEXT = "Confirmar troca";
const FINALIZED_TEXT = "Troca realizada"
export class Trade extends Component {
    static displayName = Trade.name;
    state = { tradeText: APPROVAL_TEXT, fairnessText: "", showFairness: false, firstParticipantScore: number, secondParticipantScore: number }

    
    constructor(props: {} | Readonly<{}>) {
        super(props);
    }
    tradeService = tradeServInstance;
    tradeState: string = PENDING_APPROVAL;
    tradeInfo: TradeInfoDto = {} as TradeInfoDto;
    firstPlayer: string | null = null;
    firstPokeSelection: string[] = [];
    secondPlayer: string | null = null;
    secondPokeSelection: string[] = [];

    render() {
        const fairnessSection = this.state.showFairness ? <Row className="justify-content-md-center"> <Badge variant="primary">{this.state.fairnessText}</Badge> </Row>
            : undefined;
        const firstParticipantScore = this.state.firstParticipantScore;
        const secondParticipantScore = this.state.secondParticipantScore;
        const firstScoreSection = this.state.showFairness ? <Row className="justify-content-md-center"> <Badge variant="primary">{firstParticipantScore}</Badge> </Row>
            : undefined;
        const secondScoreSection = this.state.showFairness ? <Row className="justify-content-md-center"> <Badge variant="primary">{secondParticipantScore}</Badge> </Row>
            : undefined;
        return (
            <Container fluid>
                <Row className="justify-content-md-right">
                    <Col>
                        <Button variant="outline-primary" size="lg" onClick={this.nextTradeState.bind(this)}>{this.state.tradeText}</Button>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <PlayerSelector onChange={this.handleFirstPlayerChange.bind(this)} placeholder="Primeiro Jogador" initialValue="Jogador 1" />
                        <PokemonSelector onChange={this.handleFirstPlayerPokemonChange.bind(this)} key="first" />
                        {firstScoreSection}
                    </Col>
                    <Col>
                        <PlayerSelector onChange={this.handleSecondPlayerChange.bind(this)} placeholder="Segundo Jogador" initialValue="Jogador 2" />
                        <PokemonSelector onChange={this.handleSecondPlayerPokemonChange.bind(this)} key="second" />
                        {secondScoreSection}
                    </Col>
                </Row>
                {fairnessSection}
            </Container>
        );
    }
    handleFirstPlayerChange(selectedPlayer: string | null) : void {
        this.firstPlayer = selectedPlayer;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL};
        this.setState(newState);
    }
    handleFirstPlayerPokemonChange(selectedPokemon: string[]) : void {
        this.firstPokeSelection = selectedPokemon;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL};
        this.setState(newState);
    }
    handleSecondPlayerChange(selectedPlayer: string | null) : void {
        this.secondPlayer = selectedPlayer;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL};
        this.setState(newState);
    }
    handleSecondPlayerPokemonChange(selectedPokemon: string[]) : void {
        this.secondPokeSelection = selectedPokemon;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL};
        this.setState(newState);
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
            const fairnessText = this.convertToText(this.tradeInfo.tradeFairness);
            const tradeText = FINALIZED_TEXT;
            this.setState({ tradeText: tradeText, fairnessText: fairnessText, showFairness: false });
        }
        else if (this.tradeState === PENDING_APPROVAL) {
            const validated = this.validateData();
            if (validated) {
                const firstParticipant = await this.tradeService.convertToParticipant(this.firstPlayer!, this.firstPokeSelection)
                const secondParticipant = await this.tradeService.convertToParticipant(this.secondPlayer!, this.secondPokeSelection)
                this.tradeInfo = await this.tradeService.approveTrade(firstParticipant, secondParticipant);
                const fairnessText = this.convertToText(this.tradeInfo.tradeFairness);
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
