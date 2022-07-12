import CompanyOffices from './companyOffices';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddCompanyOffice, UpdateCompanyOffice, DeleteCompanyOffices, FetchState, FetchCountry, FetchCity, ShowButtonHandler, ClearStateCityData,ClearCityData, EditCompanyOffice } from '../../../viewComponents/company/companyAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchStateId,FetchCityId } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {
    return {
        companyOfficeDetail: state.CompanyReducer.companyDetail.CompanyOffices == null ? [] : state.CompanyReducer.companyDetail.CompanyOffices,
        editRecord: isEmptyReturnDefault(state.CompanyReducer.editedcompanyOffice,'object'),        
        showButton: isEmptyReturnDefault(state.CompanyReducer.showButton,'boolean'),
        // cityMasterData: state.CompanyReducer.cityMasterData,
        // stateMasterData: isEmptyReturnDefault(state.CompanyReducer.stateMasterData),
        //countryMasterData: isEmptyReturnDefault(state.CompanyReducer.countryMasterData),
        cityMasterData: state.masterDataReducer.cityMasterData, //Added for ITK D1536
        stateMasterData: isEmptyReturnDefault(state.masterDataReducer.stateMasterData), //Added for ITK D1536
        countryMasterData: isEmptyReturnDefault(state.masterDataReducer.countryMasterData), //Added for ITK D1536
        loggedInUser: state.appLayoutReducer.loginUser,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {                
                AddCompanyOffice,
                UpdateCompanyOffice,
                DeleteCompanyOffices,                
                FetchState,
                FetchStateId,
                FetchCountry,
                FetchCity,
                FetchCityId,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                ClearStateCityData,
                EditCompanyOffice,
                ClearCityData
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(CompanyOffices);