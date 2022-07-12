import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty } from '../../../../utils/commonUtils';
import { AddNotesDetails,EditNotesDetails } from '../../../viewComponents/customer/customerAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        notesData: isEmpty(state.CustomerReducer.customerDetail.Notes) ? [] : state.CustomerReducer.customerDetail.Notes,
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddNotesDetails,
                DisplayModal,
                HideModal,
                EditNotesDetails//D661 issue8
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);