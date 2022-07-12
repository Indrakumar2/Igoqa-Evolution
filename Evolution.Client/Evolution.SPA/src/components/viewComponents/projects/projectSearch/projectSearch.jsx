import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './headerData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomerAndCountrySearch from '../../../applicationComponents/customerAndCountrySearch';
import { AppMainRoutes } from '../../../../routes/routesConfig';

const localConstant = getlocalizeData();
const defaultSearchField={
    customerName:'',
    projectNumber:'',
    customerProjectNumber:'',
    customerProjectName:'',
    projectStatus:'O',
    coordinatorName:'',
    division:'',
    office:'',
    searchDocumentType:'',
    documentSearchText:''
};
const ProjectSearchFilter = (props) => (
    <form id="contractSearch"
        onSubmit={props.searchProjects}
         autoComplete="off">
        <div className="row mb-0">
            <CustomerAndCountrySearch divClassName="s4"isSupplier={true}/>
            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.PROJECT_NUMBER}
                onValueChange={props.handlerChange}
                type='text'
                dataType='numeric'
                valueType='value'
                colSize='s4 pl-0'
                inputClass="customInputs"
                name='projectNumber'
                maxLength={20}
                value={props.searchFieldData.projectNumber} />

            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.CUSTOMER_PROJECT_NUMBER}
                onValueChange={props.handlerChange}
                type='text'
                dataValType='valueText'
                colSize='s4 pl-0'
                inputClass="customInputs"
                name='customerProjectNumber'
                value={props.searchFieldData.customerProjectNumber} />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.CUSTOMER_PROJECT_NAME}
                onValueChange={props.handlerChange}
                type='text'
                dataValType='valueText'
                value={props.searchFieldData.customerProjectName}
                colSize='s4'
                inputClass="customInputs"
                name='customerProjectName'
                maxLength={60} />
            <CustomInput hasLabel={true}
                name='projectStatus'
                divClassName='col'
                label={localConstant.contract.STATUS}
                type='select'
                colSize='s4 pl-0'
                inputClass="customInputs"
                optionsList={localConstant.commonConstants.status || []}
                optionName='name'
                optionValue="value"
                className="browser-default"
                defaultValue={props.searchFieldData.projectStatus}
                onSelectChange={props.handlerChange}
                optionSelecetLabel = 'All'
            />
            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.COORDINATOR_NAME}
                onValueChange={props.handlerChange}
                type='text'
                dataValType='valueText'
                value={props.searchFieldData.coordinatorName}
                colSize='s4 pl-0'
                inputClass="customInputs"
                name='coordinatorName'
                maxLength={50} />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.DIVISION}
                onValueChange={props.handlerChange}
                type='text'
                dataValType='valueText'
                value={props.searchFieldData.division}
                colSize='s4'
                inputClass="customInputs"
                name='division'
                maxLength={50} />

            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.project.OFFICE}
                onValueChange={props.handlerChange}
                type='text'
                dataValType='valueText'
                value={props.searchFieldData.office}
                colSize='s4 pl-0'
                inputClass="customInputs"
                name='office'
                maxLength={50} />

            <CustomInput hasLabel={true}
                name='businessUnit'
                divClassName='col'
                label={localConstant.project.BUSINESS_UNIT}
                type='select'
                colSize='s4 pl-0'
                inputClass="customInputs"
                optionsList={props.projectBusinessUnit || []}
                optionName='name'
                optionValue="id"
                className="browser-default"
                defaultValue={props.searchFieldData.businessUnit}
                onSelectChange={props.handlerChange}
            />
        </div>
        <div className="row mb-0">
            <CustomInput hasLabel={true}
                divClassName='col'
                name="searchDocumentType"
                id="searchDocumentType"
                onSelectChange={props.handlerChange}
                label={localConstant.contract.SEARCH_DOCUMENTS}
                type='select'
                colSize='s4'
                className="browser-default"
                optionName="name"
                optionValue="name"
                defaultValue={props.searchFieldData.searchDocumentType}
                optionsList={props.documentMasterData}
                 />
            <CustomInput hasLabel={true}
                divClassName='col'
                label={localConstant.contract.SEARCH_TEXT}
                type='text'
                dataValType='valueText'
                value={props.searchFieldData.documentSearchText}
                name="documentSearchText"
                id="documentSearchText"
                colSize='s4 pl-0'
                className="browser-default"
                inputClass="customInputs"
                onValueChange={props.handlerChange}               
                maxLength={200} />
            <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">Search</button>
            <button type="button" onClick={props.clearSearchData} className="mt-4x modal-close waves-effect waves-green btn" >Reset</button>
        </div>
    </form>
);

// const SearchCustomer = (props) => (
//     <div className="row mb-0">
//         <CustomInput hasLabel={true}
//             name='customerName'
//             divClassName='col' 
//             label={localConstant.companyDetails.generalDetails.NAME}
//             type='text'
//             colSize='s6'
//             inputClass="customInputs"
//             dataValType='valueText'
//             onValueChange={props.searchCustomer}
//             onValueKeyDown={props.handlerKeyDown}
//             value={props.defaultCustomerName}
//             autocomplete="off" />
//         <CustomInput hasLabel={true}
//             name='operatingCountry'
//             divClassName='col'
//             label={localConstant.modalConstant.COUNTRY}
//             type='select'
//             colSize='s6'
//             inputClass="customInputs"
//             optionsList={props.countryMasterData}
//             optionName='name'
//             optionValue="name"
//             className="browser-default"
//             onSelectChange={props.selectedCountrySearch}
//             defaultValue={props.defaultCountryName} />
//     </div>

// );

class ProjectSearch extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            searchCustomerName: '',
            isReset:false,
            searchFieldData:defaultSearchField,
            projectSearchList:[], //Search Optimization
            projectSearchTotalCount:0,            
        };
        this.projectSearchFilter = {
            projectStatus: this.props.projectStatus
        };
    }

    componentDidMount() {
        // this.props.actions.FetchBusinessUnit();
        //  this.props.actions.FetchDocumentTypeMasterData();
        const statusUpdate = { ...this.state.searchFieldData } ;
        statusUpdate.projectStatus = this.props.projectStatus;
        this.setState({ searchFieldData:statusUpdate });
        this.getPanelSubTitle();
        this.clearSearchData();
    }

    componentWillUnmount() {
        this.props.actions.ClearData(); //Changes for ITK 648
    }

    panelClick = (e) => {
        this.setState((state) => {
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }

    handlerChange = (e) => {
        // this.projectSearchFilter.customerName = this.state.searchCustomerName;
        const filterData = { ...this.state.searchFieldData };
        filterData.customerName = this.props.defaultCustomerName;
        filterData[e.target.name] = e.target.value;    
        if (this.props.contractHoldingCompany === '')           
            filterData.operatingCountry = this.props.contractHoldingCompany;
        
            this.setState({ searchFieldData:filterData });
        }

    //Clear Search Data
    clearSearchData = () => {
        this.setState({          
            searchFieldData:defaultSearchField,
            projectSearchList:[],
            projectSearchTotalCount:0
        });
        this.props.actions.ClearSearchProjectList();
        this.projectSearchFilter={};
    }

    //project search
    searchProjects = (e,isLoadMore) => {
        e.preventDefault();
        const projectSearchFilter = { ...this.state.searchFieldData };
        const customerName = this.props.defaultCustomerName;
        const customerId = this.props.defaultCustomerId;

        projectSearchFilter.customerName = customerName ? customerName.trim() : "";
        projectSearchFilter.customerId = customerId ? customerId : "";
        /*Code added for Assignment module And User rightClick open Menu New Tab the PROPS.CURRENT PAGE is Empty.*/
        if ((this.props.location.pathname === AppMainRoutes.assignmentSearchProject && this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE)|| (this.props.location.pathname === '/Assignment/SearchProject')) { 
            projectSearchFilter.contractHoldingCompanyCode = this.props.selectedCompany;
        }
        /*Code added for Supplier PO module And User rightClick open Menu New Tab the PROPS.CURRENT PAGE is Empty.*/
        else if ((this.props.location.pathname === AppMainRoutes.createSupplierPO && this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE) ||(this.props.location.pathname === '/SupplierPO/SearchProject')) {
            projectSearchFilter.contractHoldingCompanyCode = this.props.selectedCompany;
            projectSearchFilter.workFlowTypeIn = "s,v";
        }
        else {
            projectSearchFilter.contractHoldingCompanyCode = null;
        }

        //search optimization
        if(isLoadMore){
            projectSearchFilter.OffSet=this.state.projectSearchList?this.state.projectSearchList.length:0;
            projectSearchFilter.projectSearchTotalCount=this.state.projectSearchTotalCount;
        }

        this.props.actions.FetchProjectListSearch(projectSearchFilter).then(res => {
            if (res) {
                if (!isLoadMore) {
                    this.setState({
                        projectSearchTotalCount: res.recordCount,
                        projectSearchList:[]
                    });
                    this.panelClick();
                }
                const tempData = Object.assign([], this.state.projectSearchList);
                if(Array.isArray(this.props.projectList) && this.props.projectList.length > 0)
                {
                    tempData.push(...this.props.projectList);               
                }
                this.setState({
                    projectSearchList: tempData
                });
            }
        });
       
    }

    loadMoreClick=(data)=>{
        this.searchProjects(data,true);
    }

    //Get Current URL Path
    getCurrentPathUrl = () => {
        if (this.props.location.pathname === AppMainRoutes.editProject) {
            this.props.actions.GetCurrentPage('editProject');
            this.props.actions.UpdateInteractionMode(false);
        } else if (this.props.location.pathname === AppMainRoutes.viewProject) {
            this.props.actions.GetCurrentPage('viewProject');
            this.props.actions.UpdateInteractionMode(true);
        }
    }
    /*Start -- Code added for suppliepo module 
               To add header name     
    */
    getPanelSubTitle = () => {
        if (this.props.location.pathname === AppMainRoutes.createSupplierPO && (this.props.currentPage === localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE || this.props.currentPage === '')) {
            return ': '+ localConstant.supplierpo.ADD_SUPPLIER_PO;
        }
        else if (this.props.location.pathname === AppMainRoutes.assignmentSearchProject  && (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE || this.props.currentPage === '')) {
            return ': ' + localConstant.assignments.ADD_ASSIGNMENT;
        }
        else {
            return ': '+ localConstant.project.EDIT_VIEW_PROJECT;
        }
    }
    /*End -- Code added for suppliepo module*/
    render() {
        const { documentTypesData }=this.props;
        const masterDocData=documentTypesData&&documentTypesData.filter(x=>x.moduleName==="Project");
        return (
            <div className="companyPageContainer customCard">
                <Panel colSize="s12"
                    isArrow={true}
                    heading={`${
                        localConstant.project.SEARCH_PROJECT
                        }`}
                    subtitle={this.getPanelSubTitle()}
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} >
                    <ProjectSearchFilter
                        interactionMode={this.props.interactionMode}
                        defaultCustomerName={this.state.searchCustomerName}
                        defaultCountryName={this.props.countryMasterData}
                        currentPage={this.props.currentPage}
                        selectedCompany={this.props.selectedCompany}
                        projectBusinessUnit={this.props.projectBusinessUnit}
                        status={localConstant.commonConstants.status}
                        defaultProjectStatus={this.props.projectStatus}
                        handlerChange={(e) => this.handlerChange(e)}
                        selectCustomerSearch={this.selectCustomerSearch}
                        clearSearchData={this.clearSearchData}
                        searchProjects={(e) => this.searchProjects(e)}
                        handlerKeyDown={this.handlerKeyDown}
                        documentMasterData={masterDocData}
                        searchFieldData={this.state.searchFieldData}
                    />

                </Panel>
                <ReactGrid
                    gridRowData={this.state.projectSearchList || []}
                    gridColData={headerData}
                    loadMoreClick={this.loadMoreClick}
                    totalCount={this.state.projectSearchTotalCount}
                />
            </div>
        );
    }
}

export default ProjectSearch;
