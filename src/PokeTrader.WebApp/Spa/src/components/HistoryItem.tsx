import React, { Component } from 'react'
import { Alert, Badge, Card, Col, Collapse, Container, ListGroup, Row } from 'react-bootstrap';
import { TradeDto } from '../services/tradeService';
import { convertToText } from './Trade';

type HistoryItemProps = {
    tradeHistory: TradeDto
}

export default class HistoryItem extends Component<HistoryItemProps> {
    state = { open: false };

    render() {
        const trade = this.props.tradeHistory
        const first = trade.info.first;
        const second = trade.info.second;
        return (
            <ListGroup.Item action onClick={() => this.setState({ open: !this.state.open})}>
                    <Alert variant="dark" className="p2">
                        <Badge variant="light">{new Date(trade.tradeDate).toLocaleString()}</Badge>
                         -  {first.trader.name} <Badge variant="light">X</Badge> {second.trader.name} - <Badge variant="primary">{convertToText(trade.info.tradeFairness, first.trader.name, second.trader.name)}</Badge>
                    </Alert>
                <Collapse in={this.state.open}>
                    <div>
                        <Container>
                            <Row>
                                <Col>
                                    {first.tradeOffers.reduce((prev, curr) => `${prev} ${curr.name}`, "")} - {trade.info.firstScore} 
                                </Col>
                                <Col>
                                    {second.tradeOffers.reduce((prev, curr) => `${prev} ${curr.name}`, "")} - {trade.info.secondScore} 
                                </Col>
                            </Row>
                        </Container>
                    </div>
                </Collapse>
            </ListGroup.Item>
        )
    }
}
