import AppSideMenu from './sideMenu';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import {
    EditContract,
    ViewContract,
    CreateContract,
    EditProject,
    CreateProject,
    EditSupplier,
    CreateSupplier,
    CreateProfile,
    EditViewTechnicalSpecialist,
    HandleMenuAction,
    EditMyProfileDetails
} from './sideMenuAction';
import { isEmptyReturnDefault, } from '../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        userMenu: isEmptyReturnDefault(state.loginReducer.userMenu)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                EditContract,
                ViewContract,
                CreateContract,
                EditProject,
                CreateProject,
                EditSupplier,
                CreateSupplier,
                CreateProfile,
                EditViewTechnicalSpecialist,
                HandleMenuAction,
                EditMyProfileDetails,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(AppSideMenu));