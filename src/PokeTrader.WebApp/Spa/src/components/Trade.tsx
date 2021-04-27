import React, { Component } from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { TradeInfoDto } from '../services/tradeService';

const PENDING_APPROVAL = "PendingApproval";
const PENDING_CONFIRMATION = "PendingConfirmation";
export class Trade extends Component {
    static displayName = Trade.name;
    state = { tradeText: 'Avaliar troca' }
    
    tradeService = tradeServInstance;
    tradeState: string = PENDING_APPROVAL;
    tradeInfo: TradeInfoDto = {} as TradeInfoDto;

    render() {
        return (
            <Container fluid>
                <Row className="justify-content-md-right">
                    <Col>
                        <Button variant="outline-primary" size="lg" onClick={this.nextTradeState.bind(this)}>{this.state.tradeText}</Button>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <PlayerSelector placeholder="Primeiro Jogador" initialValue="Jogador 1"></PlayerSelector>
                        <PokemonSelector key="first" />
                    </Col>
                    <Col>
                        <PlayerSelector placeholder="Segundo Jogador" initialValue="Jogador 2"></PlayerSelector>
                        <PokemonSelector key="second" />
                    </Col>
                </Row>
            </Container>
        );
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
    async nextTradeState() {
        if (this.tradeState === PENDING_CONFIRMATION) {
            if (typeof this.tradeInfo != typeof undefined) {
                await this.tradeService.makeTrade(this.tradeInfo);
            }
        }
        else if (this.tradeState === PENDING_APPROVAL) {

        }
    }
    
}
