import React, { Component } from 'react';
import { Button, Col, Container, Form, ListGroup, Row } from 'react-bootstrap';
import { PokemonSelector } from './PokemonSelector';
import { PlayerSelector } from './PlayerSelector';
import tradeServInstance, { Fairness, TradeDto, TradeInfoDto } from '../services/tradeService';
import HistoryItem from './HistoryItem';

export class History extends Component {
    static displayName = History.name;
    constructor(props: {} | Readonly<{}>) {
        super(props);
    }
    state = { history: [] };
    async componentDidMount() {
        const history = await tradeServInstance.getHistory();
        this.setState({ history: history });
    }

    render() {

        let history = this.state.history;
        if (typeof history === typeof undefined || typeof history.map === typeof undefined)
            history = [];
        return (
            <ListGroup>
                {history.map((hst, i) => {
                    return <HistoryItem tradeHistory={hst} key={i} />
                })}
            </ListGroup>
        );
    }

}

