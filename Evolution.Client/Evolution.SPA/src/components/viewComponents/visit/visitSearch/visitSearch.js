import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './visitSearchHeader';
import Panel from '../../../../common/baseComponents/panel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler, isEmptyOrUndefine,isEmpty } from '../../../../utils/commonUtils';
import CustomerAndCountrySearch  from '../../../applicationComponents/customerAndCountrySearch';
import moment from 'moment';
import dateUtil from '../../../../utils/dateUtil';
import InputWithPopUpSearch from '../../../applicationComponents/inputWithPopUpSearch';
import { validateObjectHasValue } from '../../../../utils/objectUtil';
import { applicationConstants } from '../../../../constants/appConstants';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { required } from '../../../../utils/validator';
import { moduleViewAllRights_Modified } from '../../../../utils/permissionUtil';
import { securitymodule } from '../../../../constants/securityConstant';
// import ExportToCSV from '../../../applicationComponents/exportToCSV';

const localConstant = getlocalizeData();
const defaultStateField={
                customerName:'',
                contractNo:'',
                customerContarctNo:'',
                assignmentNo:'',
                reportNo:'',
                ProjectNo:'',
                customerProjectName:'',
                supplierPoNo:'',
                supplierSubSupplier:'',
                technicalSpecialist:'',
                notificationRef:'',
                eariliestvisitDate:'',
                lastvaisitdate:'',
                contractHoldingCompany:'',
                chCoordinator:'',
                operatingCompany:'',
                ocCoordinatorName:'',
                searchDocumentType:'',
                searchText:'',
                materialDescription:'',
                visitCategory:'',
                visitSubCategory:'',
                visitService:''
};
const VisitSearchDiv = (props) => (
    <form onSubmit={props.visitSearch} autoComplete="off">
        <div>
        <div className="row mb-0">
            <CustomerAndCountrySearch  colSize='col s3 pl-0 pr-0' isMandate={false} />
            <CustomInput
                    hasLabel={true}
                    name="contractNo"
                    colSize='s2'
                    label={localConstant.visit.CONTRACT_NUMBER}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.contractNo} />
                     <CustomInput
                    hasLabel={true}
                    name="customerContarctNo"
                    colSize='s2 pl-0'
                    label={localConstant.visit.CUSTOMER_CONTRACT_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.customerContarctNo} 
                    maxLength={250}
                />
                    <CustomInput
                    hasLabel={true}
                    name="assignmentNo"
                    colSize='s2 pl-0 pr-0'
                    label={localConstant.visit.ASSIGNMENT_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.assignmentNo}
                />
                <CustomInput
                    hasLabel={true}
                    name="reportNo"
                    colSize='s3'
                    label={localConstant.visit.REPORT_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={60}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.reportNo} />
                    </div>
                    <div className="row mb-0">
                 <CustomInput
                    hasLabel={true}
                    name="ProjectNo"
                    colSize='s3 pr-0'
                    label={localConstant.visit.PROJECT_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={100}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.ProjectNo} />
                     <CustomInput
                    hasLabel={true}                    
                    name="customerProjectName"
                    colSize='s2'
                    label={localConstant.visit.CUSTOMER_PROJECT_NAME}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={100}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.customerProjectName}
                     />
                    
                <CustomInput
                    hasLabel={true}                    
                    name="supplierPoNo"
                    colSize='s2 pl-0'
                    label={localConstant.visit.SUPPLIER_PO_NO}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={150}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.supplierPoNo} />

                <InputWithPopUpSearch
                    colSize='col s2 subSupplierDiv pl-0'                    
                    hasLabel={true}
                    label={localConstant.visit.SUPPLIER_SUBSUPPLIER}
                    headerData={props.headerData.supplierSearchHeader}
                    name="supplierSubSupplier"
                    searchModalTitle={localConstant.supplierpo.SUPPLIER_LIST}
                    gridRowData={props.modalRowData}
                    defaultInputValue={props.searchFieldData.supplierSubSupplier}
                    onAddonBtnClick={props.supplierPopupOpen}
                    onModalSelectChange={props.getMainSupplier}
                    onInputBlur={props.getMainSupplier}
                    onSubmitModalSearch={props.getSelectedMainSupplier}
                    objectKeySelector="supplierName"
                    columnPrioritySort={props.columnPrioritySort}
                    callBackFuncs={props.callBackFuncs}
                    searchcolSize={"s11 pr-0 p1-0"}
                />
                {/* <CustomInput
                    hasLabel={true}
                    name="supplierSubSupplier"
                    colSize='s3 pr-0' 
                    label={localConstant.visit.SUPPLIER_SUBSUPPLIER}
                    type='text'
                    inputClass="customInputs"
                    maxLength={10}
                    onValueChange={props.inputHandleChange} /> */}
                <CustomInput
                    hasLabel={true}
                    name="technicalSpecialist"
                    colSize='s3 p1-0'
                    label={localConstant.visit.TECHNICALSPECIALIST}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={115}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.technicalSpecialist} /> 
                    </div>
                    <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    name="notificationRef"
                    colSize='s3 pr-0'
                    label={localConstant.visit.NOTIFICATION_REF}
                    type='text'
                    dataValType='valueText'
                    inputClass="customInputs"
                    maxLength={50}
                    onValueChange={props.inputHandleChange}
                    value={props.searchFieldData.notificationRef}
                     /> 
                <CustomInput
                    hasLabel={true}
                    name="eariliestvisitDate"
                    label={localConstant.visit.EARILIEST_VISIST_DATE}
                    colSize='s2'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}                    
                    type='date'
                    inputClass="customInputs"
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.searchFieldData.eariliestvisitDate)}
                    onDateChange={props.startDateChange}                    
                />
                <CustomInput
                    hasLabel={true}
                    name="lastvisitdate"
                    colSize='s2 pl-0'
                    dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                    label={localConstant.visit.LAST_VISIST_DATE}
                    type='date'
                    inputClass="customInputs"
                    autocomplete="off"
                    selectedDate={dateUtil.defaultMoment(props.searchFieldData.lastvisitdate)}
                    onDateChange={props.endDateChange}
                    />
                <CustomInput
                    hasLabel={true}
                    name="contractHoldingCompany"
                    id="contractHoldingCompanyCode"
                    colSize='s2 pr-0 pl-0'
                    label={localConstant.visit.CONTRACT_HOLDER_COMPANY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.companyList}
                    optionName='companyName'
                    optionValue="id"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.searchFieldData.contractHoldingCompany}/>               
               
                <CustomInput
                    hasLabel={true}
                    name="chCoordinator"
                    colSize='s3'
                    label={localConstant.visit.CH_COORDINATOR_NAME}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.contractHoldingCoodinatorList}
                    optionName='miDisplayName'
                    optionValue="id"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.searchFieldData.chCoordinator}
                    />
</div>
<div className="row mb-0">
            <CustomInput
                hasLabel={true}
                name="visitCategory"
                id="visitCategory"
                colSize='s3 pr-0'
                label={localConstant.visit.CATEGORY}
                type='select'
                inputClass="customInputs"
                optionsList={props.taxonomyCategory}
                optionSelecetLabel="Select"
                optionName= 'name'
                optionValue="id"
                onSelectChange={props.inputHandleChange}
                defaultValue={props.searchFieldData.visitCategory}
                disabled={props.isVisitCategoryDisabled?props.isVisitCategoryDisabled:false} />
            <CustomInput
                hasLabel={true}
                name="visitSubCategory"
                id="visitSubCategory"
                colSize='s2'
                label={localConstant.visit.SUB_CATEGORY}
                type='select'
                optionSelecetLabel="Select"
                inputClass="customInputs"
                optionsList={props.techSpecSubCategory}
                optionName='taxonomySubCategoryName'
                optionValue="id"
                onSelectChange={props.inputHandleChange}
                defaultValue={props.searchFieldData.visitSubCategory}
                disabled={props.isVisitSubCategoryDisabled?props.isVisitSubCategoryDisabled:false} />
            <CustomInput
                hasLabel={true}
                name="visitService"
                id="visitService"
                colSize='s2 pl-0'
                label={localConstant.visit.SERVICES}
                type='select'
                optionSelecetLabel="Select"
                inputClass="customInputs"
                optionsList={props.techSpecServices}
                optionName='taxonomyServiceName'
                optionValue="id"
                onSelectChange={props.inputHandleChange}
                defaultValue={props.searchFieldData.visitService}
                disabled={props.isVisitServiceDisabled?props.isVisitServiceDisabled:false} />  
                <CustomInput
                    hasLabel={true}
                    name="operatingCompany"
                    id="operatingCompanyCode"
                    colSize='s2 pl-0 pr-0'
                    label={localConstant.visit.OPERATING_COMPANY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.companyList}
                    optionName='companyName'
                    optionValue="id"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.searchFieldData.operatingCompany}/>
               <CustomInput
                    hasLabel={true}
                    name="ocCoordinatorName"
                    colSize='s3'
                    label={localConstant.visit.OC_COORDINATOR_NAME}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.operatingCoordinatorList}
                    optionName='miDisplayName'
                    optionValue="id"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.searchFieldData.ocCoordinatorName}/>
                    </div>
                    <div className="row mb-0">
                    <CustomInput
                    hasLabel={true}
                    name="materialDescription"
                    colSize='s3'
                    label={localConstant.visit.MATERIAL_DESCRIPTION}
                    type='text'
                    dataValType='valueText'
                    value={props.searchFieldData.materialDescription}
                    inputClass="customInputs"
                    maxLength={200} 
                    onValueChange={props.inputHandleChange} />
                <CustomInput
                    hasLabel={true}                    
                    name="searchDocumentType"
                    id="searchDocumentType"
                    onSelectChange={props.inputHandleChange}
                    colSize='s3 pr-0'
                    label={localConstant.visit.SEARCH_DOCUMENT}
                    type='select'
                    className="browser-default"
                    optionName="name"
                    optionValue="name"
                    defaultValue={props.searchFieldData.searchDocumentType}
                    optionsList={props.documentMasterData && props.documentMasterData.filter(x=>x.moduleName==="Visit")} />

                <CustomInput
                    hasLabel={true}
                    name="searchText"
                    colSize='s3'
                    label={localConstant.visit.SEARCH_TEXT}
                    type='text'
                    dataValType='valueText'
                    value={props.searchFieldData.searchText}
                    inputClass="customInputs"
                    maxLength={200} 
                    onValueChange={props.inputHandleChange} />
                    
            <div className="col mt-4x" >
                <button type="submit" className="waves-effect btn ">{localConstant.commonConstants.BTN_SEARCH}</button>
                <button type="button" onClick={props.clearSearchData} className="ml-2 waves-effect btn" >{localConstant.commonConstants.BTN_RESET}</button>

                {/* <ExportToCSV csvExportClick={props.csvExportClick}
                    csvRef={props.csvRef}
                    buttonClass={"ml-2 waves-effect btn"}
                    csvClass={"displayNone"}
                    filename={"Visit Search Data.csv"}
                    data={props.csvData.baseVisit}
                    header={props.csvData.header}
                /> */}
            </div>
            </div>
        </div>
    </form>
);

class VisitSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            startDate: '',
            endDate: '',
            errorList: [],
            searchFieldData:defaultStateField,
            visitSearchList:[], //Search Optimization
            visitSearchTotalCount:0,
            isVisitCategoryDisabled:false,
            isVisitSubCategoryDisabled:false,
            isVisitServiceDisabled:false,
            // csvData:[],
            // headers:[]
        };
        this.visitSearchFilter = {};
        // this.headers=[];
        this.updatedData = {};
        this.modelBtns = {
            errorListButton: [
              {
                type:'button',
                name: localConstant.commonConstants.OK,
                action: (e) => this.closeErrorList(e),
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
              }
            ]
        };      
        this.callBackFuncs ={
            onReset:()=>{}
          };

          this.csvLink = React.createRef();
    }   

    componentDidMount = () => {
        this.props.actions.FetchTaxonomyBusinessUnitForSearch();
        this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();
       this.props.actions.ClearData();

       this.props.actions.FetchContractHoldingCoordinator();
       this.props.actions.FetchOperatingCoordinator();
       
        this.setState({
            isVisitCategoryDisabled: false,
            isVisitSubCategoryDisabled: true,
            isVisitServiceDisabled: true,
        });
    }

    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    //Panel Click Event Handler
    panelClick = () => {
        this.setState({ isPanelOpen: !this.state.isPanelOpen });
    }

    startDateChange = (date) => {
       
        const filterData = { ...this.state.searchFieldData }; 
        if (isEmpty(date)) {
            filterData.eariliestvisitDate="";           
        }
        else {
            filterData.eariliestvisitDate = moment(date);           
        }
        this.setState({
            searchFieldData: filterData
        });
    }

    // endDateChange = (date) => {        
    //     this.setState({
    //         endDate: date
    //     }, () => {
    //         const data = moment.parseZone(this.state.endDate).utc().format();
    //         this.visitSearchFilter.lastvaisitdate = data;
    //         this.props.actions.UpdateVisitEndDate(data);
    //     });
    // }
    endDateChange = (date) => { 
        const filterData = { ...this.state.searchFieldData }; 
        if (isEmpty(date)) {
            filterData.lastvisitdate="";           
        }
        else {
            filterData.lastvisitdate = moment(date);           
        }
        this.setState({
            searchFieldData: filterData
        });
    }

    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const searchData={ ...this.state.searchFieldData };
        const inputvalue = formInputChangeHandler(e);        
        //this.visitSearchFilter.customerName = this.props.defaultCustomerName;
        if(inputvalue.type === "select")   {
            searchData[e.target.name] = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
            searchData[e.target.id] = e.nativeEvent.target.value;
        }else{
            searchData[e.target.name] = inputvalue.value;
        }
        if(inputvalue.name === "contractHoldingCompany"){
            if(e.nativeEvent.target.value===""){
                this.props.actions.FetchContractHoldingCoordinator();
            }
            else
            this.props.actions.FetchContractHoldingCoordinator(e.nativeEvent.target.value);
        }
        if(inputvalue.name === "operatingCompany"){
            if(e.nativeEvent.target.value===""){
                this.props.actions.FetchOperatingCoordinator();
            }
            else
            this.props.actions.FetchOperatingCoordinator(e.nativeEvent.target.value);
        }
        if (e.target.name === "visitCategory") {
            this.props.actions.ClearSubCategory();
            if (e.target.value === '') {
                this.setState({
                    isVisitSubCategoryDisabled: true,
                    isVisitServiceDisabled: true,
                });
            }
            else{
                this.props.actions.FetchTechSpecSubCategory(e.target.value,true);
                this.setState({
                    isVisitSubCategoryDisabled: false,
                    isVisitServiceDisabled: true,
                });
            }
            searchData['visitSubCategory'] = '';
            searchData['visitService'] = '';
            this.props.actions.ClearServices();
        }
        if (e.target.name === "visitSubCategory") {
            this.props.actions.ClearServices();
            if (e.target.value === ''){
                this.setState({
                    isVisitServiceDisabled: true,
                });
            }
            else{
                this.props.actions.FetchTechSpecServices(searchData.visitCategory, e.target.value,true);
                this.setState({
                    isVisitServiceDisabled: false,
                });
            }
            searchData['visitService'] = '';
        }
         this.setState({ searchFieldData:searchData });
    }
    //Form Search 
    FormSearch = (e,isLoadMore) => {
        e.preventDefault();
        // this.setState({ isPanelOpen: !this.state.isPanelOpen });
        const visitSearchFilter = { ...this.state.searchFieldData };
        const customerName = this.props.defaultCustomerName;
        const customerId=this.props.defaultCustomerId;

        visitSearchFilter.customerName = customerName ? customerName.trim() : "";
        visitSearchFilter.customerId = customerId ? customerId : "";
        
        if(!isEmpty(visitSearchFilter.eariliestvisitDate)){
            visitSearchFilter.eariliestvisitDate= moment(visitSearchFilter.eariliestvisitDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }
        if(!isEmpty(visitSearchFilter.lastvisitdate)){
            visitSearchFilter.lastvisitdate= moment(visitSearchFilter.lastvisitdate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }

        //validate the search fields
        const isValid = validateObjectHasValue(visitSearchFilter, Object.keys(applicationConstants.visitSearchValidateFields));
        if (!isValid) {
            this.setState({
                errorList: Object.values(applicationConstants.visitSearchValidateFields)
            });
            return false;
        }
        visitSearchFilter.loggedInCompanyCode = this.props.selectedCompany;
        if (Array.isArray(this.props.contractHoldingCompany) && this.props.contractHoldingCompany.length > 0) {
            const tempCompany = this.props.contractHoldingCompany.filter(company => this.props.selectedCompany === company.companyCode);
            visitSearchFilter.loggedInCompanyId = Array.isArray(tempCompany) && tempCompany.length > 0  ? tempCompany[0].id : "";
        }
        if(!moduleViewAllRights_Modified(securitymodule.VISIT, this.props.viewAllRightsCompanies)){
            if(required(visitSearchFilter.contractHoldingCompany) && required(visitSearchFilter.operatingCompany)){
                visitSearchFilter.loggedInCompanyCode = this.props.selectedCompany;
                visitSearchFilter.isOnlyViewVisit = true;
                
            }
            // if(!required(visitSearchFilter.contractHoldingCompany) && !required(visitSearchFilter.operatingCompany) &&
            // visitSearchFilter.contractHoldingCompany !== this.props.selectedCompany && visitSearchFilter.operatingCompany !== this.props.selectedCompany){
            //     this.props.actions.ClearVisitSearchResults();
            //     return false;
            // }
            if(!required(visitSearchFilter.contractHoldingCompany) && required(visitSearchFilter.operatingCompany) && visitSearchFilter.contractHoldingCompany !== this.props.selectedCompany){
                visitSearchFilter.operatingCompany = this.props.selectedCompany;
            }
            if(required(visitSearchFilter.contractHoldingCompany) && !required(visitSearchFilter.operatingCompany) && visitSearchFilter.operatingCompany !== this.props.selectedCompany){
                visitSearchFilter.contractHoldingCompany = this.props.selectedCompany;
            }
         }
         if(!required(visitSearchFilter.visitCategory)){
            const categoryMandatoryValues=[];
            let isAlreadAdded = false;
            if(!required(visitSearchFilter.visitSubCategory)){
                if(required(visitSearchFilter.visitService)){
                    if (!isAlreadAdded) {
                    categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                    isAlreadAdded = true;
                    }
                }
            }
            else{
                if (!isAlreadAdded) {
                categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                isAlreadAdded = true;
                }
            }
            if(categoryMandatoryValues.length>0){
                this.setState({
                    errorList: categoryMandatoryValues
                });
                return false;
            }
        }

        //search optimization
        if(isLoadMore){
            visitSearchFilter.OffSet=this.state.visitSearchList?this.state.visitSearchList.length:0;
            visitSearchFilter.visitSearchTotalCount=this.state.visitSearchTotalCount;
        }
        this.props.actions.FetchVisitsForSearch(visitSearchFilter).then((res) => {
            if (res) {
                if (!isLoadMore) {
                    this.setState({
                        visitSearchTotalCount: res.recordCount,
                        visitSearchList:[],
                        isPanelOpen: !this.state.isPanelOpen
                    });
                }
                const tempData = Object.assign([], this.state.visitSearchList);
                if (Array.isArray(this.props.visitSearchData) && this.props.visitSearchData.length > 0)
                    tempData.push(...this.props.visitSearchData);
                this.setState({
                    visitSearchList: tempData
                });
            }
        });
    }
    componentWillUnmount()
    {
        this.props.actions.ClearData();
        this.props.actions.ClearVisitSearchResults();
        this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();
    }
    //Clearing From Search 
    ClearSearchData = () => {
        this.visitSearchFilter={};
        this.props.actions.ClearVisitSearchResults();
        this.supplierList={};
        this.props.actions.ClearData();       
        this.callBackFuncs.onReset();
        this.setState({ 
            searchFieldData: defaultStateField,
            visitSearchList:[], //search optimization
            visitSearchTotalCount:0,
            isVisitCategoryDisabled:false,
            isVisitSubCategoryDisabled:true,
            isVisitServiceDisabled:true,
        });     

        this.props.actions.ClearData();
       this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();

       this.props.actions.FetchContractHoldingCoordinator();
       this.props.actions.FetchOperatingCoordinator();

    }

    getPanelSubTitle = () => {
        if (this.props.location.pathname === AppMainRoutes.visitSearchAssignment){
            return localConstant.visit.CREATE_VISIT;
        }         
        else {
            return localConstant.visit.EDIT_VIEW_VISIT;
        }
    }

    supplierPopupOpen = (data) => {
        this.props.actions.ClearSupplierSearchList();
    }

    getMainSupplier = (data) => {
        const params = {
            supplierName: data.serachInput,
            country: data.selectedCountry
        };
        this.props.actions.FetchSupplierSearchList(params);
    }

    getSelectedMainSupplier = (data) => {
        const filterData = { ...this.state.searchFieldData };
        if (data) {
            filterData["supplierSubSupplier"] = data[0] && data[0].supplierName; //Search Optimization
            filterData["supplierId"] = data[0] && data[0].supplierId; //Search Optimization
        }
        this.setState({ searchFieldData:filterData });
    }

    loadMoreClick=(data)=>{
        this.FormSearch(data,true);
    }

    csvExportClick = async (event, done) => {
        event.preventDefault();
        
        const visitSearchFilter = { ...this.state.searchFieldData };
        const customerName = this.props.defaultCustomerName;
        const customerId=this.props.defaultCustomerId;
        visitSearchFilter.isExport = true;
        visitSearchFilter.customerName = customerName ? customerName.trim() : "";
        visitSearchFilter.customerId = customerId ? customerId : "";
        
        if(!isEmpty(visitSearchFilter.eariliestvisitDate)){
            visitSearchFilter.eariliestvisitDate= moment(visitSearchFilter.eariliestvisitDate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }
        if(!isEmpty(visitSearchFilter.lastvisitdate)){
            visitSearchFilter.lastvisitdate= moment(visitSearchFilter.lastvisitdate).format(localConstant.commonConstants.MONTH_DATE_FORMAT);
        }

        //validate the search fields
        const isValid = validateObjectHasValue(visitSearchFilter, Object.keys(applicationConstants.visitSearchValidateFields));
        if (!isValid) {
            this.setState({
                errorList: Object.values(applicationConstants.visitSearchValidateFields)
            });
            return false;
        }
        if(!moduleViewAllRights_Modified(securitymodule.VISIT, this.props.viewAllRightsCompanies)){
            if(required(visitSearchFilter.contractHoldingCompany) && required(visitSearchFilter.operatingCompany)){
                visitSearchFilter.loggedInCompanyCode = this.props.selectedCompany;
                visitSearchFilter.isOnlyViewVisit = true;
            }
            if(!required(visitSearchFilter.contractHoldingCompany) && !required(visitSearchFilter.operatingCompany) &&
            visitSearchFilter.contractHoldingCompany !== this.props.selectedCompany && visitSearchFilter.operatingCompany !== this.props.selectedCompany){
                this.props.actions.ClearVisitSearchResults();
                return false;
            }
            if(!required(visitSearchFilter.contractHoldingCompany) && required(visitSearchFilter.operatingCompany) && visitSearchFilter.contractHoldingCompany !== this.props.selectedCompany){
                visitSearchFilter.operatingCompany = this.props.selectedCompany;
            }
            if(required(visitSearchFilter.contractHoldingCompany) && !required(visitSearchFilter.operatingCompany) && visitSearchFilter.operatingCompany !== this.props.selectedCompany){
                visitSearchFilter.contractHoldingCompany = this.props.selectedCompany;
            }
        }
        this.props.actions.FetchVisitsForSearch(visitSearchFilter).then(res => {
            if (res && res.result && res.result.baseVisit && res.result.header) {
                // this.setState({
                //     headers:res.headers
                // });
                // this.headers=res.headers;
                this.csvLink.link.click();
            }
        });
    }

    render() {
        const subTitle = this.getPanelSubTitle();
        const defaultSort = [
            { "colId": "supplierName",
                "sort": "asc" },
        ];

        const filteredSupplierList=this.props.supplierList;        

        return (
            <div className="customCard">
                <Panel heading="Search Visit : " isArrow={true} 
                    subtitle={subTitle} onpanelClick={this.panelClick} isopen={this.state.isPanelOpen} >
                    <VisitSearchDiv
                        //visitStartDate = {this.props.visitStartDate}
                        //visitEndDate = {this.props.visitEndDate}
                        defaultCustomerName={this.state.searchCustomerName}
                        searchFieldData={this.state.searchFieldData}
                        companyList={this.props.contractHoldingCompany}
                        contractHoldingCoodinatorList={this.props.contractHoldingCoodinatorList}
                        operatingCoordinatorList={this.props.operatingCoordinatorList}
                        inputHandleChange={(e) => this.inputHandleChange(e)}
                        visitSearch={this.FormSearch}
                        startDateChange={this.startDateChange}
                        endDateChange={this.endDateChange}
                        clearSearchData={this.ClearSearchData}
                        headerData={HeaderData}
                        supplierPopupOpen={data => this.supplierPopupOpen(data)}
                        getMainSupplier={data => this.getMainSupplier(data)}
                        getSelectedMainSupplier={data => this.getSelectedMainSupplier(data)}
                        modalRowData={filteredSupplierList}
                        columnPrioritySort={defaultSort}
                        documentMasterData={this.props.documentMasterData}
                        callBackFuncs={this.callBackFuncs}
                        csvExportClick={this.csvExportClick}
                        csvData={this.props.visitSearchData}
                        // headers={this.state.headers}
                        csvRef={(r) => this.csvLink = r}
                        taxonomyCategory={this.props.taxonomyCategory}
                        techSpecServices={this.props.techSpecServices}
                        techSpecSubCategory={this.props.techSpecSubCategory}
                        isVisitCategoryDisabled={this.state.isVisitCategoryDisabled}
                        isVisitSubCategoryDisabled={this.state.isVisitSubCategoryDisabled}
                        isVisitServiceDisabled={this.state.isVisitServiceDisabled}
                    />
                </Panel>
                <ReactGrid gridRowData={this.state.visitSearchList} gridColData={HeaderData.visitSearchHeader} 
                loadMoreClick={this.loadMoreClick} 
                totalCount={this.state.visitSearchTotalCount}
                />
                {/* {this.state.errorList.length > 0 ? */}
                <Modal title={localConstant.commonConstants.CHECK_ANY_MANDATORY_FIELD}
                  titleClassName="chargeTypeOption"
                  modalContentClass="extranetModalContent"
                  modalId="errorListPopup"
                  formId="errorListForm"
                  buttons={this.modelBtns.errorListButton}
                  isShowModal={this.state.errorList.length > 0}>
                  <ErrorList errors={this.state.errorList} />
                </Modal> 
                {/* : null} */}
            </div>
        );
    }
}

export default VisitSearch;
