import SupplierInformation from './supplierInformation';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchSupplierContacts,
         FetchSubsuppliers,
         FetchSubsupplierContacts,
         AddSubSupplierInformation,
         UpdateSupplierInformation,
         AddMainSupplierContactInformation,
         AddSupSupplierTechSpec,
         UpdateMainSupplierInfo,
         UpdateSubSuppliers,
         deleteNDTSubSupplierTS,
         updateSubSupplierContact,
         updateSubSupplierFirstVisit,
         updatePartOfAssignment,
      //   UpdateTechSpecAssignedSubSupplier,
         FetchAssignmentTechSpec } from '../../../../actions/assignment/supplierInformationAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import  {    isOperatorCompany,
    isContractHolderCompany,
    isInterCompanyAssignment,
    isARS  } from '../../../../selectors/assignmentSelector';
import { FetchAssignmentVisits } from '../../../../actions/visit/visitAction';

const mapStateToProps = (state) => {
    const assignmentInfo = isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object');
    const selectedCompany= state.appLayoutReducer.selectedCompany;
    const businessUnit = isEmptyReturnDefault(state.masterDataReducer.businessUnit);
    return {
        supplierContacts: isEmptyReturnDefault(state.rootAssignmentReducer.supplierContacts),
        assignmentDetails: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo,'object'),
        subsuppliers:isEmptyReturnDefault(state.rootAssignmentReducer.subsuppliers),
        subsupplierContacts:isEmptyReturnDefault(state.rootAssignmentReducer.subsupplierContacts),
        assignmentSubSupplier:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentSubSuppliers),
        currentPage: state.CommonReducer.currentPage,
        assignedtechSpec: isEmptyReturnDefault(state.rootAssignmentReducer.assignedSubSupplierTS),
        techSpec:isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists),
        assignmentId:state.rootAssignmentReducer.assignmentDetail &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentId,
        assignmentTechSpec : isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentTechnicalSpecialists),
        supplierPOList: isEmptyReturnDefault(state.rootAssignmentReducer.supplierPO),
        isOperatorCompany:isOperatorCompany(assignmentInfo.assignmentOperatingCompanyCode,
            selectedCompany),
        isContractHolderCompany:isContractHolderCompany(assignmentInfo.assignmentContractHoldingCompanyCode,
            selectedCompany),
        isInterCompanyAssignment:isInterCompanyAssignment(assignmentInfo.assignmentContractHoldingCompanyCode,assignmentInfo.assignmentOperatingCompanyCode),
        isArsAssignment: isARS({ projectReportParams: businessUnit,businessUnit: assignmentInfo.assignmentProjectBusinessUnit }),
        isSubSupplierContactUpdated: state.rootAssignmentReducer.isSubSupplierContactUpdated,
        visitList: isEmptyReturnDefault(state.rootVisitReducer.visitList),
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierContacts,
                FetchSubsuppliers,
                FetchSubsupplierContacts,
                AddSubSupplierInformation,
                UpdateSupplierInformation,
                AddMainSupplierContactInformation,
                AddSupSupplierTechSpec,
                UpdateMainSupplierInfo,
                updateSubSupplierContact,
                updateSubSupplierFirstVisit,
                updatePartOfAssignment,
            //    UpdateTechSpecAssignedSubSupplier,
                FetchAssignmentTechSpec,
                deleteNDTSubSupplierTS,
                UpdateSubSuppliers,
                DisplayModal,
                FetchAssignmentVisits,
                HideModal
            },
            dispatch
        )
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(SupplierInformation);