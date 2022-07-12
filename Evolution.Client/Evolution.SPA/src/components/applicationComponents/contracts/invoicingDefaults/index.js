import InvoicingDefaults from './invoicingDefaults';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
    FetchInvoicingDefaults,    
    InvoiceAttachmentTypesModalState,
    InvoiceAttachmentTypesEditCheck,
    InvoiceReferenceModalState,
    InvoiceReferenceEditCheck,
    AddNewInvoiceDefault,
    DeleteInvoiceDefault,
    AddAttachmentTypes,
    DeleteAttachmentTypes,
    UpdateInvoiceDefault,
    UpdateAttachmentTypes,
    AddUpdateInvoicingDefaults
} from '../../../../actions/contracts/invoicingDefaultsAction';
import { filterDocTypes } from '../../../../common/selector';

const mapStateToProps = (state) => {
    return {
        customerContractContact: state.RootContractReducer.ContractDetailReducer.customerContractContact,
        taxes: state.RootContractReducer.ContractDetailReducer.taxes,        
        invoicePaymentTerms: state.masterDataReducer.invoicePaymentTerms,
        customerContractAddress: state.RootContractReducer.ContractDetailReducer.customerContractAddress,
        invoiceCurrency: state.masterDataReducer.currencyMasterData,
        invoiceGrouping: state.RootContractReducer.ContractDetailReducer.invoiceGrouping,        
        invoiceRemittanceandFooterText: state.RootContractReducer.ContractDetailReducer.invoiceRemittanceandFooterText,        
        defaultInvoiceRefernces: (state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences === null || state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences === undefined) ? [] : state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceReferences,
        defaultInvoiceAttachmentTypes: (state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments === null || state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments === undefined) ? [] : state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInvoiceAttachments,
        referenceType: state.masterDataReducer.referenceType,        
        loggedInUser: state.appLayoutReducer.loginUser,
        selectedCustomerCode: state.RootContractReducer.ContractDetailReducer.selectedCustomerCode,
        isInvoiceReferenceModalOpen: state.RootContractReducer.ContractDetailReducer.isInvoiceReferenceModalOpen,
        isInvoiceReferenceEdit: state.RootContractReducer.ContractDetailReducer.isInvoiceReferenceEdit,
        isInvoiceAttachmentTypesModalOpen: state.RootContractReducer.ContractDetailReducer.isInvoiceAttachmentTypesModalOpen,
        isInvoiceAttachmentTypesEdit: state.RootContractReducer.ContractDetailReducer.isInvoiceAttachmentTypesEdit,
        //contractDocumentTypeMasterData: state.masterDataReducer.attachmentTypeData,
        contractDocumentTypeMasterData: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        editInvoiceReferences: state.RootContractReducer.ContractDetailReducer.editInvoiceReferences,
        editAttachmentTypes: state.RootContractReducer.ContractDetailReducer.editAttachmentTypes,
        invoicingContractInfo: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
        currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
        generalDetailsCreateContractCustomerDetails: state.RootContractReducer.ContractDetailReducer.customerName,
        selectedContract:state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
        pageMode:state.CommonReducer.currentPageMode
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchInvoicingDefaults,                
                InvoiceReferenceModalState,
                InvoiceReferenceEditCheck,
                InvoiceAttachmentTypesModalState,
                InvoiceAttachmentTypesEditCheck,
                AddNewInvoiceDefault,
                DeleteInvoiceDefault,
                AddAttachmentTypes,
                DeleteAttachmentTypes,
                UpdateInvoiceDefault,
                UpdateAttachmentTypes,
                AddUpdateInvoicingDefaults,            
                DisplayModal,
                HideModal,
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(InvoicingDefaults);