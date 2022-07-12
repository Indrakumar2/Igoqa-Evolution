import React, { Component, Fragment } from 'react';
import { renderToString } from "react-dom/server";
// import Hello from "./Hello";
import jsPDF from "jspdf";
import 'jspdf-autotable';
import Panel from '../../../../common/baseComponents/panel';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,
    formInputChangeHandler,
    bindAction, 
    isEmpty, 
    mergeobjects, 
    isEmptyReturnDefault,
    customRegExValidator,
    processMICoordinators,
    parseQueryParam, 
    isEmptyOrUndefine,
    deepCopy
} from '../../../../utils/commonUtils';
import { HeaderData } from './preAssignmentHeader';
import { SearchGRM, OptionalSearch } from '../../../../common/resourceSearch/searchGRM/searchGRM';
import MoreDetails from '../../../../common/resourceSearch/moreDetails';
import ActionSearch from '../../../../common/resourceSearch/actionSearch';
import ResourceSearchSaveBar from '../../applicationComponent/resourceSearchSaveBar';
import { required } from '../../../../utils/validator';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalMessageConstant,modalTitleConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import { preAssignmentProperty } from './preAssignmentProperty';
import ErrorList from '../../../../common/baseComponents/errorList';
import { AppDashBoardRoutes } from '../../../../routes/routesConfig';
import CustomerAndCountrySearch from '../../../../components/applicationComponents/customerAndCountrySearch';
import moment from 'moment';
import { ResourceAssigned } from '../../../../common/resourceSearch/resourceFields';
import arrayUtil from '../../../../utils/arrayUtil';
import  DirectionsGoogleMap from '../../../../common/resourceSearch/googleMap';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import Table from '../../../../common/baseComponents/table';
import dateUtil from '../../../../utils/dateUtil';

const localConstant = getlocalizeData();
const ref = React.createRef();

export const PreAssignmentDiv = (props) => {
    const { companyList, ocCompanyList,inputChangeHandler,contractHoldingCoodinatorList,operatingCoordinatorList,searchParameter,
        customerList,defaultCustomerName } = props;    
    return(  
        <div className="row mb-2">
            <div className="col s12 pr-0 pl-0">
                { !props.interactionMode ?     //D-717 IGO QA
                <CustomerAndCountrySearch
                    customerName = {defaultCustomerName ? defaultCustomerName :searchParameter.customerName}
                    colSize="col s3 pl-0"
                    isMandate={ true }
                    disabled={props.interactionMode}
                    readOnly={props.interactionMode}
                    contractCustomerChange={props.customerChangeHandler} //D576
                   />
            //D-717 IGO QA Started
                   :
                   <CustomInput
                   hasLabel={true}
                   divClassName='col'
                   label={localConstant.customer.CUSTOMER_NAME}
                   type='text'
                   dataValType='valueText'
                   colSize='s3'
                   inputClass="customInputs disabled"
                   name="contractNumber"
                   maxLength={12}
                   readOnly={true}
                   disabled={true}
                value={defaultCustomerName ? defaultCustomerName :searchParameter.customerName}/> 
            //D-717 IGO QA -End
                }  
                           
             <CustomInput
                    hasLabel={true}
                    name="projectName"
                    colSize='s3'
                    label={localConstant.resourceSearch.PROJECT_NAME}
                    labelClass="customLabel"
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    value={searchParameter.projectName ? searchParameter.projectName : ""}
                    onValueChange={props.inputChangeHandler}
                    readOnly={props.interactionMode} />
            </div>
            <div className="col s12 pr-0 pl-0">
                <CustomInput
                    hasLabel={true}
                    name="chCompanyCode"
                    id="chCompanyCodeId"
                    colSize='s3'
                    label={localConstant.resourceSearch.CH_COMPANY}
                    optionsList={companyList}
                    labelClass="customLabel mandate"
                    optionName='companyName'
                    optionValue="companyCode"
                    disabled={ props.interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={searchParameter.chCompanyCode}
                    onSelectChange={inputChangeHandler} 
                    />
                <CustomInput
                    hasLabel={true}
                    name="chCoordinatorLogOnName"
                    id="chCoordinatorLogOnNameId"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.CH_COORDINATOR}
                    optionsList={contractHoldingCoodinatorList}
                    labelClass="customLabel mandate"
                    optionName='miDisplayName'
                    optionValue="username"
                    disabled={props.interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={searchParameter.chCoordinatorLogOnName}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="opCompanyCode"
                    id="opCompanyCodeId"
                    colSize='s3 pl-0'
                    // divClassName="right"
                    label={localConstant.resourceSearch.OPERATING_COMPANY}
                    optionsList={ocCompanyList}
                    labelClass="customLabel mandate"
                    optionName='companyName'
                    optionValue="companyCode"
                    disabled={props.interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={searchParameter.opCompanyCode}
                    onSelectChange={inputChangeHandler} />
                <CustomInput
                    hasLabel={true}
                    name="opCoordinatorLogOnName"
                    id="opCoordinatorLogOnNameId"
                    colSize='s3 pl-0'
                    // divClassName="right"
                    label={localConstant.resourceSearch.OC_COORDINATOR}
                    optionsList={operatingCoordinatorList}
                    labelClass="customLabel mandate"
                    optionName='miDisplayName'
                    optionValue="username"
                    disabled={props.interactionMode}
                    type='select'
                    inputClass="customInputs"
                    defaultValue={searchParameter.opCoordinatorLogOnName}
                    onSelectChange={inputChangeHandler} />
            </div>
        </div>
);
};
const SelectionPopUpModal = (props) => {
    return (
        <Fragment>
            <ReactGrid gridRowData={props.optionList} 
            gridColData={props.headerData} onRef={props.gridRef}
             />
        </Fragment>
    );
};
/** Sub-Supplier modal - pre-Assignment */
export const SubSupplierModalPopup = (props) => {
    const { inputChangeHandler,subSupplierData } = props;
    return(
        <div className="row">
            <CustomInput
                hasLabel={true}
                name="subSupplier"
                id = "subSupplierId"
                labelClass="customLabel mandate"
                colSize='s4'
                label={localConstant.resourceSearch.SUB_SUPPLIER}
                type='text'
                inputClass="customInputs"
                htmlFor="newSubSupplier"
                maxLength={100}
                defaultValue = {subSupplierData.subSupplier}
                onValueChange={inputChangeHandler} />
            <CustomInput
                hasLabel={true}
                name="subSupplierLocation"
                id="subSupplierLocationId"
                labelClass="customLabel mandate"
                divClassName='pl-0'
                colSize='s8'
                label={localConstant.resourceSearch.SUB_SUPPLIER_LOCATION}
                type='textarea'
                inputClass="customInputs"
                htmlFor="newSubSupplierLocation"
                maxLength={500}
                defaultValue = {subSupplierData.subSupplierLocation}
                onValueChange={inputChangeHandler} />
        </div>
    );
};

class PreAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            actionDivDynamicClass:false,
            isPanelOpenMoreDetails: false,
            isPanelOpenOptionalSearch:false,
            isPanelOpenSearchParams: true,
            isSubSupplierModalOpen: false,
            isSubSupplierEditMode: false,
            showGoogleMap: false,
            isShowMapBtn: false,
            errorList: [],
            firstVisitFromDate: dateUtil.postDateFormat(new Date(), '-'),//def 1408 & 1384 IE EDGE version 44 fix
            firstVisitToDate: dateUtil.postDateFormat(new Date(), '-'),//def 1408 &  1384 IE EDGE version 44 fix
            expiryFromDate: '',
            expiryToDate: '',
            optionAttributs: {
                optionName: 'name',
                optionValue: 'name'
            },
            isOpensearchSlide: false,
            showExceptionList: false,
            isSelectionPopUpOpen: false
        };
        this.groupingParam = {
            groupName: "location",
            dataName: "resourceSearchTechspecInfos"
        };
        this.exceptiongGoupingParam = {
            groupName: "supplierName",
            dataName: "searchExceptionResourceInfos"
        };
        this.updatedData = {};
        this.editedRow = {};
        this.preAssignmentActions = [];

        /** Sub suppleir grid buttons */
        this.subSupplierGridBtn = [
            {
                name: localConstant.commonConstants.ADD,
                action: this.subSupplierAddHandler,
                btnClass: "btn-small mr-1 mt-1",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.DELETE,
                action: this.subSupplierDeleteHandler,
                btnClass: "btn-small mt-1 dangerBtn",
                showbtn: true
            }
        ];
        /** Grm Search buttons */
        this.optionalSearchGrm = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: (e) => this.preAssignmentSearch(e),
                btnClass: "btn-small mt-1",
                showbtn: true
            }
        ];
        /** Pre assignment search buttons */
        this.preAssignmentSearchBtn = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: (e) => this.preAssignmentSearch(e),
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        /** Sub-Supplier buttons */
        this.subSupplierButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                type: 'reset',
                action: this.cancelsubSupplier,
                btnID: "cancelSubSupplier",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type: "button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                type: 'submit',
                btnID: "addSubSupplier",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];

        /** error list button */
        this.errorListButton = [
            {
                name: localConstant.commonConstants.OK,
                action: (e) => this.closeErrorList(e),
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.googleMapCloseButton = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.showGoogleMap,
                btnID: "closeErrorList",
                btnClass: "zmdi-close mr-3 mt-2",
                showbtn: true
            }
        ];
        this.exceptionListloseButton = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.closeExceptionList,
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];

        /** export buttons */
        this.exportGridBtn = [
            {
                name: localConstant.commonConstants.VIEW_MAP,
                action: this.showGoogleMap,
                btnClass: "btn-flat mr-1 ",
                showbtn: true,
                disabled: true
            },
            {
                name: localConstant.commonConstants.SEARCHEXCEPTIONLIST,
                action: this.searchExceptionList,
                btnClass: "btn-flat mr-1",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.EXPORT_TO_STANDARDCV,
                action:this.selectionPopUp,//action: this.exportToStandardCV,
                btnClass: "btn-flat mr-1 ",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.EXPORT_TO_CHEVRONCV,
                action: this.exportToChevronCv,
                btnClass: "btn-flat mr-1 ",
                showbtn: true
            },
            {
                name: localConstant.commonConstants.EXPORT_TO_PDF,
                action: this.exportToPDF,
                btnClass: "btn-flat ",
                showbtn: true
            }
        ];
        this.SelectionPopUpButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSelectionPopUp,
                btnID: "cancelSelectionPopUp",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.exportToStandardCV,
                btnID: "exportToStandardCV",
                btnClass: "waves-effect waves-teal btn-small ",
                showbtn: true
            }
        ];
        const functionRefs = {};
        functionRefs["enableEditColumn"] = this.enableEditColumn;
        functionRefs["getInterCompanyInfo"] = this.getInterCompanyInfo;
        this.headerData = HeaderData(functionRefs);
        this.exportDocument = this.exportDocument.bind(this);
    }

    exportDocument = (epin,isChevronExport) => {
        this.props.actions.ExportToCV(epin,isChevronExport);
    }

    enableEditColumn = (e) => {
        return this.props.isPreAssignmentWon;
    }

    getInterCompanyInfo =(e)=> { 
        const opComp= this.props.preAssignmentDetails.searchParameter && this.props.preAssignmentDetails.searchParameter.opCompanyCode;
      if( !isEmptyOrUndefine(opComp) && opComp!== this.props.selectedHomeCompany)
      {
          return opComp;
      }
     return null;
    }
       /**TechSpec Search */
    preAssignmentSearch (e){
        e.preventDefault();
        const isValid = this.preAssignmentSearchMandatoryFieldCheck();
        if(isValid){
            if (this.props.selectedCustomerData.length>0) {
                this.updatedData.customerCode = this.props.selectedCustomerData[0].customerCode;
                this.props.actions.UpdateActionDetails(this.updatedData);
                this.updatedData.customerName = this.props.selectedCustomerData[0].customerName;
            }
            this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            this.updatedData={};
            this.props.actions.searchPreAssignmentTechSpec();
        }
       // this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
        this.setState({ isShowMapBtn:true });
    }

    componentDidMount(){
        const result = this.props.location.search && parseQueryParam(this.props.location.search);
        const queryObj={ id:parseInt(result.preID) };
       if(result.searchType === "PreAssignment"){            
            this.props.actions.FetchPreAssignment(queryObj);
        }
            /**
         * this.props.isGrmMasterDataFeteched is bool flag which will be set as true once the
         * GRM master data is loaded. For subsequent GRM related menus we check for this flag to load master data
         * If user goes out of GRM/Resource module NEED set the flag to false to reload masterdata 
         */
        // if (this.props.isGrmMasterDataFeteched === false) {
        //     this.props.actions.grmDetailsMasterData();
        // }       
        this.props.actions.FetchTechSpecCategory();
        this.props.actions.FetchTaxonomyCustomerApproved();
        this.props.actions.FetchCertificates();
        this.props.actions.FetchEquipment();
        // this.props.actions.FetchAssignmentType();
        //   this.preAssignmentActions = localConstant.resourceSearch.preAssignmentAction;
        if(this.state.firstVisitFromDate && this.state.firstVisitToDate){
            this.updatedData.firstVisitFromDate = this.state.firstVisitFromDate;
            this.updatedData.firstVisitToDate = this.state.firstVisitToDate;
            this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            this.updatedData={};
        }
    }
//D576
    customerChangeHandler = (data) => {
        this.props.actions.TechSpechUnSavedData(false); //D576
    }

    /** Component unmount */
    componentWillUnmount() {
        this.props.actions.clearPreAssignmentDetails();
        // this.props.actions.ClearMySearchData();
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearServices();
    }

    showGoogleMap = (e) => {
        e.preventDefault(); 
        if (!this.state.showGoogleMap) {
            this.props.actions.GetGeoLocationInfo().then(response => {
                if (response)
                    this.setState({ showGoogleMap: !this.state.showGoogleMap });
            });
        }
        else
        {
            this.setState({ showGoogleMap: !this.state.showGoogleMap });  
        } 
    };

    /** searchExceptionList button handler */

    searchExceptionList = (e) => {
        e.preventDefault();
        this.setState({ showExceptionList: true });
    }
    closeExceptionList = (e) => {
        e.preventDefault();
        this.setState({ showExceptionList: false });
    }
    selectionPopUp = (e) => {
        e.preventDefault();
        const selectedRecords = this.gridChild.gridApi.getSelectedRows();
        if (selectedRecords && selectedRecords.length > 0) {
            if (selectedRecords.length > 5) {
                IntertekToaster(localConstant.resourceSearch.RESOURCE_MAXIMUM_SELECTION, "warningToast Google Map");
                return false;
            }
        }
        else {
            IntertekToaster(localConstant.resourceSearch.RESOURCE_MINIMUM_SELECTION, "warningToast Google Map");
            return false;
        }
        this.setState({
            isSelectionPopUpOpen: true,
        });
    };
    cancelSelectionPopUp = (e) => {
        e.preventDefault();
        this.setState({
            isSelectionPopUpOpen: false
        });
    }
/** exportToChevronCv button handler */
exportToChevronCv = (e) => {
    e.preventDefault();
    const exportCVFrom=6;
    const selectedRecords = this.gridChild.gridApi.getSelectedRows();
    if(selectedRecords && selectedRecords.length > 0){
     if(selectedRecords.length > 5){
            IntertekToaster(localConstant.resourceSearch.RESOURCE_MAXIMUM_SELECTION,"warningToast Google Map");
        } 
        else{
            this.props.actions.ExportToMultiCV(selectedRecords,true,exportCVFrom);
            IntertekToaster(localConstant.resourceSearch.RESOURCE_DOWNLOAD_MESSAGE,"warningToast Google Map");
        }
        
    }
   
    else {
        IntertekToaster(localConstant.resourceSearch.RESOURCE_MINIMUM_SELECTION,"warningToast Google Map");
    }
};

exportToStandardCV =(e) =>{
    e.preventDefault();
    const selectedSections = this.selectGridChild.getSelectedRows();
    const exportCVFrom=6;
    const selectedRecords = this.gridChild.gridApi.getSelectedRows();
    if(selectedRecords.length > 0){
        if(selectedRecords.length > 5){
            IntertekToaster(localConstant.resourceSearch.RESOURCE_MAXIMUM_SELECTION,"warningToast Google Map");
        } 
        else{
            this.props.actions.ExportToMultiCV(selectedRecords,false,exportCVFrom,"","","",selectedSections);
            IntertekToaster(localConstant.resourceSearch.RESOURCE_DOWNLOAD_MESSAGE,"warningToast Google Map");
           
        }
    } else {
        IntertekToaster(localConstant.resourceSearch.RESOURCE_MINIMUM_SELECTION,"warningToast Google Map");
    }
    this.setState({
        isSelectionPopUpOpen: false
    });
}

/** exportToPDF button handler */
    exportToPDF = (e) => {
        e.preventDefault();
        if (Array.isArray(this.props.techSpecList) && this.props.techSpecList.length > 0) {
            const doc = new jsPDF('l', 'pt', 'a4');
            const styleDef = { fillColor: "#FFFFFF", textColor: "#1E1E1E", lineWidth: 1 };
            doc.autoTable({ html: "#exportTableId", theme: "grid", styles: styleDef });
            doc.save(localConstant.commonConstants.PDF_FILE_NAME + ".pdf");
        }
    };

    /** Panel click handler */
    panelClick = (params) => {
        this.setState({ [`isPanelOpen${ params }`]: !this.state[`isPanelOpen${ params }`]  });   
    }

    /** Input change handler - for search parameters */
    inputChangeHandler =(e)=>{
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        //def 245
        const coordinatorParams = {
            companyCode: inputvalue.value ,
            userTypes: [ "Coordinator","MICoordinator" ],
            isActiveCoordinators: true
        };
        if(inputvalue.name === "customerCode"){
            if(!required(inputvalue.value)){
                this.updatedData[inputvalue.name] = inputvalue.value;
                this.props.actions.UpdateActionDetails(this.updatedData);
                this.updatedData = {};
                this.updatedData["customerName"] =  e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            }
        }
        else if(inputvalue.name === "chCompanyCode"){
            if(!required(inputvalue.value)){
                const companyForMI = [ ];
                coordinatorParams.companyCode =inputvalue.value; //def 245
                this.props.actions.FetchContractHoldingCoordinator(coordinatorParams,true);
                this.updatedData["companyCode"] = inputvalue.value;
                this.props.actions.UpdateActionDetails(this.updatedData);
                this.updatedData = {};
               // this.props.actions.FetchOperatingCoordinator(coordinatorParams); //not required as binding this in FetchContractHoldingCoordinator 
                this.updatedData["opCompanyCode"] = inputvalue.value;
                this.updatedData["opCompanyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                this.updatedData["chCompanyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            }
            this.props.actions.ClearContractHoldingCoordinator();
            this.props.actions.ClearOperatingCoordinator();
        }
        else if(inputvalue.name === "opCompanyCode"){
            if(!required(inputvalue.value))
            {  
                coordinatorParams.companyCode =inputvalue.value; //def 245
                this.props.actions.FetchOperatingCoordinator(coordinatorParams);
                this.updatedData["companyCode"] = inputvalue.value;
                this.props.actions.UpdateActionDetails(this.updatedData);
                this.updatedData = {};
                this.updatedData["opCompanyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            }
            this.props.actions.ClearOperatingCoordinator();
        }
        else if(inputvalue.name === "categoryName"){
            this.props.actions.ClearSubCategory();
            this.props.actions.ClearServices();
            if(!isEmptyOrUndefine( inputvalue.value) )
            {
                this.props.actions.FetchTechSpecSubCategory(inputvalue.value);
            } 
            this.updatedData[inputvalue.name] = inputvalue.value;
            if(inputvalue.value === ""){
                this.updatedData["subCategoryName"] = inputvalue.value;
                this.updatedData["serviceName"] = inputvalue.value;
                this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            }
            this.props.actions.UpdateActionDetails(this.updatedData);
            this.updatedData = {};
        }
        else if(inputvalue.name === "subCategoryName"){
            this.props.actions.ClearServices();
            if(!isEmptyOrUndefine( inputvalue.value) )
            {
                this.props.actions.FetchTechSpecServices(this.props.preAssignmentDetails.categoryName, inputvalue.value);//def 916 fix
            } 
            this.updatedData[inputvalue.name] = inputvalue.value;
            if(inputvalue.value === ""){
                this.updatedData["serviceName"] = inputvalue.value;
                this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            }
            this.props.actions.UpdateActionDetails(this.updatedData);
            this.updatedData = {};
        }
        else if(inputvalue.name === "serviceName"){
            this.updatedData[inputvalue.name] = inputvalue.value;
            this.props.actions.UpdateActionDetails(this.updatedData);
            this.updatedData = {};
        }
        this.updatedData[inputvalue.name] = inputvalue.value;
        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
        this.updatedData = {};
    };

    /**Email validation */
    emailValidation(value) {
        if ((value) &&
            customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i', value)) {
            IntertekToaster(localConstant.techSpec.contactInformation.EMAIL_VALIDATION, 'warningToast');
            return true;
        }
    }
    /** on change handler - for action section */
    actionOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        if(inputValue.name === "searchAction" && (inputValue.value === "SD" || inputValue.value === "L")){
            this.props.actions.FetchDispositionType(inputValue.value);
        }
        if(inputValue.name === "searchAction" && inputValue.value === "W"){
            this.updatedData["description"] = ""; //Scenario Defect 106
        }
        // Ui Aliginment Dynamic Margin Class Added for Action selection Scenario Defect -137
        if(inputValue.name === "searchAction" && inputValue.value === "" || inputValue.name === "searchAction" && inputValue.value === "CUP"){
            this.setState({ actionDivDynamicClass:false });
        }else{
            this.setState({ actionDivDynamicClass:true });
        }
        this.props.actions.UpdateActionDetails(this.updatedData);
        this.updatedData = {};
    };

    /**on Change handler - for OptionalSearch */
    optionalOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        this.props.actions.AddOptionalSearch(this.updatedData);
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

    /** Sub-Supplier validation handler */
    subSupplierValidationHandler = (data) => {
        const subSupplier = document.getElementById('newSubSupplier').value.trim();
        if(required(subSupplier)){
            IntertekToaster(localConstant.validationMessage.PA_SUB_SUPPLIER_VAL,"warningToast subSupplierNameVal");
            return false;
        }
        const subSupplierLocation = document.getElementById('newSubSupplierLocation').value.trim();
        if(required(subSupplierLocation)){
            IntertekToaster(localConstant.validationMessage.PA_SUB_SUPPLIER_LOCATION_VAL,"warningToast subSupplierLocationVal");
            return false;
        }
        return true;
    };

    /** Sub-Supplier Add button handler */
    subSupplierAddHandler = (e) => {
        e.preventDefault();
        this.setState({
            isSubSupplierModalOpen:true,
            isSubSupplierEditMode:false
        });
    };

    /** Sub-Supplier delete button handler */
    subSupplierDeleteHandler = (e) => {
        e.preventDefault();
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.SUB_SUPPLIER_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSubSupplier,
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
            IntertekToaster(localConstant.commonConstants.SELECT_RECORD_TO_DELETE,'warningToast subSupplierDeleteToaster');
        }
    };

    /** Sub-Supplier edit button handler */
    subSupplierEditHandler = (data) => {
        this.setState({
            isSubSupplierModalOpen:true,
            isSubSupplierEditMode:true
        });
        this.editedRow = data;
    };

    /** Sub-Supplier Submit function */
    submitSubSupplier=(e)=>{
        e.preventDefault();
        if (this.state.isSubSupplierEditMode) {
            const data = mergeobjects(this.editedRow, this.updatedData);
            if(this.updatedData !== undefined){
                if (this.subSupplierValidationHandler(this.updatedData)) {
                    if (this.subSupplierAlreadyExist(data)) {
                        IntertekToaster(localConstant.validationMessage.PA_SUB_SUPPLIER_ALREADY_EXISTS_VAL, "warningToast NoChangesOccured");
                        return false;
                    }
                    this.props.actions.UpdateSubSupplier(data, this.editedRow);
                    this.cancelsubSupplier(e);
                } 
            }
        }
        else{
            if(this.subSupplierValidationHandler(this.updatedData)){
                if(this.subSupplierAlreadyExist(this.updatedData)){
                    IntertekToaster(localConstant.validationMessage.PA_SUB_SUPPLIER_ALREADY_EXISTS_VAL,"warningToast NoChangesOccured");
                    return false;
                }
                this.updatedData.subSupplierFullAddress = this.updatedData.subSupplierLocation;
                this.props.actions.AddSubSupplier(this.updatedData);
                this.cancelsubSupplier(e);
            }
        }
    }

    /** Sub-Supplier already exist */
    subSupplierAlreadyExist = (subSupplierData) => {
        let isAlreadyExist = false;
        const subSuppliers = isEmptyReturnDefault(this.props.subSuppliers.subSupplierInfos);
        subSuppliers.forEach(iteratedValue=>{
            if(iteratedValue.subSupplier === subSupplierData.subSupplier && iteratedValue.subSupplier !== this.editedRow.subSupplier){//Changes for D907
                isAlreadyExist = true;
            }
        });
        return isAlreadyExist;
    };
    
    /** Sub-Supplier delete function */
    deleteSubSupplier=(e)=>{
        e.preventDefault();
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteSubSupplier(selectedRecords);
        this.props.actions.HideModal();
    }

    /** Cancel Sub-Supplier changes */
    cancelsubSupplier = (e) => {
        e.preventDefault();
        this.setState({
            isSubSupplierModalOpen:false,
            isSubSupplierEditMode:false
        });
        this.editedRow = {};
        this.updatedData = {};
    };

    /** Confirmation Reject Handler */
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    /** Save Pre-Assignment Details */
    savePreAssignment = (e) => {
        e.preventDefault(); 
        const selectedRecords = this.gridChild.getSelectedRows();
        if (this.props.selectedCustomerData.length>0) {
            this.updatedData.customerCode = this.props.selectedCustomerData[0].customerCode;
            this.props.actions.UpdateActionDetails(this.updatedData);
            this.updatedData.customerName = this.props.selectedCustomerData[0].customerName;
        } 

        /**
         * ITK D-625
         * Once we get Supplier and Sub-Supplier information from search. Uncomment the code to resolve the issue.
         */
        const searchParameter = isEmptyReturnDefault(this.props.preAssignmentDetails.searchParameter, 'object');
        const subSuppliers = isEmptyReturnDefault(searchParameter.subSupplierInfos); 
        const selectedTechSpecEpins = [];
        if (selectedRecords.length !== 0) {  
            if (subSuppliers.length !== 0 && !this.props.isInterCompanyPreAssignment) { //def 662 
                subSuppliers.forEach(iteratedValue  => {
                    let isClearSelectedSubSupplierTS=true;
                    let subSupplierLocation = `${ iteratedValue.subSupplier },${ iteratedValue.subSupplierLocation }`;
                    subSupplierLocation = subSupplierLocation.replace( /[\r\n]+/gm, " " ); //D901
                    selectedRecords.forEach(selectedRecord => {
                        const supplierName = selectedRecord.supplierLocationId ? selectedRecord.supplierLocationId.substring(2) : ''; //Changes For Sanity Def145
                        const selectedSubSupplierTechspec = {
                            epin: selectedRecord.epin,
                            lastName: selectedRecord.lastName,
                            firstName: selectedRecord.firstName,
                        };  
                        if (subSupplierLocation.trim() === supplierName.trim()) {
                            if(isClearSelectedSubSupplierTS)
                            {
                                iteratedValue.selectedSubSupplierTS = [];
                                isClearSelectedSubSupplierTS=false;
                            }
                            iteratedValue.selectedSubSupplierTS.push(selectedSubSupplierTechspec); 
                        } 
                    });
                });
            } 
            selectedRecords.forEach(selectedRecord => {
            const supplierName = selectedRecord.supplierLocationId ? selectedRecord.supplierLocationId.substring(2) : ''; //Changes For Sanity Def145
            //let mainSupplierLocation = `${ searchParameter.supplier }, ${ searchParameter.supplierLocation }`;
            let mainSupplierLocation = `${ searchParameter.supplierLocation }`;
            mainSupplierLocation = mainSupplierLocation.replace( /[\r\n]+/gm, " " ); //D901
            const selectedTechspec = {
                epin: selectedRecord.epin,
                lastName: selectedRecord.lastName,
                firstName: selectedRecord.firstName,
            }; 
            if (mainSupplierLocation.trim() === supplierName.trim()) {
                selectedTechSpecEpins.push(selectedTechspec);
            }
        });
              
        }
        else
        { 
            subSuppliers.forEach(iteratedValue => {
                iteratedValue.selectedSubSupplierTS = [];
            });
        } 
        this.updatedData.selectedTechSpecInfo = selectedTechSpecEpins;
        this.updatedData.subSupplierInfos = subSuppliers;

        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
        this.updatedData={};

        const isValid = this.preAssignmentSaveMandatoryFieldCheck();
        if(isValid){
            if(this.props.preAssignmentDetails.id){
                this.props.actions.UpdatePreAssignment().then(res=>{
                    if(res){
                        this.props.actions.ClearMySearchData();
                        this.props.history.push(AppDashBoardRoutes.mysearch);
                    }
                });
            }
            else{
                this.props.actions.SavePreAssignment().then(res=>{
                    if(res){
                        this.props.actions.ClearMySearchData();
                        this.props.history.push(AppDashBoardRoutes.mysearch);
                    }
                });
            }
        }
    };

    /** Pre-Assignment Search Mandatory field check */
    preAssignmentSearchMandatoryFieldCheck = () => {
        const searchParameters = isEmptyReturnDefault(this.props.preAssignmentDetails.searchParameter,'object');
        const optionalSearch = isEmptyReturnDefault(searchParameters.optionalSearch,'object');
        const customerCode = this.props.selectedCustomerData.length > 0 ? this.props.selectedCustomerData[0].customerCode:searchParameters.customerCode;
        const errors = [];
        if(required(customerCode)){
            errors.push(`${ localConstant.validationMessage.PA_CUSTOMER_VALIDATION }`);
        }
        if(required(searchParameters.chCompanyCode)){
            errors.push(`${ localConstant.validationMessage.PA_CH_COMPANY_VALIDATION }`);
        }
        if(required(searchParameters.chCoordinatorLogOnName  )){
            errors.push(`${ localConstant.validationMessage.PA_CH_COORDINATOR_VALIDATION }`);
        }
        if(required(searchParameters.opCompanyCode)){
            errors.push(`${ localConstant.validationMessage.PA_OP_COMPANY_VALIDATION }`);
        }
        if(required(searchParameters.opCoordinatorLogOnName)){
            errors.push(`${ localConstant.validationMessage.PA_OP_COORDINATOR_VALIDATION }`);
        }
        if(required(searchParameters.supplier)){
            errors.push(`${ localConstant.resourceSearch.SUPPLIER } - ${ localConstant.validationMessage.PA_SUPPLIER_VALIDATION }`);
        }
        if(required(searchParameters.supplierLocation)){
            errors.push(`${ localConstant.resourceSearch.SUPPLIER } - ${ localConstant.validationMessage.PA_SUPPLIER_LOCATION_VAL }`);
        }
        // if(required(searchParameters.assignmentType)){
        //     errors.push(`${ localConstant.resourceSearch.ASSIGNMENT_TYPE } - ${ localConstant.validationMessage.PA_ASSIGNMENT_TYPE_VAL }`);
        // }
        if(required(searchParameters.categoryName )){
            errors.push(`${ localConstant.resourceSearch.TAXONOMY } - ${ localConstant.validationMessage.PA_CATEGORY_VAL }`);
        }
        if(required(searchParameters.subCategoryName  )){
            errors.push(`${ localConstant.resourceSearch.TAXONOMY } - ${ localConstant.validationMessage.PA_SUB_CAGTEGORY_VAL }`);
        }
        if(required(searchParameters.serviceName  )){
            errors.push(`${ localConstant.resourceSearch.TAXONOMY } - ${ localConstant.validationMessage.PA_SERVICE_VAL }`);
        }

        if (errors.length > 0) {
            this.setState({
                errorList: errors
            });
            return false;
        }
        else{
            if (!isEmpty(searchParameters.firstVisitToDate)) {
            if(!moment(searchParameters.firstVisitToDate).isAfter((searchParameters.firstVisitFromDate),'day')){
                if(!moment(searchParameters.firstVisitFromDate).isSame(moment(searchParameters.firstVisitToDate), 'day')){
                    IntertekToaster(localConstant.validationMessage.TO_DATE_SHOULD_NOT_LESS_THAN_FROM_DATE, 'warningToast gdEndDateValCheck');
                    return false;
                }
            }
        }

            if (!required(searchParameters.customerContactEmail)){
               if(this.emailValidation(searchParameters.customerContactEmail)){
                return false;
               }  
            }

            if (!isEmpty(optionalSearch.certificationExpiryTo)) {
            if(!moment(optionalSearch.certificationExpiryTo).isAfter((optionalSearch.certificationExpiryFrom),'day')){
                if(!moment(optionalSearch.certificationExpiryFrom).isSame(moment(optionalSearch.certificationExpiryTo), 'day')){
                    IntertekToaster(localConstant.validationMessage.EXPIRY_TO_DATE_SHOULD_NOT_LESS_THAN_EXPIRY_FROM_DATE, 'warningToast gdEndDateValCheck');
                    return false;
                }
            }
        }
        }

        return true;
    };

    /** Pre-Assignment Save Mandatory field check */
    preAssignmentSaveMandatoryFieldCheck =()=>{
        const isValid = this.preAssignmentSearchMandatoryFieldCheck();
        const preAssignmentDetails = this.props.preAssignmentDetails;
        const errors = [];
        if(isValid){
            if(required(preAssignmentDetails.searchAction  )){
                errors.push(`${ localConstant.resourceSearch.ACTION }`);
            }
            if(preAssignmentDetails.searchAction ==="SD"){
                if(required(preAssignmentDetails.dispositionType)){
                    errors.push(`${ localConstant.resourceSearch.DISPOSITION_DETAILS }`);
                }
            } else if(preAssignmentDetails.searchAction === "L"){ //Fixes for Scenario failed by SMN QA
                if(required(preAssignmentDetails.dispositionType)){
                    errors.push(`${ localConstant.resourceSearch.DISPOSITION_DETAILS }`);
                }
            }
    
            if (errors.length > 0) {
                this.setState({
                    errorList: errors
                });
                return false;
            } else{
                return true;
            }
        }
    }
   
    /** close error list handler */
    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    };

    /**first visit date handler */
    firstVisitFrom = (date) => {
        this.setState({ 
            firstVisitFromDate: date 
        },() => {
            this.updatedData.firstVisitFromDate = this.state.firstVisitFromDate !== null ? this.state.firstVisitFromDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            this.updatedData = {};
        }); 
    }
    
    firstVisitTo = (date) => {
        this.setState({
            firstVisitToDate: date
        },() => {
            this.updatedData.firstVisitToDate = this.state.firstVisitToDate !== null ? this.state.firstVisitToDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
            this.updatedData = {};
        }); 
    }

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

    //Multi Select Change Event
    equipmentDescriptionMultiselect =(value)=>{
        const equipmentsdescription = [];
        value.map(eqipment => {
            equipmentsdescription.push(eqipment.label);
        });
        this.updatedData["equipmentMaterialDescription"] = equipmentsdescription;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={};
    }

    certificationMultiselect = (selectedvalue) => {
        const certificates = [];
        selectedvalue.map(certificate => {
            certificates.push(certificate.label);
        });
        this.updatedData["certification"] = certificates;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={}; //Added for Sanity Def 165
    }

    customerApprovalMultiselect =(value)=>{
        const customerApproval = [];
        value.map(customer => {
            customerApproval.push(customer.label);
        });
        this.updatedData["customerApproval"] = customerApproval;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={}; //D564
    }

    languageSpeakingMultiselect = (selectedvalue) => {
        const languages = [];
        selectedvalue.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageSpeaking"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={}; //Added for Sanity Def 165
    }

    languageWritingMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageWriting"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={};//Added for Sanity Def 165
    }

    languageComprehensionMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData["languageComprehension"] = languages;
        this.props.actions.AddOptionalSearch(this.updatedData);
        this.updatedData={};//Added for Sanity Def 165
    }

    convertToMultiSelectObject=(datas)=>{
        const multiselectArray = [];
        if (datas && Array.isArray(datas)) {
            datas.map(data => {
                multiselectArray.push({ value: data , label: data });
            });
        }
        return multiselectArray;
    }

    resourceDatas = (data) => {
        if(data && data.length > 0){
            // const techSpecArray = [];
            const uniqueTechSpecs = arrayUtil.removeDuplicates(data,'epin');
            return uniqueTechSpecs.map(iteratedValue => {
                return `${ iteratedValue.lastName } ${ iteratedValue.firstName }`.toString();
            }).join(',');
        }
        return "N/A";
    };

    /** Cancel Pre-Assignment changes */
    cancelPreAssignmentHandler = (e) => {
        e.preventDefault();
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.CANCEL_CHANGES,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: localConstant.commonConstants.YES,
                onClickHandler:(e)=> this.cancelPreAssignment(e),
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
    };

    /** confirmationRejectHandler */
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    };

    /** cancel pre-assignment changes */
    cancelPreAssignment = (e) => {
        e.preventDefault();
        this.props.actions.ClearMySearchData();
        this.setState({ actionDivDynamicClass:false });
        this.props.preAssignmentDetails.id ?
            this.props.actions.CancelEditPreAssignmentDetails() :
            this.props.actions.CancelCreatePreAssignmentDetails();
        this.props.actions.FetchMySearchData();
        this.props.actions.HideModal();
    };
    toggleSlide=(e)=>{
        this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
       }

       rowSelectHandler = (e) => {
        if(e.data && e.data.epin){
            const searchResource =  isEmptyReturnDefault(this.props.techSpecList);
            const currentRowSupplierLocationId = e.data && e.data.supplierLocationId; // resourceSearchTechspecInfos
            if(searchResource.length > 0)
                searchResource.forEach(iteratedValue => {
                    if(iteratedValue.supplierLocationId === currentRowSupplierLocationId){
                        const resourceIndex = iteratedValue.resourceSearchTechspecInfos && iteratedValue.resourceSearchTechspecInfos.findIndex(x => x.epin === e.data.epin);
                        if(resourceIndex > -1){
                            iteratedValue.resourceSearchTechspecInfos[resourceIndex].isSelected = e.node && e.node.selected;
                        }
                    }
                });
        }
    }
    
    render() {
        const { currentPage,preAssignmentDetails,subSuppliers,userRoleCompanyList,companyList,contractHoldingCoodinatorList,operatingCoordinatorList,
            customerList,techSpecList,dispositionType,languages,certificates,taxonomyCustomerApproved,equipment,isInterCompanyPreAssignment,isOperatorCompany } = this.props;
        let sortedCompanyList = userRoleCompanyList;

        if(isOperatorCompany && isInterCompanyPreAssignment)   
        {
            sortedCompanyList = companyList; //intercompany scenario CH company drop down values
        }      
        sortedCompanyList =  arrayUtil.sort(sortedCompanyList, 'companyName', 'asc');
        const subSupplierList = isEmptyReturnDefault(subSuppliers.subSupplierInfos);
        const searchParameters = deepCopy(isEmptyReturnDefault(preAssignmentDetails.searchParameter, 'object'));
        const optionalSearch= isEmptyReturnDefault(searchParameters.optionalSearch, 'object');
        const techSpecs = isEmptyReturnDefault(searchParameters.selectedTechSpecInfo);
        const isOperatingCompany = (this.props.preAssignmentDetails.id && isInterCompanyPreAssignment && isOperatorCompany) ? true : false;
            
        let headingSub = "";
        if(searchParameters.chCompanyCode && searchParameters.opCompanyCode){
            if(searchParameters.chCompanyCode === searchParameters.opCompanyCode)
                headingSub = localConstant.resourceSearch.DOMESTIC;
            else
                headingSub = localConstant.resourceSearch.INTERCOMPANY;
        }
        if(searchParameters.subSupplierInfos && searchParameters.subSupplierInfos.length>0)
        {
            searchParameters.subSupplierInfos.forEach(subSup=>{
                if(subSup.selectedSubSupplierTS && subSup.selectedSubSupplierTS.length>0)
                {
                    techSpecs.push(...subSup.selectedSubSupplierTS); 
                } 
            });
            
        }
        const assignedResources = this.resourceDatas(techSpecs);

        //Default values for Optional Param Multi select
        const defaultCertificateMultiSelection =this.convertToMultiSelectObject(optionalSearch.certification);
        // const defaultCertificateMultiSelection = [ { name: 'PT-SNT-TC-1A' } ];
        const defaultLanguageSpeakingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageSpeaking);
        const defaultLanguageWritingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageWriting);
        const defaultLanguageComprehensionMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageComprehension);
        const defaultCustomerApprovalMultiSelection=this.convertToMultiSelectObject(optionalSearch.customerApproval);
        const defaultEquipmentMultiSelection =this.convertToMultiSelectObject(optionalSearch.equipmentMaterialDescription);
        const SearchEvolutionHeader=<div>Search(Optional Parameters)</div>;
    //    this.headerData = HeaderData(techSpecs);
    
    // ITK D-625
    const actionDivClass=(preAssignmentDetails && (preAssignmentDetails.searchAction === 'W' || preAssignmentDetails.searchAction === 'L' || preAssignmentDetails.searchAction === 'SD')) ? " quickSearchMarginTop" :" topSectionMargin";

        const HideValues=[];
        if(this.props.preAssignmentDetails.id){
          //  HideValues.push("SD"); // def 702 issue 1 #3 fix
            if(this.props.isInterCompanyPreAssignment && this.props.isOperatorCompany){
                HideValues.push("W");
                HideValues.push("L");
            }
            // -----------------DEF 625 #6  fixes----- Don't uncomment-------------------
            // if (techSpecList) { 
            //     for(let j=0;j<techSpecList.length;j++) {
            //         if (techSpecList[j].resourceSearchTechspecInfos) {
            //             for (let i = 0; i < techSpecList[j].resourceSearchTechspecInfos.length; i++) {
            //                 if (techSpecs) {
            //                     techSpecs.map(techSpec => {
            //                         if (techSpec.epin === techSpecList[j].resourceSearchTechspecInfos[i].epin) {
            //                             techSpecList[j].resourceSearchTechspecInfos[i]["isSelected"] = true;
            //                         }
            //                     });
            //                 }
            //             }
            //         }
            //     };
            // }
            // -----------------DEF 625 #6  fixes------------------------

        } else {
            HideValues.push("W");
            HideValues.push("L");
           // HideValues.push("SD");// def 702 issue 1 #3 fix
        }
        this.preAssignmentActions = arrayUtil.negateFilter(localConstant.resourceSearch.preAssignmentAction, 'value', HideValues);
        
         /** Check for Inactive MI-Coordinator */
         let preAssignmentMICHCoordinatorList = [];
         if(!isEmpty(contractHoldingCoodinatorList)){
             const filteredCoorinators = this.props.preAssignmentDetails.id ? contractHoldingCoodinatorList : contractHoldingCoodinatorList.filter(x=>required(x.status));
             preAssignmentMICHCoordinatorList = processMICoordinators(filteredCoorinators);
         }
 
         let preAssignmentMIOPCoordinatorList = [];
         if(!isEmpty(operatingCoordinatorList)){
            const filteredCoorinators = this.props.preAssignmentDetails.id ? operatingCoordinatorList : operatingCoordinatorList.filter(x=>required(x.status));
             preAssignmentMIOPCoordinatorList = processMICoordinators(filteredCoorinators);
         }
        
        // const contractHoldingCoodinatorListArray=contractHoldingCoodinatorList.filter(row=> row.userType == 'MICoordinator');
        // const operatingCoordinatorListArray=operatingCoordinatorList.filter(row=> row.userType == 'MICoordinator');

        bindAction(this.headerData.supplierHeader, "EditColumn", this.subSupplierEditHandler);
        // bindAction(this.headerData.searchResourceHeader, "ExportColumn", this.exportDocument);

        let groupName={};
      const  exceptionArray=[];
       techSpecList &&techSpecList.map(x=>{
           if(x.supplierInfo){
            x.supplierInfo.map((y)=>{
            
                groupName ={ 
                    "supplierName":y.supplierName,
                "searchExceptionResourceInfos":x.searchExceptionResourceInfos
              };
        exceptionArray.push(groupName);
        });
           }
           
       });
        return (
            <Fragment>
                <CustomModal />
                {this.state.showGoogleMap && <Modal title={localConstant.commonConstants.VIEW_MAP}
                        titleClassName="viewGoogleMap"
                        modalContentClass="extranetModalContent"
                        modalId="googlePopup"
                        formId="googleForm"
                        buttons={this.googleMapCloseButton}
                        isShowModal={true}
                        isShowButtonHeader={true}
                        isCloseIcon={true}
                        isDefaultColseIcon={true}
                        isDisableDrag={true}> 
                                                    
                        <DirectionsGoogleMap mapData={techSpecList} />
                    </Modal>
               }
               {this.state.showExceptionList && 
                <Modal title={' Search Exception List'}
                    buttons={this.exceptionListloseButton}
                    isShowModal={this.state.showExceptionList}
                    modalClass="modalSearchException"
                >
                   
                    <ReactGrid gridColData={this.headerData.exeptionSearchResourceHeader}
                     gridRowData={exceptionArray}
                     groupName={this.exceptiongGoupingParam.groupName} 
                     dataName={this.exceptiongGoupingParam.dataName} 
                     isGrouping={true} />
                </Modal>
               }
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
                {this.state.isSubSupplierModalOpen ?
                    <Modal title={localConstant.resourceSearch.SUB_SUPPLIER} modalId="subSupplierPopup" formId="subSupplierForm" modalClass="popup-position" onSubmit={(e)=>this.submitSubSupplier(e)} buttons={this.subSupplierButtons} isShowModal={this.state.isSubSupplierModalOpen}>
                        <SubSupplierModalPopup 
                            inputChangeHandler = {this.subSupplierOnchangeHandler}
                            subSupplierData =  {this.editedRow}/>
                    </Modal>:null
                }
                <ResourceSearchSaveBar
                    currentMenu={localConstant.resourceSearch.RESOURCE}
                    currentSubMenu={headingSub?headingSub:localConstant.resourceSearch.PRE_ASSIGNMENT}
                    isbtnDisable={this.props.isbtnDisable }
                    buttons={[
                            {
                                name: localConstant.commonConstants.SAVE,
                                clickHandler: () => this.savePreAssignment,
                                className: "btn-small mr-0 ml-2",
                              
                                showBtn: true,
                                // isDisabled:this.props.isPreAssignmentWon
                                isDisabled:this.props.isTechSpecDataChanged//D576

                            },
                            {
                                name: localConstant.commonConstants.CANCEL,
                                clickHandler: () => this.cancelPreAssignmentHandler,
                                className: "btn-small mr-0 ml-2",
                                showBtn:true,
                                type:"button",
                                // isDisabled:this.props.isPreAssignmentWon //D-717 IGO QA   //C  
                                isDisabled:this.props.isTechSpecDataChanged //D576         
                            }
                        ]}
                    childComponents = {
                        <ActionSearch 
                            actionList = { this.preAssignmentActions }
                            dispositionTypeList={ dispositionType }
                            currentPage = { currentPage }
                            isPreAssignmentPage = { true }
                            actionData = {preAssignmentDetails}
                            inputChangeHandler={(e) => this.actionOnchangeHandler(e)}
                            // interactionMode={this.props.isPreAssignmentWon}  //D-717 IGO QA
                            defaultActionValue={preAssignmentDetails.searchAction} //D662-#6 (refering 17-01-2020 ALM Doc) --//Scenario Def 129(02-04-2020)
                            /> 
                    }
                />
                <div className= { this.state.actionDivDynamicClass ? "customCard searchParams quickSearchActionTopMarginUpdate" : "customCard quickSearchTopSectionMargin searchParams " + actionDivClass }>
                    {/* <form autoComplete="off"> */}
                       <CardPanel className="white lighten-4 black-text" colSize="s12 pl-0 pr-0 mb-2">
                        <PreAssignmentDiv
                            inputChangeHandler={(e) => this.inputChangeHandler(e)}
                          //  techSpecSearch={this.preAssignmentSearch}
                            companyList = { sortedCompanyList }
                            ocCompanyList = { companyList && arrayUtil.sort(companyList,"companyName","asc") }
                            customerList = { customerList }
                            contractHoldingCoodinatorList = { preAssignmentMICHCoordinatorList && arrayUtil.sort(preAssignmentMICHCoordinatorList,'displayName','asc') }
                            operatingCoordinatorList = { preAssignmentMIOPCoordinatorList && arrayUtil.sort(preAssignmentMIOPCoordinatorList,'displayName','asc') } 
                            searchParameter = {searchParameters} 
                            defaultCustomerName = {this.props.defaultCustomerName}
                            interactionMode={this.props.isPreAssignmentWon || isOperatingCompany }  //D-717 IGO QA
                            customerChangeHandler={(e) => this.customerChangeHandler(e) } //D576
                            />  
                        </CardPanel>
                        <Panel colSize="s12" heading={ localConstant.resourceSearch.MORE_DETAILS } isArrow={true} onpanelClick={(e) => this.panelClick('MoreDetails')} isopen={this.state.isPanelOpenMoreDetails} >
                            <MoreDetails
                                gridData={subSupplierList}
                                gridRef={ref => { this.child = ref; }}
                                gridHeaderData={this.headerData.supplierHeader}
                                moreDetailsData={searchParameters}
                                moreDetailsParam={preAssignmentProperty}
                                isSubSupplierGrid={ !isInterCompanyPreAssignment } //def 662 changes as in GRM
                                subSupplierGridBtn={this.subSupplierGridBtn}
                                buttons={this.preAssignmentSearchBtn}
                                currentPage={currentPage}
                                isPreAssignmentPage = {true}
                                inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                firstVisitFrom={this.firstVisitFrom}
                                firstVisitTo={this.firstVisitTo}
                                interactionMode={this.props.isPreAssignmentWon || isOperatingCompany }    //D-717 IGO QA
                                />
                                 
                            {preAssignmentDetails.id ? 
                                <ResourceAssigned 
                                    assignedResourceData = {assignedResources}
                                    interactionMode = {true} /> 
                                : null}
                        </Panel>
                        <Panel colSize="s12 pl-0 pr-0 mb-0" heading={ SearchEvolutionHeader } isArrow={true} onpanelClick={(e) => this.panelClick('OptionalSearch')} isopen={ this.state.isPanelOpenOptionalSearch } >
                        <OptionalSearch
                            buttons={this.optionalSearchGrm}                        
                            optionalParamLabelData={searchParameters}
                            inputChangeHandler={(e) => this.optionalOnchangeHandler(e)}
                            multiSelectOptionsList={languages}
                            certificatesMultiSelectOptionsList={certificates}
                            equipment={equipment}
                            defaultMultiSelection={[]}
                            defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
                            defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                            defaultLanguageSpeakingMultiSelection={defaultLanguageSpeakingMultiSelection}
                            defaultLanguageWritingMultiSelection={defaultLanguageWritingMultiSelection}
                            defaultLanguageComprehensionMultiSelection={defaultLanguageComprehensionMultiSelection}
                            optionAttributs={this.state.optionAttributs}
                            equipmentDescriptionMultiselectValueChange={(value)=> this.equipmentDescriptionMultiselect(value)}
                            certificationMultiSelectValueChange={(value) => this.certificationMultiselect(value)}
                            languageSpeakingMultiSelectValueChange={(value) => this.languageSpeakingMultiselect(value)}
                            languageWritingMultiSelectValueChange={(value) => this.languageWritingMultiselect(value)}
                            languageComprehensionMultiSelectValueChange={(value) => this.languageComprehensionMultiselect(value)}
                            expiryFrom={this.expiryFrom}
                            expiryTo={this.expiryTo}
                            searchGRMData={optionalSearch}
                            searchPanelOpen={this.toggleSlide}
                            isSearchPanelOpen={this.state.isOpensearchSlide} 
                            taxonomyCustomerApproved={taxonomyCustomerApproved}
                            defaultCustomerApprovalMultiSelection={defaultCustomerApprovalMultiSelection}
                            customerApprovalMultiselectValueChange={(value) => this.customerApprovalMultiselect(value)}
                            //isShowMapBtn={ (techSpecList.length  > 0 || this.state.isShowMapBtn) ? true : false}
                            isShowMapBtn={ techSpecList.length  > 0 ? true : false}
                            interactionMode={this.props.isPreAssignmentWon}  //D-717 IGO QA //Removed Operationg Company Check for Interaction Mode ITK D1318 Ref ALM And Email BUG_1318_D1318-Pre-Assignment issues - FAIL - 16-09-2020
                            />
                    </Panel>
                        <Panel colSize="s12" heading={ localConstant.resourceSearch.SERACH_RESULT } isArrow={true} onpanelClick={() => this.panelClick('SearchParams')} isopen={this.state.isPanelOpenSearchParams} >
                            <SearchGRM
                                gridCustomClass={'searchPopupGridHeight'}
                                gridData={techSpecList}
                                gridHeaderData={this.headerData.searchResourceHeader}
                                gridRef={ref => { this.gridChild = ref; }}
                                gridGroupProps = {this.groupingParam}
                                buttons={this.optionalSearchGrm}
                                exportButtons={this.exportGridBtn}
                                optionalParamLabelData={searchParameters}
                                inputChangeHandler={(e) => this.optionalOnchangeHandler(e)}
                                multiSelectOptionsList={languages}
                                certificatesMultiSelectOptionsList={certificates}
                                equipment={equipment}
                                defaultMultiSelection={[]}
                                defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
                                defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                                defaultLanguageSpeakingMultiSelection={defaultLanguageSpeakingMultiSelection}
                                defaultLanguageWritingMultiSelection={defaultLanguageWritingMultiSelection}
                                defaultLanguageComprehensionMultiSelection={defaultLanguageComprehensionMultiSelection}
                                optionAttributs={this.state.optionAttributs}
                                equipmentDescriptionMultiselectValueChange={(value)=> this.equipmentDescriptionMultiselect(value)}
                                certificationMultiSelectValueChange={(value) => this.certificationMultiselect(value)}
                                languageSpeakingMultiSelectValueChange={(value) => this.languageSpeakingMultiselect(value)}
                                languageWritingMultiSelectValueChange={(value) => this.languageWritingMultiselect(value)}
                                languageComprehensionMultiSelectValueChange={(value) => this.languageComprehensionMultiselect(value)}
                                expiryFrom={this.expiryFrom}
                                expiryTo={this.expiryTo}
                                searchGRMData={optionalSearch}
                                searchPanelOpen={this.toggleSlide}
                                isSearchPanelOpen={this.state.isOpensearchSlide} 
                                taxonomyCustomerApproved={taxonomyCustomerApproved}
                                defaultCustomerApprovalMultiSelection={defaultCustomerApprovalMultiSelection}
                                customerApprovalMultiselectValueChange={(value) => this.customerApprovalMultiselect(value)}
                                //isShowMapBtn={ (techSpecList.length  > 0 || this.state.isShowMapBtn) ? true : false}
                                isShowMapBtn={ techSpecList.length  > 0 ? true : false}
                                interactionMode={this.props.isPreAssignmentWon || isOperatingCompany }  //D-717 IGO QA
                                rowSelectedHandler = {this.rowSelectHandler} 
                            />
                        </Panel>
                    {/* </form> */}
                </div>
                {
                    Array.isArray(this.props.techSpecList) && this.props.techSpecList.length > 0 ?
                        <div style={{ display: "none" }}>
                            <Table columnData={this.props.techSpecList} headerData={localConstant.technicalSpecialist} />
                        </div>
                        : null
                }
                {
                    this.state.isSelectionPopUpOpen ?
                        <Modal title={localConstant.resourceSearch.CV_SECTION_MODAL}
                            modalId="SelectionPopUpModal"
                            formId="SelectionPopUpForm"
                            modalClass="selectionPopUpModal"
                            buttons={this.SelectionPopUpButtons}
                            isShowModal={this.state.isSelectionPopUpOpen}>
                            <SelectionPopUpModal 
                            optionList={localConstant.resourceSearch.optionValueList} 
                            headerData={this.headerData.cvOptionsHeader} 
                            gridRef={ref => { this.selectGridChild = ref; }} 
                            />
                        </Modal>
                        : null
                }           
            </Fragment>
        );
    }
}

export default PreAssignment;
