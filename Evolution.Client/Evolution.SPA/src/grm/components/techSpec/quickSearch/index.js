import QuickSearch from './quickSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { grmDetailsMasterData, grmFetchCustomerData, clearCustomerList, FetchTechSpecCategory, ClearSubCategory, ClearServices, FetchTechSpecSubCategory,FetchTechSpecServices,
    FetchTaxonomyCustomerApproved,FetchCertificates,FetchEquipment } from '../../../../common/masterData/masterDataActions';
import { FetchAssignmentType,FetchDispositionType,TechSpechUnSavedData } from '../../../actions/techSpec/preAssignmentAction';
import { updateQuickSearchData, searchDetails, saveQuickSearchDetails,clearAllQuickSearchDetails,updateQuickSearchDetails,AddOptionalSearch,CancelEditQuickSearchDetails,CancelCreateQuickSearchDetails } from '../../../actions/techSpec/quickSearchAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { ClearMySearchData,FetchMySearchData } from '../../../actions/techSpec/mySearchActions';
import { ExportToCV,ExportToMultiCV } from '../../../actions/techSpec/techSpecDetailAction';
import { FetchQuickSearchData } from '../../../actions/techSpec/quickSearchAction';
const mapStateToProps = (state) => {
    return {
        currentPage: state.CommonReducer.currentPage,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList,
        companyList: state.appLayoutReducer.companyList,
        customerList: state.masterDataReducer.customerList,
        languages: isEmptyReturnDefault(state.masterDataReducer.languages),
        certificates: isEmptyReturnDefault(state.masterDataReducer.certificates),
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        dispositionType:state.RootTechSpecReducer.TechSpecDetailReducer.dispositionType,
        quickSearchDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.quickSearchDetails, 'object'),
        quickSearchResults: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.quickSearchResults, 'array'),
        selectedCustomerData:isEmptyReturnDefault(state.CustomerAndCountrySearch.selectedCustomerData, 'array'),
        equipment: state.masterDataReducer.equipment,
        taxonomyCustomerApproved: state.masterDataReducer.taxonomyCustomerApproved,
        isGrmMasterDataFeteched:state.CommonReducer.isGrmMasterDataFeteched,
        isTechSpecDataChanged:state.RootTechSpecReducer.TechSpecDetailReducer.isTechSpecDataChanged,
        selectedHomeCompany:state.appLayoutReducer.selectedCompany,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                grmDetailsMasterData,
                updateQuickSearchData,
                grmFetchCustomerData,
                clearCustomerList,
                searchDetails,
                saveQuickSearchDetails,
                updateQuickSearchDetails,
                FetchTechSpecCategory,
                ClearSubCategory,
                ClearServices,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                FetchAssignmentType,
                FetchDispositionType,
                clearAllQuickSearchDetails,
                AddOptionalSearch,
                DisplayModal,
                HideModal,
                CancelEditQuickSearchDetails,
                CancelCreateQuickSearchDetails,
                ClearMySearchData,
                FetchMySearchData,
                ExportToCV,
                ExportToMultiCV ,
                FetchQuickSearchData,
                TechSpechUnSavedData,
                FetchTaxonomyCustomerApproved,
                FetchCertificates,
                FetchEquipment
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(QuickSearch));