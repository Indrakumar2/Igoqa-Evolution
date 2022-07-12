import React, { Component } from 'react';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { getlocalizeData,isEmpty } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();
class AssignmentAnchor extends Component {
    openAssignment = (redirectionURL) => {
        window.open(redirectionURL + '?assignmentId='+this.props.assignmentId +'&assignmentNumber='+ this.props.assignmentNumber,'_blank');  //D-654
    }
    selectedRowHandler = (e,redirectionURL) => {      
        const { assignmentId,assignmentNumber,assignmentData, supplierPOAssignmentData,selectedCompany } = this.props;
        this.props.actions.SaveSelectedAssignmentId({ assignmentId,assignmentNumber });
        //D-654 Start
        let assignmentList = [];
        if(this.props.currentPage === localConstant.sideMenu.EDIT_VIEW_PROJECT)
            assignmentList = assignmentData;
        if(this.props.currentPage === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE)
            assignmentList = supplierPOAssignmentData;
        const assignmentInfo=assignmentList.filter(x=>x.assignmentId === assignmentId);
        if (this.props.currentPage === localConstant.sideMenu.EDIT_VIEW_PROJECT || this.props.currentPage === localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO_CURRENTPAGE) {
            if ((!isEmpty(assignmentInfo) && assignmentInfo.length > 0 &&
                (assignmentInfo[0].assignmentContractHoldingCompanyCode === selectedCompany || assignmentInfo[0].assignmentOperatingCompanyCode === selectedCompany))) {
                this.openAssignment(redirectionURL);
            }
            else {
                IntertekToaster(localConstant.validationMessage.ONLY_CH_AND_OC_ALLOWED, 'warningToast');
            }
        }
        else {
            this.openAssignment(redirectionURL);
        }   
        //D-654 End   
    }

    render() {      
        let redirectionURL = AppMainRoutes.editAssignment;
        const assignmentNumber = this.props.assignmentNumber;
        //let disabledClass=  this.props.pageMode !== "View" ?"link":"isDisabled";
        let disabledClass="link";
        /*Start -- Code added for Timesheet module*/
        // D-138
        if (this.props.location.pathname === '/Timesheet/SearchAssignment' ){
            redirectionURL = AppMainRoutes.timesheetDetails;
        }
        /*End -- Code added for timesheet module*/

        /*Start -- Code added for Visit module*/
        // D-138
        if (this.props.location.pathname === '/Visit/SearchAssignment'){            
            redirectionURL = AppMainRoutes.visitDetails;
        }
        /*End -- Code added for Visit module*/

        /*Start -- Code added for Project module*/
        // ITK D-654 
        if (this.props.location.pathname === '/ProjectDetails'  
        && this.props.currentPage===localConstant.sideMenu.EDIT_VIEW_PROJECT ) {            
            disabledClass="link";
        }
        /*End -- Code added for Project module*/
  
        return (
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(e,redirectionURL)} className={ disabledClass }>{assignmentNumber}</a> //D-654
        );
    }
}

export default AssignmentAnchor;