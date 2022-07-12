import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Link,withRouter } from 'react-router-dom';
import { GetSelectedCustomerCode } from '../customerAction';
import{ ObjectIntoQuerySting }from '../../../../utils/commonUtils';
import { AppMainRoutes } from '../../../../routes/routesConfig';

class CustomerAnchor extends Component {     
    selectedRowHandler = (redirectionURL, queryStr) => {                
            //this.props.GetSelectedCustomerCode(this.props.data.customerCode);
        this.openCustomer(redirectionURL, queryStr);
    }

    openCustomer = (redirectionURL,queryStr) => {
      window.open(redirectionURL + '?'+queryStr,'_blank');
  };

    render() {
        const redirectionURL = AppMainRoutes.customerDetails;
        const queryObj = {
          customerCode:this.props.data.customerCode,
          selectedCompany:this.props.selectedCompany
        };
        const queryStr = ObjectIntoQuerySting(queryObj);  
        return (
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(redirectionURL,queryStr)} className="link" >{ this.props.data.customerCode }</a>
            // <Link target="_blank" to={{ pathname:redirectionURL, search:`?`+queryStr }} onClick={this.selectedRowHandler}  className="link">{this.props.data.customerCode}</Link>
        );
    }
}

const mapStateToProps = (state) => {
    return {
      selectedCompany:state.appLayoutReducer.selectedCompany,
    };
  };

export default withRouter(connect(mapStateToProps, { GetSelectedCustomerCode })(CustomerAnchor));