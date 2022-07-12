import React, { Component, Fragment } from 'react';
import Panel from '../../../../common/baseComponents/panel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,formInputChangeHandler, isEmptyReturnDefault, isEmpty,addElementToArray,isEmptyOrUndefine,bindAction, deepCopy, isUndefined } from '../../../../utils/commonUtils';
import { HeaderData } from './arsSearchHeader';
import { SearchGRM, OptionalSearch } from '../../../../common/resourceSearch/searchGRM/searchGRM';
import MoreDetails from '../../../../common/resourceSearch/moreDetails';
import ActionSearch from '../../../../common/resourceSearch/actionSearch';
import SubSupplierDetails from '../../../../common/resourceSearch/subSupplierDetails';
import AssignedResource from '../../../../common/resourceSearch/subSupplierDetails';
import { SupplierInfoDiv,
    AssignMentType,
    CategoryInfoDiv,
    FirstVisit,
    ResourceAssigned,
    PLOSearchGrm,
    Btn } from '../../../../common/resourceSearch/resourceFields';
import { required } from '../../../../utils/validator';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import arrayUtil from '../../../../utils/arrayUtil';
import Modal from '../../../../common/baseComponents/modal';
import ReactGrid from '../../../../common/baseComponents/reactAgGridTwo';
import GridView from '../../../../common/baseComponents/reactAgGrid';
import  DirectionsGoogleMap from '../../../../common/resourceSearch/googleMap';
import jsPDF from "jspdf";
import 'jspdf-autotable';
import Table from '../../../../common/baseComponents/table';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';

const localConstant = getlocalizeData();
export const ArsSearchDiv = (props) => {
    const { preAssignmentIds, isDashboardARSView, preAssignmentSearchModalOpen, hideModal, getPreAssignmentName, 
        btnSearchPreAssignmentPopup, preAssignmentSearchHeader, preAssignmentID, onRef, handleAllCoOrdinator  } = props;
    return(         
        <div className="row mb-2">
        <div className="col s12 pr-0 pl-0">
                { isDashboardARSView ? null :         
                    <Fragment>
                        <Modal id="preAssignmentModalPopup"
                            title={'PreAssignment list '}
                            modalClass="contractModal"
                            buttons={[
                                {
                                    name: 'Cancel', 
                                    action: hideModal,
                                    type: "reset",
                                    btnClass: 'btn-small mr-2',
                                    showbtn: true
                                },
                                {
                                    name: 'Submit',
                                    type: "submit",
                                    action: getPreAssignmentName,
                                    btnClass: 'btn-small',
                                    showbtn: true
                                }
                            ]}
                            isShowModal={preAssignmentSearchModalOpen} >
                            <ReactGrid gridRowData={preAssignmentIds}
                                gridColData={preAssignmentSearchHeader} onRef={onRef} />
                            <label>
                                <input className='filled-in' type="checkbox" onClick={handleAllCoOrdinator} ref={props.allCoordinatorCheck}/>
                                <span>{localConstant.commonConstants.ALL_COORDINATORS_ASSIGNMENTS}</span>
                            </label>
                        </Modal>
                        <div className="col s3 pl-0" >
                            <CustomInput
                                dataValType='valueText'
                                hasLabel={true}
                                divClassName='col customerSearchBox'
                                label={localConstant.resourceSearch.PRE_ASSIGNMENT_ID}
                                labelClass={""}
                                type='text'
                                colSize='s11 pr-0'
                                name='id'
                                inputClass="customInputs"
                                maxLength={200}
                                value={ preAssignmentID ? preAssignmentID : ''}
                                autocomplete="off"
                                disabled={true} />
                            <div className="customerSearchButton">
                                <button type="button" ref={btnSearchPreAssignmentPopup}  className="waves-effect waves-green btn p-2 btn-lineHeight" onClick={props.selectPreAssignmnetSearch} >...</button>
                            </div>
                        </div>
                    </Fragment>
                }
            </div>
            { props.isResourceNotMatched ?
                <ResourceAssigned
                    assignedResourceData={props.assignedResourceData}
                    interactionMode={true}
                    isResourceNotMatched={props.isResourceNotMatched} 
                    isDashboardARSView={props.isDashboardARSView} />
            : null} 
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
class ArsSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpenMoreDetails: false,
            isPanelOpenActionDetails: false, //As per Francina disscued, Action panel should be close In ARSPOPUP
            isPanelOpenSearchParams:true,
            isPanelOpenOptionalSearch:false,
            isPanelOpenSubSupplier:false,
            isPanelOpenAssignedResource:false,
            isPanelOpenARSviewValidate:false,
            isPanelOpenGrmSearch:false,
            preAssignmentSearchModalOpen:false,
            isPloResourceSelected:false,
            windowRestoreGoogleMap:false,
            windowRestoreARS:false, //For ARSPOPUP Maximize
            optionAttributs: {
                optionName: 'name',
                optionValue: 'name'
            },
            techSpecOptionAttributes:{
                optionName: 'fullName',
                optionValue: 'epin'
            },
            expiryFromDate:'',
            expiryToDate:'',
            isOpensearchSlide:false,
            showGoogleMap:false,
            isShowMapBtn:false,
            showExceptionList:false,
            showCommentsReport:false,
            isSelectionPopUpOpen:false,
        };
        this.groupingParam = {
            groupName:"location",
            dataName:"resourceSearchTechspecInfos"
        };
        this.exceptiongGoupingParam = {
            groupName:"supplierName",
             dataName:"searchExceptionResourceInfos"
         };
        this.updatedData = {}; 
        this.techSpecToDelete = [];
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
        this.googleMapCloseButton =[
            {
              name:'WindowRestore',            
              action: this.windowRestoreGoogleMap,
              btnID: "minMaxIconList",
              btnClass: "zmdi-window-restore minMaxIcon",
              showbtn: true,             
            },
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.showGoogleMap,
                btnID: "closeErrorList",
                btnClass: "zmdi-close closeIcon",
                showbtn: true,              
              }
          ];
        this.exceptionListloseButton =[
            {
              name: localConstant.commonConstants.CANCEL,
              action: this.closeExceptionList,
              btnID: "closeErrorList",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
              showbtn: true
            }
          ];
        this.exportGridBtn = [
            {
                name: localConstant.commonConstants.VIEW_MAP,
                action: this.showGoogleMap,
                btnClass: "btn-flat mr-1 ",
                showbtn: true,
                disabled:true
            },
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
                action: this.resourceSearch,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.btnSearchPreAssignmentPopup= React.createRef();
        this.allCoordinatorCheck=React.createRef();
        
        this.exportGridBtn = [
            {
                name: ' Search Exception List',
                action: this.showExceptionList,
                btnClass: "btn-flat mr-1 ",
                showbtn: true,
                disabled:false
            }  ,
            {
                name: localConstant.commonConstants.VIEW_MAP,
                action: this.showGoogleMap,
                btnClass: "btn-flat mr-1 ",
                showbtn: true,
                disabled:true
            },
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
        this.commentsReportButtons =[
            {
              name: localConstant.commonConstants.CANCEL,
              action: this.closeCommentsHistoryReport,
              btnID: "commentReportCancelBtn",
              btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
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
        functionRefs["disableEdit"] = this.disableEdit; 
        functionRefs["getInterCompanyInfo"] = this.getInterCompanyInfo;//def 957 fix
        this.headerData = HeaderData(functionRefs); 
        this.exportDocument = this.exportDocument.bind(this);
        this.techSpecValues = [];
        this.techSpec = [];
    } 

    getInterCompanyInfo =()=> {   
        if(!isEmptyOrUndefine(this.props.opCompanyCode) && this.props.opCompanyCode !== this.props.selectedHomeCompany)
        { 
            return this.props.opCompanyCode;
        }
        return null;
    }

    exportDocument = (epin,isChevronExport) => {
        this.props.actions.ExportToCV(epin,isChevronExport);
    }
    disableEdit = (e) => {
        return this.props.isARSSearch || this.props.interactionMode;
    }
    selectionPopUp = (e) => {
        e.preventDefault();
        const selectedRecords = this.searchGridChild && this.searchGridChild.getSelectedRows();
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
    /** exportToStandardCV button handler */
    exportToStandardCV = (e) => {
        e.preventDefault();
        const selectedSections = this.selectGridChild.getSelectedRows();
        const exportCVFrom=7;
        const customerCode = this.props.assignmentData.searchParameter.customerCode;
        const projectNumber= this.props.assignmentData.searchParameter.projectNumber;
        const assignmentNumber=this.props.assignmentData.searchParameter.assignmentNumber;
        const selectedRecords = this.searchGridChild && this.searchGridChild.getSelectedRows();
        if(selectedRecords.length > 0){
            if(selectedRecords.length > 5){
                IntertekToaster(localConstant.resourceSearch.RESOURCE_MAXIMUM_SELECTION,"warningToast Google Map");
            } 
            else{
                this.props.actions.ExportToMultiCV(selectedRecords,false,exportCVFrom,customerCode,projectNumber,assignmentNumber,selectedSections);
                IntertekToaster(localConstant.resourceSearch.RESOURCE_DOWNLOAD_MESSAGE,"warningToast Google Map");
            }
        }
        else {
            IntertekToaster(localConstant.resourceSearch.RESOURCE_MINIMUM_SELECTION,"warningToast Google Map");
        }
        this.setState({
            isSelectionPopUpOpen: false
        });
    };

    /** exportToChevronCv button handler */
    exportToChevronCv = (e) => {
        e.preventDefault();
        const exportCVFrom=7;
        const customerCode = this.props.assignmentData.searchParameter.customerCode;
       const projectNumber= this.props.assignmentData.searchParameter.projectNumber;
       const assignmentNumber=this.props.assignmentData.searchParameter.assignmentNumber;
       const selectedRecords = this.searchGridChild && this.searchGridChild.getSelectedRows();
        if(selectedRecords.length > 0){
            if(selectedRecords.length > 5){
                 IntertekToaster(localConstant.resourceSearch.RESOURCE_MAXIMUM_SELECTION,"warningToast Google Map");
            } 
            else{
                this.props.actions.ExportToMultiCV(selectedRecords,true,exportCVFrom,customerCode,projectNumber,assignmentNumber);
                IntertekToaster(localConstant.resourceSearch.RESOURCE_DOWNLOAD_MESSAGE,"warningToast Google Map");
            }
        } 
        
        else {
            IntertekToaster(localConstant.resourceSearch.RESOURCE_MINIMUM_SELECTION,"warningToast Google Map");
        }
    };
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

    showExceptionList = (e) => {
        e.preventDefault();
        this.setState({ showExceptionList: true });
    }
    closeExceptionList = (e) => {
        e.preventDefault();
        this.setState({ showExceptionList: false });
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
    windowRestoreGoogleMap = (e) => {
        e.preventDefault();        
            this.setState({ windowRestoreGoogleMap: !this.state.windowRestoreGoogleMap });      
    };
    //For ARSPOPUP Maximize started
    windowRestoreARS =(e) =>{
        e.preventDefault();
        this.setState({
            windowRestoreARS:!this.state.windowRestoreARS
        });
    };
    //For ARSPOPUP Maximize Ended

    /** Panel click handler */
    panelClick = (params) => {
        this.setState({ [`isPanelOpen${ params }`]: !this.state[`isPanelOpen${ params }`]  });   
    }
    //When open ARS Model popup from Dashboard First Accordion Should be closed.
    //because of Model Spacing issues by default closed First Accordion.
    ARSActionDefaultpanelClick = (panelName,panelvalidateName) => {         
            this.setState({ [`isPanelOpen${ panelName }`]: !this.state[`isPanelOpen${ panelName }`] });
    }

    /** Input change handler - for search parameters */
    inputChangeHandler =(e)=>{
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        this.updatedData = {};
    };
    /** on change handler - for action section */
    actionOnchangeHandler = (e) => {
        const inputValue = formInputChangeHandler(e);
        this.updatedData[inputValue.name] = inputValue.value;
        if(inputValue.name === "searchAction"){
            if (this.props.selectedMyTask.taskType !== "PLO to RC") {
                this.updatedData.dispositionType = "";
            }
            this.props.actions.UpdateActionDetails(this.updatedData);
            if(inputValue.value === "SD" || inputValue.value === "PLO"){
                this.props.actions.FetchDispositionType(inputValue.value);
            }
            if(inputValue.value === "OPR"){
                this.updatedData.assignedToOmLognName = ""; //Changes for D1326
                this.props.actions.OverridenResources([]);
                this.props.actions.UpdatePreAssignmentSearchParam({ ploTaxonomyInfo:null });
                this.props.actions.FetchOperationalManagers(this.props.assignmentData.companyCode);
                this.props.actions.FetchTechnicalSpecialists(this.props.assignmentData.companyCode);
            }
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

    /** Remove duplicate */
    removeDuplicates = (myArr, prop) => {
        return myArr.filter((obj, pos, arr) => {
            return arr.map(mapObj => mapObj[prop]).indexOf(obj[prop]) === pos;
        });
    }

    supplierFirstVisitCheck = (resourceList, firstVisitSupplier) => {
        let isTsSelectedForFirstVisit = false, firstVisitSupplierWithNoTsSelected = "";
        if(Array.isArray(resourceList) && resourceList.length > 0){
            for(let index =0,len=resourceList.length;index<len;index++){
                const eachTs = resourceList[index];
                if(eachTs.supplierLocationId){
                    const supplierId = eachTs.supplierLocationId.split('_')[0];
                    if(supplierId == firstVisitSupplier.supplierId){
                        isTsSelectedForFirstVisit = true;
                        break;
                    }
                }
            };
            firstVisitSupplierWithNoTsSelected = isTsSelectedForFirstVisit?"":firstVisitSupplier.supplierName;
        }
        return firstVisitSupplierWithNoTsSelected;
    };

    supplierSubSupplierTsValidation = (selectedTechSpecs)=>{
        const { assignmentSubsuppliers,assignmentDetail } = this.props;
        const { searchParameter = {} } = this.props.assignmentData;
        
        let firstVisitSupplierWithNoTsSelected = "";
        if(!this.props.isDashboardARSView && Array.isArray(assignmentSubsuppliers) && assignmentSubsuppliers.length > 0){
            const mainSupplierFirstVisitObj = assignmentSubsuppliers.find(obj =>obj.isMainSupplierFirstVisit === true && obj.recordStatus !== "D");
            const subSupplierFirstVisitObj = assignmentSubsuppliers.find(obj =>obj.isSubSupplierFirstVisit === true && obj.recordStatus !== "D"); //Changes for not validating previous supplier
            if(isEmptyOrUndefine(subSupplierFirstVisitObj) && isEmptyOrUndefine(mainSupplierFirstVisitObj)){
                return firstVisitSupplierWithNoTsSelected;
            }
            if (!isEmptyOrUndefine(mainSupplierFirstVisitObj)) {
                const mainSupplierObj = {
                    "supplierId": assignmentDetail.assignmentSupplierId,
                    "supplierName": assignmentDetail.assignmentSupplierName
                };
                let arsData = false;
                if (searchParameter.assignedResourceInfos && searchParameter.assignedResourceInfos !== null) {
                    searchParameter.assignedResourceInfos.forEach(x => {
                        if (x.supplierId == mainSupplierObj.supplierId && x.assignedTechSpec.length > 0) {
                            arsData = true;
                        }
                    });
                    if (!arsData)
                        firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, mainSupplierObj);
                }
                else {
                    firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, mainSupplierObj);
                }
            }

            if(!isEmptyOrUndefine(subSupplierFirstVisitObj)){
                const subSupplierObj = {
                    "supplierId" : subSupplierFirstVisitObj.subSupplierId,
                    "supplierName" : subSupplierFirstVisitObj.subSupplierName
                };
                let arsData = false;
                if (searchParameter.assignedResourceInfos && searchParameter.assignedResourceInfos !== null) {
                    searchParameter.assignedResourceInfos.forEach(x => {
                        if (x.supplierId == subSupplierObj.supplierId && x.assignedTechSpec.length > 0) {
                            arsData = true;
                        }
                    });
                    if (!arsData)
                        firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, subSupplierObj);
                }
                else {
                    firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, subSupplierObj);
                }
            }
        }
        if(this.props.isDashboardARSView && (this.props.assignmentData.searchAction === 'AR' || this.props.assignmentData.searchAction === 'SSSPC')){
            if(searchParameter.firstVisitSupplierId){
                const firstVisitSupplierObj = {};
                if(searchParameter.firstVisitSupplierId == searchParameter.supplierId){
                    firstVisitSupplierObj[ "supplierId" ] = searchParameter.supplierId;
                    firstVisitSupplierObj[ "supplierName" ] = searchParameter.supplier; 
                } else {
                    const { subSupplierInfos = [] } = searchParameter;
                    const firstVisitSubSupplier = subSupplierInfos.find(x => x.subSupplierId == searchParameter.firstVisitSupplierId);
                    if(!isEmptyOrUndefine(firstVisitSubSupplier)){
                        firstVisitSupplierObj[ "supplierId" ] = firstVisitSubSupplier.subSupplierId;
                        firstVisitSupplierObj[ "supplierName" ] = firstVisitSubSupplier.subSupplier; 
                    }
                }
                let arsData = false;
                if (searchParameter.assignedResourceInfos && searchParameter.assignedResourceInfos !== null) {
                    searchParameter.assignedResourceInfos.forEach(x => {
                        if (x.supplierId == firstVisitSupplierObj.supplierId && x.assignedTechSpec.length > 0) {
                            arsData = true;
                        }
                    });
                    if (!arsData)
                        firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, firstVisitSupplierObj);
                }
                else{
                    firstVisitSupplierWithNoTsSelected = this.supplierFirstVisitCheck(selectedTechSpecs, firstVisitSupplierObj);
                }
            }
        }
        return firstVisitSupplierWithNoTsSelected;
    }
    setSelectedTSToSubSupplier = (selectedTS) => {
        const { assignmentSubsuppliers } = this.props;
        const filteredAssignmentSubSuppliers = assignmentSubsuppliers.filter(x => x.recordStatus !== 'D');
        if (Array.isArray(assignmentSubsuppliers) && assignmentSubsuppliers.length > 0) {
            const mainSupplierId = filteredAssignmentSubSuppliers[0].mainSupplierId; // Getting Main Supplier Id
            const mainSupplierTSExisting = filteredAssignmentSubSuppliers[0].assignmentSubSupplierTS;
            
            // Filtering Main Supplier TS
            const mainSupplierTS = selectedTS.filter(x => {
                const supplierId = x.supplierLocationId && x.supplierLocationId.split('_')[0];
                if(supplierId == mainSupplierId){
                    let alreadyExistMSTS = [];
                    if(!isEmpty(mainSupplierTSExisting) && mainSupplierTSExisting.length > 0){
                        alreadyExistMSTS = mainSupplierTSExisting.filter(val => val.epin === x.epin && val.isAssignedToThisSubSupplier === false && val.isDeleted !== true);
                        if(isEmpty(alreadyExistMSTS)){
                            return x;}
                    }
                    else{  return x; }   
                }
            });
            assignmentSubsuppliers.forEach((assigSubSup,assignmentSubsupplierIndex) => {
                const supSuppliersWithTs = [];
                const subSupplierTSExisting = isEmptyReturnDefault(assigSubSup.assignmentSubSupplierTS); 

                // Updating subSuppliersWithTs with Main Supplier TS
                if(mainSupplierTS && mainSupplierTS.length > 0 && assigSubSup.recordStatus !== 'D'){
                    mainSupplierTS.forEach(mainSupTS => {
                        supSuppliersWithTs.push({
                            assignmentSubSupplierId: assigSubSup.assignmentSubSupplierId,
                            epin: mainSupTS.epin,
                            subSupplierId: assigSubSup.subSupplierId,
                            isAssignedToThisSubSupplier: false
                        });
                    });
                }

                for(const item in selectedTS){
                    const eachTs = selectedTS[item];
                    if (eachTs.supplierLocationId) {
                        const subSupplierId = eachTs.supplierLocationId.split('_')[0];
                        let alreadyExistSSTS = [];
                        if (assigSubSup.subSupplierId == subSupplierId && assigSubSup.recordStatus !== 'D') {
                            alreadyExistSSTS = subSupplierTSExisting.filter(val => val.epin === eachTs.epin && val.isAssignedToThisSubSupplier !== false && val.isDeleted !== true);
                            if(isEmpty(alreadyExistSSTS)){
                                supSuppliersWithTs.push({
                                    assignmentSubSupplierId: assigSubSup.assignmentSubSupplierId,
                                    epin: eachTs.epin,
                                    subSupplierId: assigSubSup.subSupplierId,
                                    isAssignedToThisSubSupplier: true
                                });
                            }
                        }
                    }
                };
                if (Array.isArray(supSuppliersWithTs) && supSuppliersWithTs.length > 0) {
                    this.props.actions.SetTechSpecToAssignmentSubSupplier(supSuppliersWithTs,assignmentSubsupplierIndex);
                }
            });
        }
    }

    /** populate mainsupplier resource in search param */
    populateMainSupplierResource = (resourceList,searchParam,isTimesheet) => {
        const selectedTSInfo = [];
        resourceList.filter(x =>{
            if(isTimesheet){
                const selectedTechspec = {};
                selectedTechspec.epin=x.epin;
                selectedTechspec.lastName=x.lastName;
                selectedTechspec.firstName=x.firstName;
                selectedTSInfo.push(selectedTechspec);
            } else {
                const mainSupId = x.supplierLocationId && x.supplierLocationId.split('_')[0];
                if(mainSupId == searchParam.supplierId){
                    const selectedTechspec = {};
                    selectedTechspec.epin=x.epin;
                    selectedTechspec.lastName=x.lastName;
                    selectedTechspec.firstName=x.firstName;
                    selectedTSInfo.push(selectedTechspec);
                }   
            }
        });
        return selectedTSInfo;
    };

    /** populate subSupplier resource in search param */
    populateSubSupplierResource = (resourceList,searchParam) => {
        const subSupplierData = deepCopy(searchParam.subSupplierInfos);
        subSupplierData.forEach(x => {
            const subSupplierTS = [];
            resourceList.filter(y =>{
                const subSupId = y.supplierLocationId && y.supplierLocationId.split('_')[0];
                if(subSupId == x.subSupplierId){
                    const selectedTechspec = {};
                    selectedTechspec.epin=y.epin;
                    selectedTechspec.lastName=y.lastName;
                    selectedTechspec.firstName=y.firstName;
                    subSupplierTS.push(selectedTechspec);
                }
            });
            x.selectedSubSupplierTS = subSupplierTS;
        });
        return subSupplierData;
    };

    /** populate assigned techspec resource */
    populateAssignedResource = (mainSupplierResource,subSupplierInformation,assignmentTS) => {
        const resultantResource = [];
        const assignmentResource = deepCopy(assignmentTS);
        mainSupplierResource.forEach(x => {
            const isTSExist = assignmentResource.find(y => y.epin == x.epin && y.recordStatus !== 'D');
            if(!isTSExist){
                const selectedTechspec = {};
                selectedTechspec.epin=x.epin,
                selectedTechspec.lastName=x.lastName,
                selectedTechspec.firstName=x.firstName,
                assignmentResource.push(selectedTechspec);
                resultantResource.push(selectedTechspec);
            }
        });
        subSupplierInformation.forEach(val => {
            const subSupplierTS = deepCopy(val.selectedSubSupplierTS);
            subSupplierTS.forEach(x => {
                const isTSExist = assignmentResource.find(y => y.epin == x.epin && y.recordStatus !== 'D');
                if(!isTSExist){
                    const selectedTechspec = {};
                    selectedTechspec.epin=x.epin,
                    selectedTechspec.lastName=x.lastName,
                    selectedTechspec.firstName=x.firstName,
                    assignmentResource.push(selectedTechspec);
                    resultantResource.push(selectedTechspec);
                }
            });
        });
        return resultantResource;
    };

    /** Save resource handler */
    saveResourceHandler = (e) => {      
        e.preventDefault();
        const { techSpecList,visitData,timesheetData,assignmentDetail } = this.props;
        const arsSearchDetail = this.props.assignmentData,
        selectedRows = this.searchGridChild.getSelectedRows();
        let techSpecListTot;
        let visitTimeListTot;
        let techList;
        
        const searchParam = isEmptyReturnDefault(arsSearchDetail.searchParameter,'object');
        const selectedRecords = this.removeDuplicates(selectedRows,'epin');
        this.props.actions.SelectedTechSpec(selectedRecords);
        let assignedResource = [];

        if (isUndefined(assignmentDetail.isCopyAssignment)) {
            if (visitData) {
                for (let i = 0; i < techSpecList.length; i++) {
                    for (let j = 0; j < visitData.length; j++) {
                        if (techSpecList[i].supplierLocationId.includes(visitData[j].supplierId)) {
                            techList = techSpecList[i].resourceSearchTechspecInfos.map(y => y.epin);
                            techSpecListTot = techSpecList[i].resourceSearchTechspecInfos.filter(x => x.isSelected).map(y => y.epin);
                            visitTimeListTot = visitData[j].techSpecialists.map(x => x.pin).filter(x => techList.includes(x));

                            if (!visitTimeListTot.every(x => techSpecListTot.includes(x))) {
                                IntertekToaster("Resource assigned in Visit/Timesheet cannot be removed from Assignment.", "warningToast firstVisitSupplierWithNoTsSelected");
                                return false;
                            }
                        }
                    }
                }
            }
            else if (timesheetData) {
                for (let i = 0; i < techSpecList.length; i++) {
                    for (let j = 0; j < timesheetData.length; j++) {
                        techList = techSpecList[i].resourceSearchTechspecInfos.map(y => y.epin);
                        techSpecListTot = techSpecList[i].resourceSearchTechspecInfos.filter(x => x.isSelected).map(y => y.epin);
                        visitTimeListTot = timesheetData[j].techSpecialists.map(x => x.pin).filter(x => techList.includes(x));

                        if (!visitTimeListTot.every(x => techSpecListTot.includes(x))) {
                            IntertekToaster("Resource assigned in Visit/Timesheet cannot be removed from Assignment.", "warningToast firstVisitSupplierWithNoTsSelected");
                            return false;
                        }
                    }
                }
            }
        }
        if (selectedRecords.length !== 0) {
            
            if (arsSearchDetail.searchAction === "AR" || arsSearchDetail.searchAction === "SSSPC") {
                this.techSpecToDelete.forEach(x => this.props.actions.deleteSubSupplierTS(x)); // ITK D - 888
                const mainSupplierResource = this.populateMainSupplierResource(selectedRows, searchParam, this.props.isTimesheetAssignment);
                const subSupplierInformation = this.populateSubSupplierResource(selectedRows, searchParam);
                assignedResource = this.populateAssignedResource(mainSupplierResource, subSupplierInformation, this.props.assignedTechSpec);
                this.updatedData.selectedTechSpecInfo = mainSupplierResource;
                this.updatedData.subSupplierInfos = subSupplierInformation;
            }
        }
        else{
            if (arsSearchDetail.searchAction === "AR") {
                IntertekToaster("No Resource have been selected. Please select at least one resource.", 'warningToast commentsMandate');
                return false;
            }
        }
        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
        if(arsSearchDetail.searchAction === 'ARR' && this.props.selectedMyTask && this.props.selectedMyTask.taskType === "OM Verify and Validate"){
            const approvedResources = arsSearchDetail.overridenPreferredResources.filter(x=>x.isApproved === true);
            if(approvedResources && approvedResources.length > 0)
              this.props.actions.ClearOptionalSearch(arsSearchDetail.searchAction);
        }
        //this.props.actions.ClearOptionalSearch(arsSearchDetail.searchAction, this.updatedData); //Changes for ITK D1465
        this.updatedData={};
        const isValid = this.arsMandatoryFieldCheck();
        if(isValid){
            const firstVisitSupplierWithNoTsSelected = this.supplierSubSupplierTsValidation(selectedRows);

            if(firstVisitSupplierWithNoTsSelected && firstVisitSupplierWithNoTsSelected.length > 0){
                IntertekToaster(`At least one resource  has to be selected  for the Supplier/Sub-Supplier for the First Visit from - "${ firstVisitSupplierWithNoTsSelected }"`,"warningToast firstVisitSupplierWithNoTsSelected");
                return false;
            }
            const arsSearchAction = arsSearchDetail.searchAction;
            const alreadyOverridenResources = isEmptyReturnDefault(arsSearchDetail.overridenPreferredResources);
            if(this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || 
                this.props.currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE){
                const overrideRes =  this.processOverridenResources(alreadyOverridenResources,this.props.overridenResources);
                this.props.actions.UpdateActionDetails({ "overridenPreferredResources" : overrideRes });
                this.props.actions.AssignTechnicalSpecialistFromARS(assignedResource);
                if(this.props.isVisitAssignment && arsSearchDetail.searchAction === "AR"){
                    this.setSelectedTSToSubSupplier(selectedRows);//MS-TS Link
                }
                // ASSIGNMENT SAVE BUTTON ENABLE DISABLE ACTION
                this.props.actions.AssignmentSaveButtonDisable(false);
                this.props.actions.ARSSearchPanelStatus(false);
                this.props.actions.AssignResourcesButtonClick(false);
                this.techSpecToDelete = [];
            }
            else if(this.props.currentPage === localConstant.sideMenu.DASHBOARD){
                this.props.actions.UpdateARSSearch().then(response => {
                    if(response) {
                        if(arsSearchAction === "AR"){
                            IntertekToaster("Resources Assigned Successfully","successToast resourceAssignedSuccess");
                        }
                        this.props.actions.FetchTechSpecMytaskData();
                        this.cancelResourceSearch(e);
                    }
                });
                this.techSpecToDelete = [];
            }
            this.props.actions.SetDefaultPreAssignmentName();
        }
    };

    /** combining already assigned overriden resources and newly overriden resources */
    processOverridenResources = (oldData, newData) => {
        const result = [ ...oldData ];
        newData.forEach(x => {
            if(x.techSpecialist && x.recordStatus !== 'D'){
                const alreadyExisitData = oldData.filter(y => y.techSpecialist && y.techSpecialist.epin === x.techSpecialist.epin);
                if(alreadyExisitData <= 0) {
                    result.push(x);
                }
            }
        });
        return result;
    };

    /** Cancel Resource Search and go back to the assigned specialist page */
    cancelResourceSearch = (e) => {
        e.preventDefault();
        const selectedRows = this.searchGridChild.getSelectedRows();
        const selectedRecords = this.removeDuplicates(selectedRows,'epin');
        this.props.actions.SelectedTechSpec(selectedRecords);

        this.techSpecToDelete = [];
        this.props.actions.ARSSearchPanelStatus(false);
        this.props.actions.AssignResourcesButtonClick(false);
        this.props.actions.clearPreAssignmentDetails();
        this.props.actions.clearARSSearchDetails();
        this.props.actions.SetDefaultPreAssignmentName();
        if(this.props.currentPage === localConstant.sideMenu.DASHBOARD){
            this.props.actions.ClearMyTasksData();
            this.props.actions.ARSSearchPanelStatus(false);
            this.props.actions.AssignResourcesButtonClick(false);
            this.props.actions.SetDefaultPreAssignmentName();
            this.props.actions.FetchTechSpecMytaskData();
        }
    };

    /** ARS Search Mandatory Field Check */
    arsMandatoryFieldCheck = () => {
        const arsSearchDetail = this.props.assignmentData;
        if(this.props.isDashboardARSView && this.props.selectedMyTask && this.props.selectedMyTask.taskType === "OM Verify and Validate") {
            if(!isEmpty(arsSearchDetail.overridenPreferredResources) && Array.isArray(arsSearchDetail.overridenPreferredResources)){
                const resourceNotTouched = arsSearchDetail.overridenPreferredResources.filter(x=>x.isApproved === null);
                const rejectedResource = arsSearchDetail.overridenPreferredResources.filter(x=>x.isApproved === false && x.recordStatus !== null);
                if(resourceNotTouched && resourceNotTouched.length > 0) {
                    IntertekToaster(localConstant.resourceSearch.APPROVED_REJECTED,'warningToast overrideAppr/RejVal');
                    this.setState({ isPanelOpenActionDetails: true });                   
                    return false;
                }
                if(rejectedResource && rejectedResource.length > 0 && required(arsSearchDetail.description)){
                    IntertekToaster(localConstant.resourceSearch.ARS_COMMENTS,'warningToast commentsMandate');
                    this.setState({ isPanelOpenActionDetails: true });                   
                    return false;
                }
            }
        }
        else {
            if(required(arsSearchDetail.searchAction)){
                IntertekToaster(localConstant.resourceSearch.ARS_SEARCH_ACTION,'warningToast searchActionVal');
                this.setState({ 
                        isPanelOpenActionDetails:true,
                       //isPanelOpenARSviewValidate: true
                     });                

                return false;
            }
            if(arsSearchDetail.searchAction ==="SD" || arsSearchDetail.searchAction === "PLO"){
                if(required(arsSearchDetail.dispositionType))
                {
                    IntertekToaster(`${ localConstant.resourceSearch.DISPOSITION_DETAILS } - ${ localConstant.resourceSearch.DISPOSITION_DETAILS }`,'warningToast searchDispositionVal');
                    this.setState({ 
                        isPanelOpenActionDetails:true,
                        //isPanelOpenARSviewValidate: true
                     });
                    return false;
                } else {
                    if (arsSearchDetail.dispositionType === "OTH") {
                        if (required(arsSearchDetail.description)) {
                            IntertekToaster(`${ localConstant.resourceSearch.COMMOENTS } - ${ localConstant.resourceSearch.COMMOENTS }`, 'warningToast searchDispositionVal');
                            this.setState({ 
                                isPanelOpenActionDetails:true,
                                //isPanelOpenARSviewValidate: true
                             });
                            return false;
                        }
                    }
                }
            }
            if(arsSearchDetail.searchAction === "OPR"){
                if(isEmpty(this.props.overridenResources)){
                    IntertekToaster(`${ localConstant.resourceSearch.ACTION } - ${ localConstant.resourceSearch.LIST_OF_TS }`,'warningToast listOfTsVal');
                    this.setState({ 
                        isPanelOpenActionDetails:true,
                        //isPanelOpenARSviewValidate: true
                     });
                    return false;
                }
                if(required(arsSearchDetail.assignedToOmLognName)){
                    IntertekToaster(`${ localConstant.resourceSearch.ACTION } - ${ localConstant.resourceSearch.LIST_OF_OM }`,'warningToast listOfOmVal');
                    this.setState({ 
                        isPanelOpenActionDetails:true,
                        //isPanelOpenARSviewValidate: true
                     });
                    return false;
                }
            }
        }
        return true;
    };
   
    componentDidMount() {
        /**
     * this.props.isGrmMasterDataFeteched is bool flag which will be set as true once the
     * GRM master data is loaded. For subsequent GRM related menus we check for this flag to load master data
     * If user goes out of GRM/Resource module NEED set the flag to false to reload masterdata 
     */
        if(!this.props.isARSMasterDataLoaded){
            this.props.actions.arsMasterData(); // ARS Performance Fine Tuning
        }
        this.props.actions.FetchARSCommentsHistoryReport(this.props.assignmentData.assignmentId);
        this.props.actions.FetchTechSpecCategory();
        this.props.actions.FetchAssignmentType();
    }

    /** Assigned Resources JSON Formation */
    assignedTechSpecJSONformation = (data) => {
        const techSpecInfo = [];
        const { techSpecList } = this.props;
        let techList = false;
        if(data.length > 0){
            data.forEach(iteratedValue => {
                iteratedValue.assignedTechSpec.forEach(element => {
                    const techSpec = {};
                    techSpec.epin = element.epin;
                    techSpec.resourceName = `${ element.lastName } ${ element.firstName }`;
                    techSpec.profileStatus = element.profileStatus;
                    techSpec.supplierName = iteratedValue.supplierName;
                    techSpec.taxonomy = iteratedValue.taxonomyServiceName;
                    if (this.props.isVisitAssignment) {
                        for (let i = 0; i < techSpecList.length; i++) {
                            if (techSpecList[i].supplierLocationId.includes(iteratedValue.supplierId)) {
                                techList = techSpecList[i].resourceSearchTechspecInfos.map(y => y.epin).includes(element.epin);
                            }
                        }
                        techSpec.InActiveDangerTag  = !techList;
                    }
                    else {
                        techSpec.InActiveDangerTag = !element.isTechSpecFromAssignmentTaxonomy;
                    }
                    techSpecInfo.push(techSpec);
                });
            });
            localStorage.setItem("techList",JSON.stringify(techSpecInfo));
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

    rejectedResourcesData = (data) => {
        if(data && data.length > 0){
            const rejectedTechSpecArray = [];
            data.forEach(iteratedValue => {
                if(iteratedValue.techSpecialist){
                    const techSpecName = `${ iteratedValue.techSpecialist.lastName } ${ iteratedValue.techSpecialist.firstName }`;
                    if(rejectedTechSpecArray.indexOf(techSpecName) === -1)
                      rejectedTechSpecArray.push(techSpecName);
                }
            });
            return rejectedTechSpecArray.join();
        }
        return "N/A";
    }

    convertToMultiSelectObject=(datas)=>{
        const multiselectArray = [];
        if (datas) {
            datas.map(data => {
                multiselectArray.push({ value: data , label: data });
            });
        }
        return multiselectArray;
    }

    TSconvertToMultiSelectObject=(datas)=>{
        const multiselectArray = [];
        if (datas) {
            datas.map(data => {
                if(data.techSpecialist){
                    const name = `${ data.techSpecialist.lastName } ${ data.techSpecialist.firstName }`;
                    const epinNo = data.techSpecialist.epin;
                    multiselectArray.push({ value: epinNo , label: name });
                }
            });
        }
        return multiselectArray;
    }

    //Multi Select Change Events
    customerApprovalMultiselect =(value)=>{
        const customerApproval = [];
        value.map(customer => {
            customerApproval.push(customer.label);
        });
        this.updatedData["customerApproval"] = customerApproval;
        this.props.actions.AddOptionalSearch(this.updatedData);
    } 

    equipmentDescriptionMultiselect =(value)=>{
        const equipmentsdescription = [];
        value.map(eqipment => {
            equipmentsdescription.push(eqipment.label);
        });
        this.updatedData["equipmentMaterialDescription"] = equipmentsdescription;
        this.props.actions.AddOptionalSearch(this.updatedData);
    }

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

    assignRejectedResource = () => {
        const technicalSpec = this.techSpecValues;
        const overrideResources = isEmptyReturnDefault(this.props.assignmentData.overridenPreferredResources);
        const alreadyAssigned = overrideResources.filter(x=>x.techSpecialist.epin === technicalSpec.value && x.isApproved !== false);
        if(alreadyAssigned && alreadyAssigned.length <=0){
            const overrideResource = {};
            this.props.technicalSpecialists.forEach(iteratedValue => {
                if(technicalSpec.value === iteratedValue.epin){
                    overrideResource.techSpecialist = {};
                    overrideResource.techSpecialist.epin = iteratedValue.epin;
                    overrideResource.techSpecialist.firstName = iteratedValue.firstName;
                    overrideResource.techSpecialist.lastName = iteratedValue.lastName;
                    overrideResource.techSpecialist.profileStatus = iteratedValue.profileStatus;
                    overrideResource.id = 0;
                    overrideResource.isApproved = null;
                    overrideResource.recordStatus = "N";
                    this.techSpec.push(overrideResource);
                }
            });
        }
        this.props.actions.OverridenResources(this.techSpec);
        this.updatedData = {};
        this.props.actions.HideModal();
    }

    rejectResource = () => {
        this.props.actions.HideModal();
    }

    /** Technical Specialist Multiselect change Handler - Override Preferred Resources */
    techSpecMultiSelectHandler = (value) => {
        const overrideResources = isEmptyReturnDefault(this.props.assignmentData.overridenPreferredResources);
        //const techSpec = [];
        this.techSpec = [];
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.OM_REJECTED_RESOURCE,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.assignRejectedResource,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.rejectResource,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        value.map(technicalSpec => {
            let alreadyApprovedOrRejected = false;
            this.techSpecValues = [];
            let popupShowed = false;
            const alreadyRejected=overrideResources.filter(x=>x.techSpecialist.epin === technicalSpec.value && x.isApproved === false);
            if(alreadyRejected && alreadyRejected.length > 0){
                const addedTechSpec = this.props.overridenResources.filter(x=>x.techSpecialist.epin === technicalSpec.value);
                const alreadyApproved = overrideResources.findIndex(x => x.techSpecialist.epin === technicalSpec.value && x.isApproved === true);
                if(alreadyApproved === -1 && addedTechSpec && addedTechSpec.length <=0){ 
                    this.techSpecValues = technicalSpec;
                    popupShowed = true;
                    this.props.actions.DisplayModal(confirmationObject); //Added for D1326
                }
            }
            const alreadyApproved=overrideResources.filter(x=>x.techSpecialist.epin === technicalSpec.value && x.isApproved === true);
            if(alreadyApproved && alreadyApproved.length > 0){
                alreadyApprovedOrRejected = true;
                alreadyApproved.forEach(row=>{
                    IntertekToaster(`${ row.techSpecialist.firstName } ${ row.techSpecialist.lastName } Resource already approved by OM`,"warningToast OMrejectVal");
                });
            }
            const alreadyAssigned = overrideResources.filter(x=>x.techSpecialist.epin === technicalSpec.value && x.isApproved !== false); //Changes for D1326
            if(alreadyAssigned && alreadyAssigned.length > 0){
                alreadyAssigned.forEach(row=>{
                    // techSpec.push(row);
                });
            }
            if(!popupShowed && !alreadyApprovedOrRejected && alreadyAssigned && alreadyAssigned.length <=0){
                const overrideResource = {};
                this.props.technicalSpecialists.forEach(iteratedValue => {
                    if(technicalSpec.value === iteratedValue.epin){
                        overrideResource.techSpecialist = {};
                        overrideResource.techSpecialist.epin = iteratedValue.epin;
                        overrideResource.techSpecialist.firstName = iteratedValue.firstName;
                        overrideResource.techSpecialist.lastName = iteratedValue.lastName;
                        overrideResource.techSpecialist.profileStatus = iteratedValue.profileStatus;
                        overrideResource.id = 0;
                        overrideResource.isApproved = null;
                        overrideResource.recordStatus = "N";
                        this.techSpec.push(overrideResource);
                    }
                });
            }
        });
        this.props.actions.OverridenResources(this.techSpec);
        this.updatedData = {};
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
        this.updatedData.taskType = this.props.selectedMyTask.taskType;
        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
        this.updatedData = {};
        this.props.actions.searchPreAssignmentTechSpec();
        this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
    }

    assignmentType = (assignmentTypeList,typeCode) => {
        const assignmentTypeName = [];
        assignmentTypeList && assignmentTypeList.forEach(iteratedValue => {
            if(iteratedValue.description === typeCode){
                assignmentTypeName.push(iteratedValue.name);
            }
        });
        return assignmentTypeName;
      };

    selectPreAssignmnetSearch =(e)=>{
        e.preventDefault();
        this.allCoordinatorCheck.current.checked = false;//Changes for D890 - Checkbox unchecked
        this.props.actions.FetchPreAssignmentIds(); //Assignment Performance related changes om 24-10-19
        this.setState({
            preAssignmentSearchModalOpen:true
        });
    }

    hideModal = (e) => {
        e.preventDefault();   
        this.btnSearchPreAssignmentPopup.current.focus();   
        this.setState({
            preAssignmentSearchModalOpen:false
        });
    }

    //get pre-assignment Name From Popup AG Grid 
    getPreAssignmentName = (e) => {
        e.preventDefault();
        const selectedPreAssignment = this.child.getSelectedRows();
        if (isEmpty(selectedPreAssignment)) {
            IntertekToaster('Please select anyone Pre-Assignment ', 'warningToast preAssignmentSearchReq');
        } else {
            selectedPreAssignment.map((preAssignment) => {
                this.props.actions.GetSelectedPreAssignment(preAssignment);
                this.props.actions.SetDefaultPreAssignmentName(preAssignment);
            });             
            this.setState({
                preAssignmentSearchModalOpen:false
            });
        }
        this.btnSearchPreAssignmentPopup.current.focus();
    }

    handleAllCoOrdinator = (e) => {
        this.updatedData["allCoOrdinator"] = e.target.checked;
        this.props.actions.FetchPreAssignmentIds(this.updatedData);
        this.updatedData={};
    }

    /** Resource Search on PLO */
    resourceSearch = (e) => {
        e.preventDefault();
        const isValid = this.grmSearhPLOMandatoryFieldCheck();
        if(isValid){
            this.props.actions.searchPreAssignmentTechSpec();
        }
    };

    grmSearhPLOMandatoryFieldCheck = () => {
        const searchParameters = isEmptyReturnDefault(this.props.assignmentData.searchParameter,'object');
        const ploTaxonomy = Object.assign({},searchParameters.ploTaxonomyInfo);
        if(required(ploTaxonomy.categoryName) || required(ploTaxonomy.subCategoryName) || required(ploTaxonomy.serviceName)){
            IntertekToaster(" Please enter all the values in GRM Search Section for Overriding the default search !!","warningToast GRMSearchPLO");
            return false;
        }
        return true;
    };

    ploTaxonomyOnChangeHandler = (e) => {
        e.preventDefault();
        const searchParameters = isEmptyReturnDefault(this.props.assignmentData.searchParameter,'object');
        const ploTaxonomy = Object.assign({},searchParameters.ploTaxonomyInfo);
        const inputvalue = formInputChangeHandler(e);
        if(inputvalue.name === "categoryName"){
            this.props.actions.ClearPLOSubCategory();
            this.props.actions.ClearPLOServices();
            this.props.actions.FetchPLOTechSpecSubCategory(inputvalue.value);
            ploTaxonomy[inputvalue.name] = inputvalue.value;
        }
        else if(inputvalue.name === "subCategoryName"){
            this.props.actions.ClearPLOServices();
            this.props.actions.FetchPLOTechSpecServices(inputvalue.value);
            ploTaxonomy[inputvalue.name] = inputvalue.value;
        }
        else if(inputvalue.name === "serviceName"){
            ploTaxonomy[inputvalue.name] = inputvalue.value;
        }
        this.updatedData["ploTaxonomyInfo"] = ploTaxonomy;
        this.props.actions.UpdatePreAssignmentSearchParam(this.updatedData);
    };  
    toggleSlide=(e)=>{
        this.setState({ isOpensearchSlide:!this.state.isOpensearchSlide });
    }

    arsRowSelectHandler = (e) => {
        const selectedRecords = this.searchGridChild && this.searchGridChild.getSelectedRows();
        //MS-TS nov 22
        //const { assignmentSubsuppliers } = this.props;
        
        const isSelected=e.node.selected;
        this.handleResourceSelection(e);
        
        if(isSelected===false)
        {
            /** Action to delete subSupplier TS in assignment Sub Suppliers */
            const tsToDelete = e && e.data;
            tsToDelete && this.techSpecToDelete.push(tsToDelete); // ITK D - 888 
        }
        //MS-TS nov 22
        if(selectedRecords && selectedRecords.length > 0){
            if(this.props.currentPage === localConstant.sideMenu.DASHBOARD){
                if(this.props.selectedMyTask && (this.props.selectedMyTask.taskType === "PLO to RC" || this.props.selectedMyTask.taskType === "OM Validated")){
                    this.setState({
                        isPloResourceSelected: false
                    });
                } else{
                    this.setState({
                        isPloResourceSelected: true
                    });
                }
            } else {
                this.onArsSearchGridRowSelected(e);
            }
        } else {
            this.setState({
                isPloResourceSelected:false
            });
        }
    }

    handleResourceSelection = (e) => {
        
        if(e.data && e.data.epin){
            const searchReource =  isEmptyReturnDefault(this.props.techSpecList);
            const currentRowSupplierLocationId = e.data && e.data.supplierLocationId; // resourceSearchTechspecInfos
            searchReource.forEach(iteratedValue => {
                if(iteratedValue.supplierLocationId === currentRowSupplierLocationId){
                    const resourceIndex = iteratedValue.resourceSearchTechspecInfos && iteratedValue.resourceSearchTechspecInfos.findIndex(x => x.epin === e.data.epin);
                    if(resourceIndex > -1){
                        iteratedValue.resourceSearchTechspecInfos[resourceIndex].isSelected = e.node && e.node.selected;
                    }
                }
            });
        }
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

    onArsSearchGridRowSelected = (e) => {
        if (!e.node.selected && e.data && e.data.epin) {
            const { assignmentSubsuppliers } = this.props;
            const currentRowData = e.data;
            for (const item of assignmentSubsuppliers) {
                const subSupplierId = currentRowData.supplierLocationId ?currentRowData.supplierLocationId.split('_')[0]:'';
                if (item.subSupplierId == subSupplierId
                    && Array.isArray(item.assignmentSubSupplierTS)
                    && item.assignmentSubSupplierTS.length > 0) {
                    for (const eachTs of item.assignmentSubSupplierTS) {
                        if (eachTs.epin === currentRowData.epin) {
                                // IntertekToaster("Resource is part of assignment cannot be Removed.", "warningToast onArsSearchGridRowSelected");
                                // e.node.setSelected(true);
                                // return false; // MS - TS - Removed this is not a correct validation as per EVO  
                        }
                    }
                }
            }
        }
    }
    
    /** Comments History Report Click Handler */
    commentsHistoryClickHandler = (e) => {
        this.setState({ showCommentsReport : true });
    };

    /** Close Comments History Report Handler */
    closeCommentsHistoryReport = (e) => {
        e.preventDefault();
        this.setState({ showCommentsReport : false });
    };

    render() {
        
        const { currentPage,companyList,contractHoldingCoodinatorList,operatingCoordinatorList,interactionMode,
            assignmentData ,assignmentTypes,assignmentStatus,taxonomyCategory,taxonomySubCategory,taxonomyServices,
            customerList,preAssignmentIds,techSpecList,dispositionType,isResourceMatched,selectedPreAssignmentSearchParam,
            languages,certificates,isShowARSModal,operationalManagers,technicalSpecialists,isDashboardARSView,selectedMyTask,defaultPreAssignmentID,
            ploTaskStatus,ploTaxonomyService,ploTaxonomySubCategory,overridenResources,equipment,taxonomyCustomerApproved,isShowGoogleMap,
            arsCHCoordinatorInfo,arsOPCoordinatorInfo, arsCommentHistory, assignmentDetail  } = this.props;
        const searchParameters = isEmptyReturnDefault(assignmentData.searchParameter,'object');
        const subSupplierInfos = isEmptyReturnDefault(searchParameters.subSupplierInfos);
        const assignedResources = isEmptyReturnDefault(searchParameters.assignedResourceInfos);
        const optionalSearch= isEmptyReturnDefault(searchParameters.optionalSearch, 'object');
        const overrideResources = isEmptyReturnDefault(assignmentData.overridenPreferredResources);
        const techSpecs = isEmptyReturnDefault(searchParameters.selectedTechSpecInfo);
        const selectedPreAssignmentTechSpecs = isEmptyReturnDefault(selectedPreAssignmentSearchParam.selectedTechSpecInfo);
        let arsSearchAction = localConstant.resourceSearch.arsActions;
        const ploSearchAction=localConstant.resourceSearch.ploActions;
        const isTimesheet = (searchParameters.workFlowType === 'M' || searchParameters.workFlowType === 'N') ? true : false;
        const isVisit = (searchParameters.workFlowType === 'V' || searchParameters.workFlowType === 'S') ? true : false;
        const unmatchedResources = this.resourceDatas(this.props.unMatchedResources);
        const isARSSearch = true;
        
        this.techSpecAssigned = this.assignedTechSpecJSONformation(assignedResources);

        //Default values for Optional Param Multi select
        const defaultCustomerApprovalMultiSelection=this.convertToMultiSelectObject(optionalSearch.customerApproval);
        const defaultEquipmentMultiSelection =this.convertToMultiSelectObject(optionalSearch.equipmentMaterialDescription);
        const defaultCertificateMultiSelection = this.convertToMultiSelectObject(optionalSearch.certification);
        const defaultLanguageSpeakingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageSpeaking);
        const defaultLanguageWritingMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageWriting);
        const defaultLanguageComprehensionMultiSelection = this.convertToMultiSelectObject(optionalSearch.languageComprehension);
        const defaultTechSpecMultiSelection = this.TSconvertToMultiSelectObject(overridenResources);
        
        const overrideTechSpecList = [];
        technicalSpecialists.forEach(iteratedValue => {
            iteratedValue.fullName = `${ iteratedValue.lastName } ${ iteratedValue.firstName }`;
            overrideTechSpecList.push(iteratedValue);
        });
        let techList;
        for(let i=0; i<techSpecList.length;i++){
            techList = techSpecList[i].resourceSearchTechspecInfos.map(y => y.epin);
        }
        const overrideTechList = technicalSpecialists.filter(x => !techList.includes(x.epin));
        let hasRejectedResources = false;
        const rejectedResources = [];
        const rejectedResource = overrideResources.filter(x=>x.isApproved === false);
        rejectedResource.forEach(x => {
            let alreadyApproved = false;
            overrideResources.forEach(y => {
                if(y.techSpecialist.epin === x.techSpecialist.epin && y.isApproved === true){
                    alreadyApproved = true;
                }
            });
            if(!alreadyApproved){
                rejectedResources.push(x);
            }
        });
        if(rejectedResources && rejectedResources.length > 0){
            hasRejectedResources = true;
        }

        let allApprovedResources = false;
        const approvedResources = overrideResources.filter(x=>x.isApproved === true);
        if(approvedResources && approvedResources.length > 0){
            if(approvedResources.length === overrideResources.length){
                allApprovedResources = true;
            }
        }

        if(rejectedResources && rejectedResources.length === 0){
            allApprovedResources = true;
        }

        if((overrideResources && overrideResources.length > 0 && !hasRejectedResources && allApprovedResources) ||
        (this.state.isPloResourceSelected && this.props.selectedMyTask && this.props.selectedMyTask.taskType !== "PLO - No Match in GRM")){
            const tabsToHide = [ 'OPR','PLO','SD' ];
            arsSearchAction = arrayUtil.negateFilter(arsSearchAction, 'value', tabsToHide);
        }
        
        const rejectedResourceValue = this.rejectedResourcesData(rejectedResources);
        const assignmentTypeARS = this.assignmentType(assignmentTypes,searchParameters.assignmentType);
        const SearchEvolutionHeader=<div>Search(Optional Parameters)</div>;
        let groupName={};
        const  exceptionArray=[];
        if(techSpecList.length>0){
            techSpecList && techSpecList.map(x=>{
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
        }

       // bindAction(this.headerData.searchResourceHeader, "ExportColumn", this.exportDocument);
               
        return (
            <Fragment>  
                { this.state.showCommentsReport && 
                    <Modal
                        modalClass="popup-position nestedModal"
                        overlayClass="nestedOverlay"
                        title={ localConstant.resourceSearch.COMMENTS_HISTORY_REPORT }
                        buttons={this.commentsReportButtons}
                        isShowModal={this.state.showCommentsReport}>
                            <GridView 
                                gridColData={this.headerData.commentsHistoryReportHeader} 
                                gridRowData={arsCommentHistory} />
                    </Modal>
                }
                { this.state.showExceptionList &&
                   <Modal 
                        modalClass="modalSearchException nestedModal"
                        overlayClass="nestedOverlay"
                        title={' Search Exception List'}
                        buttons={this.exceptionListloseButton}
                        isShowModal={this.state.showExceptionList} > 
                        <GridView gridColData={this.headerData.exceptionSearchResourceHeader} 
                            gridRowData={exceptionArray}
                            groupName={this.exceptiongGoupingParam.groupName} 
                            dataName={this.exceptiongGoupingParam.dataName} 
                            isGrouping={true} />
                    </Modal>
                }
                { this.state.showGoogleMap && 
                    <Modal title={localConstant.commonConstants.VIEW_MAP}
                        titleClassName="viewGoogleMap mt-3"
                        modalClass={this.state.windowRestoreGoogleMap ? 'windowMinRestoreIcon nestedModal' : 'windowMaxRestoreIcon nestedModal' }
                        modalContentClass="extranetModalContent"
                        overlayClass="nestedOverlay"
                        modalId="googlePopup"
                        formId="googleForm"
                        buttons={this.googleMapCloseButton}
                        isShowModal={true}
                        isShowButtonHeader={true}
                        isCloseIcon={true}
                        isWindowRestoreIcon={true}
                        isDisableDrag={true}
                        >                         
                        <DirectionsGoogleMap mapData={techSpecList} />
                    </Modal>
                }
                <Modal id="arsSearchModalPopup"
                    title={'Assignment Resource Search'}
                    modalClass={isDashboardARSView ?'dashboardaArsSearchModal' : 'arsSearchModal'}         //For ARSPOPUP Maximize class added                  
                    isShowButtonHeader={true}
                    isCloseIcon={false}
                    isFxiedComponent={true}
                    fixedComponent={                        
                        // <Panel colSize="s12 pl-0 pr-0 mb-2" heading={ localConstant.resourceSearch.ASSIGNMENT_RESOURCE_SEARCH } isArrow={true} onpanelClick={(e) => this.ARSActionDefaultpanelClick('ActionDetails','ARSviewValidate')} isopen={isDashboardARSView ? (this.state.isPanelOpenARSviewValidate ? true : false) : this.state.isPanelOpenActionDetails } >
                        <Panel colSize="s12 pl-0 pr-0 mb-0" heading={ localConstant.resourceSearch.ASSIGNMENT_RESOURCE_SEARCH } isArrow={true} onpanelClick={(e) => this.ARSActionDefaultpanelClick('ActionDetails','ARSviewValidate')} isopen={ this.state.isPanelOpenActionDetails } >
                            <ActionSearch
                                currentPage={currentPage}
                                isARSSearch={isARSSearch}
                                dispositionTypeList={ dispositionType }
                                actionList={(selectedMyTask.taskType==="PLO to RC") ? ploSearchAction:arsSearchAction}
                                actionData={assignmentData}
                                technicalSpecialists = { overrideTechList }
                                optionAttributs={this.state.techSpecOptionAttributes}
                                techSpecMultiSelectChangeHandler = { (value) => this.techSpecMultiSelectHandler(value) }
                                defaultTechSpecMultiSelection = {defaultTechSpecMultiSelection}
                                operationalManagers = {operationalManagers}
                                inputChangeHandler={(e) => this.actionOnchangeHandler(e)}
                                overrideGridRowData = {overrideResources}
                                overrideGridHeaderData = {this.headerData.OverrideResourceHeader}
                                selectedMyTask = {selectedMyTask}
                                isDashboardARSView={isDashboardARSView}
                                rejectedResourceValue = { rejectedResourceValue }
                                hasRejectedResources = { hasRejectedResources }
                                overrideTaskStatus = { this.props.overrideTaskStatus } 
                                defaultActionValue={assignmentData.searchAction}
                                commentsHistoryClickHandler = { (e) => this.commentsHistoryClickHandler(e) } />                    
                            <ArsSearchDiv
                                inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                assignMentStatusList={assignmentStatus}
                                assignmentData = { searchParameters }
                                preAssignmentIds = { preAssignmentIds }
                                interactionMode={ interactionMode }
                                selectedMyTask = { selectedMyTask }
                                isDashboardARSView={isDashboardARSView}
                                selectPreAssignmnetSearch={(e) => this.selectPreAssignmnetSearch(e)} 
                                preAssignmentSearchModalOpen={this.state.preAssignmentSearchModalOpen}
                                hideModal={(e) => this.hideModal(e)}
                                getPreAssignmentName={(e) => this.getPreAssignmentName(e)}
                                btnSearchPreAssignmentPopup={this.btnSearchPreAssignmentPopup}
                                allCoordinatorCheck={this.allCoordinatorCheck}
                                preAssignmentSearchHeader={this.headerData.preAssignmentSearchHeader}
                                preAssignmentID={assignmentDetail.preAssignmentId ? assignmentDetail.preAssignmentId : defaultPreAssignmentID} 
                                onRef={ref => { this.child = ref; }}
                                handleAllCoOrdinator={this.handleAllCoOrdinator}
                                assignedResourceData = {unmatchedResources}
                                isResourceNotMatched = {!isResourceMatched} />
                            </Panel>
                    }
                    buttons={[
                        /** Assign resources to assignment button */ 
                        {
                            name: localConstant.commonConstants.CANCEL,
                            action: (e) => this.cancelResourceSearch(e),
                            btnClass: "btn-small mr-2 ",
                            showbtn: true,
                            type: "button"
                        }, {
                            name: localConstant.resourceSearch.SAVE_RESOURCE,
                            action: (e) => this.saveResourceHandler(e),
                            btnClass: "btn-small mr-1",
                            showbtn: true
                        }
                    ]}
                    isShowModal={isShowARSModal}
                    isWindowRestoreIcon={true} //For ARSPOPUP Maximize button 
                >                           
                    { isDashboardARSView && 
                        <Panel colSize="s12 pl-0 pr-0" heading={ localConstant.resourceSearch.MORE_DETAILS } isArrow={true} onpanelClick={(e) => this.panelClick('MoreDetails')} isopen={this.state.isPanelOpenMoreDetails} >
                            <MoreDetails                               
                                currentPage={currentPage}
                                isARSSearch = {isARSSearch}
                                interactionMode={interactionMode}
                                moreDetailsData={searchParameters}
                                companyList = { companyList }
                                chCoordinatorInfo = { arsCHCoordinatorInfo }
                                opCoordinatorInfo = { arsOPCoordinatorInfo }
                                contractHoldingCoodinatorList = { contractHoldingCoodinatorList }
                                operatingCoordinatorList = { operatingCoordinatorList }
                                inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                isDashboardARSView={isDashboardARSView} />
                        </Panel>
                    }
                    { isDashboardARSView &&  
                        <Panel colSize="s12 pl-0 pr-0" heading={isVisit? localConstant.resourceSearch.SUPPLIER + '/' +localConstant.resourceSearch.SUBSUPPLIER : localConstant.resourceSearch.TIMESHEET_DETAILS} isArrow={true} onpanelClick={(e) => this.panelClick('SubSupplier')} isopen={this.state.isPanelOpenSubSupplier} >
                            <div className="row mb-0">
                                { isVisit ?
                                    <Fragment>
                                        <SupplierInfoDiv
                                            interactionMode={interactionMode}
                                            isARSSearch={isARSSearch}
                                            moreDetailsData={searchParameters}
                                            inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                            isDashboardARSView={isDashboardARSView} />
                                        <SubSupplierDetails
                                            gridData={subSupplierInfos}
                                            gridHeaderData={this.headerData.supplierHeader}
                                            gridRef={ref => { this.gridChild = ref; }}
                                            isSubSupplierGrid={true}
                                            subSupplierGridBtn={this.subSupplierGridBtn}
                                            buttons={this.preAssignmentSearchBtn}
                                            currentPage={currentPage}
                                            isARSSearch={isARSSearch}
                                            interactionMode={interactionMode}
                                            inputChangeHandler={(e) => this.inputChangeHandler(e)} 
                                            isDashboardARSView={isDashboardARSView}/>
                                    </Fragment> : null
                                }
                                <FirstVisit
                                    isARSSearch={isARSSearch}
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                    isDashboardARSView={isDashboardARSView} />
                                <AssignMentType
                                    isARSSearch={isARSSearch}
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    assignmentType={assignmentTypes}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)}
                                    assignMentStatusList={assignmentStatus}
                                    optionAttributs={this.state.optionAttributs} 
                                    isDashboardARSView={isDashboardARSView}
                                    assignmentTypeData = { !isEmpty(assignmentTypeARS) && assignmentTypeARS[0] } />
                                <CategoryInfoDiv
                                    interactionMode={interactionMode}
                                    moreDetailsData={searchParameters}
                                    taxonomyCategory={taxonomyCategory}
                                    taxonomySubCategory={taxonomySubCategory}
                                    taxonomyServices={taxonomyServices}
                                    inputChangeHandler={(e) => this.inputChangeHandler(e)} 
                                    isDashboardARSView={isDashboardARSView}/>
                            </div>
                        </Panel>
                    }
                    <Panel colSize="s12 pl-0 pr-0" heading={localConstant.resourceSearch.ASSIGNED_RESOURCE} isArrow={true} onpanelClick={(e) => this.panelClick('AssignedResource')} isopen={this.state.isPanelOpenAssignedResource} >
                        <div className="row">
                            <AssignedResource
                                gridData={this.techSpecAssigned}
                                gridHeaderData={this.headerData.AssignedResourceHeader}
                                gridRef={ref => { this.gridChild = ref; }}
                                isSubSupplierGrid={true}
                                subSupplierGridBtn={this.assignedResourcesGridBtn}
                                buttons={this.assignedSearchBtn}
                                currentPage={currentPage}
                                isARSSearch={isARSSearch}
                                interactionMode={interactionMode}
                                rowClassRules={{ allowDangerTag: true }}
                                inputChangeHandler={(e) => this.inputChangeHandler(e)} />
                        </div>
                    </Panel>
                    { isDashboardARSView && selectedMyTask && ploTaskStatus.includes(selectedMyTask.taskType) ?
                        <Panel colSize="s12 pl-0 pr-0" heading={localConstant.resourceSearch.GRM_SEARCH} isArrow={true} onpanelClick={(e) => this.panelClick('GrmSearch')} isopen={this.state.isPanelOpenGrmSearch} >
                            <div className="row">
                                <PLOSearchGrm
                                    interactionMode={(selectedMyTask && selectedMyTask.taskType === "PLO to RC")? false:true}
                                    moreDetailsData={searchParameters}
                                    taxonomyCategory={taxonomyCategory}
                                    taxonomySubCategory={ploTaxonomySubCategory}
                                    taxonomyServices={ploTaxonomyService}
                                    inputChangeHandler={(e) => this.ploTaxonomyOnChangeHandler(e)} />
                                { (selectedMyTask && selectedMyTask.taskType === "PLO to RC")? 
                                    <Btn buttons={this.assignedSearchBtn} />:null }
                            </div>
                        </Panel>
                    : null}
                    <Panel colSize="s12 pl-0 pr-0 mb-0" heading={ SearchEvolutionHeader} isArrow={true} onpanelClick={(e) => this.panelClick('OptionalSearch')} isopen={ this.state.isPanelOpenOptionalSearch } >
                    <OptionalSearch                            
                            buttons={this.optionalSearchGrm}                           
                            multiSelectOptionsList={languages}
                            certificatesMultiSelectOptionsList={certificates}
                            equipment={equipment}
                            defaultMultiSelection={[]}
                            defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
                            defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                            defaultLanguageSpeakingMultiSelection={defaultLanguageSpeakingMultiSelection}
                            defaultLanguageWritingMultiSelection={defaultLanguageWritingMultiSelection}
                            defaultLanguageComprehensionMultiSelection={defaultLanguageComprehensionMultiSelection}
                            equipmentDescriptionMultiselectValueChange={(value)=> this.equipmentDescriptionMultiselect(value)}
                            certificationMultiSelectValueChange={(value) => this.certificationMultiselect(value)}
                            languageSpeakingMultiSelectValueChange={(value) => this.languageSpeakingMultiselect(value)}
                            languageWritingMultiSelectValueChange={(value) => this.languageWritingMultiselect(value)}
                            languageComprehensionMultiSelectValueChange={(value) => this.languageComprehensionMultiselect(value)}
                            optionAttributs={this.state.optionAttributs}
                           
                            searchGRMData={optionalSearch}
                            expiryFrom={this.expiryFrom}
                            expiryTo={this.expiryTo}
                            optionalParamLabelData={searchParameters}
                            inputChangeHandler={(e) => this.optionalOnchangeHandler(e)}
                           
                            taxonomyCustomerApproved={!isEmpty(taxonomyCustomerApproved) ? addElementToArray(taxonomyCustomerApproved) :[]}
                            customerApprovalMultiselectValueChange={(value) => this.customerApprovalMultiselect(value)}
                            defaultCustomerApprovalMultiSelection={defaultCustomerApprovalMultiSelection}/>
                    </Panel>
                    <Panel colSize="s12 pl-0 pr-0" heading={ localConstant.resourceSearch.SERACH_RESULT } isArrow={true} onpanelClick={() => this.panelClick('SearchParams')} isopen={this.state.isPanelOpenSearchParams} >
                        <SearchGRM
                            gridCustomClass={currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || 
                                currentPage === localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE ? 'grmSearchPopupGridHeight' : 'searchPopupGridHeight'}
                            isARSSearch={isARSSearch}
                            gridData={techSpecList}
                            gridGroupProps = {this.groupingParam}
                            buttons={this.optionalSearchGrm}
                            exportButtons={this.exportGridBtn}
                            multiSelectOptionsList={languages}
                            certificatesMultiSelectOptionsList={certificates}
                            equipment={equipment}
                            defaultMultiSelection={[]}
                            defaultEquipmentMultiSelection={defaultEquipmentMultiSelection}
                            defaultCertificateMultiSelection={defaultCertificateMultiSelection}
                            defaultLanguageSpeakingMultiSelection={defaultLanguageSpeakingMultiSelection}
                            defaultLanguageWritingMultiSelection={defaultLanguageWritingMultiSelection}
                            defaultLanguageComprehensionMultiSelection={defaultLanguageComprehensionMultiSelection}
                            equipmentDescriptionMultiselectValueChange={(value)=> this.equipmentDescriptionMultiselect(value)}
                            certificationMultiSelectValueChange={(value) => this.certificationMultiselect(value)}
                            languageSpeakingMultiSelectValueChange={(value) => this.languageSpeakingMultiselect(value)}
                            languageWritingMultiSelectValueChange={(value) => this.languageWritingMultiselect(value)}
                            languageComprehensionMultiSelectValueChange={(value) => this.languageComprehensionMultiselect(value)}
                            optionAttributs={this.state.optionAttributs}
                            gridHeaderData={this.headerData.searchResourceHeader}
                            gridRef={ref => { this.searchGridChild = ref; }}
                            rowSelectedHandler = {this.arsRowSelectHandler}                               
                            searchGRMData={optionalSearch}
                            expiryFrom={this.expiryFrom}
                            expiryTo={this.expiryTo}
                            optionalParamLabelData={searchParameters}
                            inputChangeHandler={(e) => this.optionalOnchangeHandler(e)}
                            searchPanelOpen={this.toggleSlide}  
                            isShowMapBtn={ isShowGoogleMap } 
                            isSearchPanelOpen={this.state.isOpensearchSlide}
                            taxonomyCustomerApproved={!isEmpty(taxonomyCustomerApproved) ? addElementToArray(taxonomyCustomerApproved) :[]}
                            customerApprovalMultiselectValueChange={(value) => this.customerApprovalMultiselect(value)}
                            defaultCustomerApprovalMultiSelection={defaultCustomerApprovalMultiSelection}
                              
                        />
                    </Panel>                       
                </Modal>  
                { Array.isArray(this.props.techSpecList) && this.props.techSpecList.length > 0 ?
                    <div style={{ display: "none" }}>
                        <Table columnData={this.props.techSpecList} headerData={localConstant.technicalSpecialist} />
                    </div>
                : null }
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
                           // inputChangeHandler={(e) => this.sectionInputChangeHandler(e)}
                            />
                        </Modal>
                        : null
                }              
            </Fragment>
        );
    }
}

export default ArsSearch;