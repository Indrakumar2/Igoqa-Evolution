import ExpectedMarginByBusinessUnit from './expectedMarginByBusinessUnit';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { ShowButtonHandler,AddExpectedMargin,DeleteExpectedMargin,EditExpectedMargin,UpdateExpectedMargin,FetchMarginType } from '../../../viewComponents/company/companyAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
const mapStateToProps = (state) => {
    return {
        expextedMarginDetail: state.CompanyReducer.companyDetail.CompanyExpectedMargins == null ? [] : state.CompanyReducer.companyDetail.CompanyExpectedMargins,
        editExpectedMarginDetails: isEmptyReturnDefault(state.CompanyReducer.editExpectedMarginDetails,'object'),
        showButton: state.CompanyReducer.showButton,
        // buisnessUnitMasterData: isEmptyReturnDefault(state.CompanyReducer.buisnessUnitMasterData),
        buisnessUnitMasterData: isEmptyReturnDefault(state.masterDataReducer.marginType),
        loggedInUser: state.appLayoutReducer.loginUser,
        gridProps: state.CompanyReducer.gridProps,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                // FetchCompanyExpectedMargin,
                ShowButtonHandler,
                AddExpectedMargin,
                DeleteExpectedMargin,
                EditExpectedMargin,
                UpdateExpectedMargin,
                //FetchBusinessUnit,
                FetchMarginType,
                DisplayModal,
                HideModal
                     
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ExpectedMarginByBusinessUnit);