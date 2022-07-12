import ViewRole from './viewRole';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchViewRole, DeleteRoleDetails,selectedRowData } from '../../../../actions/viewRole/viewRoleAction';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
         roleData:isEmptyReturnDefault(state.rootAdminReducer.viewRole),
         roleActivityData: isEmptyReturnDefault(state.rootAdminReducer.roleActivityData),
         activities:state.appLayoutReducer.activities,
         pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { FetchViewRole,
                selectedRowData,
                DeleteRoleDetails,
                DisplayModal,
                HideModal,
                SetCurrentPageMode
             },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ViewRole));