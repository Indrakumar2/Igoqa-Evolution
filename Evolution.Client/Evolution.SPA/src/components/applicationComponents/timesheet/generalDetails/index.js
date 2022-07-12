import GeneralDetails from './generalDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    UpdateTimesheetDetails,
    SelectedTimesheetTechSpecs,
    AddTimesheetTechnicalSpecialist,
    RemoveTimesheetTechnicalSpecialist,
    FetchTimesheetGeneralDetail,
    AddTimesheetCalendarData,
    UpdateTimesheetCalendarData,
    FetchTimesheetCalendarDetail,
    RemoveTSTimesheetCalendarData,
    UpdateTimesheetTechnicalSpecialist
} from '../../../../actions/timesheet/timesheetGeneralDetails';
import {
    UpdateTimesheetExchangeRates,
    FetchUnusedReason
} from '../../../../actions/timesheet/timesheetAction';
import {
    AddTimesheetTechnicalSpecialistTime,
    AddTimesheetTechnicalSpecialistTravel,
    AddTimesheetTechnicalSpecialistExpense,
    AddTimesheetTechnicalSpecialistConsumable,
    DeleteTimesheetTechnicalSpecialistTime,
    DeleteTimesheetTechnicalSpecialistTravel,
    DeleteTimesheetTechnicalSpecialistExpense,
    DeleteTimesheetTechnicalSpecialistConsumable
} from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction';
import {
    getTimesheetStatus,
    isTimesheetCreateMode,
    isCoordinatorCompany
} from '../../../../selectors/timesheetSelector';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';

//Fetch Calendar data
import { FetchCalendarData,
    FetchVisitTimesheetCalendarData,
    FetchPreAssignment,
    FetchTimeOffRequestData,
    FetchVisitByID
 } from '../../../../grm/actions/techSpec/globalCalendarAction';
import { applicationConstants } from '../../../../constants/appConstants';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { 
    FetchCurrencyExchangeRate
} from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction';

const mapStateToProps = (state, ownProps) => {
    const timesheetInfo = isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo, 'object');
    const currentPage = ownProps.currentPage;
    return {
        timesheetInfo: timesheetInfo,
        timesheetStatus: getTimesheetStatus({ ...state.rootTimesheetReducer.timesheetDetail.TimesheetInfo, currentPage }),
        timesheetStatusList: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetStatus),
        timesheetSelectedTechSpecs: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetSelectedTechSpecs),
        timesheetId: timesheetInfo.timesheetId ? timesheetInfo.timesheetId :
            (state.rootTimesheetReducer.selectedTimesheetId) ? state.rootTimesheetReducer.selectedTimesheetId : null,
        isTimesheetCreateMode: isTimesheetCreateMode(currentPage),
        isInterCompanyAssignment: isInterCompanyAssignment(timesheetInfo.timesheetContractCompanyCode,
            timesheetInfo.timesheetOperatingCompanyCode),
        techSpecRateSchedules: isEmptyReturnDefault(state.rootTimesheetReducer.techSpecRateSchedules, 'object'),
        selectedCompanyCode: state.appLayoutReducer.selectedCompany,
        userTypes: localStorage.getItem(applicationConstants.Authentication.USER_TYPE),
        timesheetCalendarData: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetCalendarData, 'array'),
        isCoordinatorCompany:isCoordinatorCompany(timesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany),
        timesheetTechnicalSpecialists: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialists),
        timesheetTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes),
        timesheetTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels),
        timesheetTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses),
        timesheetTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables),
        companyList: state.appLayoutReducer.companyList,
        timesheetExchangeRates: state.rootTimesheetReducer.timesheetExchangeRates,
        unusedReason: isEmptyReturnDefault(state.rootVisitReducer.unusedReason)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UpdateTimesheetDetails,
                SelectedTimesheetTechSpecs,
                AddTimesheetTechnicalSpecialist,
                RemoveTimesheetTechnicalSpecialist,
                FetchTimesheetGeneralDetail,
                AddTimesheetTechnicalSpecialistTime,
                AddTimesheetTechnicalSpecialistTravel,
                AddTimesheetTechnicalSpecialistExpense,
                AddTimesheetTechnicalSpecialistConsumable,
                FetchCalendarData,
                AddTimesheetCalendarData,
                UpdateTimesheetCalendarData,
                FetchVisitTimesheetCalendarData,
                FetchTimesheetCalendarDetail,
                FetchPreAssignment,
                FetchTimeOffRequestData,
                FetchVisitByID,
                DisplayModal,
                HideModal,
                RemoveTSTimesheetCalendarData,
                UpdateTimesheetTechnicalSpecialist,
                DeleteTimesheetTechnicalSpecialistTime,
                DeleteTimesheetTechnicalSpecialistTravel,
                DeleteTimesheetTechnicalSpecialistExpense,
                DeleteTimesheetTechnicalSpecialistConsumable,
                FetchCurrencyExchangeRate,
                UpdateTimesheetExchangeRates,
                FetchUnusedReason
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(GeneralDetails));