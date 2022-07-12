import React, { Component, Fragment } from 'react';
import ContractList from '../../../components/viewComponents/contracts/contractList';

class Contract extends Component {

    render() {
        return (
            <Fragment >
                <ContractList />
            </Fragment>
        );
    }
}

export default Contract;