import CompanyDetails from './companyDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchCompanyDetails,
    FetchCountry,
    CompanyFetchVatPrefix,
    SaveCompanyDetails,
    CancelCompanyDetails,
    GetSelectedCompanyCode,
    ClearCompanyDocument
} from '../../company/companyAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { FetchDivisionName,FetchExportPrefixes } from '../../../appLayout/appLayoutActions';

const mapStateToProps = (state) => {
    return {
        companyDetail: state.CompanyReducer.companyDetail.CompanyInfo,
        isbtnDisable:state.CompanyReducer.isbtnDisable,
        loader: state.CustomerReducer.Loader,
        companyUpdatedStatus:state.CompanyReducer.companyUpdated,
        isOpen:state.CustomModalReducer.isOpen,
        selectedCompany: state.CompanyReducer.selectedCompanyCode,   // D-55 
        companyInvoiceInfo: state.CompanyReducer.companyDetail.CompanyInvoiceInfo,
        companyDocuments:state.CompanyReducer.companyDetail.CompanyDocuments,
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchCompanyDetails,
                FetchCountry,
                CompanyFetchVatPrefix,
                SaveCompanyDetails,
                DisplayModal,
                HideModal,
                FetchDivisionName,
                FetchExportPrefixes,
                CancelCompanyDetails,
                GetSelectedCompanyCode,
                ClearCompanyDocument
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CompanyDetails);