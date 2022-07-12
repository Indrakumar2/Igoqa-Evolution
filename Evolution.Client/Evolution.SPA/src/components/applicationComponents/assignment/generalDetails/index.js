import GeneralDetails from './generalDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchState,
    FetchCity,
    ClearStateCityData,
    ClearCityData,
} from '../../../../common/masterData/masterDataActions';
import { UpdateAssignmentGeneralDetails, 
    FetchCoordinatorForOperatingCompany,
    FetchTimesheetState,
    FetchTimesheetCity } from '../../../../actions/assignment/generalDetailsActions';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ClearHostDiscounts,ClearContractHoldingDiscounts } from '../../../../actions/assignment/assignmentICDiscountsAction';
import { withRouter } from 'react-router-dom';
import { FetchSupplierContacts,FetchSubsuppliers,AddSubSupplierInformation,FetchMainSupplierName,deletSupplierInformations, FetchSubsupplierContacts, UpdateSubSuppliers,
    addMainSupplierData } from '../../../../actions/assignment/supplierInformationAction';
import { isEmptyReturnDefault,getlocalizeData } from '../../../../utils/commonUtils';
import { 
    isOperator,
    isInterCompanyAssignment,
    isOperatorCompany,
    GetProjectReportingParams,
    isTimesheet,
    isVisit,
    isSettlingTypeMargin,
    getCoordinatorData } from '../../../../selectors/assignmentSelector';
import { FetchAssignmentVisits } from '../../../../actions/visit/visitAction';
import { ClearICDependentData,IsInternalAssignment } from '../../../../actions/assignment/assignmentAction';

const localConstant=getlocalizeData();

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const operatingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators);
    const contractHoldingCompanyCoordinators = isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators);
    const customerAssignmentContact = isEmptyReturnDefault(state.rootAssignmentReducer.customerAssignmentContact);
    
    const workflowType = {
        workflowTypeParams: localConstant.commonConstants.workFlow,
        workflowType: assignmentInfo.assignmentProjectWorkFlow && assignmentInfo.assignmentProjectWorkFlow.trim()
    };

    const businessUnits = {
        projectReportParams: isEmptyReturnDefault(state.masterDataReducer.businessUnit),
        businessUnit: assignmentInfo.assignmentProjectBusinessUnit
    };

    return {
        companyList: state.appLayoutReducer.companyList,
        country: state.masterDataReducer.countryMasterData,
        state: state.rootAssignmentReducer.workLocationState,
        city: state.rootAssignmentReducer.workLocationCity,
        assignmentGeneralDetails: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo,'object'),
        assignmentDetail : isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail,'object'),
        isInterCompanyDiscountChanged : state.rootAssignmentReducer.isInterCompanyDiscountChanged,
        assignmentStatus: state.rootAssignmentReducer.assignmentStatus,
        assignmentType: state.rootAssignmentReducer.assignmentType,
        assignmentLifeCycle: state.rootAssignmentReducer.assignmentLifeCycle,
        contractHoldingCompanyCoordinators: contractHoldingCompanyCoordinators,
        operatingCompanyCoordinators: operatingCompanyCoordinators,
        customerAssignmentContact: customerAssignmentContact,
        assignmentCompanyAddress: state.rootAssignmentReducer.assignmentCompanyAddress,
        supplierPO: state.rootAssignmentReducer.supplierPO,
        currentPage:state.CommonReducer.currentPage,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
            state.appLayoutReducer.selectedCompany),
        isTimesheetAssignment:isTimesheet(workflowType),
        isVisitAssignment:isVisit(workflowType),
        visitList: isEmptyReturnDefault(state.rootVisitReducer.visitList),
        isbtnDisable:state.rootAssignmentReducer.isbtnDisable,
        
        isSettlingTypeMargin: isSettlingTypeMargin(businessUnits),
        assignmentValidationData:isEmptyReturnDefault(state.rootAssignmentReducer.dataToValidateAssignment,'object'), //Added for Assignment Lifecycle Validation
        reviewAndModerationProcess: isEmptyReturnDefault(state.rootAssignmentReducer.reviewAndModerationProcess),

        chCoordinatorEmail : assignmentInfo.assignmentContractHoldingCompanyCoordinatorCode && getCoordinatorData(assignmentInfo.assignmentContractHoldingCompanyCoordinatorCode,
            contractHoldingCompanyCoordinators, "username", "email"),
        opCoordinatorEmail : assignmentInfo.assignmentOperatingCompanyCoordinatorCode && getCoordinatorData(assignmentInfo.assignmentOperatingCompanyCoordinatorCode,
            operatingCompanyCoordinators, "username", "email"),
        assignmentContactEmail : assignmentInfo.assignmentCustomerAssigmentContact && getCoordinatorData(assignmentInfo.assignmentCustomerAssigmentContact,
            customerAssignmentContact, "contactPersonName", "email"),
        isInternalAssignment: assignmentInfo.isInternalAssignment,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(
            {
                FetchState,
                FetchCity,
                ClearStateCityData,
                ClearCityData,                
                UpdateAssignmentGeneralDetails,
                FetchCoordinatorForOperatingCompany,
                DisplayModal, 
                HideModal,
                ClearHostDiscounts,
                ClearContractHoldingDiscounts,
                FetchSupplierContacts,
                FetchSubsuppliers,
                AddSubSupplierInformation,
                FetchMainSupplierName,
                deletSupplierInformations,
                FetchAssignmentVisits,
                FetchSubsupplierContacts,
                UpdateSubSuppliers,
                addMainSupplierData,
                FetchTimesheetState,
                FetchTimesheetCity,
                ClearICDependentData,
                IsInternalAssignment
            },
            dispatch
        )
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(GeneralDetails));