import React, { Component, Fragment } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import {
    EditDocumentDetails,
    EditAssignmentReference,
    EditAccountReference,
    EditAddressReference,
    EditContactReference,
    ExtranetUserAccountModalState,
    AddExtranetUser
} from "../../../components/viewComponents/customer/customerAction";
import { SelectedDocumentToApprove, RejectDocument } from '../../../components/viewComponents/dashboard/dahboardActions';
import {
    EditCompanyDocumentDetails,
    EditCompanyOffice,
    EditInvoiceFooter,
    EditInvoiceRemittance,
    EditExpectedMargin,
    EditCompanyDivisionCostcentre,
    EditPayrollPeriodName,
    EditCompanyTaxes
} from '../../../components/viewComponents/company/companyAction';
// import { EditFixedExchangeRate } from '../../viewComponents/contracts/contractAction';
import { ChargeTypeModalState, ChargeTypeEditCheck, EditChargeType } from '../../../actions/contracts/rateScheduleAction';
import {
    EditInvoiceDefault,
    InvoiceReferenceEditCheck,
    InvoiceReferenceModalState,
    EditAttachmentTypes,
    InvoiceAttachmentTypesEditCheck,
    InvoiceAttachmentTypesModalState
} from '../../../actions/contracts/invoicingDefaultsAction';
import {
    EditedClientNotificationData
} from '../../../actions/project/clientNotificationAction';
import { EditFixedExchangeRate, ExchangeRateEditCheck, ExchangeRateModalState } from '../../../actions/contracts/fixedExchangeRatesAction';
const mapStateToProps = (state) => {
    return {
        interactionMode: state.RootContractReducer.ContractDetailReducer.interactionMode,
        pageMode: state.CommonReducer.currentPageMode
    };
};

class EditRenderer extends Component {
    selectedRowHandler = () => {       
        if(this.props.action === "EditInvoiceRemittance" || this.props.action === "EditInvoiceFooter"){
            document.getElementById("remittanceTextPopup").reset();
            document.getElementById("invoiceFooterTextPopup").reset();
        }        
        else if (this.props.popupAction) {
            this.props[this.props.popupAction](true);
        }
        if (this.props.buttonAction) {
            this.props[this.props.buttonAction](true);            
        } 
        this.props[this.props.action](this.props.data);

    }

    render() {        
        const { interactionMode,pageMode } = this.props;
        return (
            <a className={"waves-effect waves-light modal-trigger " + (interactionMode === true||pageMode==="View" ? "isDisabled" : null)} onClick={this.selectedRowHandler} href={(this.props.popupId ? "#" + this.props.popupId : "javascript:void(0)")} disabled={interactionMode === true||pageMode==="View"?true:false} >{this.props.label ? this.props.label : "Edit"}</a>
        );
    }
}

export default connect(mapStateToProps, {
    EditDocumentDetails,
    EditCompanyDocumentDetails,
    EditAssignmentReference,
    EditAccountReference,
    EditAddressReference,
    EditContactReference,
    EditCompanyOffice,
    EditExpectedMargin,
    EditInvoiceFooter,
    EditInvoiceRemittance,
    EditCompanyDivisionCostcentre,
    EditPayrollPeriodName,
    EditCompanyTaxes,    
    SelectedDocumentToApprove,
    EditFixedExchangeRate,
    ChargeTypeEditCheck,
    ChargeTypeModalState,
    RejectDocument,
    EditInvoiceDefault,
    InvoiceReferenceEditCheck,
    InvoiceReferenceModalState,
    ExchangeRateEditCheck,
    ExchangeRateModalState,
    EditAttachmentTypes,
    InvoiceAttachmentTypesEditCheck,
    InvoiceAttachmentTypesModalState,
    EditChargeType,
    EditedClientNotificationData,
    ExtranetUserAccountModalState,
    AddExtranetUser

})(EditRenderer);
