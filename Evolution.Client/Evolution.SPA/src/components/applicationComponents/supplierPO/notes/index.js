import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddSupplierPoNotesDetails,EditSupplierPoNotesDetails  } from '../../../../actions/supplierPO/supplierPONoteAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {    
    return {
        supplierPoNotesData: state.rootSupplierPOReducer.supplierPOData && isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPONotes),        
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode,
        supplierPOViewMode:state.rootSupplierPOReducer.supplierPOViewMode   //For D-456
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                AddSupplierPoNotesDetails,
                EditSupplierPoNotesDetails
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);
