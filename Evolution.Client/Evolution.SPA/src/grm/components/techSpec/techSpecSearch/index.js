import TechSpecSearch from './techSpecSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchTechSpecData,TechSpecClearSearch } from '../../../actions/techSpec/techSpecSearchActions';
import { SetCurrentPageMode } from '../../../../common/commonAction';
import {   
    FetchStateId,
    FetchCityId,
    ClearStateCityData,
    ClearCityData,  
    FetchTechSpecSubCategory,
    FetchTechSpecServices,
    ClearSubCategory,
    ClearServices,
    grmSearchMatsterData,
    FetchProfilestatus
    
} from '../../../../common/masterData/masterDataActions';
import { FetchUserRoleCompanyList } from "../../../../components/appLayout/appLayoutActions";
import { fetchMatchedObject } from '../../../../common/selector';

const mapStateToProps = (state) => {
    return {
        countryMasterData: isEmptyReturnDefault(state.masterDataReducer.countryMasterData),
        stateMasterData: isEmptyReturnDefault(state.masterDataReducer.stateMasterData),
        cityMasterData: isEmptyReturnDefault(state.masterDataReducer.cityMasterData),
        category: isEmptyReturnDefault(state.masterDataReducer.techSpecCategory),
        subCategory: isEmptyReturnDefault(state.masterDataReducer.techSpecSubCategory),
        services: isEmptyReturnDefault(state.masterDataReducer.techSpecServices),
        employmentStatus:state.masterDataReducer.employmentStatus,               
        techSpecSearchData:state.RootTechSpecReducer.TechSpecDetailReducer.techSpecSearchData,
        profileStatus:state.masterDataReducer.profileStatus,          
        userName:state.appLayoutReducer.username,
        userRoleCompanyList: state.appLayoutReducer.userRoleCompanyList, 
        selectedCompany: state.appLayoutReducer.selectedCompany,
        activities:state.appLayoutReducer.activities,
        documentTypeMasterData: isEmptyReturnDefault(state.masterDataReducer.documentTypeMasterData),     /** Added for Hot Fixes on NDT */  
        selectedCompanyName:fetchMatchedObject({ list:state.appLayoutReducer.userRoleCompanyList,param:'companyCode',id:state.appLayoutReducer.selectedCompany }),   
        selectedHomeCompany:state.appLayoutReducer.selectedCompany,
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                FetchStateId, //Added for ITK D1536
                FetchCityId, //Added for ITK D1536
                ClearStateCityData,
                ClearCityData,
                FetchTechSpecData,
                FetchProfilestatus,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                ClearSubCategory,
                ClearServices,
                TechSpecClearSearch,
                grmSearchMatsterData,
                SetCurrentPageMode,
                FetchUserRoleCompanyList,
                
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TechSpecSearch));