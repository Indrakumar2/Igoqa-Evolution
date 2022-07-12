import AssignedSpecialist from './assignedSpecialists';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault , getlocalizeData, isEmpty, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { FetchTechSpecCategory, 
        FetchTechSpecSubCategory,
        FetchTechSpecServices,
        ClearSubCategory,
        ClearServices } from '../../../../common/masterData/masterDataActions';
import { AssignTechnicalSpecialist,
        UpdateAssignedTechSpec,
        FetchPaySchedule,
        AddTechSpecSchedules,
        UpdateTechSpecSchedules,
        DeleteTechSpecSchedules,
        DeleteAssignedTechSpec,
        UpdateTaxomony,
        FetchChargeRates,
        FetchPayRates,
        ClearChargeAndPayRates,
        ARSSearchPanelStatus,
        AssignResourcesButtonClick,
        FetchTaxonomyBusinessUnit,
        DeleteNDTAssignedTechSpec,
        FetchTaxonomyURL } from '../../../../actions/assignment/assignedSpecialistsAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { TechSpecClearSearch } from '../../../../grm/actions/techSpec/techSpecSearchActions';
import { LoadARSSearchData,FetchPreAssignmentIds,GetARSSearchData,GetAssigedResourcesForARS,SetTechSpecToAssignmentSubSupplier } from '../../../../actions/assignment/arsSearchAction';
import { FetchContractScheduleName } from '../../../../actions/assignment/contractRateScheduleActions';
import { SaveAssignmentDetails } from '../../../../actions/assignment/assignmentAction';
import { FetchAssignmentVisits } from '../../../../actions/visit/visitAction';//D546
import { FetchAssignmentTimesheets } from '../../../../actions/timesheet/timesheetAction';//D546
import { 
    isContractHolderCompany, 
    isOperator, 
    isCoordinator, 
    isInterCompanyAssignment,
    isOperatorCompany,
    isTimesheet,
    isVisit,
    isARS
} from '../../../../selectors/assignmentSelector';
import { activitycode } from '../../../../constants/securityConstant';

const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const contractHoldingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators),
        operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    const businessUnit = isEmptyReturnDefault(state.masterDataReducer.businessUnit);
    return {
        techSpecCategory: state.masterDataReducer.techSpecCategory,
        techSpecSubCategory: state.masterDataReducer.techSpecSubCategory,
        techSpecServices: state.masterDataReducer.techSpecServices,
        taxonomyCategory: isEmptyReturnDefault(state.rootAssignmentReducer.taxonomyBusinessUnit),
        ndtSubCategory : isEmptyReturnDefault(state.rootAssignmentReducer.ndtSubCategory),
        ndtServices : isEmptyReturnDefault(state.rootAssignmentReducer.ndtServices),
        businessUnit: businessUnit,
        assignmentInfo: assignmentInfo,
        contractSchedules: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentContractSchedules),
        rateScheduleNames:state.rootAssignmentReducer.RateScheduleNames,
        chargeRates: state.rootAssignmentReducer.chargeRates,
        payRates: state.rootAssignmentReducer.payRates,
        paySchedules: isEmptyReturnDefault(state.rootAssignmentReducer.paySchedules),
        assignedTechSpec: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists),
        subSupplierTechSpec: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers),
        taxonomy: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTaxonomy),
        selectedCompany : state.appLayoutReducer.selectedCompany,
        loginUser:state.appLayoutReducer.loginUser,
        isARSSearch:state.rootAssignmentReducer.isARSSearch,
        arsSearchData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails,'object'),
        isContractHolderCompany: isContractHolderCompany(assignmentInfo.assignmentContractHoldingCompanyCode,
            state.appLayoutReducer.selectedCompany),
        isOperator: isOperator(assignmentInfo.assignmentOperatingCompanyCoordinator,
            operatingCompanyCoordinators,
            state.appLayoutReducer.username),
        isCoordinator: isCoordinator(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
            contractHoldingCompanyCoordinators,
            state.appLayoutReducer.username),
        isInterCompanyAssignment: isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,
            assignmentInfo.assignmentOperatingCompanyCode),
        currentPage : state.CommonReducer.currentPage,
        pageMode:state.CommonReducer.currentPageMode,
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
            state.appLayoutReducer.selectedCompany),
        visitData:state.rootVisitReducer.visitList,//D546
        timesheetData:state.rootTimesheetReducer.timesheetList,//D546
        isTimesheetAssignment:isTimesheet(workflowType),
        isVisitAssignment:isVisit(workflowType),
        isArsAssignment: isARS({ projectReportParams: businessUnit,businessUnit: assignmentInfo.assignmentProjectBusinessUnit }),
        selectedHomeCompany:state.appLayoutReducer.selectedCompany,
        opCompanyCode: !isEmpty(state.rootAssignmentReducer.assignmentDetail) ? isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentOperatingCompanyCode) : (!isEmpty(state.rootAssignmentReducer.arsOPCoordinatorInfo)? state.rootAssignmentReducer.arsOPCoordinatorInfo.companyCode :'' ),
        taxonomyConstantURL:isEmptyReturnDefault(state.rootAssignmentReducer.taxonomyConstantURL, 'object'), 
        viewPayRate: (isEmptyOrUndefine(state.appLayoutReducer.activities) ? false 
                                : state.appLayoutReducer.activities.filter(x=>x.activity === activitycode.VIEW_PAYRATE).length > 0),
        techSpecList:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList),

    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(
            {
                FetchTechSpecCategory,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                ClearSubCategory,
                ClearServices,
                AssignTechnicalSpecialist,
                UpdateAssignedTechSpec,
                FetchPaySchedule,
                AddTechSpecSchedules,
                UpdateTechSpecSchedules,
                DeleteTechSpecSchedules,
                DeleteAssignedTechSpec,
                DisplayModal,
                HideModal,
                TechSpecClearSearch,
                UpdateTaxomony,
                FetchChargeRates,
                FetchPayRates,
                ClearChargeAndPayRates,
                ARSSearchPanelStatus,
                LoadARSSearchData,
                FetchPreAssignmentIds,
                GetARSSearchData,
                SaveAssignmentDetails,
                AssignResourcesButtonClick, 
                FetchContractScheduleName,
                GetAssigedResourcesForARS,
                FetchTaxonomyBusinessUnit,
                SetTechSpecToAssignmentSubSupplier,
                FetchAssignmentTimesheets,//D546
                FetchAssignmentVisits,//D546
                DeleteNDTAssignedTechSpec,
                FetchTaxonomyURL, 
            },
            dispatch
        )
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AssignedSpecialist);