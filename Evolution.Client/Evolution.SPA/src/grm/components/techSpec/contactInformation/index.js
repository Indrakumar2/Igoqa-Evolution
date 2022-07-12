import ContactInformation from './contactInformation';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { isEmptyReturnDefault, isUndefined, getNestedObject,getlocalizeData,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';
import {
    FetchState,
    FetchCity,
    FetchStateId,
    FetchCityId,
    ClearStateCityData,
    ClearCityData,
} from '../../../../common/masterData/masterDataActions';
import { UpdateContactInformation,UpdateContact,AutoGenerate,IsRCRMUpdatedContactInformation } from '../../../actions/techSpec/contactInformationAction';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {
    const IsRCRMUpdatedContactInfo = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedContactInfo" ]);  
    return {
        countryMasterData: state.masterDataReducer.countryMasterData,
        stateMasterData: state.masterDataReducer.stateMasterData,
        cityMasterData: state.masterDataReducer.cityMasterData,
        securityQuestionsMasterData: state.RootTechSpecReducer.TechSpecDetailReducer.passwordSecurityQuestionArray,
        contactInformationDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'),
        contactInfo: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistContact),
        defaultFieldType:state.RootTechSpecReducer.TechSpecDetailReducer.defaultFieldType,
        salutationMasterData: state.masterDataReducer.salutationMasterData,

        selectedProfileDraftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        selectedProfileDraftContactData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistContact),
        selectedProfileDraftContactInfoData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo,'object'),

        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,
        draftContactData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistContact),
        draftContactInfoData: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistInfo,'object'),
        currentPage : state.CommonReducer.currentPage,
        activities:state.appLayoutReducer.activities,
        isRCRMUpdate: IsRCRMUpdatedContactInfo?IsRCRMUpdatedContactInfo:false,
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RM }),
        isTSUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TS }),
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }),
        oldProfileAction: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchState,
                FetchCity,
                FetchStateId, // Added For ITK DEf 1536
                FetchCityId, // Added For ITK DEf 1536
                ClearStateCityData,
                ClearCityData,
                UpdateContactInformation,
                UpdateContact,
                AutoGenerate,
                IsRCRMUpdatedContactInformation
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ContactInformation));
