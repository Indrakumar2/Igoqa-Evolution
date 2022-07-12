import SupplierDetails from './supplierDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { isEmpty } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { FetchSupplierData,DeleteSupplierData,SaveSupplierData,UpdateSupplierData ,
    SupplierDuplicateName,CancelEditSupplierDetailsDocument,ClearSupplierDetails } from '../../../../actions/supplier/supplierAction';
import { EditSupplier,CreateSupplier } from '../../../sideMenu/sideMenuAction';
import { GetSelectedSupplier } from '../../../../actions/supplier/supplierSearchAction';
import { SetCurrentPageMode,UpdateInteractionMode,UpdateCurrentPage } from '../../../../common/commonAction';

const mapStateToProps = (state) => {    
    return{
        supplierInfo : state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierInfo,
        selectedSupplier: state.RootSupplierReducer.SupplierDetailReducers.selectedSupplier,
        interactionMode: state.CommonReducer.interactionMode,
        isbtnDisable: state.RootSupplierReducer.SupplierDetailReducers.isbtnDisable,
        currentPage: state.CommonReducer.currentPage,
        activities:state.appLayoutReducer.activities,
        duplicateMessage :state.RootSupplierReducer.SupplierDetailReducers.duplicateMessage,
        pageMode:state.CommonReducer.currentPageMode,
        documentrowData: isEmpty(state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments) ? []
            : state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                DisplayModal,
                HideModal,
                FetchSupplierData,
                DeleteSupplierData,
                SaveSupplierData,
                EditSupplier,
                CreateSupplier,
                UpdateSupplierData,
                GetSelectedSupplier,
                SupplierDuplicateName,
                SetCurrentPageMode,
                UpdateInteractionMode,
                UpdateCurrentPage,
                CancelEditSupplierDetailsDocument,
                ClearSupplierDetails
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(SupplierDetails));