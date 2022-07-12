import SupplierpoDetails from './supplierpoDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
    FetchSupplierPoData,
    DeleteSupplierPoData,
    SaveSupplierPoData,
    CancelCreateSupplierPODetails,
    CancelEditSupplierPODetails,
    FetchProjectDetailForSupplier,
    SupplierPODuplicateName, //Added For SupplierPO Duplicate Warning
    ClearSupplierPOData,
    SetSupplierPOViewMode   //For D-456
} from '../../../../actions/supplierPO/supplierPOAction';
import {
    SaveSelectedProjectNumber
} from '../../../../actions/project/projectAction';
import { DeleteAlert } from '../../customer/alertAction';
import { HandleMenuAction } from '../../../sideMenu/sideMenuAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchProjectForSupplierPOCreation
} from '../../../../actions/supplierPO/supplierPOSearchAction';
import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        supplierPoInfo: state.rootSupplierPOReducer.supplierPOData ? state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo : {},
        interactionMode: state.CommonReducer.interactionMode,
        isbtnDisable: state.rootSupplierPOReducer.isbtnDisable,
        projectDetails: state.rootSupplierPOReducer.projectDetails,
        currentPage: state.CommonReducer.currentPage,
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        documentrowData: state.rootSupplierPOReducer.supplierPOData && isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPODocuments),
        projectNumber:state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo,
        supplierPOViewMode:state.rootSupplierPOReducer.supplierPOViewMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                FetchSupplierPoData,
                DeleteSupplierPoData,
                SaveSupplierPoData,                
                DeleteAlert,
                CancelCreateSupplierPODetails,
                CancelEditSupplierPODetails,
                SaveSelectedProjectNumber,
                HandleMenuAction,
                FetchProjectDetailForSupplier,
                SupplierPODuplicateName,      //Added For SupplierPO Duplicate Warning           
                FetchProjectForSupplierPOCreation,
                ClearSupplierPOData,
                SetSupplierPOViewMode, //For D-456
                SetCurrentPageMode
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(SupplierpoDetails));
