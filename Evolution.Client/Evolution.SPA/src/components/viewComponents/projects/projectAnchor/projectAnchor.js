import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { getlocalizeData,ObjectIntoQuerySting } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();

class ProjectAnchor extends Component {
    openProject = (redirectionURL,queryStr) => {
        window.open(redirectionURL + '?'+queryStr,'_blank');
    };

    selectedRowHandler = (e,redirectionURL,queryStr) => {
        if(this.props.location.pathname === '/SupplierPO/SearchProject'){
            const projectData = this.props.data;
            if(projectData.isContractHoldingCompanyActive){
                this.openProject(redirectionURL,queryStr);
            }
            else {
               IntertekToaster(localConstant.validationMessage.INACTIVE_COMPANY_SUPPLIERPO_CREATION_VAL,"warningToast");
            }
        } else {
            this.openProject(redirectionURL,queryStr);
        }
    }

    render() {
        let redirectionURL = AppMainRoutes.projectDetail;
        const projectNumber = this.props.value;
        const queryObj={
            projectNumber:projectNumber && projectNumber.toString(),
            selectedCompany:this.props.selectedCompany
        };
        let queryStr = ObjectIntoQuerySting(queryObj);  

        /*Start -- Code added for Assignment module And User rightClick open Menu New Tab the PROPS.CURRENT PAGE is Empty.*/
        if ((this.props.location.pathname === '/Assignment/SearchProject' && this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) || (this.props.location.pathname === '/Assignment/SearchProject')) {
            redirectionURL = AppMainRoutes.createAssignment;
        }
        /*End -- Code added for Assignment module*/

        /*Start -- Code added for Supplier PO module And User rightClick open Menu New Tab the PROPS.CURRENT PAGE is Empty.*/
        if ((this.props.location.pathname === '/SupplierPO/SearchProject' && this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE)||(this.props.location.pathname === '/SupplierPO/SearchProject')) {
            redirectionURL = AppMainRoutes.supplierPoDetails;
            const queryObj={
                projectNumber:projectNumber && projectNumber.toString(),
                selectedCompany:this.props.selectedCompany,
                chCompany :this.props.data.contractHoldingCompanyCode
            };
            queryStr = ObjectIntoQuerySting(queryObj);  
        }
        /*End -- Code added for Supplier PO module*/
        
        return (           
            // <Link target='_blank' to={{
            //     pathname: redirectionURL, search: "?"+queryStr
            // }} onClick={this.selectedRowHandler} className="link">{projectNumber}</Link>  /*Changes For D479 issue 1*/
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(e,redirectionURL,queryStr)} className="link" >{ projectNumber }</a>
        );
    }
}

export default ProjectAnchor;