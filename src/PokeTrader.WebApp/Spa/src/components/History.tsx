import React, { Component } from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { Fairness, TradeInfoDto } from '../services/tradeService';

export class History extends Component {
    static displayName = History.name;
    constructor(props: {} | Readonly<{}>) {
        super(props);
    }

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
                    <span>{this.state.fairnessText}</span>
                </Row>
            </Container>
        );
    }

}
