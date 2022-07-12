import CreateUser from './createUser';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import {
    FetchCompanyOffice,
    ResetRolesState,
    ResetUserDetailState,
    ResetCompanyOfficeState,
    SaveUser,
    SearchRoles,
    UserUnSavedDatas,
    SetUserDetailData,   
} from '../../../../actions/security/userAction';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { getUserTypesJsonArray } from '../../../../utils/jsonUtil';

const mapStateToProps = (state) => {
    return {
        roles: isEmptyReturnDefault(state.rootSecurityReducer.roles),
        userDetailData: isEmptyReturnDefault(state.rootSecurityReducer.userDetailData),
        userLandingPageData: isEmptyReturnDefault(state.rootSecurityReducer.userLandingPageData),
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        userTypes: getUserTypesJsonArray(),
        companyOffices: isEmptyReturnDefault(state.rootSecurityReducer.companyOffices),
        homeSelectedCompany: state.appLayoutReducer.selectedCompany,
        selectedUserType: isEmptyReturnDefault(state.rootSecurityReducer.userDetailData.companyUserTypes),
        // userLogonName: isEmptyReturnDefault(state.appLayoutReducer.loginUser) - commented - because it should be SAMAccount name for adding user type. and analysed that it is used only for adding and modifying the user types.
        userLogonName : state.appLayoutReducer.username
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                SaveUser,
                SearchRoles,
                FetchCompanyOffice,
                ResetRolesState,
                ResetUserDetailState,
                ResetCompanyOfficeState,
                DisplayModal,
                HideModal,
                UserUnSavedDatas,
                SetUserDetailData               
            },
            dispatch
        ),
    };
};

export default (withRouter(connect(mapStateToProps, mapDispatchToProps)(CreateUser)));