import ClientNotification from './clientNotification';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty } from '../../../../utils/commonUtils';
import {
    AddClientNotification,
    DeleteClientNotification,
    UpdatetNotificationData,
    FetchCustomerContact
} from '../../../../actions/project/clientNotificationAction';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        clientNotificationGrid:isEmpty(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotifications)?[]:state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectNotifications,
        editedClientNotificationData:state.RootProjectReducer.ProjectDetailReducer.editedClientNotificationData,
        loggedInUser: state.appLayoutReducer.loginUser,
        customerContacts:state.RootProjectReducer.ProjectDetailReducer.customerContact,
        // interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddClientNotification,
                DeleteClientNotification,
                UpdatetNotificationData,
                FetchCustomerContact,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(ClientNotification);