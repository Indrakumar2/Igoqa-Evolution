import ContractDetails from './contractDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { DisplayModal, 
    HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { UpdateInteractionMode, 
    FetchContractData, 
    SaveContractDetails, 
    DeleteContractDetails, 
    CancelSearchDetails, 
    CancelContractDataOnEdit, 
    ClearContractData,
    GetSelectedContract,ClearContractDocument } from '../../../../actions/contracts/contractAction';
import { EditContract,CreateContract } from '../../../sideMenu/sideMenuAction';
import { GetSelectedCustomerName } from '../../../../actions/contracts/contractSearchAction';
import { CreateProject } from '../../../sideMenu/sideMenuAction';
import { isEmpty,isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DeleteAlert } from '../../customer/alertAction';
import { FetchCompanyOffices } from '../../../../actions/contracts/generalDetailsAction';
import { ClearData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { ClearAdminChargeRates } from '../../../../common/masterData/masterDataActions';
import { ClearAdminChargeScheduleValueChange,
    AdminContractRatesModalState } from '../../../../actions/contracts/rateScheduleAction';
import { FetchBatchProcessData } from '../../../../actions/batchProcess/batchProcessAction';

const mapStateToProps = (state) => {
    const scheduleBatchData = isEmptyReturnDefault(state.batchProcessReducer.batchData, 'object');
    return {
        interactionMode: state.RootContractReducer.ContractDetailReducer.interactionMode,
        currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
        isbtnDisable: state.RootContractReducer.ContractDetailReducer.isbtnDisable,
        isOpen: state.RootContractReducer.ContractDetailReducer.isOpen,
        selectedContract: isEmpty(state.RootContractReducer.ContractDetailReducer.selectedContract)?[]:state.RootContractReducer.ContractDetailReducer.selectedContract,
        contractDetails: isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,'object'),
        contractDetailTabs: state.RootContractReducer.ContractDetailReducer.contractDetailTabs,
        selectedContractType: state.RootContractReducer.ContractDetailReducer.selectedContractType,
        ContractDocuments: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments,
        loader:state.CommonReducer.loader, 
        contractSchedules: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules,
        contractRates:state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates,
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        contractprojectDetail:isEmptyReturnDefault(state.ContractProjectReducer.ContractProjects),
        scheduleBatch : scheduleBatchData,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UpdateInteractionMode,
                FetchContractData,
                SaveContractDetails,
                DeleteContractDetails,
                EditContract,
                CreateContract,
                DisplayModal,
                CancelSearchDetails,
                HideModal,
                GetSelectedCustomerName,                
                CreateProject,
                CancelContractDataOnEdit,
                FetchCompanyOffices,
                ClearData,
                ClearAdminChargeRates,
                ClearAdminChargeScheduleValueChange,
                AdminContractRatesModalState,
                ClearContractData,
                DeleteAlert,
                GetSelectedContract,
                ClearContractDocument,
                FetchBatchProcessData
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ContractDetails));