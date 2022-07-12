import InterCompanyDiscount from './interCompanyDiscount';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { FetchInterCompanyDiscounts, UpdateICDiscounts } from '../../../../actions/visit/interCompanyDiscountsAction';
import { isCoordinatorCompany } from '../../../../selectors/visitSelector';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';
import { FetchContractData, FetchContractDataForAssignment } from '../../../../actions/contracts/contractAction';

const mapStateToProps = (state) => { 
    const visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object');
    return {        
        InterCompanyDiscounts: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInterCompanyDiscounts,'object'),
        currentPage: state.CommonReducer.currentPage,
        visitInfo: visitInfo,
        loggedInUser: state.appLayoutReducer.loginUser,
        isCoordinatorCompany: isCoordinatorCompany(visitInfo.visitContractCompanyCode, state.appLayoutReducer.selectedCompany),
        isInterCompanyAssignment: isInterCompanyAssignment(visitInfo.visitContractCompanyCode, visitInfo.visitOperatingCompanyCode)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchInterCompanyDiscounts,
                UpdateICDiscounts,
                FetchContractDataForAssignment
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(InterCompanyDiscount));