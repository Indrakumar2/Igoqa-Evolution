import PreAssignment from './preAssignment';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { UpdatePreAssignmentSearchParam,
    FetchContractHoldingCoordinator,
    FetchOperatingCoordinator,
    ClearContractHoldingCoordinator,
    ClearOperatingCoordinator,
    FetchAssignmentType,
    AddSubSupplier,
    UpdateSubSupplier,
    DeleteSubSupplier,
    UpdateActionDetails,
    searchPreAssignmentTechSpec,
    FetchDispositionType,
    SavePreAssignment,
    UpdatePreAssignment,
    AddOptionalSearch,
    clearPreAssignmentDetails,
    CancelEditPreAssignmentDetails,
    CancelCreatePreAssignmentDetails,
    GetGeoLocationInfo,
    FetchPreAssignment,
    TechSpechUnSavedData,
    } from '../../../actions/techSpec/preAssignmentAction';

import { FetchTechSpecCategory,
    ClearSubCategory,
    ClearServices,
    FetchTechSpecSubCategory,
    FetchTechSpecServices,
    grmFetchCustomerData,grmDetailsMasterData,
    FetchTaxonomyCustomerApproved,FetchCertificates,FetchEquipment } from '../../../../common/masterData/masterDataActions';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ClearMySearchData,FetchMySearchData } from '../../../actions/techSpec/mySearchActions';
import { isInterCompanyAssignment,isOperatorCompany } from '../../../../selectors/assignmentSelector';
import { ExportToCV, ExportToMultiCV }  from '../../../actions/techSpec/techSpecDetailAction';

const mapStateToProps = (state) => {
    const preAssignmentData = isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails,'object');
    const searchParam = isEmptyReturnDefault(preAssignmentData.searchParameter,'object');
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    return {
        currentPage: state.CommonReducer.currentPage,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList,
        companyList: state.appLayoutReducer.companyList,
        customerList: state.masterDataReducer.customerList,
        languages: state.masterDataReducer.languages,
        certificates: state.masterDataReducer.certificates,
        isPreAssignmentWon:state.RootTechSpecReducer.TechSpecDetailReducer.isPreAssignmentWon,
        contractHoldingCoodinatorList: state.RootTechSpecReducer.TechSpecDetailReducer.chCoordinators,
        operatingCoordinatorList: state.RootTechSpecReducer.TechSpecDetailReducer.ocCoordinators,
        preAssignmentDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails,'object'),
        subSuppliers: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.preAssignmentDetails.searchParameter,'object'),
        taxonomyCategory: state.masterDataReducer.techSpecCategory,
        taxonomySubCategory: state.masterDataReducer.techSpecSubCategory,
        taxonomyServices: state.masterDataReducer.techSpecServices,
        techSpecList:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techspecList),
        dispositionType:state.RootTechSpecReducer.TechSpecDetailReducer.dispositionType,
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        selectedCustomerData:isEmptyReturnDefault(state.CustomerAndCountrySearch.selectedCustomerData, 'array'),
        interactionMode:true,
        taxonomyCustomerApproved: state.masterDataReducer.taxonomyCustomerApproved,
        equipment: state.masterDataReducer.equipment,
        isInterCompanyPreAssignment: isInterCompanyAssignment(searchParam.chCompanyCode,searchParam.opCompanyCode),
        isOperatorCompany: isOperatorCompany(searchParam.opCompanyCode,selectedCompany),
        isGrmMasterDataFeteched:state.CommonReducer.isGrmMasterDataFeteched,
        isTechSpecDataChanged:state.RootTechSpecReducer.TechSpecDetailReducer.isTechSpecDataChanged,
        selectedHomeCompany:state.appLayoutReducer.selectedCompany,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UpdatePreAssignmentSearchParam,
                FetchContractHoldingCoordinator,
                FetchOperatingCoordinator,
                ClearContractHoldingCoordinator,
                ClearOperatingCoordinator,
                FetchTechSpecCategory,
                ClearSubCategory,
                ClearServices,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                FetchAssignmentType,
                AddSubSupplier,
                UpdateSubSupplier,
                DeleteSubSupplier,
                UpdateActionDetails,
                grmFetchCustomerData,
                DisplayModal,
                HideModal,
                searchPreAssignmentTechSpec,
                FetchDispositionType,
                SavePreAssignment,
                UpdatePreAssignment,
                grmDetailsMasterData,
                AddOptionalSearch,
                clearPreAssignmentDetails,
                CancelEditPreAssignmentDetails,
                CancelCreatePreAssignmentDetails,
                ClearMySearchData,
                FetchMySearchData,
                GetGeoLocationInfo,
                ExportToCV,
                ExportToMultiCV,
                FetchPreAssignment,  
                TechSpechUnSavedData, 
                FetchTaxonomyCustomerApproved,
                FetchCertificates,
                FetchEquipment,           
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(PreAssignment));