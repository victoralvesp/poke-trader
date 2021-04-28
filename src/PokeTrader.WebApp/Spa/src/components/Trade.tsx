import React, { Component } from 'react';
import { Badge, Button, Col, Container, Form, FormLabel, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { Fairness, TradeInfoDto } from '../services/tradeService';
import { number } from 'prop-types';
import { toast, ToastContainer } from 'react-toastify';

const PENDING_APPROVAL = "PendingApproval";
const FINALIZED = "Finalized";
const PENDING_CONFIRMATION = "PendingConfirmation";
const APPROVAL_TEXT = "Avaliar troca";
const CONFIRMATION_TEXT = "Confirmar troca";
const FINALIZED_TEXT = "Troca realizada"



export class Trade extends Component {
    static displayName = Trade.name;
    state = { tradeText: APPROVAL_TEXT, fairnessText: "", showFairness: false, firstParticipantScore: number, secondParticipantScore: number, tradeState: PENDING_APPROVAL }


    constructor(props: {} | Readonly<{}>) {
        super(props);
    }
    tradeService = tradeServInstance;
    tradeState: string = PENDING_APPROVAL;
    tradeInfo: TradeInfoDto = {} as TradeInfoDto;
    firstPlayer: string | null = null;
    firstScore: number | null = 0;
    firstPokeSelection: string[] = [];
    secondPlayer: string | null = null;
    secondScore: number | null = 0;
    secondPokeSelection: string[] = [];

    componentDidUpdate() {
        this.tradeState = this.state.tradeState
    }

    render() {
        const fairnessSection = this.state.showFairness ? <Row className="justify-content-md-center"> <h3><Badge variant="primary">{this.state.fairnessText}</Badge></h3> </Row>
            : undefined;
        const firstParticipantScore = this.firstScore;
        const secondParticipantScore = this.secondScore;
        const firstScoreSection = this.state.showFairness ? <Row className="justify-content-md-center"> <h4><Badge variant="primary">{firstParticipantScore}</Badge> </h4> </Row>
            : undefined;
        const secondScoreSection = this.state.showFairness ? <Row className="justify-content-md-center"> <h4><Badge variant="primary">{secondParticipantScore}</Badge> </h4> </Row>
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
                        {firstScoreSection}
                        <PlayerSelector onChange={this.handleFirstPlayerChange.bind(this)} placeholder="Primeiro Jogador" initialValue="Jogador 1" />
                        <PokemonSelector onChange={this.handleFirstPlayerPokemonChange.bind(this)} key="first" />
                    </Col>
                    <Col>
                        {secondScoreSection}
                        <PlayerSelector onChange={this.handleSecondPlayerChange.bind(this)} placeholder="Segundo Jogador" initialValue="Jogador 2" />
                        <PokemonSelector onChange={this.handleSecondPlayerPokemonChange.bind(this)} key="second" />
                    </Col>
                </Row>
                {fairnessSection}
                <ToastContainer position="top-right"
                    autoClose={2000}
                    hideProgressBar={false}
                    newestOnTop
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable
                    pauseOnHover />
            </Container>
        );
    }
    showMessage() {
        toast.success('Troca efetuada!', {
            position: "top-right",
            autoClose: 2000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }
    handleFirstPlayerChange(selectedPlayer: string | null): void {
        this.firstPlayer = selectedPlayer;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL };
        this.setState(newState);
    }
    handleFirstPlayerPokemonChange(selectedPokemon: string[]): void {
        this.firstPokeSelection = selectedPokemon;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL };
        this.setState(newState);
    }
    handleSecondPlayerChange(selectedPlayer: string | null): void {
        this.secondPlayer = selectedPlayer;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL };
        this.setState(newState);
    }
    handleSecondPlayerPokemonChange(selectedPokemon: string[]): void {
        this.secondPokeSelection = selectedPokemon;
        const newState = { ... this.state, showFairness: false, tradeState: PENDING_APPROVAL };
        this.setState(newState);
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
    async nextTradeState() {
        if (this.tradeState === PENDING_CONFIRMATION) {
            const validated = this.validateData();
            if (validated) {
                if (typeof this.tradeInfo != typeof undefined) {
                    await this.tradeService.makeTrade(this.tradeInfo);
                }
                else {
                    const firstParticipant = await this.tradeService.convertToParticipant(this.firstPlayer!, this.firstPokeSelection)
                    const secondParticipant = await this.tradeService.convertToParticipant(this.secondPlayer!, this.secondPokeSelection)
                    const info = await this.tradeService.approveTrade(firstParticipant, secondParticipant);
                    await this.tradeService.makeTrade(info);
                }
                const fairnessText = convertToText(this.tradeInfo.tradeFairness, this.firstPlayer!, this.secondPlayer!);
                const tradeText = APPROVAL_TEXT;
                this.showMessage();
                this.setState({ tradeText: tradeText, fairnessText: fairnessText, showFairness: false, tradeState: PENDING_APPROVAL });
            }
        }
        else if (this.tradeState === PENDING_APPROVAL) {
            const validated = this.validateData();
            if (validated) {
                const firstParticipant = await this.tradeService.convertToParticipant(this.firstPlayer!, this.firstPokeSelection)
                const secondParticipant = await this.tradeService.convertToParticipant(this.secondPlayer!, this.secondPokeSelection)
                this.tradeInfo = await this.tradeService.approveTrade(firstParticipant, secondParticipant);
                const fairnessText = convertToText(this.tradeInfo.tradeFairness, this.firstPlayer!, this.secondPlayer!);
                this.firstScore = this.tradeInfo.firstScore;
                this.secondScore = this.tradeInfo.secondScore
                const tradeText = CONFIRMATION_TEXT;
                this.setState({ tradeText: tradeText, fairnessText: fairnessText, showFairness: true, tradeState: PENDING_CONFIRMATION });
            }
        }
    }

    validateData(): boolean {
        return (this.firstPlayer != null && this.secondPlayer != null && this.firstPokeSelection.length > 0 && this.secondPokeSelection.length > 0);
    }

}

export const convertToText = (tradeFairness: Fairness, firstPlayer: string, secondPlayer: string) => {
    switch (tradeFairness) {
        case Fairness.Fair:
            return "Troca justa";
        case Fairness.FavorsFirst:
            return `Troca favorece ${firstPlayer}`;
        case Fairness.SlightlyFavorsFirst:
            return `Troca favorece ${firstPlayer} ligeiramente`;
        case Fairness.FavorsSecond:
            return `Troca favorece ${secondPlayer}`;
        case Fairness.SlightlyFavorsSecond:
            return `Troca favorece ${secondPlayer} ligeiramente`;
        default:
            return "";
    }
};
