import Assignment from './assignment';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchAssignmentReference,DeleteAccountReference,ShowButtonHandler,UpdateAccountReference,UpdateAssignmentReference, DeleteAssignmentReference, AddAssignmentReference, AddAccountReference, FetchAccountReference, FetchAssignmentRefTypes } from '../../../viewComponents/customer/customerAction';
import { DisplayModal,HideModal,CustomModalToggle } from '../../../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        customerAssignmentData: state.CustomerReducer.customerDetail.AssignmentReferences,        
        customerAccountData: state.CustomerReducer.customerDetail.AccountReferences,
        companyData: state.appLayoutReducer.companyList,
        // gridProps: state.CompanyReducer.gridProps,
        // secondGridProps:state.CompanyReducer.secondGridProps,
        assignmentEditRecord: state.CustomerReducer.editedAssignmentDetails,
        accountEditRecord: state.CustomerReducer.editedAccountDetails,
        // assignmentReferenceTypes: state.CustomerReducer.assignmentReferenceTypes,
        assignmentReferenceTypes: state.masterDataReducer.referenceType,
        showButton: state.CustomerReducer.showButton,
        editedAssignmentReference:state.CustomerReducer.editedAssignmentReference,
        editedAccountReference:state.CustomerReducer.editedAccountReference,
        loginUser:state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentRefTypes,
                AddAssignmentReference,
                AddAccountReference,
                DeleteAssignmentReference,
                DeleteAccountReference,
                UpdateAssignmentReference,
                UpdateAccountReference,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                CustomModalToggle
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Assignment);