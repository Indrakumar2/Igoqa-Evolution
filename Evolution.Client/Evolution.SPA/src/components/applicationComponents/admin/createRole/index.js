import CreateRole from './createRole';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchRoleData, AddRoleData, UpdateRoleData } from '../../../../actions/createRole/createRoleAction';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { UserUnSavedDatas } from '../../../../actions/security/userAction';
import { selectedRowData } from '../../../../actions/viewRole/viewRoleAction';
import { UpdateCurrentModule, UpdateCurrentPage } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        viewRole: isEmptyReturnDefault(state.rootAdminReducer.viewRole),
        moduleList: isEmptyReturnDefault(state.rootAdminReducer.moduleList),
        //moduleListData: isEmptyReturnDefault(state.rootAdminReducer.moduleListData),
        moduleActivityData: isEmptyReturnDefault(state.rootAdminReducer.moduleActivityData),
        roleActivityData: isEmptyReturnDefault(state.rootAdminReducer.roleActivityData),
        selectedRole:isEmptyReturnDefault(state.rootAdminReducer.selectedRole)        
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                FetchRoleData,
                AddRoleData,
                UpdateRoleData,                
                DisplayModal, 
                HideModal,
                UserUnSavedDatas,
                selectedRowData,
                UpdateCurrentModule,
                UpdateCurrentPage 
             },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(CreateRole));