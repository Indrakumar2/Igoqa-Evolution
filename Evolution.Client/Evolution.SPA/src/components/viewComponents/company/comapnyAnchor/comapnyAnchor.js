import React, { Component } from 'react';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { GetSelectedCompanyCode } from '../companyAction';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { ObjectIntoQuerySting } from '../../../../utils/commonUtils';

class CompanyAnchor extends Component {     
    selectedRowHandler = () => {                
        //this.props.GetSelectedCompanyCode(this.props.data.companyCode);
        const redirectionURL = AppMainRoutes.companyDetails;
        const companyCode = this.props.data.companyCode;
        const queryObj={
            companyCode:companyCode && companyCode,
            selectedCompany:this.props.selectedCompany
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        this.openCompany(redirectionURL,queryStr);
    }

    openCompany = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    render() {
        //const redirectionURL = this.props.redirectionURL ?this.props.redirectionURL : "companyDetails";
        return (
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler()} className="link" >{ this.props.data.companyCode }</a>
            // <Link target="_blank" to={{ pathname:redirectionURL, search:`?companyCode=${ this.props.data.companyCode }&selectedCompany=${ this.props.selectedCompany }` }} onClick={this.selectedRowHandler}  className="link">{this.props.data.companyCode}</Link>            
            // <Link to={redirectionURL} onClick={this.selectedRowHandler}  className="link">{this.props.data.companyCode}</Link>
        );
    }
}
const mapStateToProps = (state) => {
    return{
        selectedCompany: state.appLayoutReducer.selectedCompany,
    };
};
export default withRouter(connect(mapStateToProps, { GetSelectedCompanyCode })(CompanyAnchor));