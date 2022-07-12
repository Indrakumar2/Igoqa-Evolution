import InvoicingDefaults from './invoicingDefaults';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmpty,isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchInvoicingDefaults,    
    AddUpdateInvoicingDefaults,
    AddInvoiceDefault,
    UpdateInvoiceDefault,
    DeleteInvoiceDefault,
    AddAttachmentTypes,
    UpdateAttachmentTypes,
    DeleteAttachmentTypes    
} from '../../../../actions/project/invoicingDefaultAction';
import { filterDocTypes } from '../../../../common/selector';

const mapStateToProps = (state) => {
    const projectInfo = isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo, 'object');
    return {
        customerContractContact: state.RootProjectReducer.ProjectDetailReducer.customerContractContact,
        taxes: state.RootProjectReducer.ProjectDetailReducer.taxes, 
        customerContractAddress: state.RootProjectReducer.ProjectDetailReducer.customerContractAddress,
        invoiceRemittanceandFooterText: state.RootProjectReducer.ProjectDetailReducer.invoiceRemittanceandFooterText,
        invoicePaymentTerms: state.masterDataReducer.invoicePaymentTerms,
        invoiceCurrency: state.masterDataReducer.currencyMasterData,
        referenceType: state.masterDataReducer.referenceType,
        loggedInUser: state.appLayoutReducer.loginUser,
        defaultInvoiceRefernces: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceReferences),
        defaultInvoiceAttachmentTypes: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInvoiceAttachments),        
        //projectDocumentTypeMasterData: state.masterDataReducer.attachmentTypeData,
        projectDocumentTypeMasterData: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        invoicingProjectInfo: projectInfo,
        parentContractInvoicingInfo: isEmptyReturnDefault(projectInfo.parentContractInvoicingDetails,'object'),
        // interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode,
        currentPage :state.CommonReducer.currentPage
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchInvoicingDefaults,                
                AddUpdateInvoicingDefaults,
                AddInvoiceDefault,
                UpdateInvoiceDefault,
                DeleteInvoiceDefault,
                AddAttachmentTypes,
                UpdateAttachmentTypes,
                DeleteAttachmentTypes,                
                DisplayModal,
                HideModal
            },
            dispatch
        )
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(InvoicingDefaults);