import TaxonomyReport from './taxonomyReport';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { connect } from 'react-redux';
import { FetchTimeOffRequestData } from '../../../actions/techSpec/timeOffRequestAction';
import {   
    FetchState,
    FetchCity,
    ClearStateCityData,
    ClearCityData,  
    FetchTechSpecSubCategory,
    FetchTechSpecServices,
    ClearSubCategory,
    ClearServices,
    grmSearchMatsterData,
    FetchProfilestatus
    
} from '../../../../common/masterData/masterDataActions';
import { FetchTaxonomyReport,FetchResourceInfo, ClearSearchData } from '../../../actions/techSpec/reportsAction';

const mapStateToProps = (state) => {    
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        selectedCompany: state.appLayoutReducer.selectedCompany,
        resourceNameList: state.RootTechSpecReducer.TechSpecDetailReducer.resourceNameList,
        countryMasterData: isEmptyReturnDefault(state.masterDataReducer.countryMasterData),
        stateMasterData: isEmptyReturnDefault(state.masterDataReducer.stateMasterData),
        cityMasterData: isEmptyReturnDefault(state.masterDataReducer.cityMasterData),
        category: isEmptyReturnDefault(state.masterDataReducer.techSpecCategory),
        subCategory: isEmptyReturnDefault(state.masterDataReducer.techSpecSubCategory),
        services: isEmptyReturnDefault(state.masterDataReducer.techSpecServices),
        employmentStatus:state.masterDataReducer.employmentStatus,
        profileStatus:state.masterDataReducer.profileStatus,   
        taxonomyReportData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.taxonomyReportData),
        companyBasedTSData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.companyBasedTSData)
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators({
            FetchTimeOffRequestData,
            FetchState,
            FetchCity,
            ClearStateCityData,
            ClearCityData,  
            FetchTechSpecSubCategory,
            FetchTechSpecServices,
            ClearSubCategory,
            ClearServices,
            grmSearchMatsterData,
            FetchProfilestatus,
            FetchTaxonomyReport,
            FetchResourceInfo,
            ClearSearchData,
        },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TaxonomyReport));