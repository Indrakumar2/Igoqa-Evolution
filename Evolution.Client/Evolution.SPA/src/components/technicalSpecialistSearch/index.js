import TechnicalSpecialistSearch from './technicalSpecialistSearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { FetchTechSpecData,TechSpecClearSearch } from '../../grm/actions/techSpec/techSpecSearchActions';
import {
    ClearStateCityData,
    ClearCityData,
    FetchTechSpecCategory,
    FetchTechSpecSubCategory,
    FetchTechSpecServices,
    ClearSubCategory,
    ClearServices ,
    FetchStateId,
    FetchCityId,
    grmSearchMatsterData,
    FetchProfilestatus
} from '../../common/masterData/masterDataActions';
import { clearNDTSubCategory,FetchNDTTaxonomyService,FetchNDTTaxonomySubCategory } from '../../actions/assignment/assignedSpecialistsAction';
import { isEmptyReturnDefault, } from '../../utils/commonUtils';//Added for Hot Fixes on NDT

const mapStateToProps = (state) => {
    return {
        countryMasterData: state.masterDataReducer.countryMasterData,
        stateMasterData: state.masterDataReducer.stateMasterData,
        cityMasterData: state.masterDataReducer.cityMasterData,
        // category: state.masterDataReducer.techSpecCategory,
        // subCategory: state.masterDataReducer.techSpecSubCategory,
        // services: state.masterDataReducer.techSpecServices,
        employmentStatus:state.masterDataReducer.employmentStatus,               
        techSpecSearchData:state.RootTechSpecReducer.TechSpecDetailReducer.techSpecSearchData,
        profileStatus:state.masterDataReducer.profileStatus,
        documentTypeMasterData: isEmptyReturnDefault(state.masterDataReducer.documentTypeMasterData),  //Added for Hot Fixes on NDT         
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                ClearStateCityData,
                ClearCityData,
                FetchTechSpecData,
                FetchTechSpecCategory,
                FetchTechSpecSubCategory,
                FetchTechSpecServices,
                ClearSubCategory,
                ClearServices,
                TechSpecClearSearch,
                FetchStateId,
                FetchCityId,
                grmSearchMatsterData,
                FetchProfilestatus,
                clearNDTSubCategory,
                FetchNDTTaxonomyService,
                FetchNDTTaxonomySubCategory
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(TechnicalSpecialistSearch);