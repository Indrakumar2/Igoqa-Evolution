import RateSchedule from './rateSchedule';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AdminRateScheduleSelect,
         CopyChargeTypeModalState,
         DeleteRateSchedule,
         DeleteChargeType,
         UpdateChargeType,
         AddChargeType,
         UpdateRateSchedule,
         EditRateSchedule,
         AddRateSchedule,
         RateScheduleEditCheck,
         ChargeTypeEditCheck,
         RateScheduleModalState,
         ChargeTypeModalState,
         AdminContractRatesModalState,
         AdminChargeScheduleValueChange,
         UpdateAllChargeRates,
         UpdateSelectedSchedule,
         AdminRateScheduleSelectAll,
        ChargeRates,UpdateChargeRatesTypes,
        ClearChargeRates,
        isRelatedFrameworkExisits,
        isDuplicateFrameworkContractSchedules,
        frameworkScheduleCopyBatch, checkIfBatchProcessCompleted } from '../../../../actions/contracts/rateScheduleAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { FetchAdminChargeRates,FetchAdminInspectionGroup,FetchAdminInspectionType,FetchAdminSchedule, ClearAdminChargeRates,ClearAdminContractRates,FetchExpenseType } from '../../../../common/masterData/masterDataActions';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';
/*AddScheduletoRF*/
import { AddShedudletoIFContract } from '../../../../actions/contracts/contractAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    const scheduleBatchData = isEmptyReturnDefault(state.batchProcessReducer.batchData, 'object');
    return {
        isRateScheduleEdit:state.RootContractReducer.ContractDetailReducer.isRateScheduleEdit,
        isChargeTypeEdit:state.RootContractReducer.ContractDetailReducer.isChargeTypeEdit,
        isRateScheduleModalOpen:state.RootContractReducer.ContractDetailReducer.isRateScheduleOpen,
        isChargeTypeModalOpen:state.RootContractReducer.ContractDetailReducer.isChargeTypeOpen,
        isCopyChargeTypeModalOpen:state.RootContractReducer.ContractDetailReducer.isCopyChargeTypeOpen,
        isAdminContractRatesModalOpen:state.RootContractReducer.ContractDetailReducer.isAdminContractRatesOpen,
        rateSchedule:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules),
        chargeTypes:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates),
        rateScheduleOnCancel:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.ContractSchedulesOnCancel),
        chargeTypesOnCancel:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.ContractScheduleRatesOnCancel),
        currency:isEmptyReturnDefault(state.masterDataReducer.currencyMasterData),
        contractChargeTypes:state.masterDataReducer.expenseType,
        rateScheduleEditData: state.RootContractReducer.ContractDetailReducer.rateScheduleEditData,
        chargeTypeEditData: state.RootContractReducer.ContractDetailReducer.chargeTypeEditData,
        selectedContract:state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
        contractInfo: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
        adminSchedules: state.masterDataReducer.adminSchedule,
        adminInspectionGroup: state.masterDataReducer.adminInspectionGroup,
        adminInspectionType: state.masterDataReducer.adminInspectionType,
        adminChargeRates: state.masterDataReducer.adminChargeRate,
        adminSelectedChargeRates: state.RootContractReducer.ContractDetailReducer.adminRateToCopy,
        currentPage: state.RootContractReducer.ContractDetailReducer.currentPage,
        loggedInUser: state.appLayoutReducer.loginUser,  
        contractRates:state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates ,
        pageMode:state.CommonReducer.currentPageMode,
        adminChargeScheduleValueChange:state.RootContractReducer.ContractDetailReducer.AdminChargeScheduleValueChange,
        adminChargeRatesValue:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.adminChargeRatesValue),
        selectedRowIndex: state.RootContractReducer.ContractDetailReducer.selectedRowIndex,
        isScheduleBatchProcess: (scheduleBatchData.processStatus == 0 || scheduleBatchData.processStatus == 1) ? true : false,
        scheduleBatch : scheduleBatchData,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {          
                RateScheduleEditCheck,      
                ChargeTypeEditCheck,
                RateScheduleModalState,
                ChargeTypeModalState,
                AddRateSchedule,
                EditRateSchedule,
                UpdateRateSchedule,
                AddChargeType,
                UpdateChargeType,
                DeleteChargeType,
                DisplayModal,
                HideModal,
                DeleteRateSchedule,
                CopyChargeTypeModalState,
                FetchExpenseType,
                FetchAdminChargeRates,
                FetchAdminInspectionGroup,
                FetchAdminInspectionType,
                FetchAdminSchedule,
                AdminContractRatesModalState,
                AdminRateScheduleSelect,
                ClearAdminChargeRates,
                /*AddScheduletoRF*/
                AddShedudletoIFContract,
                AdminChargeScheduleValueChange,
                ChargeRates,
                UpdateChargeRatesTypes,
                UpdateAllChargeRates,
                UpdateSelectedSchedule,
                AdminRateScheduleSelectAll,
                ShowLoader,
                HideLoader,
                ClearAdminContractRates,
                ClearChargeRates,
                isRelatedFrameworkExisits,
                isDuplicateFrameworkContractSchedules,
                frameworkScheduleCopyBatch,
                checkIfBatchProcessCompleted
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(RateSchedule);