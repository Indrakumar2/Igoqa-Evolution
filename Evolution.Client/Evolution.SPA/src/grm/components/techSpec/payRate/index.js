import PayRate from './payRate';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault,getlocalizeData } from '../../../../utils/commonUtils';

import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
  EditPayRateSchedule,
  UpdatePayRateSchedule,
  AddPayRateSchedule,
  DeletePayRateSchedule,
  AddDetailPayRate,
  ClearDetailPayRate,
  UpdateDetailPayRate,
  DeleteDetailPayRate,
  UpdatePayRate,
  FetchPayRollNameByCompany,
} from '../../../actions/techSpec/payRateAction';
import { userTypeCheck,filterExpenseType } from '../../../../selectors/techSpechSelector';
//import { FetchPayRollName } from '../../../../common/masterData/masterDataActions';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
  return {
    isbtnDisable: state.RootTechSpecReducer.TechSpecDetailReducer.isbtnDisable,
    currencyData: state.masterDataReducer.currencyMasterData,
    payRateSchedule: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPaySchedule),
    ispayRateScheduleEdit: state.RootTechSpecReducer.TechSpecDetailReducer.ispayRateScheduleEdit,
    rateScheduleEditData: state.RootTechSpecReducer.TechSpecDetailReducer.rateScheduleEditData,
    DetailPayRate: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistPayRate),
    payrollName: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.payRollNameByCompany),
    expenseType: state.masterDataReducer.expenseType,
    technicalSpecialistInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, 'object'),
    oldProfileActionType: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,
    pageMode:state.CommonReducer.currentPageMode,
    activities:state.appLayoutReducer.activities,
    payRateScheduleOnCancel: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.TechnicalSpecialistPayScheduleOnCancel),
    DetailPayRateOnCancel: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.TechnicalSpecialistPayRateOnCancel),
    isRCUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RC }),
    isRMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RM }),
    isTMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TM }),
    isTSUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TS }),
    isMIUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.userTypeList.MICoordinator }),
    isOMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.userTypeList.OperationManager }),
    payRateType:filterExpenseType( { array: state.masterDataReducer.expenseType ,param:"NDT" }), //Added for D1089 (Ref ALM 22-05-2020 Doc)
  };
};

const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {
        EditPayRateSchedule,
        UpdatePayRateSchedule,
        AddPayRateSchedule,
        DeletePayRateSchedule,
        DisplayModal,
        HideModal,
        FetchPayRollNameByCompany,
        //FetchPayRollName,
        AddDetailPayRate,
        ClearDetailPayRate,
        UpdateDetailPayRate,
        DeleteDetailPayRate,
        UpdatePayRate
      },
      dispatch

    )
  };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(PayRate)); 
