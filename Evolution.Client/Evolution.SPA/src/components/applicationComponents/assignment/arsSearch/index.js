import ArsSearch from './arsSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, isEmpty, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { FetchAssignmentType,
        UpdateActionDetails,
        FetchDispositionType,
        clearPreAssignmentDetails,
        AddOptionalSearch,
        searchPreAssignmentTechSpec,
        UpdatePreAssignmentSearchParam,
        ClearOptionalSearch,
        GetGeoLocationInfo } from '../../../../grm/actions/techSpec/preAssignmentAction';
import { FetchTechSpecCategory,
    grmFetchCustomerData ,
    arsMasterData } from '../../../../common/masterData/masterDataActions';
import { AssignTechnicalSpecialistFromARS,
    clearARSSearchDetails,
    FetchOperationalManagers,
    FetchTechnicalSpecialists,
    SaveARSSearch,
    UpdateARSSearch,
    SetDefaultPreAssignmentName,
    FetchPreAssignmentIds,
    ClearPLOServices,
    ClearPLOSubCategory,
    FetchPLOTechSpecServices,
    FetchPLOTechSpecSubCategory,
    OverridenResources,
    SetTechSpecToAssignmentSubSupplier,
    GetSelectedPreAssignment,
    FetchARSCommentsHistoryReport,
    } from '../../../../actions/assignment/arsSearchAction';
import { AssignmentSaveButtonDisable,SelectedTechSpec } from '../../../../actions/assignment/assignmentAction';
import { ARSSearchPanelStatus,AssignResourcesButtonClick } from '../../../../actions/assignment/assignedSpecialistsAction';
import { getlocalizeData } from '../../../../utils/commonUtils';
import { FetchTechSpecMytaskData,ClearMyTasksData } from '../../../../grm/actions/techSpec/techSpecMytaskActions';
import { isVisit,isTimesheet } from '../../../../selectors/assignmentSelector'; 
import { ExportToCV,ExportToMultiCV } from '../../../../grm/actions/techSpec/techSpecDetailAction';
import { deleteSubSupplierTS } from '../../../../actions/assignment/supplierInformationAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';

const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    return {
        currentPage: state.CommonReducer.currentPage,
        isARSMasterDataLoaded: state.CommonReducer.isARSMasterDataFeteched,
        customerList: state.masterDataReducer.customerList,
        companyList: state.appLayoutReducer.companyList,
        languages: state.masterDataReducer.languages,
        certificates: state.masterDataReducer.certificates,
        arsCHCoordinatorInfo: isEmptyReturnDefault(state.rootAssignmentReducer.arsCHCoordinatorInfo,'object'),
        arsOPCoordinatorInfo: isEmptyReturnDefault(state.rootAssignmentReducer.arsOPCoordinatorInfo,'object'),
        contractHoldingCoodinatorList: state.RootTechSpecReducer.TechSpecDetailReducer.chCoordinators,
        operatingCoordinatorList: state.RootTechSpecReducer.TechSpecDetailReducer.ocCoordinators,
        assignmentTypes: state.RootTechSpecReducer.TechSpecDetailReducer.assignmentType,
        assignmentStatus: state.rootAssignmentReducer.assignmentStatus,
        taxonomyCategory: state.masterDataReducer.techSpecCategory,
        taxonomySubCategory: state.masterDataReducer.techSpecSubCategory,
        taxonomyServices: state.masterDataReducer.techSpecServices,
        ploTaxonomySubCategory: state.rootAssignmentReducer.ploTaxonomySubCategory,
        ploTaxonomyService: state.rootAssignmentReducer.ploTaxonomyService,
        preAssignmentIds: state.rootAssignmentReducer.preAssignmentIds,
        arsCommentHistory: isEmptyReturnDefault(state.rootAssignmentReducer.arsCommentHistory),
        techSpecList:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList),
        isShowGoogleMap:state.RootTechSpecReducer.TechSpecDetailReducer.isShowGoogleMap,
        assignmentData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails,'object'),
        assignedTechSpec: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists),
        dispositionType:state.RootTechSpecReducer.TechSpecDetailReducer.dispositionType,
        isResourceMatched : state.rootAssignmentReducer.isResourceMatched,
        unMatchedResources : isEmptyReturnDefault(state.rootAssignmentReducer.unMatchedResources),
        selectedPreAssignmentSearchParam : isEmptyReturnDefault(state.rootAssignmentReducer.selectedPreAssignment.searchParameter,'object'),
        operationalManagers: isEmptyReturnDefault(state.rootAssignmentReducer.operationalManagers),
        technicalSpecialists: isEmptyReturnDefault(state.rootAssignmentReducer.technicalSpecialists),
        selectedMyTask : isEmptyReturnDefault(state.rootAssignmentReducer.selectedARSMyTask,'object'),
        overrideTaskStatus : isEmptyReturnDefault(localConstant.resourceSearch.overrideTaskStatus),
        ploTaskStatus : isEmptyReturnDefault(localConstant.resourceSearch.ploTaskStatus),
        interactionMode:true,
        defaultPreAssignmentID:state.rootAssignmentReducer.defaultPreAssignmentID,
        overridenResources: isEmptyReturnDefault(state.rootAssignmentReducer.overridenResources),
        equipment: state.masterDataReducer.equipment,
        taxonomyCustomerApproved: state.masterDataReducer.taxonomyCustomerApproved,
        assignmentSubsuppliers: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers),
        isVisitAssignment:isVisit(workflowType),
        isTimesheetAssignment:isTimesheet(workflowType),
        assignmentDetail:assignmentInfo,
        isGrmMasterDataFeteched:state.CommonReducer.isGrmMasterDataFeteched,
        selectedHomeCompany:state.appLayoutReducer.selectedCompany,
        opCompanyCode: !isEmptyOrUndefine(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo) ? isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentOperatingCompanyCode) : (!isEmpty(state.rootAssignmentReducer.arsOPCoordinatorInfo)? state.rootAssignmentReducer.arsOPCoordinatorInfo.companyCode :'' ),
        visitData:!isEmpty(state.rootVisitReducer.visitList) ? state.rootVisitReducer.visitList : '',
        timesheetData: !isEmpty(state.rootTimesheetReducer.timesheetList) ? state.rootTimesheetReducer.timesheetList : ''
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentType,
                FetchTechSpecCategory,
                grmFetchCustomerData,
                UpdateActionDetails,
                FetchDispositionType,
                AssignTechnicalSpecialistFromARS,
                ARSSearchPanelStatus,
                clearPreAssignmentDetails,
                GetSelectedPreAssignment,
                clearARSSearchDetails,
                searchPreAssignmentTechSpec,
                AddOptionalSearch,
                UpdatePreAssignmentSearchParam,
                ClearOptionalSearch,
                AssignResourcesButtonClick,
                FetchOperationalManagers,
                FetchTechnicalSpecialists,
                SaveARSSearch,
                UpdateARSSearch,
                FetchTechSpecMytaskData,
                SetDefaultPreAssignmentName,
                FetchPreAssignmentIds,
                ClearPLOServices,
                ClearPLOSubCategory,
                FetchPLOTechSpecServices,
                FetchPLOTechSpecSubCategory,
                ClearMyTasksData,
                OverridenResources,
                GetGeoLocationInfo,
                SetTechSpecToAssignmentSubSupplier,
                ExportToCV,
                arsMasterData, // ARS Performance Fine Tuning
                deleteSubSupplierTS,
                FetchARSCommentsHistoryReport,
                AssignmentSaveButtonDisable,
                DisplayModal,
                HideModal,
                ExportToMultiCV,
                SelectedTechSpec
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ArsSearch));