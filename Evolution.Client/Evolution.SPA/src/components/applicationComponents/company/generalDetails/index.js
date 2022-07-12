import GeneralDetails from './generalDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {    
    ShowButtonHandler,
    AddInvoiceRemittance,
    UpdateInvoiceRemittance,
    DeleteInvoiceRemittance,
    AddInvoiceFooter,
    UpdateInvoiceFooter,
    DeleteInvoiceFooter,
    AddUpdateInvoiceDefaults,
    AddUpdateCompanyInfo,
    FetchCompanyLogoMasterData
} from '../../../viewComponents/company/companyAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {    
    return {
        companyDetail: state.CompanyReducer.companyDetail.CompanyInfo,
        InvoicingDetails: state.CompanyReducer.companyDetail.CompanyInvoiceInfo,
        editInvoiceRemittance: isEmptyReturnDefault(state.CompanyReducer.editInvoiceRemittance,'object'),
        editInvoiceFooter: isEmptyReturnDefault(state.CompanyReducer.editInvoiceFooter,'object'),
        loggedInUser: state.appLayoutReducer.loginUser,
        showButton: state.CompanyReducer.showButton,
        companyVatPrefixMasterData: state.CompanyReducer.companyVatPrefixMasterData,
        showModal:state.ModalReducer.showModal,
        companyLogo: state.CompanyReducer.companyLogo,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                //FetchCompanyDetails,
                //FetchInvoiceRemittance,
                AddInvoiceRemittance,
                UpdateInvoiceRemittance,
                DeleteInvoiceRemittance,
                //FetchInvoiceFooter,
                AddInvoiceFooter,
                UpdateInvoiceFooter,
                DeleteInvoiceFooter,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                AddUpdateInvoiceDefaults,
                AddUpdateCompanyInfo,
                FetchCompanyLogoMasterData
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(GeneralDetails);