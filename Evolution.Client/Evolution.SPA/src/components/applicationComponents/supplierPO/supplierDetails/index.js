import SupplierDetail from './supplierDetail';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';
import {
    UpdateSubSupplier,
    AddSubSupplier,
    DeleteSubSupplier,
    updateSupplierDetails,
    FetchProjectDetailForSupplier
} from '../../../../actions/supplierPO/supplierPOAction';
import {
    FetchSupplierSearchList,
    ClearSupplierSearchList,
} from '../../../../actions/supplierPO/supplierPOSearchAction';
import {
    FetchProjectDetail
} from '../../../../actions/project/projectAction';
import  { UpdateCurrentPage } from '../../../../common/commonAction';
import { GetSelectedSupplier } from '../../../../actions/supplier/supplierSearchAction';
const mapStateToProps = (state) => {
    return {
        supplierPoDetails: state.rootSupplierPOReducer.supplierPOData ? state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo : {},
        currentPage: state.CommonReducer.currentPage,
        subSupplierData: state.rootSupplierPOReducer.supplierPOData && isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPOSubSupplier),
        supplierList: state.rootSupplierPOReducer.supplierList,
        selectedSupplierpo: state.rootSupplierPOReducer.selectedSupplierpo,
        loggedInUser: state.appLayoutReducer.loginUser,
        selectedCompany: state.appLayoutReducer.selectedCompany,
        selectedCompanyData: state.appLayoutReducer.selectedCompanyData,
        pageMode:state.CommonReducer.currentPageMode,
        supplierPOViewMode:state.rootSupplierPOReducer.supplierPOViewMode   //For D-456
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UpdateSubSupplier,
                AddSubSupplier,
                DeleteSubSupplier,
                DisplayModal,
                HideModal,
                FetchSupplierSearchList,
                ClearSupplierSearchList,
                updateSupplierDetails,
                FetchProjectDetail,
                GetSelectedSupplier,
                FetchProjectDetailForSupplier,
                UpdateCurrentPage
               
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(SupplierDetail));