import TimesheetDetails from './timesheetDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { 
    FetchAssignmentForTimesheetCreation,
    SaveTimesheetDetails,
    CancelCreateTimesheetDetails,
    CancelEditTimesheetDetails,
    DeleteTimesheet,
    UpdateTimesheetStatus,
    CreateNewTimesheet,
    ProcessApprovalEmailNotification,
    SendCustomerReportingNotification,
    FetchTimesheetDetail,
    ClearTimesheetCalendarData,
    GetSelectedTimesheet,
    CancelEditTimesheetDetailsDocument,
    ClearTimesheetDetails,
    SaveValidTimesheetCalendarDataForSave,
    FetchTimesheetValidationData
    } from '../../../../actions/timesheet/timesheetAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault, isUndefined, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { 
    isInterCompanyAssignment
} from '../../../../selectors/assignmentSelector';
import { 
    isOperator,
    isCoordinator,
    getTimesheetStatus,
    isTimesheetCreateMode,
    isOperatorCompany,
    isCoordinatorCompany,
    isCompanyOperatorCoordinator
} from '../../../../selectors/timesheetSelector';
import { AddTimesheetNotes } from '../../../../actions/timesheet/timesheetNoteAction';
import { UpdateTimesheetDetails } from '../../../../actions/timesheet/timesheetGeneralDetails';
import { FetchReferencetypes } from '../../../../actions/timesheet/timesheetReferenceAction'; 
import { FetchTimesheetTechnicalSpecialists } from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction'; 
import { DeleteAlert } from '../../customer/alertAction';
import { HandleMenuAction } from '../../../sideMenu/sideMenuAction';
import { SetCurrentPageMode,UpdateInteractionMode } from '.././../../../common/commonAction';
import { FetchAssignmentsDetailInfo,SaveSelectedAssignmentId } from '../../../../actions/assignment/assignmentAction';
import { activitycode } from '../../../../constants/securityConstant';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {
    const timesheetInfo= isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo,'object');
    const currentPage = state.CommonReducer.currentPage;
    return {
        currentPage: currentPage,
        interactionMode: (isCompanyOperatorCoordinator(timesheetInfo.timesheetOperatingCompanyCode,timesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany)
                            ? state.CommonReducer.interactionMode : true),        
        timesheetInfo: timesheetInfo,
        timesheetStatus:getTimesheetStatus({ ...state.rootTimesheetReducer.timesheetDetail.TimesheetInfo, currentPage }),
        selectedTimesheetStatus: isEmptyOrUndefine(state.rootTimesheetReducer.selectedTimesheetStatus) ? '' 
        : state.rootTimesheetReducer.selectedTimesheetStatus,
        isbtnDisable:state.rootTimesheetReducer.isbtnDisable,
        timesheetSelectedTechSpecs: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetSelectedTechSpecs),
        isInterCompanyAssignment: isInterCompanyAssignment(timesheetInfo.timesheetContractCompanyCode,
            timesheetInfo.timesheetOperatingCompanyCode),
        isOperator:isOperator(timesheetInfo.timesheetOperatingCoordinatorCode,state.appLayoutReducer.username),
        isCoordinator:isCoordinator(timesheetInfo.timesheetContractCoordinatorCode,state.appLayoutReducer.username),
        timeLineItems: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTimes),
        travelLineItems: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistTravels),
        expenseLineItems: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistExpenses),
        consumableLineItems: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialistConsumables),
        timesheetTechnicalSpecilists: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetTechnicalSpecialists),
        isTimesheetCreateMode:isTimesheetCreateMode(currentPage),
        timesheetId: state.rootTimesheetReducer.selectedTimesheetId,
        timesheetDocuments:isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments),
        customerReportingNotificationContant:isEmptyReturnDefault(state.rootTimesheetReducer.customerReportingNotificationContant),
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        timesheetValidationData: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetValidationData),
        isOperatorCompany:isOperatorCompany(timesheetInfo.timesheetOperatingCompanyCode,state.appLayoutReducer.selectedCompany),
        isCoordinatorCompany:isCoordinatorCompany(timesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany),
        emailTemplate:state.CommonReducer.emailTemplate,
        timesheetCalendarData: isEmptyReturnDefault(state.rootTimesheetReducer.timesheetCalendarData, 'array'),
        selectedTimesheet:isEmptyReturnDefault(state.rootTimesheetReducer.selectedTimesheet),        
        loggedInUser: state.appLayoutReducer.loginUser,
        loggedInUserName: state.appLayoutReducer.userName,
        modifyPayUnitPayRate: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                            : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.TIMESHEET_MODIFY_PAYUNIT_PAYRATE).length > 0),
        attachedDocs: isEmptyReturnDefault(state.rootVisitReducer.attachedFiles)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                DisplayModal,
                HideModal,
                FetchAssignmentForTimesheetCreation,
                SaveTimesheetDetails,
                CancelCreateTimesheetDetails,
                CancelEditTimesheetDetails,
                DeleteTimesheet,
                UpdateTimesheetDetails,
                UpdateTimesheetStatus,
                FetchReferencetypes,
                CreateNewTimesheet,
                FetchTimesheetTechnicalSpecialists,
                DeleteAlert,
                ProcessApprovalEmailNotification,
                SendCustomerReportingNotification,
                FetchTimesheetDetail,
                ClearTimesheetCalendarData,
                GetSelectedTimesheet,
                HandleMenuAction,
                SetCurrentPageMode,
                FetchAssignmentsDetailInfo,
                SaveSelectedAssignmentId,
                CancelEditTimesheetDetailsDocument,
                ClearTimesheetDetails,
                SaveValidTimesheetCalendarDataForSave,    
                AddTimesheetNotes,
                ShowLoader,
                HideLoader,
                FetchTimesheetValidationData,
                UpdateInteractionMode
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(TimesheetDetails));