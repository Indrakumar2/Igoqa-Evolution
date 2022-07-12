import AppHeader from './header';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { logOut ,FetchAnnoncements,ChangeDataAvailableStatus,FetchAboutInformation } from '../appLayout/appLayoutActions';
import { handleLogOut , UserMenu , UserType  } from '../../actions/login/loginAction';
import { AboutShowModal, AboutHideModal, ClearRefreshMasterData } from './headerAction';
import { ClearDashboardReducer , 
         ToggleAllCoordinator , 
         MyTaskPropertyChange , 
         MySearchPropertyChange ,
         Dashboardrefresh,
         FetchDashboardCount } from '../viewComponents/dashboard/dahboardActions';
import { bindActionCreators } from 'redux';
import { applicationConstants } from '../../constants/appConstants';
import { FetchAllMasterData, 
         ReloadMasterData ,
         getViewAllRightsCompanies ,
         UpdateMasterCurrency } from '../../common/masterData/masterDataActions';
import { DashboardDocumentationDetails } from '../../actions/dashboard/documentationAction';
import { FetchCostSaleReference } from '../viewComponents/company/companyAction';
import { FetchCompanyList, FetchChargeTypes, FetchUserRoleCompanyList ,FetchUserPermissionsData, UpdateSelectedCompany } from '../appLayout/appLayoutActions';
import { FetchAssignmentRefTypes } from '../viewComponents/customer/customerAction';
import { FetchTaxesOnRefresh } from '../../actions/contracts/invoicingDefaultsAction';
import { FetchGeneralDetailsMasterData  } from '../../actions/assignment/assignmentAction';
import { FetchSupplierPerformanceType } from '../../actions/visit/supplierPerformanceAction';
import { FetchPayRollNameByCompany } from '../../grm/actions/techSpec/payRateAction';
import { FetchMyTaskAssignUsers } from '../../grm/actions/techSpec/techSpecMytaskActions';

const mapStateToProps = (state) => {
  return {
    selectedCompany: state.appLayoutReducer.selectedCompany,
    companyData: state.appLayoutReducer.companyList,
    loginUser: localStorage.getItem(applicationConstants.Authentication.DISPLAY_NAME),
    showModal: state.headerReducer.showModal,
    userTypes: localStorage.getItem(applicationConstants.Authentication.USER_TYPE),
    annoncementData:state.appLayoutReducer.annoncementData,
    isDataAvailable:state.appLayoutReducer.isDataAvailable,
    systemSettingData:state.appLayoutReducer.systemSettingData,
    username: state.appLayoutReducer.username
  };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            logOut,
            handleLogOut,
            ClearDashboardReducer,
            AboutShowModal,
            AboutHideModal,
            FetchAllMasterData,
            FetchAnnoncements,
            ChangeDataAvailableStatus,
            FetchAboutInformation,
            DashboardDocumentationDetails,
            //FetchMarginType,
            FetchCostSaleReference,
            FetchAssignmentRefTypes,
            FetchCompanyList,
            FetchTaxesOnRefresh,
            FetchGeneralDetailsMasterData,
            FetchChargeTypes,
            FetchSupplierPerformanceType,
            FetchPayRollNameByCompany,
            ReloadMasterData,
            ClearRefreshMasterData,
            FetchUserRoleCompanyList,
            UserMenu,
            getViewAllRightsCompanies,
            UpdateSelectedCompany,
            FetchUserPermissionsData,
            ToggleAllCoordinator,
            MyTaskPropertyChange,
            MySearchPropertyChange,
            Dashboardrefresh,
            FetchDashboardCount,
            UserType,
            UpdateMasterCurrency,
            FetchMyTaskAssignUsers
         }, 
        dispatch
      ),
    };
  };
export default withRouter(connect(mapStateToProps,mapDispatchToProps)(AppHeader));