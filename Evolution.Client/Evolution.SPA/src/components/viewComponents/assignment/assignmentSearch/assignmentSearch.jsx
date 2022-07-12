import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { headerData } from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';
import SearchFilter from '../../../searchFilter';
import { searchFilterWithFunctionRefs,defaultFilterData } from './assignmentSearchFields';
import { ReplaceString } from '../../../../utils/stringUtil';
import { applicationConstants } from '../../../../constants/appConstants';
import { validateObjectHasValue } from '../../../../utils/objectUtil';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import { required } from '../../../../utils/validator';
import { moduleViewAllRights_Modified } from '../../../../utils/permissionUtil';
import { securitymodule } from '../../../../constants/securityConstant';

const localConstant = getlocalizeData();

class AssignmentSearch extends Component {
    constructor(props) {
        super(props);
        this.childSearchFilter = React.createRef();
        this.csvLink = React.createRef();
        const functionRefs = {};
        functionRefs["changeHandler"] = this.changeHandler;
        functionRefs["resetAssignment"] = this.resetAssignment;
        functionRefs["csvExportClick"] = this.csvExportClick;
        this.searchFields = searchFilterWithFunctionRefs(functionRefs);
        this.state = {
            errorList: [],
            assignmentFilterData:defaultFilterData,
            assignmentSearchList:[], //Search Optimization
            assignmentSearchTotalCount:0,
            searchFields:this.searchFields
        };
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
    }

    componentDidMount() {
        this.props.actions.FetchAssignmentStatus();
        this.props.actions.ClearData();//D648 CustomerName Retained
        this.props.actions.FetchTaxonomyBusinessUnitForSearch();

        let isChanged = false;
        this.searchFields.subComponents.forEach(componentProps => {
            if (componentProps.properties && componentProps.properties.name === 'assignmentSubCategory' || componentProps.properties.name === 'assignmentService') {
                componentProps.properties.disabled = true;
                isChanged = true;
                return true;
            }
        });
        if (isChanged) {
            this.setState({
                searchFields: this.searchFields
            });
        }
    }
    componentWillUnmount()
    {
        this.props.actions.ClearData();
        this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();
    }

    /*Start -- Code added for assignment module 
             To add header name     
        */
       getPanelSubTitle = () => {
        if (this.props.location.pathname === AppMainRoutes.searchTimesheet) {
            return localConstant.commonConstants.TO + localConstant.assignments.ADD_ASSIGNMENT;
            // D-138
         } 
        // D-138
        else if (this.props.location.pathname === AppMainRoutes.timesheetSearchAssignment &&
        (this.props.currentPage === localConstant.timesheet.CREATE_TIMESHEET_MODE || this.props.currentPage === '')) {
             defaultFilterData.workFlowTypeIn ="m,n";         
            return localConstant.timesheet.CREATE_TIMESHEET;
            // D-138
        } else if (this.props.location.pathname === AppMainRoutes.visitSearchAssignment &&
        (this.props.currentPage === localConstant.visit.CREATE_VISIT_MODE||this.props.currentPage === '')) {
             defaultFilterData.workFlowTypeIn ="s,v";           
            return localConstant.visit.CREATE_VISIT;
        }
        else {
            defaultFilterData.workFlowTypeIn ="";  
            return localConstant.assignments.VIEW_EDIT_ASSIGNMENT;
        }
    }
    /*End -- Code added for assignment module*/

    changeHandler = (e) => {
        let isChanged=false;
        const filterData = { ...this.state.assignmentFilterData };
        filterData.customerName = this.props.defaultCustomerName;
        if(e.target.dataType === "numeric" && !isEmpty(e.target.value)){
            e.target.value = e.target.value.trim();
        }
        filterData[e.target.name] = e.target.value;
        if (e.target.name === "assignmentCategory") {
            this.props.actions.ClearSubCategory();
            if (e.target.value === '') {
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'assignmentService' || componentProps.properties.name === 'assignmentSubCategory') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            else {
                this.props.actions.FetchTechSpecSubCategory(e.target.value, true);
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'assignmentService') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                        return true;
                    }
                    if (componentProps.properties && componentProps.properties.name === 'assignmentSubCategory') {
                        componentProps.properties.disabled = false;
                        isChanged = true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            filterData['assignmentSubCategory'] = "";
            filterData['assignmentService'] = "";
            this.props.actions.ClearServices();
        }
        if (e.target.name === "assignmentSubCategory") {
            this.props.actions.ClearServices();
            if (e.target.value === ''){
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'assignmentService') {
                        componentProps.properties.disabled = true;
                        isChanged = true;
                        return true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            else{
                this.props.actions.FetchTechSpecServices(filterData.assignmentCategory, e.target.value,true);
                this.searchFields.subComponents.forEach(componentProps => {
                    if (componentProps.properties && componentProps.properties.name === 'assignmentService') {
                        componentProps.properties.disabled = false;
                        isChanged = true;
                        return true;
                    }
                });
                if (isChanged) {
                    this.setState({
                        searchFields: this.searchFields
                    });
                }
            }
            filterData['assignmentService'] = "";
        }
        this.setState({
            assignmentFilterData : filterData
        }); 
    }
   
    //Assignment search
    searchAssignment = (e,isLoadMore) => {
        e.preventDefault();
        const filteredData = { ...this.state.assignmentFilterData };
        filteredData.customerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        filteredData.assignmentCustomerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';
        const isValid = validateObjectHasValue(filteredData, Object.keys(applicationConstants.assignmentSearchValidateFields));
        if(!isValid){
            this.setState({
                errorList: Object.values(applicationConstants.assignmentSearchValidateFields)
            });
            return false;
        }
        const $this = this;

        if(!moduleViewAllRights_Modified(securitymodule.ASSIGNMENT, this.props.viewAllRightsCompanies)){
        // if(isEmpty(this.props.viewAllRightsCompanies)){
            let selectedCompanyId = "";
            if(Array.isArray(this.props.companyList) && this.props.companyList.length > 0) {
                const tempCompany = this.props.companyList.filter(company => this.props.selectedCompany === company.companyCode);
                selectedCompanyId = Array.isArray(tempCompany) && tempCompany.length > 0  ? tempCompany[0].id : "";
            }
            if(required(filteredData.contractHoldingCompanyCode) && required(filteredData.operatingCompanyCode)){
                filteredData.loggedInCompanyCode = this.props.selectedCompany;
                filteredData.isOnlyViewAssignment = true;
                // if (Array.isArray(this.props.companyList) && this.props.companyList.length > 0) {
                //     const tempCompany = this.props.companyList.filter(company => this.props.selectedCompany === company.companyCode);
                //     filteredData.loggedInCompanyId = Array.isArray(tempCompany) && tempCompany.length > 0  ? tempCompany[0].id : "";
                // }
                filteredData.loggedInCompanyId = selectedCompanyId;
            }
            if(!required(filteredData.contractHoldingCompanyCode) && !required(filteredData.operatingCompanyCode) &&
                filteredData.contractHoldingCompanyCode != selectedCompanyId && filteredData.operatingCompanyCode != selectedCompanyId){
                this.props.actions.ClearAssignmentSearchResults();
                return false;
            }
            if(!required(filteredData.contractHoldingCompanyCode) && required(filteredData.operatingCompanyCode) && filteredData.contractHoldingCompanyCode != selectedCompanyId){
                filteredData.operatingCompanyCode = selectedCompanyId;
            }
            if(required(filteredData.contractHoldingCompanyCode) && !required(filteredData.operatingCompanyCode) && filteredData.operatingCompanyCode != selectedCompanyId){
                filteredData.contractHoldingCompanyCode = selectedCompanyId;
            }
        }
        if (!required(filteredData.assignmentCategory)) {
            const categoryMandatoryValues = [];
            let isAlreadAdded = false;
            if (!required(filteredData.assignmentSubCategory)) {
                if (required(filteredData.assignmentService)) {
                    if (!isAlreadAdded) {
                        categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                        isAlreadAdded = true;
                    }
                }
            }
            else {
                if (!isAlreadAdded) {
                    categoryMandatoryValues.push(localConstant.assignments.CATEGORY_VALIDATION);
                    isAlreadAdded = true;
                }
            }
            if (categoryMandatoryValues.length > 0) {
                this.setState({
                    errorList: categoryMandatoryValues
                });
                return false;
            }
        }

        //search optimization
        if(isLoadMore){
            filteredData.offSet=this.state.assignmentSearchList?this.state.assignmentSearchList.length:0;
            filteredData.assignmentSearchTotalCount=this.state.assignmentSearchTotalCount;
        }
        this.props.actions.FetchAssignmentSearchResultswithLoadMore(filteredData).then((res) => {
            if (res) {
                if (!isLoadMore) {
                    this.setState({
                        assignmentSearchTotalCount: res.recordCount,
                        assignmentSearchList:[],
                    });
                    if($this.childSearchFilter && $this.childSearchFilter.current){
                        $this.childSearchFilter.current.panelClick();
                    }
                }
                const tempData = Object.assign([], this.state.assignmentSearchList);
                if (Array.isArray(this.props.assignmentList) && this.props.assignmentList.length > 0)
                    tempData.push(...this.props.assignmentList);
                this.setState({
                    assignmentSearchList: tempData
                });
            }
        });
    }

    csvExportClick = async (event, done) => {
        event.preventDefault();
        const filteredData = { ...this.state.assignmentFilterData };
        filteredData.isExport = true;
        filteredData.customerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        filteredData.assignmentCustomerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';
        const isValid = validateObjectHasValue(filteredData, Object.keys(applicationConstants.assignmentSearchValidateFields));
        if(!isValid){
            this.setState({
                errorList: Object.values(applicationConstants.assignmentSearchValidateFields)
            });
            return false;
        }
        const $this = this;

        if(!moduleViewAllRights_Modified(securitymodule.ASSIGNMENT, this.props.viewAllRightsCompanies)){
        // if(isEmpty(this.props.viewAllRightsCompanies)){
            if(required(filteredData.contractHoldingCompanyCode) && required(filteredData.operatingCompanyCode)){
                filteredData.loggedInCompanyCode = this.props.selectedCompany;
                filteredData.isOnlyViewAssignment = true;
            }
            if(!required(filteredData.contractHoldingCompanyCode) && !required(filteredData.operatingCompanyCode) &&
                filteredData.contractHoldingCompanyCode !== this.props.selectedCompany && filteredData.operatingCompanyCode !== this.props.selectedCompany){
                this.props.actions.ClearAssignmentSearchResults();
                return false;
            }
            if(!required(filteredData.contractHoldingCompanyCode) && required(filteredData.operatingCompanyCode) && filteredData.contractHoldingCompanyCode !== this.props.selectedCompany){
                filteredData.operatingCompanyCode = this.props.selectedCompany;
            }
            if(required(filteredData.contractHoldingCompanyCode) && !required(filteredData.operatingCompanyCode) && filteredData.operatingCompanyCode !== this.props.selectedCompany){
                filteredData.contractHoldingCompanyCode = this.props.selectedCompany;
            }
        }
        this.props.actions.FetchAssignmentSearchResultswithLoadMore(filteredData).then((res) => {
            if (res && res.result && res.result.baseAssignment && res.result.header) {
                // this.setState({
                //     headers:res.headers
                // });
                // this.headers=res.headers;
                this.csvLink.link.click();
            }
        });
    }

    loadMoreClick=(data)=>{
        this.searchAssignment(data,true);
    }

    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    resetAssignment = (e) => {
        this.setState({
            assignmentFilterData: defaultFilterData,
            assignmentSearchTotalCount: 0,
            assignmentSearchList: [],
        });
       this.props.actions.ClearAssignmentSearchResults();
       this.props.actions.ClearData();
       this.props.actions.ClearSubCategory();
       this.props.actions.ClearServices();

       let isChanged = false;
        this.searchFields.subComponents.forEach(componentProps => {
            if (componentProps.properties && componentProps.properties.name === 'assignmentSubCategory'||componentProps.properties.name === 'assignmentService') {
                componentProps.properties.disabled = true;
                isChanged = true;
                return true;
            }
        });
        if(isChanged){
        this.setState({
            searchFields: this.searchFields
        });
    }
    }
    
    render() {     
        const subTitle = this.getPanelSubTitle();
        const { assignmentStatus,companyList,documentTypesData,taxonomyCategory,techSpecServices,techSpecSubCategory } = this.props;

        const defaultCustomerNameSortOrder =[
            { "colId": "assignmentCustomerName",
              "sort": "asc" },
            {  "colId": "assignmentSupplierPurchaseOrderNumber",
               "sort": "asc" },
        ];
        return (
            <div className="companyPageContainer customCard">
                <SearchFilter colSize="s12"
                    heading={`${ localConstant.assignments.SEARCH_ASSIGNMENT }: `}
                    isArrow={true}
                    subtitle={subTitle}
                    searchFields={this.state.searchFields}
                    filterFieldsData={{
                        assignmentStatus,
                        companyList,
                        documentTypesData,
                        taxonomyCategory,
                        techSpecServices,
                        techSpecSubCategory
                        // csvData: this.props.assignmentList && this.props.assignmentList.baseAssignment ? this.props.assignmentList.baseAssignment : [],
                        // csvHeader: this.props.assignmentList && this.props.assignmentList.header ? this.props.assignmentList.header : [],
                    }}
                    setData={
                        this.state.assignmentFilterData
                    }
                    resetHandler={(e)=>this.resetAssignment(e)}
                    submitHandler={(e)=>this.searchAssignment(e)}
                    ref={this.childSearchFilter}
                    csvRef={(r) => this.csvLink = r}
                />
                <ReactGrid
                    gridRowData={this.state.assignmentSearchList || []}
                    gridColData={headerData}
                    columnPrioritySort={defaultCustomerNameSortOrder}
                    loadMoreClick={this.loadMoreClick}
                    totalCount={this.state.assignmentSearchTotalCount}
                />
                <Modal title={localConstant.commonConstants.CHECK_ANY_MANDATORY_FIELD}
                    titleClassName="chargeTypeOption"
                    modalContentClass="extranetModalContent"
                    modalId="errorListPopup"
                    formId="errorListForm"
                    buttons={this.modelBtns.errorListButton}
                    isShowModal={this.state.errorList.length > 0}>
                    <ErrorList errors={this.state.errorList} />
                </Modal> 
            </div>
        );
    }
}

export default AssignmentSearch;
AssignmentSearch.propTypes = {
};

AssignmentSearch.propTypes = {
    
};