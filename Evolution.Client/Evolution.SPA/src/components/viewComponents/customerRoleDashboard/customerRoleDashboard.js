import React, { Component, Fragment } from 'react';
import { withRouter } from 'react-router-dom';
const DashboardMsg =(props) =>{
    return(
        <Fragment>
            <h6>{props.title}</h6>
            <p className="blogHighlight">{props.dashboardmsg}</p>
        </Fragment>
    );
};

class CustomerRoleDashboard extends Component { 
    render() {
        return (
            <Fragment>               
               <div className="dashboardOtherUser customCard">
                      <DashboardMsg dashboardmsg={this.props.dashboardmsg} title="Customer Message"/>                     
                </div>
            </Fragment>

        );
    }
}

export default withRouter(CustomerRoleDashboard);
