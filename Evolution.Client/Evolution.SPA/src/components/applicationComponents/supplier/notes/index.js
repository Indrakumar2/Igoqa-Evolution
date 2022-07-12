import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddSupplierNotes,EditSupplierNotes } from '../../../../actions/supplier/notesAction';
import { isEmpty } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {    
    return{
        supplierNotesData: state.RootSupplierReducer.SupplierDetailReducers.supplierData&&
         isEmpty(state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierNotes)?[]:state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierNotes,            
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                AddSupplierNotes,
                EditSupplierNotes
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);