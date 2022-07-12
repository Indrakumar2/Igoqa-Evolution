import React, { Component, Fragment } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import {
    //  PayRateDetailsModalOpen, DetailPayRateCheck
    EditDetailPayRate } from '../../../actions/techSpec/payRateAction';

// import { EditResouceStatus } from '../../../actions/techSpec/resourceStatusAction';
// import {
    // EditLanguageCapability,
    // EditCertificateDetails,
    // EditCommodityDetails,
    // EditTrainingDetails,
// } from '../../../actions/techSpec/resourceCapabilityAction';

// import {
//     EditWorkHistoryDetails
    
// } from '../../../actions/techSpec/professionalDetailAction';

// import {
//     EditSensitiveDetails
// } from '../../../actions/techSpec/SensitiveDocumentsAction';

const mapStateToProps = (state) => {
    return {
        interactionMode: state.RootContractReducer.ContractDetailReducer.interactionMode,
        editContractDocumentDetails: state.RootContractReducer.ContractDetailReducer.editContractDocumentDetails,
    };
};

class GridEditRenderer extends Component {
    selectedRowHandler = () => {
        this.props[this.props.action](this.props.data);               
    }

    render() {
        const { interactionMode } = this.props;
        return (
            <a className={"waves-effect waves-light modal-trigger " + (interactionMode === true ? "isDisabled" : null)} onClick={this.selectedRowHandler} >{this.props.label ? this.props.label : "Edit"}</a>
        );
    }
}

export default connect(mapStateToProps, {
    
    //taxonomy
    //Resource Status
    // EditResouceStatus, 

    // EditWorkHistoryDetails,  
    // EditEducationalDetails,
    
    //Resource Capability
    // EditLanguageCapability,
    // EditCertificateDetails,
    // EditCommodityDetails,
    // EditTrainingDetails,
    //taxonomy approval
    // EditTaxonomyApprovalDetails,

    //comments

})(GridEditRenderer);
