import VisitDetails from './visitDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { 
    SaveVisitDetails,
    FetchAssignmentForVisitCreation,
    CancelCreateVisitDetails,
    CancelEditVisitDetails,
    DeleteVisit,
    UpdateVisitStatus,
    FetchReferencetypes,
    FetchVisitStatus,
    FetchTechnicalSpecialistList,
    FetchSupplierList,
    FetchVisitValidationData,
    FetchSubsuppliers,
    FetchTechSpecRateDefault,
    FetchVisitDetail,
    CreateNewVisit,
    ProcessApprovalEmailNotification,
    SendCustomerReportingNotification,
    ClearCalendarData,
    GetSelectedVisit,
    CancelEditVisitUploadDocument,
    ClearVisitDetails,
    SaveValidCalendarDataForSave
} from '../../../../actions/visit/visitAction';
import {
    FetchProjectDetail
} from '../../../../actions/project/projectAction';
import { updateVisitCalendarData,isTBAVisitStatus } from '../../../../actions/visit/generalDetailsAction';
import {
    FetchCalendarData,
    FetchVisitTimesheetCalendarData,
    FetchTimesheetGeneralDetail,
    FetchPreAssignment,
    FetchTimeOffRequestData
} from '../../../../grm/actions/techSpec/globalCalendarAction';
import { FetchAssignmentsDetailInfo,SaveSelectedAssignmentId } from '../../../../actions/assignment/assignmentAction';
import { SetCurrentPageMode, UpdateInteractionMode } from '.././../../../common/commonAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { 
    isInterCompanyAssignment
} from '../../../../selectors/assignmentSelector';
import { 
    isOperator,
    isCoordinator,
    isOperatorCompany,
    isCoordinatorCompany,
    getVisitStatus,
    isCompanyOperatorCoordinator
} from '../../../../selectors/visitSelector';
import { DeleteAlert } from '../../customer/alertAction';
import {
    AddUpdateGeneralDetails
} from '../../../../actions/visit/generalDetailsAction';
import { activitycode } from '../../../../constants/securityConstant';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {
    const visitInfo= isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo,'object');
    const VisitInterCompanyDiscounts= isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInterCompanyDiscounts,'object');
    const currentPage = state.CommonReducer.currentPage;
    return {
        visitDetails:[],
        currentPage: currentPage,
        interactionMode: state.CommonReducer.interactionMode,
        // interactionMode: (isCompanyOperatorCoordinator(visitInfo.visitOperatingCompanyCode,visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany)
        //         ? state.CommonReducer.interactionMode : true),
        notLoggedinCompany: !isCompanyOperatorCoordinator(visitInfo.visitOperatingCompanyCode,visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany),
        visitInfo: visitInfo,
        VisitInterCompanyDiscounts:VisitInterCompanyDiscounts,
        visitValidationData: isEmptyReturnDefault(state.rootVisitReducer.visitValidationData),        
        //visitStatus:getVisitStatus({ ...state.rootVisitReducer.visitDetails.VisitInfo, currentPage }), 
        visitStatus: isEmptyOrUndefine(state.rootVisitReducer.selectedVisitStatus) ? visitInfo.visitStatus
                        : state.rootVisitReducer.selectedVisitStatus,
        //visitStatus: state.rootVisitReducer.selectedVisitData.visitStatus ? state.rootVisitReducer.selectedVisitData.visitStatus : '',      
        isbtnDisable:state.rootVisitReducer.isbtnDisable,
        visitTechnicalSpecialists: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists),
        visitSelectedTechSpecs: isEmptyReturnDefault(state.rootVisitReducer.visitSelectedTechSpecs),
        isInterCompanyAssignment: isInterCompanyAssignment(visitInfo.visitContractCompanyCode,
            visitInfo.visitOperatingCompanyCode),
        visitSupplierPerformances: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitSupplierPerformances),
        isOperator:isOperator(visitInfo.visitOperatingCompanyCoordinatorCode,state.appLayoutReducer.username),
        isOperatorCompany:isOperatorCompany(visitInfo.visitOperatingCompanyCode,state.appLayoutReducer.selectedCompany),
        isCoordinator:isCoordinator(visitInfo.visitContractCoordinatorCode,state.appLayoutReducer.username),
        isCoordinatorCompany:isCoordinatorCompany(visitInfo.visitContractCompanyCode,state.appLayoutReducer.selectedCompany),
        timeLineItems: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes),
        travelLineItems: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels),
        expenseLineItems: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses),
        consumableLineItems: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables),
        VisitDocuments: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitDocuments),
        subSupplierList: isEmptyReturnDefault(state.rootVisitReducer.subSupplierList),
        customerReportingNotificationContant:isEmptyReturnDefault(state.rootTimesheetReducer.customerReportingNotificationContant),
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        emailTemplate:state.CommonReducer.emailTemplate,
        visitCalendarData: isEmptyReturnDefault(state.rootVisitReducer.visitCalendarData, 'array'),
        selectedVisitData:isEmptyReturnDefault(state.rootVisitReducer.selectedVisitData),
        loggedInUser: state.appLayoutReducer.loginUser,
        loggedInUserName: state.appLayoutReducer.userName,
        modifyPayUnitPayRate: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VISIT_MODIFY_PAYUNIT_PAYRATE).length > 0),
        attachedDocs: isEmptyReturnDefault(state.rootVisitReducer.attachedFiles),
        isTBAVisitStatus: isEmptyReturnDefault(state.rootVisitReducer.isTBAVisitStatus),
        selectedCompanyCode: state.appLayoutReducer.selectedCompany,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                ShowLoader,
                HideLoader,
                SaveVisitDetails,
                FetchAssignmentForVisitCreation,
                CancelCreateVisitDetails,
                CancelEditVisitDetails,
                DeleteVisit,
                DisplayModal,
                HideModal,
                UpdateVisitStatus,
                FetchReferencetypes,
                FetchVisitStatus,                
                FetchTechnicalSpecialistList,
                FetchSupplierList,
                FetchVisitValidationData,
                FetchSubsuppliers,
                FetchTechSpecRateDefault,
                FetchVisitDetail,
                CreateNewVisit,
                DeleteAlert,
                ProcessApprovalEmailNotification,
                SendCustomerReportingNotification,
                ClearCalendarData,
                GetSelectedVisit,
                SetCurrentPageMode,                
                FetchAssignmentsDetailInfo,
                SaveSelectedAssignmentId,
                AddUpdateGeneralDetails,
                CancelEditVisitUploadDocument,
                ClearVisitDetails,
                SaveValidCalendarDataForSave,
                updateVisitCalendarData,
                isTBAVisitStatus,
                FetchVisitTimesheetCalendarData,
                UpdateInteractionMode,
                FetchProjectDetail
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(VisitDetails));