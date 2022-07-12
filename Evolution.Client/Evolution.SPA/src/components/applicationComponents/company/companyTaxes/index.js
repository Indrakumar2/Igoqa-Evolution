import CompanyTaxes from './companyTaxes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { AddCompanyTaxes, UpdateCompanyTaxes, DeleteCompanyTaxes, ShowButtonHandler } from '../../../viewComponents/company/companyAction';

const mapStateToProps = (state) => {
    return {
        CompanyTaxes: state.CompanyReducer.companyDetail.CompanyTaxes == null ? [] : state.CompanyReducer.companyDetail.CompanyTaxes,        
        taxMasterData: isEmptyReturnDefault(state.CompanyReducer.taxMasterData),
        editCompanyTaxes: isEmptyReturnDefault(state.CompanyReducer.editCompanyTaxes,'object'),
        showButton: isEmptyReturnDefault(state.CompanyReducer.showButton,'boolean'),
        loggedInUser: state.appLayoutReducer.loginUser,
        selectedCompanyCode: state.CompanyReducer.selectedCompanyCode,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddCompanyTaxes,
                UpdateCompanyTaxes,
                DeleteCompanyTaxes,
                ShowButtonHandler,
                DisplayModal,
                HideModal          
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CompanyTaxes);