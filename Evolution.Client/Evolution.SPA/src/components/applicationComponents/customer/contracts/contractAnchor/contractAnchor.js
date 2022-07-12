import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Link,withRouter } from 'react-router-dom';
import { GetSelectedContractNumber } from '../../../../viewComponents/customer/customerAction';
class ContractAnchor extends Component {     
    selectedRowHandler = () => {                
            this.props.GetSelectedContractNumber(this.props.data.contractNumber);
    }

    render() {        
        return (
            <Link to={'/contracts'} onClick={this.selectedRowHandler}  className="waves-effect waves-light">{this.props.data.contractNumber}</Link>
                  );
    }
}
export default connect(null, { GetSelectedContractNumber })(withRouter( ContractAnchor));