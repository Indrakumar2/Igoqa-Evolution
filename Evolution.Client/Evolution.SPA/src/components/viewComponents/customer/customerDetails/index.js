import CustomerDetails from './customerDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { SaveCustomerDetails, FetchCustomerDetail, SetLoader,CancelCustomerDetail,GetSelectedCustomerCode,ClearCustomerDocument } from '../customerAction';
import { FetchCity, FetchCountry, FetchState, FetchSalutation, FetchVatPrefix } from '../../company/companyAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        cutomerSave: state.CustomerReducer.customerSave,
        customerDetailData: isEmptyReturnDefault(state.CustomerReducer.customerDetail.Detail,'object'),
        loader: state.CustomerReducer.Loader,
        customerUpdatedStatus:state.CustomerReducer.customerUpdated,
        isbtnDisable:state.CustomerReducer.isbtnDisable,
        isOpen:state.CustomModalReducer.isOpen,
        selectedCustomer:state.CustomerReducer.selectedCustomerCode, // D-55.
        activities:state.appLayoutReducer.activities,
        pageMode:state.CommonReducer.currentPageMode,
        DocumentsData: state.CustomerReducer.customerDetail.Documents
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                SaveCustomerDetails,
                FetchCustomerDetail,
                CancelCustomerDetail,
                SetLoader,
                FetchCity,
                FetchCountry,
                FetchState,
                FetchSalutation,
                FetchVatPrefix,
                DisplayModal,
                HideModal,
                GetSelectedCustomerCode,
                ClearCustomerDocument
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CustomerDetails);   