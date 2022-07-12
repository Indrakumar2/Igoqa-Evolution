import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { ObjectIntoQuerySting } from '../../../../utils/commonUtils';

class SupplierpoAnchor extends Component {
    selectedRowHandler = () => {             
        //this.props.actions.FetchSupplierPoData(this.props);
        const redirectionURL = AppMainRoutes.supplierPoDetails;
        const supplierPOId = this.props.data.supplierPOId;
        const queryObj={
            supplierPOId:supplierPOId && supplierPOId,
            selectedCompany:this.props.selectedCompany,
            chCompany:this.props.data.supplierPOCompanyCode
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        this.openSupplierPO(redirectionURL,queryStr);
    }

    openSupplierPO = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    render() {        
        
        const supplierPONumber = this.props.data.supplierPONumber;
        return (              
                // <Link target='_blank' //Changes for ITK D1531
                //     to={{ 
                //     pathname: redirectionURL , search:`?supplierPOId=${
                //     supplierPOId }&selectedCompany=${ this.props.selectedCompany }&chCompany=${ this.props.data.supplierPOCompanyCode } ` //IGO QC D-900 Issue 2
                //     }} 
                //     onClick={this.selectedRowHandler} 
                //     className="link"
                //     >{supplierPONumber}
                // </Link>
                <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler()} className="link" >{ supplierPONumber }</a>
        );
    }
}

export default SupplierpoAnchor;