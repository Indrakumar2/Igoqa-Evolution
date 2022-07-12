import ProjectDetail from './projectDetail';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import {
    FetchContractForProjectCreation,
    SaveProjectDetails,
    CancelCreateProjectDetails,
    CancelEditProjectDetails,
    DeleteProjectDetails,
    FetchProjectDetail,
    SaveSelectedProjectNumber,
    UpdateProjectMode,
    ClearDocumentDetails
} from '../../../../actions/project/projectAction';
import {   
    GetSelectedContractData,   
    } from '../../../../actions/contracts/contractSearchAction';
import { HandleMenuAction,EditProject } from '../../../sideMenu/sideMenuAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { DeleteAlert } from '../../customer/alertAction';
import { withRouter } from 'react-router-dom';
import { FetchProjectForSupplierPOCreation } from '../../../../actions/supplierPO/supplierPOSearchAction';
import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
    return {
        selectedContract: state.RootContractReducer.ContractDetailReducer.selectedCustomerData,
        selectedProjectNo: state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo,
        projectDetails: state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo,
        projectDocuments:state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments,
        businessUnit: state.masterDataReducer.businessUnit,
        projectMode: state.RootProjectReducer.ProjectDetailReducer.projectMode,
        isbtnDisable: state.RootProjectReducer.ProjectDetailReducer.isbtnDisable,
        generalDetails: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo, 'object'),
        interactionMode: state.CommonReducer.interactionMode,
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                GetSelectedContractData,
                FetchContractForProjectCreation,
                SaveProjectDetails,
                DisplayModal,
                HideModal,
                CancelCreateProjectDetails,
                CancelEditProjectDetails,
                DeleteProjectDetails,
                DeleteAlert,
                FetchProjectDetail,
                SaveSelectedProjectNumber,
                HandleMenuAction,
                FetchProjectForSupplierPOCreation,
                SetCurrentPageMode,
                EditProject,
                UpdateProjectMode,
                ClearDocumentDetails
            },
            dispatch
        )
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ProjectDetail));