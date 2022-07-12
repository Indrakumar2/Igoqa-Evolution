import ResourceCapability from './resourceCapability';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, isUndefined, getNestedObject,getlocalizeData,isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { UploadDocumentDetails ,RemoveDocUniqueCode,ClearDocumentUploadedDocument,RevertDeletedDocument } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import {
    //Add 
    AddLanguageCapabilityDetails,
    AddCertificateDetails,
    AddCommodityDetails,
    AddTrainingDetails,
    //Update
    UpdateLanguageDetails,
    UpdateCertificateDetails,
    UpdateCommodityDetails,
    UpdateTrainingDetails,
    UpdateResourceCapabilityCodeStandard,
    UpdateResourceCapabilityComputerKnowledge,

    //Delete
    DeleteLanguageDetails,
    DeleteCertificateDetails,
    DeleteCommodityDetails,
    DeleteTrainingDetails,
    FetchIntertekWorkHistoryReport,
    IsRCRMUpdatedResourceCapability,
    IsRCRMUpdatedResourceCapabilityCodeStandard,
    IsRCRMUpdatedResourceCapabilityComKnowledge
} from '../../../actions/techSpec/resourceCapabilityAction';
import { RemoveDocumentDetails } from '../../../../common/baseComponents/uploadDocument/uploadDocumentAction';
import {
    FetchEquipment
} from '../../../../common/masterData/masterDataActions';
import { userTypeCheck, userTypeGet } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => { 
    const IsRCRMUpdatedResourceCapabalityInfo = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedResourceCapabalityInfo" ]);  
    const IsRCRMUpdatedResourceCapabalityCodeStandardInfo = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedResourceCapabalityCodeStandardInfo" ]);  
    const IsRCRMUpdatedResourceCapabalityCompKnowledInfo = getNestedObject(state.RootTechSpecReducer.TechSpecDetailReducer, [ "RCRMUpdatedTabs", "IsRCRMUpdatedResourceCapabalityCompKnowledInfo" ]);  
    return {
        currentPage: state.RootTechSpecReducer.TechSpecDetailReducer.currentPage,
        epin: isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo)
            ? null : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        languageDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistLanguageCapabilities),
        certificateDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCertification),
        commodityDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCommodityAndEquipment),
        //commodityDetails:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.commodityDetails),
        trainingDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistTraining),
        selectedCodeStanded: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistCodeAndStandard),
        selectedComputerKnowledge: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistComputerElectronicKnowledge),
        codeStandard: state.masterDataReducer.codeStandard,
        computerKnowledge: state.masterDataReducer.computerKnowledge,
        languages: state.masterDataReducer.languages,
        certificates: state.masterDataReducer.certificates,
        commodity: state.masterDataReducer.commodity,
        equipment: state.masterDataReducer.equipment,
        training: state.masterDataReducer.training,
        loggedInUser: state.appLayoutReducer.username,
        uploadDocument: isEmptyReturnDefault(state.UploadDocumentReducer.uploadDocument),
        oldProfileActionType: state.RootTechSpecReducer.TechSpecDetailReducer.oldProfileActionType,  
        activities:state.appLayoutReducer.activities,
        
        selectedProfileDraft: state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft,
        selectedDraftLanguageDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistLanguageCapabilities),
        selectedDraftCertificationDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistCertification),
        selectedDraftEquipmentDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistCommodityAndEquipment),        
        selectedDrafttrainingDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistTraining),
        selectedDraftcodeandstand: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistCodeAndStandard),            
        selectedDraftcomputerKnowledge: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistComputerElectronicKnowledge),

        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,
        draftLanguageDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistLanguageCapabilities),
        draftCertificationDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistCertification),
        draftEquipmentDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistCommodityAndEquipment),        
        drafttrainingDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistTraining),
        draftcodeandstand: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistCodeAndStandard),            
        draftcomputerKnowledge: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistComputerElectronicKnowledge),
        pageMode:state.CommonReducer.currentPageMode,

        technicalSpecialistInfo:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'),/**Added for TM Edit/View Access changes, as per the Admin User Guide document 20-11-19 (ITK requirement)*/
        intertekWorkHistoryReport: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.TechnicalSpecialistIntertekWorkHistory),
        isRCRMUpdate: IsRCRMUpdatedResourceCapabalityInfo?IsRCRMUpdatedResourceCapabalityInfo:false,
        isRCRMUpdateCodeStandard :IsRCRMUpdatedResourceCapabalityCodeStandardInfo?IsRCRMUpdatedResourceCapabalityCodeStandardInfo:false,
        isRCRMUpdateCompKnowledge : IsRCRMUpdatedResourceCapabalityCompKnowledInfo?IsRCRMUpdatedResourceCapabalityCompKnowledInfo:false,
        isRCUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.RM }),
        isTSUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TS }), 
        isTMUserTypeCheck:userTypeCheck( { array: userTypeGet() ,param:localConstant.techSpec.userTypes.TM }), 
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                AddLanguageCapabilityDetails,
                AddCertificateDetails,
                AddCommodityDetails,
                AddTrainingDetails,
                UpdateLanguageDetails,
                UpdateCertificateDetails,
                UpdateCommodityDetails,
                UpdateTrainingDetails,
                UpdateResourceCapabilityCodeStandard,
                UpdateResourceCapabilityComputerKnowledge,
                DeleteLanguageDetails,
                DeleteCertificateDetails,
                DeleteCommodityDetails,
                DeleteTrainingDetails,
                UploadDocumentDetails,
                DisplayModal,
                HideModal,
                RemoveDocumentDetails,
                RemoveDocUniqueCode,
                FetchEquipment,
                ClearDocumentUploadedDocument,
                RevertDeletedDocument,
                FetchIntertekWorkHistoryReport,
                IsRCRMUpdatedResourceCapability,
                IsRCRMUpdatedResourceCapabilityCodeStandard,
                IsRCRMUpdatedResourceCapabilityComKnowledge
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(ResourceCapability));