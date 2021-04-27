import React, { Component } from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';


export class Trade extends Component {
    static displayName = Trade.name;
    state = { tradeText: 'Avaliar troca' }

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
                        <Form.Control placeholder="Primeiro Jogador"></Form.Control>
                        <PokemonSelector key="first" />
                    </Col>
                    <Col>
                        <Form.Control placeholder="Segundo Jogador"></Form.Control>
                        <PokemonSelector key="second" />
                    </Col>
                </Row>
            </Container>
        );
    }
    noOptions() {
        return "Nenhum pokemon encontrado"
    }
    nextTradeState() {
    }
    
}
