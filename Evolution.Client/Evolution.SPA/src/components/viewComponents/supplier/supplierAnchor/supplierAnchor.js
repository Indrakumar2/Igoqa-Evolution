import React,{ Component,Fragment } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { getlocalizeData } from '../../../../utils/commonUtils';
import { required } from '../../../../utils/validator';
import { ObjectIntoQuerySting } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class supplierAnchor extends Component{
//commented due to unneccessary action calls
    // selectedRowHandler = () => {
    //     //this.props.actions.GetSelectedSupplier(this.props.data);
    //     // this.props.actions.SetCurrentPageMode();
    //     // this.props.actions.UpdateCurrentPage(localConstant.supplier.EDIT_VIEW_SUPPLIER);
    //     // this.props.actions.UpdateCurrentModule(localConstant.moduleName.SUPPLIER);
    // }

    selectedRowHandler = () => {
        const redirectionURL = AppMainRoutes.supplierDetail;
        const supplierId =  this.props.data.supplierId;
        const queryObj={
            supplierId:supplierId && supplierId,
            selectedCompany:this.props.selectedCompany,
            operatingCountry:this.props.selectedCompanyData.operatingCountry
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        this.openSupplier(redirectionURL,queryStr);
    }

    openSupplier = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    render(){
        const { supplierIdKey, supplierNameKey } = this.props;
        const supplierId = !required(supplierIdKey) ? this.props.data[ supplierIdKey ] : this.props.data.supplierId;
        const supplierName = !required(supplierNameKey) ? this.props.data[ supplierNameKey ] : this.props.data.supplierName;
        return(
            // <Link to={{ pathname:redirectionURL }} onClick={this.selectedRowHandler}  className="link">{this.props.data.supplierName}</Link>
            // <Link 
            //     target='_blank' 
            //     to={{ pathname:redirectionURL, search:`?supplierId=${ supplierId }&selectedCompany=${ this.props.selectedCompany }&operatingCountry=${ this.props.selectedCompanyData.operatingCountry }` }} 
            //     className={supplierId ? "link" :"link isDisabled waves-effect" }>
            //         {supplierName}
            // </Link> //Changes for IGO D979
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler()} className={supplierId ? "link" :"link isDisabled waves-effect" }>{ supplierName }</a>            
        );
    }
}

export default supplierAnchor;