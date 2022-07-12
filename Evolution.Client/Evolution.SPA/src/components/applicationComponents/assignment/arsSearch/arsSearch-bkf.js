import React, { Component, Fragment } from 'react';
import Panel from '../../../../common/baseComponents/panel';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,formInputChangeHandler, isEmptyReturnDefault, isEmpty } from '../../../../utils/commonUtils';
import { HeaderData } from './arsSearchHeader';
import SearchGRM from '../../../../common/resourceSearch/searchGRM';
import MoreDetails from '../../../../common/resourceSearch/moreDetails';
import ActionSearch from '../../../../common/resourceSearch/actionSearch';
import SubSupplierDetails from '../../../../common/resourceSearch/subSupplierDetails';
import AssignedResource from '../../../../common/resourceSearch/subSupplierDetails';
import { SupplierInfoDiv,AssignMentType,CategoryInfoDiv,MaterialInfoDiv,FirstVisit,TimesheetLocation,ResourceAssigned } from '../../../../common/resourceSearch/resourceFields';
import CustomerAndCountrySearch from '../../../../components/applicationComponents/customerAndCountrySearch';
import { required } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();
export const ArsSearchDiv = (props) => {
    const { companyList,inputChangeHandler,assignmentData,preAssignmentIds } = props;
    return(  
        <div className="row mb-2">
            <div className="col s12 pr-0 pl-0">
                <CustomerAndCountrySearch
                    customerName = {assignmentData.customerName}
                    disabled = {true} 
                    colSize="col s4 pl-0"/>
                <CustomInput
                    hasLabel={true}                   
                    name="contractNumber"
                    colSize='s4 pl-0'
                    label={localConstant.resourceSearch.CONTRACT_NUMBER}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    disabled={props.interactionMode}
                    value = {assignmentData.contractNumber}
                    onValueChange={props.inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="id"
                    id="preAssignmentId"
                    colSize='s4 pl-0'
                    label={localConstant.resourceSearch.PRE_ASSIGNMENT_ID}
                    optionsList={preAssignmentIds}
                    labelClass="customLabel"
                    optionName='id'
                    optionValue="id"
                    type='select'
                    inputClass="customInputs"
                    defaultValue={""}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="projectName"
                    colSize='s4'
                    label={localConstant.resourceSearch.PROJECT_NAME}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    disabled={props.interactionMode}
                    value = {assignmentData.projectName}
                    onValueChange={props.inputChangeHandler} />                
                <CustomInput
                    hasLabel={true}
                    name="projectNumber"
                    colSize='s4 pl-0'
                    label={localConstant.resourceSearch.PROJECT_NUMBER}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    disabled={props.interactionMode}
                    value = {assignmentData.projectNumber}
                    onValueChange={props.inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="assignmentNumber"
                    colSize='s4 pl-0'
                    label={localConstant.resourceSearch.ASSIGNMENT_NUMBER}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    disabled={props.interactionMode}
                    value = {assignmentData.assignmentNumber}
                    onValueChange={props.inputChangeHandler} />
            </div>
        </div>
    );
};

class ArsSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpenMoreDetails: false,
            isPanelOpenSearchParams:true,
            isPanelOpenSubSupplier:false,
            isPanelOpenAssignedResource:false,
            optionAttributs: {
                optionName: 'name',
                optionValue: 'name'
            },
            expiryFromDate:'',
            expiryToDate:'',
        };
        this.updatedData = {}; 
        /** Sub suppleir grid buttons */
        this.subSupplierGridBtn = [
            {
                name: localConstant.commonConstants.ADD,
                action: this.addSubSupplier,
                btnClass: "btn-small mr-1 mt-1",
                showbtn: false
            },
            {
                name: localConstant.commonConstants.DELETE,
                action: this.deleteSubSupplier,
                btnClass: "btn-small mt-1 ",
                showbtn: false
            }
        ]; 
        /** Assigned Resource grid buttons */
        this.assignedResourcesGridBtn = [
            {
                name: localConstant.commonConstants.ADD,
                action: this.addAssignedResource,
                btnClass: "btn-small mr-1 mt-1",
                showbtn: false
            },
            {
                name: localConstant.commonConstants.DELETE,
                action: this.deleteAssignedResource,
                btnClass: "btn-small mt-1 ",
                showbtn: false
            }
        ]; 
        /** Grm Search buttons */
        this.optionalSearchGrm = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.arsOptionalSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        /** Assign resources to assignment button */
        this.arsAssignResourcesBtn = [
            {
                name: localConstant.resourceSearch.SAVE_RESOURCE,
                action: (e) => this.saveResourceHandler(e),
                btnClass: "btn-small mr-1",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.CANCEL,
                action: (e) => this.cancelResourceSearch(e),
                btnClass: "btn-small ",
                showbtn: true,
                type:"button"
            }
        ];    
        /** ARS search buttons */
        this.ARSSearchBtn = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.preAssignmentSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.assignedSearchBtn = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.assignedSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
    }  
                 
    /** Panel click handler */
    panelClick = (params) => {
        this.setState({ [`isPanelOpen${ params }`]: !this.state[`isPanelOpen${ params }`]  });   
    }

    /** Input change handler - for search parameters */
    inputChangeHandler =(e)=>{
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if(inputvalue.name === "id"){
            this.props.actions.GetSelectedPreAssignment({ "id":inputvalue.value });
        }
        this.updatedData = {};
    };

    /** on change handler - for action section */
    actionOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        if(inputValue.name === "searchAction" && (inputValue.value === "SD")){
            this.props.actions.FetchDispositionType(inputValue.value);
        }
        this.props.actions.UpdateActionDetails(this.updatedData);
        this.updatedData = {};
    };

    /** on change handler - for optional parameter */
    optionalParamOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
    };

    /** on change handler - for Sub-Supplier infos */
    subSupplierOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
    };

    /** Save resource handler */
    saveResourceHandler = (e) => {
        e.preventDefault();
        const isValid = this.arsMandatoryFieldCheck();
        if(isValid){
            const selectedRecords = this.searchGridChild.getSelectedRows();
            const selectedTechSpec = [];
            if(selectedRecords.length > 0) {
                selectedRecords.forEach(iteratedValue => {
                    let alreadySelected = false;
                    let duplicateTechSpec = {};
                    if (this.props.assignedTechSpec && this.props.assignedTechSpec.length > 0) {
                        this.props.assignedTechSpec.forEach(element => {
                            if (element.epin === iteratedValue.epin && element.recordStatus !== 'D') {
                                duplicateTechSpec = element;
                                alreadySelected = true;
                            }
                        });
                    }
                    if (alreadySelected) {
                        IntertekToaster(duplicateTechSpec.technicalSpecialistName + " already selected", "warningToast alreadyAssigned" + duplicateTechSpec.technicalSpecialistName);
                    }
                    else{
                        selectedTechSpec.push(iteratedValue);
                    }
                });
                this.props.actions.AssignTechnicalSpecialistFromARS(selectedTechSpec);
            }
            this.props.actions.ARSSearchPanelStatus(false);
            this.props.actions.clearPreAssignmentDetails();
            this.props.actions.clearARSSearchDetails();
        }
    };

    /** Cancel Resource Search and go back to the assigned specialist page */
    cancelResourceSearch = (e) => {
        e.preventDefault();
        this.props.actions.ARSSearchPanelStatus(false);
        this.props.actions.clearPreAssignmentDetails();
        this.props.actions.clearARSSearchDetails();
    };

    /** ARS Search Mandatory Field Check */
    arsMandatoryFieldCheck = () => {
        const arsSearchDetail = this.props.assignmentData;
        if(required(arsSearchDetail.searchAction)){
            IntertekToaster(`${ localConstant.resourceSearch.ACTION } - ${ localConstant.resourceSearch.ACTION }`,'warningToast searchActionVal');
            return false;
        }
        if(arsSearchDetail.searchAction ==="SD" || arsSearchDetail.searchAction === "OPR" || arsSearchDetail.searchAction === "PLO"){
            IntertekToaster("Under Construction",'warningToast searchDispositionVal');
            return false;
            /** TO-DO: Do disposition type check and proceed the flow. */
            // if(required(arsSearchDetail.dispositionType)){
            //     IntertekToaster(`${ localConstant.resourceSearch.DISPOSITION_DETAILS } - ${ localConstant.resourceSearch.DISPOSITION_DETAILS }`,'warningToast searchDispositionVal');
            //     return false;
            // }
        }
        return true;
    };
   
    componentDidMount(){
        this.props.actions.grmDetailsMasterData();
        this.props.actions.FetchTechSpecCategory();
        this.props.actions.FetchAssignmentType();
    }

    /** Assigned Resources JSON Formation */
    assignedTechSpecJSONformation = (data) => {
        const techSpecInfo = [];
        if(data.length > 0){
            data.forEach(iteratedValue => {
                iteratedValue.assignedTechSpec.forEach(element => {
                    const techSpec = {};
                    techSpec.epin = element.epin;
                    techSpec.resourceName = `${ element.lastName } ${ element.firstName }`;
                    techSpec.profileStatus = element.profileStatus;
                    techSpec.supplierName = iteratedValue.supplierName;
                    techSpec.taxonomy = iteratedValue.taxonomyServiceName;
                    techSpecInfo.push(techSpec);
                });
            });
            return techSpecInfo;
        }
        else{
            return techSpecInfo;
        }
    };

    resourceDatas = (data) => {
        if(data && data.length > 0){
            const techSpecArray = [];
            data.forEach(iteratedValue => {
                const techSpecName = `${ iteratedValue.lastName } ${ iteratedValue.firstName }`;
                techSpecArray.push(techSpecName);
            });
            return techSpecArray.join();
        }
        return "N/A";
    };

    convertToMultiSelectObject=(datas)=>{
        const multiselectArray = [];
        if (datas) {
            datas.map(data => {
                multiselectArray.push({ name: data });
            });
        }
        return multiselectArray;
    }

     //Multi Select Change Events
     certificationMultiselect = (value) => {
        const certificates = [];
        value.map(certificate => {
            certificates.push(certificate.label);
        });
        this.updatedData["certification"] = certificates;
        this.props.actions.AddOptionalSearch(this.updatedData);
    }

    languageSpeakingMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageSpeaking"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
    }

    languageWritingMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageWriting"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
    }

    languageComprehensionMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageComprehension"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
    }

    /**on Change handler - for OptionalSearch */
    optionalOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData = {};
    };

     /**expirt Date Hander */
     expiryFrom = (date) => {
        this.setState({ 
            expiryFromDate: date 
        },() => {
            this.updatedData.certificationExpiryFrom = this.state.expiryFromDate !== null ? this.state.expiryFromDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            this.props.actions.AddOptionalSearch(this.updatedData);
            this.updatedData = {};
        }); 
    }

    expiryTo = (date) => {
        this.setState({ 
            expiryToDate: date 
        },() => {
            this.updatedData.certificationExpiryTo = this.state.expiryToDate !== null ? this.state.expiryToDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            this.props.actions.AddOptionalSearch(this.updatedData);
            this.updatedData = {};
        }); 
    }

    /**TechSpec Search */
    arsOptionalSearch = (e) => {
        e.preventDefault();
        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
        this.updatedData = {};
        this.props.actions.searchPreAssignmentTechSpec();
    }
    
    render() {
        const { currentPage,companyList,contractHoldingCoodinatorList,operatingCoordinatorList,interactionMode,
            assignmentData ,assignmentTypes,assignmentStatus,taxonomyCategory,taxonomySubCategory,taxonomyServices,
            customerList,preAssignmentIds,techSpecList,dispositionType,isResourceMatched,selectedPreAssignmentSearchParam,
            languages,certificates } = this.props;
        const searchParameters = isEmptyReturnDefault(assignmentData.searchParameter,'object');
        const subSupplierInfos = isEmptyReturnDefault(assignmentData.subSupplierInfos);
        const assignedResources = isEmptyReturnDefault(searchParameters.assignedResourceInfos);
        const optionalSearch= isEmptyReturnDefault(searchParameters.optionalSearch, 'object');
        const techSpecs = isEmptyReturnDefault(searchParameters.selectedTechSpecInfo);
        const selectedPreAssignmentTechSpecs = isEmptyReturnDefault(selectedPreAssignmentSearchParam.selectedTechSpecInfo);
        const arsSearchAction = localConstant.resourceSearch.arsActions;
        const isTimesheet = (searchParameters.workFlowType === 'M' || searchParameters.workFlowType === 'N') ? true : false;
        const isVisit = (searchParameters.workFlowType === 'V' || searchParameters.workFlowType === 'S') ? true : false;
        this.headerData = HeaderData(techSpecs);
        const unmatchedResources = this.resourceDatas(selectedPreAssignmentTechSpecs);
        const isARSSearch = true;

        this.techSpecAssigned = this.assignedTechSpecJSONformation(assignedResources);

                //Default values for Optional Param Multi select
                const defaultCertificateMultiSelection =this.convertToMultiSelectObject(optionalSearch.certification);
                // const defaultCertificateMultiSelection = [ { name: 'PT-SNT-TC-1A' } ];
                const defaultLanguageSpeakingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageSpeaking);
                const defaultLanguageWritingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageWriting);
                const defaultLanguageComprehensionMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageComprehension);

        return (
            <Fragment>              
                <div className="customCard topSectionMargin">
                    <form onSubmit={this.techSpecSearch} autoComplete="off">
                    <CardPanel className="white lighten-4 black-text" colSize="s12 mb-2">
                        <ArsSearchDiv
                            inputChangeHandler={(e) => this.inputChangeHandler(e)}
                            techSpecSearch={this.preAssignmentSearch}
                            assignmentData = { searchParameters }
                            preAssignmentIds = { preAssignmentIds }
                            customerList = {customerList}
                            interactionMode={interactionMode}/>
                    </CardPanel>
                        <Panel colSize="s12 pl-0 pr-0" heading={ localConstant.resourceSearch.MORE_DETAILS } isArrow={true} onpanelClick={(e) => this.panelClick('MoreDetails')} isopen={this.state.isPanelOpenMoreDetails} >
                            <MoreDetails                               
                                currentPage={currentPage}
                                isARSSearch = {isARSSearch}
                                interactionMode={interactionMode}
                                moreDetailsData={searchParameters}
                                companyList = { companyList }
                                contractHoldingCoodinatorList = { contractHoldingCoodinatorList }
                                operatingCoordinatorList = { operatingCoordinatorList }
                                inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                        </Panel>
                        <Panel colSize="s12 pl-0 pr-0" heading={isVisit? localConstant.resourceSearch.SUPPLIER + '/' +localConstant.resourceSearch.SUBSUPPLIER : localConstant.resourceSearch.TIMESHEET_DETAILS} isArrow={true} onpanelClick={(e) => this.panelClick('SubSupplier')} isopen={this.state.isPanelOpenSubSupplier} >
                            <div className="row mb-0">
                                {isVisit ?
                                    <div>
                                        <SupplierInfoDiv
                                            interactionMode={interactionMode}
                                            isARSSearch={isARSSearch}
                                            moreDetailsData={searchParameters}
                                            inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                                        <SubSupplierDetails
                                            gridData={subSupplierInfos}
                                            gridHeaderData={this.headerData.supplierHeader}
                                            isSubSupplierGrid={true}
                                            subSupplierGridBtn={this.subSupplierGridBtn}
                                            buttons={this.preAssignmentSearchBtn}
                                            currentPage={currentPage}
                                            isARSSearch={isARSSearch}
                                            interactionMode={interactionMode}
                                            inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                                    </div> : null

                                }
                                {/* <MaterialInfoDiv
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                    moreDetailsData={searchParameters}
                                    interactionMode={interactionMode} /> */}
                                <FirstVisit
                                    isARSSearch={isARSSearch}
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                               
                                <AssignMentType
                                    isARSSearch={isARSSearch}
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    assignmentType={assignmentTypes}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                    assignMentStatusList={assignmentStatus}
                                    optionAttributs={this.state.optionAttributs} />
                                <CategoryInfoDiv
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    taxonomyCategory={taxonomyCategory}
                                    taxonomySubCategory={taxonomySubCategory}
                                    taxonomyServices={taxonomyServices}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                                {!isResourceMatched ?
                                <ResourceAssigned
                                    assignedResourceData = {unmatchedResources}
                                    interactionMode = {true} 
                                    isResourceNotMatched = {!isResourceMatched} /> : null}
                            </div>
                        </Panel>
                        <Panel colSize="s12 pl-0 pr-0" heading={localConstant.resourceSearch.ASSIGNED_RESOURCE} isArrow={true} onpanelClick={(e) => this.panelClick('AssignedResource')} isopen={this.state.isPanelOpenAssignedResource} >
                            <div className="row">
                                <AssignedResource
                                    gridData={this.techSpecAssigned}
                                    gridHeaderData={this.headerData.AssignedResourceHeader}
                                    isSubSupplierGrid={true}
                                    subSupplierGridBtn={this.assignedResourcesGridBtn}
                                    buttons={this.assignedSearchBtn}
                                    currentPage={currentPage}
                                    isARSSearch={isARSSearch}
                                    interactionMode={interactionMode}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                            </div>
                        </Panel>
                        <Panel colSize="s12 pl-0 pr-0" heading={localConstant.resourceSearch.SEARCH_GRM_OPT_PARAM} isArrow={true} onpanelClick={() => this.panelClick('SearchParams')} isopen={this.state.isPanelOpenSearchParams} >
                            <SearchGRM
                                isARSSearch={isARSSearch}
                                gridData={techSpecList}
                                buttons={this.optionalSearchGrm}
                                multiSelectOptionsList={languages}
                                certificatesMultiSelectOptionsList={certificates}
                                defaultMultiSelection={[]}
                                efaultCertificateMultiSelection={defaultCertificateMultiSelection}
                                defaultLanguageSpeakingMultiSelection={defaultLanguageSpeakingMultiSelection}
                                defaultLanguageWritingMultiSelection={defaultLanguageWritingMultiSelection}
                                defaultLanguageComprehensionMultiSelection={defaultLanguageComprehensionMultiSelection}
                                certificationMultiSelectValueChange={(value) => this.certificationMultiselect(value)}
                                languageSpeakingMultiSelectValueChange={(value) => this.languageSpeakingMultiselect(value)}
                                languageWritingMultiSelectValueChange={(value) => this.languageWritingMultiselect(value)}
                                languageComprehensionMultiSelectValueChange={(value) => this.languageComprehensionMultiselect(value)}
                                optionAttributs={this.state.optionAttributs}
                                gridHeaderData={this.headerData.searchResourceHeader}
                                gridRef={ref => { this.searchGridChild = ref; }}
                                buttons={this.optionalSearchGrm}
                                searchGRMData={optionalSearch}
                                expiryFrom={this.expiryFrom}
                                expiryTo={this.expiryTo}
                                optionalParamLabelData={searchParameters}
                                inputChangeHandler={(e) => this.optionalOnchangeHandler(e)} />
                        </Panel>
                        <CardPanel className="white lighten-4 black-text" colSize="s12 mb-2">
                        <ActionSearch
                            currentPage={currentPage}
                            isARSSearch={isARSSearch}
                            dispositionTypeList={ dispositionType }
                            actionList={arsSearchAction}
                            actionData={assignmentData}
                            inputChangeHandler={(e) => this.actionOnchangeHandler(e)}
                            buttons={this.arsAssignResourcesBtn} />
                            </CardPanel>
                    </form>
                </div>
            </Fragment>
        );
    }
}

export default ArsSearch;
