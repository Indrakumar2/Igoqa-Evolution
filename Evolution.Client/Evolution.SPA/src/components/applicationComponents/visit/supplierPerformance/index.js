import SupplierPerformance from './supplierPerformance';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { 
    FetchSupplierPerformance,
    AddSupplierPerformance,
    UpdateSupplierPerformance,
    DeleteSupplierPerformance,
    FetchSupplierPerformanceType
} from '../../../../actions/visit/supplierPerformanceAction';
import { 
    DisplayModal, 
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';
import {   
    isOperatorApporved,
    isOperatorCompany
} from '../../../../selectors/visitSelector';
import { 
    isInterCompanyAssignment
} from '../../../../selectors/assignmentSelector';

const mapStateToProps = (state) => { 
    const visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object');
    return {        
        visitInfo: visitInfo,
        VisitSupplierPerformances: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitSupplierPerformances),
        loggedInUser: state.appLayoutReducer.loginUser,
        supplierPerformanceTypeList: isEmptyReturnDefault(state.rootVisitReducer.supplierPerformanceTypeList),
        currentPage: state.CommonReducer.currentPage,
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        pageMode:state.CommonReducer.currentPageMode,
        isOperatorApporved: isOperatorApporved(visitInfo),
        isOperatingCompany: isOperatorCompany(visitInfo.visitOperatingCompanyCode, state.appLayoutReducer.selectedCompany),
        isInterCompanyAssignment: isInterCompanyAssignment(visitInfo.visitContractCompanyCode,
            visitInfo.visitOperatingCompanyCode),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchSupplierPerformance,
                DisplayModal, 
                HideModal,
                AddSupplierPerformance,
                UpdateSupplierPerformance,
                DeleteSupplierPerformance,
                FetchSupplierPerformanceType
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(SupplierPerformance));