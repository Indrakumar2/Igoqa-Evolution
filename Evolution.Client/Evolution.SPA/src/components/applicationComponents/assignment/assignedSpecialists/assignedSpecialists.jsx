import React, { Component, Fragment } from 'react'; 
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData,
    isEmpty,
    formInputChangeHandler,
    bindAction, 
    isEmptyReturnDefault, 
    isEmptyOrUndefine } from '../../../../utils/commonUtils';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData, AssignedSpecialistHeader } from './headerData';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Modal from '../../../../common/baseComponents/modal';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import CascadeDropdown from '../../../../common/baseComponents/cascadeDropdown';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import TechSpecSearch from '../../../technicalSpecialistSearch';
import { modalMessageConstant,modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import { required } from '../../../../utils/validator';
import ErrorList from '../../../../common/baseComponents/errorList';
import { StringFormat } from '../../../../utils/stringUtil';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import ArsSearch from '../arsSearch';
// import { HeaderData as techSpecHeader } from '../../../../grm/components/techSpec/techSpecSearch/techSpecSearchHeader';

const localConstant = getlocalizeData();

/** Technical Specialist Popup - Temporary */
const TechnicalSpecialistPopup = (props) => {
    const taxonomy  = props.taxonomy[0] ?props.taxonomy[0]:{};
    return(
        <TechSpecSearch
            headerData={props.HeaderData} //def 957 
            gridRef={props.gridRef} 
            isfromNDTAssignment={true} //Changes for Hot Fixes on NDT
            ocCompanyCode={ props.assignmentOperatingCompanyCode }
            assignedTSList={props.techSpecList}
            category = {props.taxonomyCategory}
            subCategory = { props.subCategory }
            services = { props.services }
            />
    );
};

/** Technical Specialist Schedule Popup */
const TechnicalSpecialistSchedulePopup = (props) => {
    const { editedRow } = props;
    return (
        <Fragment>
            <CustomInput
                hasLabel={true}
                //required={true}
                divClassName='col'
                label={localConstant.assignments.CHARGE_SCHEDULE}
                type='select'
                colSize='s12 m4'
                className="browser-default"
                optionsList={props.chargeSchedules}
                labelClass="customLabel mandate"
                optionName='contractScheduleName'
                optionValue="contractScheduleName"
                name="contractScheduleName"
                id="contractScheduleId"
                disabled={props.interactionMode || !props.isEditable}
                defaultValue={isEmpty(editedRow.contractScheduleName)?"":editedRow.contractScheduleName}
                onSelectChange={props.onChangeHandler}
            />
            <CustomInput
                hasLabel={true}
                //required={true}
                divClassName='col'
                label={localConstant.assignments.PAY_SCHEDULE}
                type='select'
                colSize='s12 m4'
                className="browser-default"
                optionsList={props.paySchedules}
                labelClass="customLabel mandate"
                optionName='payScheduleName'
                optionValue="payScheduleName"
                name="technicalSpecialistPayScheduleName"
                id="technicalSpecialistPayScheduleId"
                disabled={props.interactionMode || !props.isEditable}
                defaultValue={isEmpty(editedRow.technicalSpecialistPayScheduleName)?"":editedRow.technicalSpecialistPayScheduleName}
                onSelectChange={props.onChangeHandler}
            />
            <CustomInput
                hasLabel={true}
                labelClass="customLabel"
                label={localConstant.assignments.SCHEDULE_NOTES_TO_PRINT_ON_INVOICE}
                divClassName="col"
                type='text'
                refProps='scheduleNoteToPrintOnInvoiceId'
                name="scheduleNoteToPrintOnInvoice"
                defaultValue={editedRow.scheduleNoteToPrintOnInvoice}
                colSize='s12 m4'
                inputClass="customInputs"
                maxLength={fieldLengthConstants.assignment.assignedSpecialists.SCHEDULE_NOTES_ON_INVOICE_MAXLENGTH}
                readOnly={props.interactionMode || !props.isEditable}
                // disabled={props.interactionMode || !props.isEditable}
                onValueChange={props.onChangeHandler}
            />
        </Fragment>
    );
};

/** Assigned Technical Specialist Popup */
const AssignedTechSpecPopup = (props) => {
    const { techSpecSelected } = props;
    return(
        <Fragment>
            <LabelWithValue
                className="custom-Badge col br1"
                colSize="s12 m6 mt-4"
                label={`${
                    localConstant.assignments.SPECIALIST_NAME
                    } : `}
                value={techSpecSelected.technicalSpecialistName}
            />
            <div>
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.assignments.INACTIVE}
                    isSwitchLabel={true}
                    switchName="isActive"
                    id="isActiveId"
                    colSize="s12 m2 mt-4"
                    disabled={props.interactionMode || !props.isEditable}
                    checkedStatus={techSpecSelected.isActive ? true : false}
                    onChangeToggle={props.switchOnChangeHandler}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.assignments.SUPERVISOR}
                    isSwitchLabel={true}
                    switchName="isSupervisor"
                    id="isSupervisorId"
                    colSize="s12 m2 mt-4"
                    disabled={props.interactionMode || !props.isEditable}
                    checkedStatus={techSpecSelected.isSupervisor ? true : false}
                    onChangeToggle={props.switchOnChangeHandler}
                />
            </div>
        </Fragment>
    );
};

/** Business Unit Section */
const BusinessUnit = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.BUSINESS_UNIT} colSize="s12">
            <LabelWithValue
                className="custom-Badge col"
                colSize="s6"
                label={`${
                    localConstant.assignments.BUSINESS_UNIT
                    }: `}
                value={props.businessUnit}
            />
            <a target="_blank" href={props.taxonomyUrl.description}>{props.taxonomyUrl.name}</a>
        </CardPanel>
    );
};

/** Classification Section */
const Classification = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.TAXONOMY} colSize="s12">
            <div className="row">
                <CascadeDropdown dropdownList={props.dropdown} alignmentClass="col s12"/>
            </div>
        </CardPanel>
    );
};

/** Assigned Technical Specialist Section */
const AssignedTechicalSpecialists = (props) => {
    const { technicalSpecialist,technicalSpecialistName } = props;
 
    return (
        /**
         * Changed Resource Dropdown to Grid - CR from ITK.
         */
        <Fragment>
            <span className={ 'right dashboardInfo p-relative' }>
                <div className="triangleTopLeft"></div>
                &nbsp;&nbsp;{ localConstant.validationMessage.RESOURCE_INACTIVE_MSG }
            </span>
            <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.ASSIGNED_TECHNICAL_SPECIALISTS} colSize="s12">
                <ReactGrid 
                    gridRowData={props.techSpecList} 
                    gridColData={props.HeaderData} 
                    onRef={props.gridRef} 
                    rowSelected={props.rowSelectedHandler} 
                    rowClassRules={{ allowDangerTag: true }}
                    paginationPrefixId={localConstant.paginationPrefixIds.assignmentAddTs} /> {/**D578 */}
            </CardPanel>
            {props.pageMode !== localConstant.commonConstants.VIEW && 
                <div className="right-align mt-2 add-text">
                    <a href="#confirmation_Modal" onClick={props.deleteHandler} className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={props.rowData && props.rowData.length <= 0 || props.interactionMode || !props.isEditable}>{localConstant.commonConstants.DELETE}</a>
                </div>
            }
        </Fragment>
    );
};

/** Technical Specialist Schedules Section */
const TechnicalSpecialistSchedules = (props) => {
    // TO-DO: Check interaction mode for buttons.
    return (
        <Fragment>
            <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.TECHNICAL_SPECIALIST_SCHEDULES} colSize="s12">
               <ReactGrid 
                    gridRowData={props.rowData} 
                    gridColData={props.HeaderData} 
                    onRef={props.gridRef} 
                    rowSelected={props.rowSelectedHandler}
                    paginationPrefixId={localConstant.paginationPrefixIds.assignmentTsSch} />
          
            </CardPanel>
            {props.pageMode !== localConstant.commonConstants.VIEW  && <div className="right-align mt-2 add-text">
                <a onClick={props.addHandler} className="btn-small waves-effect" disabled={props.technicalSpecialistName && props.technicalSpecialistName.toLowercase() === "select" || props.technicalSpecialistName === "" || props.interactionMode || !props.isEditable}>{localConstant.commonConstants.ADD}</a>
                <a href="#confirmation_Modal" onClick={props.deleteHandler} 
                 className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" disabled={props.rowData && props.rowData.length <= 0 || props.interactionMode || !props.isEditable}>{localConstant.commonConstants.DELETE}</a>
            </div> }    
        </Fragment>
    );
};

/** Charge Schedule Rates Section */
const ChargeScheduleRates = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.CHARGE_SCHEDULE_RATES} colSize="s12">
            <ReactGrid gridRowData={ props.rowData } gridColData={ props.HeaderData } />
        </CardPanel>
    );
};

/** Pay Schedule Rates Section */
const PayScheduleRates = (props) => {
    return (
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.PAY_SCHEDULE_RATES} colSize="s12">
            <ReactGrid gridRowData={ props.rowData } gridColData={ props.HeaderData } />
        </CardPanel>
    );
};

// TO-DO: Check interaction mode properly
class AssignedSpecialist extends Component {
    constructor(props){
        super(props);
        this.state = {
            isARSSearchvisiable:false,
            isTechSpecModalOpen : false,
            isTechSpecScheduleModalOpen : false,
            isTechSpecScheduleEditMode : false,
            isAssignedTechSpecModalOpen : false,
            selectedTechnicalSpecialist:{},
            selectedTechnicalSpecialistName:"",
            isViewAllTechnicalSpecialist:false,
            projectInvoiceFormat:"",
            isEditable:false,
            errorList:[],
            orderSequenceId:1,
        };
        this.addResourcedisabled = false;
        this.isTechSpecScheduleAdded = false;
        this.editRowData = {};
        this.selectedTechnicalSpecialist = {};
        this.updatedData = {};
        this.techSpecData={};
        this.techSpecScheduleList = [];
        const functionRefs = {}; 
        functionRefs["getInterCompanyInfo"] = this.getInterCompanyInfo;//def 957 fix
        this.headerData = HeaderData(functionRefs); 

        this.techSpecButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelTechSpecHandler,
                btnID: "cancelTechSpec",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addTechSpecHandler,
                btnID: "addTechSpec",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.techSpecScheduleButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelTechSpecSchedule,
                btnID: "cancelTechSpec",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type: 'submit',
                btnID: "addTechSpec",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.assignedTechSpecButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelAssignedTechSpec,
                btnID: "cancelAssignedTechSpec",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type: 'submit',
                btnID: "updateAssignedTechSpec",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action:(e) => this.closeErrorList(e),
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
        ];
        this.ViewAllTechSpecialist =[
            {
              name: localConstant.commonConstants.CANCEL,
              action:this.onCancelViewTechnicalSpecialist,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
        ];
        this.classificationDropdowns = [];
        this.props.callBackFuncs.onCancel =()=>{
            this.clearLocalState();
        };
          //D709
          this.props.callBackFuncs.onSave =()=>{
            this.clearLocalState();
        };
        /** Resource Dropdown to Grid - ITK CR */
        this.headerParam = {};
        this.assignedSpecHeader = AssignedSpecialistHeader(this.headerParam);
    }

    getInterCompanyInfo =()=> { 
        if(!isEmptyOrUndefine(this.props.opCompanyCode) && this.props.opCompanyCode !== this.props.selectedHomeCompany)
        { 
            return this.props.opCompanyCode;
        }
        return null;
    }

    /** Component Did Mount Life-cycle */
    componentDidMount(){  
        const tabInfoComponentCount =  this.props.tabInfo && this.props.tabInfo.componentRenderCount === 0;
        this.props.actions.FetchTaxonomyURL(localConstant.assignments.taxonomyConstants);
        // ITK D - 456
        if(this.props.assignmentInfo && this.props.isOperatorCompany){
            this.setState({ isEditable:true });
        }
        if(this.props.assignmentInfo.assignmentProjectBusinessUnit){
            let bussinessUnitId = "";
            this.props.businessUnit.forEach(iteratedValue => {
                if(iteratedValue.name === this.props.assignmentInfo.assignmentProjectBusinessUnit){
                    this.setState({
                         projectInvoiceFormat:iteratedValue.invoiceType,
                         useARSFromAdminSetup: iteratedValue.isARS
                    });
                    bussinessUnitId = iteratedValue.id;
                }
            });
            const categoryParam = {
                "projectTypeId": bussinessUnitId
            };
            tabInfoComponentCount && this.props.actions.FetchTaxonomyBusinessUnit(categoryParam);
            //D546 Start didmount
            const assignmentworkFlow=this.props.assignmentInfo.assignmentProjectWorkFlow;
          if (tabInfoComponentCount){
            if(this.props.isVisitAssignment){//S => Visit NDT , V=>Visit
                this.props.actions.FetchAssignmentVisits();
            }else{//M =>Timesheet and N=>Timsheet NDT
                this.props.actions.FetchAssignmentTimesheets();
            }
          }       
            //D546 END didmount
        }
        
        let taxonomy = this.props.taxonomy[0];
        taxonomy = { ...taxonomy };
        if(required(taxonomy.taxonomyCategory)){
            this.props.actions.ClearSubCategory();
        }
        if(!required(taxonomy.taxonomyCategory) && tabInfoComponentCount){  
            this.props.actions.ClearSubCategory();
            this.props.actions.FetchTechSpecSubCategory(taxonomy.taxonomyCategory);
        }
        if(required(taxonomy.taxonomySubCategory)){
            this.props.actions.ClearServices();
        }
        if(!required(taxonomy.taxonomyCategory) && !required(taxonomy.taxonomySubCategory) && tabInfoComponentCount){
            this.props.actions.ClearServices();
            this.props.actions.FetchTechSpecServices(taxonomy.taxonomyCategory,taxonomy.taxonomySubCategory);//def 916 fix
        } 

    }   
    checkDuplicateSchedules(techSpecScheduleList,editedData){
        let isDuplicate=false;
        const uniqueScheduleIdProp = editedData.recordStatus === 'N' ? "assignmentTechnicalSpecialistScheduleUniqueId" : "assignmentTechnicalSpecialistScheduleId";
            !isEmpty(techSpecScheduleList) && techSpecScheduleList.forEach(items=>{
                if(items.contractScheduleName===editedData.contractScheduleName && items.technicalSpecialistPayScheduleName===editedData.technicalSpecialistPayScheduleName && items[uniqueScheduleIdProp] !== editedData[uniqueScheduleIdProp] && items.recordStatus !== 'D')
                {
                    IntertekToaster(localConstant.validationMessage.TECHSPEC_DUPLICATE_SCHEDULE,'warningToast TechSpecRatesValidation');       
                    isDuplicate=true;
                }
                
            });
            return isDuplicate;
        }

    /** Search Resources Button Handler */
    searchResourcesHandler = (e) => {
        if (this.props.taxonomy.length <= 0 || required(this.props.taxonomy[0].taxonomyCategory) ||
            required(this.props.taxonomy[0].taxonomySubCategory) ||
            required(this.props.taxonomy[0].taxonomyService)) {
            IntertekToaster(localConstant.validationMessage.TAXONOMY_REQ_FOR_TECHSPEC_SEARCH_VAL, "warningToast taxonomyReqValForSearch");
            return false;
        }
        const obj=[];
        this.props.assignedTechSpec && this.props.assignedTechSpec.forEach(techSpec =>{
            const techSpecData = {};
              techSpecData["epin"]=techSpec.epin; 
              obj.push( techSpecData);         
        });

        if(!this.state.useARSFromAdminSetup){
            const isValid =this.props.callBackFuncs.NDTAssignmentSaveValidation();
            if(isValid){
            this.setState({ isTechSpecModalOpen:true });
            }
        }
        else{
            this.props.actions.AssignResourcesButtonClick(true);
            /** validation for ARS search */
            const isValid = this.props.callBackFuncs.ARSAssignmentSaveValidation(true);
            if(isValid){
                this.props.actions.LoadARSSearchData(obj);
                if(this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
                    this.props.actions.GetAssigedResourcesForARS(this.props.assignmentInfo);
                }
              //  this.props.actions.FetchPreAssignmentIds(); //Assignment Performance related changes om 24-10-19
                this.props.actions.ARSSearchPanelStatus(true);
            }
        }
        
    };

    /** Add selected Technical Specialist from popup */
    addTechSpecHandler = (e) => {
        e.preventDefault();
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords && selectedRecords.length <= 0) {
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST_VALIDATION, 'warningToast selectTechSpecVal');
            return false;
        }
        let selectedTechSpec = [];
        
            selectedTechSpec = this.props.assignedTechSpec;
            selectedRecords.forEach(iteratedValue => {
                let alreadySelected = false;
                // let deletedRecord = false;
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
                else {
                    const techSpec = {};
                    techSpec.technicalSpecialistName = iteratedValue.lastName ? `${ iteratedValue.lastName } ${ iteratedValue.firstName }` : iteratedValue.firstName;
                    techSpec.epin = iteratedValue.epin;
                    techSpec.isActive = true;
                    techSpec.isSupervisor = false;
                    techSpec.assignmentId = this.props.assignmentInfo.assignmentId;
                    // techSpec.assignmentTechnicalSplId = Math.floor(Math.random() * 99) -100;
                    techSpec.assignmentTechnicalSplId = null;
                    techSpec.assignmentTechnicalSplUniqueId = Math.floor(Math.random() * 9999) -10000;
                    techSpec.assignmentTechnicalSpecialistSchedules = [];
                    techSpec.modifiedBy = this.props.loginUser;
                    techSpec.recordStatus = "N";
                    selectedTechSpec.push(techSpec);
                    this.props.actions.AssignTechnicalSpecialist(selectedTechSpec);
                }
            });
            
            let subSupplierAssigned = false;
            if(this.props.subSupplierTechSpec.length > 0){
                this.props.subSupplierTechSpec.forEach(iteratedValue => {
                    if(iteratedValue.assignmentSubSupplierTS && iteratedValue.assignmentSubSupplierTS.length > 0){
                        subSupplierAssigned = true;
                    }
                });
            }
            if(subSupplierAssigned){
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.TECH_SPEC_ADD_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: localConstant.commonConstants.OK,
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                this.props.actions.DisplayModal(confirmationObject);
            }

            const assignmentSubSupplier = this.props.subSupplierTechSpec;
            const filteredAssignmentSubSupplier = assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
            const mainSupplierData = filteredAssignmentSubSupplier.length > 0 && filteredAssignmentSubSupplier[0];
            if(!isEmpty(mainSupplierData)){
                const mainSupplierTS = isEmptyReturnDefault(mainSupplierData.assignmentSubSupplierTS);
                const mainSupplierResources = [];
                selectedRecords.forEach( x => {
                    const tsAlreadyExisit = mainSupplierTS.filter(y => y.epin === x.epin && y.recordStatus !== 'D' && y.isAssignedToThisSubSupplier === false);
                    if(isEmpty(tsAlreadyExisit)){
                        mainSupplierResources.push({
                            assignmentSubSupplierId: mainSupplierData.assignmentSubSupplierId,
                            epin: x.epin,
                            subSupplierId: mainSupplierData.subSupplierId,
                            isAssignedToThisSubSupplier: mainSupplierData.isSubSupplierFirstVisit === true ? true : false
                        });
                    }
                });
                if(mainSupplierResources.length > 0){
                    assignmentSubSupplier.forEach((value,index) => {
                        if(value.recordStatus !== 'D')
                            this.props.actions.SetTechSpecToAssignmentSubSupplier(mainSupplierResources,index);
                    });
                }
            }

        this.cancelTechSpecHandler(e);
    };

    /** Cancel Technical Specialist search and close the popup */
    cancelTechSpecHandler = (e) => {
        e.preventDefault();
        this.props.actions.TechSpecClearSearch();
        // this.props.actions.ClearSubCategory();
        this.setState({ isTechSpecModalOpen:false });
    };

    /** Technical Specialist Change Handler */
    technicalSpecialistChangeHandler = (e) => {
        const techSpec = e.target.value;
        if(isEmpty(techSpec)){
            this.setState((state) => {
                return {
                    selectedTechnicalSpecialist:{},
                    selectedTechnicalSpecialistName:techSpec
                };
            });
            this.selectedTechnicalSpecialist = {};
        }
        else{
            this.props.assignedTechSpec && this.props.assignedTechSpec.forEach(iteratedValue => {
                if(iteratedValue.technicalSpecialistName === techSpec){
                    this.selectedTechnicalSpecialist = iteratedValue;
                    this.setState({
                        selectedTechnicalSpecialist:iteratedValue,
                        selectedTechnicalSpecialistName:iteratedValue.technicalSpecialistName
                    },()=>{
                        this.props.actions.FetchPaySchedule(this.state.selectedTechnicalSpecialist.epin);
                        if(this.techSpecScheduleList.length > 0){
                       const chargeAndPayRatesObj=    {
                            ...this.techSpecScheduleList[0],
                            technicalSpecialistEpin:this.state.selectedTechnicalSpecialist.epin, 
                           };
                            this.chargeAndPayRatesAction(chargeAndPayRatesObj);
                        }
                    });
                }
            });
        }
    };

 /** Resources Schedules Validation Handler */
 resourcesSchedulesValidation = (data) => {
    if(isEmpty(data.contractScheduleName)){
        IntertekToaster(localConstant.validationMessage.ASSIGNMENT_SPECIALISTS_CHARGE_SCHEDULE,"warningToast ChargeScheduleVal");
        return false;
    }
    else if(isEmpty(data.technicalSpecialistPayScheduleName)){
        IntertekToaster(localConstant.validationMessage.ASSIGNMENT_SPECIALISTS_PAY_SCHEDULE,"warningToast PayScheduleVal");
        return false;
    }
    else
        return true;
} 

    /** Assigned Technical Specialist Submit Handler */
    assignedTechSpecSubmitHandler = (e) => {
        e.preventDefault();
        if(isEmpty(this.updatedData)){
            IntertekToaster(localConstant.validationMessage.NO_CHANGES_FOUND, "warningToast noChangesHappened");
            return false;
        }
        let techSpec = this.state.selectedTechnicalSpecialist;
        techSpec = Object.assign({},this.state.selectedTechnicalSpecialist,this.updatedData);
        if(techSpec && techSpec.recordStatus !== "N"){
            techSpec.recordStatus = "M";
        }
        this.setState({
            selectedTechnicalSpecialist : techSpec
        });
        this.selectedTechnicalSpecialist = techSpec;
        if(this.resourcesSchedulesValidation(techSpec)){
            this.props.actions.UpdateAssignedTechSpec(techSpec);
            this.cancelAssignedTechSpec(e);
        }
    };

    /** Assigned Technical Specialist Cancel handler */
    cancelAssignedTechSpec = (e) => {
        e.preventDefault();
        this.setState({ isAssignedTechSpecModalOpen:false });
        this.updatedData = {};
        this.editRowData = {};
    };

    /** Technical Specialist Schedule Change Handler */
    scheduleChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(result.name === 'technicalSpecialistPayScheduleName'){
            const paySchedule = this.props.paySchedules.filter(x=>x.payScheduleName === result.value);
            this.updatedData['scheduleNoteToPrintOnInvoice'] = (!isEmpty(paySchedule) && paySchedule.length > 0) ? !required(paySchedule[0].payScheduleNote) ? paySchedule[0].payScheduleNote.substring(0, 29) : "" : ""; // Sanity Defect 46
        }
        this.editRowData = Object.assign({},this.editRowData,this.updatedData);
        this.setState({
            updatedScheduleRow: this.editRowData
        });
    };
   
    /** Technical Specialist Schedule Add button handler */
    scheduleAddHandler = () => {
        if(isEmpty(this.state.selectedTechnicalSpecialist)){
            IntertekToaster(localConstant.validationMessage.SELECT_TECHNICAL_SPECIALIST_VALIDATION,'warningToast techSpectVal');
            return false;
        }
        if(isEmpty(this.props.contractSchedules)){
            IntertekToaster(localConstant.validationMessage.SELECT_CONTRACT_SCHEDULE,'warningToast techSpectScheduleVal');
            return false;
        }
        this.setState({
            isTechSpecScheduleModalOpen:true,
            isTechSpecScheduleEditMode:false
        });
    };

    /** Technical Specialist Schedule Edit button handler */
    scheduleEditHandler = (data) => {
        this.setState({
            isTechSpecScheduleModalOpen:true,
            isTechSpecScheduleEditMode:true
        });
        this.editRowData = data;
    };

    /** Technical Specialist Schedule Delete button handler */
    scheduleDeleteHandler = (e) => {
        e.preventDefault();
        const selectedRecords = this.scheduleChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.TECH_SPEC_SCHEDULE_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteTechSpecSchedule,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast supplierContactDeleteToaster');
        }
    };

    /** Add Technical Specialist Schedule */
    submitTechSpecSchedule = (e) => {
        e.preventDefault();
        const techSpec = this.state.selectedTechnicalSpecialist;
        const updatedEditedData = Object.assign({},this.editRowData,this.updatedData);
        const isValidSchedule = this.resourcesSchedulesValidation(updatedEditedData);
        if(!isValidSchedule)
            return false;
        const isDuplicateShcedule = this.state.isTechSpecScheduleEditMode?this.checkDuplicateSchedules(this.techSpecScheduleList,updatedEditedData):this.checkDuplicateSchedules(this.techSpecScheduleList,this.updatedData);
        if(this.state.isTechSpecScheduleEditMode){         
            if(!isDuplicateShcedule){
            if(this.editRowData.recordStatus !== "N"){
                this.editRowData.recordStatus = "M";
            }
            this.editRowData.technicalSpecialistEpin=techSpec.epin;
            this.editRowData = Object.assign({},this.editRowData,this.updatedData);      
            //Code to set the contractScheduleId and PayScheduleId
            const { contractSchedules, paySchedules } = this.props;
            for (let csIndex = 0, csLength = contractSchedules.length; csIndex < csLength; ++csIndex) {
                if (contractSchedules[csIndex].contractScheduleName === this.editRowData.contractScheduleName) {
                    this.editRowData.contractScheduleId = parseInt(contractSchedules[csIndex].contractScheduleId);
                    break;
                }
            }
            for (let psIndex = 0, psLength = paySchedules.length; psIndex < psLength; ++psIndex) {
                if (paySchedules[psIndex].payScheduleName === this.editRowData.technicalSpecialistPayScheduleName) {
                    this.editRowData.technicalSpecialistPayScheduleId = paySchedules[psIndex].id;
                    break;
                }
            }
            this.props.actions.FetchPayRates(this.editRowData).then(resPayRates =>{
                if (resPayRates.code === "1" && Array.isArray(resPayRates.result) && resPayRates.result.length > 0 ) {
                    this.isTechSpecScheduleAdded = true;
                    this.props.actions.UpdateTechSpecSchedules(this.editRowData);
                    /** Charge rate and Pay rate fetch handler
                     * param 1: editedRowData
                     * param 2: isChargerate fetched - boolean
                     * param 3: isPayrate fetched - boolean
                     */
                    this.chargeAndPayRatesAction(this.editRowData,false,true);
                    this.cancelTechSpecSchedule(e);
                }
                else{
                    IntertekToaster(localConstant.validationMessage.TECHSPEC_SHOULD_HAVE_PAYRATE,'warningToast PayScheduleRatesValidation');
                    return false;
                }
                
            }).catch(err=>{
                // IntertekToaster(localConstant.validationMessage.TECHSPEC_SHOULD_HAVE_PAYRATE,'warningToast PayScheduleRatesValidation');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                return false;
            });
        }
        }
        else{
            let contractScheduleId,payScheduleId; 
            if(!isDuplicateShcedule)
            {
                this.props.contractSchedules.forEach(iteratedValue=>{
                    if(iteratedValue.contractScheduleName === this.updatedData.contractScheduleName){
                        contractScheduleId = iteratedValue.contractScheduleId;
                    }
                });
                this.props.paySchedules.forEach(iteratedValue=>{
                    if(iteratedValue.payScheduleName === this.updatedData.technicalSpecialistPayScheduleName){
                        payScheduleId = iteratedValue.id;
                    }
                });
                this.updatedData.contractScheduleId = contractScheduleId;
                this.updatedData.technicalSpecialistPayScheduleId = payScheduleId;
                this.updatedData.assignmentTechnicalSpecialistScheduleId = null;
                // this.updatedData.assignmentTechnicalSpecialistScheduleId = Math.floor(Math.random() * 99) -100;
                this.updatedData.assignmentTechnicalSpecialistScheduleUniqueId = Math.floor(Math.random() * 9999) -10000;
                this.updatedData.assignmentTechnicalSpecilaistId = techSpec.assignmentTechnicalSplId;
                this.updatedData.assignmentTechnicalSplUniqueId = techSpec.assignmentTechnicalSplUniqueId;
                this.updatedData.technicalSpecialistEpin = techSpec.epin;
                this.updatedData["modifiedBy"] = this.props.loginUser;
                this.updatedData["isSelected"]=true;
                this.updatedData.recordStatus = "N";
                 
                this.props.actions.FetchPayRates(this.updatedData).then(resPayRates =>{
                    if (resPayRates.code === "1" && Array.isArray(resPayRates.result) && resPayRates.result.length > 0 ) {
                       /*Lateef to check if commenting below line is a problem or not */
                        //this.isTechSpecScheduleAdded = true;
                       this.props.actions.AddTechSpecSchedules(this.updatedData);
                        /** Charge rate and Pay rate fetch handler
                         * param 1: editedRowData
                         * param 2: isChargerate fetched - boolean
                         * param 3: isPayrate fetched - boolean
                         */
                        this.chargeAndPayRatesAction(this.updatedData,false,true);
                        this.cancelTechSpecSchedule(e);
                    }
                    else{
                        IntertekToaster(localConstant.validationMessage.TECHSPEC_SHOULD_HAVE_PAYRATE,'warningToast PayScheduleRatesValidation');
                        return false;
                    }
                }).catch(err=>{
                    // IntertekToaster(localConstant.validationMessage.TECHSPEC_SHOULD_HAVE_PAYRATE,'warningToast PayScheduleRatesValidation');
                    IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                        return false;
                });
            }
        }
    };
    
    clearLocalState = () => {
        this.setState({
            selectedTechnicalSpecialist:{},
            selectedTechnicalSpecialistName:"",
        });
        this.editRowData = {};
        this.selectedTechnicalSpecialist = {};
        this.updatedData = {};
        this.techSpecScheduleList = [];
    };
    /** Delete Technical Specialist Schedule */
    deleteTechSpecSchedule = (e) => {
        e.preventDefault();
        const selectedRecords = this.scheduleChild.getSelectedRows();
        this.scheduleChild.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteTechSpecSchedules(selectedRecords);
        this.props.actions.HideModal();
    };

    /** Confirmation Reject Handler */
    confirmationRejectHandler = () =>{
        this.props.actions.HideModal();
    }

    /** Cancel Technical Specialist Schedule */
    cancelTechSpecSchedule = (e) => {
        e.preventDefault();
        this.setState({
            isTechSpecScheduleModalOpen:false,
            isTechSpecScheduleEditMode:false,
        });
        this.updatedData={};
        this.editRowData={};
    };

    /** Classification Change Handler */
    classificationChangeHandler = (e) => {
        this.updatedData[e.target.name] = e.target.value;
        if(this.props.taxonomy.length <= 0){
            this.updatedData.assignmentTaxonomyId = null;
            this.updatedData.assignmentTaxonomyUniqueId = Math.floor(Math.random() * 99);
            this.updatedData.assignmentId = this.props.assignmentInfo.assignmentId;
            this.updatedData.recordStatus = "N";
            this.updatedData.modifiedBy = this.props.loginUser;
        }
        else{
            this.updatedData = Object.assign({},this.props.taxonomy[0],this.updatedData);
            if(this.props.taxonomy[0].recordStatus !== "N"){
                this.updatedData.recordStatus = "M";
            }
        }
        if(e.target.name === "taxonomyCategoryId"){
            this.props.actions.ClearSubCategory();
            this.updatedData['taxonomyCategory'] = !isEmpty(e.target.value) ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].innerText  : "";
            this.updatedData.taxonomySubCategory = null;
            this.updatedData.taxonomySubCategoryId = null;
            this.updatedData.taxonomyService = null;
            this.updatedData.taxonomyServiceId = null;
            this.updatedData.taxonomyCategory && this.props.actions.FetchTechSpecSubCategory(this.updatedData.taxonomyCategory);
        }
        if(e.target.name === "taxonomySubCategoryId"){
            this.props.actions.ClearServices();
            this.updatedData['taxonomySubCategory'] = !isEmpty(e.target.value) ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].innerText  : "";
            this.updatedData.taxonomyService = null;
            this.updatedData.taxonomyServiceId = null;
            this.updatedData.taxonomyCategory && 
                this.updatedData.taxonomySubCategory &&
                    this.props.actions.FetchTechSpecServices(this.updatedData.taxonomyCategory,this.updatedData.taxonomySubCategory);//def 916 fix
        }
        else{
            this.updatedData['taxonomyService'] = !isEmpty(e.target.value) ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].innerText  : "";
        }
        this.props.actions.UpdateTaxomony(this.updatedData);
        this.updatedData = {};
    };

    /** Assigned Technical Specialist Edit Handler */
    editTechSpecHandler = () => {
        this.setState({
            isAssignedTechSpecModalOpen:true
        });
    };

    /** Assigned Technical Specialist Delete handler */
    deleteTechSpecHandler = (e) => {
        /** Modified for Assignment specialist Grid Process - Inline editing ITK CR */
        e.preventDefault();
        const selectedRecords = this.assignedSpecChild.getSelectedRows();
        let subSupplierAssigned = false;
        if(selectedRecords && selectedRecords.length > 0){
            if(this.props.subSupplierTechSpec.length > 0){
                selectedRecords.forEach(techSpec => {
                    this.props.subSupplierTechSpec.forEach(iteratedValue => {
                        if(iteratedValue.assignmentSubSupplierTS && iteratedValue.assignmentSubSupplierTS.length > 0){
                           iteratedValue.assignmentSubSupplierTS.forEach(row => {
                               if (this.props.techSpecList.length > 0) {
                                   this.props.techSpecList.forEach(x => {
                                       if ((x.resourceSearchTechspecInfos.filter(y => y.epin === techSpec.epin && y.isSelected === true).length > 0) ||
                                       (row.isDeleted === false && row.epin === techSpec.epin)) {
                                           subSupplierAssigned = true;
                                       }
                                   });
                               }
                               else if (row.isDeleted === false && row.epin === techSpec.epin) {
                                   subSupplierAssigned = true;
                               }
                           });
                        }
                    });
                });
            }
            if(subSupplierAssigned && this.props.isArsAssignment){
                const confirmationObject = {
                    title: modalTitleConstant.CONFIRMATION,
                    message: modalMessageConstant.TECH_SPEC_ASSOCIATED_DELETE_MESSAGE,
                    type: "confirm",
                    modalClassName: "warningToast",
                    buttons: [
                        {
                            buttonName: localConstant.commonConstants.OK,
                            onClickHandler: this.confirmationRejectHandler,
                            className: "modal-close m-1 btn-small"
                        }
                    ]
                };
                this.props.actions.DisplayModal(confirmationObject);
            }
            else{
                //D546 Start 
                //Scenario :
                // Assignment SPEC delete - any TS is exists with the Visits 
                let showDeletePopup = false;
                const VisitOrTimesheetFilterData = this.props.isVisitAssignment ? isEmptyReturnDefault(this.props.visitData) : isEmptyReturnDefault(this.props.timesheetData);

                VisitOrTimesheetFilterData && VisitOrTimesheetFilterData.forEach(iteratedValue => {
                    if (iteratedValue.techSpecialists && iteratedValue.techSpecialists.length > 0) {
                        for (let index = 0; index < iteratedValue.techSpecialists.length; index++) {
                            //const eachTS = iteratedValue.techSpecialists[index];
                            // if (!isEmpty(selectedRecords)) {
                                for (let i = 0; i< selectedRecords.length; i++) {
                                if (iteratedValue.techSpecialists[index].pin === selectedRecords[i].epin) {
                                    showDeletePopup = true;//it Means TS is assigned to one of the visit(which is greater than Awaiting)
                                    break;
                                }
                            }
                            // }
                        }
                    }
                });
                if (showDeletePopup) {// TS Assigned to ANY of the Visit for that Assignemnt
                    const confirmationObject = {
                        title: modalTitleConstant.ALERT_WARNING,
                        message: modalMessageConstant.TECH_SPEC_ASSOCIATED_DELETE_MESSAGE_VISITORTIMESHEET,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: localConstant.commonConstants.OK,
                                onClickHandler: this.confirmationRejectHandler,
                                className: "modal-close m-1 btn-small"
                            }
                        ]
                    };
                    this.props.actions.DisplayModal(confirmationObject);
                }
                //D546 End
                else {// if  TS is not assigned to any Visit / Timesheet
                    const confirmationObject = {
                        title: modalTitleConstant.CONFIRMATION,
                        message: modalMessageConstant.TECH_SPEC_DELETE_MESSAGE,
                        type: "confirm",
                        modalClassName: "warningToast",
                        buttons: [
                            {
                                buttonName: localConstant.commonConstants.YES,
                                onClickHandler: this.deleteAssignedTechSpec,
                                className: "modal-close m-1 btn-small"
                            },
                            {
                                buttonName: localConstant.commonConstants.NO,
                                onClickHandler: this.confirmationRejectHandler,
                                className: "modal-close m-1 btn-small"
                            }
                        ]
                    };
                    this.props.actions.DisplayModal(confirmationObject);
                }
            }
        }
        else{
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast supplierContactDeleteToaster');
        }
    };    

    /** Delete Assigned Technical Specialist */
    deleteAssignedTechSpec = () => {
        /** Modified Assigned Specialist Delete code based for Grid process - Inline editing ITK CR */
        const selectedRecords = this.assignedSpecChild.getSelectedRows();
        if(selectedRecords && selectedRecords.length > 0){
            if(!this.props.isArsAssignment && this.props.isVisitAssignment)
                // Resource Delete action for NDT Visit flow - this.props.actions.DeleteNDTAssignedTechSpec(selectedRecords)
                this.props.actions.DeleteNDTAssignedTechSpec(selectedRecords);
            else
                this.props.actions.DeleteAssignedTechSpec(selectedRecords);
        }
        this.props.actions.HideModal();
    };

    /** On row select of schedule populate the charge rate and pay rate */
    scheduleRowSelectHandler = (e) => {
        if(e.node.selected){
            const obj = this.props.assignedTechSpec && this.props.assignedTechSpec.find(techSpec =>{
                const assignmentTechSpecProperty = techSpec.recordStatus === 'N' ? 'assignmentTechnicalSplUniqueId' : 'assignmentTechnicalSplId';
                const scheduleTechSpecProperty = techSpec.recordStatus === 'N' ? 'assignmentTechnicalSplUniqueId' : 'assignmentTechnicalSpecilaistId';
                const check=  techSpec[assignmentTechSpecProperty] === e.data[scheduleTechSpecProperty];
                return check;
            });
            const selectedSchedule = e.data, epin = obj && obj.epin;
            selectedSchedule.technicalSpecialistEpin = epin;
            !this.isTechSpecScheduleAdded && this.chargeAndPayRatesAction(selectedSchedule);
            this.isTechSpecScheduleAdded = false;
        }
        else{
            if(this.techSpecScheduleList.length > 0){
                this.props.actions.ClearChargeAndPayRates();
            }
            this.isTechSpecScheduleAdded = false;
        }
    };

    /** Calls the action to populate charge rate and pay rate for selected schedule */
    chargeAndPayRatesAction = (selectedSchedule,isChargeRateFetched,isPayRateFetched) => {
        !isChargeRateFetched && this.props.actions.FetchChargeRates(selectedSchedule);
        !isPayRateFetched && this.props.actions.FetchPayRates(selectedSchedule);
    };    
    isTaxonomyDisabled(){
        if (this.props.interactionMode) {
            return true;
        }
        if(!this.props.isInterCompanyAssignment) {
            return false;
        }
        if(this.props.isContractHolderCompany){
            return false;
        }
        return true;
    }

    /** Returns validation message for ARS button click */
    arsValidationMessage = (data) => {
        const assignmentNumber = this.props.assignmentInfo.assignmentNumber;
        const arsTaskId = this.props.assignmentInfo.resourceSearchId; 
        if(data && data.arsTaskType){
            if(data.arsTaskType === "OM Verify and Validate"){
                this.addResourcedisabled = true;
                return StringFormat(localConstant.validationMessage.OM_VERIFY_AND_VALIDATE_MSG,assignmentNumber,arsTaskId);
            }
            else if(data.arsTaskType === "OM Validated"){
                this.addResourcedisabled = true;
                return StringFormat(localConstant.validationMessage.OM_VALIDATED_MSG,assignmentNumber,arsTaskId);
            }
            else if(data.arsTaskType === "PLO to RC"){
                this.addResourcedisabled = true;
                return StringFormat(localConstant.validationMessage.PLO_TO_RC_MSG,assignmentNumber,arsTaskId);
            }
            else if(data.arsTaskType === "PLO - No Match in GRM"){
                this.addResourcedisabled = true;
                return StringFormat(localConstant.validationMessage.PLO_BACK_TO_PC_MSG,assignmentNumber,arsTaskId);
            }
            else if(data.arsTaskType === "PLO - Search and Save Resources"){
                this.addResourcedisabled = true;
                return StringFormat(localConstant.validationMessage.PLO_BACK_TO_PC_MSG,assignmentNumber,arsTaskId);
            }
            else{
                this.addResourcedisabled = false;
                return null;
            }
        }
    };
    viewAllTechnicalSpecialist = (e) => {
        e.preventDefault();
        this.setState({ isViewAllTechnicalSpecialist:true });
    };
    onCancelViewTechnicalSpecialist=(e)=>{
        e.preventDefault();
        this.setState({ isViewAllTechnicalSpecialist:false });
    }

    /** Assignment Specialist Switch change handler - inline editing ITK CR */
    techSpecSwitchChangeHandler = (e,data) => {
        e.preventDefault();
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.name == "isActive" ? !result.value : result.value;
        const editedData = Object.assign({},data,this.updatedData);
        if(editedData && editedData.recordStatus !== "N"){
            editedData.recordStatus = "M";
        }
        this.props.actions.UpdateAssignedTechSpec(editedData);
        this.updatedData={};
    };

    /** Assigned Specialist Row Select Handler - inline editing ITK CR */
    techSpecRowSelectHandler = (e) => {
        const selectedRecords = this.assignedSpecChild.getSelectedRows();
        if(selectedRecords && selectedRecords.length > 0){
            const techSpec = selectedRecords[0];
            this.props.assignedTechSpec && this.props.assignedTechSpec.forEach(iteratedValue => {
                if(iteratedValue.epin === techSpec.epin){
                    this.selectedTechnicalSpecialist = iteratedValue;
                    this.setState({
                        selectedTechnicalSpecialist:iteratedValue,
                        selectedTechnicalSpecialistName:iteratedValue.technicalSpecialistName
                    },()=>{
                        this.props.actions.FetchPaySchedule(this.state.selectedTechnicalSpecialist.epin);
                    });
                }
            });
        }
        else{
            this.setState((state) => {
                return {
                    selectedTechnicalSpecialist:{},
                    selectedTechnicalSpecialistName:""
                };
            });
            this.selectedTechnicalSpecialist = {};
            this.techSpecScheduleList = [];
            this.props.actions.ClearChargeAndPayRates();
        }
    };

    render() {
        const { assignmentInfo,contractSchedules,interactionMode,chargeRates,payRates,assignedTechSpec,taxonomy,paySchedules,techSpecCategory,techSpecSubCategory,techSpecServices,
            taxonomyCategory,ndtSubCategory,ndtServices,arsSearchData,isARSSearch,taxonomyConstantURL } = this.props;
        const assignedSpecialist = [];
        this.techSpecScheduleList = [];       
        assignedTechSpec.forEach(iteratedValue => {
            let checkProperty = "assignmentTechnicalSplId";
            if (iteratedValue.recordStatus === 'N') {
                checkProperty = "assignmentTechnicalSplUniqueId";
            }
            if(iteratedValue[checkProperty] == this.selectedTechnicalSpecialist[checkProperty]
                 && iteratedValue.recordStatus !== 'D'){
                this.techSpecScheduleList = iteratedValue.assignmentTechnicalSpecialistSchedules.filter(row => row.recordStatus !== "D");
            }
        });
        if(this.techSpecScheduleList <= 0 && (chargeRates.length > 0 || payRates.length > 0)){
            this.props.actions.ClearChargeAndPayRates();
        }
        const scheduleLists=this.techSpecScheduleList;

        // D-709
        if(scheduleLists.length > 0){
                scheduleLists[0].isSelected = true;
        }
        const arsTask = this.props.assignmentInfo.arsTaskType;
        const isTaxonomyDisable =this.isTaxonomyDisabled();

        const arsSearchResourceValidation = this.arsValidationMessage(assignmentInfo);

        this.classificationDropdowns = [
            {
                hasLabel:true,
                required:true,
                divClassName:'col',
                label:localConstant.assignments.CATEGORY,
                type:'select',
                colSize:'s12 m4',
                className:"browser-default",
                optionsList:taxonomyCategory,
                labelClass:"customLabel mandate",
                optionName:'category',
                optionValue:"categoryId",
                name:"taxonomyCategoryId",
                id:"techSpecCategoryId",
                disabled:isTaxonomyDisable,
                defaultValue:taxonomy.length > 0 && taxonomy[0].taxonomyCategoryId,
                onSelectChange:this.classificationChangeHandler
            },
            {
                hasLabel:true,
                required:true,
                divClassName:'col',
                label:localConstant.assignments.SUB_CATEGORY,
                type:'select',
                colSize:'s12 m4',
                className:"browser-default",
                optionsList:techSpecSubCategory,
                labelClass:"customLabel mandate",
                optionName:'taxonomySubCategoryName',
                optionValue:"id",
                name:"taxonomySubCategoryId",
                id:"techSpecSubCategoryId",
                disabled:isTaxonomyDisable,
                defaultValue:taxonomy.length > 0 && taxonomy[0].taxonomySubCategoryId,
                onSelectChange:this.classificationChangeHandler
            },
            {
                hasLabel:true,
                required:true,
                divClassName:'col',
                label:localConstant.assignments.SERVICE,
                type:'select',
                colSize:'s12 m4',
                className:"browser-default",
                optionsList:techSpecServices,
                labelClass:"customLabel mandate",
                optionName:'taxonomyServiceName',
                optionValue:"id",
                name:"taxonomyServiceId",
                id:"techSpecServicesId",
                disabled:isTaxonomyDisable,
                defaultValue:taxonomy.length > 0 && taxonomy[0].taxonomyServiceId,
                onSelectChange:this.classificationChangeHandler
            }
        ];

        /** Processing Assigned Specialist header param - inline editing ITK CR */
        this.headerParam = {
            "disabled" : interactionMode || !this.state.isEditable,
        };
        this.assignedSpecHeader = AssignedSpecialistHeader(this.headerParam);

        bindAction(this.headerData.technicalSpecialistSchedules, "EditColumn", this.scheduleEditHandler);
        bindAction(this.assignedSpecHeader, "isSupervisor", (e,data) => this.techSpecSwitchChangeHandler(e,data));
        bindAction(this.assignedSpecHeader, "isActive", (e,data) => this.techSpecSwitchChangeHandler(e,data));

        return (
            <Fragment>
                <CustomModal />
                {this.state.errorList.length > 0 ?
                    <Modal title={localConstant.commonConstants.CHECK_MANDATORY_FIELD}
                        titleClassName="chargeTypeOption"
                        modalContentClass="extranetModalContent"
                        modalClass="ApprovelModal"
                        modalId="errorListPopup"
                        formId="errorListForm"
                        buttons={this.errorListButton}
                        isShowModal={true}>
                        <ErrorList errors={this.state.errorList} />
                    </Modal> : null
                }
                {this.state.isTechSpecModalOpen ?
                    <Modal title={localConstant.assignments.SEARCH_RESOURCES}
                     modalId="searchResourcePopup" 
                     formId="searchResourceForm"                    
                     modalClass="searchTechSpecModal" 
                     buttons={this.techSpecButtons} 
                     isShowModal={this.state.isTechSpecModalOpen}>
                        <TechnicalSpecialistPopup 
                            HeaderData={this.headerData.techSpecSearch }
                            taxonomy={taxonomy}
                            gridRef={ref => { this.child = ref; }}
                            interactionMode = {interactionMode}
                            assignmentOperatingCompanyCode={assignmentInfo && assignmentInfo.assignmentOperatingCompanyCode}//Changes for Hot Fixes on NDT
                            techSpecList={assignedSpecialist}
                            taxonomyCategory = { taxonomyCategory }
                            subCategory = { ndtSubCategory }
                            services = { ndtServices }
                            />
                    </Modal>:null
                }
                {this.state.isTechSpecScheduleModalOpen ?
                    <Modal title={localConstant.assignments.TECHNICAL_SPECIALIST_SCHEDULES} modalId="techSpecSchedulePopup" formId="techSpecScheduleForm" modalClass="popup-position" onSubmit={(e)=>this.submitTechSpecSchedule(e)} buttons={this.techSpecScheduleButtons} isShowModal={this.state.isTechSpecScheduleModalOpen}>
                        <TechnicalSpecialistSchedulePopup 
                            chargeSchedules={Array.isArray(contractSchedules)?contractSchedules.filter(eachSch =>eachSch.recordStatus !== 'D'):[]}
                            paySchedules = {paySchedules}
                            editedRow = {this.editRowData}
                            onChangeHandler = {this.scheduleChangeHandler}
                            interactionMode = {interactionMode}
                            isEditable = {this.state.isEditable}/>
                    </Modal>:null
                }
                {this.state.isAssignedTechSpecModalOpen ?
                    <Modal title={localConstant.assignments.ASSIGNED_TECHNICAL_SPECIALISTS} modalId="assignedTechSpecPopup" formId="assignedTechSpecForm" modalClass="popup-position" onSubmit={(e)=>this.assignedTechSpecSubmitHandler(e)} buttons={this.assignedTechSpecButtons} isShowModal={this.state.isAssignedTechSpecModalOpen}>
                        <AssignedTechSpecPopup 
                            switchOnChangeHandler={this.scheduleChangeHandler}
                            techSpecSelected = {this.state.selectedTechnicalSpecialist}
                            interactionMode = {interactionMode}
                            isEditable = {this.state.isEditable}/>
                    </Modal> : null
                }
                 {this.state.isViewAllTechnicalSpecialist ?
                    <Modal title={localConstant.assignments.VIEW_ALL_TECHNICIAL_SPECIALIST} modalId="assignedTechSpecPopup" formId="assignedTechSpecForm" modalClass="popup-position" onSubmit={(e)=>this.assignedTechSpecSubmitHandler(e)} buttons={this.ViewAllTechSpecialist} isShowModal={this.state.isViewAllTechnicalSpecialist}>
                        <ReactGrid gridColData={this.headerData.viewTechnicalSpecialist} gridRowData={assignedSpecialist}/>
                    </Modal> : null
                }
                {/* <div className={!this.props.isARSSearch ? "genralDetailContainer customCard" : 'hide'}> */}
                    <div className="genralDetailContainer customCard">
                    <BusinessUnit 
                        businessUnit={assignmentInfo.assignmentProjectBusinessUnit}
                        taxonomyUrl={taxonomyConstantURL}/>
                    <Classification 
                        dropdown={this.classificationDropdowns}
                    />
                    {this.props.pageMode !== localConstant.commonConstants.VIEW && <div className="right-align mt-2 add-text">
                        <span className="dashboardInfo">{arsSearchResourceValidation}</span>
                        <a onClick={(e) => this.searchResourcesHandler(e)} className="btn-small ml-2 waves-effect" disabled={assignmentInfo.isAssignmentCompleted || interactionMode || !this.state.isEditable || (arsTask !== null ? this.addResourcedisabled : false) || !required(arsSearchResourceValidation)}>{localConstant.assignments.ADD_RESOURCES}</a>
                    </div>}
                    <AssignedTechicalSpecialists 
                        technicalSpecialist={this.state.selectedTechnicalSpecialist}
                        techSpecList={assignedTechSpec && assignedTechSpec.filter(x => x.recordStatus !== 'D')}
                        technicalSpecialistName = {this.state.selectedTechnicalSpecialistName}
                        onChangeHandler = {this.technicalSpecialistChangeHandler}
                        editHandler = {this.editTechSpecHandler}
                        deleteHandler = {this.deleteTechSpecHandler}
                        interactionMode = {interactionMode}
                        HeaderData={this.assignedSpecHeader}
                        gridRef={ref => { this.assignedSpecChild = ref; }}
                        pageMode={this.props.pageMode}
                        currentPage={this.props.currentPage}
                        isEditable = {this.state.isEditable}
                        rowSelectedHandler = {this.techSpecRowSelectHandler}
                        viewAllTechnicalSpecialist={this.viewAllTechnicalSpecialist}
                        />
                    <TechnicalSpecialistSchedules
                        HeaderData={this.headerData.technicalSpecialistSchedules} 
                        addHandler={this.scheduleAddHandler}
                        deleteHandler={this.scheduleDeleteHandler}
                        rowData = { scheduleLists }
                        gridRef={ref => { this.scheduleChild = ref; }}
                        interactionMode = {interactionMode}
                        isEditable = {this.state.isEditable}
                        pageMode={this.props.pageMode}
                        rowSelectedHandler = {this.scheduleRowSelectHandler}/>
                    <div className="row">                       
                                <div className="col s6">
                                    <ChargeScheduleRates
                                        HeaderData={this.headerData.chargeScheduleRates}
                                        rowData={chargeRates} />
                                </div>
                          { /*D-720*/ }  
                        {this.props.isContractHolderCompany && this.props.isInterCompanyAssignment  ?
                             null: (!this.props.viewPayRate? null:
                             <div className="col s6">
                             <PayScheduleRates
                                 HeaderData={this.headerData.payScheduleRates}
                                 rowData={payRates} />
                         </div>)
                       }
                    </div>
                </div>
                { this.props.isARSSearch && <ArsSearch isShowARSModal={this.props.isARSSearch} /> }
            </Fragment>
        );
    }
}

export default AssignedSpecialist;