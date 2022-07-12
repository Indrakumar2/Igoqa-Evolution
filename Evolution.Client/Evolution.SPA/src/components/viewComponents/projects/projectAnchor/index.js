import ProjectAnchor from './projectAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { 
    FetchProjectDetail,
    SaveSelectedProjectNumber,
 } from '../../../../actions/project/projectAction';
import {
    ClearSupplierPOData
} from '../../../../actions/supplierPO/supplierPOAction';
import {
    FetchProjectForSupplierPOCreation
} from '../../../../actions/supplierPO/supplierPOSearchAction';

import { SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps =(state)=>{
    return {
        currentPage: state.CommonReducer.currentPage,
        selectedCompany:state.appLayoutReducer.selectedCompany,
        pageMode:state.CommonReducer.currentPageMode,//Added for D479 issue1
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {                
                FetchProjectDetail,
                SaveSelectedProjectNumber,
                ClearSupplierPOData,
                FetchProjectForSupplierPOCreation,
                SetCurrentPageMode
            },
            dispatch
        )
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(ProjectAnchor));