import React, { Component, Fragment } from 'react';
import Panel from '../../../../common/baseComponents/panel';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler,isEmpty, isEmptyReturnDefault,customRegExValidator,bindAction,parseQueryParam, isEmptyOrUndefine,mergeobjects } from '../../../../utils/commonUtils';
import { HeaderData } from './quickSearchHeader';
import Modal from '../../../../common/baseComponents/modal';
import { modalMessageConstant,modalTitleConstant } from '../../../../constants/modalConstants';
import { SearchGRM, OptionalSearch } from '../../../../common/resourceSearch/searchGRM/searchGRM';
import MoreDetails from '../../../../common/resourceSearch/moreDetails';
import ActionSearch from '../../../../common/resourceSearch/actionSearch';
import ResourceSearchSaveBar from '../../applicationComponent/resourceSearchSaveBar';
import CustomerAndCountrySearch from '../../../../components/applicationComponents/customerAndCountrySearch';
import { AppDashBoardRoutes } from '../../../../routes/routesConfig';
import ErrorList from '../../../../common/baseComponents/errorList';
import arrayUtil from '../../../../utils/arrayUtil';
import moment from 'moment';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
import { required } from '../../../../utils/validator';
import  DirectionsGoogleMap from '../../../../common/resourceSearch/googleMap';
import jsPDF from "jspdf";
import 'jspdf-autotable';
import Table from '../../../../common/baseComponents/table';
import ReactGrid from '../../../../common/baseComponents/reactAgGridTwo';

const localConstant = getlocalizeData();
export const QuickSearchDiv = (props) => {
    const { quickSearchDetails } = props;
    return (
        <div className="row mb-0">             
                <CustomerAndCountrySearch
                    customerName = {props.customerName}
                    colSize="col s3 pl-0" 
                    contractCustomerChange={props.customerChangeHandler} //D576
                    />
                <CustomInput
                    hasLabel={true}
                    name="projectName"
                    colSize='s3'
                    label={localConstant.resourceSearch.PROJECT_NAME}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={50}
                    onValueChange={props.inputHandleChange} 
                    value={quickSearchDetails.projectName ? quickSearchDetails.projectName :""}/>
                <CustomInput
                    hasLabel={true}
                    labelClass="mandate"
                    name="chCompanyCode"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.CH_COMPANY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.companyList}
                    optionName='companyName'
                    optionValue='companyCode'
                    onSelectChange={props.inputHandleChange}
                    maxLength={250}
                    defaultValue={quickSearchDetails.chCompanyCode}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="mandate"
                    name="opCompanyCode"
                    colSize='s3 pl-0'
                    label={localConstant.resourceSearch.OPERATING_COMPANY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.ocCompanyList}
                    optionName='companyName'
                    optionValue='companyCode'
                    onSelectChange={props.inputHandleChange}
                    maxLength={250}
                    defaultValue={quickSearchDetails.opCompanyCode}
                />
           
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
class QuickSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            actionDivDynamicClass:false,
            isPanelOpenMoreDetails: false,
            isPanelOpenOptionalSearch:false,
            isPanelOpenSearchParams: true,
            isShowMapBtn:false,
            showGoogleMap:false,
            isSelectionPopUpOpen: false,
            errorList:[],
            multiSelectOptions: [ {
                name: 'a',
                value: 'a'
            },
            {
                name: 'b',
                value: 'c'
            }
            ],
            optionAttributs: {
                optionName: 'name',
                optionValue: 'name'
            },
            expiryFromDate:'',
            expiryToDate:'',
            firstVisitDate:'',
            actionData: { searchAction: "" },
            isOpensearchSlide:false
        };
        this.groupingParam = {
            groupName:"location",
            dataName:"resourceSearchTechspecInfos"
        };
        this.quickSearchActions=[];
        this.errors = [];
        this.updatedData = {
            searchParameter: {
                optionalSearch: {},
            },
            subSupplierInfos: {}
        };
        this.updatedData["searchType"] = 'Quick';

        if (this.props.currentPage === 'Edit')
            this.updatedData["recordStatus"] = 'M';
        else
            this.updatedData["recordStatus"] = 'N';

        this.errorListButton =[
            {
              name: localConstant.commonConstants.OK,
              action: this.closeErrorList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
          this.googleMapCloseButton =[
            {
              name: localConstant.commonConstants.CANCEL,
              action: this.showGoogleMap,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
        //Buttons
        this.searchBtn = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.quickSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        //optionalSearchButtons
        this.optionalSearchGrm = [
            {
                name: localConstant.commonConstants.SEARCH,
                action: this.quickSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.exportGridBtn = [
            // {
            //     name: localConstant.commonConstants.VIEW_MAP,
            //     action: this.showGoogleMap,
            //     btnClass: "btn-flat mr-1 ",
            //     showbtn: true,
            //     disabled:true
            // },
            {
                name: localConstant.commonConstants.EXPORT_TO_STANDARDCV,
                action:this.selectionPopUp,//this.exportToStandardCV, 
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
        functionRefs["getInterCompanyInfo"] = this.getInterCompanyInfo; //def 957  fix
        this.headerData = HeaderData(functionRefs);
        this.exportDocument = this.exportDocument.bind(this);
    }

    getInterCompanyInfo =()=> { 
        const opComp= this.props.quickSearchDetails.searchParameter && this.props.quickSearchDetails.searchParameter.opCompanyCode;
      if( !isEmptyOrUndefine(opComp) && opComp!== this.props.selectedHomeCompany)
      {
          return opComp;
      }
     return null;
    }

    exportDocument = (epin,isChevronExport) => {
        this.props.actions.ExportToCV(epin,isChevronExport);
    } 

    //Panel Click Event Handler
    panelClick = (params) => {
        this.setState({ [`isPanelOpen${ params }`]: !this.state[`isPanelOpen${ params }`]  });
    }

    /** exportToStandardCV button handler */
    showGoogleMap = (e) => {
        e.preventDefault();
        this.setState({ showGoogleMap:!this.state.showGoogleMap });
        //IntertekToaster("Under Construction","warningToast Google Map");
    };

    /** exportToChevronCv button handler */
    exportToChevronCv = (e) => {
        e.preventDefault();
        const exportCVFrom=5;
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
    selectionPopUp = (e) => {
        e.preventDefault();
        const selectedRecords = this.gridChild.gridApi.getSelectedRows();
        if (selectedRecords.length > 0) {
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
    exportToStandardCV =(e) =>{
        e.preventDefault();
        const selectedSections = this.selectGridChild.getSelectedRows();
        const exportCVFrom=5;
        const selectedRecords = this.gridChild.gridApi.getSelectedRows();
        if(selectedRecords && selectedRecords.length > 0){
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
        if (Array.isArray(this.props.quickSearchResults) && this.props.quickSearchResults.length > 0) {
            const doc = new jsPDF('l', 'pt', 'a4');
            const styleDef = { fillColor: "#FFFFFF", textColor: "#1E1E1E", lineWidth: 1 };
            doc.autoTable({ html: "#exportTableId", theme: "grid", styles: styleDef });
            doc.save(localConstant.commonConstants.PDF_FILE_NAME + ".pdf");
        }
    };
    //D576
    customerChangeHandler = (data) => {
        this.props.actions.TechSpechUnSavedData(false); //D576
    }

    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        switch (inputvalue.name) {
            case "projectName":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "chCompanyCode":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                // this.updatedData.searchParameter["chCompanyName"] = e.target.selectedOptions[0].text;
                this.updatedData.searchParameter["chCompanyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                this.updatedData["companyCode"] = inputvalue.value;
                this.updatedData.searchParameter["opCompanyCode"] = inputvalue.value;
                // this.updatedData.searchParameter["opCompanyName"] = e.target.selectedOptions[0].text;
                this.updatedData.searchParameter["opCompanyName"] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                const quickSearchData = Object.assign({},this.props.quickSearchDetails,this.updatedData);
                this.props.actions.updateQuickSearchData(quickSearchData);
                break;
            case "opCompanyCode":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                // this.updatedData["companyCode"] = inputvalue.value;
                this.updatedData.searchParameter["opCompanyName"] =  e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                break;
            case "customerContactPerson":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "customerPhoneNumber1":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "customerPhoneNumber2":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "customerMobileNumber":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "customerContactEmail":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "materialDescription":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "supplier":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "supplierLocation":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "assignmentType":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "firstVisitFromDate":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "firstVisitToDate":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "categoryName":
                this.props.actions.ClearSubCategory();
                this.props.actions.ClearServices();
                if( !isEmptyOrUndefine( inputvalue.value) )
                {
                    this.props.actions.FetchTechSpecSubCategory(inputvalue.value);
                } 
                this.updatedData["categoryName"] = inputvalue.value;
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "subCategoryName":
                this.props.actions.ClearServices();
                if( !isEmptyOrUndefine( inputvalue.value) )
                {
                    this.props.actions.FetchTechSpecServices(this.props.quickSearchDetails.categoryName,inputvalue.value);//def 916 fix
                } 
                this.updatedData["subCategoryName"] = inputvalue.value;
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "serviceName":
                this.updatedData["serviceName"] = inputvalue.value;
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            case "selectedTechSpecEpins":
                this.updatedData.searchParameter[inputvalue.name] = inputvalue.value;
                break;
            // case "equipmentMaterialDescription":
            //     this.updatedData.searchParameter.optionalSearch[inputvalue.name] = inputvalue.value;
            //     break;
            case "certificationExpiryFrom":
                this.updatedData.searchParameter.optionalSearch[inputvalue.name] = inputvalue.value;
                break;
            case "certificationExpiryTo":
                this.updatedData.searchParameter.optionalSearch[inputvalue.name] = inputvalue.value;
                break;
            case "radius":
                this.updatedData.searchParameter.optionalSearch[inputvalue.name] = inputvalue.value;
                break;
            case "searchInProfile":
                this.updatedData.searchParameter.optionalSearch[inputvalue.name] = inputvalue.value;
                break;
            default:
                this.updatedData[inputvalue.name] = inputvalue.value;
        }
        const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
        this.props.actions.updateQuickSearchData(quickSearchData);
    }

    certificationMultiselect = (value) => {
        const certificates = [];
        value.map(certificate => {
            certificates.push(certificate.label);
        });
        this.updatedData.searchParameter.optionalSearch["certification"] = certificates;
        const certificationData={};
        certificationData["certification"]=certificates;
        this.props.actions.AddOptionalSearch(certificationData);
    }

    equipmentDescriptionMultiselect =(value)=>{
        const equipmentsdescription = [];
        value.map(eqipment => {
            equipmentsdescription.push(eqipment.label);
        });
        this.updatedData.searchParameter.optionalSearch["equipmentMaterialDescription"] = equipmentsdescription;
        const equipmentData={};
        equipmentData["equipmentMaterialDescription"]=equipmentsdescription;
        this.props.actions.AddOptionalSearch(equipmentData);
    }

    customerApprovalMultiselect =(value)=>{
        const customerApproval = [];
        value.map(customer => {
            customerApproval.push(customer.label);
        });
        this.updatedData.searchParameter.optionalSearch["customerApproval"] = customerApproval;
        const customerData={};
        customerData["customerApproval"]=customerApproval;
        this.props.actions.AddOptionalSearch(customerData);
    }

    languageSpeakingMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData.searchParameter.optionalSearch["languageSpeaking"] = languages;
        const languageData={};
        languageData["languageSpeaking"]=languages;
        this.props.actions.AddOptionalSearch(languageData);
    }
    languageWritingMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData.searchParameter.optionalSearch["languageWriting"] = languages;
        const languageData={};
        languageData["languageWriting"]=languages;
        this.props.actions.AddOptionalSearch(languageData);
    }
    languageComprehensionMultiselect = (value) => {
        const languages = [];
        value.map(language => {
            languages.push(language.label);
        });
        this.updatedData.searchParameter.optionalSearch["languageComprehension"] = languages;
        const languageData={};
        languageData["languageComprehension"]=languages;
        this.props.actions.AddOptionalSearch(languageData);
    }
    //Form Search 
    quickSearch = (e) => {
        e.preventDefault();
        if (this.props.selectedCustomerData.length > 0) {
            this.updatedData["customerName"] = this.props.selectedCustomerData[0].customerName;
            this.updatedData["customerCode"] = this.props.selectedCustomerData[0].customerCode;
            this.updatedData.searchParameter["customerCode"] = this.props.selectedCustomerData[0].customerCode;
            this.updatedData.searchParameter["customerName"] = this.props.selectedCustomerData[0].customerName;
        }
        const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
        this.props.actions.updateQuickSearchData(quickSearchData);
        const valid=this.mandatoryFieldsValidationCheckForSearch(quickSearchData);
        if (valid)
        this.props.actions.searchDetails(quickSearchData); //Changes For Sanity Def 146
        //this.setState({ isShowMapBtn:true });     
        // this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
    }

        /**Email validation */
    emailValidation(value) {
            if ((value) &&
                customRegExValidator(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/, 'i', value)) {
                IntertekToaster(localConstant.techSpec.contactInformation.EMAIL_VALIDATION, 'warningToast');
                return true;
            }
    }
    ConvertToselectedTechSpecJSON() {
        const selectedRecords = this.gridChild.gridApi.getSelectedNodes();
        const finalResult = [];
        let selectedRecordsObject = {
            location: "",
            techSpecialist: []
        };
        const techSpecialistObj = {};
        let selectedRecordIndex = 0;
        selectedRecords.map(selectedRecord => {
            if (this.gridChild.gridApi.getSelectedNodes()[selectedRecordIndex].parent.data) {
                if (selectedRecordIndex === 0 || this.gridChild.gridApi.getSelectedNodes()[selectedRecordIndex].parent.data.location !== this.gridChild.gridApi.getSelectedNodes()[selectedRecordIndex - 1].parent.data.location) {
                    if(selectedRecordIndex !== 0){
                        finalResult.push(selectedRecordsObject);
                    }
                    selectedRecordsObject = {
                        location: "",
                        techSpecialist: []
                    };
                    selectedRecordsObject.location = this.gridChild.gridApi.getSelectedNodes()[selectedRecordIndex].parent.data.location;
                }
                techSpecialistObj["epin"] = selectedRecord.data.epin;
                techSpecialistObj["lastName"] = selectedRecord.data.lastName;
                techSpecialistObj["firstName"] = selectedRecord.data.firstName;
                selectedRecordsObject.techSpecialist.push(techSpecialistObj);
            }
            selectedRecordIndex++;
        });
        finalResult.push(selectedRecordsObject);

    }

    //Form Save 
    quickSearchSave = (e) => {
        e.preventDefault();
        const selectedRecords = this.gridChild.getSelectedRows();

        //TO-DO for Json formation
        // const selectedTechSpecJson=this.ConvertToselectedTechSpecJSON();
        if (this.props.selectedCustomerData.length>0) {
            this.updatedData.customerName = this.props.selectedCustomerData[0].customerName;
            this.updatedData["customerCode"] = this.props.selectedCustomerData[0].customerCode;
            this.updatedData.searchParameter["customerCode"] = this.props.selectedCustomerData[0].customerCode;
            this.updatedData.searchParameter["customerName"] = this.props.selectedCustomerData[0].customerName;
        }
        const selectedTechSpecEpins= [];
        selectedRecords.map(selectedRecord => {
            selectedTechSpecEpins.push({
                "epin": selectedRecord.epin,
                "lastName": selectedRecord.lastName,
                "firstName": selectedRecord.firstName
            });
        });
        this.updatedData.searchParameter["selectedTechSpecInfo"]=selectedTechSpecEpins;
        const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
        this.props.actions.updateQuickSearchData(quickSearchData);

        let valid = this.mandatoryFieldsValidationCheckForSearch(quickSearchData);
        if(required(this.props.quickSearchDetails.searchAction) && this.props.quickSearchDetails.id){ //id check added for sanity Def 199
            IntertekToaster(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.ACTION }`,'warningToast actionVal');
            valid = false;
        }
        if(this.props.quickSearchDetails && this.props.quickSearchDetails.searchAction === "L"){ //Fixes for Scenario failed by SMN QA
            if(required(this.props.quickSearchDetails.dispositionType)){
                IntertekToaster(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.DISPOSITION_DETAILS }`,'warningToast dispositionType');
                valid = false;
            }
        }
        if (valid) {
            if (!this.props.quickSearchDetails.id) {
                this.props.actions.saveQuickSearchDetails(this.updatedData).then(res => {
                    if (res) {
                        this.props.actions.ClearMySearchData();
                        this.props.history.push(AppDashBoardRoutes.mysearch);
                    }
                });
            }
            else {
                this.props.actions.updateQuickSearchDetails(this.updatedData).then(res => {
                    if (res) {
                        this.props.actions.ClearMySearchData();
                        this.props.history.push(AppDashBoardRoutes.mysearch);
                    }
                });
            }
        }
    }

    mandatoryFieldsValidationCheckForSearch = (quickSearchData) => {
        if (quickSearchData && quickSearchData.searchParameter) {
            // if (isEmpty(quickSearchData.customerCode)) {
            //     this.errors.push(`${ localConstant.resourceSearch.QUICK_SEARCH } - ${ localConstant.resourceSearch.CUSTOMER }`);
            // }
            if (isEmpty(quickSearchData.searchParameter.chCompanyCode)) {
                this.errors.push(`${ localConstant.validationMessage.PA_CH_COMPANY_VALIDATION }`);
            }
            if (isEmpty(quickSearchData.searchParameter.opCompanyCode)) {
                this.errors.push(`${ localConstant.validationMessage.PA_OP_COMPANY_VALIDATION }`);
            }
            // if (isEmpty(quickSearchData.searchParameter.assignmentType)) {
            //     this.errors.push(`${ localConstant.resourceSearch.QUICK_SEARCH } - ${ localConstant.resourceSearch.ASSIGNMENT_TYPE }`);
            // }

            if (this.errors.length > 0) {
                this.setState({
                    errorList: this.errors
                });
                return false;
            }
            else{
                if (!isEmpty(quickSearchData.searchParameter.optionalSearch.certificationExpiryTo)) {
                if(!moment(quickSearchData.searchParameter.optionalSearch.certificationExpiryTo).isAfter((quickSearchData.searchParameter.optionalSearch.certificationExpiryFrom),'day')){
                    if(!moment(quickSearchData.searchParameter.optionalSearch.certificationExpiryFrom).isSame(moment(quickSearchData.searchParameter.optionalSearch.certificationExpiryTo), 'day')){
                        IntertekToaster(localConstant.validationMessage.EXPIRY_TO_DATE_SHOULD_NOT_LESS_THAN_EXPIRY_FROM_DATE, 'warningToast gdEndDateValCheck');
                        return false;
                    }
                }
            }   
            
            if (!required(quickSearchData.searchParameter.customerContactEmail)){
                if(this.emailValidation(quickSearchData.searchParameter.customerContactEmail)){
                 return false;
                }  
             }
            }
                return true;
        }
        return false;
    }

    // mandatoryFieldsValidationCheck = () => {
    //     if (this.props.quickSearchDetails && this.props.quickSearchDetails.searchParameter) {
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.customerCode)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.CUSTOMER }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.chCompanyCode)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.CH_COMPANY }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.opCompanyCode)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.OPERATING_COMPANY }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.supplier)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.SUPPLIER }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.supplierLocation)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.SUPPLIER_LOCATION }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.assignmentType)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.ASSIGNMENT_TYPE }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.categoryName)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.CATEGORY }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.subCategoryName)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.SUB_CATEGORY }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchParameter.serviceName)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.SERVICES }`);
    //         }
    //         if (isEmpty(this.props.quickSearchDetails.searchAction)) {
    //             this.errors.push(`${ localConstant.resourceSearch.RESOURCE_SEARCH } - ${ localConstant.resourceSearch.ACTION }`);
    //         }

    //         if(this.errors.length > 0){
    //             this.setState({
    //               errorList:this.errors
    //             });
    //             return false;
    //           }
    //           else
    //              return true;
    //     }
    //     return false;
    // }
    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
        this.errors = [];
    }
    OptionalSearchFilter = (e) => {
        e.preventDefault();
    }
    componentDidMount() {
        const result = this.props.location.search && parseQueryParam(this.props.location.search);
        const queryObj={ id:parseInt(result.preID) };  
        if(result.searchType === "Quick"){              
            this.props.actions.FetchQuickSearchData(queryObj);
        } 
            /**
         * this.props.isGrmMasterDataFeteched is bool flag which will be set as true once the
         * GRM master data is loaded. For subsequent GRM related menus we check for this flag to load master data
         * If user goes out of GRM/Resource module NEED set the flag to false to reload masterdata 
         */ 

        /** commented for PT Failure 
        // if(this.props.isGrmMasterDataFeteched === false){     
        //     this.props.actions.grmDetailsMasterData();
        // }
        // this.props.actions.grmFetchCustomerData();
        // this.props.actions.FetchAssignmentType();
        */
        this.props.actions.FetchTechSpecCategory();
        this.props.actions.FetchTaxonomyCustomerApproved();
        this.props.actions.FetchCertificates();
        this.props.actions.FetchEquipment();
    }
    componentWillUnmount() {
        // this.props.actions.clearCustomerList();
        // this.props.actions.ClearMySearchData();
        this.props.actions.clearAllQuickSearchDetails();
    }
    /** on change handler - for action section */
    actionOnchangeHandler = (e) => {       
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        if (inputValue.name === "searchAction") {
            // this.setState({
            //     actionData: {
            //         searchAction: inputValue.value
            //     }
            // });
            this.props.actions.FetchDispositionType(inputValue.value);
        }
        if(inputValue.name === "searchAction" && inputValue.value === "W"){
            this.updatedData["description"] = ""; //Scenario Defect 106            
        }
        // Ui Aliginment Dynamic Margin Class Added for Action selection Scenario Defect -137
        if(inputValue.name === "searchAction" && inputValue.value === ""){
            this.setState({ actionDivDynamicClass:false });
        }else{
            this.setState({ actionDivDynamicClass:true });
        }
        const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
        this.props.actions.updateQuickSearchData(quickSearchData);
        //this.updatedData = {};
    };

    /**expirt Date Hander */
    expiryFrom = (date) => {
        this.setState({
            expiryFromDate: date
        }, () => {
            this.updatedData.searchParameter.optionalSearch["certificationExpiryFrom"] = this.state.expiryFromDate !== null ? this.state.expiryFromDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
            this.props.actions.updateQuickSearchData(quickSearchData);
        });
    }

    expiryTo = (date) => {
        this.setState({
            expiryToDate: date
        }, () => {
            this.updatedData.searchParameter.optionalSearch["certificationExpiryTo"] = this.state.expiryToDate !== null ? this.state.expiryToDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
            this.props.actions.updateQuickSearchData(quickSearchData);
        });
    }
    
    firstVisitChange = (date) => {
        this.setState({
            firstVisitDate: date
        }, () => {
            this.updatedData.searchParameter["firstVisitFromDate"] = this.state.firstVisitDate !== null ? this.state.firstVisitDate.format(localConstant.techSpec.common.DATE_FORMAT) : "";
            const quickSearchData = mergeobjects(this.props.quickSearchDetails,this.updatedData);
            this.props.actions.updateQuickSearchData(quickSearchData);
        });
    }

    convertToMultiSelectObject = (datas) => {
        const multiselectArray = [];
        if (Array.isArray(datas) ) {
            datas.map(data => {
                multiselectArray.push({ value: data , label: data });
            });
        }
        return multiselectArray;
    }

     /** Cancel Quick Search changes */
     cancelQuickSearchHandler = (e) => {
        e.preventDefault();
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.CANCEL_CHANGES,
            modalClassName: "warningToast",
            type: "confirm",
            buttons: [
              {
                buttonName: "Yes",
                onClickHandler:(e)=> this.cancelQuickSearch(e),
                className: "modal-close m-1 btn-small"
              },
              {
                buttonName: "No",
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

    /** cancel Quick Search changes */
    cancelQuickSearch = (e) => {
        e.preventDefault();
        this.props.actions.ClearMySearchData();
        this.updatedData = {
            searchParameter: {
                optionalSearch: {},
            },
            subSupplierInfos: {},
            searchType:'Quick',//sanity difect fix
        };
            if(this.props.quickSearchDetails.id){
                this.props.actions.CancelEditQuickSearchDetails();
            } else {
                this.updatedData["recordStatus"] = 'N';
                this.props.actions.CancelCreateQuickSearchDetails();
            }
        this.props.actions.FetchMySearchData();
        this.props.actions.HideModal();
    };
    toggleSlide=(e)=>{
     this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
    }

    rowSelectHandler = (e) => {
        if(e.data && e.data.epin){
            const searchResource =  isEmptyReturnDefault(this.props.quickSearchResults);
            if(searchResource.length > 0)
                searchResource.forEach(iteratedValue => {
                        const resourceIndex = iteratedValue.resourceSearchTechspecInfos && iteratedValue.resourceSearchTechspecInfos.findIndex(x => x.epin === e.data.epin);
                        if(resourceIndex > -1){
                            iteratedValue.resourceSearchTechspecInfos[resourceIndex].isSelected = e.node && e.node.selected;
                        }
                });
        }
    }
    render() {
        const { currentPage, companyList, userRoleCompanyList, customerList, quickSearchDetails,defaultCustomerName, quickSearchResults, languages, certificates, dispositionType,equipment,taxonomyCustomerApproved } = this.props;
        const sortedCompanyList = arrayUtil.sort(userRoleCompanyList, 'companyName', 'asc');
        const sortedcertificates=arrayUtil.sort(certificates,'name','asc');
        const sortedLanguages=arrayUtil.sort(languages,'name','asc');
        const quickSearchParameters = isEmptyReturnDefault(quickSearchDetails.searchParameter, 'object');
        const optionalSearch = isEmptyReturnDefault(quickSearchParameters.optionalSearch, 'object');        
        const quickSearchAction = localConstant.resourceSearch.quickSearchAction;
        // const quickSearchAction = localConstant.resourceSearch.quickSearchAction;
        const techSpecs = isEmptyReturnDefault(quickSearchParameters.selectedTechSpecInfo); 
        //this.headerData = HeaderData(techSpecs);
        let defaultActionValue="";

        let headingSub = "";
        if(quickSearchParameters.chCompanyCode && quickSearchParameters.opCompanyCode){
            if(quickSearchParameters.chCompanyCode === quickSearchParameters.opCompanyCode)
                headingSub = localConstant.resourceSearch.DOMESTIC;
            else
                headingSub = localConstant.resourceSearch.INTERCOMPANY;
        }
        const HideValues=[];
        if (this.props.currentPage === 'quickSearch') {
            HideValues.push("W");
            HideValues.push("L");
            defaultActionValue="SS";
            this.updatedData["searchAction"]="SS";
        }
        else {
            HideValues.push("SS");
            /** Commented for D128 Checkbox selection handel by backend common to all searches */
            //this.updatedData = quickSearchDetails;
            // if (quickSearchResults) {
            //     for(let j=0;j<quickSearchResults.length;j++) {
            //         if (quickSearchResults[j].resourceSearchTechspecInfos) {
            //             for (let i = 0; i < quickSearchResults[j].resourceSearchTechspecInfos.length; i++) {
            //                 if (techSpecs) {
            //                     techSpecs.map(techSpec => {
            //                         if (techSpec.epin === quickSearchResults[j].resourceSearchTechspecInfos[i].epin) {
            //                             quickSearchResults[j].resourceSearchTechspecInfos[i]["isSelected"] = true;
            //                         }
            //                     });
            //                 }
            //             }
            //         }
            //     };
            // }
        }

        this.quickSearchActions = arrayUtil.negateFilter(localConstant.resourceSearch.quickSearchAction, 'value', HideValues);
        //Default values for Optional Param Multi select
        const defaultCustomerApprovalMultiSelection=this.convertToMultiSelectObject(optionalSearch.customerApproval);
        const defaultEquipmentMultiSelection =this.convertToMultiSelectObject(optionalSearch.equipmentMaterialDescription);
        const defaultCertificateMultiSelection =this.convertToMultiSelectObject(optionalSearch.certification);
        const defaultLanguageSpeakingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageSpeaking);
        const defaultLanguageWritingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageWriting);
        const defaultLanguageComprehensionMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageComprehension);
        const SearchEvolutionHeader=<div>Search(Optional Parameters)</div>;
        const quickSearchDivClass = this.props.quickSearchDetails.id ? "quickSearchTopSectionMargin" : "topSectionMargin";
        const actionDivClass=!required(this.state.actionData.searchAction) ? " quickSearchMarginTop" :" topSectionMargin";

        // bindAction(this.headerData.searchResourceHeader, "ExportColumn", this.exportDocument);

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
                        isDisableDrag={true}>  
                        <DirectionsGoogleMap mapData={quickSearchResults} />
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
                <ResourceSearchSaveBar
                    currentMenu={localConstant.resourceSearch.RESOURCE}
                    currentSubMenu={headingSub?headingSub:localConstant.resourceSearch.QUICK_SEARCH}
                    isbtnDisable={this.props.isbtnDisable}
                    divClassName ={this.props.quickSearchDetails.id ? "quickSearchTopSectionActionMarginOn" : "quickSearchTopSectionActionOff"}
                    buttons={
                        [ {
                            name: localConstant.commonConstants.SAVE,
                            clickHandler: () => this.quickSearchSave,
                            className: "btn-small mr-0 ml-2",
                            showBtn: true,
                            isDisabled:this.props.isTechSpecDataChanged //D576
                        },
                        {
                            name: localConstant.commonConstants.CANCEL,
                            clickHandler: () => this.cancelQuickSearchHandler,
                            className: "btn-small mr-0 ml-2",
                            showBtn: true,
                            type:"button",
                            isDisabled:this.props.isTechSpecDataChanged //D576
                        } ]
                    }
                    childComponents = {
                        this.props.quickSearchDetails.id ?
                            // <CardPanel className="white lighten-4 black-text" colSize="s12 pl-0 pr-0">    
                                <ActionSearch  
                                    actionList = { this.quickSearchActions }
                                    multiSelectOptionsList={this.state.multiSelectOptions}
                                    defaultMultiSelection={this.state.multiSelectdefaultOptions} 
                                    dispositionTypeList={dispositionType}
                                    isQuickSearch={true}
                                    multiSelectValueChange={(value) => this.multiselect(value)}
                                    inputChangeHandler={(e) => this.actionOnchangeHandler(e)}
                                    actionData={quickSearchDetails}
                                    defaultActionValue={quickSearchDetails.searchAction} />
                            // </CardPanel>
                        : null
                    }
                />
                <div className = { this.state.actionDivDynamicClass ? "customCard searchParams quickSearchActionTopMarginUpdate" : "customCard searchParams " + quickSearchDivClass + actionDivClass }>
                    {/* <form autoComplete="off"> */}
                        <CardPanel className="white lighten-4 black-text" colSize="s12 pl-0 pr-0 mb-2">
                            <QuickSearchDiv
                                inputHandleChange={(e) => this.inputHandleChange(e)}
                                companyList={sortedCompanyList}
                                ocCompanyList = {arrayUtil.sort(companyList,"companyName","asc")}
                                customerList={customerList}
                                quickSearchDetails={quickSearchParameters}
                                customerName={defaultCustomerName}
                                customerChangeHandler={(e) => this.customerChangeHandler(e) } //D576
                                 />
                        </CardPanel>
                        <Panel colSize="s12" heading={ localConstant.resourceSearch.MORE_DETAILS }  isArrow={true} onpanelClick={() => this.panelClick('MoreDetails')} isopen={this.state.isPanelOpenMoreDetails} >
                            <MoreDetails 
                                isSubSupplierGrid={false} 
                                buttons={this.searchBtn}
                                isQuickSearch={true} 
                                inputChangeHandler={(e) => this.inputHandleChange(e)}
                                onClick={(e) => this.quickSearch(e)} 
                                moreDetailsData={quickSearchParameters} 
                                // interactionMode={true}
                                firstVisitChange={this.firstVisitChange}
                               />
                        </Panel>
                        <Panel colSize="s12" heading={SearchEvolutionHeader} isArrow={true} onpanelClick={() => this.panelClick('OptionalSearch')} isopen={this.state.isPanelOpenOptionalSearch} >
                        <OptionalSearch
                                buttons={this.optionalSearchGrm}                                
                                optionalParamLabelData = {quickSearchParameters}                               
                                isShowMapBtn={this.state.isShowMapBtn}
                                inputChangeHandler={(e) => this.inputHandleChange(e)} 
                                multiSelectOptionsList={sortedLanguages}
                                certificatesMultiSelectOptionsList={sortedcertificates}
                                equipment={equipment}
                                defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                                defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
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
                                customerApprovalMultiselectValueChange={(value) => this.customerApprovalMultiselect(value)}/>
                        </Panel> 
                        <Panel colSize="s12" heading={localConstant.resourceSearch.SERACH_RESULT} isArrow={true} onpanelClick={() => this.panelClick('SearchParams')} isopen={this.state.isPanelOpenSearchParams} >
                            <SearchGRM 
                                gridCustomClass={'searchPopupGridHeight'}
                                buttons={this.optionalSearchGrm} 
                                gridData={quickSearchResults}
                                gridHeaderData={this.headerData.searchResourceHeader}
                                gridRef={ref => { this.gridChild = ref; }}
                                gridGroupProps = {this.groupingParam}
                                optionalParamLabelData = {quickSearchParameters}
                                exportButtons={this.exportGridBtn}
                                isShowMapBtn={this.state.isShowMapBtn}
                                inputChangeHandler={(e) => this.inputHandleChange(e)} 
                                multiSelectOptionsList={sortedLanguages}
                                certificatesMultiSelectOptionsList={sortedcertificates}
                                equipment={equipment}
                                defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                                defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
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
                                rowSelectedHandler = {this.rowSelectHandler} />
                        </Panel> 
                    {/* </form> */}
                </div>
                {
                    Array.isArray(this.props.quickSearchResults) && this.props.quickSearchResults.length > 0 ?
                        <div style={{ display: "none" }}>
                            <Table columnData={this.props.quickSearchResults} headerData={localConstant.technicalSpecialist} />
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

export default QuickSearch;
