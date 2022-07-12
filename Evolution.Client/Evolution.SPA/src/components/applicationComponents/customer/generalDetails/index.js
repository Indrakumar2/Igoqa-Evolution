import GeneralDetails from './generalDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchCustomerList,
    AddCustomerAddress,
    AddCustomerContact,
    DeleteCustomerAddress,
    DeleteCustomerContact,
    UpdateAddressReference,
    ShowButtonHandler,
    UpdateContactReference,
    ClearStateCityData,
    ClearCityData,
    ClearMasterStateCityData,
    ClearMasterCityData,
    DisplayFullAddress,
    ExtranetUserAccountModalState,
    AddExtranetUser,
    AssignExtranetUserToContact,
    DeactivateExtranetUserAccount,
    EditStateAddressReference, 
} from '../../../viewComponents/customer/customerAction';
// import { FetchCity, FetchState } from '../../../viewComponents/company/companyAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchProjectList } from '../../../../actions/project/projectSearchAction'; 
import { FetchCity, FetchState, FetchStateId, FetchCityId } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {
    return {
        customerData: state.CustomerReducer.customerDetail.Detail,
        customerAddressData: state.CustomerReducer.customerDetail.Addresses,
        customerContactData: state.CustomerReducer.customerDetail.Addresses,
        stateMasterData: isEmptyReturnDefault(state.masterDataReducer.stateMasterData),
        cityMasterData: isEmptyReturnDefault(state.masterDataReducer.cityMasterData),
        countryMasterData: state.masterDataReducer.countryMasterData,
        salutationMasterData: state.masterDataReducer.salutationMasterData,
        vatPrefixMasterData: state.CompanyReducer.vatPrefixMasterData,
        showButton: state.CustomerReducer.showButton,
        // gridProps: state.CompanyReducer.gridProps,
        // secondGridProps:state.CompanyReducer.secondGridProps,
        editedAddressReference: state.CustomerReducer.editedAddressReference,
        editedContactReference: state.CustomerReducer.editedContactReference,
        loggedInUser: state.appLayoutReducer.loginUser,
        fullAddress:state.CustomerReducer.fullAddress,
        pageMode:state.CommonReducer.currentPageMode,
        isExtranetUserAccountModal:state.CustomerReducer.isExtranetUserAccountModal,
        extranetUser:isEmptyReturnDefault(state.CustomerReducer.extranetUser,'object'),
        customerProjectList:isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectSearchList),
        securityQuestionsMasterData: state.RootTechSpecReducer.TechSpecDetailReducer.passwordSecurityQuestionArray,
        selectedCustomerCode:state.CustomerReducer.selectedCustomerCode,
        selectedCompany:state.appLayoutReducer.selectedCompany,
        companyList:state.appLayoutReducer.companyList,
        extranetUsers:isEmptyReturnDefault(state.CustomerReducer.extranetUsers),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchCustomerList,
                AddCustomerAddress,
                DeleteCustomerAddress,
                DeleteCustomerContact,
                AddCustomerContact,
                FetchCity,
                FetchState,
                UpdateAddressReference,
                UpdateContactReference,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                ClearCityData,
                ClearMasterCityData,
                ClearStateCityData,
                ClearMasterStateCityData,
                DisplayFullAddress,
                ExtranetUserAccountModalState,
                AddExtranetUser,
                AssignExtranetUserToContact,
                DeactivateExtranetUserAccount, 
                FetchProjectList,
                EditStateAddressReference,
                FetchStateId,
                FetchCityId,
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(GeneralDetails);