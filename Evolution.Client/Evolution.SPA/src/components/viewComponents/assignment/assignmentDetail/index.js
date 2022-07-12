import AssignmentDetail from './assignmentDetail';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { FetchContractData } from '../../../../actions/contracts/contractAction';
import { withRouter } from 'react-router-dom';
import { FetchProjectForAssignmentCreation,
     SaveAssignmentDetails,
     CancelCreateAssignmentDetails,
     CancelEditAssignmentDetails,
     DeleteAssignment,
     copyAssignment,
     SaveSelectedAssignmentId,
     FetchAssignmentDetail,
     cancelCopyAssignment,
     FetchGeneralDetailsMasterData,
     OnAssignmentUnmount,
     OnCancelUploadDocument } from '../../../../actions/assignment/assignmentAction'; // D - 631
     import { ARSSearchPanelStatus,  
            AssignTechnicalSpecialist,
            handleAssignedSpecialistTaxonomyLOV } from '../../../../actions/assignment/assignedSpecialistsAction'; //Added AssignTechnicalSpecialist for D634 issue 2
     
import { isEmptyReturnDefault,isEmpty, getlocalizeData } from '../../../../utils/commonUtils';
import { DeleteAlert } from '../../customer/alertAction';
import {
    SaveSelectedProjectNumber
} from '../../../../actions/project/projectAction';
import { 
    isOperatorCompany,
    isContractHolderCompany,
    isInterCompanyAssignment,
    isCoordinator,
    isTimesheet,
    isVisit,
    isSettlingTypeMargin } from '../../../../selectors/assignmentSelector';
import { HandleMenuAction } from '../../../sideMenu/sideMenuAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';
import { SetCurrentPageMode,UpdateCurrentPage,UpdateCurrentModule } from '../../../../common/commonAction';
    import { ClearData,ClearReportsData, UpdateReportCustomer, ClearCustomerData } from '../../../applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
    import {  
        updateSupplierDetails,  
    } from '../../../../actions/supplierPO/supplierPOAction';
    import {
        FetchSupplierSearchList,
        ClearSupplierSearchList,
    
    } from '../../../../actions/supplierPO/supplierPOSearchAction';

const localConstant = getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const selectedCompany= state.appLayoutReducer.selectedCompany;
    const contractHoldingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators);
    const businessUnit =  { 
        projectReportParams: isEmptyReturnDefault(state.masterDataReducer.businessUnit),
        businessUnit: assignmentInfo.assignmentProjectBusinessUnit 
    };
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };
    return {
        isbtnDisable: state.rootAssignmentReducer.isbtnDisable,
        interactionMode: state.CommonReducer.interactionMode,
        currentPage: state.CommonReducer.currentPage,
        assignmentOldTaxonomy: isEmptyReturnDefault(state.rootAssignmentReducer.AssignmentTaxonomyold),
        assignmentOldLifeCycle: state.rootAssignmentReducer.AssignmentLifeCycleOld,
        assignmentOldSupplierPO: state.rootAssignmentReducer.AssignmentSupplierPOOld,
        assignmentDetails: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail,"object"),
        taxonomy: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTaxonomy),
        dataToValidateAssignment: isEmptyReturnDefault(state.rootAssignmentReducer.dataToValidateAssignment,"object"),
        selectedCompany: selectedCompany,
        isSupplierPOChanged: state.rootAssignmentReducer.isSupplierPOChanged,
        assignmentSubSuppliers: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers),
        isARSSearch:state.rootAssignmentReducer.isARSSearch,
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
            selectedCompany),
        isContractHolderCompany:isContractHolderCompany(assignmentInfo.assignmentContractHoldingCompanyCode,
            selectedCompany),
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        isCoordinator: isCoordinator(assignmentInfo.assignmentContractHoldingCompanyCoordinator,
            contractHoldingCompanyCoordinators,
            state.appLayoutReducer.username),
        isTimesheetAssignment:isTimesheet(workflowType),
        isSettlingTypeMargin: isSettlingTypeMargin(businessUnit),
        isVisitAssignment:isVisit(workflowType),
        assignmentContributionCalculator: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentContributionCalculators),
        documentrowData:isEmpty(state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments)?[]:state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments,
        projectNumber:state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo,
        //added for supplierperformancereport
        companyList: state.appLayoutReducer.companyList, //Changes for IGO - D901
        supplierList: state.rootSupplierPOReducer.supplierList,    
        isViewAllAssignment : isEmptyReturnDefault(state.masterDataReducer.viewAllRightsCompanies),
        reportsCustomerName:state.CustomerAndCountrySearch.reportsCustomerName, 
        defaultReportCustomerName:state.CustomerAndCountrySearch.defaultReportCustomerName,
        techSpecList: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList),
        selectedTechSpec:   isEmptyReturnDefault(state.rootAssignmentReducer.selectedTechSpec),

    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ShowLoader,
                HideLoader,
                DisplayModal,
                HideModal,
                FetchProjectForAssignmentCreation,
                SaveAssignmentDetails,
                CancelCreateAssignmentDetails,
                CancelEditAssignmentDetails,
                DeleteAssignment,
                copyAssignment,
                DeleteAlert,
                ARSSearchPanelStatus,
                SaveSelectedAssignmentId,
                FetchAssignmentDetail,
                HandleMenuAction,
                SetCurrentPageMode,
                UpdateCurrentPage,
                FetchContractData,
                UpdateCurrentModule,
                SaveSelectedProjectNumber,
                cancelCopyAssignment, // D - 631
                AssignTechnicalSpecialist, //Added for D634 issue 2
                FetchGeneralDetailsMasterData,
                OnAssignmentUnmount,
                //added for SupplierVisitPerformance
                ClearData,
                ClearCustomerData,
                ClearSupplierSearchList,
                updateSupplierDetails,
                FetchSupplierSearchList,
                OnCancelUploadDocument,
                handleAssignedSpecialistTaxonomyLOV,
                ClearReportsData,
                UpdateReportCustomer
            },
            dispatch
        )
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(AssignmentDetail));