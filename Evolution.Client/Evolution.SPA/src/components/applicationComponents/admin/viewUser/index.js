import ViewUser from './viewUser';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import {
    SearchUser,
    DeleteUser,
    SetUserDetailData,
    ResetUserLandingPageDataState,
    ResetUserDetailState,
    ResetRolesState,
    ResetCompanyOfficeState
} from '../../../../actions/security/userAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        userLandingPageData: isEmptyReturnDefault(state.rootSecurityReducer.userLandingPageData),
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode  
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                SearchUser,
                DeleteUser,
                SetUserDetailData,
                ResetUserLandingPageDataState,
                ResetUserDetailState,
                ResetRolesState,
                ResetCompanyOfficeState,
                DisplayModal,
                HideModal,
                SetCurrentPageMode
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ViewUser));