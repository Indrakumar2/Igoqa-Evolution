import SelectDataType from './selectDataType';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import {
    UpdateDetailPayRate,
    UpdatePayRateSchedule
} from '../../../grm/actions/techSpec/payRateAction';
import { isEmptyReturnDefault,getlocalizeData } from '../../../utils/commonUtils';
import { filterExpenseType } from '../../../selectors/techSpechSelector';
const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
    return {
        expenseType: state.masterDataReducer.expenseType && state.masterDataReducer.expenseType.filter(x => x.chargeType !== "Q" && x.chargeType !== "C"),
        DetailPayRate: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate),
        currency:isEmptyReturnDefault(state.masterDataReducer.currencyMasterData),
        payRateSchedule: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule),
        payRateType:filterExpenseType( { array: state.masterDataReducer.expenseType ,param:"NDT" }), //Changes for D1089 (Ref ALM 22-05-2020 Doc)
    };    
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                UpdateDetailPayRate,
                UpdatePayRateSchedule
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(SelectDataType);