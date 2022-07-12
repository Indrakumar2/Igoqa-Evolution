import GeneralDetails from './generalDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import {
    FetchVisitByID,
    FetchVisitStatus,
    AddUpdateGeneralDetails,
    FetchSupplierList,
    FetchTechnicalSpecialistList,
    SelectedVisitTechSpecs,
    AddVisitTechnicalSpecialist,
    RemoveVisitTechnicalSpecialist,
    FetchAssignmentForVisitCreation,
    FetchVisitTechnicalSpecialists,
    FetchTechnicalSpecialistTime,
    FetchTechnicalSpecialistTravel,
    FetchTechnicalSpecialistExpense,
    FetchTechnicalSpecialistConsumable,
    FetchTechSpecRateSchedulesDefault,
    UpdateVisitTechnicalSpecialist,
    FetchTechSpecRateDefault,
    isTBAVisitStatus,
    addVisitCalendarData,
    updateVisitCalendarData,
    FetchVisitCalendarByID,
    FetchFinalVisitId,
    RemoveCalendarForTechSpec
} from '../../../../actions/visit/generalDetailsAction';
import {
    UpdateVisitExchangeRates,
    FetchUnusedReason
} from '../../../../actions/visit/visitAction';
import {
    AddTechnicalSpecialistTime,    
    AddTechnicalSpecialistTravel,    
    AddTechnicalSpecialistExpense,    
    AddTechnicalSpecialistConsumable,
    DeleteTechnicalSpecialistTime,
    DeleteTechnicalSpecialistTravel,
    DeleteTechnicalSpecialistExpense,
    DeleteTechnicalSpecialistConsumable
} from '../../../../actions/visit/technicalSpcialistAction';
import {
    isOperator,
    isCoordinator,
    getVisitStatus,
    lineitemsExists,
    isOperatorApporved,
    isCoordinatorCompany,
    isOperatorCompany
} from '../../../../selectors/visitSelector';

import {
    FetchCalendarData,
    FetchVisitTimesheetCalendarData,
    FetchTimesheetGeneralDetail,
    FetchPreAssignment,
    FetchTimeOffRequestData
} from '../../../../grm/actions/techSpec/globalCalendarAction';
import { applicationConstants } from '../../../../constants/appConstants';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';
import { 
    FetchCurrencyExchangeRate
} from '../../../../actions/timesheet/timesheetTechSpecsAccountsAction';

const mapStateToProps = (state, ownProps) => {
    const visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object');
    const currentPage = ownProps.currentPage;
    const lineItems = { VisitTechnicalSpecialistTimes:state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes, 
            VisitTechnicalSpecialistExpenses: state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses,
            VisitTechnicalSpecialistTravels: state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels,
            VisitTechnicalSpecialistConsumables: state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables };
    const isCHCompany = isCoordinatorCompany(visitInfo.visitContractCompanyCode, state.appLayoutReducer.selectedCompany);//fixes for D825
    const isOCCompany = isOperatorCompany(visitInfo.visitOperatingCompanyCode, state.appLayoutReducer.selectedCompany);
    const iInterCompanyAssignment = isInterCompanyAssignment(visitInfo.visitContractCompanyCode, visitInfo.visitOperatingCompanyCode);//fixes for D825
    return {
        visitInfo: visitInfo, //isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo,'object'),
        visitValidationData: isEmptyReturnDefault(state.rootVisitReducer.visitValidationData),        
        visitStatus: isEmptyReturnDefault(state.rootVisitReducer.visitStatus),
        unusedReason: isEmptyReturnDefault(state.rootVisitReducer.unusedReason),
        //visitStatus: getVisitStatus({ ...state.rootVisitReducer.visitDetails.VisitInfo, currentPage }),
        supplierList: isEmptyReturnDefault(state.rootVisitReducer.supplierList),
        subSupplierList: isEmptyReturnDefault(state.rootVisitReducer.subSupplierList),
        visitSupplierList: isEmptyReturnDefault(state.rootVisitReducer.visitSupplierList),
        technicalSpecialistList: isEmptyReturnDefault(state.rootVisitReducer.technicalSpecialistList),
        visitTechnicalSpecialists: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialists),
        currentPage: state.CommonReducer.currentPage,
        isOperator: isOperator(visitInfo.VisitOperatingCompanyCode, state.appLayoutReducer.username),
        isCoordinator: isCoordinator(visitInfo.VisitContractCompanyCode, state.appLayoutReducer.username),
        defaultTechSpecRateSchedules: isEmptyReturnDefault(state.rootVisitReducer.defaultTechSpecRateSchedules),
        isTBAVisitStatus: isEmptyReturnDefault(state.rootVisitReducer.isTBAVisitStatus),
        selectedCompanyCode: state.appLayoutReducer.selectedCompany,
        userLogonName: state.appLayoutReducer.username,
        userTypes: localStorage.getItem(applicationConstants.Authentication.USER_TYPE),
        visitId: state.rootVisitReducer.selectedVisitData ? state.rootVisitReducer.selectedVisitData.visitId : "",
        visitCalendarData: isEmptyReturnDefault(state.rootVisitReducer.visitCalendarData, 'array'),        
        hasLineItems: lineitemsExists(lineItems),        
        isOperatorApporved: isOperatorApporved(visitInfo),
        visitTechnicalSpecialistTimes: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTimes),
        visitTechnicalSpecialistTravels: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistTravels),
        visitTechnicalSpecialistExpenses: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistExpenses),
        visitTechnicalSpecialistConsumables: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitTechnicalSpecialistConsumables),
        isInterCompanyAssignment: iInterCompanyAssignment,//fixes for D825
        isCoordinatorCompany: isCHCompany,//fixes for D825
        isOperatingCompany: isOCCompany,
        companyList: state.appLayoutReducer.companyList,
        visitExchangeRates: state.rootVisitReducer.visitExchangeRates,
        isOCApprovedByOC: (isOCCompany && visitInfo.visitStatus === "O" ? true : false),
        isCHApprovedByOC: (isCHCompany && visitInfo.visitStatus === "O" ? true : false),
        isOCApprovedByCH: (iInterCompanyAssignment ? (isOCCompany && visitInfo.visitStatus === "A" ? true : false) : false),
        isCHApprovedByCH: (iInterCompanyAssignment ? (isCHCompany && visitInfo.visitStatus === "A" ? true : false) : false),
        isDisableField: ((isOCCompany || isCHCompany) && [ 'O','A' ].includes(visitInfo.visitStatus) ? true : false)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchVisitByID,
                FetchVisitStatus,
                AddUpdateGeneralDetails,
                FetchSupplierList,
                FetchTechnicalSpecialistList,
                SelectedVisitTechSpecs,
                AddVisitTechnicalSpecialist,
                RemoveVisitTechnicalSpecialist,
                FetchAssignmentForVisitCreation,
                FetchVisitTechnicalSpecialists,
                FetchTechnicalSpecialistTime,
                FetchTechnicalSpecialistTravel,
                FetchTechnicalSpecialistExpense,
                FetchTechnicalSpecialistConsumable,
                FetchTechSpecRateSchedulesDefault,
                UpdateVisitTechnicalSpecialist,
                FetchTechSpecRateDefault,
                AddTechnicalSpecialistTime,
                AddTechnicalSpecialistTravel,
                AddTechnicalSpecialistExpense,
                AddTechnicalSpecialistConsumable,
                isTBAVisitStatus,
                FetchCalendarData,
                addVisitCalendarData,
                updateVisitCalendarData,
                FetchVisitTimesheetCalendarData,
                FetchTimesheetGeneralDetail,
                FetchPreAssignment,
                FetchTimeOffRequestData,
                FetchVisitCalendarByID,
                FetchFinalVisitId,
                DisplayModal,
                HideModal,
                RemoveCalendarForTechSpec,
                DeleteTechnicalSpecialistTime,
                DeleteTechnicalSpecialistTravel,
                DeleteTechnicalSpecialistExpense,
                DeleteTechnicalSpecialistConsumable,
                FetchCurrencyExchangeRate,
                UpdateVisitExchangeRates,
                FetchUnusedReason
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(GeneralDetails));