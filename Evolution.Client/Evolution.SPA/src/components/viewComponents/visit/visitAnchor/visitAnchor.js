import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { ObjectIntoQuerySting } from '../../../../utils/commonUtils';

class VisitAnchor extends Component {

    selectedRowHandler = (e,redirectionURL,queryStr) => {
        this.props.actions.GetSelectedVisit(this.props.data);
        this.props.actions.SetCurrentPageMode();
        this.openVisit(redirectionURL,queryStr);
    }

    openVisit = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    render() {
        const redirectionURL = AppMainRoutes.visitDetails;
        const linkText = this.props.displayLinkText ? this.props.displayLinkText : this.props.data.visitNumber;
        const queryObj = {
            vId: this.props.data.visitId,
            vAssId: this.props.data.visitAssignmentId,
            vSPOId: this.props.data.visitSupplierPOId,
            vProNo:this.props.data.visitProjectNumber
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        return (
            //  <Link target='_blank'  to={{ pathname:redirectionURL, search: `?vId=${ this.props.data.visitId }&vAssId=${ this.props.data.visitAssignmentId }&vSPOId=${ this.props.data.visitSupplierPOId }&vProNo=${ this.props.data.visitProjectNumber }` }} onClick={this.selectedRowHandler}  className="link">{linkText}</Link>
            <a href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(e, redirectionURL, queryStr)} className="link" >{linkText}</a>

        );
    }
}

export default VisitAnchor;