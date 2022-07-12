import SelectContractRate from './selectContractRate';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import { 
    UpdateChargeType,
    UpdateRateSchedule
 } from '../../../actions/contracts/rateScheduleAction';
 import { isEmptyReturnDefault } from '../../../utils/commonUtils';

const mapStateToProps = (state) => {    
    const scheduleBatchData = isEmptyReturnDefault(state.batchProcessReducer.batchData, 'object');
    return {        
        contractChargeTypes:state.masterDataReducer.expenseType,
        currency:isEmptyReturnDefault(state.masterDataReducer.currencyMasterData),
        chargeTypes:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractScheduleRates),
        rateSchedule:isEmptyReturnDefault(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractSchedules),
        contractInfo: state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo,
        isScheduleBatchProcess: (scheduleBatchData.processStatus == 0 || scheduleBatchData.processStatus == 1) ? true : false,
    };    
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                UpdateChargeType,
                UpdateRateSchedule
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(SelectContractRate);